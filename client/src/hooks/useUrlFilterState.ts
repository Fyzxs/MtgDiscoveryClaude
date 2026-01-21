import { useMemo } from 'react';
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
  filters?: Record<string, unknown>;
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
      search: (typeof initialValues[urlParams.search || 'search'] === 'string' ? initialValues[urlParams.search || 'search'] : (defaults.search || '')) as string,
      sort: (typeof initialValues[urlParams.sort || 'sort'] === 'string' ? initialValues[urlParams.sort || 'sort'] : (defaults.sort || filterConfig.defaultSort || '')) as string,
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
    const state: Record<string, unknown> = {
      [urlParams.sort || 'sort']: filterState.sortBy
    };
    
    Object.entries(filterState.filters).forEach(([key, value]) => {
      const urlKey = urlParams.filters?.[key] || key;
      state[urlKey] = value;
    });
    
    return state;
  }, [filterState.sortBy, filterState.filters, urlParams]);

  const urlSyncConfig = useMemo(() => {
    const config: Record<string, { default: unknown }> = {
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
      'release-desc': <T extends { releasedAt: string }>(a: T, b: T) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime(),
      'release-asc': <T extends { releasedAt: string }>(a: T, b: T) => new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime(),
      'name-asc': <T extends { name: string }>(a: T, b: T) => a.name.localeCompare(b.name),
      'name-desc': <T extends { name: string }>(a: T, b: T) => b.name.localeCompare(a.name),
      'cards-desc': <T extends { cardCount?: number }>(a: T, b: T) => (b.cardCount || 0) - (a.cardCount || 0),
      'cards-asc': <T extends { cardCount?: number }>(a: T, b: T) => (a.cardCount || 0) - (b.cardCount || 0)
    },
    filterFunctions: {
      setTypes: <T extends { setType: string }>(item: T, selectedTypes: string[]) => {
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
      'collector-asc': <T extends { collectorNumber: string }>(a: T, b: T) => {
        const aNum = parseInt(a.collectorNumber) || 0;
        const bNum = parseInt(b.collectorNumber) || 0;
        return aNum - bNum;
      },
      'collector-desc': <T extends { collectorNumber: string }>(a: T, b: T) => {
        const aNum = parseInt(a.collectorNumber) || 0;
        const bNum = parseInt(b.collectorNumber) || 0;
        return bNum - aNum;
      },
      'name-asc': <T extends { name: string }>(a: T, b: T) => a.name.localeCompare(b.name),
      'name-desc': <T extends { name: string }>(a: T, b: T) => b.name.localeCompare(a.name),
      'rarity-asc': <T extends { rarity: string }>(a: T, b: T) => {
        const rarityOrder = { common: 1, uncommon: 2, rare: 3, mythic: 4 };
        return (rarityOrder[a.rarity as keyof typeof rarityOrder] || 0) -
               (rarityOrder[b.rarity as keyof typeof rarityOrder] || 0);
      },
      'rarity-desc': <T extends { rarity: string }>(a: T, b: T) => {
        const rarityOrder = { common: 1, uncommon: 2, rare: 3, mythic: 4 };
        return (rarityOrder[b.rarity as keyof typeof rarityOrder] || 0) -
               (rarityOrder[a.rarity as keyof typeof rarityOrder] || 0);
      }
    },
    filterFunctions: {
      rarities: <T extends { rarity: string }>(item: T, selectedRarities: string[]) => {
        if (!selectedRarities || selectedRarities.length === 0) return true;
        return selectedRarities.includes(item.rarity);
      },
      artists: <T extends { artist: string }>(item: T, selectedArtists: string[]) => {
        if (!selectedArtists || selectedArtists.length === 0) return true;
        return selectedArtists.includes(item.artist);
      },
      showDigital: <T extends { digital?: boolean }>(item: T, showDigital: boolean) => {
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