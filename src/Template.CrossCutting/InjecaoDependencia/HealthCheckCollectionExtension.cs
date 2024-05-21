using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Template.CrossCutting.Health;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class HealthCheckCollectionExtension
    {
        public static IServiceCollection AddHealthChecksInjection(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<ExampleHealthCheck>("Example_Healthy");
            return services;
        }
    }
}