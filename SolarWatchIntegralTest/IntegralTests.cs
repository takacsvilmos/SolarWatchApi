    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using SolarWatch.Models;

    namespace SolarWatchIntegralTest
    {
        public class IntegralTests
        {
           

                [Collection("IntegrationTests")]
                public class MyControllerIntegrationTest
                {
                    private readonly SolarWatchWebApplicationFactory _factory;
                    private readonly HttpClient _client;
        
                    public MyControllerIntegrationTest()
                    {
                        _factory = new SolarWatchWebApplicationFactory();
                        _client = _factory.CreateClient();

                    }

                    [Fact]
                    public async Task TestEndPoint()
                    {
                        var response = await _client.GetAsync("/SunsetSunrise/AllCities");

                        response.EnsureSuccessStatusCode();

                        var data = await response.Content.ReadFromJsonAsync<List<City>>();
                        Assert.NotNull(data);
                        Assert.Equal(data[0].CityName, "Pécs");
                    }

                }
            }
    }