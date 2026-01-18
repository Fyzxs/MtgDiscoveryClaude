# Refactoring Plan: Code Review Findings

**Date:** 2026-01-14
**Related Report:** [code-review-refactoring-report.md](./code-review-refactoring-report.md)
**Scope:** All items from code review report EXCEPT 7.1 (Scryfall Ingestion overhaul) and 3.2 (Collection/Wishlist unification investigation)

---

## Summary of Changes

| Issue | Files | Effort |
|-------|-------|--------|
| Boolean negation (`!`) | 1 | Low |
| Greater-than operator (`>`) | 2 | Low |
| Missing ConfigureAwait | 0 (already compliant) | None |
| ReadPointItem mapper duplication | 3 → 1 shared | Medium |
| XfrEntity file typo | 1 rename | Low |
| Missing sealed modifiers | 6 | Low |

---

## Phase 1: Quick Fixes (Low Effort)

### 1.1 Fix Boolean Negation

**File:** `src/Lib.Cosmos/Apis/Operators/OpResponse.cs:39`

Current:
```csharp
public bool IsNotSuccessful() => !IsSuccessful();
```

Change to:
```csharp
public bool IsNotSuccessful() => IsSuccessful() is false;
```

### 1.2 Fix Greater-Than Operators

**File 1:** `src/Lib.Adapter.Artists/Queries/CardsByArtistNameAdapter.cs:102`
- Change `sortedMatches.Count > 1` to `1 < sortedMatches.Count`

**File 2:** `src/Lib.Aggregator.UserCards/Queries/UserCardsForSigning/Mappers/UserCardsToSigningResultMapper.cs`
- Line 72: `unsignedCopies > 0` → `0 < unsignedCopies`
- Line 101: `c.UnsignedCopies > 0` → `0 < c.UnsignedCopies`
- Line 162: `unsignedCopies > 0` → `0 < unsignedCopies`

### 1.3 Fix XfrEntity File Typo

**Rename file:**
- From: `src/Lib.Adapter.Artists/Apis/Entities/IArtistSearchTermXrfEntity.cs`
- To: `src/Lib.Adapter.Artists/Apis/Entities/IArtistSearchTermXfrEntity.cs`

(Interface name inside is already correct: `IArtistSearchTermXfrEntity`)

### 1.4 Add Missing Sealed Modifiers

Add `sealed` keyword to these classes:

| File | Class |
|------|-------|
| `src/App.MtgDiscovery.GraphQL/Queries/ApiQuery.cs` | `ApiQuery` |
| `src/App.MtgDiscovery.GraphQL/Queries/ArtistQueryMethods.cs` | `ArtistQueryMethods` |
| `src/App.MtgDiscovery.GraphQL/Queries/CardQueryMethods.cs` | `CardQueryMethods` |
| `src/App.MtgDiscovery.GraphQL/Queries/SetQueryMethods.cs` | `SetQueryMethods` |
| `src/App.MtgDiscovery.GraphQL/Mutations/ApiMutation.cs` | `ApiMutation` |
| `src/App.MtgDiscovery.GraphQL/ErrorHandling/HttpStatusCodeErrorFilter.cs` | `HttpStatusCodeErrorFilter` |

---

## Phase 2: ReadPointItem Mapper Consolidation (Medium Effort)

### Current State (3 duplicate mappers)

1. `src/Lib.Adapter.Cards/Queries/Mappers/CollectionCardIdToReadPointItemMapper.cs`
2. `src/Lib.Adapter.Sets/Queries/Mappers/CollectionSetCodeToReadPointItemMapper.cs`
3. `src/Lib.Adapter.Sets/Queries/Mappers/CollectionSetIdToReadPointItemMapper.cs`

All do the same thing: convert `IEnumerable<string>` → `ICollection<ReadPointItem>` where each string becomes both the Id and Partition.

### Implementation Plan

**Step 1:** Create shared mapper in `Lib.Cosmos`

```
src/Lib.Cosmos/Apis/Mappers/
├── IStringToReadPointItemMapper.cs
└── StringToReadPointItemMapper.cs
```

**Step 2:** Update existing mappers to delegate to shared mapper
- Each domain-specific mapper becomes a thin wrapper that calls the shared mapper
- Preserves domain-specific interfaces for type safety

**Step 3:** Update DI registrations if needed

---

## Phase 3: Verification

1. **Build:** `dotnet build src/MtgDiscoveryVibe.sln`
2. **Test:** `dotnet test src/MtgDiscoveryVibe.sln`
3. **Verify no regressions in:**
   - Card queries by ID
   - Set queries by code/ID
   - Artist searches

---

## Excluded Items (Per User Direction)

- **7.1 Scryfall Ingestion overhaul** - Technical debt, separate effort
- **3.2 Validator duplication** - Future architectural investigation (Collection/Wishlist unification)

---

## Note: ConfigureAwait Finding

The original report indicated missing `ConfigureAwait(false)` calls. Upon detailed exploration, **the codebase is already fully compliant** - all async calls have proper `ConfigureAwait(false)`. No changes needed.
