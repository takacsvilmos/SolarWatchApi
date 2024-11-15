namespace SolarWatch.Models
{
    public class SunriseSunset
    {
        public Guid Id { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }

        public SunriseSunset(Guid cityId, string sunrise, string sunset)
        {
            Id = Guid.NewGuid();
            Sunrise = sunrise;
            Sunset = sunset;
            CityId = cityId;
        }
        public SunriseSunset() { }
    }
}
