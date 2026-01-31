# Contexts Folder

## Purpose
React Context-based state management for app-wide concerns. Each context encapsulates a domain (auth, collections, user, notifications) with a reducer pattern for state machines.

## Organization

```
contexts/
  ├─ AuthStateContext.tsx           (Auth state machine)
  ├─ CollectionContext.tsx          (Collections state)
  ├─ UserContext.tsx                (User profile)
  ├─ NotificationContext.tsx        (Notifications)
  ├─ ToastContext.tsx               (Toast messages)
  ├─ EntryModeContext.tsx           (Card entry mode)
  ├─ LinkParamsContext.tsx          (Navigation params)
  ├─ CollectionManagementContext.tsx (Collection operations)
  ├─ WishlistContext.tsx            (Wishlist state)
  └─ SealedCollectionContext.tsx    (Sealed products)
```

## Pattern: Reducer-Based State Machine

Contexts use `useReducer` for complex state with multiple actions. This enables:
- Testable state transitions
- Clear action semantics
- Predictable state mutations

### Example: Auth Context

See: `contexts/AuthStateContext.tsx` — Complete example of:
- AuthState interface with status union type
- AuthAction discriminated union
- Reducer function with state transitions
- Provider component with useReducer
- Custom hook for consumption

## Key Contexts

### Auth (`AuthStateContext.tsx`)
- **Purpose**: Manage Auth0 authentication state
- **State**: Status (initializing|authenticated|unauthenticated|syncing), user profile, errors
- **Actions**: INITIALIZE, LOGIN, SYNC, LOGOUT

See: `contexts/AuthStateContext.tsx`

### Collections (`CollectionContext.tsx` + `CollectionManagementContext.tsx`)
- **Purpose**: Collections state + operations (create, rename, grant access, etc.)
- **State**: Collections list, current collection, loading/error states

See: `contexts/CollectionContext.tsx`, `contexts/CollectionManagementContext.tsx`

### User (`UserContext.tsx`)
- **Purpose**: User profile information (separate from auth)
- **State**: Profile data (email, name, preferences)

See: `contexts/UserContext.tsx`

### Notifications (`NotificationContext.tsx`)
- **Purpose**: Queued notifications system
- **State**: Notification list with IDs, types, messages

See: `contexts/NotificationContext.tsx`

### Toast (`ToastContext.tsx`)
- **Purpose**: Toast messages (success, error, info)
- **State**: Visible toasts with auto-dismiss behavior

See: `contexts/ToastContext.tsx`

### Entry Mode (`EntryModeContext.tsx`)
- **Purpose**: Card entry mode (keyboard input, manual, etc.)
- **State**: Current entry mode and settings

See: `contexts/EntryModeContext.tsx`

### Wishlist (`WishlistContext.tsx`)
- **Purpose**: Wishlist state management
- **State**: Wishlist items, loading states

See: `contexts/WishlistContext.tsx`

## Implementation Pattern

Standard Context Structure:

1. Define state type
2. Define action types (discriminated union)
3. Create context with `createContext`
4. Provider component with `useReducer`
5. Custom hook for consumer with error handling

See: `contexts/AuthStateContext.tsx` for complete implementation pattern

## Reducer Function Pattern

Implements state machine transitions — each action determines next state. Characteristics:
- Pure function (no side effects)
- Immutable updates (spread operator)
- Exhaustive action handling (default case)
- Clear state transitions

See: `contexts/AuthStateContext.tsx` for reducer pattern

## Provider Composition

Wrap app with multiple providers in order of dependency. See: main app file for provider nesting pattern.

## When to Create a Context

1. **App-wide state**: Needed by many components across hierarchy
2. **Shared state machine**: Multiple related actions (auth, notifications)
3. **Complex updates**: Reducer logic clearer than useState chain
4. **Avoid prop drilling**: Deep component trees that need same data

**Don't create context for:**
- Local component state (use useState)
- Simple pass-through (use props)
- Infrequently accessed state (use query hooks)
- Single-use values (use props from parent)

## Custom Hooks Pattern

Always provide custom hook for consuming context. This provides:
- Better error messages (fails at consumption site)
- Easy refactoring (change context structure without updating all consumers)
- Discoverability (IDE autocomplete finds use* pattern)

Example pattern from `contexts/AuthStateContext.tsx`:
```typescript
export function useAuthState() {
  const context = useContext(AuthStateContext)
  if (!context) {
    throw new Error('useAuthState must be used within AuthStateProvider')
  }
  return context
}
```

## Type Safety

Always define explicit types. See: `contexts/AuthStateContext.tsx` for:
- State interface with specific fields
- Discriminated union for actions (ensures correct payload for each action)
- Return type for reducer

## Guidelines

- ✓ One domain per context file
- ✓ Use reducer for multi-action state (avoid useState chains)
- ✓ Discriminated union for actions (ensures type safety)
- ✓ Immutable state updates (no mutations)
- ✓ Custom hook for consumption (type safety + discoverability)
- ✓ Throw error if used outside provider
- ✓ Comment complex state transitions
- ✓ Keep reducer logic simple (extract complex logic to utils)

## Real-World Examples

See: `contexts/` directory for complete context implementations:
- `AuthStateContext.tsx` — Auth state machine
- `CollectionContext.tsx` — Collection management
- `UserContext.tsx` — User profile
- `ToastContext.tsx` — Toast notifications
