# i18n Implementation Plan

**Analysis Date:** 2025-11-20
**Status:** Documented - Pending Implementation
**Estimated Effort:** 25-35 hours

---

## Executive Summary

The frontend has i18n infrastructure already installed and configured, but actual translation usage is not implemented. The main work involves extracting ~150-180 hardcoded strings into translation files and completing translations for 5 additional locales.

---

## 1. Current State

### Installed Dependencies
```json
{
  "i18next": "^25.5.2",
  "react-i18next": "^16.0.0",
  "i18next-browser-languagedetector": "^8.2.0",
  "i18next-http-backend": "^3.0.2"
}
```

### Existing Configuration
- **Config file:** `/client/src/i18n/config.ts`
- **Translation keys:** `/client/src/i18n/keys.ts`
- **Namespaces:** common, cards, sets, collection, navigation, errors, symbols
- **Supported languages:** en, es, fr, de, ja, pt

### Translation File Status

| Locale | Files Present | Completeness |
|--------|---------------|--------------|
| en (English) | common, cards, collection, errors, navigation, sets, symbols | Baseline |
| es (Spanish) | common, navigation, symbols | ~25% |
| ja (Japanese) | symbols | ~10% |
| fr (French) | None | 0% |
| de (German) | None | 0% |
| pt (Portuguese) | None | 0% |

### UI Component Status
- `LanguageSwitcher` component exists at `/client/src/components/organisms/shared/LanguageSwitcher.tsx`
- Currently disabled in Header (commented out with note: "Disabled until translations are available")

---

## 2. Hardcoded Strings Audit

### Summary by Category

| Category | Count | Description |
|----------|-------|-------------|
| Label props | ~51 | Filter panels, forms, sort options |
| Aria-labels | ~17 | Header, filter panels, navigation |
| Placeholders | ~7 | Search inputs, filters |
| Title/Tooltip | ~8 | Modals, tooltips |
| Button text | ~20 | Auth, navigation, actions |
| Error messages | ~15 | Error boundaries, empty states |
| Static content | ~40 | Page headers, descriptions |
| Sort options | 14 | cardSortOptions.ts |
| **Total** | **~150-180** | |

### Priority 1 Files (Core UI) - 14 files

| File | Estimated Strings | Key Strings |
|------|-------------------|-------------|
| `/client/src/components/organisms/shared/Header.tsx` | 12 | "MtgDiscovery", "Jump to Set", "All Sets", "Search", "Cards", "Artists", aria-labels |
| `/client/src/components/auth/AuthButton.tsx` | 5 | "Loading...", "Welcome, {name}", "Login", "Logout" |
| `/client/src/components/utils/ErrorBoundary.tsx` | 10 | "Something went wrong", "Try Again", "Go Home", "Failed to load" |
| `/client/src/components/utils/ErrorBoundaries.tsx` | 5 | Error messages, recovery prompts |
| `/client/src/components/pages/CardSearchPage.tsx` | 8 | "Card Search", "Enter card name...", pluralization |
| `/client/src/components/pages/ArtistSearchPage.tsx` | 5 | "Artist Search", placeholders |
| `/client/src/components/pages/AllSetsPage.tsx` | 15 | "All Sets", "Search sets...", filter labels, sort labels |
| `/client/src/components/pages/SetPage.tsx` | 10 | Set details labels, collection stats |
| `/client/src/components/pages/CardAllPrintingsPage.tsx` | 8 | Printing display labels |
| `/client/src/components/pages/ArtistCardsPage.tsx` | 6 | Artist page labels |
| `/client/src/components/pages/SignInRedirectPage.tsx` | 5 | Auth flow messages |
| `/client/src/config/cardSortOptions.ts` | 14 | All sort option labels |
| `/client/src/utils/dateFormatters.ts` | 8 | Relative date strings |
| `/client/src/components/molecules/shared/ResultsSummary.tsx` | 2 | "Showing X of Y" pattern |

### Priority 2 Files (Filters & UI) - 12 files

| File | Estimated Strings |
|------|-------------------|
| `/client/src/components/organisms/filters/FilterPanel.tsx` | 5 |
| `/client/src/components/organisms/Cards/CardFilterPanel.tsx` | 8 |
| `/client/src/components/organisms/Artists/ArtistPageFilters.tsx` | 6 |
| `/client/src/components/molecules/shared/EmptyState.tsx` | 4 |
| `/client/src/components/molecules/shared/BackToTopFab.tsx` | 1 |
| `/client/src/components/molecules/shared/MultiSelectDropdown.tsx` | 3 |
| `/client/src/components/molecules/shared/ExpandableSection.tsx` | 2 |
| `/client/src/components/molecules/shared/DebouncedSearchInput.tsx` | 2 |
| `/client/src/components/atoms/shared/SkipNavigation.tsx` | 2 |
| `/client/src/components/molecules/Cards/RulingsDisplay.tsx` | 2 |
| `/client/src/components/organisms/Cards/AllPrintingsDisplay.tsx` | 2 |
| `/client/src/components/organisms/Cards/CardDetailsModal.tsx` | 5 |

### Priority 3 Files (Remaining) - 15+ files
- Various organism components
- Card display components
- Set display components
- Collection components

---

## 3. Special Issues

### Date/Time Formatting
**File:** `/client/src/utils/dateFormatters.ts`

All date formatting is hardcoded to `'en-US'` locale:
- `formatReleaseDate` - "Mar 2024" format
- `formatRulingDate` - "March 15, 2024" format
- `formatFullDate` - Full date format
- `formatRelativeDate` - "Today", "Yesterday", "X days ago", etc.

**Required Changes:**
1. Accept locale parameter or use i18next's current language
2. Move relative date strings to translation files
3. Consider using `date-fns` with locale support or i18next formatting

### Pluralization Patterns
Found patterns that need i18next plural handling:

```typescript
// Current (CardSearchPage.tsx)
`character${remainingChars === 1 ? '' : 's'}`

// Should become
t('search.charactersRemaining', { count: remainingChars })

// Translation file
{
  "search": {
    "charactersRemaining_one": "{{count}} character",
    "charactersRemaining_other": "{{count}} characters"
  }
}
```

### Dynamic String Construction
Found template literals that need refactoring:

```typescript
// Current (FilterPanel.tsx)
`All ${auto.label}`

// Should become
t('filters.all', { type: auto.label })

// Or use specific keys
t(`filters.all${auto.type}`)
```

### API Response Translations
These values come from the backend and need client-side translation mapping:
- Rarity values: "Common", "Uncommon", "Rare", "Mythic"
- Set types: "Core", "Expansion", "Masters", etc.
- Card types, colors, etc.

---

## 4. Implementation Phases

### Phase 1: Complete Translation Files
**Effort:** 2-4 hours

Create missing translation JSON files:

```
/client/public/locales/
├── en/  (review and complete)
├── es/  (add: cards.json, collection.json, errors.json, sets.json)
├── fr/  (add all 7 files)
├── de/  (add all 7 files)
├── ja/  (add: common.json, cards.json, collection.json, errors.json, navigation.json, sets.json)
└── pt/  (add all 7 files)
```

### Phase 2: Date Localization
**Effort:** 4-6 hours

1. Refactor `/client/src/utils/dateFormatters.ts`:
   - Add locale parameter to all functions
   - Use `Intl.DateTimeFormat` with dynamic locale
   - Move relative time strings to translation files

2. Update all components using date formatters to pass current locale

### Phase 3: High Priority String Extraction
**Effort:** 8-12 hours

For each Priority 1 file:
1. Import `useTranslation` hook
2. Replace hardcoded strings with `t()` calls
3. Add new keys to appropriate namespace files
4. Test component renders correctly

### Phase 4: Medium Priority String Extraction
**Effort:** 6-8 hours

Apply same process to Priority 2 and 3 files.

### Phase 5: Pluralization & Interpolation
**Effort:** 2-3 hours

1. Convert all plural patterns to i18next plural syntax
2. Update dynamic strings to use interpolation: `t('key', { variable })`
3. Test edge cases (0, 1, many)

### Phase 6: Enable Language Switcher
**Effort:** 15 minutes

1. Uncomment LanguageSwitcher in Header.tsx
2. Test language switching works correctly
3. Verify localStorage persistence

---

## 5. New Translation Keys Required

Add these categories to `/client/src/i18n/keys.ts`:

```typescript
export const TRANSLATION_KEYS = {
  // Existing keys...

  // Header/Navigation (new)
  header: {
    siteName: 'header.siteName',
    jumpToSet: 'header.jumpToSet',
    allSets: 'header.allSets',
    searchOptions: 'header.searchOptions',
    ariaMainNav: 'header.ariaMainNav',
    ariaSearchOptions: 'header.ariaSearchOptions',
  },

  // Auth (new)
  auth: {
    loading: 'auth.loading',
    welcome: 'auth.welcome',
    login: 'auth.login',
    logout: 'auth.logout',
  },

  // Sort options (new)
  sort: {
    collectorAsc: 'sort.collectorAsc',
    collectorDesc: 'sort.collectorDesc',
    nameAsc: 'sort.nameAsc',
    nameDesc: 'sort.nameDesc',
    rarityAsc: 'sort.rarityAsc',
    rarityDesc: 'sort.rarityDesc',
    priceAsc: 'sort.priceAsc',
    priceDesc: 'sort.priceDesc',
    releaseAsc: 'sort.releaseAsc',
    releaseDesc: 'sort.releaseDesc',
    setNameAsc: 'sort.setNameAsc',
    setNameDesc: 'sort.setNameDesc',
    collectionAsc: 'sort.collectionAsc',
    collectionDesc: 'sort.collectionDesc',
  },

  // Filters (new)
  filters: {
    all: 'filters.all',
    allTypes: 'filters.allTypes',
    allFormats: 'filters.allFormats',
    allStatuses: 'filters.allStatuses',
    clearAll: 'filters.clearAll',
  },

  // Empty states (new)
  emptyState: {
    noResults: 'emptyState.noResults',
    noSetsFound: 'emptyState.noSetsFound',
    noCardsFound: 'emptyState.noCardsFound',
    tryAdjusting: 'emptyState.tryAdjusting',
    clearFilters: 'emptyState.clearFilters',
  },

  // Results (new)
  results: {
    showing: 'results.showing',
    total: 'results.total',
  },

  // Dates (new)
  dates: {
    today: 'dates.today',
    yesterday: 'dates.yesterday',
    tomorrow: 'dates.tomorrow',
    daysAgo: 'dates.daysAgo',
    weeksAgo: 'dates.weeksAgo',
    monthsAgo: 'dates.monthsAgo',
    inDays: 'dates.inDays',
    inWeeks: 'dates.inWeeks',
    inMonths: 'dates.inMonths',
  },
};
```

---

## 6. Sample Translation File Updates

### English `/client/public/locales/en/common.json` additions:

```json
{
  "header": {
    "siteName": "MtgDiscovery",
    "jumpToSet": "Jump to Set",
    "allSets": "All Sets",
    "searchOptions": "Search options",
    "ariaMainNav": "Main navigation",
    "ariaSearchOptions": "Search options menu"
  },
  "auth": {
    "loading": "Loading...",
    "welcome": "Welcome, {{name}}",
    "login": "Login",
    "logout": "Logout"
  },
  "sort": {
    "collectorAsc": "Collector # (Low-High)",
    "collectorDesc": "Collector # (High-Low)",
    "nameAsc": "Name (A-Z)",
    "nameDesc": "Name (Z-A)",
    "rarityAsc": "Rarity (Common-Mythic)",
    "rarityDesc": "Rarity (Mythic-Common)",
    "priceAsc": "Price (Low-High)",
    "priceDesc": "Price (High-Low)",
    "releaseAsc": "Release Date (Oldest)",
    "releaseDesc": "Release Date (Newest)",
    "setNameAsc": "Set Name (A-Z)",
    "setNameDesc": "Set Name (Z-A)",
    "collectionAsc": "Collection Count (Low-High)",
    "collectionDesc": "Collection Count (High-Low)"
  },
  "filters": {
    "all": "All {{type}}",
    "allTypes": "All Types",
    "allFormats": "All Formats",
    "allStatuses": "All Statuses",
    "clearAll": "Clear all filters"
  },
  "emptyState": {
    "noResults": "No results found",
    "noSetsFound": "No sets found matching your criteria",
    "noCardsFound": "No cards found matching \"{{searchTerm}}\"",
    "tryAdjusting": "Try adjusting your filters or search terms",
    "clearFilters": "Clear filters"
  },
  "results": {
    "showing": "Showing {{current}} of {{total}} {{label}}",
    "total": "{{total}} {{label}}"
  },
  "dates": {
    "today": "Today",
    "yesterday": "Yesterday",
    "tomorrow": "Tomorrow",
    "daysAgo_one": "{{count}} day ago",
    "daysAgo_other": "{{count}} days ago",
    "weeksAgo_one": "{{count}} week ago",
    "weeksAgo_other": "{{count}} weeks ago",
    "monthsAgo_one": "{{count}} month ago",
    "monthsAgo_other": "{{count}} months ago",
    "inDays_one": "in {{count}} day",
    "inDays_other": "in {{count}} days",
    "inWeeks_one": "in {{count}} week",
    "inWeeks_other": "in {{count}} weeks",
    "inMonths_one": "in {{count}} month",
    "inMonths_other": "in {{count}} months"
  }
}
```

---

## 7. Verification Checklist

After each phase:
- [ ] `npm run build` completes without errors
- [ ] `npm run lint` passes
- [ ] `npm run dev` starts correctly
- [ ] Verify English text displays correctly
- [ ] Test language switching (after Phase 6)
- [ ] Verify no hardcoded strings visible in UI

### Final Verification:
- [ ] All supported languages render correctly
- [ ] Date/time formats respect locale
- [ ] Pluralization works correctly (0, 1, many)
- [ ] Language preference persists across sessions
- [ ] No console warnings about missing translations

---

## 8. Notes

- **DO NOT translate:** Card names, set names, artist names, oracle text (these come from Scryfall API)
- **DO translate:** UI labels, error messages, navigation, filter options, sort options
- Consider using a translation management service (Crowdin, Lokalise) for ongoing maintenance
- RTL support not currently needed but MUI theme supports it if Arabic/Hebrew added later
