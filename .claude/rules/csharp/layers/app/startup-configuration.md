---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs"
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs"
---

# Startup & Program Configuration

## Program Entry Point

`AppMtgDiscoveryGraphQlProgram.cs` configures the host:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs`

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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs`

**Key behaviors:**
- `[Conditional("RELEASE")]` makes this a no-op in DEBUG builds — local dev never hits Azure
- Even in RELEASE, the `"Local"` environment skips Azure App Configuration
- The method builds a temporary config to read the endpoint URL, then adds Azure App Configuration as a source
- Only `Auth0:*` keys are selected from Azure App Configuration

#### EntraAuth Detail

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs`

Registers a `DefaultAzureCredential` in the `ServiceLocator` for infrastructure components that need Azure authentication (e.g., Cosmos DB). This is only active in RELEASE builds — local development uses connection strings or emulators instead.

### Logging

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/AppMtgDiscoveryGraphQlProgram.cs`

## Startup Class

`Startup.cs` configures services and the middleware pipeline.

### ConfigureServices

Registration order:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

### Middleware Pipeline

Order matters -- this is the exact sequence:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

### Health Check Endpoints

| Endpoint | Purpose |
|----------|---------|
| `/health` | Basic liveness (always 200) |
| `/health/startup` | Container startup probe |
| `/health/live` | Container liveness probe |
| `/health/ready` | Readiness probe (checks "ready" tagged dependencies) |
| `/health/check` | General health |

### GraphQL Endpoint

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

## Auth0 JWT Configuration

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

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
