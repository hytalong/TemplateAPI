
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Template.Batch.Commands;
using Template.CrossCutting.InjecaoDependencia;
using McMaster.Extensions.CommandLineUtils;

namespace Template.Batch
{
    [Command("Template")]
    [Subcommand(
        typeof(HelloCommand),
        typeof(WeatherForecastCommand))]
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var services = new ServiceCollection()
                .AddLogger(configuration)
                .AddMediator()
                .AddRepository(configuration)
                .BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            app.Execute(args);
        }

        protected void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }

    }
}
