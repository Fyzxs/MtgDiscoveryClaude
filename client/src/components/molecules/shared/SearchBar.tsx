import React, { useState, useEffect } from 'react';
import { 
  Box, 
  Paper, 
  Typography, 
  Chip, 
  Stack,
  SxProps,
  Theme
} from '@mui/material';
import { SearchInput } from '../../atoms/shared/SearchInput';

export interface SearchFilter {
  key: string;
  label: string;
  value: string;
  removable?: boolean;
}

interface SearchBarProps {
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearch: (query: string) => void;
  placeholder?: string;
  label?: string;
  filters?: SearchFilter[];
  onFilterRemove?: (filterKey: string) => void;
  suggestions?: string[];
  onSuggestionSelect?: (suggestion: string) => void;
  showFilters?: boolean;
  debounceMs?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Enhanced search bar with filters, suggestions, and debounced search
 * Combines SearchInput with additional search functionality
 */
export const SearchBar: React.FC<SearchBarProps> = React.memo(({
  searchValue,
  onSearchChange,
  onSearch,
  placeholder = "Search...",
  label = "Search",
  filters = [],
  onFilterRemove,
  suggestions = [],
  onSuggestionSelect,
  showFilters = true,
  debounceMs = 300,
  sx = {},
  className = ''
}) => {
  const [debouncedValue, setDebouncedValue] = useState(searchValue);

  // Debounce search input
  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(searchValue);
    }, debounceMs);

    return () => clearTimeout(timer);
  }, [searchValue, debounceMs]);

  // Trigger search when debounced value changes
  useEffect(() => {
    if (debouncedValue !== searchValue) {
      onSearch(debouncedValue);
    }
  }, [debouncedValue, onSearch, searchValue]);

  const handleSearch = () => {
    onSearch(searchValue);
  };

  const hasActiveFilters = filters.length > 0;

  return (
    <Paper
      className={className}
      sx={{
        p: 2,
        mb: 2,
        ...sx
      }}
    >
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        {/* Search Input */}
        <SearchInput
          value={searchValue}
          onChange={onSearchChange}
          onSubmit={handleSearch}
          placeholder={placeholder}
          label={label}
          expandable={false}
        />

        {/* Active Filters */}
        {showFilters && hasActiveFilters && (
          <Box>
            <Typography variant="subtitle2" gutterBottom color="text.secondary">
              Active Filters:
            </Typography>
            <Stack direction="row" spacing={1} flexWrap="wrap" sx={{ gap: 1 }}>
              {filters.map((filter) => (
                <Chip
                  key={filter.key}
                  label={`${filter.label}: ${filter.value}`}
                  variant="outlined"
                  size="small"
                  onDelete={filter.removable !== false && onFilterRemove ? () => onFilterRemove(filter.key) : undefined}
                  sx={{
                    '& .MuiChip-deleteIcon': {
                      fontSize: '16px'
                    }
                  }}
                />
              ))}
            </Stack>
          </Box>
        )}

        {/* Search Suggestions */}
        {suggestions.length > 0 && onSuggestionSelect && (
          <Box>
            <Typography variant="subtitle2" gutterBottom color="text.secondary">
              Suggestions:
            </Typography>
            <Stack direction="row" spacing={1} flexWrap="wrap" sx={{ gap: 1 }}>
              {suggestions.slice(0, 8).map((suggestion, index) => (
                <Chip
                  key={index}
                  label={suggestion}
                  variant="outlined"
                  size="small"
                  clickable
                  onClick={() => onSuggestionSelect(suggestion)}
                  sx={{
                    '&:hover': {
                      backgroundColor: 'primary.light',
                      color: 'primary.contrastText'
                    }
                  }}
                />
              ))}
            </Stack>
          </Box>
        )}
      </Box>
    </Paper>
  );
});

interface QuickSearchBarProps {
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearch: (query: string) => void;
  placeholder?: string;
  label?: string;
  debounceMs?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Minimal search bar without paper wrapper for inline use
 */
export const QuickSearchBar: React.FC<QuickSearchBarProps> = React.memo(({
  searchValue,
  onSearchChange,
  onSearch,
  placeholder = "Search...",
  label = "Search",
  debounceMs = 300,
  sx = {},
  className = ''
}) => {
  const [debouncedValue, setDebouncedValue] = useState(searchValue);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(searchValue);
    }, debounceMs);

    return () => clearTimeout(timer);
  }, [searchValue, debounceMs]);

  useEffect(() => {
    if (debouncedValue !== searchValue) {
      onSearch(debouncedValue);
    }
  }, [debouncedValue, onSearch, searchValue]);

  const handleSearch = () => {
    onSearch(searchValue);
  };

  return (
    <Box className={className} sx={sx}>
      <SearchInput
        value={searchValue}
        onChange={onSearchChange}
        onSubmit={handleSearch}
        placeholder={placeholder}
        label={label}
      />
    </Box>
  );
});

interface SearchResultsHeaderProps {
  query: string;
  totalResults: number;
  hasFilters?: boolean;
  onClearSearch?: () => void;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Header component to display search results summary
 */
export const SearchResultsHeader: React.FC<SearchResultsHeaderProps> = React.memo(({
  query,
  totalResults,
  hasFilters = false,
  onClearSearch,
  sx = {},
  className = ''
}) => {
  if (!query && !hasFilters) return null;

  return (
    <Box 
      className={className}
      sx={{ 
        mb: 2, 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        ...sx 
      }}
    >
      <Typography variant="h6" color="text.secondary">
        {query ? (
          <>
            Found <strong>{totalResults}</strong> results for "{query}"
            {hasFilters && ' (filtered)'}
          </>
        ) : (
          <>
            Showing <strong>{totalResults}</strong> filtered results
          </>
        )}
      </Typography>
      
      {onClearSearch && (query || hasFilters) && (
        <Chip
          label="Clear search"
          variant="outlined"
          size="small"
          clickable
          onClick={onClearSearch}
        />
      )}
    </Box>
  );
});

SearchBar.displayName = 'SearchBar';
QuickSearchBar.displayName = 'QuickSearchBar';
SearchResultsHeader.displayName = 'SearchResultsHeader';