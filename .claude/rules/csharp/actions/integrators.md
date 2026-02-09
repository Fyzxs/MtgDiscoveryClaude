---
paths:
  - "csharp/src/**/Integrators/**"
---

# Integrator Pattern

## Purpose

Integrators **merge changes from a delta object into a state object**, returning a state object with the integrated changes. They implement the read-modify-write pattern for command operations.

## Base Interface

```csharp
public interface IIntegrator<TState, in TDelta>
{
    Task<TState> Integrate(TState current, TDelta change);
}
```

**Location**: `common/Lib.Shared.Abstractions/Actions/Integrators/IIntegrator.cs`

## Naming Convention

`{Domain}Integrator` — e.g., `UserCardIntegrator`, `UserSetCardIntegrator`

## Implementation Pattern

```csharp
// Interface
internal interface IUserCardIntegrator
    : IIntegrator<UserCardExtEntity, IAddUserCardXfrEntity>;

// Implementation
internal sealed class UserCardIntegrator : IUserCardIntegrator
{
    private readonly ICollectedItemsMergeMapper _mergeMapper;
    private readonly ICollectedItemsReplaceMapper _replaceMapper;
    private readonly IUserCardMetadataMapper _metadataMapper;

    public UserCardIntegrator()
        : this(new CollectedItemsMergeMapper(),
               new CollectedItemsReplaceMapper(),
               new UserCardMetadataMapper())
    { }

    private UserCardIntegrator(
        ICollectedItemsMergeMapper mergeMapper,
        ICollectedItemsReplaceMapper replaceMapper,
        IUserCardMetadataMapper metadataMapper)
    {
        _mergeMapper = mergeMapper;
        _replaceMapper = replaceMapper;
        _metadataMapper = metadataMapper;
    }

    public Task<UserCardExtEntity> Integrate(
        UserCardExtEntity current,
        IAddUserCardXfrEntity change)
    {
        ICollection<UserCardDetailsExtEntity> updatedCollectedList = change.ReplaceMode
            ? _replaceMapper.Map([.. current.CollectedList], change.Details)
            : _mergeMapper.Map([.. current.CollectedList], change.Details);

        UserCardExtEntity result = _metadataMapper.Map(current, change, updatedCollectedList);

        return Task.FromResult(result);
    }
}
```

**Key points:**
- Extend `IIntegrator<TState, TDelta>` in the interface
- `TState` is typically an `ExtEntity` (the persisted entity)
- `TDelta` is typically an `IXfrEntity` (the incoming change)
- Use mappers for complex merge logic
- Constructor chain pattern for dependencies

## Location in Adapters

`{Adapter}/Commands/Integrators/`

## Usage in Command Adapters

Integrators are used in read-modify-write command flows:

```csharp
// 1. Read current state
OpResponse<UserCardExtEntity> readResponse = await _gopher.ReadAsync<UserCardExtEntity>(readPoint, ct);

// 2. Resolve to concrete entity (creates new if not found)
UserCardExtEntity current = _resolver.Resolve(readResponse, input);

// 3. Integrate changes
UserCardExtEntity updated = await _integrator.Integrate(current, input);

// 4. Write back
return await _scribe.UpsertAsync(updated, ct);
```

## Existing Implementations

| Integrator | State | Delta |
|------------|-------|-------|
| `UserCardIntegrator` | `UserCardExtEntity` | `IAddUserCardXfrEntity` |
| `UserSetCardIntegrator` | `UserSetCardExtEntity` | `IAddCardToSetXfrEntity` |
| `AddSetGroupIntegrator` | `UserSetCardExtEntity` | `IAddSetGroupXfrEntity` |

See: `Lib.Adapter.UserCards/Commands/Integrators/`, `Lib.Adapter.UserSetCards/Commands/Integrators/`

## Related Patterns

- **Resolver**: Create new entities when reads return empty — see `resolvers.md`
- **Scribe**: Write the integrated result — see `../cosmos/cosmos-scribe.md`
- **Gopher**: Read current state — see `../cosmos/cosmos-gopher.md`
