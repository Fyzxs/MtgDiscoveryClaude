# User Sealed Products Testing Guide

## Overview
This guide helps you test the User Sealed Products collection tracking feature.

## Prerequisites
1. GraphQL server running on `https://localhost:65203/graphql`
2. JWT Bearer token (Auth0) - set as `{{BEARER_TOKEN}}` in your HTTP client
3. User must be registered (run user registration mutation first if needed)

## Testing Workflow

### 1. Get Available Sealed Products
**File**: `getSealedProductsBySetCodeWithUserQuantity.http`

Run the authenticated query to get sealed products for a set (e.g., "mkm"):
```graphql
query GetSealedProductsBySetCode($args: GetSealedProductsBySetCodeArgEntityInput!) {
  sealedProductsBySetCode(args: $args) {
    data {
      uuid          # <- Copy this for the next step
      setId         # <- Copy this too
      name
      category
      userQuantity  # <- Should be 0 if not in collection yet
    }
  }
}
```

Variables:
```json
{
  "args": {
    "setCode": "mkm",
    "collectionId": "your-user-id"  // Same value as your userId
  }
}
```

**Note**: The `collectionId` is typically the same as your `userId`. The backend validates that the authenticated user (from JWT token) has permission to access the specified collection.

### 2. Add Product to Collection
**File**: `addUserSealedProduct.http`

Use the `uuid` and `setId` from step 1:
```graphql
mutation AddUserSealedProduct($args: AddUserSealedProductInput!) {
  addUserSealedProduct(args: $args) {
    data {
      productUuid
      count       # <- Should return the new total count
    }
  }
}
```

Variables:
```json
{
  "args": {
    "productUuid": "uuid-from-step-1",
    "setId": "setId-from-step-1",
    "collectionId": "your-user-id",  // Same as your userId
    "countDelta": 1
  }
}
```

### 3. Verify Collection Updated
**File**: `getSealedProductsBySetCodeWithUserQuantity.http`

Run the same query from step 1 again. The product you added should now show `userQuantity: 1`.

### 4. Test Other Operations

**Add Multiple**:
```json
{
  "args": {
    "productUuid": "same-uuid",
    "setId": "same-setId",
    "collectionId": "your-user-id",
    "countDelta": 5
  }
}
```
Result: `count` should increase by 5

**Remove Some**:
```json
{
  "args": {
    "productUuid": "same-uuid",
    "setId": "same-setId",
    "collectionId": "your-user-id",
    "countDelta": -2
  }
}
```
Result: `count` should decrease by 2

**Remove All**:
Use negative of current count to remove completely:
```json
{
  "args": {
    "productUuid": "same-uuid",
    "setId": "same-setId",
    "collectionId": "your-user-id",
    "countDelta": -999
  }
}
```
Result: Product removed from collection (document deleted), `userQuantity` returns to 0

## Expected Behavior

### Delta-Based Updates
- `countDelta: 1` → Add 1 to current count
- `countDelta: 5` → Add 5 to current count
- `countDelta: -1` → Remove 1 from current count
- `countDelta: -X` (where X >= current count) → Remove product entirely

### Merge Logic
The backend automatically:
1. Reads current count
2. Adds the delta
3. If new count > 0: Updates the count
4. If new count <= 0: Deletes the product from collection

### Authentication
- **Unauthenticated queries**: `userQuantity` always returns 0
- **Authenticated queries**: `userQuantity` populated from user's collection
- **Mutations**: Always require authentication (401 if no Bearer token)
- **Collection Authorization**: The `collectionId` parameter identifies which collection to access/modify. The backend validates that the authenticated user (from JWT) has permission to access the specified collection. In most cases, `collectionId` equals your `userId`.

## Recommended Test Sets

### MKM (Murders at Karlov Manor)
```json
{ "setCode": "mkm" }
```

### DSK (Duskmourn)
```json
{ "setCode": "dsk" }
```

### BLB (Bloomburrow)
```json
{ "setCode": "blb" }
```

## Common Product Categories
When testing, you'll see products with these categories:
- `box` - Booster Boxes
- `bundle` - Bundles
- `pack` - Individual Booster Packs
- `prerelease_pack` - Prerelease Kits
- `commander_deck` - Commander Decks
- `starter_deck` - Starter Decks

## Troubleshooting

### "Failed to query user sealed products"
- Check that the JWT token is valid
- Verify the user exists in the database

### "Product not found"
- The productUuid or setId is invalid
- Run the query first to get valid UUIDs

### userQuantity always shows 0
- Make sure you're including the `Authorization: Bearer {{BEARER_TOKEN}}` header
- Verify the token is valid and not expired

### Mutation returns "Unauthorized"
- The mutation requires authentication
- Check that `{{BEARER_TOKEN}}` is set correctly in your HTTP client

## Files Reference
- `addUserSealedProduct.http` - Add/remove sealed products from collection
- `getSealedProductsBySetCodeWithUserQuantity.http` - Query sealed products with user collection data
- `getUserSealedProducts.http` - Testing guide and example workflow
