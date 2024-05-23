using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;
using System.Text.Json.Serialization;
using System.Text.Json;
using LogManager.Domain;
using System;

namespace Template.Infrastructure.Client
{
    public class QuoteReadClient : IQuoteReadRepository
    {
        private IHttpClientFactory HttpClientFactory { get; }
        protected readonly ILogger Logger;

        public QuoteReadClient(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            HttpClientFactory = httpClientFactory;
            Logger = logger;
        }

        public async Task<Dictionary<string, Quote>> Find(string coins)
        {
            Logger.Information("Starting client find");

            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://economia.awesomeapi.com.br/last/" + coins);


            var client = HttpClientFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<Dictionary<string, Quote>>(responseStream);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Information("Finish client find with error: @ex.Message", ex.Message);
                throw;
            }
        }
    }
}