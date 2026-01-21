using App.MtgDiscovery.GraphQL.ErrorHandling;
using App.MtgDiscovery.GraphQL.Schemas;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
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

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Register IConfiguration for DI
        _ = services.AddSingleton(_configuration);

        _ = services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                _ = policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
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
                    // Allow both API audiences from the token
                    ValidAudiences =
                    [
                        "api://mtg-discovery", // Primary API audience
                        "https://dev-63szoyl0kt0p7e5q.us.auth0.com/userinfo", // Auth0 userinfo endpoint
                        _configuration["Auth0:ClientId"]  // ID token audience (optional, for backwards compat)
                    ]
                };
            });

        _ = services.AddAuthorization();

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
            .AddUserSetCardsSchema()
            .AddAuthorization()
            .AddErrorFilter<HttpStatusCodeErrorFilter>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
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
            _ = endpoints.MapGraphQL()
                .WithOptions(new GraphQLServerOptions
                {
                    Tool = { Enable = true }
                });
        });
    }
}
