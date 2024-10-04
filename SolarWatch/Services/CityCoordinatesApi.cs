using System.Net;
using static System.Net.WebRequestMethods;

namespace SolarWatch.Services
{
    public class CityCoordinatesApi
    {
        private readonly ILogger<CityCoordinatesApi> _logger;

        public CityCoordinatesApi(ILogger<CityCoordinatesApi> logger)
        {
            _logger = logger;
        }

        public string GetCityCoordinates(string city)
        {
            var apiKey = "498c6649b077e74b3bee6e05708d9bfb";
            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid={apiKey}";
            using var client = new WebClient();
            
            _logger.LogInformation("Calling openWeather API with url: {url}", url);
            return client.DownloadString(url);
        }
    }
}
