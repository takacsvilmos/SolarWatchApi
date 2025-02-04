using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;
using SolarWatch.Models;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    //Create a new db name for each SolarWatchWebApplicationFactory. This is to prevent tests failing from changes done in db by a previous test. 
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
       
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Clear existing configuration sources (including secrets.json)
            config.Sources.Clear();

            // Add in-memory configuration for testing
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;MultipleActiveResultSets=true"
            });
        });

        builder.ConfigureServices(services =>
        {

            //Get the previous DbContextOptions registrations 
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarwatchDbContext>));
            var usersDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));
            
            //Remove the previous DbContextOptions registrations
            services.Remove(solarWatchDbContextDescriptor);
            services.Remove(usersDbContextDescriptor);
            
            //Add new DbContextOptions for our two contexts, this time with inmemory db 
            services.AddDbContext<SolarwatchDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            //We will need to initialize our in memory databases. 
            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();
            
            //We use this scope to request the registered dbcontexts, and initialize the schemas
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarwatchDbContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            solarContext.Cities.Add(new City
            {
                CityName = "Pécs",
                Latitude = 46.08333,
                Longitude = 18.23333,
                State = "Baranya",
                Country = "HU"
            });
            solarContext.SaveChangesAsync().Wait();

            var userContext = scope.ServiceProvider.GetRequiredService<UsersContext>();

            userContext.SaveChangesAsync().Wait();

            userContext.Database.EnsureDeleted();
            userContext.Database.EnsureCreated();

        });
    }
}
