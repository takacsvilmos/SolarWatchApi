using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services;

namespace SolarWatchTests
{
    public class SunriseSunsetApiTests
    {

        [Test]
        public async Task GetSunriseSunsetString_ShouldReturnExpectedResult()
        {
            var expectedResponse = "{\"sunrise\":\"6:30:00 AM\", \"sunset\":\"7:30:00 PM\"}";
            var latitude = 47.4979;
            var longitude = 19.0402;
            var date = new DateOnly(2023, 10, 1);

            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            var mockLogger = new Mock<ILogger<SunriseSunsetApi>>();
            var service = new SunriseSunsetApi(mockLogger.Object, mockWebClient.Object);

            var result = await service.GetSunriseSunsetString(latitude, longitude, date);
            Assert.AreEqual(expectedResponse, result);
        }

        [Test]
        public async Task GetCityCoordinates_ShouldReturnExpectedResult()
        {
            var expectedResponse = "{latitude:47.4979, longitude: 19.0402 }";
            var city = "Budapest";

            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);
            var mockLogger = new Mock<ILogger<CityCoordinatesApi>>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(config => config["SomeKey"]).Returns("SomeValue");
            var service = new CityCoordinatesApi(mockLogger.Object, mockWebClient.Object, mockConfig.Object);

            var result = await service.GetCityCoordinates(city);
            Assert.AreEqual(expectedResponse, result);
        }

    }
}