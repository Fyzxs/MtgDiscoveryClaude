# GraphQL Schema Contracts: Collection Identity

**Phase**: 1 - Contracts | **Date**: 2026-01-27

## Types

### Enums (represented as string validation in MicroObjects, GraphQL enums for schema)

```graphql
enum CollectionType {
  DEFAULT
  CUSTOM
  CUBE
  TRADE
}

enum CollectionVisibility {
  PRIVATE
  PUBLIC
}

enum CollectionRole {
  ADMIN
  EDITOR
  VIEWER
}
```

### Object Types

```graphql
type Collection {
  id: ID!
  ownerId: ID!
  name: String!
  type: CollectionType!
  visibility: CollectionVisibility!
  isDefault: Boolean!
  authorizedUsers: [AuthorizedUser!]!
  createdAt: DateTime!
  updatedAt: DateTime!
}

type AuthorizedUser {
  userId: ID!
  role: CollectionRole!
  grantedAt: DateTime!
  grantedBy: ID!
}
```

### Response Union Types

```graphql
union CollectionResponse = CollectionSuccessResponse | FailureResponse
union CollectionsResponse = CollectionsSuccessResponse | FailureResponse
union AuthorizedUsersResponse = AuthorizedUsersSuccessResponse | FailureResponse

type CollectionSuccessResponse {
  data: Collection!
  status: StatusInfo!
}

type CollectionsSuccessResponse {
  data: [Collection!]!
  status: StatusInfo!
}

type AuthorizedUsersSuccessResponse {
  data: [AuthorizedUser!]!
  status: StatusInfo!
}
```

### Input Types

```graphql
input CreateCollectionInput {
  name: String!
  type: CollectionType!
  visibility: CollectionVisibility = PRIVATE
}

input RenameCollectionInput {
  collectionId: ID!
  name: String!
}

input UpdateCollectionVisibilityInput {
  collectionId: ID!
  visibility: CollectionVisibility!
}

input TransferCollectionOwnershipInput {
  collectionId: ID!
  targetUserId: ID!
}

input GrantCollectionAccessInput {
  collectionId: ID!
  targetUserId: ID!
  role: CollectionRole!
}

input RevokeCollectionAccessInput {
  collectionId: ID!
  targetUserId: ID!
}
```

## Queries

```graphql
extend type Query {
  # Get a single collection by ID (respects visibility: public collections viewable by any authenticated user)
  getCollection(collectionId: ID!): CollectionResponse! @authorize

  # Get all collections owned by the authenticated user
  myCollections: CollectionsResponse! @authorize

  # Get all collections accessible to the authenticated user (owned + shared)
  accessibleCollections: CollectionsResponse! @authorize

  # Get collections shared with the authenticated user (not owned)
  sharedCollections: CollectionsResponse! @authorize

  # Get the access list for a collection (owner and admins only)
  collectionAccessList(collectionId: ID!): AuthorizedUsersResponse! @authorize
}
```

## Mutations

```graphql
extend type Mutation {
  # Create a new collection for the authenticated user
  createCollection(args: CreateCollectionInput!): CollectionResponse! @authorize

  # Rename an existing collection (owner only)
  renameCollection(args: RenameCollectionInput!): CollectionResponse! @authorize

  # Update collection visibility (owner only)
  updateCollectionVisibility(args: UpdateCollectionVisibilityInput!): CollectionResponse! @authorize

  # Delete a non-default collection (owner only)
  deleteCollection(collectionId: ID!): CollectionResponse! @authorize

  # Transfer ownership to an authorized user (owner only, non-default)
  transferCollectionOwnership(args: TransferCollectionOwnershipInput!): CollectionResponse! @authorize

  # Grant access to a user (owner or admin, editor/viewer roles only)
  grantCollectionAccess(args: GrantCollectionAccessInput!): CollectionResponse! @authorize

  # Revoke access from a user (owner, admin, or self-removal)
  revokeCollectionAccess(args: RevokeCollectionAccessInput!): CollectionResponse! @authorize
}
```

## Modified Existing Mutations

```graphql
# Updated input - collectionId is optional, defaults to user's default collection
input AddCardToCollectionInput {
  cardId: ID!
  setId: ID!
  userId: ID!              # Deprecated: use collectionId
  collectionId: ID         # Optional: defaults to user's default collection
  userCardDetails: UserCardDetailsInput!
}

input AddCardToWishlistInput {
  cardId: ID!
  setId: ID!
  userId: ID!              # Deprecated: use collectionId
  collectionId: ID         # Optional: defaults to user's default collection
  userCardDetails: UserCardDetailsInput!
}
```

## HotChocolate Type Registration Pattern

Each response type requires three classes following the constitution's GraphQL Type Definition Pattern:

### Collection Response Types

```
CollectionResponseModelUnionType : UnionType
  → CollectionSuccessDataResponseModelType : ObjectType<CollectionSuccessDataResponseModel>
  → CollectionOutEntityType : ObjectType<CollectionOutEntity>
  → FailureResponseModelType (existing)

CollectionsResponseModelUnionType : UnionType
  → CollectionsSuccessDataResponseModelType : ObjectType<CollectionsSuccessDataResponseModel>
  → CollectionOutEntityType (shared)

AuthorizedUsersResponseModelUnionType : UnionType
  → AuthorizedUsersSuccessDataResponseModelType : ObjectType<AuthorizedUsersSuccessDataResponseModel>
  → AuthorizedUserOutEntityType : ObjectType<AuthorizedUserOutEntity>
```

### Schema Registration (in CollectionSchemaExtensions)

```
.AddType<CollectionResponseModelUnionType>()
.AddType<CollectionSuccessDataResponseModelType>()
.AddType<CollectionOutEntityType>()
.AddType<AuthorizedUserOutEntityType>()
.AddType<CollectionsResponseModelUnionType>()
.AddType<CollectionsSuccessDataResponseModelType>()
.AddType<AuthorizedUsersResponseModelUnionType>()
.AddType<AuthorizedUsersSuccessDataResponseModelType>()
```

## Frontend GraphQL Operations

### Queries

**getMyCollections.graphql**
```graphql
query GetMyCollections {
  myCollections {
    __typename
    ... on CollectionsSuccessResponse {
      data {
        id
        ownerId
        name
        type
        visibility
        isDefault
        authorizedUsers {
          userId
          role
          grantedAt
          grantedBy
        }
        createdAt
        updatedAt
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```

**getCollection.graphql**
```graphql
query GetCollection($collectionId: ID!) {
  getCollection(collectionId: $collectionId) {
    __typename
    ... on CollectionSuccessResponse {
      data {
        id
        ownerId
        name
        type
        visibility
        isDefault
        authorizedUsers {
          userId
          role
          grantedAt
          grantedBy
        }
        createdAt
        updatedAt
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```

### Mutations

**createCollection.graphql**
```graphql
mutation CreateCollection($args: CreateCollectionInput!) {
  createCollection(args: $args) {
    __typename
    ... on CollectionSuccessResponse {
      data {
        id
        ownerId
        name
        type
        visibility
        isDefault
        createdAt
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```

**grantCollectionAccess.graphql**
```graphql
mutation GrantCollectionAccess($args: GrantCollectionAccessInput!) {
  grantCollectionAccess(args: $args) {
    __typename
    ... on CollectionSuccessResponse {
      data {
        id
        authorizedUsers {
          userId
          role
          grantedAt
          grantedBy
        }
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```
