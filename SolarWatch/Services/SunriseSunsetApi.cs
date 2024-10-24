using System.Net;

namespace SolarWatch.Services
{
    public class SunriseSunsetApi
    {
        private readonly ILogger<SunriseSunsetApi> _logger;
        private readonly IWebClient _webClient;

        public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger, IWebClient webClient)
        {
            _logger = logger;
            _webClient = webClient;
        }

        public async Task<string> GetSunriseSunsetString(double latitude, double longitude, DateOnly date)
        {
            
            var url = $"https://api.sunrise-sunset.org/json?lat={latitude}&lng={longitude}&date={date.ToString("yyyy-MM-dd")}";
            _logger.LogInformation("Calling Sunset and sunrise times API with url: {url}", url);
            return await _webClient.DownloadString(url);
        }
    }
}
