# Common Libraries

## Purpose
Shared infrastructure and abstractions used across all layers. These establish cross-cutting patterns for data flow, service execution, error handling, and integrations.

## Key Projects

**Lib.Shared.DataModels**: Entity type contracts
- `IArgEntity` — Input entities (GraphQL)
- `IItrEntity` — Internal transfer entities
- `IOutEntity` — Output entities (GraphQL responses)
- `IXfrEntity` — Transfer entities (Aggregator → Adapter)
- `IOufEntity` — Output from Adapter layer

See: `Lib.Shared.DataModels/Abstractions/`

**Lib.Shared.Abstractions**: Service patterns
- `IServiceExecute<TInput, TOutput>` — Base service contract
- Cross-cutting action patterns (see `csharp-layer-patterns.md`)

See: `Lib.Shared.Abstractions/`

**Lib.Shared.Invocation**: Operation handling
- `IOperationResponse<T>` — Operation response with HTTP status
- `OperationException` hierarchy — Exception types with status mapping
- `IExecutionContext` — Execution context with caller info
- Response factories — Converting operations to response models

See: `Lib.Shared.Invocation/Operations/`, `Lib.Shared.Invocation/Exceptions/`

## Established Patterns

**Entity Type Contracts**: Marker interfaces establish layer boundaries. Compile-time type safety for data transformations.

**Operation Response**: All operations return `IOperationResponse<T>`. HTTP status codes baked into exception hierarchy.

**Rate-Limited HTTP**: Polly-based rate limiting for external APIs (Lib.Scryfall.Ingestion)

**Configuration as Objects**: Immutable domain objects from config files, not scattered strings

## When to Add

**Add to Common if**:
- Establishes cross-cutting pattern
- Shared infrastructure (all layers use it)
- Domain-agnostic utility

**Don't add if**:
- Domain-specific (put in Domain layer)
- Only used by Adapters (keep in Adapter project)
- Only used by 1-2 projects (keep local)

## Key Principle

Common libraries are **foundational** — focus on clear abstractions and contracts. Changes impact the entire platform, so prioritize stability.
