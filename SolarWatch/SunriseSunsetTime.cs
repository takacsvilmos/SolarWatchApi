namespace SolarWatch
{
    public class SunriseSunsetTime
    {
        public string Sunrise { get; }
        public string Sunset { get;}

        public SunriseSunsetTime(string sunrise, string sunset)
        {
            Sunrise = sunrise;
            Sunset = sunset;
        }
    }
}
