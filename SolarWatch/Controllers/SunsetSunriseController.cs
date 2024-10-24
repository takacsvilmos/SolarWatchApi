using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

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
    }
}
