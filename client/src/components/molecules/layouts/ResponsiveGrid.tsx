import React from 'react';
import { Box, useTheme } from '../../atoms';
import type { GridLayoutProps } from '../../../types/components';

export interface ResponsiveGridProps extends GridLayoutProps {
  minItemWidth?: number | string;
  onKeyDown?: (event: React.KeyboardEvent) => void;
  tabIndex?: number;
  'data-grid-container'?: string;
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
  component = 'div',
  onKeyDown,
  tabIndex,
  'data-grid-container': dataGridContainer
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
      onKeyDown={onKeyDown}
      tabIndex={tabIndex}
      data-grid-container={dataGridContainer}
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