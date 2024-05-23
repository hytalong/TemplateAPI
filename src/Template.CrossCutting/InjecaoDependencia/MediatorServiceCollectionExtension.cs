using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Template.CrossCutting.InjecaoDependencia
{
    public static class MediatorServiceCollectionExtension
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
	    {
            var assembly = AppDomain.CurrentDomain.Load("Template.Application");
		    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
	    }
    }
}