# Lib.Domains Layer

## Purpose
Pure orchestration layer that delegates business operations to Aggregators. Domain services are thin pass-through classes that receive ItrEntity objects and return IOperationResponse results. **Zero business logic** — logic lives elsewhere.

## Architecture Pattern

**Passthrough Facade → Router → (Specialized Operations) → Aggregators**

```
Apis/{Domain}DomainService                (public passthrough facade)
  ├── Commands/{Domain}CommandDomainService    (router)
  │     ├── {Behavior}Domain                   (specialized operation, optional)
  │     └── {Behavior}Domain
  └── Queries/{Domain}QueryDomainService       (router)
        ├── {Behavior}Domain                   (specialized operation, optional)
        └── {Behavior}Domain
```

### Passthrough Facade

The `{Domain}DomainService` in `Apis/` delegates every call to a command or query router. It contains NO logic — pure passthrough with `await ... .ConfigureAwait(false)`.

See: `.claude/rules/csharp/layers/domain/apis.md` for full documentation.

### Router Classes

`{Domain}CommandDomainService` and `{Domain}QueryDomainService` implement the CQRS interfaces from `Apis/`. They use one of two delegation patterns:

- **Direct passthrough**: Router calls aggregator service directly (e.g., Collections)
- **Specialized operations**: Router delegates to single-`Execute()` services (e.g., Artists)

See: `cqrs-commands.md` and `cqrs-queries.md` for full documentation.

## Entity Flow

```
ItrEntity (from Entry)  →  [Call aggregator]  →  IOperationResponse<OufEntity>  →  Entry
```

Domain services are **pure pass-through**: Take ItrEntity, call aggregator, return response. No transformation, filtering, or enrichment. The domain layer owns NO entity types — ItrEntities come from Entry/Shared, OufEntities come from Aggregator.

## Naming Conventions

| Item | Scope | Pattern | Example |
|------|-------|---------|---------|
| Composite Interface | public | `I{Domain}DomainService` | `ICollectionsDomainService` |
| Composite Implementation | public | `{Domain}DomainService` | `CollectionsDomainService` |
| Command Router | public | `{Domain}CommandDomainService` | `CollectionCommandDomainService` |
| Query Router | public | `{Domain}QueryDomainService` | `ArtistsQueryDomainService` |
| Specialized Interface | internal | `I{Behavior}Domain` | `IArtistSearchDomain` |
| Specialized Implementation | internal | `{Behavior}Domain` | `ArtistSearchDomain` |

Scope-based suffix distinction: `DomainService` = public scope, `Domain` = internal scope. Specialized interfaces inherit `IOperationResponseService<TInput, TOutput>` — never define `Execute` manually.

## Key Rules

1. **Constructor Chain**: Public logger → private dependencies (aggregator or specialized services)
2. **Pure Delegation**: ONLY call aggregator methods or specialized operation `Execute()` — no business logic
3. **ItrEntity → Aggregator → IOperationResponse**: No intermediate transforms
4. **Single Responsibility**: Each specialized operation has exactly one `Execute` method (inherited from `IOperationResponseService`)
5. **Composite Pattern**: Main service inherits from all CQRS interfaces
6. **ConfigureAwait(false)**: All async calls
7. **No Exceptions**: Return `IOperationResponse<T>`, never throw
8. **No Entities, Mappers, or Exceptions**: Domain projects contain only `Apis/`, `Commands/`, and `Queries/` folders

## Key Patterns

**Direct Passthrough (Collections)**:
- Router: `CollectionCommandDomainService.cs` — single aggregator dependency, delegates all methods directly
- Simplest pattern, used when all operations go to the same aggregator

**Specialized Operations (Artists)**:
- Router: `ArtistsQueryDomainService.cs` — constructs specialized operations, delegates via `Execute()`
- Specialized: `ArtistSearchDomain.cs` — inherits `IOperationResponseService`, single `Execute()` calling aggregator
- Used when operations need individual service isolation

## When Adding a New Domain Service

1. Create composite interface in `Apis/` (inherits CQRS interfaces)
2. Create passthrough facade in `Apis/`
3. Create router class in `Commands/` and/or `Queries/`
4. Choose delegation pattern:
   - **Direct passthrough**: If all operations go to a single aggregator
   - **Specialized operations**: If operations need isolation — create `I{Behavior}Domain` (inheriting `IOperationResponseService`) + `{Behavior}Domain` per operation
5. Register in facade

See: `Lib.Domain.Collections/` for direct passthrough, `Lib.Domain.Artists/` for specialized operations.
