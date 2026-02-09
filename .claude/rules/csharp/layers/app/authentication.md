---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Authentication/**"
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/**"
---

# GraphQL Authentication Pattern

## Purpose

The authentication pattern extracts user identity from JWT tokens and makes it available as a typed entity for Entry layer consumption. This bridges the HotChocolate authorization system to the application's entity model.

## Auth Flow

```
JWT Token (Auth0)
    ↓ [HotChocolate validates]
ClaimsPrincipal
    ↓ [AuthUserArgEntity extracts]
IAuthUserArgEntity
    ↓ [ArgsMapper combines with input]
ICombinedArgsEntity { AuthUser + OperationArgs }
    ↓ [Entry layer validates and processes]
```

## AuthUserArgEntity

Extracts user information from the JWT `ClaimsPrincipal` on demand:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Authentication/AuthUserArgEntity.cs`

**Key points:**
- `UserId` is a **deterministic GUID** derived from the Auth0 subject claim
- Properties are lazily evaluated from `ClaimsPrincipal`
- Custom claim namespaces (`https://mtg-discovery-api/`) are checked first, then standard claims
- Throws `InvalidOperationException` if required claims are missing

## Mutation Authorization Pattern

All mutations that require user identity follow this pattern:

### 1. Mark with `[Authorize]`

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/` (mutation classes with `[Authorize]` decorator)

### 2. Combine Auth + Input via Mapper

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/` (mutation method implementation pattern)

### 3. ArgsMapper Implementation

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/AddCardToCollectionArgsMapper.cs`

**Key points:**
- Uses `ICreateMapper<TSource1, TSource2, TResult>` (dual-source mapper)
- Creates `AuthUserArgEntity` from `ClaimsPrincipal` inside the mapper
- Returns combined args entity wrapping both auth and input

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Auth entity | `AuthUserArgEntity` | (singleton) |
| Args mapper interface | `I{Operation}ArgsMapper` | `IAddCardToCollectionArgsMapper` |
| Args mapper implementation | `{Operation}ArgsMapper` | `AddCardToCollectionArgsMapper` |
| Combined args interface | `I{Operation}ArgsEntity` | `IAddCardToCollectionArgsEntity` |
| Combined args implementation | `{Operation}ArgsEntity` | `AddCardToCollectionArgsEntity` |

## Optional Auth for Queries

Some queries accept an optional `UserId` to enrich results with user-specific data (e.g., collection status on cards). These queries do NOT require `[Authorize]`:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Queries/CardQueryMethods.cs`

The ArgEntity includes an optional `UserId` field:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Entities/Args/CardIdsArgEntity.cs` (if exists, otherwise reference `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Apis/ICardIdsArgEntity.cs`)

When `UserId` is provided, the Entry layer enriches the response with user collection data. When empty, the query still succeeds but without personalized enrichment.

## ArgsMapper Naming (App Layer)

ArgsMappers in the App layer combine `ClaimsPrincipal` + input into a single combined args entity. They live in `Actions/Mappers/` or `Actions/Mappers/{Domain}/`.

| Type | Pattern | Example |
|------|---------|---------|
| Interface | `I{Operation}ArgsMapper` | `IAddCardToCollectionArgsMapper` |
| Implementation | `{Operation}ArgsMapper` | `AddCardToCollectionArgsMapper` |
| Base interface | `ICreateMapper<ClaimsPrincipal, TArgEntity, TResult>` | dual-source mapper |

These are distinct from Entry/Domain/Aggregator mappers -- they exist only to bridge `ClaimsPrincipal` into the typed entity model.

### Scalar Parameter ArgsMappers

When the GraphQL method accepts a primitive parameter (e.g., `string collectionId`) instead of an `ArgEntity`, the ArgsMapper uses the scalar type directly:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/` (ArgsMapper implementations for scalar parameters)

**When to use scalar parameters:**
- The operation needs only one simple input (e.g., an ID) plus the authenticated user
- Creating an `ArgEntity` class for a single field would be unnecessary indirection

The mapper still constructs `AuthUserArgEntity` internally and produces a combined args entity — the only difference is the second parameter type.

### Multiple ArgsMappers Per Class

Mutation and query classes that handle many operations for a single domain will have one ArgsMapper per operation:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Mutations/` (mutation classes showing multiple ArgsMapper fields)

All are constructed in the public constructor chain and injected via the private constructor. When a class reaches 7+ ArgsMappers, this is expected — it reflects the domain's command surface, not a violation of single responsibility. The class itself remains a thin translation layer with no logic.

## Key Rules

1. **`[Authorize]` on all mutations** that access user identity
2. **Never access ClaimsPrincipal directly in Entry layer** — always via `IAuthUserArgEntity`
3. **ArgsMapper handles the ClaimsPrincipal → AuthUserArgEntity bridge** — mutation methods never construct `AuthUserArgEntity` directly
4. **Entry layer validates auth-to-input consistency** — e.g., `AuthUserMatchesUserIdValidator` checks the JWT user matches the requested user
5. Queries that optionally use user context accept `IUserIdArgEntity` (may have empty UserId)
6. **Queries do NOT use `[Authorize]`** — optional user context is passed via `UserId` field on the ArgEntity

## File Location

| Type | Location |
|------|----------|
| `AuthUserArgEntity` | `Authentication/AuthUserArgEntity.cs` |
| Args mappers | `Actions/Mappers/` or `Actions/Mappers/{Domain}/` |
| Combined args entities | `Lib.MtgDiscovery.Entry/Entities/` |

## Reference Implementations

- **Auth entity**: `Authentication/AuthUserArgEntity.cs`
- **Mutation with auth**: `Mutations/UserCardsMutationMethods.cs`
- **Args mapper**: `Actions/Mappers/AddCardToCollectionArgsMapper.cs`
- **Auth validator**: `Lib.MtgDiscovery.Entry/Commands/Actions/Validators/AuthUserMatchesUserIdValidator.cs`
