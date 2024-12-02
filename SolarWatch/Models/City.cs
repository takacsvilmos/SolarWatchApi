namespace SolarWatch.Models
{
    public class City
    {
        public Guid Id { get; init; }
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public ICollection<SunriseSunset> SunriseSunsets { get; init; } = new List<SunriseSunset>();
        public City(string cityName, double latitude, double longitude, string state, string country)
        {
            Id = Guid.NewGuid();
            CityName = cityName;
            Latitude = latitude;
            Longitude = longitude;
            State = state;
            Country = country;
        }
        public City() { }
    }
}
