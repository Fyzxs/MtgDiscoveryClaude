---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/Args/**"
---

# InputType Descriptor Pattern

## Purpose

InputType descriptors map C# `ArgEntity` classes to GraphQL input types using HotChocolate's `InputObjectType<T>`. Every `ArgEntity` that appears as a query/mutation parameter needs a matching InputType.

## Implementation Pattern

```csharp
internal sealed class CardIdsArgEntityInputType : InputObjectType<CardIdsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<CardIdsArgEntity> descriptor)
    {
        _ = descriptor.Name("CardIdsInput")
            .Description("Input for querying cards by card IDs");

        _ = descriptor.Field(x => x.CardIds)
            .Name("cardIds")
            .Type<NonNullType<ListType<NonNullType<StringType>>>>()
            .Description("The collection of card IDs to query");
        _ = descriptor.Field(x => x.UserId)
            .Name("userId")
            .Type<StringType>()
            .Description("Optional user identifier to enrich cards with collection data");
    }
}
```

## Structure

Every InputType follows this structure:

1. **Type metadata** -- `.Name()` and `.Description()` on the descriptor
2. **Field definitions** -- one `.Field()` call per property, each with `.Name()`, `.Type<>()`, `.Description()`
3. **Discard return values** -- use `_ =` since the fluent API returns are not needed

## Naming Conventions

| Element | Pattern | Example |
|---------|---------|---------|
| Class name | `{Domain}{Concept}ArgEntityInputType` | `CardIdsArgEntityInputType` |
| GraphQL name | `"{Concept}Input"` | `"CardIdsInput"` |
| Field names | camelCase | `"cardIds"`, `"userId"` |

## HotChocolate Type Mappings

| C# Type | GraphQL Type | HotChocolate |
|---------|-------------|--------------|
| `string` | `String` | `StringType` |
| `string` (required) | `String!` | `NonNullType<StringType>` |
| `int` | `Int` | `IntType` |
| `bool` | `Boolean` | `BooleanType` |
| `ICollection<string>` | `[String!]!` | `NonNullType<ListType<NonNullType<StringType>>>` |
| Nested input | Custom | `NonNullType<FinishCountsInputType>` |

## Nested Input Types

When an ArgEntity contains another ArgEntity, reference the nested InputType directly:

```csharp
// Parent InputType references child InputType
_ = descriptor.Field(x => x.Counts)
    .Name("counts")
    .Type<NonNullType<FinishCountsInputType>>()
    .Description("The card counts by finish type");
```

The nested InputType follows the same pattern:

```csharp
internal sealed class FinishCountsInputType : InputObjectType<FinishCountsArgEntity>
{
    protected override void Configure([NotNull] IInputObjectTypeDescriptor<FinishCountsArgEntity> descriptor)
    {
        _ = descriptor.Name("FinishCountsInput")
            .Description("Card finish counts by type");

        _ = descriptor.Field(x => x.Total)
            .Name("total")
            .Type<NonNullType<IntType>>()
            .Description("Total count across all finishes");
    }
}
```

## File Location

InputType descriptors mirror the `Entities/Args/` folder structure:

```
Entities/Args/CardIdsArgEntity.cs
    ↕ paired with
Entities/Types/Args/CardIdsArgEntityInputType.cs

Entities/Args/UserCards/AddUserCardArgEntity.cs
    ↕ paired with
Entities/Types/Args/UserCards/AddCardToCollectionArgEntityInputType.cs
```

## Registration

All InputTypes must be registered in the appropriate schema extension:

```csharp
// In ApiQueryExtensions.cs
.AddType<CardIdsArgEntityInputType>()

// In ApiMutationExtensions.cs
.AddType<AddCardToCollectionArgEntityInputType>()
```

See: `layers/app/schema-extensions.md` for full registration pattern.

## Key Rules

1. **Every descriptor MUST have `Name` set** -- prevents schema naming conflicts
2. **Every descriptor MUST have `Description` set** -- provides API documentation
3. **Field names use camelCase** -- GraphQL convention
4. **Required fields use `NonNullType<>`** -- maps to `!` in GraphQL schema
5. **`internal sealed`** -- InputTypes are never public or abstract

## Reference Files

- **Simple InputType**: `Entities/Types/Args/AllSetsArgEntityInputType.cs`
- **List InputType**: `Entities/Types/Args/CardIdsArgEntityInputType.cs`
- **Nested InputType**: `Entities/Types/Args/UserSetCards/AddSetGroupToUserSetCardArgEntityInputType.cs`
- **Registration**: `Schemas/ApiQueryExtensions.cs`, `Schemas/ApiMutationExtensions.cs`
