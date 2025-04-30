using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Template.Domain.Entidades;
using Template.Domain.Repositorios;

namespace Template.Application.Querys.Quotes
{
    public class QuoteHandler : IRequestHandler<QuoteRequest, QuoteResponse>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ILogger Logger {get;}
        private IQuoteReadRepository QuoteReadRepository { get; }

        public QuoteHandler(
            IQuoteReadRepository quoteReadRepository, 
            ILogger logger)
        {
            QuoteReadRepository = quoteReadRepository; 
            Logger = logger;
        }

        public async Task<QuoteResponse> Handle(QuoteRequest request, CancellationToken cancellationToken)
        {
            //var rng = new Random();
            Logger.Information("Starting Handle request");
            
            var response = new QuoteResponse()
            {
                Quotes = await QuoteReadRepository.Find(request.coins)
            };

            Logger.Information("Finish Handle request");
            return await Task.FromResult(response);
        }
    }
}
