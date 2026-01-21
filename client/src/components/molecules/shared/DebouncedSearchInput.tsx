import React, { useRef, useEffect, useState, useCallback } from 'react';
import { TextField, InputAdornment, IconButton, Skeleton, CircularProgress } from '../../atoms';
import type { SearchInputProps } from '../../../types/components';
import { SearchIcon, ClearIcon } from '../../atoms/Icons';

interface DebouncedSearchInputProps extends SearchInputProps {
  loading?: boolean;
  disabled?: boolean;
}

interface HTMLInputElementWithClear extends HTMLInputElement {
  __clearSearch?: () => void;
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
  const clearDelayTimer = useRef<NodeJS.Timeout | undefined>(undefined);
  const onChangeRef = useRef(onChange);

  // Keep onChange ref up to date
  useEffect(() => {
    onChangeRef.current = onChange;
  }, [onChange]);

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

    // Cancel any pending clear delay - user is typing, so skip the "show all" step
    if (clearDelayTimer.current) {
      clearTimeout(clearDelayTimer.current);
      clearDelayTimer.current = undefined;
    }

    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current);
    }

    // OPTIMIZATION: Skip debounce when clearing to empty string
    // Defer the update to keep UI responsive during expensive re-filtering
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

    // Normal debounced behavior for typing
    debounceTimer.current = setTimeout(() => {
      onChangeRef.current(newValue);
    }, debounceMs);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    // When Tab is pressed in search box, skip filter/sort controls and go to first card
    if (e.key === 'Tab' && !e.shiftKey) {
      // Find the first card in the card grid
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
    if (searchInputRef.current) {
      searchInputRef.current.value = '';
      setHasText(false);

      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }

      // Defer the state update by 200ms
      // If user starts typing within 200ms, handleChange will cancel this
      // This avoids the expensive "show all cards" step if user is typing a new search
      clearDelayTimer.current = setTimeout(() => {
        onChangeRef.current('');
        clearDelayTimer.current = undefined;
      }, 200);
    }
  }, []); // No deps - uses ref

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
      onKeyDown={handleKeyDown}
      disabled={disabled}
      inputProps={{
        'data-search-input': 'true'
      }}
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
              data-search-clear="true"
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