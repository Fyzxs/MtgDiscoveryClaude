# Implementation Plan: Collection Identity Architecture

**Branch**: `001-collection-identity` | **Date**: 2026-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-collection-identity/spec.md`

## Summary

Transition the implicit 1:1 User:Collection model to first-class Collection entities. Collections become independent data objects with their own identity, owner, authorized users (admin/editor/viewer roles), visibility (private/public), and type (default/custom/cube/trade). The feature spans three epics: core infrastructure (Cosmos container, service layers, authorization, GraphQL schema, migration), collection selection UI (React context, selector, management page), and collection sharing (grant/revoke access, ownership transfer, shared collections display). The implementation follows the existing MicroObjects layered architecture with new projects for Domain, Aggregator, and Adapter layers for collections.

## Technical Context

**Language/Version**: C# .NET 9.0 (backend), TypeScript/React 19 (frontend)
**Primary Dependencies**: HotChocolate (GraphQL), Auth0 JWT, Apollo Client, Material-UI, Newtonsoft.Json
**Storage**: Azure Cosmos DB (new Collections container + updated UserCards/UserWishlistCards/UserSetCards containers)
**Testing**: MSTest with AwesomeAssertions (backend), component tests (frontend)
**Target Platform**: Azure Container Apps (backend), browser SPA (frontend)
**Project Type**: Web application (full-stack: .NET GraphQL API + React SPA)
**Performance Goals**: Standard web app expectations; collection switching < 2 interactions; sharing operations < 5 seconds user-perceived
**Constraints**: Backward compatible with existing card operations (collectionId optional, defaults to user's default collection); Cosmos DB partition strategy must support efficient owner-based and authorized-user-based queries
**Scale/Scope**: Existing user base; new Collections Cosmos container at 400 RU/s; 8 new backend project libraries; ~15 new frontend components/contexts; ~100+ new unit tests

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I. MicroObjects Architecture | PASS | All new entities follow interface-first pattern. Each concept (Collection, AuthorizedUser, roles) represented as explicit types. No enums (roles stored as strings with validation). |
| II. Layered Architecture Flow | PASS | Full 7-layer flow: App (GraphQL) → Entry → Shared → Domain → Aggregator → Adapter → Infrastructure. Entity types follow ArgEntity → ItrEntity → XfrEntity → ExtEntity → OufEntity → OutEntity. |
| III. Test-First Development | PASS | MSTest + AwesomeAssertions. Fakes for all services. TypeWrapper for private constructors. Self-contained tests. |
| IV. Null Boundary Guards | PASS | Validators at Entry layer check for null on all collection inputs (CollectionId, Name, Type, UserId). Interior code assumes non-null. |
| V. Scope and Access Control | PASS | Public scope only in Apis folders. Operators public (Cosmos requirement). All service implementations internal. |
| VI. Code Style Consistency | PASS | File-scoped namespaces, sealed/abstract classes, ConfigureAwait(false), init setters, no comments, MUI sx props on frontend. |
| VII. NoArgsEntity Pattern | PASS | Used for parameter-less queries (e.g., GetMyCollections uses authenticated user context, not NoArgsEntity since it needs ClaimsPrincipal). |

**Gate Result**: PASS - No violations. Proceed to Phase 0.

**Post-Phase 1 Re-check**: PASS - Data model, GraphQL contracts, and research decisions all conform to constitution principles. Entity flow follows ArgEntity → ItrEntity → XfrEntity → ExtEntity → OufEntity → OutEntity. GraphQL types follow three-part union pattern. No enums used (roles/types as validated strings). All new projects follow per-domain separation pattern.

## Project Structure

### Documentation (this feature)

```text
specs/001-collection-identity/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
│   └── graphql-schema.md
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
src/
├── App.MtgDiscovery.GraphQL/              # GraphQL API (existing, modified)
│   ├── Entities/
│   │   ├── Args/Collections/              # NEW: Collection ArgEntities
│   │   └── Types/Collections/             # NEW: Collection GraphQL types
│   ├── Mutations/
│   │   └── CollectionMutationMethods.cs   # NEW: Collection mutations
│   ├── Queries/
│   │   └── CollectionQueryMethods.cs      # NEW: Collection queries
│   └── Schemas/
│       └── CollectionSchemaExtensions.cs  # NEW: Schema registration
│
├── Lib.Shared.DataModels/                 # Shared interfaces (existing, modified)
│   └── Entities/
│       ├── Args/                          # NEW: ICreateCollectionArgEntity, etc.
│       ├── Itrs/                          # NEW: ICollectionItrEntity, etc.
│       ├── Oufs/                          # NEW: ICollectionOufEntity, etc.
│       └── Outs/                          # NEW: ICollectionOutEntity, etc.
│
├── Lib.MtgDiscovery.Entry/               # Entry layer (existing, modified)
│   ├── Commands/Collections/              # NEW: Collection entry services
│   │   ├── Validators/                    # NEW: Collection validators
│   │   └── Entities/                      # NEW: Collection entity implementations
│   └── Entities/Outs/Collections/         # NEW: Collection OutEntity classes
│
├── Lib.Domain.Collections/               # NEW PROJECT: Collection domain logic
│   ├── Apis/                              # Public interfaces
│   ├── Commands/                          # Command domain services
│   ├── Queries/                           # Query domain services
│   └── Authorization/                     # Authorization service
│
├── Lib.Aggregator.Collections/           # NEW PROJECT: Collection aggregation
│   ├── Apis/                              # Public interfaces
│   ├── Commands/                          # Command aggregator services
│   ├── Queries/                           # Query aggregator services
│   └── Entities/                          # OufEntity implementations
│
├── Lib.Adapter.Collections/              # NEW PROJECT: Collection adapter abstraction
│   ├── Apis/                              # Public interfaces
│   ├── Commands/                          # Command adapters
│   └── Queries/                           # Query adapters
│
├── Lib.Adapter.Scryfall.Cosmos/          # Cosmos infrastructure (existing, modified)
│   ├── Apis/
│   │   ├── CosmosItems/
│   │   │   ├── CollectionExtEntity.cs            # NEW: Collection Cosmos document
│   │   │   ├── AuthorizedUserExtEntity.cs        # NEW: Nested authorized user
│   │   │   ├── UserCardExtEntity.cs              # MODIFIED: Add collection_id
│   │   │   ├── UserWishlistCardExtEntity.cs      # MODIFIED: Add collection_id
│   │   │   └── UserSetCardExtEntity.cs           # MODIFIED: Add collection_id
│   │   └── Operators/
│   │       ├── Gophers/CollectionGopher.cs       # NEW
│   │       ├── Scribes/CollectionScribe.cs       # NEW
│   │       └── Inquisitors/CollectionsInquisitor.cs # NEW
│   └── Cosmos/Containers/
│       ├── CollectionsCosmosContainer.cs         # NEW
│       └── Definitions/CollectionsCosmosContainerDefinition.cs # NEW
│
├── Lib.Domain.User/                      # User domain (existing, modified)
│   └── Commands/                          # Hook default collection creation
│
├── Lib.Aggregator.User/                  # User aggregation (existing, modified)
│   └── Commands/                          # Hook default collection creation
│
├── Cli.MtgDiscovery.DataMigration/       # CLI migration tool (existing, modified)
│   └── Collections/                       # NEW: Migration scripts
│
└── [Test projects - mirrors above with .Tests suffix]
    ├── Lib.Domain.Collections.Tests/      # NEW
    ├── Lib.Aggregator.Collections.Tests/  # NEW
    ├── Lib.Adapter.Collections.Tests/     # NEW
    └── Lib.MtgDiscovery.Entry.Tests/      # MODIFIED: Add collection tests

client/
├── src/
│   ├── contexts/
│   │   ├── CollectionManagementContext.tsx  # NEW: Collection state management
│   │   ├── CollectionContext.tsx            # MODIFIED: Use active collection
│   │   └── WishlistContext.tsx              # MODIFIED: Use active collection
│   ├── components/
│   │   ├── atoms/shared/
│   │   │   ├── CollectionSelector.tsx       # NEW
│   │   │   └── CollectionBadge.tsx          # NEW
│   │   ├── molecules/shared/
│   │   │   ├── CollectionCard.tsx           # NEW
│   │   │   └── SharedCollectionCard.tsx     # NEW
│   │   └── organisms/
│   │       ├── CreateCollectionDialog.tsx   # NEW
│   │       ├── GrantAccessDialog.tsx        # NEW
│   │       └── AccessListDialog.tsx         # NEW
│   ├── pages/
│   │   ├── CollectionsPage.tsx              # NEW
│   │   └── UserProfilePage.tsx             # NEW (user ID display)
│   └── graphql/
│       ├── queries/
│       │   ├── getMyCollections.graphql     # NEW
│       │   ├── getAccessibleCollections.graphql # NEW
│       │   ├── getCollection.graphql        # NEW
│       │   ├── getCollectionAccessList.graphql # NEW
│       │   └── getSharedCollections.graphql # NEW
│       └── mutations/
│           ├── createCollection.graphql     # NEW
│           ├── renameCollection.graphql     # NEW
│           ├── deleteCollection.graphql     # NEW
│           ├── updateCollectionVisibility.graphql # NEW
│           ├── grantCollectionAccess.graphql # NEW
│           ├── revokeCollectionAccess.graphql # NEW
│           └── transferCollectionOwnership.graphql # NEW
└── [generated/ updated via npm run codegen]
```

**Structure Decision**: Follows the existing web application pattern with full-stack .NET backend + React frontend. New backend projects (`Lib.Domain.Collections`, `Lib.Aggregator.Collections`, `Lib.Adapter.Collections`) follow the established per-domain project separation. Cosmos infrastructure stays in the shared `Lib.Adapter.Scryfall.Cosmos` project. Frontend follows atomic design with domain-organized components.

## Complexity Tracking

No constitution violations to justify. The addition of 3 new backend library projects (Domain, Aggregator, Adapter for Collections) follows the established per-domain pattern used by UserCards, UserSetCards, UserWishlistCards, etc.
