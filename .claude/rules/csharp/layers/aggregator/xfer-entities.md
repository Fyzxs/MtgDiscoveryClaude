---
paths:
  - "csharp/src/Lib.Aggregators/**/Entities/*"
---

# Aggregator Entities

## XfrEntity (Transfer Entity)

The internal implementation of a `XfrEntity` transfers information from the `ItrEntity` to the Adapter layer. Each XfrEntity has an associated `ItrToXfr` mapper that populates it from an `ItrEntity`.

### Location

- Command XfrEntities: `Commands/Entities/`
- Query XfrEntities: `Queries/Entities/`

### CacheKey Requirement

Every XfrEntity must include a computed `CacheKey` string property. This enables caching at the adapter layer.

```csharp
internal sealed class ArtistSearchTermXfrEntity : IArtistSearchTermXfrEntity
{
    public ICollection<string> SearchTerms { get; init; }
    public string Normalized { get; init; }
    public string CacheKey => $"artist:search:{Normalized}";
}
```

### Property Rules

- All properties use `{ get; init; }`
- No `required` keyword
- No default values (no `= []`, no `= ""`, etc.)

## OufEntity (Output Entity)

The `OufEntity` is the output from the aggregator back to the Domain layer. It is produced by mapping an `ExtEntity` (adapter response) through an `ExtToOuf` mapper.

### Location

- Shared OufEntities: `Entities/` at project root (used by both commands and queries)

### Property Rules

Same as XfrEntity:
- All properties use `{ get; init; }`
- No `required` keyword
- No default values

See: `.claude/rules/csharp/entities.md` for base entity rules.
