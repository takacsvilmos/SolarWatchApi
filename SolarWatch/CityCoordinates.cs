namespace SolarWatch
{
    public class CityCoordinates
    {
        public double Lat { get; }
        public double Longitude { get; }

        public CityCoordinates(double lat, double longitude)
        {
            Lat = lat;
            Longitude = longitude;
        }

    }
}
