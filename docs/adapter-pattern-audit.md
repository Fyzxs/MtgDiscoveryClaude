# Comprehensive Lib.Adapter.* Pattern Compliance Audit

**Generated**: 2026-02-05
**Branch**: pattern-check
**Scope**: All `Lib.Adapter.*` projects in `/csharp/src/Lib.Adapters/`

## Executive Summary

After reviewing 440+ source files across 11 adapter projects and 5 test projects, I've identified significant deviations from documented patterns, undocumented patterns that exist in code, and structural inconsistencies that would undermine this codebase's role as a canonical reference implementation.

---

## Part 1: Deviations from Documented Patterns

### 1.1 Missing Resolver/Integrator Patterns (Critical)

**Documentation states**: Commands should follow read-modify-write using Mapper → Resolver → Integrator → Scribe.

**Actual deviations**:

| Project | Has Resolver | Has Integrator | Status |
|---------|-------------|----------------|--------|
| `Lib.Adapter.UserCards` | ✅ | ✅ | **Compliant** |
| `Lib.Adapter.UserSetCards` | ✅ (3 resolvers) | ✅ (2 integrators) | **Compliant** |
| `Lib.Adapter.User` | ✅ | ❌ | Partial |
| `Lib.Adapter.Collections` | ❌ | ❌ | **Non-compliant** |
| `Lib.Adapter.UserWishlistCards` | ❌ | ❌ | **Non-compliant** |
| `Lib.Adapter.UserSealedProducts` | ❌ | ❌ | **Non-compliant** |

---

### 1.2 CollectionCommandAdapter.cs — Major Pattern Violations

**File**: `Lib.Adapter.Collections/Commands/CollectionCommandAdapter.cs`

**Issues** (416 lines of violations):

1. **No Mappers**: Inline `ReadPointItem` creation on lines 71-75, 117-120, 165-167, 225-229, 280-284, 325-329, 361-365
   ```csharp
   // Current (lines 71-75)
   ReadPointItem readItem = new()
   {
       Id = new ProvidedCosmosItemId(entity.CollectionId),
       Partition = new ProvidedPartitionKeyValue(entity.OwnerId)
   };
   ```
   **Expected**: Use `ICollectionXfrToReadPointMapper`

2. **No Resolvers**: Manual null checks with early returns instead of Null Object pattern

3. **No Integrators**: Inline entity construction on lines 89-100, 135-146, 197-208, 252-263, etc.

4. **Inline DateTime.UtcNow**: Lines 99, 145, 193, 207, 262, 352, 387
   ```csharp
   UpdatedAt = DateTime.UtcNow.ToString("o")
   ```
   **Expected**: Timestamps should be provided by context/mapper

5. **Inline Mapping Method**: `MapAuthorizedUsersToExt` (lines 403-415) should be separate mapper class

6. **Business Logic in Adapter**: Lines 298-301 (default collection check), 343-347 (same)
   ```csharp
   if (existing.IsDefault)
   {
       return new FailureOperationResponse<CollectionExtEntity>(
           new CollectionAdapterException("Cannot delete the default collection"));
   }
   ```

---

### 1.3 AddUserWishlistCardAdapter.cs — Pattern Violations

**File**: `Lib.Adapter.UserWishlistCards/Commands/AddUserWishlistCardAdapter.cs`

**Issues**:

1. **Inline ReadPointItem creation** (lines 37-41)

2. **No Resolver**: Manual `if/else` for existing vs new (lines 46-54)

3. **No Integrator**: Inline `MergeWishlistItems` method (lines 67-119) duplicates logic from `UserCardIntegrator`

4. **Inline DateTime.UtcNow** (line 117)

---

### 1.4 AddUserSealedProductAdapter.cs — Pattern Violations

**File**: `Lib.Adapter.UserSealedProducts/Commands/AddUserSealedProductAdapter.cs`

**Issues**:

1. **Inline ReadPointItem creation** (lines 68-72, 83-87)

2. **No Resolver/Integrator**: Manual entity construction (lines 99-118, 122-141)

3. **Inline DateTime.UtcNow** (lines 117, 140)

4. **Multiple responsibilities**: Delete logic mixed with upsert logic

---

### 1.5 UserCardIntegrator.cs — Documentation Mismatch

**Documentation states** (`integrators.md`):
> Use mappers for complex merge logic

**Actual** (`UserCardIntegrator.cs` lines 37-91):
- Inline `MergeCollectedItem` method
- Inline `ReplaceCollectedItem` method
- No mappers used

The documented example shows using mappers (`ICollectedItemsMergeMapper`, `ICollectedItemsReplaceMapper`), but actual implementation has inline logic.

---

### 1.6 Mapper Naming Convention Deviations

**Documentation** (`mappers.md`):
> `{SourceType}{SourceEntity}To{DestinationType}{DestinationEntity}Mapper`

**Deviations found**:

| File | Actual Name | Expected Name |
|------|-------------|---------------|
| `UserSealedProductReadPointMapper.cs` | Takes primitives `(string, string)` | Should take `IXfrEntity` |
| `AllUserSetCardsXfrToArgsMapper.cs` | Interface + class in same file | Should be separate files |
| `ICollectionCardIdToReadPointItemMapper.cs` | Takes `IEnumerable<string>` | Should take typed entity |

---

### 1.7 XfrEntity Concrete Class in Apis/Entities

**Documentation** (`adapter-apis-entities.md`):
> These must ONLY be interfaces.

**Violation**: `Lib.Adapter.SealedProducts/Apis/Entities/SealedProductsBySetCodeXfrEntity.cs`
```csharp
internal sealed class SealedProductsBySetCodeXfrEntity : ISealedProductsBySetCodeXfrEntity
```

Concrete class in `Apis/Entities/` instead of `Queries/Entities/`.

---

### 1.8 Mappers in Wrong Location

**Documentation** (`adapter-folder-structure.md`):
> Mappers belong in `{Adapter}/Commands/Mappers/` or `{Adapter}/Queries/Mappers/`

**Violation**: `Lib.Adapter.Scryfall.Cosmos/Apis/Mappers/UserSealedProducts/`

Mapper is in `Lib.Adapter.Scryfall.Cosmos` (infrastructure) instead of domain adapter.

---

## Part 2: Undocumented Patterns Present in Code

### 2.1 Janitor Pattern (Delete Operations)

**Present in code but NOT documented**:

- `CollectionJanitor.cs`
- `UserSealedProductsJanitor.cs`
- `UserWishlistCardsJanitor.cs`

**Interface**: `ICosmosContainerDeleteOperator`

**Should have documentation**: Similar to `cosmos-gopher.md` and `cosmos-scribe.md`

```csharp
public sealed class CollectionJanitor : ICosmosContainerDeleteOperator
{
    public async Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item, CancellationToken ct)
}
```

---

### 2.2 Inquisitor Pattern (Query Executors)

**Present in code but NOT documented**:

- 12+ Inquisitor implementations in `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/`

**Pattern**: Inquisitors are the query execution layer that Inquisitions delegate to.

```csharp
public sealed class UserCardsInquisitor : CosmosInquisitor
{
    public UserCardsInquisitor(ILogger logger)
        : base(new UserCardsCosmosContainer(logger))
    { }
}
```

**Documentation gap**: `cosmos-inquisition.md` mentions `ICosmosInquisitor` but doesn't document the Inquisitor pattern fully.

---

### 2.3 CosmosContainer Pattern

**Present in code but NOT documented**:

- 20+ container implementations in `Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/`
- Container definitions in `Cosmos/Containers/Definitions/`
- Container name primitives in `Cosmos/Primitives/`

**Should document**: Container creation, naming conventions, definition pattern.

---

### 2.4 InquiryDefinition Pattern

**Present in code but NOT documented**:

- Used by all Inquisitions for SQL query definitions
- Example: `UserCardItemsBySetQueryDefinition.cs`

---

### 2.5 ICosmosJanitor Interface

**Used in** `AddUserSealedProductAdapter.cs` line 25:
```csharp
private readonly ICosmosJanitor _userSealedProductsJanitor;
```

**Not documented** in any markdown file.

---

## Part 3: Structural Inconsistencies

### 3.1 Interface and Implementation in Same File

**File**: `Lib.Adapter.UserSetCards/Queries/Mappers/AllUserSetCardsXfrToArgsMapper.cs`

Contains both:
```csharp
internal interface IAllUserSetCardsXfrToArgsMapper : ICreateMapper<...>;
internal sealed class AllUserSetCardsXfrToArgsMapper : IAllUserSetCardsXfrToArgsMapper
```

**Expected**: Separate files per documented conventions.

---

### 3.2 Inconsistent Folder Structure

**Lib.Adapter.SealedProducts** has `Apis/Queries/` subfolder (non-standard):
```
Lib.Adapter.SealedProducts/
├── Apis/
│   └── Queries/   ← Non-standard location
│       └── SealedProductsBySetCodeAdapter.cs
└── Queries/
    └── SealedProductsQueryAdapter.cs
```

**Expected**: Query adapters in `Queries/` only.

---

### 3.3 Missing Mappers Subfolder

**Lib.Adapter.Collections** has no `Mappers/` folder despite multiple mapping operations inline.

---

### 3.4 Strategies Only in One Project

`Commands/Strategies/` folder only exists in `Lib.Adapter.UserCards`.

No retry strategies in projects with complex operations:
- `Lib.Adapter.Collections`
- `Lib.Adapter.UserSetCards`
- `Lib.Adapter.UserSealedProducts`

---

## Part 4: Documentation Accuracy Issues

### 4.1 Incorrect Example in integrators.md

**Documentation shows**:
```csharp
private readonly ICollectedItemsMergeMapper _mergeMapper;
private readonly ICollectedItemsReplaceMapper _replaceMapper;
```

**Actual code**: These mappers don't exist. `UserCardIntegrator` has inline methods.

---

### 4.2 Missing IXfrEntity Marker Interface

**Documentation** (`adapter-apis-entities.md`):
> inherit the marker interface `IXfrEntity`

**Actual**: No XfrEntity interfaces inherit from `IXfrEntity`:
```csharp
public interface ISealedProductsBySetCodeXfrEntity  // No : IXfrEntity
{
    string SetCode { get; }
}
```

---

### 4.3 Inconsistent Method Naming in ICreateMapper

**Documentation** (`mappers.md`) shows method named `Map`:
```csharp
Task<TResult> Map(TSource source);
```

**Some implementations use `Create`** (based on doc examples):
```csharp
public ReadPointItem Create(IAddUserCardXfrEntity source)  // Wrong
```

**Actual interface uses `Map`** — documentation example is wrong where it shows `Create`.

---

## Part 5: Summary of Required Actions

### Critical (Pattern Violations)

1. **Lib.Adapter.Collections**: Extract mappers, create resolvers, create integrators
2. **Lib.Adapter.UserWishlistCards**: Add resolver, add integrator, extract mapper
3. **Lib.Adapter.UserSealedProducts**: Add resolver, add integrator, extract mapper
4. **All inline DateTime.UtcNow**: Centralize timestamp generation

### Documentation Updates Needed

1. Create `cosmos-janitor.md` documenting delete operations
2. Create `cosmos-inquisitor.md` documenting query executors
3. Create `cosmos-container.md` documenting container pattern
4. Update `integrators.md` to match actual code (no mapper dependencies)
5. Document `IXfrEntity` marker interface requirement or remove from docs

### Structural Fixes

1. Move `SealedProductsBySetCodeXfrEntity.cs` to `Queries/Entities/`
2. Move `UserSealedProductReadPointMapper.cs` to domain adapter
3. Split `AllUserSetCardsXfrToArgsMapper.cs` into interface + implementation files
4. Flatten `Lib.Adapter.SealedProducts/Apis/Queries/` into `Queries/`

---

## Part 6: File-by-File Findings Index

| File | Issues |
|------|--------|
| `CollectionCommandAdapter.cs` | No mappers, no resolvers, no integrators, inline DateTime, business logic |
| `AddUserWishlistCardAdapter.cs` | No resolver, no integrator, inline merge, inline DateTime |
| `AddUserSealedProductAdapter.cs` | No resolver, no integrator, inline mapping, inline DateTime |
| `UserCardIntegrator.cs` | Inline merge/replace logic (contradicts docs) |
| `SealedProductsBySetCodeXfrEntity.cs` | Concrete class in wrong location |
| `AllUserSetCardsXfrToArgsMapper.cs` | Interface + class combined |
| `UserSealedProductReadPointMapper.cs` | In wrong project, takes primitives |
| `AddUserWishlistCardXfrToExtMapper.cs` | Inline DateTime.UtcNow |
| `SearchArtistsAdapter.cs` | Inline args creation (no mapper) |
| `SealedProductsBySetCodeAdapter.cs` | Inline ReadPointItem creation |

---

## Appendix A: Compliant Reference Files

These files correctly implement documented patterns and can serve as templates:

### Command Adapters
- `Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs` — Full pattern with Mapper, Resolver, Integrator, Strategy
- `Lib.Adapter.User/Commands/RegisterUserAdapter.cs` — Mapper + Resolver pattern

### Integrators
- `Lib.Adapter.UserCards/Commands/Integrators/UserCardIntegrator.cs` — Correct interface inheritance
- `Lib.Adapter.UserSetCards/Commands/Integrators/UserSetCardIntegrator.cs` — With Resolver dependency

### Resolvers
- `Lib.Adapter.UserCards/Commands/Resolvers/UserCardResolver.cs` — ICosmosResolver pattern
- `Lib.Adapter.User/Commands/Resolvers/UserInfoResolver.cs` — Simple resolver

### Mappers
- `Lib.Adapter.UserCards/Commands/Mappers/AddUserCardXfrToReadPointMapper.cs` — Correct naming
- `Lib.Adapter.UserCards/Queries/Mappers/UserCardsSetXfrToArgsMapper.cs` — Query mapper pattern

### Cosmos Operators
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Gophers/UserCardsGopher.cs` — Point-read pattern
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserCardsScribe.cs` — Upsert pattern
- `Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/UserCardItemsBySetInquisition.cs` — Query pattern

---

## Appendix B: Projects Reviewed

| Project | Files | Status |
|---------|-------|--------|
| Lib.Adapter.Artists | 15 | Query-only, mostly compliant |
| Lib.Adapter.Cards | 20 | Query-only, compliant |
| Lib.Adapter.Collections | 21 | **Non-compliant** — needs major refactoring |
| Lib.Adapter.Scryfall.Cosmos | 202 | Infrastructure, needs documentation |
| Lib.Adapter.SealedProducts | 10 | Structural issues |
| Lib.Adapter.Sets | 22 | Query-only, compliant |
| Lib.Adapter.User | 12 | Partial compliance |
| Lib.Adapter.UserCards | 49 | **Reference implementation** |
| Lib.Adapter.UserSealedProducts | 12 | **Non-compliant** |
| Lib.Adapter.UserSetCards | 57 | Compliant |
| Lib.Adapter.UserWishlistCards | 22 | **Non-compliant** |
