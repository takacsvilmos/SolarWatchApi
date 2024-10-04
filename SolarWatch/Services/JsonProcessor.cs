using System.Text.Json;

namespace SolarWatch.Services
{
    public class JsonProcessor : IJsonProcessor
    {
        public CityCoordinates Process(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement rootArray = json.RootElement;

            if (rootArray.GetArrayLength() > 0)
            {
                JsonElement firstElement = rootArray[0];
                double latitude = firstElement.GetProperty("lat").GetDouble();
                double longitude = firstElement.GetProperty("lon").GetDouble();

                return new CityCoordinates(latitude, longitude);
            }

            throw new InvalidOperationException("No city available");
        }

        public SunriseSunsetTime MakeSunriseSunsetTime(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement rootElement = json.RootElement;
            JsonElement resultsElement = rootElement.GetProperty("results");
            string sunrise = resultsElement.GetProperty("sunrise").GetString();
            string sunset = resultsElement.GetProperty("sunset").GetString();

            return new SunriseSunsetTime(sunrise, sunset);
        }
    }
}
