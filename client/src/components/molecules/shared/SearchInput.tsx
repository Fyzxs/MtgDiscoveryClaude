import React, { useRef, useEffect, useState, useCallback } from 'react';
import {
  TextField,
  Box,
  InputAdornment,
  IconButton,
  Skeleton,
  CircularProgress
} from '../../atoms';
import type { SxProps, Theme } from '@mui/material';
import { SearchIcon, ClearIcon } from '../../atoms';

interface SearchInputProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;

  // Debounce behavior
  debounceMs?: number;        // If provided, enables debouncing

  // Expandable behavior
  expandable?: boolean;
  expandedWidth?: number;
  collapsedWidth?: number;
  label?: string;

  // States
  loading?: boolean;          // Show skeleton
  disabled?: boolean;

  // Callbacks
  onSubmit?: () => void;      // Form submit
  onEnter?: (value: string) => void;  // Enter key

  // Styling
  size?: 'small' | 'medium';
  fullWidth?: boolean;
  minWidth?: number;
  sx?: SxProps<Theme>;
}

interface HTMLInputElementWithClear extends HTMLInputElement {
  __clearSearch?: () => void;
}

const SearchInputComponent: React.FC<SearchInputProps> = ({
  value = '',
  onChange,
  placeholder = 'Search...',
  debounceMs,
  expandable = false,
  expandedWidth = 200,
  collapsedWidth = 150,
  label,
  loading = false,
  disabled = false,
  onSubmit,
  onEnter,
  size = 'small',
  fullWidth = false,
  minWidth = 300,
  sx = {}
}) => {
  const [isFocused, setIsFocused] = useState(false);
  const [hasText, setHasText] = useState(!!value);
  const [internalValue, setInternalValue] = useState(value);

  const searchInputRef = useRef<HTMLInputElement>(null);
  const debounceTimer = useRef<NodeJS.Timeout | undefined>(undefined);
  const clearDelayTimer = useRef<NodeJS.Timeout | undefined>(undefined);
  const onChangeRef = useRef(onChange);

  // Keep onChange ref up to date
  useEffect(() => {
    onChangeRef.current = onChange;
  }, [onChange]);

  // Sync internal value with external value
  useEffect(() => {
    setInternalValue(value);
    setHasText(!!value);
  }, [value]);

  // Cleanup timers on unmount
  useEffect(() => {
    return () => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
      if (clearDelayTimer.current) {
        clearTimeout(clearDelayTimer.current);
      }
    };
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setInternalValue(newValue);
    setHasText(!!newValue);

    // Cancel any pending clear delay
    if (clearDelayTimer.current) {
      clearTimeout(clearDelayTimer.current);
      clearDelayTimer.current = undefined;
    }

    // If debouncing is enabled
    if (debounceMs !== undefined && debounceMs > 0) {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }

      // OPTIMIZATION: Skip debounce when clearing to empty string
      if (newValue === '') {
        if ('requestIdleCallback' in window) {
          requestIdleCallback(() => {
            onChangeRef.current('');
          });
        } else {
          setTimeout(() => {
            onChangeRef.current('');
          }, 50);
        }
        return;
      }

      // Normal debounced behavior
      debounceTimer.current = setTimeout(() => {
        onChangeRef.current(newValue);
      }, debounceMs);
    } else {
      // No debouncing - call onChange immediately
      onChangeRef.current(newValue);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    // Handle Enter key press
    if (e.key === 'Enter') {
      e.preventDefault();

      if (onEnter && searchInputRef.current) {
        onEnter(searchInputRef.current.value);
      } else if (onSubmit && internalValue?.trim()) {
        onSubmit();
      }
      return;
    }

    // When Tab is pressed in search box, skip filter/sort controls and go to first card
    if (e.key === 'Tab' && !e.shiftKey && debounceMs !== undefined) {
      const firstCard = document.querySelector('[data-card-id]') as HTMLElement;

      if (firstCard) {
        e.preventDefault();

        // Clear any existing selections
        const allSelected = document.querySelectorAll('[data-selected="true"]');
        allSelected.forEach(selected => {
          selected.setAttribute('data-selected', 'false');
        });

        // Select and focus the first card
        firstCard.setAttribute('data-selected', 'true');
        firstCard.focus();
      }
    }
  };

  const handleClear = useCallback(() => {
    setInternalValue('');
    setHasText(false);

    if (searchInputRef.current) {
      searchInputRef.current.value = '';
    }

    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    // If debouncing, defer the state update
    if (debounceMs !== undefined && debounceMs > 0) {
      clearDelayTimer.current = setTimeout(() => {
        onChangeRef.current('');
        clearDelayTimer.current = undefined;
      }, 200);
    } else {
      // No debouncing - clear immediately
      onChangeRef.current('');
    }
  }, [debounceMs]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (internalValue?.trim() && onSubmit) {
      onSubmit();
    }
  };

  // Expose clear function for the X button
  useEffect(() => {
    const currentRef = searchInputRef.current;
    if (currentRef) {
      (currentRef as HTMLInputElementWithClear).__clearSearch = handleClear;
    }
    return () => {
      if (currentRef) {
        delete (currentRef as HTMLInputElementWithClear).__clearSearch;
      }
    };
  }, [handleClear]);

  // Show skeleton when loading
  if (loading) {
    return (
      <Skeleton
        variant="rectangular"
        height={size === 'small' ? 40 : 56}
        sx={{
          minWidth: fullWidth ? undefined : (expandable ? collapsedWidth : minWidth),
          width: fullWidth ? '100%' : (expandable ? expandedWidth : minWidth),
          borderRadius: 1,
          ...sx
        }}
      />
    );
  }

  // Calculate current width for expandable behavior
  const currentWidth = expandable && (isFocused || hasText) ? expandedWidth : collapsedWidth;
  const showLabel = label && (isFocused || Boolean(internalValue));

  // Determine which value prop to use based on debouncing
  const inputValueProp = debounceMs !== undefined
    ? { defaultValue: value, inputRef: searchInputRef }
    : { value: internalValue };

  const textField = (
    <TextField
      {...inputValueProp}
      fullWidth={fullWidth}
      size={size}
      variant="outlined"
      placeholder={placeholder}
      onChange={handleChange}
      onKeyDown={handleKeyDown}
      onFocus={() => setIsFocused(true)}
      onBlur={() => setIsFocused(false)}
      disabled={disabled}
      label={showLabel ? label : ''}
      inputProps={{
        'data-search-input': 'true'
      }}
      sx={{
        minWidth: fullWidth ? undefined : (expandable ? currentWidth : minWidth),
        width: expandable ? currentWidth : (fullWidth ? '100%' : undefined),
        transition: expandable ? 'all 0.3s ease' : undefined,
        '& .MuiInputBase-root': expandable ? {
          backgroundColor: 'action.hover'
        } : undefined,
        ...sx
      }}
      InputProps={{
        startAdornment: (
          <InputAdornment position="start">
            {disabled && debounceMs !== undefined ? (
              <CircularProgress size={20} />
            ) : (
              <SearchIcon fontSize="small" />
            )}
          </InputAdornment>
        ),
        endAdornment: hasText && !disabled ? (
          <InputAdornment position="end">
            <IconButton
              size="small"
              onClick={handleClear}
              edge="end"
              data-search-clear="true"
            >
              <ClearIcon fontSize="small" />
            </IconButton>
          </InputAdornment>
        ) : null
      }}
      InputLabelProps={label ? {
        shrink: showLabel,
        sx: {
          color: isFocused ? 'primary.main' : 'text.secondary',
          '&.MuiInputLabel-shrink': {
            transform: 'translate(14px, -9px) scale(0.75)',
            backgroundColor: 'background.paper',
            px: 0.5
          }
        }
      } : undefined}
    />
  );

  // Wrap in form if onSubmit is provided
  if (onSubmit) {
    return (
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{ position: 'relative' }}
      >
        {textField}
      </Box>
    );
  }

  return textField;
};

/**
 * Unified search input component with optional debouncing and expandable behavior.
 *
 * Features:
 * - Optional debouncing (set debounceMs prop)
 * - Expandable width on focus (set expandable prop)
 * - Loading skeleton state
 * - Form submission support
 * - Tab navigation to first card (when debounced)
 * - Clear button
 * - Optimized performance with refs and memoization
 */
export const SearchInput = React.memo(SearchInputComponent);
