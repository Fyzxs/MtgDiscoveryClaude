# I18n Folder (Internationalization)

## Purpose
Internationalization (i18n) setup using react-i18next. Handles translation strings, cultural symbol mapping, and multi-language support.

## Current Status

**Planned Implementation** using react-i18next with namespace structure.

## Architecture

```
i18n/
  ├─ locales/               (Translation files per language)
  │   ├─ en/
  │   │   ├─ common.json
  │   │   ├─ cards.json
  │   │   ├─ sets.json
  │   │   └─ [namespace].json
  │   ├─ es/, fr/, [language]/
  ├─ config.ts              (i18next configuration)
  ├─ useTranslation.ts      (Custom translation hook wrapper)
  └─ symbolMappings.ts      (Cultural symbol mappings)
```

## Namespace Structure

Translation files organized by feature domain: common, cards, sets, collection, navigation, errors, symbols.

See existing README files for implementation plan and symbol mapping strategy.

## Usage in Components

Import and use translation hook:

```typescript
const { t } = useTranslation('cards')
<h1>{t('cardName')}</h1>
```

Supports interpolation: `t('welcome', { name: 'Alice' })`

Supports pluralization: `t('cardsInSet', { count: cardCount })`

## Cultural Symbol Mapping (`symbolMappings.ts`)

Magic symbols vary by culture. Map human-readable symbols to locale-specific representations (emoji-based for English, text-based for Japanese, etc.).

## Configuration (`config.ts`)

Configure react-i18next with:
- Language detection
- Namespaces
- Default language
- Resource files

## When to Add Translations

1. **New Feature**: Translate all UI text
2. **New Page**: Extract all strings to translation files
3. **New Dialog/Modal**: All user-facing text to i18n
4. **Error Messages**: All error text to errors namespace

## Language Support Roadmap

- **Phase 1 (MVP):** English only
- **Phase 2:** Add Spanish
- **Phase 3:** European Languages (French, German)
- **Phase 4:** Asian Languages (Japanese, Portuguese)

## Key String Patterns

- Actions/Buttons: Short imperative phrases
- Messages: Feedback and status updates
- Formats: Date, time, currency formatting
- Symbols: Cultural symbol mappings

## Guidelines

- ✓ All user-facing text in translation files
- ✓ One namespace per domain
- ✓ Descriptive keys: `cardNotFound` not `error403`
- ✓ Use interpolation for dynamic content
- ✓ Group related translations in nested objects
- ✓ Include symbol mappings for all languages
- ✓ Test with long translations (German, French)
- ✓ Use `defaultValue` fallback for missing translations in dev

See: i18n folder README and symbolMappings.ts for implementation details.
