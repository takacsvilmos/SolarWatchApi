namespace SolarWatch.Contracts
{
    public class CityUpdateDto
    {
        public string CityName { get; set; }
        public double? Latitude { get; set; } 
        public double? Longitude { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
