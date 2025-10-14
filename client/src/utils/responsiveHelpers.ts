import type { Theme, SxProps } from '@mui/material/styles';
import { responsiveSpacing, responsiveFontSizes, cardDimensions, gridConfig } from '../theme/designTokens';

/**
 * Utility functions for creating responsive styles and layouts
 */

// Type for breakpoint keys
export type Breakpoint = 'xs' | 'sm' | 'md' | 'lg' | 'xl';

// Type for responsive values
export type ResponsiveValue<T> = T | Partial<Record<Breakpoint, T>>;

/**
 * Creates a responsive spacing object using theme spacing units
 */
export const createResponsiveSpacing = (
  values: ResponsiveValue<keyof typeof responsiveSpacing.xs>
): number | Record<string, number> => {
  if (typeof values === 'string') {
    return responsiveSpacing.md[values] || 2;
  }

  const responsiveValues: Record<string, number> = {};

  Object.entries(values).forEach(([breakpoint, value]) => {
    if (value && breakpoint in responsiveSpacing) {
      const bp = breakpoint as keyof typeof responsiveSpacing;
      responsiveValues[breakpoint] = responsiveSpacing[bp][value] || 2;
    }
  });

  return responsiveValues;
};

/**
 * Creates responsive padding using the spacing scale
 */
export const createResponsivePadding = (
  values: ResponsiveValue<keyof typeof responsiveSpacing.xs>
) => ({
  padding: createResponsiveSpacing(values),
});

/**
 * Creates responsive margin using the spacing scale
 */
export const createResponsiveMargin = (
  values: ResponsiveValue<keyof typeof responsiveSpacing.xs>
) => ({
  margin: createResponsiveSpacing(values),
});

/**
 * Creates responsive gap for flex/grid layouts
 */
export const createResponsiveGap = (
  values: ResponsiveValue<keyof typeof responsiveSpacing.xs>
) => ({
  gap: createResponsiveSpacing(values),
});

/**
 * Creates responsive font sizes from the design tokens
 */
export const createResponsiveFontSize = (
  scale: keyof typeof responsiveFontSizes
): SxProps<Theme> => ({
  fontSize: responsiveFontSizes[scale],
});

/**
 * Creates responsive card dimensions
 */
export const createResponsiveCardSize = (
  size: keyof typeof cardDimensions
): SxProps<Theme> => {
  const dimensions = cardDimensions[size];
  return {
    width: dimensions,
    height: Object.keys(dimensions).reduce((acc, breakpoint) => {
      const bp = breakpoint as keyof typeof dimensions;
      if (bp in dimensions) {
        acc[breakpoint] = dimensions[bp].height;
      }
      return acc;
    }, {} as Record<string, string>),
  };
};

/**
 * Creates responsive grid layout styles
 */
export const createResponsiveGrid = (
  customConfig?: Partial<Record<keyof typeof gridConfig, Partial<(typeof gridConfig)[keyof typeof gridConfig]>>>
): SxProps<Theme> => {
  const config = { ...gridConfig, ...customConfig };

  return {
    display: 'grid',
    gridTemplateColumns: {
      xs: `repeat(${config.mobile.columns}, 1fr)`,
      sm: `repeat(${config.tablet.columns}, 1fr)`,
      md: `repeat(${config.desktop.columns}, 1fr)`,
      lg: `repeat(${config.wide.columns}, 1fr)`,
    },
    gap: {
      xs: config.mobile.gap,
      sm: config.tablet.gap,
      md: config.desktop.gap,
      lg: config.wide.gap,
    },
    padding: {
      xs: config.mobile.padding,
      sm: config.tablet.padding,
      md: config.desktop.padding,
      lg: config.wide.padding,
    },
  };
};

/**
 * Creates responsive flex layout with direction changes
 */
export const createResponsiveFlex = (
  direction: ResponsiveValue<'row' | 'column'> = { xs: 'column', md: 'row' },
  align: ResponsiveValue<string> = 'stretch',
  justify: ResponsiveValue<string> = 'flex-start'
): SxProps<Theme> => ({
  display: 'flex',
  flexDirection: direction,
  alignItems: align,
  justifyContent: justify,
});

/**
 * Creates responsive text alignment
 */
export const createResponsiveTextAlign = (
  alignment: ResponsiveValue<'left' | 'center' | 'right'> = { xs: 'center', md: 'left' }
): SxProps<Theme> => ({
  textAlign: alignment,
});

/**
 * Creates responsive visibility (show/hide at breakpoints)
 */
export const createResponsiveVisibility = (
  visibility: Partial<Record<Breakpoint, boolean>>
): SxProps<Theme> => {
  const display: Record<string, string> = {};

  Object.entries(visibility).forEach(([breakpoint, visible]) => {
    display[breakpoint] = visible ? 'block' : 'none';
  });

  return { display };
};

/**
 * Creates touch-optimized sizing for interactive elements
 */
export const createTouchOptimizedSize = (
  theme: Theme,
  size: 'minimum' | 'comfortable' | 'large' = 'comfortable'
): SxProps<Theme> => {
  const touchSize = theme.mtg.spacing.touch[size === 'minimum' ? 'minTarget' : size === 'large' ? 'large' : 'comfortable'];

  return {
    minHeight: `${touchSize}px`,
    minWidth: `${touchSize}px`,
    // Ensure the element is tappable on touch devices
    '@media (pointer: coarse)': {
      minHeight: `${Math.max(touchSize, 44)}px`,
      minWidth: `${Math.max(touchSize, 44)}px`,
    },
  };
};

/**
 * Creates responsive container with max-width constraints
 */
export const createResponsiveContainer = (
  maxWidth: keyof typeof gridConfig = 'wide'
): SxProps<Theme> => ({
  width: '100%',
  maxWidth: {
    xs: '100%',
    sm: maxWidth === 'mobile' ? '100%' : '768px',
    md: maxWidth === 'mobile' || maxWidth === 'tablet' ? '768px' : '1024px',
    lg: maxWidth === 'wide' ? '1440px' : '1024px',
  },
  marginX: 'auto',
  paddingX: {
    xs: 2,
    sm: 3,
    md: 4,
  },
});

/**
 * Creates responsive card layout for different screen sizes
 */
export const createResponsiveCardLayout = (
  cardSize: keyof typeof cardDimensions = 'normal',
  customGap?: ResponsiveValue<number>
): SxProps<Theme> => {
  const dimensions = cardDimensions[cardSize];

  return {
    display: 'grid',
    gridTemplateColumns: {
      xs: `repeat(auto-fill, minmax(${dimensions.xs.width}, 1fr))`,
      sm: `repeat(auto-fill, minmax(${dimensions.sm.width}, 1fr))`,
      md: `repeat(auto-fill, minmax(${dimensions.md.width}, 1fr))`,
      lg: `repeat(auto-fill, minmax(${dimensions.lg.width}, 1fr))`,
    },
    gap: customGap || {
      xs: 2,
      sm: 3,
      md: 4,
      lg: 5,
    },
    justifyContent: 'center',
  };
};

/**
 * Utility for responsive conditional rendering props
 */
export const getResponsiveProps = <T>(
  values: ResponsiveValue<T>,
  currentBreakpoint: Breakpoint
): T => {
  if (typeof values !== 'object' || values === null) {
    return values as T;
  }

  // Type guard to check if values is a Record<Breakpoint, T>
  const breakpointValues = values as Partial<Record<Breakpoint, T>>;

  // Find the best matching breakpoint value
  const breakpointOrder: Breakpoint[] = ['xs', 'sm', 'md', 'lg', 'xl'];
  const currentIndex = breakpointOrder.indexOf(currentBreakpoint);

  // Look for exact match first
  if (breakpointValues[currentBreakpoint] !== undefined) {
    return breakpointValues[currentBreakpoint] as T;
  }

  // Fall back to previous breakpoints
  for (let i = currentIndex - 1; i >= 0; i--) {
    const bp = breakpointOrder[i];
    if (breakpointValues[bp] !== undefined) {
      return breakpointValues[bp] as T;
    }
  }

  // Fall back to next breakpoints
  for (let i = currentIndex + 1; i < breakpointOrder.length; i++) {
    const bp = breakpointOrder[i];
    if (breakpointValues[bp] !== undefined) {
      return breakpointValues[bp] as T;
    }
  }

  // Return first available value
  return Object.values(breakpointValues)[0] as T;
};

export default {
  createResponsiveSpacing,
  createResponsivePadding,
  createResponsiveMargin,
  createResponsiveGap,
  createResponsiveFontSize,
  createResponsiveCardSize,
  createResponsiveGrid,
  createResponsiveFlex,
  createResponsiveTextAlign,
  createResponsiveVisibility,
  createTouchOptimizedSize,
  createResponsiveContainer,
  createResponsiveCardLayout,
  getResponsiveProps,
};