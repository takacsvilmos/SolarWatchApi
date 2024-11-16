using SolarWatch.Data;

namespace SolarWatch.Services
{
    public class CityService
    {
        private readonly SolarwatchDbContext _dbContext;

        public CityService(SolarwatchDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
