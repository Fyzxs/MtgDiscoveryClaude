---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Entities/**"
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Entities/**"
  - "csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Queries/Entities/**"
---

# Entry Layer Entities

## Entity Types in the Entry Layer

The Entry layer works with three entity types:

| Entity Type | Direction | Purpose |
|-------------|-----------|---------|
| **ArgEntity** | In (from App) | GraphQL input, validated by Entry |
| **ItrEntity** | Internal (Entry → Domain) | Validated, mapped internal representation |
| **OutEntity** | Out (to App) | Response data for GraphQL layer |

The Entry layer also receives **OufEntities** back from the Domain/Aggregator and maps them to OutEntities.

## ArgEntity Interfaces

Arg entity interfaces define the contract between the App layer and Entry layer. They live in one of three locations, depending on scope:

| Location | When to Use | Example |
|----------|-------------|---------|
| `Apis/` | Shared across domains or consumed directly by App-layer query/mutation methods | `ICardIdsArgEntity`, `ISetCodesArgEntity` |
| `Lib.Shared.DataModels/Entities/Args/` | Used across multiple Entry projects or shared with Domain/Aggregator layers | `ICollectionIdArgEntity`, `IAuthUserArgEntity` |
| `Commands/{Domain}/Entities/` or `Queries/{Domain}/Entities/` | Domain-specific and only used by a single operation service | `AddSetGroupToUserSetCardArgEntity` |

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/ICardIdsArgEntity.cs`

ArgEntity interfaces are `public` — they are consumed by the App layer.

## Combined ArgsEntity

For authenticated mutations, a combined ArgsEntity wraps both auth user data and operation input:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Entities/IAddCardToCollectionArgsEntity.cs`

Combined ArgsEntities and their concrete implementations live at:
- `Entities/` root — for general-purpose combined entities
- `Entities/{Domain}/` — for domain-specific combined entities

## ItrEntity

ItrEntities are the internal representation after validation and mapping. They live in:

- `Commands/Entities/` — command-specific ItrEntities
- `Queries/Entities/` — query-specific ItrEntities
- `Entities/Itrs/{Domain}/` — domain-organized ItrEntities
- `Commands/{Domain}/Entities/` — domain-specific command ItrEntities

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Entities/UserCardCollectionItrEntity.cs`

### Property Rules

- All properties use `{ get; init; }`
- No `required` keyword
- No default values

## OutEntity

OutEntities are the response data sent back to the App/GraphQL layer. ALL OutEntities live in `Entities/Outs/` organized by domain:

```
Entities/Outs/
├── Artists/
│   ├── ArtistSearchResultOutEntity.cs
│   └── IArtistSearchResultOutEntity.cs
├── Cards/
│   ├── CardItemOutEntity.cs
│   └── ICardItemOutEntity.cs
├── Collections/
│   ├── CollectionOutEntity.cs
│   └── ICollectionOutEntity.cs
└── ...
```

OutEntities MUST NOT be placed inside `Commands/` or `Queries/` subfolders.

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Entities/Outs/Cards/CardItemOutEntity.cs`

### OutEntity Property Rules

- Most properties use `{ get; init; }`
- Properties that are enriched post-construction may use `{ get; set; }` (e.g., `UserCollection`)
- OutEntities are `public` — they are consumed by the App layer

## Entity Flow

```
ArgEntity (from App)
    ↓ [Validator validates]
    ↓ [ArgToItr Mapper transforms]
ItrEntity
    ↓ [Domain service processes]
OufEntity (from Domain/Aggregator)
    ↓ [OufToOut Mapper transforms]
OutEntity (to App)
    ↓ [Optional: Enrichment adds user data]
OutEntity (enriched, to App)
```

## Reference Files

- **ArgEntity interface**: `Apis/ICardIdsArgEntity.cs`
- **Combined ArgsEntity**: `Entities/IAddCardToCollectionArgsEntity.cs`
- **ItrEntity**: `Commands/Entities/UserCardCollectionItrEntity.cs`
- **OutEntity**: `Entities/Outs/Cards/CardItemOutEntity.cs`
