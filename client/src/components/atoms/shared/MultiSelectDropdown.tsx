import React from 'react';
import { 
  Select, 
  FormControl, 
  InputLabel, 
  MenuItem, 
  Chip, 
  Typography
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select';

export interface MultiSelectOption {
  value: string;
  label: string;
  chipColor?: 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';
}

interface MultiSelectDropdownProps {
  value: string[];
  onChange: (value: string[]) => void;
  options: MultiSelectOption[] | string[]; // Can accept simple string array or objects
  label?: string;
  placeholder?: string;
  minWidth?: number | string;
  maxDisplay?: number; // Max items to show before showing "X selected"
  showClearAll?: boolean;
  fullWidth?: boolean;
  sx?: any;
}

export const MultiSelectDropdown: React.FC<MultiSelectDropdownProps> = ({
  value,
  onChange,
  options,
  label = 'Select',
  placeholder = 'All',
  minWidth = 150,
  maxDisplay = 2,
  showClearAll = true,
  fullWidth = false,
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

  return (
    <FormControl 
      fullWidth={fullWidth} 
      sx={{ minWidth, ...sx }}
    >
      <InputLabel>{label}</InputLabel>
      <Select
        multiple
        value={value}
        onChange={handleChange}
        label={label}
        renderValue={renderValue}
      >
        {showClearAll && (
          <MenuItem 
            value="CLEAR_ALL"
            sx={{ borderBottom: '1px solid rgba(255,255,255,0.12)' }}
          >
            <Typography variant="body2" color="text.secondary">
              Clear All
            </Typography>
          </MenuItem>
        )}
        {normalizedOptions.map((option) => (
          <MenuItem key={option.value} value={option.value}>
            <Chip
              size="small"
              label={option.label}
              color={value.includes(option.value) ? (option.chipColor || 'primary') : 'default'}
              sx={{ mr: 1 }}
            />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};