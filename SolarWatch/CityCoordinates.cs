using SolarWatch;

namespace SolarWatch
{
    public struct CityCoordinates
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