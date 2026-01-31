# Hooks Folder

## Purpose
Custom React hooks encapsulating reusable logic: data fetching, state management, event handling, UI interaction patterns. Hooks compose together to build complex behavior in components.

## Organization

```
hooks/
  ├─ auth/                   (Auth-specific hooks)
  ├─ __tests__/              (Hook tests)
  └─ [feature]Hooks.ts       (Organized by feature)
```

## Hook Patterns

### Data Fetching Hooks

Encapsulate GraphQL queries with loading/error handling.

See: `hooks/useCardFiltering.ts` — filtering and sorting hook

### Collection/Filtering Hooks

Manage filtered data with sorting and filtering logic.

See: `hooks/useCardCollectionEntry.ts` — state management with callback stabilization

### State Management with Refs

Hooks that manage complex state with refs to stabilize callbacks. The critical pattern is using refs to prevent callback re-registration:

See: `hooks/useCardCollectionEntry.ts` — shows how refs (like `availableFinishesRef`, `onSubmitRef`) stabilize callback registration with global handlers, preventing hidden overlays and stale closures.

### Query/Navigation Hooks

Manage URL query parameters and navigation.

See: `hooks/useSetPageData.ts`, `hooks/useMobileLayout.ts` — various hook patterns for managing page state and layout.

## Hook Naming Convention

| Pattern | Example | Usage |
|---------|---------|-------|
| `use{Feature}Data` | `useCardCollectionEntry` | Returns data + control functions |
| `use{Feature}` | `useCardFiltering` | Manages feature-specific state |
| `use{Domain}Query` | `useCardQuery` | GraphQL query wrapper |
| `use{Domain}Mutation` | `useAddCardMutation` | GraphQL mutation wrapper |
| `useAuth{Operation}` | `useAuthCallback` | Auth-specific operations |

## Hook Return Types

Always define explicit return types. Check existing hooks like `useCardCollectionEntry.ts` for `UseCardCollectionEntryReturn` pattern.

## Hook Composition

Hooks can compose other hooks. See: `hooks/` directory for examples of composed hooks.

## Testing Hooks

Test hooks with `@testing-library/react`. See: `__tests__/` folder for test examples.

## When to Create a Hook

1. **Logic Reuse**: Same logic used in multiple components
2. **Complex State**: Multiple useState/useEffect calls
3. **GraphQL Wrapper**: Wrapping useQuery/useMutation
4. **Event Handlers**: Extracting complex event handling
5. **Derived State**: Computing values from other state

**Don't create hooks for:**
- Single-use logic (keep in component)
- Simple prop pass-through (use component instead)
- Very small utilities (use utils/ instead)

## Guidelines

- ✓ Use `use` prefix (React convention)
- ✓ Define explicit return type interface
- ✓ Document parameters in JSDoc comments
- ✓ Use useCallback/useMemo when returning functions
- ✓ Ref-based state for stabilizing callbacks (see `useCardCollectionEntry.ts`)
- ✓ useEffect for side effects with clear dependency arrays
- ✓ Throw error if used outside required context
- ✓ Export hook + return type for consumers
- ✓ Test hooks independently from components
- ✓ Keep hooks focused (single responsibility)

## Real Example Flow

See: `hooks/useCardCollectionEntry.ts` — demonstrates:
- Constructor with options parameter
- Refs for mutable state
- useEffect to update refs without re-registering
- Global event handler registration
- Return type interface

See: `hooks/useCardFiltering.ts` — demonstrates:
- Derived state from inputs
- useMemo for expensive operations
- Sorting and filtering logic
- Control functions returned

See: `hooks/` directory for more patterns and examples.
