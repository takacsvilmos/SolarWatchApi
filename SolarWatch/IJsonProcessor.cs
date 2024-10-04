namespace SolarWatch
{
    public interface IJsonProcessor
    {
        CityCoordinates Process(string data);
    }
}
