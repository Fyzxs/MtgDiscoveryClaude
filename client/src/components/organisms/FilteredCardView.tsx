import React, { useState, useMemo } from 'react';
import { Box, Typography, CircularProgress } from '@mui/material';
import { SearchBar, SearchFilter } from '../molecules/shared/SearchBar';
import { FilterBar, FilterOption } from '../molecules/shared/FilterBar';
import { CardGridSection } from '../molecules/shared/CardGrid';
import { ScrollToTop } from '../atoms/shared/ScrollToTop';

export interface FilteredItem {
  id: string;
  [key: string]: any;
}

export interface FilterConfig {
  key: string;
  title: string;
  options: FilterOption[];
  onToggle: (key: string, active: boolean) => void;
  showCounts?: boolean;
  multiSelect?: boolean;
}

interface FilteredCardViewProps<T extends FilteredItem> {
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearch: (query: string) => void;
  searchPlaceholder?: string;
  filters?: FilterConfig[];
  activeSearchFilters?: SearchFilter[];
  onSearchFilterRemove?: (filterKey: string) => void;
  isLoading?: boolean;
  emptyState?: React.ReactNode;
  title?: string;
  subtitle?: string;
  showHeader?: boolean;
  showCount?: boolean;
  enableScrollToTop?: boolean;
  className?: string;
}

/**
 * Unified organism for displaying filterable card collections
 * Combines search, filters, and grid display patterns
 */
export const FilteredCardView = <T extends FilteredItem>({
  items,
  renderItem,
  searchValue,
  onSearchChange,
  onSearch,
  searchPlaceholder = "Search...",
  filters = [],
  activeSearchFilters = [],
  onSearchFilterRemove,
  isLoading = false,
  emptyState,
  title,
  subtitle,
  showHeader = true,
  showCount = true,
  enableScrollToTop = true,
  className = ''
}: FilteredCardViewProps<T>) => {
  const [selectedItems, setSelectedItems] = useState<Set<string>>(new Set());

  const hasActiveFilters = useMemo(() => {
    return filters.some(filter => filter.options.some(option => option.isActive)) ||
           activeSearchFilters.length > 0 ||
           searchValue.trim().length > 0;
  }, [filters, activeSearchFilters, searchValue]);

  const displayTitle = useMemo(() => {
    if (!title) return '';
    return showCount ? `${title} (${items.length})` : title;
  }, [title, items.length, showCount]);

  if (isLoading) {
    return (
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        minHeight: 200,
        flexDirection: 'column',
        gap: 2
      }}>
        <CircularProgress />
        <Typography color="text.secondary">Loading...</Typography>
      </Box>
    );
  }

  if (items.length === 0) {
    if (emptyState) {
      return <>{emptyState}</>;
    }

    return (
      <Box sx={{ 
        textAlign: 'center', 
        py: 8,
        color: 'text.secondary'
      }}>
        <Typography variant="h6" gutterBottom>
          {hasActiveFilters ? 'No results found' : 'No items available'}
        </Typography>
        <Typography variant="body2">
          {hasActiveFilters 
            ? 'Try adjusting your search or filters'
            : 'Check back later for new items'
          }
        </Typography>
      </Box>
    );
  }

  return (
    <Box className={className}>
      {/* Search Bar */}
      <SearchBar
        searchValue={searchValue}
        onSearchChange={onSearchChange}
        onSearch={onSearch}
        placeholder={searchPlaceholder}
        filters={activeSearchFilters}
        onFilterRemove={onSearchFilterRemove}
      />

      {/* Filter Bar */}
      {filters.length > 0 && (
        <FilterBar
          sections={filters}
          title="Filters"
        />
      )}

      {/* Results Header */}
      {(searchValue || hasActiveFilters) && (
        <Box sx={{ mb: 2 }}>
          <Typography variant="h6" color="text.secondary">
            {searchValue ? (
              <>
                Found <strong>{items.length}</strong> results for "{searchValue}"
                {hasActiveFilters && ' (filtered)'}
              </>
            ) : (
              <>
                Showing <strong>{items.length}</strong> filtered results
              </>
            )}
          </Typography>
        </Box>
      )}

      {/* Card Grid */}
      <CardGridSection
        title={displayTitle}
        subtitle={subtitle}
        items={items}
        renderItem={renderItem}
        showHeader={showHeader && !!title}
        showCount={false} // We handle count in the title
      />

      {/* Scroll to Top */}
      {enableScrollToTop && <ScrollToTop />}
    </Box>
  );
};

interface SimpleFilteredViewProps<T extends FilteredItem> {
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  searchValue: string;
  onSearchChange: (value: string) => void;
  isLoading?: boolean;
  title?: string;
  className?: string;
}

/**
 * Simplified filtered view with just search (no complex filters)
 */
export const SimpleFilteredView = <T extends FilteredItem>({
  items,
  renderItem,
  searchValue,
  onSearchChange,
  isLoading = false,
  title,
  className = ''
}: SimpleFilteredViewProps<T>) => {
  const handleSearch = (query: string) => {
    onSearchChange(query);
  };

  return (
    <FilteredCardView
      items={items}
      renderItem={renderItem}
      searchValue={searchValue}
      onSearchChange={onSearchChange}
      onSearch={handleSearch}
      isLoading={isLoading}
      title={title}
      className={className}
    />
  );
};