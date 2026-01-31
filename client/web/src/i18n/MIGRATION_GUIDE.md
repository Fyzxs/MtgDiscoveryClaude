# I18n Migration Guide

## Step 1: Install Dependencies

```bash
npm install react-i18next i18next i18next-browser-languagedetector i18next-http-backend
```

## Step 2: Update Main App Component

```typescript
// src/App.tsx
import { I18nProvider } from './components/providers/I18nProvider';

function App() {
  return (
    <I18nProvider>
      {/* Your existing app content */}
    </I18nProvider>
  );
}
```

## Step 3: Replace Hardcoded Emojis

### Before (Current CollectionSummary)
```typescript
const EMOJI_TOOLTIPS = {
  '🔹': 'Nonfoil',
  '✨': 'Foil',
  '⚡': 'Etched'
};

// Hardcoded usage
<span>🔹</span>
```

### After (Localized CollectionSummary)
```typescript
import { useSymbols, useFinishDisplay } from '../../../hooks/useSymbols';

function MyComponent() {
  const { getFinishSymbol } = useSymbols();
  const finishDisplay = useFinishDisplay();

  // Localized usage
  const symbol = finishDisplay.getSymbol('nonfoil'); // Gets localized symbol
  const label = finishDisplay.getLabel('nonfoil');   // Gets localized label

  return <span title={label}>{symbol}</span>;
}
```

## Step 4: Replace Hardcoded Strings

### Before
```typescript
<Typography>Search cards...</Typography>
<Button>Add to Collection</Button>
```

### After
```typescript
import { useTranslation } from 'react-i18next';
import { TRANSLATION_KEYS } from '../i18n/keys';

function MyComponent() {
  const { t } = useTranslation('common');
  const { t: tCollection } = useTranslation('collection');

  return (
    <>
      <Typography>{t(TRANSLATION_KEYS.COMMON.SEARCH)}</Typography>
      <Button>{tCollection(TRANSLATION_KEYS.COLLECTION.ADD_TO_COLLECTION)}</Button>
    </>
  );
}
```

## Step 5: Add Language Switcher

```typescript
// In your header/navigation component
import { LanguageSwitcher } from './components/atoms/shared/LanguageSwitcher';

<LanguageSwitcher showLabel={true} />
```

## Step 6: Update Components with Cultural Symbols

### Priority Components to Update:
1. `CollectionSummary.tsx` → Use `CollectionSummaryI18n.tsx` pattern
2. `SetCollectionTracker.tsx` → Add useSymbols hook
3. `CardBadges.tsx` → Replace finish type emojis
4. Any component with hardcoded strings

### Example: Updating CardBadges
```typescript
// Before
const badges = ['🔹', '✨', '⚡'];

// After
function CardBadges() {
  const { getAllFinishSymbols } = useSymbols();
  const symbols = getAllFinishSymbols();

  const badges = [symbols.nonfoil, symbols.foil, symbols.etched];
}
```

## Step 7: Add More Languages

### Create translation files:
```
public/locales/
├── en/
├── es/
│   ├── common.json
│   ├── cards.json
│   ├── collection.json
│   └── symbols.json
├── fr/
└── ja/
```

### Example Spanish translations:
```json
// public/locales/es/common.json
{
  "search": "Buscar",
  "loading": "Cargando...",
  "error": "Error"
}

// public/locales/es/cards.json
{
  "finish": {
    "nonfoil": "No foil",
    "foil": "Foil",
    "etched": "Grabado"
  }
}
```

## Step 8: Test Different Cultures

### Test with Japanese symbols:
- Switch language to Japanese
- Verify symbols change from emojis to text characters
- Test layout with different character widths

### Test with Spanish:
- Verify all strings are translated
- Test longer text strings (Spanish is ~20% longer than English)
- Check responsive layouts

## Step 9: Advanced Features

### Add RTL Support (for future Arabic/Hebrew):
```typescript
// In theme configuration
import { createTheme } from '@mui/material/styles';
import { useTranslation } from 'react-i18next';

function useDirectionalTheme() {
  const { i18n } = useTranslation();
  const isRTL = ['ar', 'he'].includes(i18n.language);

  return createTheme({
    direction: isRTL ? 'rtl' : 'ltr',
    // ... rest of theme
  });
}
```

### Add Number/Date Formatting:
```typescript
import { useTranslation } from 'react-i18next';

function useLocaleFormatting() {
  const { i18n } = useTranslation();

  return {
    formatNumber: (num: number) => new Intl.NumberFormat(i18n.language).format(num),
    formatDate: (date: Date) => new Intl.DateTimeFormat(i18n.language).format(date),
    formatCurrency: (amount: number, currency = 'USD') =>
      new Intl.NumberFormat(i18n.language, { style: 'currency', currency }).format(amount)
  };
}
```

## Benefits After Migration

### For Users:
- **Cultural Appropriate**: Symbols and emojis that work across all devices/cultures
- **Native Language**: UI in their preferred language
- **Better Accessibility**: Proper labels and ARIA support

### For Development:
- **Maintainable**: Centralized translation management
- **Scalable**: Easy to add new languages
- **Consistent**: Standardized symbol/emoji usage
- **Type Safe**: Translation keys are typed and validated

### For Global Reach:
- **SEO**: Language-specific pages for better search results
- **Community**: Enables community-driven translations
- **Market Expansion**: Can target non-English speaking MTG communities

## Performance Considerations

- **Bundle Size**: Only load needed language files
- **Caching**: Translations cached in localStorage
- **Lazy Loading**: Load translations per route/component
- **Fallbacks**: Graceful handling of missing translations

This migration transforms the app from English-only with hardcoded emojis to a truly international application that respects cultural differences and provides native language experiences for users worldwide.