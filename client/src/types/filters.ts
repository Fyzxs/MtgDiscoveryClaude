/**
 * Common filter-related type definitions used across the application
 */

/**
 * Sort direction for ordering items
 */
export type SortDirection = 'asc' | 'desc';

/**
 * A single sort option in a dropdown
 */
export interface SortOption {
  value: string;
  label: string;
  /** Optional condition to show this option */
  condition?: boolean;
}

/**
 * Multi-select option for dropdowns
 */
export interface MultiSelectOption {
  value: string;
  label: string;
  chipColor?: 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';
}

/**
 * Search input configuration
 */
export interface SearchConfig {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  debounceMs?: number;
  minWidth?: number | string;
  fullWidth?: boolean;
}

/**
 * Multi-select dropdown configuration
 */
export interface MultiSelectConfig {
  key: string;
  value: string[];
  onChange: (value: string[]) => void;
  options: string[] | MultiSelectOption[];
  label: string;
  placeholder?: string;
  minWidth?: number | string;
  fullWidth?: boolean;
}

/**
 * Autocomplete configuration
 */
export interface AutocompleteConfig {
  key: string;
  value: string[];
  onChange: (value: string[]) => void;
  options: string[];
  label: string;
  placeholder?: string;
  minWidth?: number | string;
  renderTags?: boolean;
  getOptionLabel?: (option: string) => string;
}

/**
 * Sort dropdown configuration
 */
export interface SortConfig {
  value: string;
  onChange: (value: string) => void;
  options: SortOption[];
  label?: string;
  minWidth?: number | string;
  fullWidth?: boolean;
}

/**
 * Complete filter panel configuration
 */
export interface FilterPanelConfig {
  search?: SearchConfig;
  multiSelects?: MultiSelectConfig[];
  autocompletes?: AutocompleteConfig[];
  sort?: SortConfig;
  customFilters?: React.ReactNode[];
}

/**
 * Filter state for managing filters
 */
export interface FilterState {
  [key: string]: any;
}

/**
 * Filter function type
 */
export type FilterFunction<T> = (item: T, value: any) => boolean;

/**
 * Sort function type
 */
export type SortFunction<T> = (a: T, b: T) => number;

/**
 * Configuration for useFilterState hook
 */
export interface UseFilterStateConfig<T> {
  searchFields?: (keyof T)[];
  sortOptions?: Record<string, SortFunction<T>>;
  filterFunctions?: Record<string, FilterFunction<T>>;
  defaultSort?: string;
}

/**
 * Initial values for filter state
 */
export interface FilterInitialValues {
  search?: string;
  sort?: string;
  filters?: FilterState;
}

/**
 * Common filter layouts
 */
export type FilterLayout = 'horizontal' | 'vertical' | 'compact';