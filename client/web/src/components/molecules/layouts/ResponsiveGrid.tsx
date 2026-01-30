import React from 'react';
import { Box, useTheme } from '../../atoms';
import type { GridLayoutProps } from '../../../types/components';
import type { Breakpoint } from '@mui/material/styles';

type ResponsiveValue<T> = T | Partial<Record<Breakpoint, T>>;

export interface ResponsiveGridProps extends Omit<GridLayoutProps, 'minItemWidth' | 'spacing'> {
  minItemWidth?: ResponsiveValue<number>;
  maxItemWidth?: ResponsiveValue<number>;
  spacing?: ResponsiveValue<number>;
  onKeyDown?: (event: React.KeyboardEvent) => void;
  tabIndex?: number;
  'data-grid-container'?: string;
}

/**
 * A responsive grid layout that automatically adjusts columns based on available space
 * Uses CSS Grid with auto-fit/auto-fill for responsive behavior
 *
 * Now supports responsive minItemWidth and spacing values:
 * - minItemWidth={250} - same width at all breakpoints
 * - minItemWidth={{ xs: 100, sm: 150, md: 200, lg: 250 }} - different widths per breakpoint
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

  // Build responsive gridTemplateColumns
  const getGridTemplateColumns = (): Record<string, string> | string => {
    if (typeof minItemWidth === 'number') {
      return `repeat(auto-fill, ${minItemWidth}px)`;
    }

    // Build responsive object for sx prop
    const breakpoints: Breakpoint[] = ['xs', 'sm', 'md', 'lg', 'xl'];
    const result: Partial<Record<Breakpoint, string>> = {};

    let lastValue = 250; // default fallback
    for (const bp of breakpoints) {
      if (minItemWidth[bp] !== undefined) {
        lastValue = minItemWidth[bp] as number;
      }
      result[bp] = `repeat(auto-fill, ${lastValue}px)`;
    }

    return result as Record<string, string>;
  };

  // Build responsive gap
  const getGap = (): number | Record<string, number> => {
    if (typeof spacing === 'number') {
      return theme.spacing(spacing) as unknown as number;
    }

    const breakpoints: Breakpoint[] = ['xs', 'sm', 'md', 'lg', 'xl'];
    const result: Partial<Record<Breakpoint, number>> = {};

    let lastValue = 3; // default fallback
    for (const bp of breakpoints) {
      if (spacing[bp] !== undefined) {
        lastValue = spacing[bp] as number;
      }
      result[bp] = lastValue;
    }

    return result as Record<string, number>;
  };

  return (
    <Box
      component={component}
      className={className}
      onKeyDown={onKeyDown}
      tabIndex={tabIndex}
      data-grid-container={dataGridContainer}
      sx={{
        display: 'grid',
        gridTemplateColumns: getGridTemplateColumns(),
        gap: getGap(),
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
 *
 * Supports responsive minItemWidth and maxItemWidth values:
 * - minItemWidth={250} - same width at all breakpoints
 * - minItemWidth={{ xs: 100, sm: 150, md: 200, lg: 250 }} - different widths per breakpoint
 * - maxItemWidth={350} - limits how much items can grow (prevents oversized cards)
 */
export const ResponsiveGridAutoFit: React.FC<ResponsiveGridProps> = (props) => {
  const {
    sx = {},
    minItemWidth = 250,
    maxItemWidth,
    spacing = 3,
    justifyContent = 'center',
    ...rest
  } = props;

  // Build responsive gridTemplateColumns with auto-fit
  const getGridTemplateColumns = (): Record<string, string> | string => {
    // Helper to get max value for a breakpoint
    // Returns '1fr' if no max is defined for that breakpoint
    const getMaxValue = (bp?: Breakpoint): string => {
      if (maxItemWidth === undefined) return '1fr';
      if (typeof maxItemWidth === 'number') return `${maxItemWidth}px`;

      // For responsive maxItemWidth, check if this specific breakpoint has a value
      // If not defined, use '1fr' (no cap) - don't inherit from previous breakpoints
      if (bp && maxItemWidth[bp] !== undefined) {
        return `${maxItemWidth[bp]}px`;
      }

      // No value defined for this breakpoint - use 1fr (original auto-fit behavior)
      return '1fr';
    };

    if (typeof minItemWidth === 'number') {
      const maxVal = getMaxValue();
      return `repeat(auto-fit, minmax(${minItemWidth}px, ${maxVal}))`;
    }

    // Build responsive object for sx prop
    const breakpoints: Breakpoint[] = ['xs', 'sm', 'md', 'lg', 'xl'];
    const result: Partial<Record<Breakpoint, string>> = {};

    let lastMinValue = 250; // default fallback
    for (const bp of breakpoints) {
      if (minItemWidth[bp] !== undefined) {
        lastMinValue = minItemWidth[bp] as number;
      }
      const maxVal = getMaxValue(bp);
      result[bp] = `repeat(auto-fit, minmax(${lastMinValue}px, ${maxVal}))`;
    }

    return result as Record<string, string>;
  };

  return (
    <ResponsiveGrid
      {...rest}
      spacing={spacing}
      minItemWidth={minItemWidth}
      justifyContent={justifyContent}
      sx={{
        gridTemplateColumns: getGridTemplateColumns(),
        ...sx
      }}
    />
  );
};
