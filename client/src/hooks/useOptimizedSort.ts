import { useMemo, useRef } from 'react';
import type { Card } from '../types/card';

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
  
  return `${data.length}-${first}-${middle}-${last}`;
}

export function useOptimizedSort(
  data: Card[] | undefined, 
  sortKey: string, 
  sortFn: (a: Card, b: Card) => number
) {
  const cacheRef = useRef<SortCache>({});
  
  return useMemo(() => {
    if (!data || data.length === 0) return [];
    
    // Small arrays don't need caching optimization
    if (data.length < 100) {
      return [...data].sort(sortFn);
    }
    
    // Generate cache key and data hash
    const dataHash = generateDataHash(data);
    const cacheKey = `${sortKey}-${dataHash}`;
    const cached = cacheRef.current[cacheKey];
    
    // Return cached result if data hasn't changed and cache is recent (within 5 minutes)
    if (cached && 
        cached.dataHash === dataHash && 
        (Date.now() - cached.timestamp) < 300000) {
      return cached.result;
    }
    
    // Perform sort operation
    console.log(`Sorting ${data.length} cards with key: ${sortKey}`);
    const startTime = performance.now();
    
    const sorted = [...data].sort(sortFn);
    
    const endTime = performance.now();
    console.log(`Sort completed in ${(endTime - startTime).toFixed(2)}ms`);
    
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