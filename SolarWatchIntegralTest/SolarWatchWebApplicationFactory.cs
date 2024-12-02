using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatchIntegralTest
{
    public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                // Use InMemory Database for Testing
                services.AddDbContext<SolarwatchDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Seed the database
                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SolarwatchDbContext>();
                SeedTestData(context);
            });
        }
        private void SeedTestData(SolarwatchDbContext context)
        {
            if (!context.Cities.Any())  // Check if data is already seeded
            {
                context.Cities.Add(new City { CityName = "Pécs", Latitude = 46.0727, Longitude = 18.2328 });
                context.SaveChanges();
            }
        }
    }
}
