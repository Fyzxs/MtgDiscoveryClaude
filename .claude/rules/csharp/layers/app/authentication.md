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

```csharp
internal sealed class AuthUserArgEntity : IAuthUserArgEntity
{
    private static readonly Guid s_userSubjectNamespace =
        new("4d746755-7365-7253-7562-6a6563744775");

    private readonly ClaimsPrincipal _claimsPrincipal;

    public AuthUserArgEntity(ClaimsPrincipal claimsPrincipal)
        => _claimsPrincipal = claimsPrincipal;

    public string UserId
    {
        get
        {
            Guid guid = GuidUtility.Create(s_userSubjectNamespace, SourceId);
            return guid.ToString();
        }
    }

    public string SourceId =>
        _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _claimsPrincipal.FindFirst("sub")?.Value
        ?? throw new InvalidOperationException("No subject claim found in token");

    public string DisplayName =>
        _claimsPrincipal.FindFirst("https://mtg-discovery-api/nickname")?.Value
        ?? _claimsPrincipal.FindFirst("nickname")?.Value
        ?? _claimsPrincipal.FindFirst("name")?.Value
        ?? throw new InvalidOperationException("No display name claim found in token");

    public string Email =>
        _claimsPrincipal.FindFirst("https://mtg-discovery-api/email")?.Value
        ?? _claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value
        ?? _claimsPrincipal.FindFirst("email")?.Value
        ?? throw new InvalidOperationException("No email claim found in token");
}
```

**Key points:**
- `UserId` is a **deterministic GUID** derived from the Auth0 subject claim
- Properties are lazily evaluated from `ClaimsPrincipal`
- Custom claim namespaces (`https://mtg-discovery-api/`) are checked first, then standard claims
- Throws `InvalidOperationException` if required claims are missing

## Mutation Authorization Pattern

All mutations that require user identity follow this pattern:

### 1. Mark with `[Authorize]`

```csharp
[Authorize]
[GraphQLType(typeof(AddCardToCollectionResponseModelUnionType))]
public async Task<ResponseModel> AddCardToCollectionAsync(
    ClaimsPrincipal claimsPrincipal,
    AddUserCardArgEntity args,
    CancellationToken cancellationToken)
```

### 2. Combine Auth + Input via Mapper

```csharp
{
    IAddCardToCollectionArgsEntity combinedArgs =
        await _argsMapper.Map(claimsPrincipal, args).ConfigureAwait(false);
    IOperationResponse<List<CardItemOutEntity>> response =
        await _entryService.AddCardToCollectionAsync(combinedArgs, cancellationToken)
            .ConfigureAwait(false);
    return await _responseMapper.Map(response).ConfigureAwait(false);
}
```

### 3. ArgsMapper Implementation

```csharp
internal interface IAddCardToCollectionArgsMapper
    : ICreateMapper<ClaimsPrincipal, AddUserCardArgEntity, IAddCardToCollectionArgsEntity>;

internal sealed class AddCardToCollectionArgsMapper : IAddCardToCollectionArgsMapper
{
    public Task<IAddCardToCollectionArgsEntity> Map(
        ClaimsPrincipal claimsPrincipal, AddUserCardArgEntity args)
    {
        IAddCardToCollectionArgsEntity result = new AddCardToCollectionArgsEntity
        {
            AuthUser = new AuthUserArgEntity(claimsPrincipal),
            AddUserCard = args
        };
        return Task.FromResult(result);
    }
}
```

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

```csharp
// No [Authorize] decorator — userId is optional
[GraphQLType(typeof(CardResponseModelUnionType))]
public async Task<ResponseModel> CardsById(
    CardIdsArgEntity ids, CancellationToken cancellationToken)
```

The ArgEntity includes an optional `UserId` field:

```csharp
internal sealed class CardIdsArgEntity : ICardIdsArgEntity
{
    public ICollection<string> CardIds { get; set; }
    public string UserId { get; set; }  // Optional — empty string if anonymous
}
```

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

```csharp
// Interface — second generic parameter is string, not an ArgEntity
internal interface ICollectionIdArgsMapper
    : ICreateMapper<ClaimsPrincipal, string, ICollectionIdArgEntity>;

// Implementation
internal sealed class CollectionIdArgsMapper : ICollectionIdArgsMapper
{
    public Task<ICollectionIdArgEntity> Map(ClaimsPrincipal claimsPrincipal, string collectionId)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        ICollectionIdArgEntity entity = new CollectionIdArgEntity
        {
            UserId = authUser.UserId,
            CollectionId = collectionId
        };

        return Task.FromResult(entity);
    }
}
```

**When to use scalar parameters:**
- The operation needs only one simple input (e.g., an ID) plus the authenticated user
- Creating an `ArgEntity` class for a single field would be unnecessary indirection

The mapper still constructs `AuthUserArgEntity` internally and produces a combined args entity — the only difference is the second parameter type.

### Multiple ArgsMappers Per Class

Mutation and query classes that handle many operations for a single domain will have one ArgsMapper per operation:

```csharp
private readonly ICreateCollectionArgsMapper _createCollectionArgsMapper;
private readonly IRenameCollectionArgsMapper _renameCollectionArgsMapper;
private readonly IDeleteCollectionArgsMapper _deleteCollectionArgsMapper;
// ... one per mutation operation
```

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
