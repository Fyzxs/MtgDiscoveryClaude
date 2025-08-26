import React, { useRef, useEffect, useState } from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';

interface DebouncedSearchInputProps {
  value?: string;
  onChange: (value: string) => void;
  placeholder?: string;
  debounceMs?: number;
  sx?: any;
  fullWidth?: boolean;
  minWidth?: number | string;
}

export const DebouncedSearchInput: React.FC<DebouncedSearchInputProps> = ({
  value = '',
  onChange,
  placeholder = 'Search...',
  debounceMs = 1000,
  sx = {},
  fullWidth = false,
  minWidth = 300
}) => {
  const [hasText, setHasText] = useState(!!value);
  const searchInputRef = useRef<HTMLInputElement>(null);
  const debounceTimer = useRef<NodeJS.Timeout>();

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

  return (
    <TextField
      inputRef={searchInputRef}
      fullWidth={fullWidth}
      sx={{ minWidth: fullWidth ? undefined : minWidth, ...sx }}
      variant="outlined"
      placeholder={placeholder}
      defaultValue={value}
      onChange={handleChange}
      InputProps={{
        startAdornment: (
          <InputAdornment position="start">
            <SearchIcon />
          </InputAdornment>
        ),
        endAdornment: hasText ? (
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