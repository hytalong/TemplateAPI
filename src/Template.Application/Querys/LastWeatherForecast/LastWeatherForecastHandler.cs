using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;
using System.Security.Cryptography;

namespace Template.Application.Querys.LastWeatherForecast
{
    public class LastWeatherForecastHandler : IRequestHandler<LastWeatherForecastRequest, LastWeatherForecastResponse>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ILogger Logger {get;}
        private IWeatherReadRepository WeatherReadRepository { get; }

        public LastWeatherForecastHandler(
            IWeatherReadRepository weatherReadRepository, 
            ILogger logger)
        {
            WeatherReadRepository = weatherReadRepository; 
            Logger = logger;
        }

        public async Task<LastWeatherForecastResponse> Handle(LastWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            //var rng = new Random();
            Logger.Information("Starting Handle request");
            
            var response = new LastWeatherForecastResponse()
            {
                WheatherForecasts = WeatherReadRepository.Find()
            };

            Logger.Information("Finish Handle request");
            return await Task.FromResult(response);
        }
    }
}
