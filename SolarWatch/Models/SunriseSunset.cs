namespace SolarWatch.Models
{
    public class SunriseSunset
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public Guid CityId { get; set; }
        

        public SunriseSunset(Guid cityId,DateOnly date, string sunrise, string sunset)
        {
            Id = Guid.NewGuid();
            Date = date;
            Sunrise = sunrise;
            Sunset = sunset;
            CityId = cityId;
        }
        public SunriseSunset() { }
    }
}
