import React from 'react';
import {
  Box,
  Grid,
  Stack,
  TextField,
  Chip,
  Autocomplete
} from '@mui/material';
import { DebouncedSearchInput } from '../../atoms/shared/DebouncedSearchInput';
import { MultiSelectDropdown } from '../../atoms/shared/MultiSelectDropdown';
import { SortDropdown } from '../../atoms/shared/SortDropdown';
import type { SortOption } from '../../atoms/shared/SortDropdown';

export interface FilterConfig {
  search?: {
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
    debounceMs?: number;
    minWidth?: number | string;
    fullWidth?: boolean;
  };
  multiSelects?: Array<{
    key: string;
    value: string[];
    onChange: (value: string[]) => void;
    options: string[] | { value: string; label: string }[];
    label: string;
    placeholder?: string;
    minWidth?: number | string;
    fullWidth?: boolean;
  }>;
  autocompletes?: Array<{
    key: string;
    value: string[];
    onChange: (value: string[]) => void;
    options: string[];
    label: string;
    placeholder?: string;
    minWidth?: number | string;
    renderTags?: boolean;
    getOptionLabel?: (option: string) => string;
  }>;
  sort?: {
    value: string;
    onChange: (value: string) => void;
    options: SortOption[];
    label?: string;
    minWidth?: number | string;
    fullWidth?: boolean;
  };
  customFilters?: React.ReactNode[];
}

interface FilterPanelProps {
  config: FilterConfig;
  layout?: 'horizontal' | 'vertical' | 'compact';
  spacing?: number;
  sx?: any;
}

export const FilterPanel: React.FC<FilterPanelProps> = ({
  config,
  layout = 'horizontal',
  spacing = 2,
  sx = {}
}) => {
  const {
    search,
    multiSelects = [],
    autocompletes = [],
    sort,
    customFilters = []
  } = config;

  // For horizontal layout using Grid
  if (layout === 'horizontal') {
    return (
      <Box sx={{ mb: 4, ...sx }}>
        <Grid container spacing={spacing} alignItems="center">
          {/* Search Input */}
          {search && (
            <Grid item xs={12} md={4}>
              <DebouncedSearchInput
                value={search.value}
                onChange={search.onChange}
                placeholder={search.placeholder}
                debounceMs={search.debounceMs}
                minWidth={search.minWidth}
                fullWidth={search.fullWidth !== false}
              />
            </Grid>
          )}

          {/* Multi-select Dropdowns */}
          {multiSelects.map((select) => (
            <Grid 
              key={select.key} 
              item 
              xs={12} 
              sm={6} 
              md={select.fullWidth ? 12 : 3}
            >
              <MultiSelectDropdown
                value={select.value}
                onChange={select.onChange}
                options={select.options}
                label={select.label}
                placeholder={select.placeholder}
                minWidth={select.minWidth}
                fullWidth={select.fullWidth !== false}
              />
            </Grid>
          ))}

          {/* Autocomplete Filters */}
          {autocompletes.map((auto) => (
            <Grid 
              key={auto.key} 
              item 
              xs={12} 
              sm={6} 
              md={3}
            >
              <Autocomplete
                multiple
                sx={{ minWidth: auto.minWidth || 250 }}
                options={auto.options}
                value={auto.value}
                onChange={(_, newValue) => {
                  auto.onChange(newValue);
                }}
                getOptionLabel={auto.getOptionLabel || ((option) => option)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label={auto.label}
                    placeholder={auto.placeholder || `All ${auto.label}`}
                    size="medium"
                  />
                )}
                renderTags={auto.renderTags !== false ? (value, getTagProps) =>
                  value.map((option, index) => {
                    const { key, ...chipProps } = getTagProps({ index });
                    const displayLabel = auto.getOptionLabel ? auto.getOptionLabel(option) : option;
                    return (
                      <Chip
                        key={key}
                        variant="outlined"
                        label={displayLabel}
                        size="small"
                        {...chipProps}
                      />
                    );
                  })
                : undefined}
              />
            </Grid>
          ))}

          {/* Sort Dropdown */}
          {sort && (
            <Grid 
              item 
              xs={12} 
              sm={6} 
              md={sort.fullWidth ? 12 : 3}
            >
              <SortDropdown
                value={sort.value}
                onChange={sort.onChange}
                options={sort.options}
                label={sort.label}
                minWidth={sort.minWidth}
                fullWidth={sort.fullWidth !== false}
              />
            </Grid>
          )}

          {/* Custom Filter Components */}
          {customFilters.map((filter, index) => (
            <Grid key={`custom-${index}`} item xs={12} sm={6} md={3}>
              {filter}
            </Grid>
          ))}
        </Grid>
      </Box>
    );
  }

  // For vertical/compact layout using Stack
  return (
    <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center', ...sx }}>
      <Stack spacing={spacing}>
        <Stack 
          direction={layout === 'vertical' ? 'column' : 'row'} 
          spacing={spacing} 
          flexWrap="wrap" 
          sx={{ rowGap: spacing }}
        >
          {/* Search Input */}
          {search && (
            <DebouncedSearchInput
              value={search.value}
              onChange={search.onChange}
              placeholder={search.placeholder}
              debounceMs={search.debounceMs}
              minWidth={search.minWidth || 300}
              fullWidth={layout === 'vertical' && search.fullWidth !== false}
            />
          )}

          {/* Multi-select Dropdowns */}
          {multiSelects.map((select) => (
            <MultiSelectDropdown
              key={select.key}
              value={select.value}
              onChange={select.onChange}
              options={select.options}
              label={select.label}
              placeholder={select.placeholder}
              minWidth={select.minWidth || 150}
              fullWidth={layout === 'vertical' && select.fullWidth !== false}
            />
          ))}

          {/* Autocomplete Filters */}
          {autocompletes.map((auto) => (
            <Autocomplete
              key={auto.key}
              multiple
              sx={{ minWidth: auto.minWidth || 250 }}
              options={auto.options}
              value={auto.value}
              onChange={(event, newValue) => {
                auto.onChange(newValue);
              }}
              getOptionLabel={auto.getOptionLabel || ((option) => option)}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label={auto.label}
                  placeholder={auto.placeholder || `All ${auto.label}`}
                  size="medium"
                />
              )}
              renderTags={auto.renderTags !== false ? (value, getTagProps) =>
                value.map((option, index) => {
                  const { key, ...chipProps } = getTagProps({ index });
                  const displayLabel = auto.getOptionLabel ? auto.getOptionLabel(option) : option;
                  return (
                    <Chip
                      key={key}
                      variant="outlined"
                      label={displayLabel}
                      size="small"
                      {...chipProps}
                    />
                  );
                })
              : undefined}
            />
          ))}

          {/* Sort Dropdown */}
          {sort && (
            <SortDropdown
              value={sort.value}
              onChange={sort.onChange}
              options={sort.options}
              label={sort.label}
              minWidth={sort.minWidth || 180}
              fullWidth={layout === 'vertical' && sort.fullWidth !== false}
            />
          )}

          {/* Custom Filter Components */}
          {customFilters}
        </Stack>
      </Stack>
    </Box>
  );
};