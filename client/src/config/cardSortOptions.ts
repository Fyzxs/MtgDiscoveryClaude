export const RARITY_ORDER: Record<string, number> = {
  common: 1,
  uncommon: 2,
  rare: 3,
  mythic: 4,
  special: 5,
  bonus: 6
};

// Parse collector number for sorting purposes
//
// COLLECTOR NUMBER SORTING STRATEGY:
// - This function determines sort order for collector numbers
// - Current strategy: STRICT NUMERIC ONLY (^(\d+)$)
//
// KNOWN ISSUES BY SET:
// - SET:40K - Special surge foil cards like "317★" should sort separately from "317"
//   Solution: Non-numeric collector numbers get high sort value (999999) to sort last
//
// - CROSS-REFERENCE: See getNumericValue() in optimizedCardGrouping.ts for range matching
//   Both functions use the same parsing strategy for consistency
//
// CURRENT SORTING BEHAVIOR:
// - "1", "2", "317" -> sorted numerically (1, 2, 317)
// - "317★", "42★"   -> sorted last (999999) as special variants
// - "42a", "DMR-271" -> sorted last (999999) as non-numeric
export const parseCollectorNumber = (num: string): number => {
  // Only treat as numeric if it's purely digits (no special characters like ★)
  const match = num.match(/^(\d+)$/);
  return match ? parseInt(match[1], 10) : 999999;
};

export interface CardLike {
  id: string;
  name: string;
  collectorNumber?: string;
  rarity?: string;
  artist?: string;
  digital?: boolean;
  prices?: {
    usd?: string | null;
    usdFoil?: string | null;
  };
  releasedAt?: string;
  setName?: string;
  setCode?: string;
}

export type SortOption<T = CardLike> = (a: T, b: T) => number;

export const createCardSortOptions = <T extends CardLike>(): Record<string, SortOption<T>> => ({
  'collector-asc': (a, b) => parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || ''),
  'collector-desc': (a, b) => parseCollectorNumber(b.collectorNumber || '') - parseCollectorNumber(a.collectorNumber || ''),
  'name-asc': (a, b) => a.name.localeCompare(b.name),
  'name-desc': (a, b) => b.name.localeCompare(a.name),
  'rarity': (a, b) => {
    return (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99);
  },
  'price-desc': (a, b) => {
    const priceA = parseFloat(a.prices?.usd || '0');
    const priceB = parseFloat(b.prices?.usd || '0');
    return priceB - priceA;
  },
  'price-asc': (a, b) => {
    const priceA = parseFloat(a.prices?.usd || '0');
    const priceB = parseFloat(b.prices?.usd || '0');
    return priceA - priceB;
  },
  'release-desc': (a, b) => {
    if (!a.releasedAt && !b.releasedAt) return 0;
    if (!a.releasedAt) return 1;
    if (!b.releasedAt) return -1;
    return new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime();
  },
  'release-asc': (a, b) => {
    if (!a.releasedAt && !b.releasedAt) return 0;
    if (!a.releasedAt) return 1;
    if (!b.releasedAt) return -1;
    return new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime();
  },
  'set-asc': (a, b) => {
    const setA = a.setName || '';
    const setB = b.setName || '';
    return setA.localeCompare(setB);
  },
  'set-desc': (a, b) => {
    const setA = a.setName || '';
    const setB = b.setName || '';
    return setB.localeCompare(setA);
  }
});

// All available sort options
export const ALL_SORT_OPTIONS = {
  'collector-asc': { value: 'collector-asc', label: 'Collector # (Low-High)' },
  'collector-desc': { value: 'collector-desc', label: 'Collector # (High-Low)' },
  'name-asc': { value: 'name-asc', label: 'Name (A-Z)' },
  'name-desc': { value: 'name-desc', label: 'Name (Z-A)' },
  'rarity': { value: 'rarity', label: 'Rarity' },
  'price-desc': { value: 'price-desc', label: 'Price (High-Low)' },
  'price-asc': { value: 'price-asc', label: 'Price (Low-High)' },
  'release-desc': { value: 'release-desc', label: 'Release Date (Newest)' },
  'release-asc': { value: 'release-asc', label: 'Release Date (Oldest)' },
  'set-asc': { value: 'set-asc', label: 'Set Name (A-Z)' },
  'set-desc': { value: 'set-desc', label: 'Set Name (Z-A)' }
};

// Page-specific sort options
export const SET_PAGE_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['collector-asc'],
  ALL_SORT_OPTIONS['collector-desc'],
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['rarity'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc'],
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc']
];

// Card detail page sort options (no collector number or name sorting)
export const CARD_DETAIL_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc'],
  ALL_SORT_OPTIONS['set-asc'],
  ALL_SORT_OPTIONS['set-desc'],
  ALL_SORT_OPTIONS['rarity'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc']
];

// All sets page sort options (if needed)
export const ALL_SETS_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc']
];