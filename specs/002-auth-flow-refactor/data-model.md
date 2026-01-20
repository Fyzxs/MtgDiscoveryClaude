# Data Model: Authentication Flow Refactoring

**Feature Branch**: `002-auth-flow-refactor`
**Date**: 2026-01-18
**Status**: Complete

## Entities

### 1. User (Backend)

Represents a registered user with identity from Auth0.

**Storage**: Azure Cosmos DB via `UserInfoExtEntity`

| Field | Type | Description | Validation |
|-------|------|-------------|------------|
| userId | string (GUID) | Unique identifier generated from Auth0 subject using namespace GUID | Required, valid GUID format |
| displayName | string | User's display name from Auth0 profile | Required, non-empty |
| email | string | User's email address | Optional, valid email format if present |
| sourceId | string | Auth0 subject identifier (sub claim) | Required, non-empty |
| createdAt | DateTime | Timestamp of first registration | Required, UTC, immutable after creation |
| lastLoginAt | DateTime | Timestamp of most recent sign-in | Required, UTC, updated on each sync |

**Identity & Uniqueness:**
- `userId` is the primary key (generated deterministically from Auth0 `sub`)
- `sourceId` is the Auth0 subject, used to link to identity provider
- Partition key: `userId`

**Lifecycle:**
- Created on first authentication via SyncUser
- Updated (lastLoginAt) on each subsequent authentication
- Never deleted within this feature scope

### 2. UserSyncResponse (Backend Output)

Response from the SyncUser operation.

| Field | Type | Description |
|-------|------|-------------|
| userId | string | User's unique identifier |
| displayName | string | User's display name |
| email | string? | User's email address (optional) |
| createdAt | DateTime | When user was first registered |
| lastLoginAt | DateTime | When user last signed in |
| isFirstLogin | bool | True if this was user's first sync (registration) |

**Derivation:**
- `isFirstLogin = (createdAt == lastLoginAt)` OR explicit flag from sync operation

### 3. AuthState (Frontend)

Represents the current authentication status in the React application.

| Field | Type | Description |
|-------|------|-------------|
| status | AuthStatus | Current state in the auth state machine |
| user | UserProfile? | Current user profile (null if not authenticated) |
| isFirstLogin | bool | Whether this session is user's first sign-in |
| error | AuthError? | Current error state (null if no error) |

**State Machine (AuthStatus):**

```
                    ┌─────────────────┐
                    │  initializing   │
                    └────────┬────────┘
                             │
            ┌────────────────┴────────────────┐
            │                                 │
            ▼                                 ▼
   ┌─────────────────┐              ┌─────────────────┐
   │ unauthenticated │◄────────────│  authenticated  │
   └────────┬────────┘   (logout)   └────────▲────────┘
            │                                 │
            │ (login clicked)                 │ (sync success)
            ▼                                 │
   ┌─────────────────┐              ┌─────────────────┐
   │ authenticating  │─────────────►│    syncing     │
   └─────────────────┘ (callback)   └─────────────────┘
```

**Valid Transitions:**
- `initializing` → `unauthenticated` (no session found)
- `initializing` → `authenticated` (valid session + cached user)
- `unauthenticated` → `authenticating` (login initiated)
- `authenticating` → `syncing` (callback received)
- `syncing` → `authenticated` (backend sync successful)
- `syncing` → `unauthenticated` (backend sync failed)
- `authenticated` → `unauthenticated` (logout)

### 4. UserProfile (Frontend)

Client-side representation of the authenticated user.

| Field | Type | Description |
|-------|------|-------------|
| id | string | User's unique identifier (from backend userId) |
| displayName | string | User's display name |
| email | string? | User's email address |
| avatarUrl | string? | URL to user's avatar image (from Auth0) |
| createdAt | string | ISO timestamp of first registration |
| lastLoginAt | string | ISO timestamp of most recent sign-in |

### 5. AuthError (Frontend)

Error information for authentication failures.

| Field | Type | Description |
|-------|------|-------------|
| code | string | Error code for programmatic handling |
| message | string | Human-readable error message |

**Error Codes:**
- `auth_failed` - Auth0 authentication failed
- `sync_failed` - Backend user sync failed
- `network_error` - Network connectivity issue
- `session_expired` - Session expired, re-authentication required

---

## Entity Relationships

```
┌─────────────────────────────────────────────────────────────────┐
│                         Backend                                  │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Auth0 (sub claim)                                              │
│         │                                                        │
│         │ determines                                             │
│         ▼                                                        │
│  ┌──────────────┐                                               │
│  │     User     │ ──────────────────────────────────────────┐   │
│  │  (Cosmos DB) │                                            │   │
│  └──────────────┘                                            │   │
│         │                                                    │   │
│         │ SyncUser mutation                                  │   │
│         ▼                                                    │   │
│  ┌──────────────────┐                                        │   │
│  │ UserSyncResponse │ ─────────────────────────────────────┐ │   │
│  │ (isFirstLogin)   │                                      │ │   │
│  └──────────────────┘                                      │ │   │
│                                                             │ │   │
└─────────────────────────────────────────────────────────────┼─┼───┘
                                                              │ │
┌─────────────────────────────────────────────────────────────┼─┼───┐
│                         Frontend                            │ │   │
├─────────────────────────────────────────────────────────────┼─┼───┤
│                                                             │ │   │
│  ┌──────────────┐                                          │ │   │
│  │  AuthState   │◄─────────────────────────────────────────┘ │   │
│  │  (Context)   │                                            │   │
│  └──────┬───────┘                                            │   │
│         │ contains                                           │   │
│         ▼                                                    │   │
│  ┌──────────────┐                                            │   │
│  │ UserProfile  │◄───────────────────────────────────────────┘   │
│  │ (cached)     │ mapped from UserSyncResponse                   │
│  └──────────────┘                                                │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Data Flow

### User Sync Flow

```
1. Auth0 callback received (code exchange complete)
   │
2. SyncUser mutation called with JWT
   │
3. Backend extracts claims: sub, email, name, picture
   │
4. Query Cosmos for user by userId (derived from sub)
   │
   ├─► User NOT found:
   │   ├─ Create user with createdAt = now, lastLoginAt = now
   │   └─ Return isFirstLogin = true
   │
   └─► User found:
       ├─ Update lastLoginAt = now, preserve createdAt
       └─ Return isFirstLogin = false
   │
5. Frontend receives UserSyncResponse
   │
6. Map to UserProfile, store in AuthState context
   │
7. Show appropriate welcome message based on isFirstLogin
```

### Logout Flow

```
1. User clicks logout
   │
2. Clear Apollo cache (removes UserProfile from memory)
   │
3. Reset AuthState to initial state
   │
4. Call Auth0 logout (clears session cookies)
   │
5. Redirect to home page
   │
6. AuthState = unauthenticated
```

---

## Validation Rules

### Backend Validation (Entry Layer)

| Field | Rule | Error Message |
|-------|------|---------------|
| JWT | Must be present | "Authentication required" |
| sub claim | Must be non-empty | "Invalid authentication token" |
| userId derived | Must be valid GUID | "Invalid user identifier" |
| displayName | Must be non-empty | "Display name required" |

### Frontend Validation

| State | Validation | Action |
|-------|------------|--------|
| AuthState.status | Must be valid enum value | State machine enforces |
| UserProfile.id | Must match backend userId | Set from response |
| isFirstLogin | Must be boolean | Type system enforces |

---

## Migration Notes

### Existing Data Handling

- **Users without `createdAt`**: Set to `lastLoginAt` (if exists) or current time on first sync
- **Users without `lastLoginAt`**: Set to current time on first sync
- **Backward compatibility**: UPSERT handles missing fields gracefully
- **No data migration required**: Fields added incrementally as users sync

### Field Additions to UserInfoExtEntity

```csharp
// New fields (init with null handling for existing documents)
[JsonProperty("created_at")]
public DateTime CreatedAt { get; init; }

[JsonProperty("last_login_at")]
public DateTime LastLoginAt { get; init; }
```

Cosmos DB's flexible schema allows adding these fields without migration. Existing documents will have default values until updated by sync operation.
