# Lib.Domains Layer

## Purpose
Pure orchestration layer that delegates business operations to Aggregators. Domain services are thin pass-through classes that receive ItrEntity objects and return IOperationResponse results. **Zero business logic** — logic lives elsewhere.

## Architecture Pattern

**Composite Service → Passthrough Service → Specialized Operations → (Aggregators)**

Example: `Lib.Domain.Artists/Apis/`
- Interface: `IArtistDomainService.cs` (composite, inherits all specialized services)
- Service: `ArtistsQueryDomainService.cs` (passthrough pattern)
- Operations: `ArtistSearchDomainService.cs`, `CardsByArtistDomainService.cs`

## Entity Flow

```
ItrEntity (from Entry)  →  [Call aggregator]  →  IOperationResponse<OufEntity>  →  Entry
```

Domain services are **pure pass-through**: Take ItrEntity, call aggregator, return response. No transformation, filtering, or enrichment.

## Key Pattern

**Specialized Operation Service**: Single Execute method calling aggregator directly
- Constructor chain: Public logger → private aggregator
- Invoke pattern: `ItrEntity in → aggregator call → IOperationResponse<OufEntity> out`

See: `ArtistSearchDomainService.cs:14-24` (complete example)

## Key Rules

1. **Constructor Chain**: Public logger → private aggregator service
2. **Pure Delegation**: ONLY call aggregator methods, no business logic
3. **ItrEntity → Aggregator → IOperationResponse**: No intermediate transforms
4. **Single Responsibility**: Each operation service has exactly one Execute method
5. **Composite Pattern**: Main service inherits from all specialized interfaces
6. **ConfigureAwait(false)**: All async calls
7. **No Exceptions**: Return `IOperationResponse<T>`, never throw

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| Composite Interface | `I{Domain}DomainService` | `IArtistDomainService` |
| Passthrough Service | `{Operation}QueryDomainService` or `{Operation}CommandDomainService` | `ArtistsQueryDomainService` |
| Specialized Interface | `I{Operation}{Domain}DomainService` | `IArtistSearchDomainService` |
| Specialized Implementation | `{Operation}{Domain}DomainService` | `ArtistSearchDomainService` |

## When Adding a New Domain Service

1. Create composite interface in `Apis/`
2. Create passthrough service in `Queries/` or `Commands/`
3. Create specialized operation service (single Execute method calling aggregator)
4. Register in composite interface

See: `Lib.Domain.Artists/` for complete example
