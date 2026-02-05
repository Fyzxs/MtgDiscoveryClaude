# Cosmos Inquisition Pattern

## Purpose

Inquisitions provide **parameterized query operations** for Cosmos DB. They execute queries against containers and return collections of results.

## Base Types

- **Non-parameterized**: `ICosmosInquisition`
- **Parameterized**: `ICosmosInquisition<TParameters>`
- **Location**: `core/Lib.Cosmos/Apis/Operators/ICosmosInquisition.cs`

## Method Signatures

```csharp
// Non-parameterized
Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(CancellationToken cancellationToken = default);

// Parameterized
Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(TParameters parameters, CancellationToken cancellationToken = default);
```

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Inquisition | `{Query}Inquisition` | `UserCardItemsBySetInquisition` |
| Parameters | `{Query}InquisitionArgs` or `{Query}ExtEntitys` | `UserCardItemsBySetExtEntitys` |

**Note**: Parameter entities use the plural suffix `ExtEntitys` (not `ExtEntities`).

## Implementation Pattern

```csharp
public sealed class UserCardItemsBySetInquisition : ICosmosInquisition<UserCardItemsBySetExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public UserCardItemsBySetInquisition(ILogger logger)
        : this(new UserCardsInquisitor(logger), new UserCardItemsBySetQueryDefinition())
    { }

    private UserCardItemsBySetInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(
        UserCardItemsBySetExtEntitys args,
        CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@userId", args.UserId)
            .WithParameter("@setId", args.SetId);

        PartitionKey partitionKey = new(args.UserId);

        return await _inquisitor.QueryAsync<T>(query, partitionKey, cancellationToken)
            .ConfigureAwait(false);
    }
}
```

**Key points:**
- Implement `ICosmosInquisition<TParameters>`
- Use `ICosmosInquisitor` for query execution
- Use `InquiryDefinition` for SQL query definition
- Parameters bound via `WithParameter()`
- Always specify partition key for efficiency

## Locations

| Type | Path |
|------|------|
| Inquisitions | `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/` |
| Parameter args | `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/Entities/` |

## Parameter Entity Pattern

```csharp
public sealed class CardsBySetIdInquisitionArgs
{
    public string SetId { get; init; }
}
```

**Key points:**
- Use `init` for immutable properties
- Keep focused — only include query parameters

## Existing Implementations

| Inquisition | Parameters |
|-------------|------------|
| `UserCardItemsBySetInquisition` | `UserCardItemsBySetExtEntitys` |
| `UserCardItemsByNameInquisition` | `UserCardItemsByNameExtEntitys` |
| `CardsByArtistIdInquisition` | `CardsByArtistIdInquisitionArgs` |
| `CardNameTrigramSearchInquisition` | `CardNameTrigramSearchInquisitionArgs` |
| `AllUserSetCardsInquisition` | (non-parameterized) |

See: `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/` for full list.

## Related Patterns

- **Gopher**: Point-read operations — see `cosmos-gopher.md`
- **Scribe**: Write operations — see `cosmos-scribe.md`
