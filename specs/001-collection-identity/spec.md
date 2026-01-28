# Feature Specification: Collection Identity Architecture

**Feature Branch**: `001-collection-identity`
**Created**: 2026-01-27
**Status**: Draft
**Input**: User description: "Transition from implicit 1:1 User:Collection model to first-class Collection entities with independent identity, multi-collection support, sharing/authorization, and collection lifecycle management."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Default Collection Auto-Creation (Priority: P1)

When a new user registers, the system automatically creates a private default collection for them. This collection serves as the implicit target for all card operations, preserving backward compatibility with the existing user experience. Users do not need to take any action; their existing workflow of adding/removing cards continues to work seamlessly.

**Why this priority**: This is the foundation for all collection functionality. Without a default collection tied to each user, existing card operations would break and no other collection features can function.

**Independent Test**: Can be fully tested by registering a new user and verifying a default collection exists, then adding cards without specifying a collection and confirming they land in the default collection.

**Acceptance Scenarios**:

1. **Given** a new user completes registration, **When** registration succeeds, **Then** a private default collection is automatically created with the user as owner.
2. **Given** an existing user adds a card without specifying a collection, **When** the card is saved, **Then** it is associated with the user's default collection.
3. **Given** a user has a default collection, **When** they attempt to delete it, **Then** the system prevents deletion.
4. **Given** existing users with card data predating collections, **When** a data migration runs, **Then** all existing cards are associated with each user's newly created default collection with private visibility.

---

### User Story 2 - Create and Manage Custom Collections (Priority: P2)

An authenticated user can create additional named collections beyond their default. Each collection has a name, a type (custom, cube, or trade), and a visibility setting (private or public, defaulting to private). Users can view all their collections, select which one is active, and switch between them. The active collection determines which cards are displayed and which collection receives newly added cards.

**Why this priority**: Multi-collection support is the core differentiator of this feature. Without it, the system remains a single-collection experience.

**Independent Test**: Can be tested by creating a custom collection, switching to it as active, adding cards, then switching back to the default collection and confirming each collection shows only its own cards.

**Acceptance Scenarios**:

1. **Given** an authenticated user, **When** they create a collection with name "Trade Binder", type "trade", and no visibility specified, **Then** a new private collection is created and appears in their collection list.
2. **Given** a user with multiple collections, **When** they select a different collection as active, **Then** all card views and operations target the newly selected collection.
3. **Given** a user creating a collection, **When** they provide a name that already exists in their collections, **Then** the system rejects the creation with an appropriate message.
4. **Given** a user creating a collection, **When** they provide a name exceeding 100 characters or use the reserved name "default", **Then** the system rejects the creation.
5. **Given** a user, **When** they view their collections page, **Then** they see all collections they own, each showing name, type badge, visibility indicator, and card count.
6. **Given** a user switches active collection, **When** they close and reopen the application, **Then** the previously selected active collection is restored.
7. **Given** a collection owner on the collections management page, **When** they rename a collection to a valid unique name, **Then** the collection name is updated and reflected everywhere it appears.
8. **Given** a collection owner on the collections management page, **When** they attempt to change the collection type, **Then** the system does not offer this option (type is immutable after creation).

---

### User Story 3 - Collection Visibility Control (Priority: P3)

Collection owners can control whether their collections are private (only authorized users can view) or public (any authenticated user with the collection ID can view the contents). Visibility defaults to private. Only the owner of a collection can change its visibility setting.

**Why this priority**: Visibility is a core access-control concept that enables public sharing scenarios (e.g., showing off a cube list) without requiring explicit grants.

**Independent Test**: Can be tested by creating a collection, changing it to public, then having a different authenticated user access the collection by ID and confirm read access without an explicit grant.

**Acceptance Scenarios**:

1. **Given** an owner of a private collection, **When** they change visibility to public, **Then** any authenticated user can view the collection contents via collection ID.
2. **Given** a public collection, **When** a non-authorized authenticated user accesses it by ID, **Then** they can view but cannot add or remove cards.
3. **Given** a public collection, **When** the owner changes visibility back to private, **Then** non-authorized users lose view access, but existing authorized users retain their granted roles.
4. **Given** a user who is an admin (not the owner), **When** they attempt to change visibility, **Then** the system denies the operation.

---

### User Story 4 - Grant and Revoke Collection Access (Priority: P4)

The collection owner and admins can grant other users access to their collections by specifying a user ID and a role (editor or viewer). Editors can add and remove cards. Viewers can only view the collection. The owner and admins can view who has access and revoke access from any non-admin, non-owner user. Users who have been granted access can also remove themselves from a shared collection.

**Why this priority**: Sharing is a key collaborative feature but requires the collection entity (P1) and multi-collection support (P2) to be meaningful.

**Independent Test**: Can be tested by granting a second user editor access, confirming they can add a card, then revoking access and confirming they can no longer add cards.

**Acceptance Scenarios**:

1. **Given** the collection owner or an admin, **When** they grant editor access to another user by user ID, **Then** that user can add and remove cards from the collection.
2. **Given** the collection owner or an admin, **When** they grant viewer access to a user, **Then** that user can view the collection but cannot modify it.
3. **Given** the collection owner or an admin, **When** they view the access list, **Then** they see all authorized users with their roles, grant timestamps, and who granted them.
4. **Given** the collection owner or an admin, **When** they revoke an editor or viewer's access, **Then** that user can no longer access the collection.
5. **Given** a user with granted access, **When** they remove themselves from a shared collection, **Then** they lose access and the collection disappears from their "Shared With Me" list.
6. **Given** the collection owner or an admin, **When** they attempt to grant the "admin" role, **Then** the system rejects the operation (admin role is only assigned when ownership is transferred).
7. **Given** an editor or viewer, **When** they attempt to grant or revoke access on a collection, **Then** the system denies the operation.

---

### User Story 5 - Collection Deletion (Priority: P5)

The owner of a non-default collection can permanently delete it. Deletion removes the collection and all associated card data (collection cards, wishlist cards, and set tracking cards). Shared access is implicitly removed when the collection ceases to exist. The default collection cannot be deleted.

**Why this priority**: Lifecycle management is important but less frequently used than creation and sharing.

**Independent Test**: Can be tested by creating a collection, adding cards, then deleting it and confirming all associated data is removed.

**Acceptance Scenarios**:

1. **Given** an owner of a non-default collection, **When** they delete the collection, **Then** the collection document and all associated card records are permanently removed.
2. **Given** a user who is an admin (not the owner), **When** they attempt to delete the collection, **Then** the system denies the operation.
3. **Given** a user's default collection, **When** deletion is attempted, **Then** the system prevents it.
4. **Given** a shared collection with multiple authorized users, **When** the owner deletes it, **Then** all users lose access immediately.

---

### User Story 6 - Transfer Collection Ownership (Priority: P6)

The owner of a non-default collection can transfer ownership to another user who already has authorized access to that collection. After transfer, the previous owner becomes an admin (retaining full operational access including granting/revoking access, but losing the ability to delete, transfer, or change visibility). The default collection cannot be transferred.

**Why this priority**: Ownership transfer is an advanced management feature needed less frequently than basic sharing.

**Independent Test**: Can be tested by granting a user access, transferring ownership to them, then verifying the new owner can delete/transfer while the previous owner cannot.

**Acceptance Scenarios**:

1. **Given** an owner of a non-default collection with an authorized user, **When** they transfer ownership to that authorized user, **Then** the target becomes the new owner.
2. **Given** ownership has been transferred, **When** the previous owner accesses the collection, **Then** they retain admin access (can add/remove cards and grant/revoke editor/viewer access) but cannot delete, transfer, or change visibility.
3. **Given** an owner, **When** they attempt to transfer to a user not in the authorized users list, **Then** the system rejects the transfer.
4. **Given** a user's default collection, **When** ownership transfer is attempted, **Then** the system prevents it.

---

### User Story 7 - View Shared Collections (Priority: P7)

Users can see collections that have been shared with them in a dedicated "Shared With Me" section. Shared collections display the owner's information, the user's role, and the collection details. Users can select a shared collection as their active collection to view or (if editor) manage its cards.

**Why this priority**: This is a read-only complement to the sharing feature (P4) and enhances discoverability of shared content.

**Independent Test**: Can be tested by granting a user access to a collection, then logging in as that user and confirming the collection appears in "Shared With Me" with correct role information.

**Acceptance Scenarios**:

1. **Given** a user who has been granted access to another user's collection, **When** they view their collections page, **Then** the shared collection appears in a "Shared With Me" section showing the owner's ID and the user's role.
2. **Given** a user with editor access to a shared collection, **When** they select it as active, **Then** they can add and remove cards from it.
3. **Given** a user with viewer access to a shared collection, **When** they select it as active, **Then** they can view cards but cannot modify the collection.

---

### User Story 8 - Collection Selection in Application Header (Priority: P8)

Authenticated users see a collection selector in the application header. The selector shows all accessible collections (owned and shared) and allows quick switching. The active collection is highlighted. A "New Collection" option is available directly from the selector dropdown.

**Why this priority**: This is a UI convenience layer on top of core functionality that already works via the collections page.

**Independent Test**: Can be tested by verifying the selector appears in the header, shows all collections with type badges, and allows switching without navigating to the collections page.

**Acceptance Scenarios**:

1. **Given** an authenticated user with multiple collections, **When** they click the collection selector in the header, **Then** they see all their owned and shared collections with type badges.
2. **Given** a user viewing the collection selector, **When** they select a different collection, **Then** the active collection changes immediately and card views update.
3. **Given** a user viewing the collection selector, **When** they click "New Collection", **Then** a creation dialog opens.

---

### Edge Cases

- What happens when a user tries to access a collection that has been deleted while they had it selected as active? The system falls back to the user's default collection.
- What happens when a user is removed from a shared collection they currently have active? The system falls back to the user's default collection.
- What happens when a user grants access to a user ID that does not exist in the system? The system returns an appropriate error message.
- What happens when two users attempt to modify the same collection simultaneously? Standard optimistic concurrency controls apply; the later write receives a conflict notification.
- What happens when a user has no default collection (data corruption)? Card operations that require a collection should fail gracefully rather than silently operating without a collection context.
- What happens when visibility is changed from public to private? Existing explicit viewer/editor grants are preserved; only implicit public read access is removed.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST create a private default collection for every newly registered user automatically during the registration process.
- **FR-002**: System MUST associate all existing user card data (collection cards, wishlist cards, set tracking cards) with a default collection via a one-time data migration.
- **FR-003**: System MUST allow authenticated users to create additional named collections with a name (max 100 characters, unique per user, "default" reserved), type (custom, cube, or trade), and visibility (private or public, defaulting to private).
- **FR-003a**: System MUST allow collection owners to rename a collection after creation (same validation rules: max 100 characters, unique per user, "default" reserved). Collection type is immutable after creation.
- **FR-004**: System MUST support collection-scoped card operations, accepting an optional collection identifier that defaults to the user's default collection when not provided.
- **FR-005**: System MUST enforce role-based authorization on all collection operations using four distinct roles: owner (the single owner of the collection, not a grantable role), admin (full operational access plus grant/revoke), editor (add/remove cards), viewer (read-only).
- **FR-006**: System MUST distinguish between the owner (can delete, transfer, change visibility; exists outside the authorized users list as the root of the collection) and admin (full operational access including granting/revoking editor and viewer access, but cannot delete, transfer, or change visibility).
- **FR-007**: System MUST allow the collection owner and admins to grant editor or viewer access to other users by user ID. The admin role cannot be directly granted; it is only assigned when ownership is transferred.
- **FR-008**: System MUST allow the collection owner and admins to revoke access from editor and viewer authorized users.
- **FR-009**: System MUST allow users to remove themselves from collections shared with them.
- **FR-010**: System MUST allow owners to permanently delete non-default collections, removing the collection and all associated card data.
- **FR-011**: System MUST prevent deletion of a user's default collection.
- **FR-012**: System MUST allow the owner to transfer ownership of non-default collections to existing authorized users, with the previous owner becoming an admin.
- **FR-013**: System MUST prevent transfer of default collections.
- **FR-014**: System MUST support private visibility (only authorized users can access) and public visibility (any authenticated user can view by collection ID but cannot edit without an explicit role grant).
- **FR-015**: System MUST allow only the owner to change a collection's visibility setting.
- **FR-016**: System MUST persist the user's active collection selection across sessions.
- **FR-017**: System MUST provide a collection selector in the application header for authenticated users.
- **FR-018**: System MUST provide a dedicated collections management page showing owned and shared collections.
- **FR-019**: System MUST display appropriate notifications for all sharing operations (grant, revoke, self-removal).
- **FR-020**: System MUST maintain backward compatibility with existing card operations by defaulting to the user's default collection when no collection is specified.
- **FR-021**: System MUST validate that collection names are unique within a single user's owned collections.
- **FR-022**: System MUST provide a way for users to view and copy their own user ID for sharing purposes.

### Key Entities

- **Collection**: A named container for card data. Has an identity (ID), an owner, a name (mutable by owner), a type (default, custom, cube, trade; immutable after creation), a visibility setting (private, public), a list of authorized users with roles, and timestamps for creation and last update. Each user has exactly one default collection (named "My Collection" at creation) that cannot be deleted or transferred.

- **Authorized User**: A record of a user's access to a specific collection. Contains the user ID, their role (admin, editor, viewer), when access was granted, and who granted it. The role hierarchy is: admin > editor > viewer. The admin role is only assigned via ownership transfer (not directly grantable). The collection owner exists outside this list as the root-level identity on the collection.

- **User Card (updated)**: An existing card record extended with a collection identifier, linking each card to a specific collection rather than just a user.

- **User Wishlist Card (updated)**: An existing wishlist card record extended with a collection identifier.

- **User Set Card (updated)**: An existing set tracking record extended with a collection identifier.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can create a new collection in under 30 seconds through the collection creation dialog.
- **SC-002**: Switching between collections via the header selector takes no more than 2 user interactions (click selector, click collection).
- **SC-003**: All existing card operations continue to work without user-visible changes when no collection is explicitly specified (100% backward compatibility).
- **SC-004**: Data migration successfully associates all pre-existing card data with default collections without data loss.
- **SC-005**: Authorization correctly prevents unauthorized access in 100% of test scenarios (owner, admin, editor, viewer, and unauthenticated access patterns).
- **SC-006**: Granting or revoking collection access completes with user confirmation within 5 seconds.
- **SC-007**: The collections management page loads and displays all owned and shared collections within standard application response expectations.
- **SC-008**: Active collection selection persists across browser sessions with 100% reliability.
- **SC-009**: Collection deletion removes all associated data (collection document, collection cards, wishlist cards, set tracking cards) completely.
- **SC-010**: Ownership transfer correctly updates the owner while assigning the previous owner the admin role.

## Clarifications

### Session 2026-01-27

- Q: Can collection name and type be modified after creation? → A: Name can be changed (on the collections management page); type is immutable after creation.
- Q: Who can grant and revoke collection access, and how should the role formerly called "co-owner" be distinguished from the owner? → A: Rename the "owner" role in authorized users to "admin". The owner is the root-level identity on the collection (not manageable, only transferable). Admins can grant/revoke editor and viewer access. The admin role is only assigned via ownership transfer. Role hierarchy: admin > editor > viewer.

## Assumptions

- Users are already authenticated via the existing authentication system (Auth0) and have a registered account before any collection operations.
- The "user ID" used for sharing is the existing system-generated GUID, not any external identifier. There is no user search functionality initially; users must exchange IDs out-of-band.
- There is no enforced limit on the number of collections a user can create.
- There is no browsing or searching of public collections; users must know the collection ID to access a public collection.
- Changing visibility from public to private does not automatically revoke existing explicit access grants.
- The data migration is a one-time, manually triggered, idempotent operation.
- Rate limiting on sharing operations is a future enhancement, not part of this initial implementation.
- No access expiration dates on shared access.
- No bulk sharing operations; users must be granted access individually.
- Collection statistics beyond card count are out of scope for this feature.
