using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data
{
    public class SolarwatchDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        private readonly IConfiguration _configuration;

        public SolarwatchDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
