import React from 'react';
import { Box, Typography, Divider, SxProps, Theme } from '@mui/material';

interface CardGridProps<T = any> {
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  columns?: 'auto-fit' | 'auto-fill' | number;
  minItemWidth?: number | string;
  maxItemWidth?: number | string;
  gap?: number;
  justifyContent?: 'center' | 'start' | 'end' | 'space-between' | 'space-around' | 'space-evenly';
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Generic responsive grid component for displaying cards and other items
 * Extracts common grid layout logic for reuse
 */
export const CardGrid: React.FC<CardGridProps> = React.memo(({
  items,
  renderItem,
  columns = 'auto-fit',
  minItemWidth = 280,
  maxItemWidth,
  gap = 3,
  justifyContent = 'center',
  sx = {},
  className = ''
}) => {
  if (items.length === 0) return null;

  const getGridTemplateColumns = () => {
    if (typeof columns === 'number') {
      return `repeat(${columns}, 1fr)`;
    }
    
    const minWidth = typeof minItemWidth === 'number' ? `${minItemWidth}px` : minItemWidth;
    const maxWidth = maxItemWidth 
      ? (typeof maxItemWidth === 'number' ? `${maxItemWidth}px` : maxItemWidth)
      : '1fr';
    
    return `repeat(${columns}, minmax(${minWidth}, ${maxWidth}))`;
  };

  return (
    <Box
      className={className}
      sx={{
        display: 'grid',
        gridTemplateColumns: getGridTemplateColumns(),
        gap,
        justifyContent,
        margin: '0 auto',
        ...sx
      }}
    >
      {items.map(renderItem)}
    </Box>
  );
});

interface CardGridSectionProps<T = any> {
  title?: string;
  subtitle?: string;
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  columns?: 'auto-fit' | 'auto-fill' | number;
  minItemWidth?: number | string;
  maxItemWidth?: number | string;
  gap?: number;
  showHeader?: boolean;
  showCount?: boolean;
  showDivider?: boolean;
  isVisible?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Card grid with optional header section (replaces CardGroup pattern)
 */
export const CardGridSection: React.FC<CardGridSectionProps> = React.memo(({
  title,
  subtitle,
  items,
  renderItem,
  columns = 'auto-fit',
  minItemWidth = 280,
  maxItemWidth,
  gap = 3,
  showHeader = false,
  showCount = true,
  showDivider = true,
  isVisible = true,
  sx = {},
  className = ''
}) => {
  if (!isVisible || items.length === 0) return null;

  const displayTitle = title || 'Items';
  const countText = showCount ? ` • ${items.length} ${items.length === 1 ? 'ITEM' : 'ITEMS'}` : '';

  return (
    <Box 
      className={className}
      sx={{ 
        mb: showHeader ? 6 : 0,
        ...sx 
      }}
    >
      {showHeader && (
        <Box sx={{ mb: 2, textAlign: 'center' }}>
          <Typography 
            variant="overline" 
            sx={{ 
              color: 'text.secondary',
              letterSpacing: 2,
              fontSize: '0.875rem'
            }}
          >
            {displayTitle.toUpperCase()}{countText}
          </Typography>
          {subtitle && (
            <Typography 
              variant="body2" 
              sx={{ 
                color: 'text.secondary',
                mt: 0.5
              }}
            >
              {subtitle}
            </Typography>
          )}
          {showDivider && <Divider sx={{ mt: 1, mb: 3 }} />}
        </Box>
      )}
      
      <CardGrid
        items={items}
        renderItem={renderItem}
        columns={columns}
        minItemWidth={minItemWidth}
        maxItemWidth={maxItemWidth}
        gap={gap}
      />
    </Box>
  );
});

interface ResponsiveCardGridProps<T = any> {
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  breakpoints?: {
    xs?: number;
    sm?: number;
    md?: number;
    lg?: number;
    xl?: number;
  };
  gap?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Card grid with responsive column counts based on breakpoints
 */
export const ResponsiveCardGrid: React.FC<ResponsiveCardGridProps> = React.memo(({
  items,
  renderItem,
  breakpoints = {
    xs: 1,
    sm: 2,
    md: 3,
    lg: 4,
    xl: 5
  },
  gap = 3,
  sx = {},
  className = ''
}) => {
  if (items.length === 0) return null;

  return (
    <Box
      className={className}
      sx={{
        display: 'grid',
        gap,
        gridTemplateColumns: {
          xs: `repeat(${breakpoints.xs || 1}, 1fr)`,
          sm: `repeat(${breakpoints.sm || 2}, 1fr)`,
          md: `repeat(${breakpoints.md || 3}, 1fr)`,
          lg: `repeat(${breakpoints.lg || 4}, 1fr)`,
          xl: `repeat(${breakpoints.xl || 5}, 1fr)`
        },
        ...sx
      }}
    >
      {items.map(renderItem)}
    </Box>
  );
});

CardGrid.displayName = 'CardGrid';
CardGridSection.displayName = 'CardGridSection';
ResponsiveCardGrid.displayName = 'ResponsiveCardGrid';