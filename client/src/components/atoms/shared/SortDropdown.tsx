import React from 'react';
import { Select, FormControl, InputLabel, MenuItem, Skeleton } from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select';
import type { SortOption } from '../../../types/filters';
import type { StyledComponentProps } from '../../../types/components';

// Re-export for backward compatibility
export type { SortOption } from '../../../types/filters';

interface SortDropdownProps extends StyledComponentProps {
  value: string;
  onChange: (value: string) => void;
  options: SortOption[];
  label?: string;
  minWidth?: number | string;
  fullWidth?: boolean;
  loading?: boolean;
  disabled?: boolean;
}

const SortDropdownComponent: React.FC<SortDropdownProps> = ({
  value,
  onChange,
  options,
  label = 'Sort By',
  minWidth = 180,
  fullWidth = false,
  loading = false,
  disabled = false,
  sx = {}
}) => {
  const handleChange = (event: SelectChangeEvent) => {
    onChange(event.target.value);
  };

  // Show skeleton when loading
  if (loading) {
    return (
      <FormControl 
        fullWidth={fullWidth} 
        sx={{ minWidth, ...sx }}
      >
        <Skeleton variant="rectangular" height={56} sx={{ borderRadius: 1 }} />
      </FormControl>
    );
  }

  return (
    <FormControl 
      fullWidth={fullWidth} 
      sx={{ minWidth, ...sx }}
      disabled={disabled}
    >
      <InputLabel>{label}</InputLabel>
      <Select
        value={value}
        onChange={handleChange}
        label={label}
        disabled={disabled}
      >
        {options.map((option) => {
          // Skip options with false condition
          if (option.condition === false) {
            return null;
          }
          
          return (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          );
        })}
      </Select>
    </FormControl>
  );
};

/**
 * Memoized SortDropdown component
 * Re-renders only when props actually change
 */
export const SortDropdown = React.memo(SortDropdownComponent);