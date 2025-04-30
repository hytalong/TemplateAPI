using System.IO;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Settings.Configuration;
using Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class LoggingServiceCollectionExtension
    {
        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();
            services.AddSingleton<Serilog.ILogger>(log);
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(log, dispose: true));
            return services;
        }
    }
    public class HttpIntercepter : DelegatingHandler
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public HttpIntercepter(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            StringValues value = "";
            HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-correlation-id", out value);
            request.Headers.Add("x-correlation-id", value.ToString());
            if (!request.Headers.Contains("x-idempotency-id"))
            {
                StringValues value2 = "";
                HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-idempotency-id", out value2);
                request.Headers.Add("x-idempotency-id", value2.ToString());
            }

            StringValues value3 = "";
            HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-session-id", out value3);
            request.Headers.Add("x-session-id", value3.ToString());
            if (!request.Headers.Contains("Authorization"))
            {
                StringValues value4 = "";
                if (HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out value4))
                {
                    request.Headers.Add("Authorization", value4.ToString());
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
    public static class LogHttpMiddlewareExtension
    {
        public static IServiceCollection AddLogHttp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LogManagerConfig>(configuration.GetSection("LogManagerConfig"));
            return services;
        }

    }
    public class LogManagerConfig
    {
        public int LogLength { get; set; } = 40;
    }
}