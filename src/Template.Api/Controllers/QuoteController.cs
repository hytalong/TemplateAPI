using Serilog;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Template.Application.Querys.Quotes;
using Swashbuckle.AspNetCore.Annotations;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class QuoteController : ControllerBase
    {
        private ILogger Logger { get; }
        private IMediator Mediator { get; }
        public QuoteController(ILogger logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }

        [HttpGet]
        [Route("{coins}")]
        [SwaggerOperation(Summary = "Example", Description = "Example quote")]
        [SwaggerResponse(200, Type = typeof(QuoteResponse))]
        [SwaggerResponse(404)]
        public async Task<ActionResult> Get(string coins, [FromHeader(Name = "x-correlation-id")] string correlationId, [FromHeader(Name = "x-idempotency-id")] string idempotencyId)
        {
            var request = new QuoteRequest() { 
                coins = coins
            };
            var response = await Mediator.Send(request);
            Logger.Information("Response: {@Response}", response);
            if (response.Quotes == null)
            {
                return NotFound();
            }
            return Ok(response.Quotes);
        }
    }
}
