using System.Net;
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
        public void GetSunriseSunsetString_ShouldReturnExpectedResult()
        {
            var expectedResponse = "{\"sunrise\":\"6:30:00 AM\", \"sunset\":\"7:30:00 PM\"}";
            var latitude = 47.4979;
            var longitude = 19.0402;
            var date = new DateOnly(2023, 10, 1);

            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .Returns(expectedResponse);

            var mockLogger = new Mock<ILogger<SunriseSunsetApi>>();
            var service = new SunriseSunsetApi(mockLogger.Object, mockWebClient.Object);

            var result = service.GetSunriseSunsetString(latitude, longitude, date);
            Assert.AreEqual(expectedResponse, result);
        }

        [Test]
        public void GetCityCoordinates_ShouldReturnExpectedResult()
        {
            var expectedResponse = "{latitude:47.4979, longitude: 19.0402 }";
            var city = "Budapest";

            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .Returns(expectedResponse);
            var mockLogger = new Mock<ILogger<CityCoordinatesApi>>();
            var service = new CityCoordinatesApi(mockLogger.Object, mockWebClient.Object);

            var result = service.GetCityCoordinates(city);
            Assert.AreEqual(expectedResponse, result);
        }

        [Test]
        public void SunsetSunriseController_SuccessCase()
        {
            var city = "Washington";
            var coordinates= "{latitude:47.4979, longitude: 19.0402 }";
            var mockWebClient = new Mock<IWebClient>();
            mockWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .Returns(coordinates);
            var mockLogger = new Mock<ILogger<CityCoordinatesApi>>();
            var cityCoordinatesService = new CityCoordinatesApi(mockLogger.Object, mockWebClient.Object);


            var expectedResponse = "{\"sunrise\":\"6:30:00 AM\", \"sunset\":\"7:30:00 PM\"}";
            var latitude =  47.4979;
            var longitude = 19.0402;
            var date = new DateOnly(2023, 10, 1);
            var mockSunsetWebClient = new Mock<IWebClient>();
            mockSunsetWebClient.Setup(client => client.DownloadString(It.IsAny<string>()))
                .Returns(expectedResponse);
            var mockSunriseSunsetLogger = new Mock<ILogger<SunriseSunsetApi>>();
            var sunsetSunriseService = new SunriseSunsetApi(mockSunriseSunsetLogger.Object, mockSunsetWebClient.Object);

            var mockJsonProcessor = new Mock<IJsonProcessor>();
            mockJsonProcessor.Setup(processor => processor.Process(It.IsAny<string>()))
                .Returns(new CityCoordinates(latitude, longitude));
            mockJsonProcessor.Setup(processor => processor.MakeSunriseSunsetTime(It.IsAny<string>()))
                .Returns(new SunriseSunsetTime("6:30:00 AM", "7:30:00 PM"));

            var controller = new SunsetSunriseController(new Mock<ILogger<SunsetSunriseController>>().Object,
                cityCoordinatesService, mockJsonProcessor.Object, sunsetSunriseService);

            var result = controller.Get(city, date.Year, date.Month, date.Day);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var responseData = okResult.Value as SunriseSunsetData;

            Assert.AreEqual("6:30:00 AM", responseData.Sunrise);
            Assert.AreEqual("7:30:00 PM", responseData.Sunset);
        }
    }
}