import type { SxProps, Theme } from '@mui/material/styles';

/**
 * Common card container styles
 */
export const cardContainer: SxProps<Theme> = {
  position: 'relative',
  overflow: 'hidden',
  cursor: 'pointer',
  transition: 'all 0.3s ease',
};

/**
 * Absolute positioning for overlays
 */
export const absoluteOverlay: SxProps<Theme> = {
  position: 'absolute',
  top: 0,
  left: 0,
  right: 0,
  bottom: 0,
};

/**
 * Card overlay positioning (bottom aligned)
 */
export const cardOverlayPosition: SxProps<Theme> = {
  position: 'absolute',
  bottom: 0,
  left: 0,
  right: 0,
  zIndex: 10,
};

/**
 * Badge container positioning (top corners)
 */
export const badgeContainer: SxProps<Theme> = {
  position: 'absolute',
  top: 8,
  left: 8,
  right: 8,
  zIndex: 20,
  pointerEvents: 'none',
};

/**
 * Hidden element for screen readers
 */
export const srOnly: SxProps<Theme> = {
  position: 'absolute',
  left: '-9999px',
  width: '1px',
  height: '1px',
  overflow: 'hidden',
};

/**
 * Card image container
 */
export const imageContainer: SxProps<Theme> = {
  position: 'relative',
  width: '100%',
  aspectRatio: '1.395', // Standard MTG card ratio
  overflow: 'hidden',
};

/**
 * Utility to combine multiple style objects
 */
export const combineStyles = (...styles: (SxProps<Theme> | undefined)[]): SxProps<Theme> => {
  return styles.filter(Boolean).reduce((acc, style) => ({ ...acc, ...(style as SxProps<Theme>) }), {} as SxProps<Theme>);
};