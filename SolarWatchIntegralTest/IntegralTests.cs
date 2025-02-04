    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SolarWatch.Data;
    using SolarWatch.Models;

    namespace SolarWatchIntegralTest
    {
        public class IntegralTests
        {
           

                [Collection("IntegrationTests")]
                public class MyControllerIntegrationTest
                {
                    private readonly SolarWatchWebApplicationFactory _app;
                    private readonly HttpClient _client;
        
                    public MyControllerIntegrationTest()
                    {
                        _app = new SolarWatchWebApplicationFactory();
                        _client = _app.CreateClient();

                    }

                    [Fact]
                    public async Task TestEndPoint()
                    {
                        var response = await _client.GetAsync("/SunsetSunrise/AllCities");

                        Console.WriteLine(response.Content);
                        Console.WriteLine(response.RequestMessage);
                        Assert.NotNull(response);
                        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
                    }

                }
            }
    }