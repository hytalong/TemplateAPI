using DataAccess.Domain;
using DataAccess.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Repositorios;
using Template.Infrastructure.Client;
using Template.Infrastructure.Oracle;
using Template.Infrastructure.Redis;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class RepositoryServiceCollectionExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfig = configuration.GetSection(nameof(DBConfig)).Get<DBConfig>();
            var db = new DataAccessFactory(dbConfig);
            services.AddSingleton<IDataAccess>(db.Create());

            var redisConfig = configuration.GetSection("RedisConfig").Get<RedisConfig>();
            services.AddSingleton(redisConfig);
            services.AddSingleton<IRedisHandler, RedisHandler>(provider => new RedisHandler(redisConfig));

            services.AddTransient<IWeatherReadRepository, WeatherReadRepository>();
            services.AddTransient<IQuoteReadRepository, QuoteReadClient>();
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration.GetSection("RedisConfig").Get<RedisConfig>();
            services.AddSingleton(redisConfig);
            services.AddSingleton<IRedisHandler, RedisHandler>(provider => new RedisHandler(redisConfig));
            return services;
        }
    }
}