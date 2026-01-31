# Types Folder

## Purpose
TypeScript type definitions for domain objects: cards, sets, collections, users, filters. Single source of truth for app-wide types — prevents duplication and ensures consistency.

## Organization

```
types/
  ├─ auth.ts              (Auth state and user types)
  ├─ card.ts              (Card and card-related types)
  ├─ collection.ts        (Collection management types)
  ├─ cardGroup.ts         (Card grouping types)
  ├─ set.ts               (Set-specific types)
  ├─ filters.ts           (Filter configuration types)
  ├─ sealedProduct.ts     (Sealed product types)
  ├─ ui.ts                (UI state types)
  └─ components.ts        (Component prop types)
```

## Type Definition Patterns

### Domain Types

Core data model types organized by business domain.

See: `types/auth.ts` — Auth state machine with discriminated union actions

See: `types/card.ts` — Card domain with Rarity, CardFinish, ImageUris, Legalities, Prices, etc.

See: `types/collection.ts` — Collection management types

See: `types/filters.ts` — Filter configuration and state

### UI State Types

UI component state types.

See: `types/ui.ts` — LoadingState, PaginationState, SelectionState, OverlayState patterns

### Component Prop Types

Props interfaces for reusable components.

See: `types/components.ts` — CardDisplayProps, CardGridProps, SearchInputProps patterns

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Type** | PascalCase | `CardFilter`, `AuthState` |
| **Union type** | PascalCase or type literal | `Rarity`, `AuthStatus` |
| **Function return type** | descriptive | `GetCardsResult`, `UseCardHookReturn` |
| **Props interface** | `{Component}Props` | `CardDisplayProps` |
| **Generic param** | PascalCase | `T`, `TItem` |

## Type Safety Principles

Use discriminated unions for actions to ensure type safety. See: `types/auth.ts` — `AuthAction` type with discriminated union pattern.

Extract common patterns to reusable types. See: `types/` files for examples of generic patterns and reuse.

Use const assertions for literals. See: existing type files for `as const` patterns.

## When to Create Types

1. **Business Domain Objects**: Entities (Card, Set, Collection)
2. **Data Models**: API responses, state shapes
3. **Configuration**: Filter options, sort presets
4. **State Machines**: Union types for actions
5. **Component Props**: Reusable component interfaces
6. **UI State**: Loading, error, selection states

**Don't over-type:**
- Simple function parameters (obvious types)
- Component internals (use inference)
- One-off values (use literals)

## Importing Types

Use consistent import style:

```typescript
// ✅ GOOD
import type { Card, CardFilter } from '@/types/card'
import { RARITY_ORDER } from '@/types/card'

// ❌ BAD: Mix type and value imports
import { Card, RARITY_ORDER } from '@/types/card'
```

## Guidelines

- ✓ One domain per type file
- ✓ Use discriminated unions for complex states
- ✓ Export both interfaces and types
- ✓ Define initial values/defaults alongside types
- ✓ Use `const assertions` for literal types
- ✓ Import types with `import type` keyword
- ✓ Document complex types with JSDoc
- ✓ Avoid `any` and `unknown` in exported types
- ✓ Use generics for reusable patterns
- ✓ Keep type definitions DRY (extract common patterns)

## Type Organization Best Practices

Keep types organized by business domain:
- `auth.ts` (Auth domain)
- `card.ts` (Card domain)
- `collection.ts` (Collection domain)
- `filters.ts` (Filter domain)

See: `types/` directory for complete type definitions by domain.
