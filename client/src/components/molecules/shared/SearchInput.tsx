import React, { useState } from 'react';
import {
  TextField,
  Box,
  InputAdornment,
  IconButton
} from '../../atoms';
import type { SearchInputProps as StandardSearchProps } from '../../../types/components';
import { SearchIcon, ClearIcon } from '../../atoms/Icons';

interface SearchInputProps extends Omit<StandardSearchProps, 'onChange'> {
  onChange: (value: string) => void;
  onSubmit: () => void;
  label?: string;
  expandable?: boolean;
  expandedWidth?: number;
  collapsedWidth?: number;
  size?: 'small' | 'medium';
  disabled?: boolean;
}

/**
 * Reusable search input component with expandable behavior and clear functionality
 */
export const SearchInput: React.FC<SearchInputProps> = ({
  value,
  onChange,
  onSubmit,
  placeholder = "Search",
  label = "",
  expandable = true,
  expandedWidth = 200,
  collapsedWidth = 150,
  size = 'small',
  disabled = false
}) => {
  const [isFocused, setIsFocused] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (value?.trim()) {
      onSubmit();
    }
  };

  const handleClear = () => {
    onChange('');
  };

  const currentWidth = expandable && (isFocused || value) ? expandedWidth : collapsedWidth;
  const showLabel = isFocused || Boolean(value);

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ position: 'relative' }}
    >
      <TextField
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
        placeholder={placeholder}
        size={size}
        disabled={disabled}
        sx={{
          width: expandable ? currentWidth : expandedWidth,
          transition: 'all 0.3s ease',
          '& .MuiInputBase-root': {
            backgroundColor: 'action.hover'
          },
          '& .MuiInputLabel-root': {
            transform: isFocused || value ? 'translate(14px, -9px) scale(0.75)' : 'translate(14px, 9px) scale(1)',
          }
        }}
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <SearchIcon fontSize="small" />
            </InputAdornment>
          ),
          endAdornment: value && (
            <InputAdornment position="end">
              <IconButton
                size="small"
                onClick={handleClear}
                edge="end"
                disabled={disabled}
              >
                <ClearIcon fontSize="small" />
              </IconButton>
            </InputAdornment>
          )
        }}
        InputLabelProps={{
          shrink: showLabel,
          sx: {
            color: isFocused ? 'primary.main' : 'text.secondary',
            '&.MuiInputLabel-shrink': {
              transform: 'translate(14px, -9px) scale(0.75)',
              backgroundColor: 'background.paper',
              px: 0.5
            }
          }
        }}
        label={showLabel && label ? label : ""}
      />
    </Box>
  );
};