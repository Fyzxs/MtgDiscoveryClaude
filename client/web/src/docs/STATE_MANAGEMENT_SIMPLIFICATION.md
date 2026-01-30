# State Management Hook Simplification

This document explains the simplified approach to filter state management that reduces complexity while maintaining functionality.

## Problem

The original state management pattern had several issues:

1. **Complex Setup**: Required 3 separate hooks (`useUrlState`, `useFilterState`, `useUrlState` again)
2. **Duplicate Configuration**: URL state config and filter config were defined separately
3. **Manual Synchronization**: Developers had to manually sync filter state with URL parameters
4. **Boilerplate Code**: ~100+ lines of repetitive state management code per page
5. **Error-Prone**: Easy to forget URL sync or use wrong debounce settings

## Solution

### New Hook: `useUrlFilterState`

A unified hook that combines filtering, sorting, and URL state management:

```typescript
import { useUrlFilterState, createUrlFilterConfig } from '../hooks/useUrlFilterState';

const {
  searchTerm,
  sortBy, 
  filters,
  filteredData,
  setSearchTerm,
  setSortBy,
  updateFilter
} = useUrlFilterState(data, config, defaults);
```

### Preset Configurations

Pre-built configurations for common patterns:

```typescript
// For set filtering (AllSetsPage)
const config = createUrlFilterConfig('sets');

// For card filtering (SetPage)
const config = createUrlFilterConfig('cards');

// Custom configuration
const config = createUrlFilterConfig('cards', {
  urlParams: { search: 'q', sort: 'order' },
  searchDebounceMs: 500
});
```

## Migration Guide

### Before (AllSetsPage - Complex)

```typescript
// 50+ lines of complex state management
const urlStateConfig = { /* ... */ };
const { getInitialValues } = useUrlState({}, urlStateConfig);
const initialValues = getInitialValues();

const filterConfig = useMemo(() => ({
  searchFields: ['name', 'code'],
  sortOptions: { /* 6 sort functions */ },
  filterFunctions: { /* filter logic */ },
  defaultSort: 'release-desc'
}), []);

const {
  searchTerm, sortBy, filters, filteredData,
  setSearchTerm, setSortBy, updateFilter
} = useFilterState(data, filterConfig, {
  search: initialValues.search || '',
  sort: initialValues.sort || 'release-desc',
  filters: { setTypes: initialValues.types || [] }
});

// Separate URL synchronization (2 more hooks)
useUrlState({ types: filters.setTypes, sort: sortBy }, /* config */, { debounceMs: 0 });
useUrlState({ search: searchTerm }, /* config */, { debounceMs: 300 });
```

### After (AllSetsPage - Simplified)

```typescript
// 10 lines of simple state management
const filterConfig = useMemo(() => 
  createUrlFilterConfig('sets', {
    urlParams: {
      search: 'search',
      sort: 'sort',
      filters: { setTypes: 'types' }
    }
  }), []);

const {
  searchTerm, sortBy, filters, filteredData,
  setSearchTerm, setSortBy, updateFilter
} = useUrlFilterState(data, filterConfig, {
  search: '',
  sort: 'release-desc',
  filters: { setTypes: [] }
});
```

## Benefits

### 1. Reduced Complexity
- **80% reduction** in state management code
- Single hook instead of 3 separate hooks
- Automatic URL synchronization

### 2. Better Maintainability  
- Preset configurations for common patterns
- Consistent API across all filtered pages
- Less error-prone implementation

### 3. Enhanced Developer Experience
- Cleaner, more readable code
- Better TypeScript support with inferred types
- Built-in optimizations and memoization

### 4. Improved Performance
- Proper debouncing for search vs filters
- Optimized re-renders with memoization
- Efficient URL parameter management

## Available Presets

### Sets Preset (`'sets'`)
- **Use Case**: AllSetsPage, set listing pages
- **Search Fields**: name, code
- **Sort Options**: release date, name, card count
- **Filter Functions**: setTypes multi-select

### Cards Preset (`'cards'`)
- **Use Case**: SetPage, card listing pages  
- **Search Fields**: name
- **Sort Options**: collector number, name, rarity
- **Filter Functions**: rarities, artists, showDigital

## Configuration Options

### URL Parameters
```typescript
urlParams: {
  search: 'q',           // URL param name for search
  sort: 'order',         // URL param name for sort
  filters: {
    rarities: 'rarity',  // URL param name for rarities filter
    artists: 'artist'    // URL param name for artists filter
  }
}
```

### Debounce Settings
```typescript
searchDebounceMs: 300,    // Debounce for search input (default: 300ms)
filterDebounceMs: 0       // Debounce for filters/sort (default: 0ms)
```

## Migration Checklist

When updating a page to use the simplified hooks:

- [ ] Replace `useUrlState` + `useFilterState` with `useUrlFilterState`
- [ ] Use preset configuration or create custom config
- [ ] Remove manual URL synchronization code
- [ ] Remove verbose filter configuration objects
- [ ] Update component to use simplified state management
- [ ] Test URL parameter synchronization
- [ ] Verify filter/search/sort functionality works correctly

## Future Enhancements

The simplified pattern enables:

1. **Enhanced Presets**: More specialized filter configurations
2. **Better Caching**: Automatic filter state persistence
3. **Advanced URL Patterns**: Custom serialization/deserialization
4. **Performance Monitoring**: Built-in filter performance metrics
5. **A11y Improvements**: Automatic accessibility enhancements for filters

## Examples

See the following files for implementation examples:

- `src/pages/AllSetsPage.tsx` - Updated to use simplified pattern
- `src/examples/SimplifiedSetPage.tsx` - Example SetPage conversion
- `src/hooks/useUrlFilterState.ts` - Hook implementation and presets