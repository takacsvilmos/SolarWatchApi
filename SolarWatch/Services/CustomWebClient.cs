using System.Net;

namespace SolarWatch.Services
{
    public class CustomWebClient : IWebClient
    {
        private readonly HttpClient _client;
        public CustomWebClient() 
            { 
            _client = new HttpClient();
            }

        public async Task<string> DownloadString(string url) 
            { 
            return await _client.GetStringAsync(url);
            }

    }
}
