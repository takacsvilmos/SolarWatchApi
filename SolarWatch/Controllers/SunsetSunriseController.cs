using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SolarWatch.Models;
using SolarWatch.Services;
using System.Linq;
using Microsoft.AspNetCore.OutputCaching;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("SunsetSunrise")]
    public class SunsetSunriseController : Controller
    {
        private readonly ILogger<SunsetSunriseController> _logger;
        private readonly CityCoordinatesApi _cityCoordinatesApi;
        private readonly IJsonProcessor _jsonProcessor;
        private readonly SunriseSunsetApi _sunriseSunsetApi;

        public SunsetSunriseController(ILogger<SunsetSunriseController> logger, CityCoordinatesApi cityCoordinatesApi, IJsonProcessor jsonProcessor, SunriseSunsetApi sunriseSunsetApi)
        {
            _logger = logger;
            _cityCoordinatesApi = cityCoordinatesApi;
            _jsonProcessor = jsonProcessor;
            _sunriseSunsetApi = sunriseSunsetApi;
          
        }

        [HttpGet("ByCity")]
        public async Task<ActionResult<SunriseSunsetData>> Get(string city, int year, int month, int day)
        {
            try
            {

                if (year < 1900 || year > DateTime.Now.Year)
                {
                    return BadRequest($"Invalid {year} please give a valid one between 1900 and {DateTime.Now.Year}.");
                }

                if (month < 1 || month > 12)
                {
                    return BadRequest($"Invalid month please give a valid month between 1 and 12.");
                }

                if (day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    return BadRequest(
                        $"Invalid day: {day}. The day must be between 1 and {DateTime.DaysInMonth(year, month)}.");
                }

                var date = new DateOnly(year, month, day);
                var cityData = await _cityCoordinatesApi.GetCityCoordinates(city);
                if (cityData == null)
                {
                    return NotFound($"City '{city}' not found.");
                }

                var cityCoordinates = _jsonProcessor.Process(cityData);
                var sunriseAndSunsetData =  await
                    _sunriseSunsetApi.GetSunriseSunsetString(cityCoordinates.Lat, cityCoordinates.Longitude, date);
                if (string.IsNullOrEmpty(sunriseAndSunsetData))
                {
                    return StatusCode(500, "Error retrieving sunrise and sunset data.");
                }

                var sunriseSunsetTime = _jsonProcessor.MakeSunriseSunsetTime(sunriseAndSunsetData);

                return Ok(new SunriseSunsetData(city, sunriseSunsetTime.Sunrise, sunriseSunsetTime.Sunset, date));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSunriseSunsetData");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("Db")]
        public async Task<ActionResult<SunriseSunset>> GetSunriseSunsetToDb(string city, string inputDate)
        {
            try
            {
                var dateArray = inputDate.Split("-");
                var date = new DateOnly(int.Parse(dateArray[0]), int.Parse(dateArray[1]),int.Parse(dateArray[2]));
                var cityData = await _cityCoordinatesApi.GetCityCoordinates(city);
                if (cityData == null)
                {
                    return NotFound($"City '{city}' not found.");
                }

                var cityObject = _jsonProcessor.MakeNewCityObjectFromApiJSON(cityData);
                var sunriseSunsetApiResponseObject =
                    await _sunriseSunsetApi.GetSunriseSunsetString(cityObject.Latitude, cityObject.Longitude, date);
                if (string.IsNullOrEmpty(sunriseSunsetApiResponseObject))
                {
                    return StatusCode(500, "Error retrieving sunset and sunrise data");
                }

                var sunriseSunsetTime = _jsonProcessor.MakeSunriseSunsetTime(sunriseSunsetApiResponseObject);
                var SunriseSunsetObject =
                    new SunriseSunset(cityObject.Id,date, sunriseSunsetTime.Sunrise, sunriseSunsetTime.Sunset);
                cityObject.SunriseSunsets.Add(SunriseSunsetObject);
                
                return Ok(cityObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting the sunrise/sunset");
                return StatusCode(500, "Unexpected error occured.");
            }
        }
        
    }
}
