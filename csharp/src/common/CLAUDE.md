# Common Libraries

## Purpose
Shared infrastructure, abstractions, and domain-specific libraries used across the entire platform. These establish cross-cutting patterns for data flow, service execution, error handling, and external integrations.

## Projects

### Lib.Shared.DataModels
Fundamental entity type hierarchy that defines the data flow contract between architectural layers.

**Key interfaces** (marker patterns for type safety):
- `IArgEntity` - Argument entities from external input (GraphQL, REST)
- `IItrEntity` - Internal transfer entities between service layers
- `IOufEntity` - Output from domain/aggregator layers (internal flow)
- `IOutEntity` - Output entities to external layers (GraphQL responses)
- `IXfrEntity` - Transfer entities from aggregator to adapter layer
- `ICacheableEntity` - Base for cacheable entities with cache key

See: `Lib.Shared.DataModels/Abstractions/`

**Usage**: Referenced by all layers (Entry, Domain, Aggregators, Adapters)

### Lib.Shared.Abstractions
Fundamental service execution patterns and utility abstractions.

**Key abstractions**:
- `IServiceExecute<TInput, TOutput>` - Base service contract

See: `Lib.Shared.Abstractions/` folders (Services, Identifiers)

**Usage**: Foundation for Entry Layer, used by all other common libraries

### Lib.Shared.Invocation
Advanced cross-cutting patterns for service invocation, error handling, and command infrastructure.

**Key components**:
- `IOperationResponse<T>` - Operation response contract with HTTP status codes
- `OperationException` hierarchy - Exception types with HTTP status mapping
- `IExecutionContext` / `IAuthNExecutionContext` - Execution context with caller info
- `ICaller` - Authenticated caller information
- Command infrastructure (`ICommandArg`, command validators, filters)
- Response factories for converting operations to response models

See: `Lib.Shared.Invocation/Operations/`, `Lib.Shared.Invocation/Exceptions/`, `Lib.Shared.Invocation/ExecutionContext/`

**Usage**: All adapters return `IOperationResponse<T>`, Entry layer uses execution context, App layer uses response factories


## Established Patterns

### Entity Type Contracts (Lib.Shared.DataModels)
Marker interfaces establish layer boundaries without implementation complexity. Enables compile-time type safety for data transformations across layers.

### Operation Response Pattern (Lib.Shared.Invocation)
All operations return `IOperationResponse<T>`. HTTP status codes are baked into exception hierarchy, enabling consistent error handling across all adapters.

See: `Lib.Shared.Invocation/Operations/IOperationResponse.cs`, `Lib.Shared.Invocation/Exceptions/OperationException.cs`

### Rate-Limited HTTP Client (Lib.Scryfall.Ingestion)
HTTP client uses Polly for sophisticated rate limiting respecting external API constraints. Encapsulated behind clean interface.

See: `Lib.Scryfall.Ingestion/Http/RateLimitedHttpClient.cs`, `Lib.Scryfall.Ingestion/Http/ScryfallRateLimiter.cs`

### Configuration as Immutable Objects (Lib.Scryfall.Ingestion)
Configuration stored as immutable domain objects with marker types. Both config-file-backed and direct instances support clean dependency injection.

See: `Lib.Scryfall.Ingestion/Configuration/` - Look for `IConfig*` interfaces and `Config*` implementations

## When to Add to Common

**Add to Common if**:
- It establishes a cross-cutting pattern or abstraction
- It's shared infrastructure (like the Operation Response pattern)
- It's a domain-agnostic utility (like Lib.Shared.Abstractions)

**Don't add to Common if**:
- It's specific to one domain (put it in Domain layer instead)
- It's only used by adapters (put it in the specific Adapter project)
- It's only used by one or two related projects

## Key Principle

Common libraries are **foundational infrastructure** - focus on establishing clear abstractions and contracts that multiple layers depend on. Changes here impact the entire platform, so prioritize stability and clarity.
