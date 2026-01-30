# Data Model: Collection Identity Architecture

**Phase**: 1 - Design | **Date**: 2026-01-27

## Entities

### Collection

The core new entity. Stored in the `Collections` Cosmos container, partitioned by `owner_id`.

| Field | Type | Required | Mutable | Notes |
|-------|------|----------|---------|-------|
| id | string (GUID) | Yes | No | Unique collection identifier |
| owner_id | string (GUID) | Yes | Yes (transfer only) | User ID of collection owner. Partition key. |
| name | string | Yes | Yes | Display name. Max 100 chars. Unique per owner. "default" reserved. |
| type | string | Yes | No | "default", "custom", "cube", "trade". Immutable after creation. |
| visibility | string | Yes | Yes (owner only) | "private" or "public". Defaults to "private". |
| is_default | bool | Yes | No | True for user's auto-created default collection. Named "My Collection" at creation. |
| authorized_users | array | Yes | Yes | Array of AuthorizedUser objects. Owner is NOT in this list. |
| created_at | string (ISO 8601) | Yes | No | Timestamp of creation. |
| updated_at | string (ISO 8601) | Yes | Yes | Timestamp of last modification. |

**Identity**: `id` (GUID)
**Partition Key**: `owner_id`
**Uniqueness Constraints**: `(owner_id, name)` must be unique. One `is_default = true` per `owner_id`.

### AuthorizedUser (embedded in Collection)

| Field | Type | Required | Mutable | Notes |
|-------|------|----------|---------|-------|
| user_id | string (GUID) | Yes | No | The authorized user's ID. |
| role | string | Yes | Yes | "admin", "editor", "viewer". |
| granted_at | string (ISO 8601) | Yes | No | When access was granted. |
| granted_by | string (GUID) | Yes | No | User ID who granted access. |

**Role Hierarchy**: admin > editor > viewer
**Constraints**: "admin" role is only assigned via ownership transfer, never via direct grant.

### UserCard (modified)

Existing entity with new field added.

| New Field | Type | Required | Default | Notes |
|-----------|------|----------|---------|-------|
| collection_id | string (GUID) | No | "" (empty string) | Links card to a collection. Empty for pre-migration data. |

### UserWishlistCard (modified)

Same modification as UserCard.

| New Field | Type | Required | Default | Notes |
|-----------|------|----------|---------|-------|
| collection_id | string (GUID) | No | "" (empty string) | Links wishlist card to a collection. |

### UserSetCard (modified)

Same modification as UserCard.

| New Field | Type | Required | Default | Notes |
|-----------|------|----------|---------|-------|
| collection_id | string (GUID) | No | "" (empty string) | Links set tracking to a collection. |

## Entity Layer Mapping

### Collection Entity Flow

```
GraphQL Input → App Layer:
  CreateCollectionArgEntity { Name, Type, Visibility }
  RenameCollectionArgEntity { CollectionId, Name }
  UpdateCollectionVisibilityArgEntity { CollectionId, Visibility }
  DeleteCollectionArgEntity { CollectionId }
  TransferCollectionOwnershipArgEntity { CollectionId, TargetUserId }
  GrantCollectionAccessArgEntity { CollectionId, TargetUserId, Role }
  RevokeCollectionAccessArgEntity { CollectionId, TargetUserId }

App → Entry Layer (validation + mapping):
  ICreateCollectionArgEntity → ICollectionItrEntity
  IRenameCollectionArgEntity → IRenameCollectionItrEntity
  IUpdateCollectionVisibilityArgEntity → IUpdateCollectionVisibilityItrEntity
  IDeleteCollectionArgEntity → IDeleteCollectionItrEntity
  ITransferCollectionOwnershipArgEntity → ITransferCollectionOwnershipItrEntity
  IGrantCollectionAccessArgEntity → IGrantCollectionAccessItrEntity
  IRevokeCollectionAccessArgEntity → IRevokeCollectionAccessItrEntity

Entry → Domain → Aggregator (business rules + orchestration):
  ICollectionItrEntity (same through layers)

Aggregator → Adapter (data transformation):
  ICollectionItrEntity → CollectionXfrEntity
  CollectionXfrEntity → CollectionExtEntity (Cosmos write)
  CollectionExtEntity → ICollectionOufEntity (Cosmos read/return)

Adapter → Entry (return path):
  ICollectionOufEntity → CollectionOutEntity

Entry → App (GraphQL output):
  CollectionOutEntity → GraphQL Collection type
```

### Shared Layer Interfaces (Lib.Shared.DataModels)

**Args (input from GraphQL):**
- `ICreateCollectionArgEntity` { Name, Type, Visibility }
- `IRenameCollectionArgEntity` { CollectionId, Name }
- `IUpdateCollectionVisibilityArgEntity` { CollectionId, Visibility }
- `IDeleteCollectionArgEntity` { CollectionId }
- `ITransferCollectionOwnershipArgEntity` { CollectionId, TargetUserId }
- `IGrantCollectionAccessArgEntity` { CollectionId, TargetUserId, Role }
- `IRevokeCollectionAccessArgEntity` { CollectionId, TargetUserId }

**Itrs (internal transfer):**
- `ICollectionItrEntity` { CollectionId, OwnerId, Name, Type, Visibility, IsDefault, AuthorizedUsers, CreatedAt, UpdatedAt }
- `IAuthorizedUserItrEntity` { UserId, Role, GrantedAt, GrantedBy }
- `IRenameCollectionItrEntity` { CollectionId, OwnerId, Name }
- `IUpdateCollectionVisibilityItrEntity` { CollectionId, OwnerId, Visibility }
- `IDeleteCollectionItrEntity` { CollectionId, OwnerId }
- `ITransferCollectionOwnershipItrEntity` { CollectionId, CurrentOwnerId, TargetUserId }
- `IGrantCollectionAccessItrEntity` { CollectionId, GrantorUserId, TargetUserId, Role }
- `IRevokeCollectionAccessItrEntity` { CollectionId, RevokerUserId, TargetUserId }

**Oufs (output from aggregator/domain):**
- `ICollectionOufEntity` { CollectionId, OwnerId, Name, Type, Visibility, IsDefault, AuthorizedUsers, CreatedAt, UpdatedAt }
- `IAuthorizedUserOufEntity` { UserId, Role, GrantedAt, GrantedBy }

**Outs (output to GraphQL):**
- `ICollectionOutEntity` { CollectionId, OwnerId, Name, Type, Visibility, IsDefault, AuthorizedUsers, CreatedAt, UpdatedAt }
- `IAuthorizedUserOutEntity` { UserId, Role, GrantedAt, GrantedBy }

## Validation Rules

### Create Collection

| Field | Rule | Error |
|-------|------|-------|
| Name | Not null, not empty | "Collection name is required" |
| Name | Max 100 characters | "Collection name must be 100 characters or fewer" |
| Name | Not "default" (case-insensitive) | "Collection name 'default' is reserved" |
| Name | Unique per owner | "A collection with this name already exists" |
| Type | Not null, not empty | "Collection type is required" |
| Type | One of: "custom", "cube", "trade" | "Invalid collection type" |
| Visibility | If provided: "private" or "public" | "Invalid visibility value" |

### Rename Collection

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| Name | Same rules as Create | (same errors) |
| Auth | User is owner | "Only the collection owner can rename" |

### Update Visibility

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| Visibility | "private" or "public" | "Invalid visibility value" |
| Auth | User is owner | "Only the collection owner can change visibility" |

### Delete Collection

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| Auth | User is owner | "Only the collection owner can delete" |
| Collection | is_default is false | "Default collection cannot be deleted" |

### Transfer Ownership

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| TargetUserId | Valid GUID, exists in authorized_users | "Target user must be an authorized user" |
| Auth | User is owner | "Only the collection owner can transfer ownership" |
| Collection | is_default is false | "Default collection cannot be transferred" |

### Grant Access

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| TargetUserId | Valid GUID, user exists in system | "User not found" |
| Role | "editor" or "viewer" | "Can only grant editor or viewer roles" |
| Auth | User is owner or admin | "Only the owner or admins can grant access" |

### Revoke Access

| Field | Rule | Error |
|-------|------|-------|
| CollectionId | Valid GUID | "Invalid collection ID" |
| TargetUserId | Valid GUID | "Invalid target user ID" |
| TargetUser | Not owner, not admin (unless revoker is owner) | "Cannot revoke owner or admin access" |
| Auth | User is owner, admin, or self-removing | "Not authorized to revoke access" |

## State Transitions

### Collection Lifecycle

```
[Created] → (active, private by default)
  ├── Rename → (name updated)
  ├── Change Visibility → (private ↔ public)
  ├── Grant Access → (authorized_users grows)
  ├── Revoke Access → (authorized_users shrinks)
  ├── Transfer Ownership → (owner_id changes, previous owner becomes admin)
  └── Delete → [Deleted] (hard delete: collection + all associated cards removed)
```

### Default Collection (restricted lifecycle)

```
[Created on registration] → (active, private, is_default=true)
  ├── Rename → (name updated)
  ├── Change Visibility → (private ↔ public)
  ├── Grant Access → (authorized_users grows)
  ├── Revoke Access → (authorized_users shrinks)
  ├── Transfer Ownership → BLOCKED
  └── Delete → BLOCKED
```

## Cosmos Container Specifications

### Collections Container (NEW)

```json
{
  "id": "Collections",
  "partitionKey": {
    "paths": ["/owner_id"],
    "kind": "Hash"
  },
  "indexingPolicy": {
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [{ "path": "/*" }],
    "excludedPaths": [{ "path": "/\"_etag\"/?" }],
    "compositeIndexes": [
      [
        { "path": "/owner_id", "order": "ascending" },
        { "path": "/created_at", "order": "descending" }
      ]
    ]
  },
  "throughput": 400
}
```

### Modified Containers (schema only)

- **UserCards**: Add `collection_id` field. No partition key change.
- **UserWishlistCards**: Add `collection_id` field. No partition key change.
- **UserSetCards**: Add `collection_id` field. No partition key change.
