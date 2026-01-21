import React from 'react';
import {
  Select,
  FormControl,
  InputLabel,
  MenuItem,
  Chip,
  Typography,
  IconButton,
  InputAdornment,
  Skeleton,
  Box,
  type SelectChangeEvent
} from '../../atoms';
import type { MultiSelectOption } from '../../../types/filters';
import type { StyledComponentProps } from '../../../types/components';
import { ClearIcon } from '../../atoms/Icons';

// Re-export for backward compatibility
export type { MultiSelectOption } from '../../../types/filters';

interface MultiSelectDropdownProps extends StyledComponentProps {
  value: string[];
  onChange: (value: string[]) => void;
  options: MultiSelectOption[] | string[]; // Can accept simple string array or objects
  label?: string;
  placeholder?: string;
  minWidth?: number | string;
  maxDisplay?: number; // Max items to show before showing "X selected"
  showClearAll?: boolean;
  fullWidth?: boolean;
  loading?: boolean;
  disabled?: boolean;
}

const MultiSelectDropdownComponent: React.FC<MultiSelectDropdownProps> = ({
  value,
  onChange,
  options,
  label = 'Select',
  placeholder = 'All',
  minWidth = 150,
  maxDisplay = 2,
  showClearAll = true,
  fullWidth = false,
  loading = false,
  disabled = false,
  sx = {}
}) => {
  // Normalize options to always be MultiSelectOption[]
  const normalizedOptions: MultiSelectOption[] = options.map(opt => {
    if (typeof opt === 'string') {
      return {
        value: opt,
        label: opt.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase())
      };
    }
    return opt;
  });

  const handleChange = (event: SelectChangeEvent<string[]>) => {
    const val = event.target.value;
    // Check if CLEAR_ALL was selected
    if (Array.isArray(val) && val.includes('CLEAR_ALL')) {
      onChange([]);
      return;
    }
    onChange(typeof val === 'string' ? val.split(',') : val);
  };

  const renderValue = (selected: string[]) => {
    if (selected.length === 0) {
      return (
        <Typography variant="body2" color="text.secondary">
          {placeholder}
        </Typography>
      );
    }

    if (selected.length <= maxDisplay) {
      // Find the display labels for selected values
      const labels = selected.map(val => {
        const option = normalizedOptions.find(opt => opt.value === val);
        return option?.label || val;
      });
      return labels.join(', ');
    }

    return `${selected.length} selected`;
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
      <InputLabel id={`${label}-label`}>{label}</InputLabel>
      <Select
        multiple
        value={value}
        onChange={handleChange}
        label={label}
        labelId={`${label}-label`}
        renderValue={renderValue}
        disabled={disabled}
        aria-label={`Select ${label.toLowerCase()}`}
        aria-describedby={`${label}-helper`}
        inputProps={{
          'aria-label': `${label} multi-select dropdown`,
          'aria-multiselectable': true
        }}
        endAdornment={
          value.length > 0 ? (
            <InputAdornment position="end">
              <IconButton
                size="small"
                onClick={(e) => {
                  e.stopPropagation();
                  onChange([]);
                }}
                onMouseDown={(e) => e.stopPropagation()}
                aria-label={`Clear all selected ${label.toLowerCase()}`}
                tabIndex={0}
                sx={{ mr: 2 }}
              >
                <ClearIcon fontSize="small" />
              </IconButton>
            </InputAdornment>
          ) : null
        }
      >
        <Box id={`${label}-helper`} sx={{ display: 'none' }} aria-live="polite">
          {value.length === 0 ? `No ${label.toLowerCase()} selected` : `${value.length} ${label.toLowerCase()} selected`}
        </Box>
        {showClearAll && (
          <MenuItem 
            value="CLEAR_ALL"
            role="option"
            aria-label="Clear all selections"
            sx={{ borderBottom: '1px solid rgba(255,255,255,0.12)' }}
          >
            <Typography variant="body2" color="text.secondary">
              Clear All
            </Typography>
          </MenuItem>
        )}
        {normalizedOptions.map((option) => (
          <MenuItem 
            key={option.value} 
            value={option.value}
            role="option"
            aria-selected={value.includes(option.value)}
            aria-label={`${option.label}${value.includes(option.value) ? ' (selected)' : ''}`}
          >
            <Chip
              size="small"
              label={option.label}
              color={value.includes(option.value) ? (option.chipColor || 'primary') : 'default'}
              sx={{ mr: 1 }}
              aria-hidden="true"
            />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

/**
 * Memoized MultiSelectDropdown component
 * Re-renders only when props actually change
 */
export const MultiSelectDropdown = React.memo(MultiSelectDropdownComponent);