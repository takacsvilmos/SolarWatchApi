using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SolarWatch.Models;
using SolarWatch.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Contracts;
using SolarWatch.Data;

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
        private readonly SolarwatchDbContext _solarwatchDbContext;

        public SunsetSunriseController(ILogger<SunsetSunriseController> logger, CityCoordinatesApi cityCoordinatesApi, IJsonProcessor jsonProcessor, SunriseSunsetApi sunriseSunsetApi, SolarwatchDbContext solarwatchDbContext)
        {
            _logger = logger;
            _cityCoordinatesApi = cityCoordinatesApi;
            _jsonProcessor = jsonProcessor;
            _sunriseSunsetApi = sunriseSunsetApi;
            _solarwatchDbContext = solarwatchDbContext;
          
        }

        [HttpGet("ByCity"), Authorize(Roles = "Admin")]
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

        [HttpGet("NewSearch"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<SunriseSunset>> GetSunriseSunsetToDb(string city, string inputDate)
        {
            try
            {
                var dateArray = inputDate.Split("-");
                var date = new DateOnly(int.Parse(dateArray[0]), int.Parse(dateArray[1]),int.Parse(dateArray[2]));

                var searchedCity = _solarwatchDbContext.Cities
                    .Include(city => city.SunriseSunsets)
                    .FirstOrDefault(c => c.CityName == city && c.SunriseSunsets.Any(s => s.Date == date));
                if (searchedCity != null)
                {
                    return Ok(searchedCity);
                }
                
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

                _solarwatchDbContext.Cities.Add(cityObject);
                await _solarwatchDbContext.SaveChangesAsync();

                return Ok(cityObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting the sunrise/sunset");
                return StatusCode(500, "Unexpected error occured.");
            }
        }

        [HttpPatch("Update/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] CityUpdateDto updateDto)
        {
            var updateCity = await _solarwatchDbContext.Cities.FindAsync(id);
            if (updateCity == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateDto.CityName))
            {
                updateCity.CityName = updateDto.CityName;
            }
            if (updateDto.Latitude.HasValue)
            {
                updateCity.Latitude = updateDto.Latitude.Value;
            }
            if (updateDto.Longitude.HasValue)
            {
                updateCity.Longitude = updateDto.Longitude.Value;
            }
            if (!string.IsNullOrEmpty(updateDto.State))
            {
                updateCity.State = updateDto.State;
            }
            if (!string.IsNullOrEmpty(updateDto.Country))
            {
                updateCity.Country = updateDto.Country;
            }

            await _solarwatchDbContext.SaveChangesAsync();
            return Ok(updateCity);
        }

        [HttpGet("AllCities"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<List<City>>> GetCities()
        {
            var cities = await _solarwatchDbContext.Cities.ToListAsync();
            return Ok(cities);
        }
        
    }
}
