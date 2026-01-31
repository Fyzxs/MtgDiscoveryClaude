# GraphQL Folder

## Purpose
GraphQL query/mutation definitions and Apollo Client configuration. All GraphQL operations (queries, mutations, subscriptions) are defined here and imported by hooks/components.

## Organization

```
graphql/
  ├─ apollo-client.ts        (Apollo Client setup + token management)
  ├─ queries/                (GraphQL query definitions)
  │   ├─ cards.ts
  │   ├─ sets.ts
  │   ├─ userCards.ts
  │   └─ [domain].ts
  └─ mutations/              (GraphQL mutation definitions)
      ├─ addCardToCollection.ts
      ├─ createCollection.ts
      └─ [domain].ts
```

## Apollo Client Setup (`apollo-client.ts`)

Connects to GraphQL backend with Auth0 token authentication.

**Key Functions:**
- `setAuth0TokenGetter(getter)`: Register function to get current token
- `setTokenReadyState(isReady)`: Signal when token is available
- `subscribeToTokenReady(callback)`: Listen for token availability

See: `graphql/apollo-client.ts` for configuration and token management pattern

## Query Definitions

Each query file exports GraphQL query operations used by hooks/components.

**Pattern:**
- File: `queries/[domain].ts` (e.g., `queries/cards.ts`)
- Export: `[OPERATION_NAME] = gql(...)`

See: `graphql/queries/cards.ts` — Card query definitions

See: `graphql/queries/sets.ts` — Set query definitions

See: `graphql/queries/userCards.ts` — User card query definitions

## Mutation Definitions

Mutation files export GraphQL mutation operations for creating/updating/deleting.

**Pattern:**
- File: `mutations/[domain].ts` (e.g., `mutations/createCollection.ts`)
- Export: `[OPERATION_NAME] = gql(...)`

See: `graphql/mutations/addCardToCollection.ts` — Add card mutation

See: `graphql/mutations/createCollection.ts` — Create collection mutation

See: `graphql/mutations/user.ts` — User mutations

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Query operation** | UPPER_SNAKE_CASE | `CARDS_BY_IDS`, `CARD_DETAILS` |
| **Mutation operation** | UPPER_SNAKE_CASE, verb first | `ADD_CARD_TO_COLLECTION`, `CREATE_COLLECTION` |
| **Query file** | `[domain].ts` | `cards.ts`, `collections.ts` |
| **Mutation file** | `[domain].ts` | `collections.ts`, `user.ts` |

## GraphQL Document Best Practices

**Fragment Reuse:** Define reusable fragments for common fields to maximize Apollo cache hits.

**Consistent Field Selection:** Always select same fields for same data type across queries.

**Type Generation:** GraphQL schema generates TypeScript types automatically in `generated/graphql.ts`.

## When to Add Queries/Mutations

1. **New Data Fetch**: Need data from backend → add query
2. **New User Action**: Creating/updating/deleting data → add mutation
3. **Related Queries**: Multiple ways to fetch same data → organize by operation

## Usage in Hooks

Import operations into hooks, never directly in components:

```typescript
// ✅ GOOD: In hook
import { CARD_DETAILS } from '@/graphql/queries/cards'
export function useCardDetails(cardId: string) {
  return useQuery(CARD_DETAILS, { variables: { id: cardId } })
}

// ❌ BAD: In component
function CardDetail({ cardId }) {
  const { data } = useQuery(gql`...`) // Define query in component
}
```

## Guidelines

- ✓ Define all GraphQL operations in `graphql/` folder
- ✓ Export operations as constants
- ✓ One operation per export (easy to find and track usage)
- ✓ Use fragments for field reuse
- ✓ Add comments for complex queries/mutations
- ✓ Import operations into hooks, never directly in components
- ✓ Use generated types for type safety
- ✓ Keep queries small (fetch only needed fields)
- ✓ Use aliases for multiple instances of same type
- ✓ Handle errors via Apollo error handling in hooks

## Real-World Examples

See: `graphql/queries/` and `graphql/mutations/` directories for:
- Query definitions (cards.ts, sets.ts, userCards.ts)
- Mutation definitions (addCardToCollection.ts, createCollection.ts, user.ts)
- Apollo Client configuration (apollo-client.ts)
