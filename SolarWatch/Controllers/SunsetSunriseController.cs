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
        public SunriseSunsetData Get(string city, int year, int month, int day)
        {
            var date = new DateOnly(year, month, day);
            var cityData = _cityCoordinatesApi.GetCityCoordinates(city);
            var cityCoordinates = _jsonProcessor.Process(cityData);
            var sunriseAndSunsetData = _sunriseSunsetApi.GetSunriseSunsetString(cityCoordinates.Lat, cityCoordinates.Longitude, date);
            var sunriseSunsetTime = _jsonProcessor.MakeSunriseSunsetTime(sunriseAndSunsetData);

            return new SunriseSunsetData(city, sunriseSunsetTime.Sunrise, sunriseSunsetTime.Sunset, date);
        }
    }
}
