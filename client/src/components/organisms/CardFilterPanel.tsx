import React from 'react';
import { Box } from '@mui/material';
import { FilterPanel } from './filters/FilterPanel';
import { MultiSelectDropdown, SortDropdown } from '../molecules';
import { getFormatOptions } from '../../utils/cardUtils';

interface SortOption {
  value: string;
  label: string;
}

interface CardFilterPanelProps {
  // Filter state
  searchTerm?: string;
  sortBy: string;
  filters: {
    rarities?: string[];
    artists?: string[];
    sets?: string[];
    formats?: string[];
  };

  // State setters
  onSearchChange?: (value: string) => void;
  onSortChange: (value: string) => void;
  onFilterChange: (filterName: string, value: string[]) => void;
  
  // Available options
  uniqueArtists: string[];
  uniqueRarities: string[];
  uniqueSets?: { value: string; label: string }[];
  uniqueFormats?: string[];

  // Control flags
  hasMultipleArtists: boolean;
  hasMultipleRarities: boolean;
  hasMultipleSets?: boolean;
  hasMultipleFormats?: boolean;
  
  // Counts
  filteredCount: number;
  totalCount: number;
  
  // Configuration
  showSearch?: boolean;
  searchPlaceholder?: string;
  sortOptions: SortOption[];
  hideIfSingleCard?: boolean;
  centerControls?: boolean;
}

export const CardFilterPanel: React.FC<CardFilterPanelProps> = ({
  searchTerm = '',
  sortBy,
  filters,
  onSearchChange,
  onSortChange,
  onFilterChange,
  uniqueArtists,
  uniqueRarities,
  uniqueSets,
  hasMultipleArtists,
  hasMultipleRarities,
  hasMultipleSets,
  hasMultipleFormats = true,
  totalCount,
  showSearch = false,
  searchPlaceholder = 'Search cards...',
  sortOptions,
  hideIfSingleCard = true,
  centerControls = false
}) => {
  // Don't show filters if only 1 card and hideIfSingleCard is true
  if (hideIfSingleCard && totalCount <= 1) {
    return null;
  }

  // Build multi-select configurations
  const multiSelects = [];
  
  // Add sets filter (only for card detail page)
  if (uniqueSets && hasMultipleSets) {
    multiSelects.push({
      key: 'sets',
      label: 'Sets',
      options: uniqueSets,
      value: filters.sets || [],
      onChange: (value: string[]) => onFilterChange('sets', value),
      placeholder: 'All Sets'
    });
  }
  
  // Add rarity filter
  if (hasMultipleRarities) {
    multiSelects.push({
      key: 'rarities',
      label: 'Rarity',
      options: uniqueRarities.map(r => ({ 
        value: r, 
        label: r.charAt(0).toUpperCase() + r.slice(1) 
      })),
      value: filters.rarities || [],
      onChange: (value: string[]) => onFilterChange('rarities', value),
      placeholder: 'All Rarities'
    });
  }
  
  // Add artist filter
  if (hasMultipleArtists) {
    multiSelects.push({
      key: 'artists',
      label: 'Artists',
      options: uniqueArtists.map(a => ({ value: a, label: a })),
      value: filters.artists || [],
      onChange: (value: string[]) => onFilterChange('artists', value),
      placeholder: 'All Artists'
    });
  }

  // Build the filter config
  const filterConfig = {
    ...(showSearch && onSearchChange ? {
      search: {
        value: searchTerm,
        onChange: onSearchChange,
        placeholder: searchPlaceholder,
        debounceMs: 300,
        minWidth: 300
      }
    } : {}),
    multiSelects,
    sort: {
      value: sortBy,
      onChange: onSortChange,
      options: sortOptions
    },
    customFilters: hasMultipleFormats ? [
      <MultiSelectDropdown
        key="formats"
        value={filters.formats || []}
        onChange={(value: string[]) => onFilterChange('formats', value)}
        options={getFormatOptions()}
        label="Format"
        placeholder="All Formats"
        minWidth={150}
        fullWidth={false}
      />
    ] : []
  };

  // If centering, use a flex layout with individual components
  if (centerControls) {
    return (
      <Box sx={{
        mb: 3,
        display: 'flex',
        justifyContent: 'center',
        flexWrap: 'wrap',
        gap: 2,
        alignItems: 'center'
      }}>
        {/* Multi-select Dropdowns */}
        {multiSelects.map((select) => (
          <MultiSelectDropdown
            key={select.label}
            value={select.value}
            onChange={select.onChange}
            options={select.options}
            label={select.label}
            placeholder={select.placeholder}
            minWidth={200}
            fullWidth={false}
          />
        ))}

        {/* Sort Dropdown */}
        <SortDropdown
          value={filterConfig.sort.value}
          onChange={filterConfig.sort.onChange}
          options={filterConfig.sort.options}
          label="Sort by"
          minWidth={200}
          fullWidth={false}
        />

        {/* Format Filter */}
        {hasMultipleFormats && (
          <MultiSelectDropdown
            value={filters.formats || []}
            onChange={(value: string[]) => onFilterChange('formats', value)}
            options={getFormatOptions()}
            label="Format"
            placeholder="All Formats"
            minWidth={150}
            fullWidth={false}
          />
        )}
      </Box>
    );
  }

  // Default layout
  return (
    <Box sx={{ mb: 3 }}>
      <FilterPanel
        config={filterConfig}
      />
    </Box>
  );
};