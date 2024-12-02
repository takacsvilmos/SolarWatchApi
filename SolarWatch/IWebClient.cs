namespace SolarWatch
{
    public interface IWebClient
    {
        Task<string> DownloadString(string url);
    }
}
