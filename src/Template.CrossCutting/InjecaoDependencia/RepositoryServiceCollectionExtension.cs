using DataAccess.Domain;
using DataAccess.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Repositorios;
using Template.Infrastructure.Client;
using Template.Infrastructure.Oracle;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class RepositoryServiceCollectionExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
	    {
            var db = new DataAccessFactory(configuration.GetSection(nameof(DBConfig)).Get<DBConfig>());
            services.AddSingleton<IDataAccess>(db.Create());
		    services.AddTransient<IWeatherReadRepository, WeatherReadRepository>();
            services.AddTransient<IQuoteReadRepository, QuoteReadClient>();
            return services;
	    }
    }
}