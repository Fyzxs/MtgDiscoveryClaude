---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/Args/**"
---

# InputType Descriptor Pattern

## Purpose

InputType descriptors map C# `ArgEntity` classes to GraphQL input types using HotChocolate's `InputObjectType<T>`. Every `ArgEntity` that appears as a query/mutation parameter needs a matching InputType.

## Implementation Pattern

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/Args/CardIdsArgEntityInputType.cs`

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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/Args/` (InputType files showing nested type references)

The nested InputType follows the same pattern:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Types/Args/` (nested InputType implementations)

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

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/ApiQueryExtensions.cs` and `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Schemas/ApiMutationExtensions.cs`

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
