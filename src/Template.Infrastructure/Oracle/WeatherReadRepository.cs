using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DataAccess.Domain;
using LogManager.Domain;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;

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


        public WeatherReadRepository(ILogger logger, IDataAccess db, IHttpClientFactory httpClientFactory)
        {
            Logger = logger;
            Db = db;
        }

        public List<WeatherForecast> Find()
        {
            var rng = new Random();

            Logger.Information("Getting last five wheather forecast");
            this.ExemploTransaction();
            return Db.Query<WeatherForecast>("SELECT * FROM WeatherForecast").ToList();

        }

        public List<WeatherForecast> Get(int id)
        {
            Logger.Information("Starting get by id");
            return Db.Query<WeatherForecast>(SQLSelect, new { Id = id }).ToList();
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