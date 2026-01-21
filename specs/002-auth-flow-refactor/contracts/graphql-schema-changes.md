# GraphQL Schema Changes: Authentication Flow Refactoring

**Feature Branch**: `002-auth-flow-refactor`
**Date**: 2026-01-18

## Overview

This document describes the GraphQL schema changes required for the authentication flow refactoring. The primary change is extending the existing `registerUserInfo` mutation response to include additional fields.

## Schema Changes

### Mutation: registerUserInfo (Updated Response)

The existing mutation signature remains the same. Only the response type is extended.

```graphql
type Mutation {
  """
  Synchronizes the authenticated user with the backend.
  Creates the user if they don't exist (first login).
  Updates lastLoginAt if they do exist (returning user).
  Returns isFirstLogin flag to enable appropriate welcome messages.

  Requires: JWT authentication (Auth0)
  """
  registerUserInfo: RegisterUserInfoResponse!
}
```

### Union Type: RegisterUserInfoResponse (Unchanged)

```graphql
"""
Union type for user registration/sync response.
Either success with user data or failure with error.
"""
union RegisterUserInfoResponse =
  | UserRegistrationSuccessResponse
  | FailureResponse
```

### Type: UserRegistrationSuccessResponse (Updated)

```graphql
"""
Successful user sync response with extended user data.
"""
type UserRegistrationSuccessResponse {
  """User sync data including first login indicator"""
  data: UserSyncData!

  """Operation status"""
  status: StatusInfo!
}
```

### Type: UserSyncData (NEW - replaces previous data structure)

```graphql
"""
User data returned from sync operation.
"""
type UserSyncData {
  """Unique user identifier (GUID)"""
  userId: ID!

  """User's display name from Auth0 profile"""
  displayName: String!

  """User's email address (optional)"""
  email: String

  """Timestamp of user's first registration (ISO 8601)"""
  createdAt: DateTime!

  """Timestamp of user's most recent sign-in (ISO 8601)"""
  lastLoginAt: DateTime!

  """
  True if this is the user's first sync (new user).
  False if the user already existed (returning user).
  Use this to determine welcome message.
  """
  isFirstLogin: Boolean!
}
```

### Type: FailureResponse (Unchanged)

```graphql
"""
Failure response with error details.
"""
type FailureResponse {
  """Error status information"""
  status: StatusInfo!
}
```

### Type: StatusInfo (Unchanged)

```graphql
"""
Status information for operation results.
"""
type StatusInfo {
  """Human-readable status message"""
  message: String!

  """HTTP-style status code"""
  statusCode: Int!
}
```

---

## Frontend Mutation

### SYNC_USER Mutation (TypeScript)

```typescript
import { gql } from '@apollo/client';

export const SYNC_USER = gql`
  mutation SyncUser {
    registerUserInfo {
      __typename
      ... on UserRegistrationSuccessResponse {
        data {
          userId
          displayName
          email
          createdAt
          lastLoginAt
          isFirstLogin
        }
        status {
          message
          statusCode
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
`;
```

### Generated Types (Expected after codegen)

```typescript
// After running npm run codegen, these types should be generated:

export type SyncUserMutation = {
  __typename?: 'Mutation';
  registerUserInfo:
    | {
        __typename: 'UserRegistrationSuccessResponse';
        data: {
          __typename?: 'UserSyncData';
          userId: string;
          displayName: string;
          email?: string | null;
          createdAt: string;
          lastLoginAt: string;
          isFirstLogin: boolean;
        };
        status: {
          __typename?: 'StatusInfo';
          message: string;
          statusCode: number;
        };
      }
    | {
        __typename: 'FailureResponse';
        status: {
          __typename?: 'StatusInfo';
          message: string;
          statusCode: number;
        };
      };
};
```

---

## Backend Implementation Notes

### HotChocolate Type Registration

The following types must be registered in the GraphQL schema:

```csharp
// In schema configuration
.AddType<UserRegistrationResponseModelUnionType>()      // Union type
.AddType<UserRegistrationSuccessDataResponseModelType>() // Success response type
.AddType<UserSyncDataType>()                             // User data type (NEW)
.AddType<FailureResponseModelType>()                     // Failure type
.AddType<StatusInfoType>()                               // Status type
```

### Response Model Structure

```csharp
// Success response data model
public sealed class UserSyncOutEntity
{
    public string UserId { get; init; }
    public string DisplayName { get; init; }
    public string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLoginAt { get; init; }
    public bool IsFirstLogin { get; init; }
}
```

---

## Breaking Changes

**None**. This is a backward-compatible change:

1. The mutation name remains `registerUserInfo`
2. The union type structure remains the same
3. New fields are added to the success response
4. Existing clients that don't request new fields will continue to work

---

## Migration Path

1. **Backend**: Add new fields to response entity and GraphQL types
2. **Frontend**: Update mutation to request new fields
3. **Codegen**: Run `npm run codegen` to generate new TypeScript types
4. **Usage**: Update callback hook to use `isFirstLogin` for welcome messages

---

## Deprecation Notes

The following will be deprecated in future cleanup:

| Item | Deprecation Plan |
|------|------------------|
| `GET_USER_INFO` query | Remove after AuthStateContext provides user data |
| Old callback route (`/signin-redirect`) | Remove after transition period verification |
| localStorage user data | Remove immediately (security concern) |
