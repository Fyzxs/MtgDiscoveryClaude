# Lib.Adapters Remediation Plan

## Overview

This plan addresses all architectural pattern violations and inconsistencies discovered during the adapter layer review. Issues are organized by priority and grouped by type for efficient batch remediation.

---

## Completed Work

### Phase 1-2: Quick Wins + Exception Standardization ✅
- [x] Added `ConfigureAwait(false)` to all Tier 2 adapters (10 files)
- [x] Standardized response checking (`IsNotSuccessful()`) in SealedProducts
- [x] Added `ConfigureAwait(false)` to CollectionsAdapterService (13 methods)
- [x] Standardized all exception classes to Pattern A (4 files)

### Phase 3: Collections Restructuring (Partial) ✅
- [x] **Task 1.5.1**: Changed concrete types to interfaces in CollectionCommandAdapter and CollectionQueryAdapter
- [x] **Task 1.2.1-1.2.2**: Extracted inline mapping to ICreateMapper classes:
  - Created `Queries/Mappers/IAuthorizedUserExtToOufMapper.cs`
  - Created `Queries/Mappers/AuthorizedUserExtToOufMapper.cs`
  - Created `Queries/Mappers/ICollectionExtToOufMapper.cs`
  - Created `Queries/Mappers/CollectionExtToOufMapper.cs`
  - Updated both adapters to use injected mappers
- [ ] **Task 1.1.1-1.1.4**: Add Tier 3 single-operation adapters (DEFERRED - large undertaking)

### Phase 4: SealedProducts Restructuring ✅
- [x] **Task 1.3.1-1.3.4**: Added Tier 2 QueryAdapter:
  - Created `Apis/ISealedProductsQueryAdapter.cs` interface
  - Created `Queries/SealedProductsQueryAdapter.cs` implementation
  - Updated `ISealedProductsAdapterService` to inherit from `ISealedProductsQueryAdapter`
  - Updated `SealedProductsAdapterService` to delegate through QueryAdapter

### Phase 6: Documentation Standardization ✅
- [x] **Task 7.1.1**: Added XML docs to `ICollectionCommandAdapter.cs`
- [x] **Task 7.1.2**: Added XML docs to `ICollectionQueryAdapter.cs`
- [x] **Task 7.1.3**: Added XML docs to `IUserSealedProductsCommandAdapter.cs`
- [x] **Task 7.1.4**: Added XML docs to `IUserSealedProductsQueryAdapter.cs`
- [x] **Task 7.1.5**: Added XML docs to `IUserWishlistCardsCommandAdapter.cs`
- [x] **Task 7.2.1**: Added XML docs to `CollectionsAdapterService.cs`
- [x] **Task 7.2.2**: `SealedProductsAdapterService.cs` already had docs (added in Phase 4)
- [x] **Task 7.2.3**: Added XML docs to `UserSealedProductsAdapterService.cs`
- [x] **Task 7.2.4**: Added XML docs to `UserWishlistCardsAdapterService.cs`

---

## Remaining Work

---

## Priority 1: Critical Architecture Violations

### 1.1 Collections — Add Missing Tier 3 (Single-Operation Adapters)

**Problem**: `CollectionQueryAdapter` and `CollectionCommandAdapter` implement logic directly instead of delegating to single-operation adapters.

**Current State**:
```
CollectionsAdapterService
    ├── CollectionQueryAdapter (5 methods with inline logic)
    └── CollectionCommandAdapter (7 methods with inline logic)
```

**Target State**:
```
CollectionsAdapterService
    ├── CollectionQueryAdapter
    │       ├── GetDefaultCollectionAdapter
    │       ├── GetCollectionsByOwnerAdapter
    │       ├── GetCollectionByIdAdapter
    │       ├── GetSharedCollectionsAdapter
    │       └── GetAccessibleCollectionsAdapter
    └── CollectionCommandAdapter
            ├── CreateCollectionAdapter
            ├── RenameCollectionAdapter
            ├── UpdateCollectionVisibilityAdapter
            ├── GrantCollectionAccessAdapter
            ├── RevokeCollectionAccessAdapter
            ├── DeleteCollectionAdapter
            └── TransferCollectionOwnershipAdapter
```

**Tasks**:

- [ ] **1.1.1** Create `Queries/` subfolder structure:
  - [ ] Create `IGetDefaultCollectionAdapter.cs` interface
  - [ ] Create `GetDefaultCollectionAdapter.cs` implementation
  - [ ] Create `IGetCollectionsByOwnerAdapter.cs` interface
  - [ ] Create `GetCollectionsByOwnerAdapter.cs` implementation
  - [ ] Create `IGetCollectionByIdAdapter.cs` interface
  - [ ] Create `GetCollectionByIdAdapter.cs` implementation
  - [ ] Create `IGetSharedCollectionsAdapter.cs` interface
  - [ ] Create `GetSharedCollectionsAdapter.cs` implementation
  - [ ] Create `IGetAccessibleCollectionsAdapter.cs` interface
  - [ ] Create `GetAccessibleCollectionsAdapter.cs` implementation

- [ ] **1.1.2** Create `Commands/` subfolder structure:
  - [ ] Create `ICreateCollectionAdapter.cs` interface
  - [ ] Create `CreateCollectionAdapter.cs` implementation
  - [ ] Create `IRenameCollectionAdapter.cs` interface
  - [ ] Create `RenameCollectionAdapter.cs` implementation
  - [ ] Create `IUpdateCollectionVisibilityAdapter.cs` interface
  - [ ] Create `UpdateCollectionVisibilityAdapter.cs` implementation
  - [ ] Create `IGrantCollectionAccessAdapter.cs` interface
  - [ ] Create `GrantCollectionAccessAdapter.cs` implementation
  - [ ] Create `IRevokeCollectionAccessAdapter.cs` interface
  - [ ] Create `RevokeCollectionAccessAdapter.cs` implementation
  - [ ] Create `IDeleteCollectionAdapter.cs` interface
  - [ ] Create `DeleteCollectionAdapter.cs` implementation
  - [ ] Create `ITransferCollectionOwnershipAdapter.cs` interface
  - [ ] Create `TransferCollectionOwnershipAdapter.cs` implementation

- [ ] **1.1.3** Refactor `CollectionQueryAdapter.cs`:
  - [ ] Inject all 5 single-operation adapters
  - [ ] Convert methods to delegation calls
  - [ ] Add `ConfigureAwait(false)` to all delegation calls

- [ ] **1.1.4** Refactor `CollectionCommandAdapter.cs`:
  - [ ] Inject all 7 single-operation adapters
  - [ ] Convert methods to delegation calls
  - [ ] Add `ConfigureAwait(false)` to all delegation calls

**Reference**: `Lib.Adapter.UserCards/Queries/UserCardsQueryAdapter.cs`

---

### 1.2 Collections — Extract Inline Mapping to ICreateMapper Classes

**Problem**: Mapping logic is duplicated inline across 8+ methods.

**Files Affected**:
- `CollectionQueryAdapter.cs:167-184` (MapToOuf method)
- `CollectionCommandAdapter.cs` (lines 76-93, 143-160, 210-227, 293-310, 369-386, 433-450, 534-551)

**Tasks**:

- [ ] **1.2.1** Create mapper interfaces and implementations:
  - [ ] Create `Queries/Mappers/ICollectionExtToOufMapper.cs`
  - [ ] Create `Queries/Mappers/CollectionExtToOufMapper.cs`
  - [ ] Create `Commands/Mappers/ICollectionItrToExtMapper.cs`
  - [ ] Create `Commands/Mappers/CollectionItrToExtMapper.cs`
  - [ ] Create `Commands/Mappers/IAuthorizedUserExtToOufMapper.cs`
  - [ ] Create `Commands/Mappers/AuthorizedUserExtToOufMapper.cs`

- [ ] **1.2.2** Inject mappers into adapters and remove inline mapping

**Reference**: `Lib.Adapter.SealedProducts/Queries/Mappers/SealedProductExtToOufMapper.cs`

---

### 1.3 SealedProducts — Add Missing Tier 2 (QueryAdapter)

**Problem**: `SealedProductsAdapterService` delegates directly to single-operation adapter, bypassing the QueryAdapter tier.

**Current State**:
```
SealedProductsAdapterService → SealedProductsBySetCodeAdapter
```

**Target State**:
```
SealedProductsAdapterService → SealedProductsQueryAdapter → SealedProductsBySetCodeAdapter
```

**Tasks**:

- [ ] **1.3.1** Create QueryAdapter:
  - [ ] Create `Queries/ISealedProductsQueryAdapter.cs` interface (in `Apis/` folder)
  - [ ] Create `Queries/SealedProductsQueryAdapter.cs` implementation
  - [ ] Move `SealedProductsBySetCodeAdapter.cs` from `Apis/Queries/` to `Queries/`

- [ ] **1.3.2** Update `ISealedProductsAdapterService.cs`:
  - [ ] Change to inherit from `ISealedProductsQueryAdapter`
  - [ ] Remove direct method declaration

- [ ] **1.3.3** Update `SealedProductsAdapterService.cs`:
  - [ ] Inject `ISealedProductsQueryAdapter` instead of `ISealedProductsBySetCodeAdapter`
  - [ ] Delegate to query adapter

- [ ] **1.3.4** Clean up folder structure:
  - [ ] Remove `Apis/Queries/` folder (move contents to `Queries/`)
  - [ ] Ensure consistent namespace updates

**Reference**: `Lib.Adapter.Artists/Queries/ArtistsQueryAdapter.cs`

---

### 1.4 Collections — Add Missing ConfigureAwait(false) in AdapterService

**Problem**: All 13 methods in `CollectionsAdapterService.cs` are missing `ConfigureAwait(false)`.

**File**: `Lib.Adapter.Collections/Apis/CollectionsAdapterService.cs`

**Tasks**:

- [ ] **1.4.1** Add `.ConfigureAwait(false)` to lines 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53

**Example Fix**:
```csharp
// Before
public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity)
    => await _commandAdapter.CreateCollectionAsync(entity);

// After
public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity)
    => await _commandAdapter.CreateCollectionAsync(entity).ConfigureAwait(false);
```

---

### 1.5 Collections — Use Interfaces Instead of Concrete Types

**Problem**: `CollectionCommandAdapter` uses concrete types in private constructor.

**File**: `Lib.Adapter.Collections/Commands/CollectionCommandAdapter.cs:33-40`

**Tasks**:

- [ ] **1.5.1** Change parameter types to interfaces:
```csharp
// Before
private CollectionCommandAdapter(
    CollectionScribe collectionScribe,
    CollectionGopher collectionGopher,
    CollectionJanitor collectionJanitor)

// After
private CollectionCommandAdapter(
    ICosmosScribe collectionScribe,
    ICosmosGopher collectionGopher,
    ICosmosJanitor collectionJanitor)
```

---

## Priority 2: Entity Type Corrections

### 2.1 Collections — Change Input Types from ItrEntity to XfrEntity

**Problem**: Collections adapter uses `ItrEntity` input types instead of `XfrEntity`.

**Files Affected**:
- `Apis/ICollectionCommandAdapter.cs`
- `Apis/ICollectionQueryAdapter.cs`
- `Commands/CollectionCommandAdapter.cs`
- `Queries/CollectionQueryAdapter.cs`

**Tasks**:

- [ ] **2.1.1** Create XfrEntity interfaces in `Apis/Entities/`:
  - [ ] `ICollectionXfrEntity.cs`
  - [ ] `IRenameCollectionXfrEntity.cs`
  - [ ] `IUpdateCollectionVisibilityXfrEntity.cs`
  - [ ] `IGrantCollectionAccessXfrEntity.cs`
  - [ ] `IRevokeCollectionAccessXfrEntity.cs`
  - [ ] `IDeleteCollectionXfrEntity.cs`
  - [ ] `ITransferCollectionOwnershipXfrEntity.cs`
  - [ ] `IOwnerIdXfrEntity.cs`
  - [ ] `ICollectionIdXfrEntity.cs`
  - [ ] `IUserIdXfrEntity.cs`

- [ ] **2.1.2** Update interface method signatures to use XfrEntity types

- [ ] **2.1.3** Update implementation classes to accept XfrEntity types

**Reference**: `Lib.Adapter.UserCards/Apis/Entities/IAddUserCardXfrEntity.cs`

---

### 2.2 User — Change Input Types from ItrEntity to XfrEntity

**Problem**: User adapter uses `ItrEntity` input types instead of `XfrEntity`.

**Files Affected**:
- `Apis/IUserCommandAdapter.cs`
- `Commands/UserCommandAdapter.cs`
- `Commands/RegisterUserAdapter.cs`

**Tasks**:

- [ ] **2.2.1** Create XfrEntity interface:
  - [ ] Create `Apis/Entities/IUserInfoXfrEntity.cs`

- [ ] **2.2.2** Update `IUserCommandAdapter.cs`:
  - [ ] Change `IUserInfoItrEntity` to `IUserInfoXfrEntity`

- [ ] **2.2.3** Update implementations to use new XfrEntity type

---

### 2.3 UserSealedProducts — Replace Raw String with XfrEntity

**Problem**: `IUserSealedProductsQueryAdapter.UserSealedProductsByUserIdAsync` takes raw `string` instead of XfrEntity.

**Files Affected**:
- `Apis/IUserSealedProductsQueryAdapter.cs`
- `Queries/UserSealedProductsQueryAdapter.cs`
- `Queries/UserSealedProductsByUserIdAdapter.cs`
- `Apis/UserSealedProductsAdapterService.cs`

**Tasks**:

- [ ] **2.3.1** Create XfrEntity:
  - [ ] Create `Apis/Entities/IUserSealedProductsByUserIdXfrEntity.cs`

- [ ] **2.3.2** Update interface:
```csharp
// Before
Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>>
    UserSealedProductsByUserIdAsync(string collectionId);

// After
Task<IOperationResponse<IEnumerable<UserSealedProductExtEntity>>>
    UserSealedProductsByUserIdAsync(IUserSealedProductsByUserIdXfrEntity query);
```

- [ ] **2.3.3** Update all implementations

---

## Priority 3: ConfigureAwait(false) Compliance

### 3.1 Add ConfigureAwait(false) to All Tier 2 Adapters

**Problem**: Most Query/Command adapters are missing `ConfigureAwait(false)` on delegation calls.

**Files to Fix**:

| File | Lines |
|------|-------|
| `Lib.Adapter.Artists/Queries/ArtistsQueryAdapter.cs` | 42, 44, 46 |
| `Lib.Adapter.Cards/Queries/CardsQueryAdapter.cs` | 44, 46, 48, 50 |
| `Lib.Adapter.Sets/Queries/SetsQueryAdapter.cs` | 41, 43, 45 |
| `Lib.Adapter.User/Commands/UserCommandAdapter.cs` | 25 |
| `Lib.Adapter.UserCards/Queries/UserCardsQueryAdapter.cs` | 39, 41, 43, 45, 47, 49 |
| `Lib.Adapter.UserCards/Commands/UserCardsCommandAdapter.cs` | 25 |
| `Lib.Adapter.UserSealedProducts/Queries/UserSealedProductsQueryAdapter.cs` | 20 |
| `Lib.Adapter.UserSealedProducts/Commands/UserSealedProductsCommandAdapter.cs` | 20 |
| `Lib.Adapter.UserSetCards/Queries/UserSetCardsQueryAdapter.cs` | 37, 39 |
| `Lib.Adapter.UserSetCards/Commands/UserSetCardsCommandAdapter.cs` | 31, 33 |

**Tasks**: ✅ ALL COMPLETE (verified in codebase)

- [x] **3.1.1** `ArtistsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.2** `CardsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.3** `SetsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.4** `UserCommandAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.5** `UserCardsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.6** `UserCardsCommandAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.7** `UserSealedProductsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.8** `UserSealedProductsCommandAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.9** `UserSetCardsQueryAdapter.cs` — Has `.ConfigureAwait(false)` on all methods
- [x] **3.1.10** `UserSetCardsCommandAdapter.cs` — Has `.ConfigureAwait(false)` on all methods

**Note**: `UserWishlistCardsQueryAdapter` and `UserWishlistCardsCommandAdapter` already have this correct.

---

## Priority 4: Exception Class Standardization

### 4.1 Standardize All Exception Classes

**Problem**: Three different exception patterns exist across projects.

**Target Pattern** (Pattern A with 2 constructors):
```csharp
#pragma warning disable CA1032
public sealed class {Domain}AdapterException : OperationException
#pragma warning restore CA1032
{
    public {Domain}AdapterException(string message)
        : base(HttpStatusCode.InternalServerError, message) { }

    public {Domain}AdapterException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
```

**Files to Standardize**:

| File | Current Pattern | Action |
|------|-----------------|--------|
| `Lib.Adapter.UserCards/Exceptions/UserCardsAdapterException.cs` | Pattern B (3 constructors, no pragma) | Remove parameterless constructor, add pragma |
| `Lib.Adapter.UserWishlistCards/Exceptions/UserWishlistCardsAdapterException.cs` | Pattern B | Remove parameterless constructor, add pragma |
| `Lib.Adapter.UserSetCards/Exceptions/UserSetCardsAdapterException.cs` | Pattern B | Remove parameterless constructor, add pragma |
| `Lib.Adapter.UserSealedProducts/Exceptions/UserSealedProductsAdapterException.cs` | Pattern C (internal) | Change to public, remove parameterless constructor, add pragma |

**Tasks**: ✅ ALL COMPLETE (verified in codebase)

- [x] **4.1.1** `UserCardsAdapterException.cs` - Already follows Pattern A
- [x] **4.1.2** `UserWishlistCardsAdapterException.cs` - Already follows Pattern A
- [x] **4.1.3** `UserSetCardsAdapterException.cs` - Already follows Pattern A
- [x] **4.1.4** `UserSealedProductsAdapterException.cs` - Already follows Pattern A

---

## Priority 5: Response Pattern Standardization

### 5.1 Standardize Response Status Checking

**Problem**: Mixed patterns for checking response success.

**Target Pattern**:
```csharp
if (response.IsNotSuccessful())  // Preferred
```

**Files to Fix**: ✅ ALL COMPLETE (verified in codebase)

- [x] **5.1.1** `SealedProductsBySetCodeAdapter.cs:57` - Already uses `IsNotSuccessful()`
- [x] **5.1.2** `SealedProductsBySetCodeAdapter.cs:71` - Already uses `IsNotSuccessful()`

### 5.2 ~~Standardize Response Property Access~~ (NO ACTION NEEDED)

**Clarification**: The `IOperationResponse<T>` interface uses `.ResponseData` as the standard property.
Some extension methods provide `.Value` as an alias, but `.ResponseData` is correct per the interface
definition in `Lib.Shared.Invocation/Operations/OperationResponse.cs:22`.

The original code using `.ResponseData` was correct. No changes needed.

---

## Priority 6: CancellationToken Handling

### 6.1 Pass CancellationToken Through Call Chain

**Problem**: CancellationToken is accepted but ignored or hardcoded.

**Files Affected**:

- [x] **6.1.1** `Lib.Adapter.SealedProducts` - ✅ DONE
  - ✅ Updated `ISealedProductsBySetCodeAdapter` interface with CancellationToken
  - ✅ Updated `SealedProductsBySetCodeAdapter.Execute()` to accept and pass CancellationToken
  - ✅ Updated `SealedProductsQueryAdapter` to pass token through
  - ✅ Updated `ISealedProductsQueryAdapter` (already had CancellationToken)
  - ✅ Updated entire stack: Aggregator → Domain → Entry → GraphQL
  - Files updated: 14 files across all layers

- [x] **6.1.2** `Lib.Adapter.Collections/Queries/CollectionQueryAdapter.cs` (lines 42, 67, 102, 129)
  - ✅ Replaced `CancellationToken.None` with proper CancellationToken parameter
  - ✅ Added CancellationToken parameter to all query interface methods
  - ✅ Updated all layers: Adapter → Aggregator → Domain → Entry → GraphQL
  - ✅ Updated all test fakes to match new signatures

---

## Priority 7: Documentation Standardization

### 7.1 Add XML Documentation to Specialized Interfaces

**Files Needing Documentation**:

| File | Status |
|------|--------|
| `Lib.Adapter.Collections/Apis/ICollectionCommandAdapter.cs` | Missing |
| `Lib.Adapter.Collections/Apis/ICollectionQueryAdapter.cs` | Missing |
| `Lib.Adapter.UserSealedProducts/Apis/IUserSealedProductsCommandAdapter.cs` | Missing |
| `Lib.Adapter.UserSealedProducts/Apis/IUserSealedProductsQueryAdapter.cs` | Missing |
| `Lib.Adapter.UserWishlistCards/Apis/IUserWishlistCardsCommandAdapter.cs` | Missing |

**Tasks**:

- [ ] **7.1.1** Add XML docs to `ICollectionCommandAdapter.cs`
- [ ] **7.1.2** Add XML docs to `ICollectionQueryAdapter.cs`
- [ ] **7.1.3** Add XML docs to `IUserSealedProductsCommandAdapter.cs`
- [ ] **7.1.4** Add XML docs to `IUserSealedProductsQueryAdapter.cs`
- [ ] **7.1.5** Add XML docs to `IUserWishlistCardsCommandAdapter.cs`

**Template** (from `IArtistQueryAdapter.cs`):
```csharp
/// <summary>
/// Specialized adapter interface for {domain} {query/command} operations.
///
/// This interface represents the {query/command}-specific adapter functionality,
/// separate from the main I{Domain}AdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   I{Domain}AdapterService : I{Domain}QueryAdapter, I{Domain}CommandAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
```

### 7.2 Add XML Documentation to AdapterService Classes

**Files Needing Documentation**:

| File | Status |
|------|--------|
| `Lib.Adapter.Collections/Apis/CollectionsAdapterService.cs` | Missing |
| `Lib.Adapter.SealedProducts/Apis/SealedProductsAdapterService.cs` | Missing |
| `Lib.Adapter.UserSealedProducts/Apis/UserSealedProductsAdapterService.cs` | Missing |
| `Lib.Adapter.UserWishlistCards/Apis/UserWishlistCardsAdapterService.cs` | Missing |

**Tasks**:

- [ ] **7.2.1** Add XML docs to `CollectionsAdapterService.cs`
- [ ] **7.2.2** Add XML docs to `SealedProductsAdapterService.cs`
- [ ] **7.2.3** Add XML docs to `UserSealedProductsAdapterService.cs`
- [ ] **7.2.4** Add XML docs to `UserWishlistCardsAdapterService.cs`

---

## Execution Order

### Phase 1: Quick Wins (ConfigureAwait + Response Patterns)
1. Task 3.1.1 - 3.1.10 (Add ConfigureAwait to all Tier 2 adapters)
2. Task 5.1.1 - 5.1.2 (Response status checking)
3. Task 5.2.1 (Response property access)
4. Task 1.4.1 (Collections AdapterService ConfigureAwait)

### Phase 2: Exception Standardization
5. Task 4.1.1 - 4.1.4 (Exception class standardization)

### Phase 3: Collections Restructuring
6. Task 1.5.1 (Use interfaces instead of concrete types) - ✅ DONE
7. Task 1.2.1 - 1.2.2 (Extract mappers) - ✅ DONE
8. Task 1.1.1 - 1.1.4 (Add Tier 3 adapters) - DEFERRED (large undertaking)
9. Task 2.1.1 - 2.1.3 (Change to XfrEntity types) - DEFERRED (large undertaking)

### Phase 4: SealedProducts Restructuring
10. Task 1.3.1 - 1.3.4 (Add Tier 2 QueryAdapter) - ✅ DONE

### Phase 5: Other Entity Type Corrections (DEFERRED)
11. Task 2.2.1 - 2.2.3 (User XfrEntity) - DEFERRED
12. Task 2.3.1 - 2.3.3 (UserSealedProducts XfrEntity) - DEFERRED

### Phase 6: Documentation
13. Task 7.1.1 - 7.1.5 (Interface documentation) - ✅ DONE
14. Task 7.2.1 - 7.2.4 (Service documentation) - ✅ DONE

### Phase 7: CancellationToken (FINAL - Comprehensive)
15. Task 6.1 - Pass CancellationToken through ALL GraphQL endpoints

**Completed:**
- [x] Task 6.1.2 Collections (4 query methods) - ✅ DONE
- [x] Task 6.1.1 SealedProducts (1 query method) - ✅ DONE

**Remaining (each requires ~10-15 file changes across GraphQL→Entry→Domain→Aggregator→Adapter):**

Query Methods:
- [ ] ArtistQueryMethods (3 methods: ArtistSearch, CardsByArtist, CardsByArtistName)
- [ ] CardQueryMethods
- [ ] SetQueryMethods
- [ ] UserCardsQueryMethods
- [ ] UserInfoQueryMethods
- [ ] UserWishlistCardsQueryMethods

Mutation Methods:
- [ ] CollectionMutationMethods
- [ ] UserCardsMutationMethods
- [ ] UserMutationMethods
- [ ] UserSealedProductsMutationMethods
- [ ] UserSetCardsMutationMethods
- [ ] UserWishlistCardsMutationMethods

**Pattern Established:** See `SealedProductsQueryMethods.cs` and `CollectionQueryMethods.cs` for reference.
Each endpoint requires updating interfaces and implementations through all 5 layers.

**Note**: CancellationToken work requires comprehensive changes across ALL layers for each endpoint.

---

## Validation Checklist

After completing all tasks, verify:

- [ ] All projects follow 3-tier hierarchy
- [ ] All composite interfaces inherit from specialized adapters
- [ ] All adapter inputs use XfrEntity types
- [ ] All async calls have ConfigureAwait(false)
- [ ] All mappers use ICreateMapper pattern
- [ ] All exceptions follow Pattern A
- [ ] All response checks use IsNotSuccessful()
- [ ] All CancellationTokens are properly passed
- [ ] All public interfaces have XML documentation
- [ ] All tests pass
- [ ] Code compiles without warnings

---

## Reference Implementations

| Pattern | Reference File |
|---------|---------------|
| 3-tier hierarchy | `Lib.Adapter.UserCards/` |
| AdapterService | `Lib.Adapter.Artists/Apis/ArtistAdapterService.cs` |
| QueryAdapter | `Lib.Adapter.Artists/Queries/ArtistsQueryAdapter.cs` |
| Single-operation adapter | `Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs` |
| XfrEntity | `Lib.Adapter.UserCards/Apis/Entities/IAddUserCardXfrEntity.cs` |
| Mapper | `Lib.Adapter.SealedProducts/Queries/Mappers/SealedProductExtToOufMapper.cs` |
| Exception | `Lib.Adapter.Artists/Exceptions/ArtistAdapterException.cs` |
| Interface docs | `Lib.Adapter.Artists/Apis/IArtistQueryAdapter.cs` |
