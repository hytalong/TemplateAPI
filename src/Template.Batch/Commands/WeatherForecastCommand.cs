using MediatR;
using System.Threading.Tasks;
using Template.Application.Querys.LastWeatherForecast;
using McMaster.Extensions.CommandLineUtils;
using System;
using Serilog;

namespace Template.Batch.Commands
{
    [Command(Name = "weather", Description = "WeatherForecast comando")]
    public class WeatherForecastCommand : ICommand
    {
        private ILogger Logger { get; }
        private IMediator Mediator { get; }
        public WeatherForecastCommand(ILogger logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }

        public async void OnExecute()
        {
            var request = new LastWeatherForecastRequest() { };
            var response = await Mediator.Send(request);
            Logger.Information<LastWeatherForecastResponse>("Response: {@Response}", response);
            // NOSONAR
            Console.WriteLine("Response: {@Response}", response);


        }
    }


}