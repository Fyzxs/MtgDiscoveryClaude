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

XfrEntities MUST live in `Commands/Entities/` or `Queries/Entities/` — never at the project root `Entities/`.

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

ALL OufEntities MUST live at the project root `Entities/` folder. OufEntities MUST NOT be placed inside `Queries/Entities/` or `Commands/Entities/` — those folders are for XfrEntities only.

### Collection Return Types

When returning collections of OufEntities, always use a **typed wrapper OufEntity** rather than raw `IEnumerable<>`:

```csharp
// Canonical — typed wrapper OufEntity
IOperationResponse<ICardItemCollectionOufEntity>

// NOT canonical — raw IEnumerable is tech debt
IOperationResponse<IEnumerable<ICardItemOufEntity>>
```

The wrapper OufEntity (e.g., `ICardItemCollectionOufEntity`) encapsulates the collection and lives at root `Entities/` alongside other OufEntities. Using `List<>` as a return type (e.g., `IOperationResponse<List<IOufEntity>>`) is also non-canonical.

### Property Rules

Same as XfrEntity:
- All properties use `{ get; init; }`
- No `required` keyword
- No default values

## ItrEntity Exclusion Rule

Aggregator projects MUST NOT define concrete `ItrEntity` classes. ItrEntities are defined in the Domain or Shared layer and flow into the aggregator via interfaces (`IItrEntity`). The aggregator receives them — it does not create them.

If an aggregator needs to pass data internally, use a XfrEntity instead.

See: `.claude/rules/csharp/entities.md` for base entity rules.
