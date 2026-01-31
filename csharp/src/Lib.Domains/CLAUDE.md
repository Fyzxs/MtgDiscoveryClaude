# Lib.Domains Layer

## Purpose
Pure orchestration layer that delegates business operations to aggregators. Domain services are thin pass-through classes that receive ItrEntity objects and return IOperationResponse results. **Zero business logic belongs here** — logic lives in specialized operation services that directly delegate to aggregators.

## Layer Pattern

**Composite Service → Passthrough Service → Specialized Operation Services → (Aggregators)**

```
IArtistDomainService (composite, public)
  ↓ delegates to
ArtistsQueryDomainService (passthrough service)
  ↓ delegates to
IArtistSearchDomainService, ICardsByArtistDomainService (specialized operations)
  ↓ call
IArtistAggregatorService (at aggregator layer)
```

See: `Lib.Domain.Artists/Apis/`
- Interface: `IArtistDomainService.cs` (composite interface)
- Passthrough Service: `ArtistsQueryDomainService.cs` (delegates to specialized services)
- Specialized Operations: `ArtistSearchDomainService.cs`, `CardsByArtistDomainService.cs`

## Architecture

### Public API
- **Composite Service Interface** (e.g., `IArtistDomainService`): Inherits from all specialized domain service interfaces (queries and commands)
- **Passthrough Service Implementation**: Delegates to specialized operation services
- **Specialized Service Interfaces**: Single-method interfaces for each operation (e.g., `IArtistSearchDomainService`)

See: `Lib.Domain.Artists/Apis/IArtistDomainService.cs` (composite pattern), `IArtistsQueryDomainService.cs` (specialized)

### Entity Flow

```
ItrEntity (input from Entry layer)
  ↓ [Pass to aggregator]
Aggregator processes via adapters
  ↓ [Receives back]
IOperationResponse<OufEntity> (returned to Entry layer)
```

Domain services are **pure pass-through** — they take an ItrEntity, call an aggregator method, and return the response. No transformation, filtering, or enrichment.

See: `ArtistSearchDomainService.cs:23` (single line delegates to aggregator)

### Domain Service Operation Pattern

Each specialized operation service:
1. Constructor chain (public with logger, private with aggregator dependency)
2. Single Execute method: receive ItrEntity → call aggregator → return IOperationResponse
3. No exception throwing (aggregator handles that)
4. ConfigureAwait(false) on all async calls

See: `Lib.Domain.Artists/Queries/ArtistSearchDomainService.cs:14-24`
- Constructor chaining: lines 18-21 (logger → aggregator → private)
- Execute pattern: line 23 (ItrEntity in → aggregator call → IOperationResponse out)
- ConfigureAwait: line 23

### Passthrough Service Pattern

Primary service delegates to specialized operation services without additional logic. Constructor chains specialized services and forwards calls.

See: `ArtistsQueryDomainService.cs:11-40`
- Constructor chaining: lines 17-31 (logger → specialized services → private)
- Delegation pattern: lines 33-40 (receives ItrEntity → calls specialized service → returns response)

## Key Rules

1. **Constructor Chain**: Public constructor with logger, private with aggregator service(s)
2. **Pure Delegation**: ONLY call aggregator methods, never add business logic
3. **ItrEntity → Aggregator → IOperationResponse**: No intermediate transforms
4. **Single Responsibility**: Each operation service has exactly one Execute method
5. **Composite Pattern**: Main service inherits from all specialized interfaces
6. **ConfigureAwait(false)**: All async calls include `.ConfigureAwait(false)`
7. **No Exceptions**: All methods return `IOperationResponse<T>`, never throw

## Common Patterns

### Composite Service Interface
Inherits from all specialized query/command interfaces. See: `IArtistDomainService.cs:3`

### Passthrough Service
Chains specialized services in constructor, delegates all calls. See: `ArtistsQueryDomainService.cs:17-40`

### Specialized Operation Service
Single-method service that calls aggregator directly. See: `ArtistSearchDomainService.cs:14-24`

### Constructor Chaining with Logger
Public constructor accepts logger, private accepts aggregator service. See: `ArtistSearchDomainService.cs:18-21`

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Composite Interface** | `I{Domain}DomainService` | `IArtistDomainService` |
| **Passthrough Service** | `{Operation}QueryDomainService` or `{Operation}CommandDomainService` | `ArtistsQueryDomainService` |
| **Specialized Interface** | `I{Operation}{Domain}DomainService` | `IArtistSearchDomainService` |
| **Specialized Implementation** | `{Operation}{Domain}DomainService` | `ArtistSearchDomainService` |
| **ItrEntity** | `I{Entity}ItrEntity` | `IArtistSearchTermItrEntity` |
| **OufEntity** | `I{Entity}OufEntity` | `IArtistSearchResultCollectionOufEntity` |

## Data Flow Checklist

When implementing a new domain service:
- ✓ Receive ItrEntity from Entry layer
- ✓ Call aggregator method with ItrEntity (inject aggregator service)
- ✓ Return IOperationResponse<OufEntity> directly (no mapping)
- ✓ All async calls use ConfigureAwait(false)
- ✓ No business logic (Aggregator layer owns coordination, Entry layer owns validation)
- ✓ No entity transformation (Aggregator handles request/response mapping)

See: `ArtistSearchDomainService.cs:23` (complete single-operation example)

## When to Add a New Domain Service

1. Create composite interface in `Apis/` (inherits from all specialized service interfaces)
2. Create passthrough service in `Queries/` or `Commands/` (delegates to specialized services)
3. Create specialized operation service (single Execute method calling aggregator)
4. Register in composite service interface

See structure: `Lib.Domain.Artists/`

