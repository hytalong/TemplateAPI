using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Template.Application.Querys.LastWeatherForecast;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private ILogger Logger { get; }
        private IMediator Mediator { get; }
        public WeatherForecastController(ILogger logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Query", Description = "Query Last weather forequest")]
        [SwaggerResponse(200, Type = typeof(LastWeatherForecastResponse))]
        [SwaggerResponse(400)]
        [SwaggerResponse(500, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult> Get([FromHeader(Name = "x-correlation-id")] string correlationId, [FromHeader(Name = "x-idempotency-id")] string idempotencyId)
        {
            var request = new LastWeatherForecastRequest() { };
            var response = await Mediator.Send(request);
            Logger.Information<LastWeatherForecastResponse>("Response: {@Response}", response);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Test", Description = "Test Last weather forequest")]
        [SwaggerResponse(204, Type = typeof(LastWeatherForecastResponse))]
        [SwaggerResponse(404, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult> Test([FromHeader(Name = "x-correlation-id")] string correlationId, [FromHeader(Name = "x-idempotency-id")] string idempotencyId)
        {
            var request = new LastWeatherForecastRequest() { };
            var response = await Mediator.Send(request);
            Logger.Information<LastWeatherForecastResponse>("Response: {@Response}", response);
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound();
        }
    }
}
