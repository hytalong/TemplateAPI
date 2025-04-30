using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;
using System;

namespace Template.Infrastructure.Client
{
    public class QuoteReadClient : IQuoteReadRepository
    {
        private readonly HttpClient _httpClient;
        protected readonly ILogger Logger;

        public QuoteReadClient(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClient = httpClientFactory.CreateClient("ResilientClient");
            Logger = logger;
        }

        public async Task<Dictionary<string, Quote>> Find(string coins)
        {
            Logger.Information("Starting client find");

            var url = $"https://economia.awesomeapi.com.br/last/{coins}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<Dictionary<string, Quote>>(responseStream);
                }
                else
                {
                    Logger.Information("Response unsuccessful with status code: {StatusCode}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Client find encountered an error: {Message}", ex.Message);
                throw;
            }
        }
    }
}