# Constants Folder

## Purpose
Static application values that never change: enums, emoji mappings, color mappings, labels. Simple key-value pairs that configure behavior without logic.

## Organization

```
constants/
  ├─ collectionEmojis.ts    (Finish emojis: foil, nonfoil, etched, etc.)
  └─ setTypeColors.ts       (Color mappings for set types)
```

## Key Patterns

### Collection Emojis (`collectionEmojis.ts`)

Emoji representation for card finishes.

See: `constants/collectionEmojis.ts` for:
- Emoji mappings (nonfoil, foil, etched, proof, signed, altered)
- Usage pattern in UI

### Set Type Colors (`setTypeColors.ts`)

Color mappings for different set types.

See: `constants/setTypeColors.ts` for set type color patterns

## When to Add Constants

1. **Enums/Mappings**: Key-value pairs (finish type → emoji, set type → color)
2. **Magic Strings**: Values hardcoded in multiple places
3. **Display Labels**: Friendly text for UI
4. **Collections**: Arrays of static options
5. **Magic Numbers**: Special values (max items, timeouts, etc.)

**Don't put in constants:**
- Functions (use `/utils/`)
- Component-specific values (define in component)
- Configuration options (use `/config/`)
- Styling (use `/theme/`)
- Environment-dependent values (use `.env`)

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **File name** | camelCase | `collectionEmojis.ts` |
| **Object export** | UPPER_SNAKE_CASE | `COLLECTION_EMOJIS` |
| **Keys** | camelCase | `nonfoil`, `foil`, `etched` |
| **Array export** | UPPER_SNAKE_CASE | `RARITY_TYPES` |

## Type Safety

Always define types for constant objects. See: `constants/collectionEmojis.ts` for Record<T, string> pattern.

## Guidelines

- ✓ Immutable (use const, Object.freeze() if needed)
- ✓ No imports between constant files
- ✓ Clear, descriptive keys (not abbreviations)
- ✓ Group related constants
- ✓ Export type definitions for autocomplete
- ✓ Document non-obvious mappings
- ✓ One constant object per file (or closely related set)

See: `constants/` directory for real constant file examples.
