using System.IO;
using LogManager;
using LogManager.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class LoggingServiceCollectionExtension
    {
        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
	    {
            var log = LogFactory.CreateLogger(configuration);
            services.AddSingleton<ILogger>(log);
            return services;
	    }
    }
}