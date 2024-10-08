using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
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
    }
}