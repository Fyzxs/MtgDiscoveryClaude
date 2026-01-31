# Utils Folder

## Purpose
Pure utility functions: data transformation, formatting, calculations, filtering, sorting. These are domain-agnostic helpers used throughout the app.

## Organization

```
utils/
  ├─ cardUtils.ts            (Card-specific utilities)
  ├─ artistUtils.ts          (Artist utilities)
  ├─ collectionFormatters.ts (Collection display formatting)
  ├─ dateFormatters.ts       (Date/time formatting)
  ├─ badgeFormatters.ts      (Badge text/styling)
  ├─ externalLinks.ts        (External link generation)
  ├─ globalCardEntryHandler.ts (Global card entry state)
  └─ directDomOverlay.ts     (Direct DOM utilities)
```

## Pure Utility Functions

### Card Utilities

Functions for card data manipulation and filtering.

See: `utils/cardUtils.ts` — getUniqueArtists, getUniqueRarities, getUniqueFinishes, getCardCollectionCount, createCardFilterFunctions

### Collection Formatters

Display-oriented formatting for collections.

See: `utils/collectionFormatters.ts` — formatCardCount, formatCollectionName patterns

### Badge Formatters

Generate badge text and styling.

See: `utils/badgeFormatters.ts` — getBadgeText, getRarityColor patterns

### Date Formatters

Date and time display formatting.

See: `utils/dateFormatters.ts` — formatDate, formatRelativeTime patterns

### External Links

Generate links to external services.

See: `utils/externalLinks.ts` — getScryfallLink, getTCGPlayerLink, getEdhrecLink patterns

## Utility Function Patterns

### Pure Functions
No side effects, deterministic. See: `utils/cardUtils.ts` for pure function examples.

### Testable
Single responsibility, easy to test. See: `utils/` files for well-organized, testable utility patterns.

### Well-Named
Function name describes what it does. See: `utils/` directory for naming convention examples.

## When to Create Utilities

1. **Reused Across Components**: Logic used in 2+ places
2. **Data Transformation**: Formatting, filtering, mapping
3. **Complex Calculations**: Computations that deserve isolation
4. **External Services**: Link generation, API construction
5. **Format/Display Logic**: Date formatting, badge generation

**Don't create utilities for:**
- Single-use logic (keep in component)
- Business rules (belongs in domain/services)
- Component-specific behavior (keep in component)
- Simple array methods (use .filter, .map directly)

## Naming Conventions

| Pattern | Example | Purpose |
|---------|---------|---------|
| `get*` | `getUniqueArtists`, `getCardCount` | Retrieve or calculate |
| `create*` | `createCardFilterFunctions` | Build/construct |
| `format*` | `formatDate`, `formatCardCount` | Convert for display |
| `is*` | `isRareOrRarer` | Boolean predicate |
| `*To*` | `cardToJson`, `dateToString` | Explicit conversion |

## Guidelines

- ✓ Pure functions (no side effects)
- ✓ Single responsibility
- ✓ Clear, descriptive names
- ✓ Type-safe with explicit parameter/return types
- ✓ Document with JSDoc comments
- ✓ Keep functions small and focused
- ✓ Avoid utility classes (use functions)
- ✓ Test utilities independently
- ✓ Group related utilities by domain
- ✓ Export multiple utilities per file if related

## Testing Utilities

Test utilities independently. See: `__tests__/` folders in utils directory for test examples.

## Real-World Examples

See: `utils/` directory for complete utility implementations:
- `cardUtils.ts` — Card manipulation functions
- `collectionFormatters.ts` — Collection formatting
- `dateFormatters.ts` — Date display
- `externalLinks.ts` — External service URLs
