# Internationalization (i18n) Implementation Plan

## Overview

This document outlines the internationalization strategy for the MTG Discovery application, addressing both text localization and cultural adaptation including emoji usage.

## Technology Stack

### Primary Libraries
- **react-i18next**: React integration for i18next
- **i18next**: Core internationalization framework
- **i18next-browser-languagedetector**: Automatic language detection
- **i18next-http-backend**: Load translations from files

### Installation
```bash
npm install react-i18next i18next i18next-browser-languagedetector i18next-http-backend
```

## Architecture

### 1. Translation Namespace Organization

```
src/locales/
├── en/
│   ├── common.json          # Common UI elements
│   ├── cards.json           # Card-related terms
│   ├── sets.json            # Set-related terms
│   ├── collection.json      # Collection management
│   ├── navigation.json      # Navigation and routing
│   ├── errors.json          # Error messages
│   └── symbols.json         # Cultural symbols and emojis
├── es/
│   ├── common.json
│   ├── cards.json
│   └── ... (same structure)
├── fr/
└── de/
```

### 2. Cultural Symbol Mapping

Instead of hardcoded emojis, use cultural-aware symbol mappings:

```typescript
// symbols.json for different locales
{
  "en": {
    "finish": {
      "nonfoil": "🔹",
      "foil": "✨",
      "etched": "⚡"
    },
    "special": {
      "artistProof": "📜",
      "signed": "✍️",
      "altered": "🎨"
    }
  },
  "ja": {
    "finish": {
      "nonfoil": "◆",
      "foil": "★",
      "etched": "◇"
    }
  }
}
```

## Implementation Strategy

### Phase 1: Core Infrastructure
1. Install and configure i18next
2. Create translation namespace structure
3. Implement useTranslation hooks
4. Create translation key constants

### Phase 2: Component Localization
1. Replace hardcoded strings with translation keys
2. Implement cultural symbol system
3. Update date/number formatting
4. Handle pluralization rules

### Phase 3: Advanced Features
1. Language switching UI
2. Right-to-left (RTL) language support
3. Dynamic locale loading
4. SEO optimization for multiple languages

## Key Components to Localize

### 1. Navigation & UI
- Page titles and headings
- Button labels and tooltips
- Form labels and placeholders
- Error messages and notifications

### 2. Card Information
- Rarity names
- Set types
- Format legality
- Card attributes

### 3. Collection Features
- Finish type labels
- Special card designations
- Collection statistics
- Progress indicators

### 4. Cultural Elements
- Emoji and symbol usage
- Date/time formatting
- Number formatting
- Currency display

## Implementation Examples

### Translation Keys Structure
```typescript
// keys/index.ts
export const TRANSLATION_KEYS = {
  COMMON: {
    SEARCH: 'common:search',
    LOADING: 'common:loading',
    ERROR: 'common:error'
  },
  CARDS: {
    RARITY: {
      COMMON: 'cards:rarity.common',
      UNCOMMON: 'cards:rarity.uncommon',
      RARE: 'cards:rarity.rare',
      MYTHIC: 'cards:rarity.mythic'
    }
  },
  SYMBOLS: {
    FINISH: {
      NONFOIL: 'symbols:finish.nonfoil',
      FOIL: 'symbols:finish.foil',
      ETCHED: 'symbols:finish.etched'
    }
  }
} as const;
```

### Hook Usage
```typescript
// hooks/useSymbols.ts
import { useTranslation } from 'react-i18next';

export function useSymbols() {
  const { t } = useTranslation('symbols');

  return {
    getFinishSymbol: (finish: string) => t(`finish.${finish}`),
    getSpecialSymbol: (special: string) => t(`special.${special}`)
  };
}
```

### Component Implementation
```typescript
// Before
const EMOJI_TOOLTIPS = {
  '🔹': 'Nonfoil',
  '✨': 'Foil',
  '⚡': 'Etched'
};

// After
function useFinishDisplay() {
  const { t } = useTranslation(['symbols', 'cards']);

  return {
    getSymbol: (finish: string) => t(`symbols:finish.${finish}`),
    getLabel: (finish: string) => t(`cards:finish.${finish}`)
  };
}
```

## Language Support Strategy

### Priority Languages
1. **English** (en) - Primary
2. **Spanish** (es) - Large MTG community
3. **French** (fr) - Official MTG language
4. **German** (de) - Major European market
5. **Japanese** (ja) - Origin market
6. **Portuguese** (pt) - Brazilian market

### Implementation Phases
- **Phase 1**: English + 1 additional language (Spanish)
- **Phase 2**: Add French and German
- **Phase 3**: Add Japanese and Portuguese
- **Phase 4**: Community-driven translations

## Technical Considerations

### 1. Bundle Size Optimization
- Lazy load translations by route
- Tree-shake unused translations
- Compress translation files

### 2. Caching Strategy
- Cache translations in localStorage
- Implement version checking
- Graceful fallbacks

### 3. SEO & URLs
- Implement locale-specific URLs
- Add hreflang tags
- Localized meta tags

### 4. User Experience
- Persist language preference
- Automatic language detection
- Smooth language switching

## Migration Strategy

### Step 1: Foundation
1. Install i18n dependencies
2. Configure i18next
3. Create English baseline translations
4. Implement translation context provider

### Step 2: Component Migration
1. Start with common components
2. Replace hardcoded strings gradually
3. Update symbol/emoji usage
4. Test with pseudo-localization

### Step 3: Feature Addition
1. Add language switcher
2. Implement locale detection
3. Add second language (Spanish)
4. Test RTL support preparation

### Step 4: Expansion
1. Add remaining priority languages
2. Implement advanced features
3. Community translation tools
4. Analytics and optimization

## Testing Strategy

### 1. Pseudo-localization
- Generate fake translations to test UI layout
- Identify hardcoded strings
- Test text expansion/contraction

### 2. Cultural Testing
- Test emoji/symbol rendering across devices
- Validate cultural appropriateness
- Test with native speakers

### 3. Technical Testing
- Language switching performance
- Translation loading errors
- Fallback behavior

## Maintenance

### 1. Translation Management
- Extract new translation keys automatically
- Validate translation completeness
- Update process for new features

### 2. Quality Assurance
- Regular translation reviews
- Community feedback integration
- Professional translation services for key languages

This implementation plan provides a robust foundation for making the MTG Discovery app truly international and culturally appropriate for a global audience.