# Cosmos ReadPointItem Pattern

## Purpose

`ReadPointItem` and related types provide **type-safe carriers** for Cosmos DB point-read operations. They encapsulate the document ID and partition key required for efficient point reads.

## Base Types

- **Abstract base**: `PointItem`
- **Read operations**: `ReadPointItem : PointItem`
- **Delete operations**: `DeletePointItem : PointItem`
- **Location**: `core/Lib.Cosmos/Apis/Operators/`

## PointItem Structure

```csharp
public abstract class PointItem
{
    public CosmosItemId Id { get; set; }
    public PartitionKeyValue Partition { get; set; }
}

public sealed class ReadPointItem : PointItem;
public sealed class DeletePointItem : PointItem;
```

## Related ID Types

| Type | Purpose | Location |
|------|---------|----------|
| `CosmosItemId` | Document ID wrapper | `core/Lib.Cosmos/Apis/Ids/` |
| `PartitionKeyValue` | Partition key wrapper | `core/Lib.Cosmos/Apis/Ids/` |

## Usage Pattern

Mappers convert from transfer entities to `ReadPointItem`:

```csharp
// Mapper interface
internal interface IAddUserCardXfrToReadPointMapper
    : ICreateMapper<IAddUserCardXfrEntity, ReadPointItem>;

// Mapper implementation
internal sealed class AddUserCardXfrToReadPointMapper : IAddUserCardXfrToReadPointMapper
{
    public ReadPointItem Create(IAddUserCardXfrEntity source)
    {
        return new ReadPointItem
        {
            Id = new CosmosItemId(source.CardId),
            Partition = new PartitionKeyValue(source.UserId)
        };
    }
}
```

## Built-in Mappers

For simple string-based IDs, use the built-in mappers:

| Mapper | Input | Output |
|--------|-------|--------|
| `IStringToReadPointItemMapper` | `string` | `ReadPointItem` |
| `IStringCollectionToReadPointItemMapper` | `IEnumerable<string>` | `IEnumerable<ReadPointItem>` |

## Usage in Adapters

```csharp
// In a command adapter
private readonly IAddUserCardXfrToReadPointMapper _toReadPointMapper;
private readonly ICosmosGopher _gopher;

public async Task<OpResponse<UserCardExtEntity>> Execute(
    IAddUserCardXfrEntity input,
    CancellationToken ct)
{
    ReadPointItem readPoint = _toReadPointMapper.Create(input);
    return await _gopher.ReadAsync<UserCardExtEntity>(readPoint, ct)
        .ConfigureAwait(false);
}
```

## When to Create Custom Mappers

Create a custom `XfrToReadPointMapper` when:
- The ID comes from a complex transfer entity
- Multiple fields combine to form the document ID or partition key
- Domain-specific validation is needed during mapping

Use built-in mappers when:
- The ID is a simple string
- No transformation is needed

## Related Patterns

- **Gopher**: Uses `ReadPointItem` for reads — see `cosmos-gopher.md`
- **Mappers**: Base mapper interfaces — see `../actions/mappers.md`
