---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/**"
---

# Entry APIs Folder

The `Apis/` folder is the **public contract** for the Entry layer. Everything here MUST be `public` scoped. Internal operation classes do NOT belong here.

## Files in Apis/

| File | Purpose |
|------|---------|
| `IEntryService.cs` | Composite interface — inherits all domain-specific sub-service interfaces |
| `EntryService.cs` | Passthrough facade — delegates to routers and sub-services |
| `I{Domain}EntryService.cs` | Domain-specific interfaces (e.g., `ICardEntryService`) |
| `{Domain}EntryService.cs` | Simple sub-services when no CQRS split is needed |
| `I{Arg}ArgEntity.cs` | Arg entity interfaces shared across domains |

## Composite Interface

The composite interface inherits from all domain-specific sub-service interfaces and defines NO methods itself — pure composition.

```csharp
public interface IEntryService :
    ICardEntryService,
    ISetEntryService,
    IArtistEntryService,
    IUserEntryService,
    IUserCardsEntryService,
    IUserCardsQueryEntryService,
    IUserSetCardsQueryEntryService,
    IUserSetCardsCommandEntryService,
    IUserWishlistCardsEntryService,
    ISealedProductsEntryService,
    IUserSealedProductsEntryService,
    ICollectionEntryCommandService,
    ICollectionEntryQueryService;
```

## Passthrough Facade Pattern

`EntryService` is a **pure passthrough facade**. It constructs all sub-services and delegates every method call directly. It MUST NOT contain any logic.

### Constructor Pattern

The facade constructs all sub-services via constructor chaining:

```csharp
public sealed class EntryService : IEntryService
{
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;
    // ... all sub-services

    public EntryService(ILogger logger) : this(
        new CardEntryService(logger),
        new SetEntryService(logger),
        // ... all sub-service constructors
    ) { }

    private EntryService(
        ICardEntryService cardEntryService,
        ISetEntryService setEntryService,
        // ... all parameters
    )
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        // ... all assignments
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args, CancellationToken cancellationToken)
        => await _cardEntryService.CardsByIdsAsync(args, cancellationToken)
            .ConfigureAwait(false);

    // Every method follows this exact delegation pattern
}
```

### Key Rules

1. Every method is a single `await` delegation with `.ConfigureAwait(false)`
2. No conditional logic, no mapping, no error handling — pure passthrough
3. Method signatures accept arg entity interfaces plus `CancellationToken`
4. Return type is always `Task<IOperationResponse<TOutEntity>>`

## Domain Sub-Service Interfaces

Each domain has a dedicated interface defining its operations:

```csharp
public interface ICardEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(
        ISetCodeArgEntity setCode, CancellationToken cancellationToken);
    // ...
}
```

For domains with CQRS split, separate command and query interfaces exist:

```csharp
public interface ICollectionEntryCommandService { /* command methods */ }
public interface ICollectionEntryQueryService { /* query methods */ }
```

## Simple Sub-Services

When a domain needs no CQRS split and has few operations, the sub-service can live directly in `Apis/`:

```csharp
internal sealed class SealedProductsEntryService : ISealedProductsEntryService
{
    private readonly ISealedProductsBySetCodeEntryService _sealedProductsBySetCode;

    public SealedProductsEntryService(ILogger logger)
        : this(new SealedProductsBySetCodeEntryService(logger)) { }

    private SealedProductsEntryService(
        ISealedProductsBySetCodeEntryService sealedProductsBySetCode)
        => _sealedProductsBySetCode = sealedProductsBySetCode;

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeArgEntity args, CancellationToken cancellationToken)
        => await _sealedProductsBySetCode.Execute(args, cancellationToken)
            .ConfigureAwait(false);
}
```

## Method Contracts

All methods on public Entry interfaces MUST:
- Accept arg entity interface(s) plus `CancellationToken`
- Return `Task<IOperationResponse<TOutEntity>>`

## Reference Implementation

`Lib.MtgDiscovery.Entry/Apis/EntryService.cs` is the canonical reference for the facade pattern.
