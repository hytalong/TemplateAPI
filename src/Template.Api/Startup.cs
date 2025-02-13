using JWTManager.JWTMiddleware;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Template.CrossCutting.InjecaoDependencia;
using Http.ResilientClient;
using Http.ResilientClient.Extensions;
using Http.ResilientClient.Options;

namespace Template.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();
            services.AddControllers();
            services.AddLogger(Configuration);
            services.AddLogHttp(Configuration);
            services.AddMediator();
            services.AddRepository(Configuration);
            services.AddJWTManager(Configuration).AddAuthenticationService();
            services.AddHealthChecksInjection();
            services.AddScoped<HttpIntercepter>();
            services.AddHttpContextAccessor();
            services.ConfigureAll<HttpClientFactoryOptions>(options =>
            {
                options.HttpMessageHandlerBuilderActions.Add(builder =>
                {
                    builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<HttpIntercepter>());
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Template",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri(Configuration["Swagger:TermsOfServiceUrl"]),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri(Configuration["Swagger:ContactUrl"])
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri(Configuration["Swagger:LicenseUrl"])
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Homolog"))
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error");
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseJWTManager();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = (context, health) =>
                    {
                        context.Response.ContentType = "application/json";
                        var options = new JsonWriterOptions { Indented = true };
                        using var memoryStream = new MemoryStream();
                        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
                        {
                            jsonWriter.WriteStartObject();
                            foreach (var healthEntry in health.Entries)
                            {
                                jsonWriter.WriteString("status", healthEntry.Value.Description);
                            }
                            jsonWriter.WriteEndObject();
                        }

                        return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
                    }
                });
            });
        }
    }
}
