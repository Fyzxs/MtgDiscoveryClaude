import React from 'react';
import { Box, useTheme } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';

export interface ResponsiveGridProps {
  children: React.ReactNode;
  minItemWidth?: number | string;
  spacing?: number | string;
  justifyContent?: 'start' | 'center' | 'end' | 'space-between' | 'space-around' | 'space-evenly';
  alignItems?: 'start' | 'center' | 'end' | 'stretch';
  sx?: SxProps<Theme>;
  className?: string;
  component?: React.ElementType;
}

/**
 * A responsive grid layout that automatically adjusts columns based on available space
 * Uses CSS Grid with auto-fit/auto-fill for responsive behavior
 */
export const ResponsiveGrid: React.FC<ResponsiveGridProps> = ({
  children,
  minItemWidth = 250,
  spacing = 3,
  justifyContent = 'center',
  alignItems = 'start',
  sx = {},
  className,
  component = 'div'
}) => {
  const theme = useTheme();
  // Use theme spacing or direct value
  const gridGap = typeof spacing === 'number' ? theme.spacing(spacing) : spacing;
  
  // Handle minItemWidth - if it's a number, add px
  const itemWidth = typeof minItemWidth === 'number' ? `${minItemWidth}px` : minItemWidth;

  return (
    <Box
      component={component}
      className={className}
      sx={{
        display: 'grid',
        gridTemplateColumns: `repeat(auto-fill, ${itemWidth})`,
        gap: gridGap,
        justifyContent,
        alignItems,
        width: '100%',
        ...sx
      }}
    >
      {children}
    </Box>
  );
};

/**
 * Variant that uses auto-fit instead of auto-fill
 * auto-fit will expand items to fill the container when there are fewer items
 * but limits the max width to prevent excessive stretching
 */
export const ResponsiveGridAutoFit: React.FC<ResponsiveGridProps> = (props) => {
  const { 
    sx = {}, 
    minItemWidth = 250, 
    spacing = 3,
    justifyContent = 'center',
    ...rest 
  } = props;
  
  const itemWidth = typeof minItemWidth === 'number' ? `${minItemWidth}px` : minItemWidth;
  
  return (
    <ResponsiveGrid
      {...rest}
      spacing={spacing}
      minItemWidth={minItemWidth}
      justifyContent={justifyContent}
      sx={{
        gridTemplateColumns: `repeat(auto-fit, ${itemWidth})`,
        ...sx
      }}
    />
  );
};