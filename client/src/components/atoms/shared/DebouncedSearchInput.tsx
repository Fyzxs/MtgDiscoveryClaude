import React, { useRef, useEffect, useState } from 'react';
import { TextField, InputAdornment, IconButton, Skeleton, CircularProgress } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import type { SearchInputProps } from '../../../types/components';

interface DebouncedSearchInputProps extends SearchInputProps {
  loading?: boolean;
  disabled?: boolean;
}

const DebouncedSearchInputComponent: React.FC<DebouncedSearchInputProps> = ({
  value = '',
  onChange,
  placeholder = 'Search...',
  debounceMs = 1000,
  sx = {},
  fullWidth = false,
  minWidth = 300,
  loading = false,
  disabled = false
}) => {
  const [hasText, setHasText] = useState(!!value);
  const searchInputRef = useRef<HTMLInputElement>(null);
  const debounceTimer = useRef<NodeJS.Timeout | undefined>(undefined);

  useEffect(() => {
    if (searchInputRef.current && value !== undefined) {
      searchInputRef.current.value = value;
      setHasText(!!value);
    }
  }, [value]);

  useEffect(() => {
    return () => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
    };
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setHasText(!!newValue);
    
    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }
    
    debounceTimer.current = setTimeout(() => {
      onChange(newValue);
    }, debounceMs);
  };

  const handleClear = () => {
    if (searchInputRef.current) {
      searchInputRef.current.value = '';
      setHasText(false);
      onChange('');
      
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
    }
  };

  // Show skeleton when loading
  if (loading) {
    return (
      <Skeleton 
        variant="rectangular" 
        height={56} 
        sx={{ 
          minWidth: fullWidth ? undefined : minWidth, 
          width: fullWidth ? '100%' : minWidth,
          borderRadius: 1,
          ...sx 
        }} 
      />
    );
  }

  return (
    <TextField
      inputRef={searchInputRef}
      fullWidth={fullWidth}
      sx={{ minWidth: fullWidth ? undefined : minWidth, ...sx }}
      variant="outlined"
      placeholder={placeholder}
      defaultValue={value}
      onChange={handleChange}
      disabled={disabled}
      InputProps={{
        startAdornment: (
          <InputAdornment position="start">
            {disabled ? <CircularProgress size={20} /> : <SearchIcon />}
          </InputAdornment>
        ),
        endAdornment: hasText && !disabled ? (
          <InputAdornment position="end">
            <IconButton
              size="small"
              onClick={handleClear}
              edge="end"
            >
              <ClearIcon fontSize="small" />
            </IconButton>
          </InputAdornment>
        ) : null,
      }}
    />
  );
};

/**
 * Memoized DebouncedSearchInput component
 * Uses custom comparison to prevent re-renders when only internal state changes
 */
export const DebouncedSearchInput = React.memo(DebouncedSearchInputComponent);