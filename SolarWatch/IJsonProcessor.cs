using SolarWatch.Models;

namespace SolarWatch
{
    public interface IJsonProcessor
    {
        CityCoordinates Process(string data);
        City MakeNewCityObjectFromApiJSON(string data);
        SunriseSunsetTime MakeSunriseSunsetTime(string data);
    }
}
