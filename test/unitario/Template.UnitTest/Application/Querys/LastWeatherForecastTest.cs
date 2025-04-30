using System;
using Xunit;
using Moq;
using Template.Domain.Repositorios;
using Template.Domain.Entidades;
using System.Collections.Generic;
using Template.Application.Querys.LastWeatherForecast;

using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Template.UnitTest.Application.Querys
{
    public class LastWeatherForecastTest
    {
        [Fact]
        public async Task Test1Async()
        {   //Setup
            var mockWeatherResponse = new WeatherForecast()
            {
              Date_Time = DateTime.Now,
              Summary = "Teste",
              TemperatureC = 30
            };
            var mockWeatherResponseList = new List<WeatherForecast>();
            mockWeatherResponseList.Add(mockWeatherResponse);
            var mockWeatherReadRepository = new Moq.Mock<IWeatherReadRepository>();
            mockWeatherReadRepository.Setup(x => x.Find()).Returns(mockWeatherResponseList);

            var mockLogger = new  Moq.Mock<ILogger>();
            mockLogger.Setup(x => x.Information(It.IsAny<String>()));
            
            var expectedResponse = new LastWeatherForecastResponse()
            {
                WheatherForecasts = mockWeatherResponseList
            };

            var lastWeatherForecastHandler = new LastWeatherForecastHandler(mockWeatherReadRepository.Object, mockLogger.Object);
            var lastWeatherForecastRequest = new LastWeatherForecastRequest();

        
            var response = await lastWeatherForecastHandler.Handle(lastWeatherForecastRequest, default(CancellationToken));

            Assert.Equal(expectedResponse.WheatherForecasts, response.WheatherForecasts);
        }
    }
}
