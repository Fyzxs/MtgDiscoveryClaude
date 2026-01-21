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

## Migration Considerations

### Data Migration
1. Create new `Collections` Cosmos container
2. For each existing user with cards:
   - Create default "Main Collection" with user as owner
   - Create default "Wishlist" collection if they have wishlist items
3. Update `UserCards` documents to include `collectionId`
4. Update `UserWishlistCards` documents to include `collectionId`

### API Changes
- All collection mutations need `collectionId` parameter
- New queries: `myCollections`, `collectionById`, `sharedWithMe`
- New mutations: `createCollection`, `shareCollection`, `removeCollectionAccess`

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
