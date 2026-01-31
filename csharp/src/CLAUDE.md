# CSHARP SOURCE CODE

## Tech stack
- .NET 10 / C# (modern style)
- ASP.NET Core (Minimal APIs)
- Azure hosting (App Service / Containers)
- Observability: Application Insights (logs + metrics)

## Repo map
- Api.MtgDiscovery.GraphQl  → The GraphQL Entry project, thin as can be
- Lib.Domains               → Access Layer for higher layers, knows about aggregators
- Lib.Aggregators           → Translates/aggregates to/from Adapters
- Lib.Adapters              → Knows about external entities
- common/                   → common libraries used across the project
- core                      → core libraries
- testShared/               → functionality shared across tests

## Hard rules (do not violate)
- NEVER add new framework layers or "Clean Architecture cosplay".
- NEVER introduce patterns we don't use (AutoMapper, repositories, magic abstractions).
- NEVER use dependency injection frameworks.
- Always pass CancellationToken through all async calls.
- No sync-over-async (no .Result/.Wait).
- No Task.Run inside request handlers.
- Outbound HTTP MUST have timeouts + cancellation.
- Caching MUST have: time budget, stampede protection strategy, and key versioning.

## Code Style
Use `.claude/rules/csharp-code-style.md` for details when writing C# code.

## Default workflow 
1) Ask for missing requirements before changing code.
2) Propose a plan + list files to touch.
3) Implement smallest change that works.
4) Add/update tests when relevant.

## Commands (edit to match your codebase)
- Build: `dotnet build`
- Test: `dotnet test`
- Format: `dotnet format --severity info`

## Output format
- Prefer short sections, small code blocks, and explain trade-offs.
- When making changes: show diff-level guidance + why.

## Common Patterns
Common patterns are represented by "Actions"
- Actions are cross-cutting patterns. Base abstractions are in Lib.Shared.Abstractions/Actions/

Common Action types:
- Validators (check if input valid)
- Mappers (transform between entity types)
- Enrichments (add data to results)
- Filters (exclude items by criteria)
- Integrators (merge delta changes)
- Transformations (sync in-place modifications)
- Resolvers (resolve input with context)

Whenever this type of operation is happening, the interface for it should implement one of the interfaces for these actions.

# Dependency Inversion
- Through Constructor Chaining
- Real Example: Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/ArtistQueryMethods .cs (lines 24-29)

