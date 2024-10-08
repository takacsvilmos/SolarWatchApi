using System.Net;

namespace SolarWatch.Services
{
    public class CustomWebClient : IWebClient
    {
        private readonly WebClient _client;
        public CustomWebClient() 
            { 
            _client = new WebClient();
            }

        public string DownloadString(string url) 
            { 
            return _client.DownloadString(url);
            }

    }
}
