# Collection Identity Architecture

## Status: Future Enhancement

## Problem Statement

Currently, a user's collection is implicitly identified by their `userId`. This creates a 1:1 coupling between User and Collection that limits functionality.

**Current Model:**
```
User (userId) ←→ UserCards (partitioned by userId)
             ←→ UserWishlistCards (partitioned by userId)
             ←→ UserSetCards (partitioned by userId)
```

The `userId` effectively IS the collection identifier.

## Proposed Model

Introduce a first-class `Collection` entity with its own identity:

```
Collection
  - id: string (unique collection identifier)
  - ownerId: string (user who created it)
  - name: string ("Main Collection", "Trade Binder", "Cube", etc.)
  - type: enum (collection, wishlist, trade, cube)
  - visibility: enum (private, public)
  - authorizedUsers: AuthorizedUser[]

AuthorizedUser
  - userId: string
  - role: enum (owner, editor, viewer)

User
  - id: string
  - collections: Collection[] (ones they own or have access to)
```

## Benefits

### Multiple Collections Per User
- Personal collection
- Trade binder
- Cube(s)
- Deck-specific collections
- "Want to buy" vs "Want to trade for" wishlists

### Shared Collections
- Multiple people managing one collection (e.g., store inventory)
- Family/household shared collection
- Playgroup cube management

### Delegated Access
- Let someone help manage your collection
- Temporary access for data entry help
- Read-only sharing for showing collection to others

### Clear Authorization Model
```
mutation AddCardToCollection(collectionId: ID!, ...) {
  // Validation: Is authenticated user in collection.authorizedUsers with editor+ role?
}
```

### Ownership Model

There are two distinct ownership concepts:

- **Primary Owner** (`owner_id` on the Collection document): The single user who has ultimate authority over the collection. Only the primary owner can:
  - Delete the collection
  - Transfer ownership
  - Change visibility

- **Co-Owner** (role "owner" in `authorized_users` array): A user granted full operational access. Co-owners can:
  - CRUD cards in the collection
  - Grant/revoke access to other users
  - Everything except delete, transfer, or change visibility

When ownership is transferred, the previous primary owner becomes a co-owner (retains "owner" role in `authorized_users`).

### Collection Deletion

Collections are deleted via hard delete — the Collection document and all associated data are removed:

- Only the **primary owner** (`owner_id`) can delete a collection
- The **default collection cannot be deleted**
- Deletion removes:
  - The Collection document itself
  - All UserCards documents with matching `collection_id`
  - All UserWishlistCards documents with matching `collection_id`
  - All UserSetCards documents with matching `collection_id`
- Shared access is implicitly removed (the collection no longer exists)

### Ownership Transfer

Ownership of non-default collections can be transferred:

- Only the **primary owner** can initiate a transfer
- The **default collection cannot be transferred**
- The target must be an existing authorized user on the collection
- On transfer:
  - `owner_id` changes to the new owner
  - The previous owner retains an "owner" role entry in `authorized_users` (becomes co-owner)
  - The new owner's `authorized_users` entry is updated to "owner" role

### Collection Visibility

Collections have a `visibility` field that controls who can view them:

- **`private`** (default): Only users listed in `authorizedUsers` can see the collection. This is where the `viewer` role is meaningful — the owner grants explicit view access to specific users.
- **`public`**: Any authenticated user can view the collection and its contents if they have the collection ID. There is no browsing or listing of public collections — users must know the collection ID to access it. The `viewer` role is still used for explicit grants (e.g., showing the collection in a user's "Shared With Me" list), but read access is not restricted to authorized users only.

**Visibility + Role interaction:**
```
private collection:
  - owner:  full control (CRUD cards, manage access, change settings)
  - editor: add/remove cards
  - viewer: read-only access
  - others:  no access (collection is invisible)

public collection:
  - owner:  full control (CRUD cards, manage access, change settings)
  - editor: add/remove cards
  - viewer: read-only (appears in "Shared With Me")
  - others:  read-only (can view via collection ID, but not in "Shared With Me")
```

**Default behavior:**
- New collections default to `private`
- Only the primary owner can change visibility
- Changing from `public` to `private` does not revoke existing `viewer` grants

## Migration Considerations

### Data Migration
1. Create new `Collections` Cosmos container
2. For each existing user with cards:
   - Create default "Main Collection" with user as owner
3. Update `UserCards` documents to include `collectionId`
4. Update `UserWishlistCards` documents to include `collectionId`

### API Changes
- All collection mutations need `collectionId` parameter
- New queries: `myCollections`, `collectionById`, `sharedWithMe`, `accessibleCollections`
- New mutations: `createCollection`, `deleteCollection`, `updateCollectionVisibility`, `transferCollectionOwnership`, `grantCollectionAccess`, `revokeCollectionAccess`

### Frontend Changes
- Collection picker/switcher UI
- Collection management page
- Sharing/permissions UI
- Update all collection mutation calls to include `collectionId`

### Partition Strategy
- Collections container: partition by `ownerId`
- UserCards: partition by `collectionId` (breaking change from `userId`)
- Consider cross-partition query implications

## Interim State

Until this is implemented:
- `userId` continues to serve as implicit collection identifier
- Frontend sends `userId` in mutation args
- Backend validates `userId` matches JWT
- 1:1 User:Collection relationship enforced by design

## Related Files

Current implementation using userId as collection identifier:
- `client/src/contexts/CollectionContext.tsx` - sends `userId: userProfile.id`
- `client/src/contexts/WishlistContext.tsx` - sends `userId: userProfile.id`
- `src/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/AuthUserMatchesUserIdValidator.cs`
- `src/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/UserWishlistCards/AuthUserMatchesUserIdWishlistValidator.cs`

## Priority

Low-Medium - Current system works for single-user collections. This becomes important when:
- Users request multiple collections
- Sharing features are requested
- Store/business use cases emerge
