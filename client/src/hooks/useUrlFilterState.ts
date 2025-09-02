import { useMemo, useCallback, useRef } from 'react';
import { useFilterState } from './useFilterState';
import { useUrlState } from './useUrlState';
import type { UseFilterStateConfig } from '../types/filters';

export interface UrlFilterConfig<T> extends UseFilterStateConfig<T> {
  // URL state configuration
  urlParams?: {
    search?: string;
    sort?: string;
    filters?: Record<string, string>;
  };
  
  // Debounce settings
  searchDebounceMs?: number;
  filterDebounceMs?: number;
}

export interface UrlFilterDefaults {
  search?: string;
  sort?: string;
  filters?: Record<string, any>;
}

/**
 * Unified hook that combines filtering, sorting, and URL state management.
 * Simplifies the common pattern of syncing filter state with URL parameters.
 */
export function useUrlFilterState<T>(
  data: T[] | undefined,
  config: UrlFilterConfig<T>,
  defaults: UrlFilterDefaults = {}
) {
  const {
    searchDebounceMs = 300,
    filterDebounceMs = 0,
    urlParams = {},
    ...filterConfig
  } = config;

  // Get initial state from URL
  const urlStateConfig = useMemo(() => ({
    search: { default: defaults.search || '' },
    sort: { default: defaults.sort || filterConfig.defaultSort || '' },
    ...Object.fromEntries(
      Object.keys(defaults.filters || {}).map(key => [
        urlParams.filters?.[key] || key,
        { default: defaults.filters?.[key] || [] }
      ])
    )
  }), [defaults, filterConfig.defaultSort, urlParams.filters]);

  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const initialValues = getInitialValues();

  // Use filter state with initial values from URL
  const filterState = useFilterState(
    data,
    filterConfig,
    {
      search: initialValues[urlParams.search || 'search'] || defaults.search || '',
      sort: initialValues[urlParams.sort || 'sort'] || defaults.sort || filterConfig.defaultSort || '',
      filters: Object.fromEntries(
        Object.entries(defaults.filters || {}).map(([key, defaultValue]) => [
          key,
          initialValues[urlParams.filters?.[key] || key] || defaultValue
        ])
      )
    }
  );

  // Sync search term with URL (debounced)
  useUrlState(
    {
      [urlParams.search || 'search']: filterState.searchTerm
    },
    {
      [urlParams.search || 'search']: { default: defaults.search || '' }
    },
    {
      debounceMs: searchDebounceMs
    }
  );

  // Sync filters and sort with URL (immediate)
  const urlSyncState = useMemo(() => {
    const state: Record<string, any> = {
      [urlParams.sort || 'sort']: filterState.sortBy
    };
    
    Object.entries(filterState.filters).forEach(([key, value]) => {
      const urlKey = urlParams.filters?.[key] || key;
      state[urlKey] = value;
    });
    
    return state;
  }, [filterState.sortBy, filterState.filters, urlParams]);

  const urlSyncConfig = useMemo(() => {
    const config: Record<string, any> = {
      [urlParams.sort || 'sort']: { default: defaults.sort || filterConfig.defaultSort || '' }
    };
    
    Object.entries(defaults.filters || {}).forEach(([key, defaultValue]) => {
      const urlKey = urlParams.filters?.[key] || key;
      config[urlKey] = { default: defaultValue };
    });
    
    return config;
  }, [defaults, filterConfig.defaultSort, urlParams]);

  useUrlState(urlSyncState, urlSyncConfig, { debounceMs: filterDebounceMs });

  return filterState;
}

/**
 * Preset configurations for common use cases
 */
export const presets = {
  /**
   * Configuration for set filtering (AllSetsPage)
   */
  sets: {
    searchFields: ['name', 'code'] as const,
    sortOptions: {
      'release-desc': (a: any, b: any) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime(),
      'release-asc': (a: any, b: any) => new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime(),
      'name-asc': (a: any, b: any) => a.name.localeCompare(b.name),
      'name-desc': (a: any, b: any) => b.name.localeCompare(a.name),
      'cards-desc': (a: any, b: any) => (b.cardCount || 0) - (a.cardCount || 0),
      'cards-asc': (a: any, b: any) => (a.cardCount || 0) - (b.cardCount || 0)
    },
    filterFunctions: {
      setTypes: (item: any, selectedTypes: string[]) => {
        if (!selectedTypes || selectedTypes.length === 0) return true;
        return selectedTypes.includes(item.setType);
      }
    },
    defaultSort: 'release-desc'
  },

  /**
   * Configuration for card filtering (SetPage)
   */
  cards: {
    searchFields: ['name'] as const,
    sortOptions: {
      'collector-asc': (a: any, b: any) => {
        const aNum = parseInt(a.collectorNumber) || 0;
        const bNum = parseInt(b.collectorNumber) || 0;
        return aNum - bNum;
      },
      'collector-desc': (a: any, b: any) => {
        const aNum = parseInt(a.collectorNumber) || 0;
        const bNum = parseInt(b.collectorNumber) || 0;
        return bNum - aNum;
      },
      'name-asc': (a: any, b: any) => a.name.localeCompare(b.name),
      'name-desc': (a: any, b: any) => b.name.localeCompare(a.name),
      'rarity-asc': (a: any, b: any) => {
        const rarityOrder = { common: 1, uncommon: 2, rare: 3, mythic: 4 };
        return (rarityOrder[a.rarity as keyof typeof rarityOrder] || 0) - 
               (rarityOrder[b.rarity as keyof typeof rarityOrder] || 0);
      },
      'rarity-desc': (a: any, b: any) => {
        const rarityOrder = { common: 1, uncommon: 2, rare: 3, mythic: 4 };
        return (rarityOrder[b.rarity as keyof typeof rarityOrder] || 0) - 
               (rarityOrder[a.rarity as keyof typeof rarityOrder] || 0);
      }
    },
    filterFunctions: {
      rarities: (item: any, selectedRarities: string[]) => {
        if (!selectedRarities || selectedRarities.length === 0) return true;
        return selectedRarities.includes(item.rarity);
      },
      artists: (item: any, selectedArtists: string[]) => {
        if (!selectedArtists || selectedArtists.length === 0) return true;
        return selectedArtists.includes(item.artist);
      },
      showDigital: (item: any, showDigital: boolean) => {
        if (!showDigital) return !item.digital;
        return true;
      }
    },
    defaultSort: 'collector-asc'
  }
};

/**
 * Helper function to create custom configurations based on presets
 */
export function createUrlFilterConfig<T>(
  preset: keyof typeof presets,
  overrides: Partial<UrlFilterConfig<T>> = {}
): UrlFilterConfig<T> {
  const baseConfig = presets[preset];
  return { ...baseConfig, ...overrides } as UrlFilterConfig<T>;
}