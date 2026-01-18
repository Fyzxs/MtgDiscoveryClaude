# Data Model: Authentication Flow Refactoring

**Feature**: Authentication Flow Refactoring
**Branch**: 001-auth-refactoring
**Date**: 2026-01-17
**Constitution Version**: 1.0.0

## Overview

This document defines the TypeScript interfaces, state transitions, and validation rules for the authentication flow refactoring. The model follows constitution's frontend requirements while translating MicroObjects principles (explicit types, immutability, boundary validation) to React/TypeScript patterns.

## Core Entities (TypeScript Interfaces)

### 1. StoredUserData

**Purpose**: User session data persisted in localStorage for returning user detection

**Location**: `client/src/utils/userStorage.ts`

**Interface**:
```typescript
export interface StoredUserData {
  // Auth0 Identity (required)
  sub: string;              // Auth0 subject identifier
  userId: string;           // Backend user ID from registration

  // Auth0 Profile (optional)
  email?: string;           // User email
  name?: string;            // User display name
  picture?: string;         // Avatar URL

  // Backend Data (optional)
  displayName?: string;     // Backend-chosen display name

  // Session Tracking (required)
  registeredAt: string;     // ISO 8601 timestamp of first registration
  lastLoginAt: string;      // ISO 8601 timestamp of most recent login
}
```

**Field Specifications**:

| Field | Type | Required | Validation | Constitution Principle |
|-------|------|----------|------------|------------------------|
| `sub` | string | Yes | Non-empty | Principle IV: Validate at boundary |
| `userId` | string | Yes | Non-empty | Principle IV: Validate at boundary |
| `email` | string | No | Valid email or undefined | Principle IV: Optional validated |
| `name` | string | No | Any string or undefined | Principle I: Explicit over implicit |
| `picture` | string | No | Valid URL or undefined | Type safety via TypeScript |
| `displayName` | string | No | Any string or undefined | Backend-controlled field |
| `registeredAt` | string | Yes | ISO 8601 format | Immutable after creation |
| `lastLoginAt` | string | Yes | ISO 8601 format | Updated on each login |

**Storage**: `localStorage['mtg-user-data']` (migrates to `STORAGE_KEYS.USER_DATA` in Phase 5)

**Lifecycle**:
- **Created**: After successful registration in `SignInRedirectPage.tsx`
- **Updated**: `lastLoginAt` updated on returning user login
- **Deleted**: On logout in `AuthButton.tsx`

**Constitution Alignment**:
- ✅ **Principle I** (Micro Objects → TypeScript): Explicit interface for every concept
- ✅ **Principle IV** (Null Boundary Guards): Validation at localStorage boundary (see Validation Rules)
- ✅ Immutability: Functions return new objects, never mutate stored data

### 2. AuthCallbackResult

**Purpose**: Routes users after Auth0 callback based on localStorage state

**Location**: `client/src/hooks/auth/useAuthCallback.ts`

**Interface**:
```typescript
type CallbackStatus = 'processing' | 'returning' | 'new-user' | 'error';

export interface AuthCallbackResult {
  status: CallbackStatus;   // Current authentication state
  redirectTo: string;       // Target route for navigation
}
```

**Status Values**:

| Status | redirectTo | User State | Action |
|--------|-----------|------------|--------|
| `processing` | `/` | Auth0 callback in progress | Show loading spinner |
| `returning` | `/` | localStorage entry found | Fast redirect to home |
| `new-user` | `/signin-redirect` | No localStorage entry | Navigate to registration |
| `error` | `/` | Authentication failed | Navigate to home (triggers re-auth) |

**Constitution Alignment**:
- ✅ **Principle I**: Explicit type for every state (not boolean flags)
- ✅ Type union enforces valid states at compile time
- ✅ Immutable - hook returns new object each render

### 3. TokenReadyState

**Purpose**: Component-local token availability status (replaces global subscription)

**Location**: `client/src/hooks/auth/useTokenReady.ts`

**Interface**:
```typescript
export interface TokenReadyState {
  isReady: boolean;      // Token successfully verified
  isWaiting: boolean;    // Token verification in progress
  error: string | null;  // Error message if verification failed
}
```

**State Combinations**:

| isReady | isWaiting | error | Meaning | User Experience |
|---------|-----------|-------|---------|-----------------|
| false | true | null | Verifying token | Loading indicator |
| true | false | null | Token ready | Proceed with GraphQL calls |
| false | false | null | Not authenticated | Don't call authenticated APIs |
| false | false | "..." | Token failed | Show error, retry button |

**Constitution Alignment**:
- ✅ **Principle II** (Layered Architecture → React): Hook layer abstracts Auth0 SDK
- ✅ Component-local state (no global state races)
- ✅ Explicit error messages (no silent failures)

## State Transitions

### User Authentication Flow

```
┌─────────────┐
│ Page Load   │
│ User Clicks │
│ Login       │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐
│ Auth0 Login Page    │
│ (External - Auth0)  │
└──────┬──────────────┘
       │
       ▼
┌─────────────────────┐
│ /auth/callback      │  ← NEW in Phase 2
│ (AuthCallbackPage)  │
│                     │
│ useAuthCallback()   │
└──────┬──────────────┘
       │
       ├──────────────────────┐
       │                      │
       ▼                      ▼
┌──────────────┐      ┌────────────────┐
│ localStorage │      │ NO localStorage│
│  EXISTS      │      │    ENTRY       │
│              │      │                │
│ sub matches  │      │  (New User)    │
│ (Returning)  │      └────────┬───────┘
└──────┬───────┘               │
       │                       ▼
       │              ┌─────────────────┐
       │              │ /signin-redirect│
       │              │                 │
       │              │ Wait for token  │
       │              │ REGISTER_USER   │
       │              │   GraphQL       │
       │              └────────┬────────┘
       │                       │
       │                       ▼
       │              ┌─────────────────┐
       │              │ saveUserData()  │
       │              │ to localStorage │
       │              └────────┬────────┘
       │                       │
       ▼                       ▼
┌──────────────────────────────┐
│  updateLastLogin()           │
│  Navigate to /               │
│  (< 1 second for returning)  │
└──────────────────────────────┘
```

### localStorage State Transitions

```
┌──────────────┐
│ NULL         │
│ (No entry -  │
│  new user)   │
└──────┬───────┘
       │
       │ [First Registration Complete]
       │
       ▼
┌──────────────────────────────┐
│ StoredUserData {             │
│   sub: "auth0|123456",       │
│   userId: "uuid-789",        │
│   email: "user@example.com", │
│   registeredAt: "2026-01-17",│
│   lastLoginAt: "2026-01-17"  │
│ }                            │
└──────┬───────────────────────┘
       │
       │ [Returning User Login]
       │
       ▼
┌──────────────────────────────┐
│ StoredUserData {             │
│   ...all fields same...,     │
│   lastLoginAt: "2026-01-18"  │← UPDATED (immutable pattern)
│ }                            │
└──────┬───────────────────────┘
       │
       │ [User Logs Out]
       │
       ▼
┌──────────────┐
│ NULL         │
│ (Cleared)    │
└──────────────┘
```

**Constitution Compliance**:
- ✅ Immutability: Update creates new object (`{ ...stored, lastLoginAt: newDate }`)
- ✅ No nulls in interior: `getStoredUserData()` returns `null` at boundary only
- ✅ Type safety: TypeScript enforces `StoredUserData` shape

## Validation Rules

### Input Validation (Principle IV: Null Boundary Guards)

#### 1. localStorage Read Validation

**Location**: `client/src/utils/userStorage.ts:getStoredUserData()`

**Type Guard Pattern** (Constitution-compliant):
```typescript
function isValidStoredUserData(data: unknown): data is StoredUserData {
  // Boundary validation - check for null/undefined
  if (typeof data !== 'object' || data === null) {
    return false;
  }

  const obj = data as Record<string, unknown>;

  // Required fields validation
  if (typeof obj.sub !== 'string' || obj.sub.length === 0) {
    return false;
  }

  if (typeof obj.userId !== 'string' || obj.userId.length === 0) {
    return false;
  }

  if (typeof obj.registeredAt !== 'string' || !isValidISODate(obj.registeredAt)) {
    return false;
  }

  if (typeof obj.lastLoginAt !== 'string' || !isValidISODate(obj.lastLoginAt)) {
    return false;
  }

  // Optional fields validation
  if (obj.email !== undefined && typeof obj.email !== 'string') {
    return false;
  }

  if (obj.name !== undefined && typeof obj.name !== 'string') {
    return false;
  }

  if (obj.picture !== undefined && typeof obj.picture !== 'string') {
    return false;
  }

  if (obj.displayName !== undefined && typeof obj.displayName !== 'string') {
    return false;
  }

  return true;
}

// Usage
export function getStoredUserData(): StoredUserData | null {
  try {
    const stored = localStorage.getItem(USER_STORAGE_KEY);

    // Boundary check #1: localStorage returns null if key doesn't exist
    if (stored === null) {
      return null;
    }

    // Boundary check #2: JSON.parse can throw or return invalid data
    const parsed: unknown = JSON.parse(stored);

    // Boundary check #3: Type guard validates structure
    if (!isValidStoredUserData(parsed)) {
      logger.warn('StoredUserData validation failed, clearing corrupted data');
      clearUserData(); // Clean up invalid data
      return null;
    }

    // Interior code: TypeScript knows this is StoredUserData
    return parsed;

  } catch (error) {
    // Boundary check #4: Parse errors
    logger.error('localStorage parse failed:', error);
    clearUserData();
    return null;
  }
}
```

**Error Handling Matrix**:

| Error Type | Action | Rationale |
|------------|--------|-----------|
| Missing key (`null`) | Return `null` | Treat as new user |
| Parse error | Clear + return `null` | Corrupted data, clean up |
| Missing required field | Clear + return `null` | Invalid data, clean up |
| Type mismatch | Clear + return `null` | Tampered data, clean up |
| Optional field wrong type | Clear + return `null` | Data integrity issue |

**Constitution Alignment**:
- ✅ **Principle IV**: Validators check null at boundary (localStorage)
- ✅ Type guards provide compile-time proof to TypeScript
- ✅ Interior code never sees invalid data

#### 2. Auth0 User Object Validation

**Location**: `client/src/components/pages/SignInRedirectPage.tsx:handleUserSetup()`

**Validation Pattern**:
```typescript
// Boundary validation from Auth0
if (user === undefined) {
  logger.error('Auth0 user undefined');
  setSetupStatus('error');
  return;
}

if (user.sub === undefined || user.sub.length === 0) {
  logger.error('Auth0 sub missing or empty');
  setSetupStatus('error');
  return;
}

// Safe to use - validated at boundary
const userData: StoredUserData = {
  sub: user.sub,                    // Non-null (validated above)
  userId: registrationResult.userId, // From GraphQL response
  email: user.email,                 // Optional, can be undefined
  name: user.name ?? user.email,     // Fallback pattern
  picture: user.picture,             // Optional
  displayName: registrationResult.displayName, // Optional
  registeredAt: new Date().toISOString(),
  lastLoginAt: new Date().toISOString(),
};

// Save to localStorage (validated structure)
saveUserData(userData);
```

**Constitution Alignment**:
- ✅ Early returns for validation failures (guard clauses)
- ✅ TypeScript strictNullChecks catches undefined access
- ✅ Explicit fallback for optional fields (`??` operator)

#### 3. Sub Mismatch Detection

**Purpose**: Detect when user cleared localStorage and logged in with different Auth0 account

**Validation**:
```typescript
export function isReturningUser(auth0Sub: string): boolean {
  const stored = getStoredUserData();

  // Boundary checks handled in getStoredUserData()
  if (stored === null) {
    return false; // No stored data = new user
  }

  // Sub must match
  if (stored.sub !== auth0Sub) {
    logger.warn('Sub mismatch - different Auth0 account, clearing old data');
    clearUserData(); // Clean up old user's data
    return false;
  }

  return true; // Same user, returning
}
```

**Constitution Alignment**:
- ✅ Boundary validation cascades from `getStoredUserData()`
- ✅ Explicit comparison (no implicit coercion)
- ✅ Clean up on mismatch (maintain data integrity)

## Performance Constraints

### 1. Returning User Redirect Time

**Goal**: < 1 second (vs current 3-5 seconds)

**Measurement Points**:
```typescript
// In SignInRedirectPage.tsx
const startTime = performance.now();

if (isReturningUser(user.sub ?? '')) {
  updateLastLogin();
  const elapsedMs = performance.now() - startTime;

  logger.debug(`Returning user redirect time: ${elapsedMs}ms`);

  // Should be < 100ms (localStorage is synchronous)
  if (elapsedMs > 1000) {
    logger.warn(`Slow returning user path: ${elapsedMs}ms`);
  }

  setSetupStatus('complete');
  navigate('/', { replace: true });
  return;
}
```

**Optimization**:
- ✅ localStorage read is synchronous (< 1ms)
- ✅ No GraphQL call needed for returning users
- ✅ No async operations before navigate

### 2. Cache Hit Rate

**Goal**: Eliminate redundant `GET_USER_INFO` calls for returning users

**Measurement**:
```typescript
// In apollo-client.ts type policy
userInfo: {
  merge(existing, incoming, { variables }) {
    logger.debug('Apollo cache merge', {
      hadExisting: existing !== undefined,
      hasIncoming: incoming !== undefined
    });

    // Prefer fresh data
    return incoming ?? existing;
  },
}
```

**Expected Behavior**:
- First render: Cache miss → GraphQL call
- Subsequent renders: Cache hit → No GraphQL call
- Background refresh via `cache-and-network` policy

## Migration Considerations

### Data Migration

**Status**: No migration required (additive only)

**Compatibility**:
1. **Existing users** (deployed before feature):
   - No localStorage entry → Full registration flow (creates entry)
   - Next login → Fast path (has localStorage)

2. **New users** (deployed after feature):
   - Registration → localStorage created → Fast path on next login

3. **Logout/login cycle**:
   - Logout → localStorage cleared
   - Next login → Full registration (backend idempotent)

**Rollback Strategy**:
```typescript
// Remove this block to rollback to old behavior
if (isReturningUser(user.sub ?? '')) {
  updateLastLogin();
  navigate('/');
  return;
}
// Falls through to original registration flow
```

**Constitution Alignment**:
- ✅ Additive changes (no breaking changes to existing code)
- ✅ Backend idempotency prevents data duplication
- ✅ Clear rollback path (single code block removal)

## Testing Data

### Valid Test Cases

```typescript
// Minimal valid returning user
const minimalUser: StoredUserData = {
  sub: "auth0|123456789",
  userId: "550e8400-e29b-41d4-a716-446655440000",
  registeredAt: "2026-01-15T10:00:00.000Z",
  lastLoginAt: "2026-01-17T08:00:00.000Z"
};

// Complete user profile
const completeUser: StoredUserData = {
  sub: "auth0|987654321",
  userId: "660e8400-e29b-41d4-a716-446655440001",
  email: "user@example.com",
  name: "Test User",
  picture: "https://example.com/avatar.jpg",
  displayName: "TestUser123",
  registeredAt: "2026-01-10T12:00:00.000Z",
  lastLoginAt: "2026-01-17T09:00:00.000Z"
};
```

### Invalid Test Cases (Should Return Null)

```typescript
// Missing required field (sub)
const missingSub = {
  userId: "770e8400-e29b-41d4-a716-446655440002",
  registeredAt: "2026-01-17T10:00:00.000Z",
  lastLoginAt: "2026-01-17T10:00:00.000Z"
};

// Wrong type for required field
const wrongType = {
  sub: 12345, // Should be string
  userId: "880e8400-e29b-41d4-a716-446655440003",
  registeredAt: "2026-01-17T10:00:00.000Z",
  lastLoginAt: "2026-01-17T10:00:00.000Z"
};

// Malformed JSON
const malformedJSON = "{ sub: 'missing-quotes', userId: }";

// Empty required field
const emptyRequired = {
  sub: "",  // Empty string not allowed
  userId: "990e8400-e29b-41d4-a716-446655440004",
  registeredAt: "2026-01-17T10:00:00.000Z",
  lastLoginAt: "2026-01-17T10:00:00.000Z"
};
```

## Summary

This data model provides:
- ✅ TypeScript interfaces for all entities (Principle I)
- ✅ Boundary validation with type guards (Principle IV)
- ✅ Immutable state transitions (React/TypeScript best practice)
- ✅ Clear error handling and edge case coverage
- ✅ Performance measurement points
- ✅ Constitution-compliant frontend patterns

**Next**: Generate component/hook contracts in `contracts/` directory.
