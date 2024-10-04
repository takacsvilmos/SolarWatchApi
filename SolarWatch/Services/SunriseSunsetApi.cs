using System.Net;

namespace SolarWatch.Services
{
    public class SunriseSunsetApi
    {
        private readonly ILogger<SunriseSunsetApi> _logger;

        public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger)
        {
            _logger = logger;
        }

        public string GetSunriseSunsetString(double latitude, double longitude, DateOnly date)
        {
            var url = $"https://api.sunrise-sunset.org/json?lat={latitude}&lng={longitude}&date={date.ToString("yyyy-MM-dd")}";
            using var client = new WebClient();
            _logger.LogInformation("Calling Sunset and sunrise times API with url: {url}", url);
            return client.DownloadString(url);
        }
    }
}
