import React from 'react';
import { Box, useTheme } from '../../atoms';
import type { GridLayoutProps } from '../../../types/components';
import type { Breakpoint } from '@mui/material/styles';

type ResponsiveValue<T> = T | Partial<Record<Breakpoint, T>>;

export interface ResponsiveGridProps extends Omit<GridLayoutProps, 'minItemWidth' | 'spacing'> {
  minItemWidth?: ResponsiveValue<number>;
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
 * Supports responsive minItemWidth values:
 * - minItemWidth={250} - same width at all breakpoints
 * - minItemWidth={{ xs: 100, sm: 150, md: 200, lg: 250 }} - different widths per breakpoint
 */
export const ResponsiveGridAutoFit: React.FC<ResponsiveGridProps> = (props) => {
  const {
    sx = {},
    minItemWidth = 250,
    spacing = 3,
    justifyContent = 'center',
    ...rest
  } = props;

  // Build responsive gridTemplateColumns with auto-fit
  const getGridTemplateColumns = (): Record<string, string> | string => {
    if (typeof minItemWidth === 'number') {
      return `repeat(auto-fit, minmax(${minItemWidth}px, 1fr))`;
    }

    // Build responsive object for sx prop
    const breakpoints: Breakpoint[] = ['xs', 'sm', 'md', 'lg', 'xl'];
    const result: Partial<Record<Breakpoint, string>> = {};

    let lastValue = 250; // default fallback
    for (const bp of breakpoints) {
      if (minItemWidth[bp] !== undefined) {
        lastValue = minItemWidth[bp] as number;
      }
      result[bp] = `repeat(auto-fit, minmax(${lastValue}px, 1fr))`;
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
