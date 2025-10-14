import { useMemo, useRef } from 'react';
import type { Card } from '../types/card';
import { logger } from '../utils/logger';

interface SortCache {
  [key: string]: {
    data: Card[];
    result: Card[];
    timestamp: number;
    dataHash: string;
  };
}

// Generate a lightweight hash for the data array to detect changes
function generateDataHash(data: Card[]): string {
  if (!data || data.length === 0) return 'empty';

  // Use first, middle, and last card IDs plus length as a fingerprint
  const first = data[0]?.id || '';
  const middle = data[Math.floor(data.length / 2)]?.id || '';
  const last = data[data.length - 1]?.id || '';

  // Include userCollection in hash to detect collection updates
  // Check if any cards have userCollection data
  const hasCollection = data[0]?.userCollection || data[Math.floor(data.length / 2)]?.userCollection || data[data.length - 1]?.userCollection;
  const collectionHash = hasCollection ? JSON.stringify(hasCollection) : 'none';

  return `${data.length}-${first}-${middle}-${last}-${collectionHash}`;
}

export function useOptimizedSort(
  data: Card[] | undefined,
  sortKey: string,
  sortFn: (a: Card, b: Card) => number
) {
  const cacheRef = useRef<SortCache>({});

  return useMemo(() => {
    if (!data || data.length === 0) return [];

    // Disable caching if sortKey contains collector context (ctor)
    // Collection data changes without changing card IDs, breaking hash-based cache
    const hasCollectorContext = sortKey.includes('ctor');

    // Small arrays don't need caching optimization, or if collector context present
    if (data.length < 100 || hasCollectorContext) {
      return [...data].sort(sortFn);
    }

    // Generate cache key and data hash
    const dataHash = generateDataHash(data);
    const cacheKey = `${sortKey}-${dataHash}`;
    const cached = cacheRef.current[cacheKey];

    // Also check if the data array reference changed (important for collection updates)
    const dataRefChanged = cached && cached.data !== data;

    // Return cached result if data hasn't changed and cache is recent (within 5 minutes)
    if (cached &&
        cached.dataHash === dataHash &&
        !dataRefChanged &&
        (Date.now() - cached.timestamp) < 300000) {
      logger.debug(`[useOptimizedSort] Cache HIT for key: ${cacheKey}`);
      logger.debug('[useOptimizedSort] Returning cached cards - first card:', cached.result[0]);
      logger.debug('[useOptimizedSort] Cached card userCollection:', cached.result[0]?.userCollection);
      return cached.result;
    }

    if (dataRefChanged) {
      logger.debug(`[useOptimizedSort] Cache MISS - data reference changed`);
    } else {
      logger.debug(`[useOptimizedSort] Cache MISS for key: ${cacheKey}`);
    }
    logger.debug('[useOptimizedSort] Input data - first card:', data[0]);
    logger.debug('[useOptimizedSort] Input card userCollection:', data[0]?.userCollection);

    // Perform sort operation
    logger.debug(`Sorting ${data.length} cards with key: ${sortKey}`);
    const startTime = performance.now();

    const sorted = [...data].sort(sortFn);

    const endTime = performance.now();
    logger.debug(`Sort completed in ${(endTime - startTime).toFixed(2)}ms`);

    // Cache the result
    cacheRef.current[cacheKey] = {
      data,
      result: sorted,
      timestamp: Date.now(),
      dataHash
    };

    // Clean old cache entries (keep only last 5 to manage memory)
    const entries = Object.entries(cacheRef.current);
    if (entries.length > 5) {
      entries.sort((a, b) => b[1].timestamp - a[1].timestamp);
      cacheRef.current = Object.fromEntries(entries.slice(0, 5));
    }

    return sorted;
  }, [data, sortKey, sortFn]);
}

// Additional hook for batch sorting operations (when multiple sorts might be needed)
export function useOptimizedBatchSort() {
  const cacheRef = useRef<SortCache>({});
  
  const sortWithCache = useMemo(() => {
    return (data: Card[], sortKey: string, sortFn: (a: Card, b: Card) => number): Card[] => {
      if (!data || data.length === 0) return [];
      
      // Small arrays don't need caching
      if (data.length < 100) {
        return [...data].sort(sortFn);
      }
      
      const dataHash = generateDataHash(data);
      const cacheKey = `${sortKey}-${dataHash}`;
      const cached = cacheRef.current[cacheKey];
      
      if (cached && 
          cached.dataHash === dataHash && 
          (Date.now() - cached.timestamp) < 300000) {
        return cached.result;
      }
      
      const sorted = [...data].sort(sortFn);
      
      cacheRef.current[cacheKey] = {
        data,
        result: sorted,
        timestamp: Date.now(),
        dataHash
      };
      
      // Memory management
      const entries = Object.entries(cacheRef.current);
      if (entries.length > 10) {
        entries.sort((a, b) => b[1].timestamp - a[1].timestamp);
        cacheRef.current = Object.fromEntries(entries.slice(0, 10));
      }
      
      return sorted;
    };
  }, []);
  
  const clearCache = useMemo(() => {
    return () => {
      cacheRef.current = {};
    };
  }, []);
  
  return { sortWithCache, clearCache };
}