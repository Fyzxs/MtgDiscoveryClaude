---
paths:
  - "csharp/src/Lib.Domains/**/Apis/**"
---

# Domain APIs Folder

The `Apis/` folder is the **public contract** for a domain project. Everything here MUST be `public` scoped. Internal operation classes do NOT belong here.

## Files in Apis/

| File | Purpose |
|------|---------|
| `I{Domain}DomainService.cs` | Composite interface — inherits both command and query interfaces |
| `{Domain}DomainService.cs` | Passthrough facade — delegates to command/query routers |
| `I{Domain}CommandDomainService.cs` | Command interface — defines all command operations |
| `I{Domain}QueryDomainService.cs` | Query interface — defines all query operations |

For single-CQRS projects (query-only or command-only), the composite interface inherits only the relevant CQRS interface, and the facade has a single dependency.

## Composite Interface

The composite interface inherits from the CQRS-specific interfaces and defines NO methods itself — pure composition.

```csharp
public interface ICollectionsDomainService
    : ICollectionCommandDomainService, ICollectionQueryDomainService
{
}
```

For query-only projects:

```csharp
public interface IArtistDomainService : IArtistsQueryDomainService
{
}
```

## Passthrough Facade Pattern

The `{Domain}DomainService` in `Apis/` is a **pure passthrough facade**. It constructs command/query router classes and delegates every method call directly. It MUST NOT contain any logic.

### Constructor Pattern

- **Both command + query**: 2 dependencies (command router + query router)
- **Query-only or command-only**: 1 dependency (single router)

### Implementation (Both CQRS)

```csharp
public sealed class CollectionsDomainService : ICollectionsDomainService
{
    private readonly ICollectionCommandDomainService _commandService;
    private readonly ICollectionQueryDomainService _queryService;

    public CollectionsDomainService(ILogger logger) : this(
        new CollectionCommandDomainService(logger),
        new CollectionQueryDomainService(logger))
    { }

    private CollectionsDomainService(
        ICollectionCommandDomainService commandService,
        ICollectionQueryDomainService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(
        ICollectionItrEntity entity, CancellationToken cancellationToken)
        => await _commandService.CreateCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

    // Every method follows this exact delegation pattern — no logic, no branching.
}
```

### Implementation (Single CQRS)

```csharp
public sealed class ArtistDomainService : IArtistDomainService
{
    private readonly IArtistsQueryDomainService _artistDomainOperations;

    public ArtistDomainService(ILogger logger) : this(new ArtistsQueryDomainService(logger))
    { }

    private ArtistDomainService(IArtistsQueryDomainService artistDomainOperations)
        => _artistDomainOperations = artistDomainOperations;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(
        IArtistSearchTermItrEntity searchTerm, CancellationToken cancellationToken)
        => await _artistDomainOperations.ArtistSearchAsync(searchTerm, cancellationToken)
            .ConfigureAwait(false);
}
```

### Key Rules

1. Every method is a single `await` delegation with `.ConfigureAwait(false)`
2. No conditional logic, no mapping, no error handling — pure passthrough
3. Method signatures accept a single `IItrEntity` param plus `CancellationToken`
4. Return type is always `Task<IOperationResponse<IOufEntity>>`

## Method Contracts

All methods on the public `I{Domain}DomainService` MUST:
- Accept a single `IItrEntity` parameter plus `CancellationToken`
- Return `Task<IOperationResponse<IOufEntity>>`

Any specialized operation interfaces do not belong here.

## Reference Implementation

`Lib.Domain.Collections/Apis/` follows this pattern exactly and is the canonical reference.
