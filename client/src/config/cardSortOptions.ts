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
// - Current strategy: NUMERIC PREFIX EXTRACTION (^(\d+))
//
// DIFFERENCE FROM GROUPING:
// - SORTING: Extracts numeric prefix for natural ordering ("317★" sorts as 317)
// - GROUPING: Uses strict matching to prevent special variants from matching ranges
// - See getNumericValue() in optimizedCardGrouping.ts for range matching behavior
//
// CURRENT SORTING BEHAVIOR:
// - "317"        -> sorted as 634 (317 * 2, pure numeric comes first)
// - "317★"       -> sorted as 635 (317 * 2 + 1, suffixed comes after)
// - "318"        -> sorted as 636 (318 * 2, pure numeric comes first) 
// - "318a"       -> sorted as 637 (318 * 2 + 1, suffixed comes after)
// - "DMR-271"    -> sorted last (999999) if no numeric prefix
export const parseCollectorNumber = (num: string): number => {
  // Extract numeric prefix - allows special characters after digits
  const match = num.match(/^(\d+)(.*)$/);
  if (!match) return 999999;
  
  const numericValue = parseInt(match[1], 10);
  const suffix = match[2];
  
  // Pure numeric gets even number, suffixed gets odd number (comes after)
  return numericValue * 2 + (suffix ? 1 : 0);
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
  userCollection?: Array<{
    finish: string;
    special: string;
    count: number;
  }> | {
    finish: string;
    special: string;
    count: number;
  };
}

export type SortOption<T = CardLike> = (a: T, b: T) => number;

export const createCardSortOptions = <T extends CardLike>(): Record<string, SortOption<T>> => ({
  'collector-asc': (a, b) => parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || ''),
  'collector-desc': (a, b) => parseCollectorNumber(b.collectorNumber || '') - parseCollectorNumber(a.collectorNumber || ''),
  'name-asc': (a, b) => a.name.localeCompare(b.name),
  'name-desc': (a, b) => b.name.localeCompare(a.name),
  'rarity-asc': (a, b) => {
    return (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99);
  },
  'rarity-desc': (a, b) => {
    return (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99);
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
  },
  'collection-asc': (a, b) => {
    const getTotal = (card: CardLike) => {
      if (!card.userCollection) return 0;
      const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
      return collectionArray.reduce((sum, item) => sum + item.count, 0);
    };
    return getTotal(a) - getTotal(b);
  },
  'collection-desc': (a, b) => {
    const getTotal = (card: CardLike) => {
      if (!card.userCollection) return 0;
      const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
      return collectionArray.reduce((sum, item) => sum + item.count, 0);
    };
    return getTotal(b) - getTotal(a);
  }
});

// All available sort options
export const ALL_SORT_OPTIONS = {
  'collector-asc': { value: 'collector-asc', label: 'Collector # (Low-High)' },
  'collector-desc': { value: 'collector-desc', label: 'Collector # (High-Low)' },
  'name-asc': { value: 'name-asc', label: 'Name (A-Z)' },
  'name-desc': { value: 'name-desc', label: 'Name (Z-A)' },
  'rarity-asc': { value: 'rarity-asc', label: 'Rarity (C-M)' },
  'rarity-desc': { value: 'rarity-desc', label: 'Rarity (M-C)' },
  'price-desc': { value: 'price-desc', label: 'Price (High-Low)' },
  'price-asc': { value: 'price-asc', label: 'Price (Low-High)' },
  'release-desc': { value: 'release-desc', label: 'Release Date (Newest)' },
  'release-asc': { value: 'release-asc', label: 'Release Date (Oldest)' },
  'set-asc': { value: 'set-asc', label: 'Set Name (A-Z)' },
  'set-desc': { value: 'set-desc', label: 'Set Name (Z-A)' },
  'collection-asc': { value: 'collection-asc', label: 'Collection Count (Low-High)' },
  'collection-desc': { value: 'collection-desc', label: 'Collection Count (High-Low)' }
};

// Page-specific sort options
export const SET_PAGE_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['collector-asc'],
  ALL_SORT_OPTIONS['collector-desc'],
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['rarity-asc'],
  ALL_SORT_OPTIONS['rarity-desc'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc'],
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc']
];

// Set page sort options with collection count (for collector view)
export const SET_PAGE_COLLECTOR_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['collection-desc'],
  ALL_SORT_OPTIONS['collection-asc'],
  ALL_SORT_OPTIONS['collector-asc'],
  ALL_SORT_OPTIONS['collector-desc'],
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['rarity-asc'],
  ALL_SORT_OPTIONS['rarity-desc'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc'],
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc']
];

// Artist page sort options
export const ARTIST_PAGE_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc'],
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['rarity-asc'],
  ALL_SORT_OPTIONS['rarity-desc'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc'],
  ALL_SORT_OPTIONS['set-asc']
];

// Artist page sort options with collection count (for collector view)
export const ARTIST_PAGE_COLLECTOR_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['collection-desc'],
  ALL_SORT_OPTIONS['collection-asc'],
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc'],
  ALL_SORT_OPTIONS['name-asc'],
  ALL_SORT_OPTIONS['name-desc'],
  ALL_SORT_OPTIONS['rarity-asc'],
  ALL_SORT_OPTIONS['rarity-desc'],
  ALL_SORT_OPTIONS['price-desc'],
  ALL_SORT_OPTIONS['price-asc'],
  ALL_SORT_OPTIONS['set-asc']
];

// Card detail page sort options (no collector number or name sorting)
export const CARD_DETAIL_SORT_OPTIONS = [
  ALL_SORT_OPTIONS['release-desc'],
  ALL_SORT_OPTIONS['release-asc'],
  ALL_SORT_OPTIONS['set-asc'],
  ALL_SORT_OPTIONS['set-desc'],
  ALL_SORT_OPTIONS['rarity-asc'],
  ALL_SORT_OPTIONS['rarity-desc'],
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