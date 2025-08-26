import React, { useEffect, useRef, useState, useCallback } from 'react';
import { Box, SxProps, Theme } from '@mui/material';

interface KeyboardNavigationGridProps<T> {
  items: T[];
  renderItem: (item: T, index: number, isSelected: boolean, isFocused: boolean) => React.ReactNode;
  onSelectionChange?: (item: T, index: number) => void;
  onActivate?: (item: T, index: number) => void;
  columns?: number;
  gap?: number;
  initialFocusIndex?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Grid component with keyboard navigation support
 * Allows arrow key navigation and Enter/Space activation
 */
export const KeyboardNavigationGrid = <T,>({
  items,
  renderItem,
  onSelectionChange,
  onActivate,
  columns = 4,
  gap = 3,
  initialFocusIndex = 0,
  sx = {},
  className = ''
}: KeyboardNavigationGridProps<T>) => {
  const [focusedIndex, setFocusedIndex] = useState(initialFocusIndex);
  const [selectedIndex, setSelectedIndex] = useState<number | null>(null);
  const gridRef = useRef<HTMLDivElement>(null);

  // Calculate grid position from index
  const getGridPosition = useCallback((index: number) => {
    const row = Math.floor(index / columns);
    const col = index % columns;
    return { row, col };
  }, [columns]);

  // Calculate index from grid position
  const getIndexFromPosition = useCallback((row: number, col: number) => {
    return row * columns + col;
  }, [columns]);

  // Handle keyboard navigation
  const handleKeyDown = useCallback((event: KeyboardEvent) => {
    if (!gridRef.current?.contains(document.activeElement)) return;

    const { row, col } = getGridPosition(focusedIndex);
    const maxRow = Math.floor((items.length - 1) / columns);
    let newIndex = focusedIndex;

    switch (event.key) {
      case 'ArrowRight':
        event.preventDefault();
        if (focusedIndex < items.length - 1) {
          newIndex = focusedIndex + 1;
        }
        break;
      
      case 'ArrowLeft':
        event.preventDefault();
        if (focusedIndex > 0) {
          newIndex = focusedIndex - 1;
        }
        break;
      
      case 'ArrowDown':
        event.preventDefault();
        if (row < maxRow) {
          const newRow = row + 1;
          const maxColInNewRow = Math.min(columns - 1, (items.length - 1) % columns);
          const newCol = Math.min(col, maxColInNewRow);
          newIndex = Math.min(getIndexFromPosition(newRow, newCol), items.length - 1);
        }
        break;
      
      case 'ArrowUp':
        event.preventDefault();
        if (row > 0) {
          const newRow = row - 1;
          newIndex = getIndexFromPosition(newRow, col);
        }
        break;
      
      case 'Home':
        event.preventDefault();
        newIndex = 0;
        break;
      
      case 'End':
        event.preventDefault();
        newIndex = items.length - 1;
        break;
      
      case 'Enter':
      case ' ':
        event.preventDefault();
        if (onActivate) {
          onActivate(items[focusedIndex], focusedIndex);
        }
        break;
      
      case 'Escape':
        event.preventDefault();
        setSelectedIndex(null);
        break;
    }

    if (newIndex !== focusedIndex) {
      setFocusedIndex(newIndex);
    }
  }, [focusedIndex, items, columns, getGridPosition, getIndexFromPosition, onActivate]);

  // Set up keyboard event listeners
  useEffect(() => {
    document.addEventListener('keydown', handleKeyDown);
    return () => {
      document.removeEventListener('keydown', handleKeyDown);
    };
  }, [handleKeyDown]);

  // Handle selection changes
  useEffect(() => {
    if (onSelectionChange && focusedIndex >= 0 && focusedIndex < items.length) {
      onSelectionChange(items[focusedIndex], focusedIndex);
    }
  }, [focusedIndex, items, onSelectionChange]);

  // Handle selection
  const handleItemClick = (index: number) => {
    setSelectedIndex(index);
    setFocusedIndex(index);
  };

  return (
    <Box
      ref={gridRef}
      role="grid"
      aria-label="Card grid"
      tabIndex={0}
      className={className}
      sx={{
        display: 'grid',
        gridTemplateColumns: `repeat(${columns}, 1fr)`,
        gap,
        outline: 'none',
        '&:focus-visible': {
          outline: '2px solid',
          outlineColor: 'primary.main',
          outlineOffset: '4px'
        },
        ...sx
      }}
    >
      {items.map((item, index) => (
        <Box
          key={index}
          role="gridcell"
          tabIndex={focusedIndex === index ? 0 : -1}
          onClick={() => handleItemClick(index)}
          sx={{
            outline: 'none',
            '&:focus-visible': {
              outline: '2px solid',
              outlineColor: 'primary.main',
              outlineOffset: '2px'
            }
          }}
        >
          {renderItem(
            item, 
            index, 
            selectedIndex === index, 
            focusedIndex === index
          )}
        </Box>
      ))}
    </Box>
  );
};

KeyboardNavigationGrid.displayName = 'KeyboardNavigationGrid';