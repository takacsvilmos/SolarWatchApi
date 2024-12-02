using System.Net;
using static System.Net.WebRequestMethods;

namespace SolarWatch.Services
{
    public class CityCoordinatesApi
    {
        private readonly ILogger<CityCoordinatesApi> _logger;
        private readonly IWebClient _client;
        private readonly IConfiguration _configuration;

        public CityCoordinatesApi(ILogger<CityCoordinatesApi> logger, IWebClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _configuration = configuration;
        }

        public async Task<string> GetCityCoordinates(string city)
        {
            var apiKey = _configuration["ApiKeys:CityCoordinatesApiKey"];
            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid={apiKey}";
            
            _logger.LogInformation("Calling openWeather API with url: {url}", url);
            return await _client.DownloadString(url);
        }
    }
}
