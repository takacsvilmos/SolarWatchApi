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

        public SunsetSunriseController(ILogger<SunsetSunriseController> logger, CityCoordinatesApi cityCoordinatesApi, IJsonProcessor jsonProcessor)
        {
            _logger = logger;
            _cityCoordinatesApi = cityCoordinatesApi;
            _jsonProcessor = jsonProcessor;
        }

        [HttpGet("City")]
        public CityCoordinates Get(string city)
        {
            var cityData = _cityCoordinatesApi.GetCityCoordinates(city);
            var cityCoordinates = _jsonProcessor.Process(cityData);
            return cityCoordinates;
        }
    }
}
