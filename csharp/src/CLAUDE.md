# C# Source Code

## Tech Stack
- .NET 10 / C# (modern style)
- ASP.NET Core (Minimal APIs)
- Azure hosting (App Service / Containers)
- Observability: Application Insights

## Repo Structure

| Directory | Purpose |
|-----------|---------|
| **Api.MtgDiscovery.GraphQL** | Thin GraphQL HTTP entry point |
| **Lib.Domains** | Orchestration layer (delegates to Aggregators) |
| **Lib.Aggregators** | Coordinates adapter calls, combines responses |
| **Lib.Adapters** | External system integration (Cosmos, APIs) |
| **common/** | Cross-cutting libraries and abstractions |
| **core/** | Core infrastructure (Cosmos, config, HTTP) |

## Core Constraints

- No framework DI containers (constructor chaining only)
- No AutoMapper, repositories, or "magic" abstractions
- No new architectural layers (7 layers exist, don't add more)
- Always use `CancellationToken` in async calls
- No sync-over-async (no `.Result`/`.Wait`)
- No `Task.Run` in request handlers
- Outbound HTTP: must have timeouts + cancellation
- Caching: must have time budget + stampede protection + key versioning

## Dependency Injection

Constructor chaining pattern (no DI framework):
- Public constructor with logger/config
- Private constructor with actual dependencies
- Example: `Api.MtgDiscovery.GraphQL/Queries/ArtistQueryMethods.cs:24-29`

## Cross-Cutting Patterns

All layer operations use standardized "Actions":
- **ICreateMapper<TSource, TResult>** — Transform data
- **IFilterAction<TItem, TStatus>** — Exclude by criteria
- **IEnrichmentAction<TTarget>** — Add data to results
- **IValidatorAction<TItem, TStatus>** — Validate input/output
- **IResolver<TId, TResult>** — Resolve identifiers
- **IIntegrator<TTarget, TSource>** — Merge changes
- **ITransformationAction<TItem>** — Transform in-place

See: `common/csharp-layer-patterns.md` for complete documentation

## Build & Test

```bash
dotnet build
dotnet test
dotnet format --severity info
```

## Workflow

1. Clarify missing requirements before coding
2. Propose plan + files to change
3. Implement smallest working change
4. Add/update tests when relevant

