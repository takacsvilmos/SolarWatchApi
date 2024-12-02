using System.Runtime.InteropServices.JavaScript;

namespace SolarWatch
{
    public class SunriseSunsetData
    {
        public string City { get; }
        public string Sunrise { get; }
        public string Sunset { get; }
        public DateOnly Date { get; }
        public SunriseSunsetData(string city, string sunrise, string sunset, DateOnly date)
        {
            City = city;
            Sunrise = sunrise;
            Sunset = sunset;
            Date = date;
        }
    }
}
