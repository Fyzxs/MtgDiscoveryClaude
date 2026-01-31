# Generated Folder

## Purpose
**Auto-generated code from GraphQL schema.** This folder contains TypeScript types generated from the GraphQL backend. Files here are NEVER manually edited — they are always overwritten by codegen tools.

## What's Here

```
generated/
  └─ graphql.ts          (Generated TypeScript types from GraphQL schema)
```

### GraphQL Types (`graphql.ts`)

Automatically generated TypeScript interfaces and types based on GraphQL schema:
- Query types (CardsByIdsQuery, CardDetailsQuery, etc.)
- Mutation types (AddCardToCollectionMutation, etc.)
- Entity types (Card, Set, Artist, etc.)
- Variables types (CardsByIdsQueryVariables, etc.)

## Usage in Code

Always import types from `generated/graphql`:

```typescript
import type { Card, CardsByIdsQuery, CardsByIdsQueryVariables } from '@/generated/graphql'

// In hooks
function useCardDetails(cardId: string) {
  const { data } = useQuery<CardsByIdsQuery, CardsByIdsQueryVariables>(CARD_DETAILS)
  return data?.cardsByIds[0]
}
```

## Key Rules

### 🚫 NEVER Edit Generated Files

These files are **completely overwritten** by codegen. Any manual changes will be lost.

### ✅ DO Regenerate When Schema Changes

When backend GraphQL schema changes:
```bash
npm run codegen
```

### ✅ DO Import Generated Types Everywhere

Use generated types for type safety throughout app.

### ✅ DO Commit Generated Files

Include generated files in git with other code changes.

## When to Regenerate

Regenerate types when:
1. **Backend Schema Changes**: New fields, types, or operations
2. **GraphQL Operations Change**: New queries/mutations added
3. **Updating Dependencies**: After updating Apollo Client or codegen
4. **On Pull**: After pulling code with schema updates

Commands:
```bash
npm run codegen:watch    # Watch mode
npm run codegen          # One-time generation
npm run build            # Includes codegen
```

## Troubleshooting

**Types are out of sync:** `npm run codegen`

**TypeScript errors in generated file:** Check GraphQL operation syntax and backend schema validity.

**Generated file is huge:** Normal for large schemas. Consider splitting if unwieldy.

## Best Practices

- ✓ Import types with `import type` for tree-shaking
- ✓ Use generated types for all GraphQL operations
- ✓ Never duplicate generated types in app code
- ✓ Regenerate when backend schema changes
- ✓ Commit generated files to version control
- ✓ Keep GraphQL operations up-to-date with schema
- ✓ Use typed hooks that wrap Apollo with generated types
- ✓ Don't edit or format generated files manually

See: `graphql/` folder for queries/mutations that drive this code generation.
