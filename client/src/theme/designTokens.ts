import type { Theme } from '@mui/material/styles';

/**
 * Responsive design tokens for the MTG Discovery application
 * Provides consistent spacing, sizing, and layout values across breakpoints
 */

// Responsive spacing scale
export const responsiveSpacing = {
  // Base spacing units (multiplied by theme.spacing())
  xs: {
    tiny: 0.5,    // 4px
    small: 1,     // 8px
    medium: 2,    // 16px
    large: 3,     // 24px
    huge: 4,      // 32px
  },
  sm: {
    tiny: 0.5,    // 4px
    small: 1.5,   // 12px
    medium: 2.5,  // 20px
    large: 4,     // 32px
    huge: 6,      // 48px
  },
  md: {
    tiny: 1,      // 8px
    small: 2,     // 16px
    medium: 3,    // 24px
    large: 5,     // 40px
    huge: 8,      // 64px
  },
  lg: {
    tiny: 1,      // 8px
    small: 2,     // 16px
    medium: 4,    // 32px
    large: 6,     // 48px
    huge: 10,     // 80px
  },
};

// Touch target sizes for different contexts
export const touchTargets = {
  minimum: '44px',      // iOS/Android minimum
  comfortable: '48px',  // Comfortable touch size
  large: '56px',        // Large touch targets (FABs, primary actions)
  huge: '64px',         // Extra large (important CTAs)
};

// Responsive font scales
export const responsiveFontSizes = {
  display: {
    xs: '1.75rem',  // 28px
    sm: '2rem',     // 32px
    md: '2.5rem',   // 40px
    lg: '3rem',     // 48px
  },
  heading: {
    xs: '1.25rem',  // 20px
    sm: '1.5rem',   // 24px
    md: '1.75rem',  // 28px
    lg: '2rem',     // 32px
  },
  subheading: {
    xs: '1rem',     // 16px
    sm: '1.125rem', // 18px
    md: '1.25rem',  // 20px
    lg: '1.375rem', // 22px
  },
  body: {
    xs: '0.875rem', // 14px
    sm: '0.875rem', // 14px
    md: '1rem',     // 16px
    lg: '1rem',     // 16px
  },
  caption: {
    xs: '0.75rem',  // 12px
    sm: '0.75rem',  // 12px
    md: '0.8125rem', // 13px
    lg: '0.875rem', // 14px
  },
};

// Layout container sizes
export const containerSizes = {
  mobile: '100%',
  tablet: '768px',
  desktop: '1024px',
  wide: '1440px',
  max: '1920px',
};

// Grid configurations for different screen sizes
export const gridConfig = {
  mobile: {
    columns: 1,
    minItemWidth: '280px',
    gap: 2,
    padding: 2,
  },
  tablet: {
    columns: 2,
    minItemWidth: '320px',
    gap: 3,
    padding: 3,
  },
  desktop: {
    columns: 3,
    minItemWidth: '300px',
    gap: 4,
    padding: 4,
  },
  wide: {
    columns: 4,
    minItemWidth: '320px',
    gap: 5,
    padding: 5,
  },
};

// Card dimension presets for different contexts
export const cardDimensions = {
  compact: {
    xs: { width: '140px', height: '195px' },
    sm: { width: '160px', height: '223px' },
    md: { width: '180px', height: '251px' },
    lg: { width: '200px', height: '279px' },
  },
  normal: {
    xs: { width: '160px', height: '223px' },
    sm: { width: '200px', height: '279px' },
    md: { width: '240px', height: '335px' },
    lg: { width: '280px', height: '391px' },
  },
  large: {
    xs: { width: '200px', height: '279px' },
    sm: { width: '240px', height: '335px' },
    md: { width: '300px', height: '419px' },
    lg: { width: '360px', height: '502px' },
  },
};

// Z-index scale for layering
export const zIndexScale = {
  behind: -1,
  base: 0,
  overlay: 100,
  dropdown: 200,
  modal: 300,
  popover: 400,
  tooltip: 500,
  notification: 600,
  maximum: 999,
};

// Animation durations and easing
export const animations = {
  durations: {
    instant: '0ms',
    fast: '150ms',
    normal: '250ms',
    slow: '350ms',
    slower: '500ms',
  },
  easings: {
    linear: 'linear',
    ease: 'ease',
    easeIn: 'ease-in',
    easeOut: 'ease-out',
    easeInOut: 'ease-in-out',
    bounce: 'cubic-bezier(0.68, -0.55, 0.265, 1.55)',
    smooth: 'cubic-bezier(0.4, 0, 0.2, 1)',
  },
};

// Helper functions for accessing responsive values
export const getResponsiveSpacing = (size: keyof typeof responsiveSpacing.xs, theme: Theme) => ({
  xs: theme.spacing(responsiveSpacing.xs[size]),
  sm: theme.spacing(responsiveSpacing.sm[size]),
  md: theme.spacing(responsiveSpacing.md[size]),
  lg: theme.spacing(responsiveSpacing.lg[size]),
});

export const getResponsiveFontSize = (scale: keyof typeof responsiveFontSizes) => responsiveFontSizes[scale];

export const getCardDimensions = (size: keyof typeof cardDimensions) => cardDimensions[size];

export const getGridConfig = (breakpoint: keyof typeof gridConfig) => gridConfig[breakpoint];

// Utility function to create responsive sx objects
export const createResponsiveSx = (
  property: string,
  values: { xs?: unknown; sm?: unknown; md?: unknown; lg?: unknown; xl?: unknown }
) => ({
  [property]: values,
});

// Common responsive patterns
export const responsivePatterns = {
  // Hide on mobile, show on desktop
  hideOnMobile: {
    display: { xs: 'none', md: 'block' },
  },

  // Show on mobile, hide on desktop
  showOnMobile: {
    display: { xs: 'block', md: 'none' },
  },

  // Stack on mobile, row on desktop
  responsiveStack: {
    flexDirection: { xs: 'column', md: 'row' },
  },

  // Full width on mobile, auto on desktop
  responsiveWidth: {
    width: { xs: '100%', md: 'auto' },
  },

  // Center on mobile, left align on desktop
  responsiveAlign: {
    textAlign: { xs: 'center', md: 'left' },
  },

  // Responsive padding
  responsivePadding: {
    padding: getResponsiveSpacing('medium', {} as Theme),
  },

  // Responsive margin
  responsiveMargin: {
    margin: getResponsiveSpacing('small', {} as Theme),
  },
};

export default {
  responsiveSpacing,
  touchTargets,
  responsiveFontSizes,
  containerSizes,
  gridConfig,
  cardDimensions,
  zIndexScale,
  animations,
  responsivePatterns,
  getResponsiveSpacing,
  getResponsiveFontSize,
  getCardDimensions,
  getGridConfig,
  createResponsiveSx,
};