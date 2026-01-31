using System;
using App.MtgDiscovery.GraphQL.ErrorHandling;
using App.MtgDiscovery.GraphQL.Schemas;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App.MtgDiscovery.GraphQL;

internal sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Register IConfiguration for DI
        _ = services.AddSingleton(_configuration);

        // Configure Application Insights
        _ = services.AddApplicationInsightsTelemetry();

        _ = services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                string[] allowedOrigins = _configuration["AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
                bool isLocalDevelopment = _environment.IsDevelopment() || _environment.EnvironmentName == "Local";
                if (isLocalDevelopment || allowedOrigins.Length == 0)
                {
                    _ = policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                }
                else
                {
                    _ = policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .WithMethods("GET", "POST")
                          .AllowCredentials();
                }
            });
        });

        _ = services.AddLogging();

        // Register ILogger as a singleton service
        _ = services.AddSingleton<ILogger>(sp =>
            sp.GetRequiredService<ILoggerFactory>().CreateLogger("GraphQL"));

        // Configure Auth0 JWT Authentication
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{_configuration["Auth0:Domain"]}/";
                options.Audience = _configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "sub", // Auth0 uses "sub" not ClaimTypes.NameIdentifier
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Restrict to primary API audience only
                    ValidAudiences =
                    [
                        _configuration["Auth0:Audience"] ?? "api://mtg-discovery"
                    ]
                };
            });

        _ = services.AddAuthorization();

        // Add health checks
        _ = services.AddHealthChecks();

        // Register query method classes for DI
        //services.AddScoped<CardQueryMethods>();
        //services.AddScoped<SetQueryMethods>();
        //services.AddScoped<ArtistQueryMethods>();

        _ = services
            .AddGraphQLServer()
            .AddApiQuery()
            .AddApiMutation()
            .AddSetSchemaExtensions()
            .AddArtistSchemaExtensions()
            .AddSealedProductsSchemaExtensions()
            .AddAuthorization()
            .AddErrorFilter<HttpStatusCodeErrorFilter>()
            .ModifyRequestOptions(opt =>
            {
                opt.IncludeExceptionDetails = _environment.IsDevelopment();
            })
            .DisableIntrospection(_environment.IsDevelopment() is false)
            .UseDefaultPipeline()
            .AddDefaultTransactionScopeHandler()
            .ModifyOptions(o => o.EnableDefer = true);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseRouting();
        _ = app.UseCors();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.UseEndpoints(endpoints =>
        {
            // Basic health endpoint for Container Apps probes
            _ = endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => false // No checks, just returns 200 if app is alive
            });

            // Startup probe - checks if the app has finished starting up
            _ = endpoints.MapHealthChecks("/health/startup", new HealthCheckOptions
            {
                Predicate = _ => false // No checks, just returns 200 if app is alive
            });

            // Liveness probe - just checks if the app is running
            _ = endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false // No checks, just returns 200 if app is alive
            });

            // Readiness probe - checks dependencies (Cosmos DB, etc.)
            _ = endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready")
            });

            // General health endpoint
            _ = endpoints.MapHealthChecks("/health/check");

            _ = endpoints.MapGraphQL()
                .WithOptions(new GraphQLServerOptions
                {
                    Tool = { Enable = true }
                });
        });
    }
}
