# Research: Collection Identity Architecture

**Phase**: 0 - Research | **Date**: 2026-01-27

## R1: Cosmos DB Partition Strategy for Collections Container

**Decision**: Partition Collections container by `owner_id`.

**Rationale**: The most frequent query pattern is "get all collections owned by user X" (myCollections). Partitioning by `owner_id` makes this an efficient in-partition query. The less frequent cross-partition query "get collections where user X is in authorized_users" (accessibleCollections / sharedCollections) is acceptable given the expected low volume of shared collections per user.

**Alternatives Considered**:
- Partition by `id` (collection ID): Efficient for single-collection lookups but would make "my collections" a cross-partition fan-out query. Rejected because "my collections" is the most common query.
- Hierarchical partition key `(owner_id, id)`: Over-engineered for current scale. Can be added later if needed.

## R2: Collection ID on Existing Card Containers

**Decision**: Add `collection_id` field to `UserCardExtEntity`, `UserWishlistCardExtEntity`, and `UserSetCardExtEntity`. Default to empty string for backward compatibility with existing documents.

**Rationale**: Cosmos DB documents without the `collection_id` field will deserialize with `string.Empty` via `init` setter default. The migration script will backfill existing documents. New documents will always have a `collection_id`. Queries will filter by `collection_id` when provided, or fall back to returning all cards for the user's default collection.

**Alternatives Considered**:
- Separate containers per collection: Rejected — would require dynamic container creation and vastly complicate the architecture.
- Composite partition key `(user_id, collection_id)`: Breaking change to existing partition strategy. Rejected for backward compatibility. Could be considered for a future major version.

## R3: Owner vs Authorized Users Data Model

**Decision**: The owner is stored as `owner_id` (a top-level field on the Collection document). Authorized users are stored in an `authorized_users` array embedded within the Collection document. The owner does NOT appear in the `authorized_users` array.

**Rationale**: The owner is a distinct concept from authorized users (see spec Clarification Q2). The owner exists outside the authorized users list as the root-level identity. This makes ownership checks simple (`owner_id == userId`) and avoids confusion between the owner and the "admin" role. On ownership transfer, the previous owner gets an "admin" entry added to `authorized_users`, and `owner_id` is updated to the new owner.

**Alternatives Considered**:
- Owner also in `authorized_users` with special role: Creates ambiguity about which is the source of truth. Rejected per spec clarification.
- Separate ownership document: Over-engineered for a single field. Rejected.

## R4: Role Enforcement Architecture

**Decision**: Create a `CollectionAuthorizationService` in `Lib.Domain.Collections/Authorization/` that encapsulates all role-checking logic. Entry-layer validators call this service to check permissions before allowing operations.

**Rationale**: Centralizing authorization logic in one service prevents duplication across validators and ensures consistent enforcement. The service fetches the collection, checks `owner_id` and `authorized_users`, and returns boolean results for each access level. This follows the existing pattern where validators delegate to services for complex checks.

**Alternatives Considered**:
- Inline role checks in each validator: Would duplicate collection fetching and role logic. Rejected.
- Middleware/attribute-based authorization: HotChocolate supports policy-based auth, but the collection-specific role check requires loading the collection document, which is better done in the service layer. Rejected for this use case.

## R5: Default Collection Creation Hook Point

**Decision**: Hook into the user registration flow at the aggregator layer. After the user adapter returns a successful registration, the user aggregator calls the collection adapter to create the default collection.

**Rationale**: The aggregator layer is the correct place for orchestrating multiple adapter calls (user creation + collection creation). This follows the existing pattern where aggregators coordinate between different adapters. The entry/domain layers remain focused on validation and business rules. The `isFirstLogin` flag from the adapter tells the aggregator whether to create a default collection.

**Alternatives Considered**:
- Create collection in the adapter layer alongside user creation: Mixes concerns (user persistence + collection persistence in one adapter). Rejected.
- Separate API call from frontend: Would create a race condition window where a user exists without a default collection. Rejected.
- Domain event/message: Over-engineered for a synchronous operation within the same request. Rejected.

## R6: Frontend Collection State Management

**Decision**: Create a new `CollectionManagementContext` that manages collection list, active collection, and CRUD operations. Existing `CollectionContext` (card operations) and `WishlistContext` will read the active collection ID from `CollectionManagementContext`.

**Rationale**: Separating collection management (CRUD, selection) from card operations (add/remove cards) follows the existing separation of concerns in the context architecture. The `CollectionManagementContext` provides `activeCollection` which other contexts consume. Active collection ID is persisted to `localStorage` for cross-session persistence.

**Alternatives Considered**:
- Merge into existing CollectionContext: Would overload an already complex context with collection management state. Rejected.
- Redux/Zustand: Adds a new state management dependency. Rejected since React Context is already the established pattern.

## R7: GraphQL Response Type Pattern

**Decision**: Follow the existing three-part union type pattern for all collection responses: `CollectionResponseModelUnionType`, `CollectionSuccessDataResponseModelType`, and `CollectionOutEntityType`.

**Rationale**: The constitution (Section VII) and codebase patterns require union types for all responses. The three-part pattern (union type class, success response type class, entity type class) is mandated by the GraphQL Development Standards in the constitution.

**Alternatives Considered**: None — this is a non-negotiable pattern per constitution.

## R8: Data Migration Strategy

**Decision**: Use the existing `Cli.MtgDiscovery.DataMigration` project to add a migration script. The migration is idempotent, manually triggered, and processes users sequentially: (1) create default collection if not exists, (2) update all UserCards/UserWishlistCards/UserSetCards with collection_id.

**Rationale**: The existing migration CLI provides the infrastructure for running one-time data operations. Idempotency ensures safe re-runs. Sequential processing per user avoids cross-partition transaction issues. The migration should be run after deploying the schema changes but before deploying the frontend that depends on collections.

**Alternatives Considered**:
- Lazy migration (migrate on first access): Adds complexity to every read path. Rejected.
- Cosmos DB change feed trigger: Over-engineered for a one-time operation. Rejected.

## R9: Collection Name Mutability

**Decision**: Collection name is mutable by the owner. A new `RenameCollectionAsync` entry service validates the new name (same rules as creation: max 100 chars, unique per owner, "default" reserved) and updates the collection document.

**Rationale**: Per spec clarification Q1, names can be changed on the collections management page. Type remains immutable after creation.

**Alternatives Considered**: None — this was a direct specification decision.

## R10: Admin Role Assignment

**Decision**: The "admin" role cannot be directly granted via the grant access API. It is only assigned when ownership is transferred (the previous owner receives the "admin" role in `authorized_users`). Grantable roles are limited to "editor" and "viewer".

**Rationale**: Per spec clarification Q2, the admin role provides elevated privileges (grant/revoke access) that should not be arbitrarily assignable. Tying it to ownership transfer creates a clear, auditable path to admin status.

**Alternatives Considered**:
- Allow owners to grant admin role: Would weaken the ownership model and make it unclear who has elevated privileges. Rejected per spec clarification.
