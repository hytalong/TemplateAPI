using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using DataAccess.Domain;
using Serilog;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;
using Template.Infrastructure.Redis;

namespace Template.Infrastructure.Oracle
{
    public class WeatherReadRepository : IWeatherReadRepository
    {
        const string SQLSelect = @"SELECT * FROM WeatherForecast 
                                        WHERE id = @Id";
        private static readonly string[] Summaries = new[]
        {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        private ILogger Logger { get; }
        private IDataAccess Db { get; }
        private IRedisHandler Cache { get; }

        public WeatherReadRepository(ILogger logger, IDataAccess db, IRedisHandler cache)
        {
            Logger = logger;
            Db = db;
            Cache = cache;
        }

        public List<WeatherForecast> Find()
        {
            Logger.Information("Getting last five weather forecasts");

            var cacheKey = "WeatherForecasts";
            var cachedData = Cache.GetValue(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Logger.Information("Returning cached data");
                return JsonSerializer.Deserialize<List<WeatherForecast>>(cachedData);
            }

            var data = Db.Query<WeatherForecast>("SELECT * FROM WeatherForecast").ToList();
            Cache.SetValue(cacheKey, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(10));

            return data;
        }

        public List<WeatherForecast> Get(int id)
        {
            Logger.Information("Starting get by id");

            var cacheKey = $"WeatherForecast_{id}";
            var cachedData = Cache.GetValue(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Logger.Information("Returning cached data");
                return JsonSerializer.Deserialize<List<WeatherForecast>>(cachedData);
            }

            var data = Db.Query<WeatherForecast>(SQLSelect, new { Id = id }).ToList();
            Cache.SetValue(cacheKey, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(10));

            return data;
        }

        public void ExemploTransaction()
        {
            Logger.Information("Starting exemplo transaction");

            using (var session = this.Db.CreateSession())
            {
                session.Begin();
                try
                {
                    var exemplo1 = session.Query<Exemplo1>(@"INSERT INTO exemplo1 (nome, descricao) 
                                                    VALUES (@Nome, @Descricao); 
                                                    SELECT LAST_INSERT_ID() as Id;",
                                    new { Nome = "Teste 1", Descricao = "Exemplo 123" }).First();

                    session.Execute("INSERT INTO exemplo1 (Nome, Descricao, exemplo1_id) VALUES (@Nome, @Descricao, @Exemplo1_id)",
                                    new { Nome = "Teste 1", Descricao = "Exemplo 123", Exemplo1_id = exemplo1.Id });
                    session.Commit();
                }
                catch (Exception e)
                {
                    session.Rollback();
                    Logger.Information("Finish exemplo transaction - Error: @e.Message", e.Message);
                    throw;
                }
            }
            Logger.Information("Starting exemplo transaction");
        }
    }
}