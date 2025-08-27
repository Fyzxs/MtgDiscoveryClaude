import { useState, useEffect, useMemo, useCallback, useRef } from 'react';
import type {
  UseFilterStateConfig,
  FilterState as FilterStateType,
  FilterFunction,
  SortFunction
} from '../types/filters';

// Re-export for backward compatibility
export type FilterConfig<T> = UseFilterStateConfig<T>;

// Keep internal FilterState interface for this hook
export interface FilterState {
  search: string;
  sort: string;
  filters: Record<string, any>;
}

/**
 * Hook to manage filtering, searching, and sorting of data
 */
export function useFilterState<T>(
  data: T[] | undefined,
  config: FilterConfig<T>,
  initialState?: Partial<FilterState>
) {
  // Use refs to store config to avoid dependency issues
  const configRef = useRef(config);
  configRef.current = config;
  
  const {
    searchFields = [],
    sortOptions = {},
    filterFunctions = {},
    defaultSort = ''
  } = configRef.current;

  // Initialize state
  const [searchTerm, setSearchTerm] = useState(initialState?.search || '');
  const [sortBy, setSortBy] = useState(initialState?.sort || defaultSort);
  const [filters, setFilters] = useState<Record<string, any>>(initialState?.filters || {});
  const [filteredData, setFilteredData] = useState<T[]>([]);

  // Generic filter update function
  const updateFilter = useCallback((filterName: string, value: any) => {
    setFilters(prev => ({
      ...prev,
      [filterName]: value
    }));
  }, []);

  // Clear a specific filter
  const clearFilter = useCallback((filterName: string) => {
    setFilters(prev => {
      const newFilters = { ...prev };
      delete newFilters[filterName];
      return newFilters;
    });
  }, []);

  // Clear all filters
  const clearAllFilters = useCallback(() => {
    setSearchTerm('');
    setFilters({});
    setSortBy(defaultSort);
  }, [defaultSort]);

  // Check if any filters are active
  const hasActiveFilters = useMemo(() => {
    return searchTerm !== '' || 
           Object.keys(filters).length > 0 || 
           sortBy !== defaultSort;
  }, [searchTerm, filters, sortBy, defaultSort]);

  // Apply filtering and sorting
  useEffect(() => {
    if (!data) {
      setFilteredData([]);
      return;
    }

    let filtered = [...data];

    // Apply search filter
    if (searchTerm && searchFields.length > 0) {
      const searchLower = searchTerm.toLowerCase();
      filtered = filtered.filter(item => 
        searchFields.some(field => {
          const value = item[field];
          if (value === null || value === undefined) return false;
          return String(value).toLowerCase().includes(searchLower);
        })
      );
    }

    // Apply custom filters
    Object.entries(filters).forEach(([filterName, filterValue]) => {
      if (filterValue === undefined || filterValue === null) return;
      
      // Handle array filters (like multi-select)
      if (Array.isArray(filterValue) && filterValue.length === 0) return;
      
      const filterFunction = filterFunctions[filterName];
      if (filterFunction) {
        filtered = filtered.filter(item => filterFunction(item, filterValue));
      }
    });

    // Apply sorting
    if (sortBy && sortOptions[sortBy]) {
      filtered.sort(sortOptions[sortBy]);
    }

    setFilteredData(filtered);
  }, [data, searchTerm, filters, sortBy]); // Config is accessed via ref, not dependencies

  return {
    // State values
    searchTerm,
    sortBy,
    filters,
    filteredData,
    
    // State setters
    setSearchTerm,
    setSortBy,
    updateFilter,
    clearFilter,
    clearAllFilters,
    
    // Helper values
    hasActiveFilters,
    totalCount: data?.length || 0,
    filteredCount: filteredData.length
  };
}

/**
 * Preset filter functions for common use cases
 */
export const commonFilters = {
  // Multi-select filter (item value must be in selected array)
  multiSelect: <T>(field: keyof T) => (item: T, selectedValues: string[]) => {
    if (!selectedValues || selectedValues.length === 0) return true;
    return selectedValues.includes(String(item[field]));
  },
  
  // Range filter
  range: <T>(field: keyof T, min?: number, max?: number) => (item: T) => {
    const value = Number(item[field]);
    if (isNaN(value)) return false;
    if (min !== undefined && value < min) return false;
    if (max !== undefined && value > max) return false;
    return true;
  },
  
  // Boolean filter
  boolean: <T>(field: keyof T) => (item: T, value: boolean) => {
    return Boolean(item[field]) === value;
  },
  
  // Contains filter for arrays
  contains: <T>(field: keyof T) => (item: T, searchValue: string) => {
    const fieldValue = item[field];
    if (!fieldValue) return false;
    if (Array.isArray(fieldValue)) {
      return fieldValue.some(v => 
        String(v).toLowerCase().includes(searchValue.toLowerCase())
      );
    }
    return String(fieldValue).toLowerCase().includes(searchValue.toLowerCase());
  }
};

/**
 * Preset sort functions for common use cases
 */
export const commonSorts = {
  // Alphabetical sort
  alphabetical: <T>(field: keyof T, ascending = true) => (a: T, b: T) => {
    const result = String(a[field]).localeCompare(String(b[field]));
    return ascending ? result : -result;
  },
  
  // Numeric sort
  numeric: <T>(field: keyof T, ascending = true) => (a: T, b: T) => {
    const result = Number(a[field]) - Number(b[field]);
    return ascending ? result : -result;
  },
  
  // Date sort
  date: <T>(field: keyof T, ascending = true) => (a: T, b: T) => {
    const dateA = new Date(String(a[field])).getTime();
    const dateB = new Date(String(b[field])).getTime();
    const result = dateA - dateB;
    return ascending ? result : -result;
  }
};