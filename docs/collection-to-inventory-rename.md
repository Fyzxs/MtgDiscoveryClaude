# Rename Domain Concept: Collection → Inventory

## Context

The domain concept "Collection" (a user's collection of cards) causes persistent naming confusion with C# `ICollection<T>` and generic "collection of items" concepts throughout the codebase. Names like `CollectionExtToOufMapper` read as "mapping a C# collection" rather than "mapping a Collection entity." The `CollectionCollectionExtToOufMapper` name (batch-mapping a Collection) is the clearest symptom.

**Rename**: "Collection" (domain) → "Inventory" everywhere — backend, frontend, Cosmos, GraphQL API.

**What does NOT change**: `CollectionCreateMapper`, `ChildCollectionMapper`, `ICollection<T>`, `ICardItemCollectionOufEntity`, or any usage of "collection" meaning "a group of items."

---

## Decisions

| Decision | Answer |
|----------|--------|
| Cosmos "UserCollections" container | Rename to "UserInventory" — not in prod, no migration |
| UserCards `user_id` → `collection_id` migration | Change target to `inventory_id` instead |
| JSON property names | Rename `collection_id` → `inventory_id` in C# attributes |
| Frontend | Included in this plan |
| Route path | `/account/collections` → `/account/inventories` |
| EntryMode literal | `'collection'` → `'inventory'` |
| localStorage key | Just rename, no migration |
| `CollectionCreateMapper` | Keep as-is — it maps collections of items |

---

## Phase 1: C# Backend — Shared DataModels

**~24 files** in `common/Lib.Shared.DataModels/Entities/`

### Directory renames
- `Entities/Args/Collections/` → `Entities/Args/Inventories/`
- `Entities/Itrs/Collections/` → `Entities/Itrs/Inventories/`
- `Entities/Oufs/Collections/` → `Entities/Oufs/Inventories/`

### Interface renames (file + type + namespace)

**Args (8 files)**:
| Old | New |
|-----|-----|
| `ICollectionIdArgEntity` | `IInventoryIdArgEntity` |
| `ICreateCollectionArgEntity` | `ICreateInventoryArgEntity` |
| `IDeleteCollectionArgEntity` | `IDeleteInventoryArgEntity` |
| `IGrantCollectionAccessArgEntity` | `IGrantInventoryAccessArgEntity` |
| `IRenameCollectionArgEntity` | `IRenameInventoryArgEntity` |
| `IRevokeCollectionAccessArgEntity` | `IRevokeInventoryAccessArgEntity` |
| `ITransferCollectionOwnershipArgEntity` | `ITransferInventoryOwnershipArgEntity` |
| `IUpdateCollectionVisibilityArgEntity` | `IUpdateInventoryVisibilityArgEntity` |

**Itrs (10 files)**:
| Old | New |
|-----|-----|
| `ICollectionIdItrEntity` | `IInventoryIdItrEntity` |
| `ICollectionItrEntity` | `IInventoryItrEntity` |
| `IDeleteCollectionItrEntity` | `IDeleteInventoryItrEntity` |
| `IGrantCollectionAccessItrEntity` | `IGrantInventoryAccessItrEntity` |
| `IRenameCollectionItrEntity` | `IRenameInventoryItrEntity` |
| `IRevokeCollectionAccessItrEntity` | `IRevokeInventoryAccessItrEntity` |
| `ITransferCollectionOwnershipItrEntity` | `ITransferInventoryOwnershipItrEntity` |
| `IUpdateCollectionVisibilityItrEntity` | `IUpdateInventoryVisibilityItrEntity` |
| `IAuthorizedUserItrEntity` | No rename (not Collection-specific) |
| `IOwnerIdItrEntity` | No rename (not Collection-specific) |

**Oufs (2 files)**:
| Old | New |
|-----|-----|
| `ICollectionOufEntity` | `IInventoryOufEntity` |
| `IAuthorizedUserOufEntity` | No rename |

**Models (2 files)**:
| Old | New |
|-----|-----|
| `ICollectionIdArgModel` | `IInventoryIdArgModel` |
| `IOptionalCollectionIdArgModel` | `IOptionalInventoryIdArgModel` |

### Property renames across ALL shared entities
- `CollectionId` → `InventoryId` (property name on interfaces across Cards, Sets, UserCards, UserSetCards, UserWishlistCards, UserSealedProducts, etc.)

---

## Phase 2: C# Backend — Cosmos/Infrastructure

**~2 files** in `Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Collections/`

### Directory rename
- `CosmosItems/Collections/` → `CosmosItems/Inventories/`

### File renames
| Old | New |
|-----|-----|
| `CollectionExtEntity.cs` | `InventoryExtEntity.cs` |
| `AuthorizedUserExtEntity.cs` | No rename |

### JSON property renames in `CollectionExtEntity` → `InventoryExtEntity`
- `[JsonProperty("collection_id")]` → `[JsonProperty("inventory_id")]`
- Class name: `CollectionExtEntity` → `InventoryExtEntity`

### Cosmos container configuration
- Find and update the container name from `"UserCollections"` to `"UserInventory"`

### UserCards ExtEntity
- `CollectionId` property: rename to `InventoryId`
- `[JsonProperty("collection_id")]` → `[JsonProperty("inventory_id")]`

### DataMigration CLI
- Update existing migration: `user_id` → `inventory_id` (was targeting `collection_id`)

---

## Phase 3: C# Backend — Adapter Layer

**~77 files** in `Lib.Adapter.Collections/` → rename entire project to `Lib.Adapter.Inventories/`

### Project rename
- `Lib.Adapter.Collections.csproj` → `Lib.Adapter.Inventories.csproj`
- Root namespace: `Lib.Adapter.Collections` → `Lib.Adapter.Inventories`
- Update all `<ProjectReference>` entries in consuming projects

### Key type renames
| Old | New |
|-----|-----|
| `ICollectionsAdapterService` | `IInventoriesAdapterService` |
| `ICollectionCommandAdapter` | `IInventoryCommandAdapter` |
| `ICollectionQueryAdapter` | `IInventoryQueryAdapter` |
| `CollectionsAdapterService` | `InventoriesAdapterService` |
| `CollectionCommandAdapter` | `InventoryCommandAdapter` |
| `CreateCollectionAdapter` | `CreateInventoryAdapter` |
| `DeleteCollectionAdapter` | `DeleteInventoryAdapter` |
| `RenameCollectionAdapter` | `RenameInventoryAdapter` |
| `GrantCollectionAccessAdapter` | `GrantInventoryAccessAdapter` |
| `RevokeCollectionAccessAdapter` | `RevokeInventoryAccessAdapter` |
| `TransferCollectionOwnershipAdapter` | `TransferInventoryOwnershipAdapter` |
| `UpdateCollectionVisibilityAdapter` | `UpdateInventoryVisibilityAdapter` |
| `DefaultCollectionAdapter` | `DefaultInventoryAdapter` |
| `CollectionByIdAdapter` | `InventoryByIdAdapter` |
| `CollectionsByOwnerAdapter` | `InventoriesByOwnerAdapter` |
| `AccessibleCollectionsAdapter` | `AccessibleInventoriesAdapter` |
| `CollectionAdapterException` | `InventoryAdapterException` |

All XfrEntity interfaces, mappers, integrators, resolvers follow the same `Collection` → `Inventory` rename pattern.

### Test project
- `Lib.Adapter.Collections.Tests/` → `Lib.Adapter.Inventories.Tests/`
- ~34 files: rename all types, namespaces, fakes

---

## Phase 4: C# Backend — Aggregator Layer

**~49 files** in `Lib.Aggregator.Collections/` → rename entire project to `Lib.Aggregator.Inventories/`

### Project rename
- `Lib.Aggregator.Collections.csproj` → `Lib.Aggregator.Inventories.csproj`
- Root namespace: `Lib.Aggregator.Collections` → `Lib.Aggregator.Inventories`

### Key type renames
| Old | New |
|-----|-----|
| `ICollectionsAggregatorService` | `IInventoriesAggregatorService` |
| `ICollectionCommandAggregatorService` | `IInventoryCommandAggregatorService` |
| `ICollectionQueryAggregatorService` | `IInventoryQueryAggregatorService` |
| `CollectionsAggregatorService` | `InventoriesAggregatorService` |
| `CollectionCommandAggregator` | `InventoryCommandAggregator` |
| `CollectionQueryAggregator` | `InventoryQueryAggregator` |
| `CollectionOufEntity` | `InventoryOufEntity` |
| `CollectionXfrEntity` | `InventoryXfrEntity` |
| `CollectionExtToOufMapper` | `InventoryExtToOufMapper` |
| `CollectionItrToXfrMapper` | `InventoryItrToXfrMapper` |
| `CollectionCollectionExtToOufMapper` | `CollectionInventoryExtToOufMapper` |
| `ICollectionCollectionExtToOufMapper` | `ICollectionInventoryExtToOufMapper` |

All command/query mappers, entities follow the same pattern.

### Test project
- `Lib.Aggregator.Collections.Tests/` → `Lib.Aggregator.Inventories.Tests/`
- ~13 files

---

## Phase 5: C# Backend — Domain Layer

**~8 files** in `Lib.Domain.Collections/` → rename to `Lib.Domain.Inventories/`

### Key type renames
| Old | New |
|-----|-----|
| `ICollectionsDomainService` | `IInventoriesDomainService` |
| `ICollectionCommandDomainService` | `IInventoryCommandDomainService` |
| `ICollectionQueryDomainService` | `IInventoryQueryDomainService` |
| `CollectionsDomainService` | `InventoriesDomainService` |
| `CollectionCommandDomainService` | `InventoryCommandDomainService` |
| `CollectionQueryDomainService` | `InventoryQueryDomainService` |

### Test project
- `Lib.Domain.Collections.Tests/` → `Lib.Domain.Inventories.Tests/`
- ~13 files

---

## Phase 6: C# Backend — Entry Layer

**~80+ files** in `Lib.MtgDiscovery.Entry/`

### Directory renames
- `Commands/Collections/` → `Commands/Inventories/`
- `Queries/Collections/` → `Queries/Inventories/`
- `Entities/Collections/` → `Entities/Inventories/`
- `Entities/Outs/Collections/` → `Entities/Outs/Inventories/`

### Key type renames
| Old | New |
|-----|-----|
| `ICollectionEntryCommandService` | `IInventoryEntryCommandService` |
| `ICollectionEntryQueryService` | `IInventoryEntryQueryService` |
| `CollectionEntryCommandService` | `InventoryEntryCommandService` |
| `CollectionEntryQueryService` | `InventoryEntryQueryService` |
| `IDefaultCollectionCreator` | `IDefaultInventoryCreator` |
| `DefaultCollectionCreator` | `DefaultInventoryCreator` |
| `CollectionOutEntity` | `InventoryOutEntity` |
| `CollectionItrEntity` | `InventoryItrEntity` |

All validators: `*CollectionArgEntityValidator*` → `*InventoryArgEntityValidator*` (~50 files)
All mappers: `*CollectionArgToItr*` → `*InventoryArgToItr*`, `CollectionOufToOut*` → `InventoryOufToOut*`

### Cross-domain references in Entry
| Old | New |
|-----|-----|
| `AddCardToCollectionArgEntity` | `AddCardToInventoryArgEntity` |
| `AddSealedProductToCollectionArgsEntity` | `AddSealedProductToInventoryArgsEntity` |
| `UserCardCollectionIntegrator` | `UserCardInventoryIntegrator` |
| `UserSetCollectionIntegrator` | `UserSetInventoryIntegrator` |
| `CollectionCardItemOufToOutMapper` | `InventoryCardItemOufToOutMapper` |
| `CollectionUserCardOufToOutMapper` | `InventoryUserCardOufToOutMapper` |
| `CollectionUserSetCardOufToOutMapper` | `InventoryUserSetCardOufToOutMapper` |
| `CollectionSealedProductOufToOutMapper` | `InventorySealedProductOufToOutMapper` |

### Test project
- `Lib.MtgDiscovery.Entry.Tests/Commands/Collections/` → `Commands/Inventories/`

---

## Phase 7: C# Backend — App/GraphQL Layer

**~39 files** in `App.MtgDiscovery.GraphQL/`

### Directory renames
- `Entities/Args/Collections/` → `Entities/Args/Inventories/`
- `Entities/Types/Args/Collections/` → `Entities/Types/Args/Inventories/`
- `Entities/Types/Collections/` → `Entities/Types/Inventories/`
- `Actions/Mappers/Collections/` → `Actions/Mappers/Inventories/`

### Key type renames
| Old | New |
|-----|-----|
| `CollectionQueryMethods` | `InventoryQueryMethods` |
| `CollectionMutationMethods` | `InventoryMutationMethods` |
| `CollectionOutEntityType` | `InventoryOutEntityType` |
| `CollectionResponseModelUnionType` | `InventoryResponseModelUnionType` |
| `CollectionsResponseModelUnionType` | `InventoriesResponseModelUnionType` |
| `CollectionsSuccessDataResponseModelType` | `InventoriesSuccessDataResponseModelType` |
| `CreateCollectionArgEntity` | `CreateInventoryArgEntity` |
| `CreateCollectionArgEntityInputType` | `CreateInventoryArgEntityInputType` |

All 7 ArgEntity files + 7 InputType files + all mapper files follow same pattern.

### GraphQL schema names (user-facing API)
| Old | New |
|-----|-----|
| `myCollections` | `myInventories` |
| `collection` | `inventory` |
| `collectionAccessList` | `inventoryAccessList` |
| `sharedCollections` | `sharedInventories` |
| `accessibleCollections` | `accessibleInventories` |
| `createCollection` | `createInventory` |
| `renameCollection` | `renameInventory` |
| `deleteCollection` | `deleteInventory` |
| `grantCollectionAccess` | `grantInventoryAccess` |
| `revokeCollectionAccess` | `revokeInventoryAccess` |
| `transferCollectionOwnership` | `transferInventoryOwnership` |
| `updateCollectionVisibility` | `updateInventoryVisibility` |
| `addCardToCollection` | `addCardToInventory` |

---

## Phase 8: C# Backend — CLI Apps

**~3 files** in `Cli.MtgDiscovery.DataMigration/Collections/` → `Inventories/`

| Old | New |
|-----|-----|
| `CollectionMigrationOrchestrator` | `InventoryMigrationOrchestrator` |
| `CollectionMigrationResult` | `InventoryMigrationResult` |

Update the existing UserCards migration to target `inventory_id` instead of `collection_id`.

---

## Phase 9: C# Backend — Cross-Domain Property Rename

**~190 files** across entire codebase where `CollectionId` appears as a property.

This is the broadest change. Every entity interface and implementation across UserCards, UserSetCards, UserWishlistCards, UserSealedProducts, and Collections that has a `CollectionId` property needs it renamed to `InventoryId`.

### Key files
- All `*ExtEntity` classes with `[JsonProperty("collection_id")]` → `[JsonProperty("inventory_id")]`
- All `*ItrEntity`, `*XfrEntity`, `*OufEntity`, `*OutEntity`, `*ArgEntity` interfaces with `CollectionId` → `InventoryId`
- All mapper implementations that reference `CollectionId`
- All validator implementations that reference `CollectionId`
- All test fakes that reference `CollectionId`

---

## Phase 10: React Frontend — GraphQL Operations

**~14 files** in `client/web/graphql/`

### File renames
- `queries/getMyCollections.ts` → `queries/getMyInventories.ts`
- `queries/getSharedCollections.ts` → `queries/getSharedInventories.ts`
- `queries/getAccessibleCollections.ts` → `queries/getAccessibleInventories.ts`
- `queries/getCollection.ts` → `queries/getInventory.ts`
- `queries/getCollectionAccessList.ts` → `queries/getInventoryAccessList.ts`
- `mutations/createCollection.ts` → `mutations/createInventory.ts`
- `mutations/renameCollection.ts` → `mutations/renameInventory.ts`
- `mutations/deleteCollection.ts` → `mutations/deleteInventory.ts`
- `mutations/grantCollectionAccess.ts` → `mutations/grantInventoryAccess.ts`
- `mutations/revokeCollectionAccess.ts` → `mutations/revokeInventoryAccess.ts`
- `mutations/transferCollectionOwnership.ts` → `mutations/transferInventoryOwnership.ts`
- `mutations/updateCollectionVisibility.ts` → `mutations/updateInventoryVisibility.ts`
- `mutations/addCardToCollection.ts` → `mutations/addCardToInventory.ts`
- `mutations/collection.ts` → `mutations/inventory.ts`

Update all query/mutation names and variable references to match new GraphQL schema names.

---

## Phase 11: React Frontend — Components, Contexts, Hooks

**~47 files**

### Context renames (3 files)
| Old | New |
|-----|-----|
| `CollectionContext.tsx` | `InventoryContext.tsx` |
| `CollectionManagementContext.tsx` | `InventoryManagementContext.tsx` |
| `SealedCollectionContext.tsx` | `SealedInventoryContext.tsx` |

Exports: `useCollection()` → `useInventory()`, `CollectionProvider` → `InventoryProvider`, etc.

### Hook renames (7 files)
| Old | New |
|-----|-----|
| `useCollectionParam.ts` | `useInventoryParam.ts` |
| `useCardCollectionEntry.ts` | `useCardInventoryEntry.ts` |
| `useCardCollectionFromCache.ts` | `useCardInventoryFromCache.ts` |
| `useMtgCardCollectionActions.ts` | `useMtgCardInventoryActions.ts` |
| `useSetCollectionProgress.ts` | `useSetInventoryProgress.ts` |
| `useSealedProductCollectionEntry.ts` | `useSealedProductInventoryEntry.ts` |
| `useSealedProductCollectionActions.ts` | `useSealedProductInventoryActions.ts` |

### Component renames (23 files)
All components with "Collection" in name → "Inventory":
- `CollectionsPage.tsx` → `InventoriesPage.tsx`
- `CollectionPreview.tsx` → `InventoryPreview.tsx`
- `CreateCollectionDialog.tsx` → `CreateInventoryDialog.tsx`
- `CollectionCard.tsx` → `InventoryCard.tsx`
- `CollectionSelector.tsx` → `InventorySelector.tsx`
- `MyCollectionButton.tsx` → `MyInventoryButton.tsx`
- etc. (full list available via codebase search)

### Type renames (1 file)
- `types/collection.ts` → `types/inventory.ts`
- `EntryMode = 'collection' | 'wishlist'` → `'inventory' | 'wishlist'`

### Utility renames (1 file)
- `utils/collectionFormatters.ts` → `utils/inventoryFormatters.ts`

---

## Phase 12: React Frontend — Localization, Routes, Config

### Localization (3 files)
- `public/locales/en/collection.json` → `inventory.json`
- Update all keys: `"collection"` → `"inventory"`, `"addToCollection"` → `"addToInventory"`, etc.
- Update navigation labels in all locale files
- Update i18n key constants

### Routes
- `App.tsx`: `/account/collections` → `/account/inventories`

### localStorage
- `ACTIVE_COLLECTION_KEY = 'mtg-discovery-active-collection'` → `'mtg-discovery-active-inventory'`

### Generated files
- Run `npm run codegen` after backend schema changes — `generated/graphql.ts` and `generated/gql.ts` will auto-regenerate

---

## Phase 13: Solution File & Project References

### Solution file updates
- Rename project entries in `.sln` for all 6 Collection projects → Inventory
- Update all `<ProjectReference>` paths across consuming `.csproj` files

### CLAUDE.md / Documentation
- Update any references to "Collection" domain concept in CLAUDE.md files
- Update aggregator CLAUDE.md folder structure examples

---

## Exclusions — Do NOT Rename

| Item | Why |
|------|-----|
| `CollectionCreateMapper<T,R>` | Maps C# collections, not the domain concept |
| `ChildCollectionMapper<T,R>` | Maps child C# collections, not the domain concept |
| `ICollection<T>` usages | C# generic type |
| `ICardItemCollectionOufEntity` | "Collection of card items" — C# collection sense |
| `ISetItemCollectionOufEntity` | Same |
| `IArtistSearchResultCollectionOufEntity` | Same |
| `ICardNameSearchCollectionOufEntity` | Same |
| `CollectedList` property on UserCard entities | "Collected" ≠ "Collection" domain concept |

---

## Execution Strategy

This rename is too large for a single pass (~470+ files). Break into PRs:

| PR | Scope | Risk |
|----|-------|------|
| **PR 1** | Phases 1-2: Shared DataModels + Cosmos | Foundation — must merge first |
| **PR 2** | Phases 3-5: Adapter + Aggregator + Domain | Layer-by-layer, builds on PR 1 |
| **PR 3** | Phases 6-8: Entry + GraphQL + CLI | Upper layers, builds on PR 2 |
| **PR 4** | Phase 9: Cross-domain `CollectionId` → `InventoryId` | Broadest single change |
| **PR 5** | Phases 10-12: React frontend | After backend API is renamed |
| **PR 6** | Phase 13: Solution file + docs cleanup | Final cleanup |

Alternative: If the branch is already isolated, do it all in one branch with phase-by-phase commits.

---

## Verification

After all changes:
1. `dotnet build` — 0 errors, 0 warnings
2. `dotnet test` — all tests pass
3. `grep -r "CollectionId" csharp/src/` — only hits `ICollection<T>` or `CollectionCreateMapper` usages
4. `grep -rn "Lib\..*\.Collection[s]\b" csharp/src/**/*.csproj` — zero project references to old names
5. `grep -r "collection_id" csharp/src/Lib.Adapters/` — zero JSON property hits
6. Frontend: `npm run codegen && npm run build` — no TypeScript errors
7. Frontend: `grep -r "collectionId\|CollectionId\|collection_id" client/web/src/` — zero hits (except generated files if codegen not yet run)
8. Verify GraphQL schema exposes `inventory`/`inventories` operations, not `collection`/`collections`
