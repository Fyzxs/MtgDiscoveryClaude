---
paths:
  - "csharp/src/**/Resolvers/**"
---

# Resolver Pattern

## Purpose

Resolvers **create or retrieve entities based on context**, typically implementing the Null Object pattern when database reads return empty. All Resolvers must derive from base interfaces in `Lib.Shared.Abstractions.Actions.Resolvers`.

## Base Interfaces

### IResolver (Generic Base)

```csharp
public interface IResolver<in TInput, out TResolved, in TContext>
{
    TResolved Resolve(TInput input, TContext context);
}
```

**Location**: `common/Lib.Shared.Abstractions/Actions/Resolvers/IResolver.cs`

### ICosmosResolver (Cosmos-Specific)

```csharp
public interface ICosmosResolver<TResolved, in TContext>
    : IResolver<OpResponse<TResolved>, TResolved, TContext>
{
    // Inherits: TResolved Resolve(OpResponse<TResolved> input, TContext context);
}
```

**Location**: `core/Lib.Cosmos/Resolvers/ICosmosResolver.cs`

**Prefer `ICosmosResolver`** for resolvers that handle Cosmos read responses.

## Naming Convention

`{Domain}Resolver` — e.g., `UserCardResolver`, `UserSetCardResolver`

## Standard Pattern (Cosmos Read Resolution)

Most resolvers handle Cosmos read responses and create Null Object entities when the read returns not-found:

```csharp
// Interface (use ICosmosResolver)
internal interface IUserCardResolver
    : ICosmosResolver<UserCardExtEntity, IAddUserCardXfrEntity>;

// Implementation
internal sealed class UserCardResolver : IUserCardResolver
{
    public UserCardExtEntity Resolve(
        OpResponse<UserCardExtEntity> input,
        IAddUserCardXfrEntity context)
    {
        if (input.IsSuccessful())
            return input.Value;

        // Create new entity from context (Null Object pattern)
        return new UserCardExtEntity
        {
            UserId = context.UserId,
            CardId = context.CardId,
            // ... populate from context
            CollectedList = []
        };
    }
}
```

**Key points:**
- Use `ICosmosResolver<TResolved, TContext>` for consistency
- `TResolved` is the entity type (`ExtEntity`)
- `TContext` is typically `IXfrEntity` (the incoming request with creation data)
- Return the existing entity if found, otherwise create a new one

## Specialized Pattern (Sub-Entity Resolution)

Some resolvers extract or resolve sub-data from an already-resolved entity:

```csharp
// Interface (use base IResolver with different type parameters)
internal interface IUserSetCardGroupResolver
    : IResolver<UserSetCardExtEntity, Dictionary<string, FinishGroupExtEntity>, IAddCardToSetXfrEntity>;

// Implementation
internal sealed class UserSetCardGroupResolver : IUserSetCardGroupResolver
{
    public Dictionary<string, FinishGroupExtEntity> Resolve(
        UserSetCardExtEntity input,  // Already resolved entity, not OpResponse
        IAddCardToSetXfrEntity context)
    {
        // Find or create the group within the entity
        var group = input.Groups.GetValueOrDefault(context.SetGroupId)
                    ?? new UserSetCardGroupExtEntity();

        return new Dictionary<string, FinishGroupExtEntity>
        {
            {"foil", group.Foil},
            {"nonfoil", group.NonFoil},
            {"etched", group.Etched}
        };
    }
}
```

**Use this pattern when:**
- Input is an already-resolved entity (not `OpResponse`)
- Output is derived/extracted data from the entity
- Resolution is about finding nested data, not handling missing entities

## Location in Adapters

`{Adapter}/Commands/Resolvers/`

## Usage in Command Adapters

```csharp
// 1. Attempt to read existing entity
OpResponse<UserCardExtEntity> readResponse = await _gopher.ReadAsync<UserCardExtEntity>(readPoint, ct);

// 2. Resolve: returns existing or creates new
UserCardExtEntity entity = _resolver.Resolve(readResponse, input);

// 3. Integrate changes and save
UserCardExtEntity updated = await _integrator.Integrate(entity, input);
return await _scribe.UpsertAsync(updated, ct);
```

## Existing Implementations

### Standard Resolvers (ICosmosResolver)

| Resolver | Resolved | Context |
|----------|----------|---------|
| `UserCardResolver` | `UserCardExtEntity` | `IAddUserCardXfrEntity` |
| `UserSetCardResolver` | `UserSetCardExtEntity` | `IAddCardToSetXfrEntity` |
| `AddSetGroupResolver` | `UserSetCardExtEntity` | `IAddSetGroupToUserSetCardXfrEntity` |
| `UserInfoResolver` | `UserInfoExtEntity` | `IUserInfoXfrEntity` |

### Specialized Resolvers (Base IResolver)

| Resolver | Input | Resolved | Context |
|----------|-------|----------|---------|
| `UserSetCardGroupResolver` | `UserSetCardExtEntity` | `Dictionary<string, FinishGroupExtEntity>` | `IAddCardToSetXfrEntity` |

See: `Lib.Adapter.UserCards/Commands/Resolvers/`, `Lib.Adapter.UserSetCards/Commands/Resolvers/`

## Related Patterns

- **Integrator**: Merge changes into resolved entity — see `integrators.md`
- **Gopher**: Provides the read response to resolve — see `../cosmos/cosmos-gopher.md`
