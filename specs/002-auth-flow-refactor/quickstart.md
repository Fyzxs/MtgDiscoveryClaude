# Quickstart: Authentication Flow Refactoring

**Feature Branch**: `002-auth-flow-refactor`
**Date**: 2026-01-18

## Prerequisites

- .NET 9.0 SDK installed
- Node.js 18+ installed
- Azure Cosmos DB emulator running (or Azure connection)
- Auth0 application configured

## Getting Started

### 1. Switch to Feature Branch

```bash
git checkout 002-auth-flow-refactor
```

### 2. Build Backend

```bash
cd src
dotnet build MtgDiscoveryVibe.sln
```

### 3. Run Backend Tests

```bash
dotnet test MtgDiscoveryVibe.sln
```

### 4. Start Backend API

```bash
dotnet run --project App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
```

API will be available at `https://localhost:5001/graphql`

### 5. Install Frontend Dependencies

```bash
cd client
npm install
```

### 6. Generate GraphQL Types

```bash
npm run codegen
```

### 7. Start Frontend Development Server

```bash
npm run dev
```

Frontend will be available at `http://localhost:5173`

---

## Key Files to Review

### Backend Changes

| File | Purpose |
|------|---------|
| `Lib.Adapter.Scryfall.Cosmos/.../UserInfoExtEntity.cs` | User entity with timestamps |
| `Lib.Adapter.Scryfall.Cosmos/.../UserInfoScribe.cs` | Sync logic with isFirstLogin |
| `Lib.Adapter.User/.../RegisterUserAdapter.cs` | Returns extended response |
| `Lib.MtgDiscovery.Entry/.../RegisterUserEntryService.cs` | Maps response with isFirstLogin |
| `App.MtgDiscovery.GraphQL/.../UserMutationMethods.cs` | GraphQL mutation handler |

### Frontend Changes

| File | Purpose |
|------|---------|
| `client/src/types/auth.ts` | Auth type definitions |
| `client/src/contexts/AuthStateContext.tsx` | Auth state machine |
| `client/src/contexts/ToastContext.tsx` | Toast notifications |
| `client/src/hooks/auth/useAuthCallback.ts` | Callback handler |
| `client/src/components/pages/AuthCallbackPage.tsx` | OAuth callback page |
| `client/src/components/auth/ProtectedRoute.tsx` | Route guard |
| `client/src/graphql/apollo-client.ts` | Simplified token handling |
| `client/src/graphql/mutations/user.ts` | SYNC_USER mutation |

---

## Testing the Auth Flow

### Test 1: New User Registration

1. Clear browser data / use incognito mode
2. Navigate to `http://localhost:5173`
3. Click "Sign In"
4. Complete Auth0 login with a NEW email
5. **Expected**: See "Welcome to MtgDiscovery, [name]!" toast
6. **Verify**: User record created in Cosmos DB with `isFirstLogin: true` response

### Test 2: Returning User Sign-In

1. Using the same browser session from Test 1
2. Click "Sign Out"
3. Click "Sign In" again with the same account
4. **Expected**: See "Welcome back, [name]!" toast
5. **Verify**: `isFirstLogin: false` in response, `lastLoginAt` updated

### Test 3: Session Persistence

1. Sign in successfully
2. Refresh the page
3. **Expected**: Still authenticated, no sign-in required
4. **Verify**: Auth state transitions: initializing → authenticated

### Test 4: Sign Out

1. While authenticated, click "Sign Out"
2. **Expected**: Redirected to home page as unauthenticated user
3. **Verify**: Apollo cache cleared, no user data visible

### Test 5: Protected Route

1. While NOT authenticated, navigate to a protected route (e.g., `/collection`)
2. **Expected**: Redirected to Auth0 login
3. After login, **Expected**: Redirected back to `/collection`

---

## GraphQL Testing

### Test SyncUser Mutation (Insomnia/Postman)

```graphql
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
```

**Headers Required:**
```
Authorization: Bearer <your-auth0-access-token>
```

**Expected Response (New User):**
```json
{
  "data": {
    "registerUserInfo": {
      "__typename": "UserRegistrationSuccessResponse",
      "data": {
        "userId": "a1b2c3d4-...",
        "displayName": "John Doe",
        "email": "john@example.com",
        "createdAt": "2026-01-18T12:00:00Z",
        "lastLoginAt": "2026-01-18T12:00:00Z",
        "isFirstLogin": true
      },
      "status": {
        "message": "Success",
        "statusCode": 200
      }
    }
  }
}
```

**Expected Response (Returning User):**
```json
{
  "data": {
    "registerUserInfo": {
      "__typename": "UserRegistrationSuccessResponse",
      "data": {
        "userId": "a1b2c3d4-...",
        "displayName": "John Doe",
        "email": "john@example.com",
        "createdAt": "2026-01-15T10:00:00Z",
        "lastLoginAt": "2026-01-18T12:00:00Z",
        "isFirstLogin": false
      },
      "status": {
        "message": "Success",
        "statusCode": 200
      }
    }
  }
}
```

---

## Common Issues

### Issue: "Missing Refresh Token" Error

**Cause**: Auth0 session expired or invalid
**Solution**: Clear browser cookies and localStorage, sign in again

### Issue: GraphQL Types Not Generated

**Cause**: Schema changed but codegen not run
**Solution**: Run `npm run codegen` after any backend schema changes

### Issue: "Invalid authentication token"

**Cause**: JWT claims missing or invalid
**Solution**: Check Auth0 configuration, ensure audience matches `api://mtg-discovery`

### Issue: Toast Not Appearing

**Cause**: ToastProvider not in component tree
**Solution**: Ensure ToastProvider wraps the component using useToast

---

## Auth0 Configuration Checklist

Ensure these settings in Auth0 Dashboard:

- [ ] Application Type: Single Page Application
- [ ] Allowed Callback URLs includes: `http://localhost:5173/auth/callback`
- [ ] Allowed Logout URLs includes: `http://localhost:5173`
- [ ] Allowed Web Origins includes: `http://localhost:5173`
- [ ] Refresh Token Rotation: Enabled
- [ ] API Audience: `api://mtg-discovery`

---

## Implementation Order

For detailed task breakdown, see `tasks.md` (generated by `/speckit.tasks`).

**Recommended order:**
1. Backend: Add timestamps to entity (Phase 1)
2. Backend: Update scribe with sync logic (Phase 1)
3. Backend: Update layers to return isFirstLogin (Phase 1)
4. Frontend: Create auth types and state machine (Phase 2)
5. Frontend: Simplify token management (Phase 3)
6. Frontend: Update GraphQL mutation (Phase 4)
7. Frontend: Create callback page (Phase 5)
8. Frontend: Add toast system (Phase 6)
9. Cleanup: Remove old code (Phase 7)
