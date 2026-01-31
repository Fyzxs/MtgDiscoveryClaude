# Config Folder

## Purpose
Application-level configuration: sorting options, business rules, constants that affect behavior. Separate from theme (use `/theme/` for visual config) and constants (use `/constants/` for static values).

## Organization

```
config/
  └─ cardSortOptions.ts    (Sort/filter configurations)
```

## Key Patterns

### Card Sort Options (`cardSortOptions.ts`)

Sort configuration with comparison logic.

See: `config/cardSortOptions.ts` for:
- `CardLike` interface for type-safe sorting
- `RARITY_ORDER` enum-like ordering
- `parseCollectorNumber()` function with suffix handling
- Sort preset arrays for different page contexts (SET_PAGE_SORT_OPTIONS, ARTIST_PAGE_SORT_OPTIONS, CARD_DETAIL_SORT_OPTIONS, WISHLIST_PAGE_SORT_OPTIONS)

**Usage in Components:**
Pass sort presets to sorting UI components. See: `config/cardSortOptions.ts` for examples.

## When to Add Config

1. **Business Rules**: Sorting orders, filtering logic, calculation parameters
2. **Context-Specific Options**: Different configurations for different pages/features
3. **Reusable Constants**: Values used by multiple components/hooks
4. **Calculation Functions**: Pure functions that define application behavior

**Don't put in config:**
- Visual styling (use `/theme/`)
- Static text labels (use `/constants/` or `i18n/`)
- Environment variables (use `.env` files)
- GraphQL queries (use `/graphql/`)

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Config file** | camelCase | `cardSortOptions.ts` |
| **Export objects** | UPPER_SNAKE_CASE | `SET_PAGE_SORT_OPTIONS` |
| **Export functions** | camelCase | `parseCollectorNumber()` |
| **Interfaces** | PascalCase | `CardLike` |

## Type Safety

Always define types for config objects. See: `config/cardSortOptions.ts` for type-safe patterns.

## Guidelines

- ✓ Pure functions (no side effects)
- ✓ No imports between config files (avoid circular dependencies)
- ✓ Descriptive names: `cardSortOptions` not just `options`
- ✓ Export type interfaces for any public config
- ✓ Add comments explaining complex logic (e.g., collector number parsing)
- ✓ Use const assertions or types to prevent mutations

See: `config/` directory for real config file examples.
