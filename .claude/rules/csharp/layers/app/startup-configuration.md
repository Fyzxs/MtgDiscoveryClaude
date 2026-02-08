---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs"
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs"
---

# Startup & Program Configuration

## Program Entry Point

`AppMtgDiscoveryGraphQlProgram.cs` configures the host:

```csharp
internal static class AppMtgDiscoveryGraphQlProgram
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(...)
            .ConfigureLogging(...)
            .ConfigureServices(...)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
```

### Configuration Precedence

1. `appsettings.json` (required)
2. `appsettings.{EnvironmentName}.json` (required, environment-specific)
3. `local.settings.json` (optional, local dev only)
4. Environment variables (highest priority)
5. Azure App Configuration (RELEASE builds only, non-Local environments)

### Conditional Compilation

Two methods use `[Conditional("RELEASE")]`:
- `ConfigureAppConfiguration` -- connects to Azure App Configuration for Auth0 settings
- `EntraAuth` -- registers `DefaultAzureCredential` via `ServiceLocator`

These are no-ops in DEBUG builds, allowing local development without Azure dependencies.

#### ConfigureAppConfiguration Detail

```csharp
[Conditional("RELEASE")]
private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
{
    if (hostingContext.HostingEnvironment.IsEnvironment("Local"))
        return;

    IConfigurationRoot tempConfig = config.Build();
    string appConfigEndpoint = tempConfig["AppConfiguration:Endpoint"];

    if (string.IsNullOrEmpty(appConfigEndpoint))
        return;

    _ = config.AddAzureAppConfiguration(options =>
    {
        _ = options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
            .Select("Auth0:*");
    });
}
```

**Key behaviors:**
- `[Conditional("RELEASE")]` makes this a no-op in DEBUG builds — local dev never hits Azure
- Even in RELEASE, the `"Local"` environment skips Azure App Configuration
- The method builds a temporary config to read the endpoint URL, then adds Azure App Configuration as a source
- Only `Auth0:*` keys are selected from Azure App Configuration

#### EntraAuth Detail

```csharp
[Conditional("RELEASE")]
private static void EntraAuth()
{
    DefaultAzureCredential defaultAzureCredential = new();
    ServiceLocator.ServiceRegister<TokenCredential>(() => defaultAzureCredential);
}
```

Registers a `DefaultAzureCredential` in the `ServiceLocator` for infrastructure components that need Azure authentication (e.g., Cosmos DB). This is only active in RELEASE builds — local development uses connection strings or emulators instead.

### Logging

```csharp
.ConfigureLogging(loggingBuilder =>
{
    _ = loggingBuilder.ClearProviders();
    _ = loggingBuilder.AddConsole();
    _ = loggingBuilder.AddApplicationInsights();
})
```

## Startup Class

`Startup.cs` configures services and the middleware pipeline.

### ConfigureServices

Registration order:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 1. Core services
    _ = services.AddSingleton(_configuration);
    _ = services.AddApplicationInsightsTelemetry();

    // 2. CORS
    _ = services.AddCors(options => { ... });

    // 3. Logging (ILogger singleton for GraphQL layer)
    _ = services.AddLogging();
    _ = services.AddSingleton(sp =>
        sp.GetRequiredService<ILoggerFactory>().CreateLogger("GraphQL"));

    // 4. Authentication (Auth0 JWT)
    _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => { ... });
    _ = services.AddAuthorization();

    // 5. Health checks
    _ = services.AddHealthChecks();

    // 6. HotChocolate GraphQL
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
```

### Middleware Pipeline

Order matters -- this is the exact sequence:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
        _ = app.UseDeveloperExceptionPage();

    _ = app.UseHttpsRedirection();
    _ = app.UseRouting();
    _ = app.UseCors();
    _ = app.UseAuthentication();
    _ = app.UseAuthorization();
    _ = app.UseEndpoints(endpoints => { ... });
}
```

### Health Check Endpoints

| Endpoint | Purpose |
|----------|---------|
| `/health` | Basic liveness (always 200) |
| `/health/startup` | Container startup probe |
| `/health/live` | Container liveness probe |
| `/health/ready` | Readiness probe (checks "ready" tagged dependencies) |
| `/health/check` | General health |

### GraphQL Endpoint

```csharp
_ = endpoints.MapGraphQL()
    .WithOptions(new GraphQLServerOptions
    {
        Tool = { Enable = true }  // Banana Cake Pop UI enabled
    });
```

## Auth0 JWT Configuration

```csharp
options.Authority = $"https://{_configuration["Auth0:Domain"]}/";
options.Audience = _configuration["Auth0:Audience"];
options.TokenValidationParameters = new TokenValidationParameters
{
    NameClaimType = "sub",
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidAudiences = [_configuration["Auth0:Audience"] ?? "api://mtg-discovery"]
};
```

## CORS Configuration

- **Development/Local**: `AllowAnyOrigin()`, `AllowAnyHeader()`, `AllowAnyMethod()`
- **Production**: Comma-separated `AllowedOrigins` config, POST/GET only, credentials allowed

## Environment-Gated Features

| Feature | Development | Production |
|---------|-------------|------------|
| Exception details in errors | Enabled | Disabled |
| GraphQL introspection | Enabled | Disabled |
| Developer exception page | Enabled | Disabled |
| Azure App Configuration | Disabled (DEBUG) | Enabled (RELEASE) |
| Entra auth credential | Disabled (DEBUG) | Enabled (RELEASE) |

## Reference Files

- **Entry point**: `AppMtgDiscoveryGraphQlProgram.cs`
- **DI + middleware**: `Startup.cs`
- **Schema extensions**: `Schemas/ApiQueryExtensions.cs`, `Schemas/ApiMutationExtensions.cs`
- **Error filter**: `ErrorHandling/HttpStatusCodeErrorFilter.cs`
