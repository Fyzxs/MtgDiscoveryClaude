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

## Constraints (not covered in rules)

- Always use `CancellationToken` in async calls
- No sync-over-async (no `.Result`/`.Wait`)
- No `Task.Run` in request handlers
- Outbound HTTP: must have timeouts + cancellation
- Caching: must have time budget + stampede protection + key versioning

## Detailed Documentation

See `.claude/rules/csharp/` for complete pattern documentation:

| File | Covers |
|------|--------|
| `architecture-guide.md` | 7-layer architecture, entity pipeline, key patterns, DI |
| `csharp-code-style.md` | MicroObjects philosophy, code style, red flags |
| `graphql-conventions.md` | GraphQL query/mutation implementation |
| `testing-guide.md` | Test patterns and conventions |
| `exceptions.md` | Exception hierarchy and patterns |

## Build & Test

```bash
dotnet build
dotnet test
dotnet format --severity info
```
