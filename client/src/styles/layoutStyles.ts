import type { SxProps, Theme } from '@mui/material/styles';

/**
 * Common flexbox layout patterns
 */

// Flex containers
export const flexRow: SxProps<Theme> = {
  display: 'flex',
  flexDirection: 'row',
};

export const flexCol: SxProps<Theme> = {
  display: 'flex',
  flexDirection: 'column',
};

export const flexCenter: SxProps<Theme> = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
};

export const flexBetween: SxProps<Theme> = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
};

export const flexStart: SxProps<Theme> = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-start',
};

export const flexEnd: SxProps<Theme> = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-end',
};

// Flex wrapping
export const flexWrap: SxProps<Theme> = {
  flexWrap: 'wrap',
};

// Common gaps
export const gap1: SxProps<Theme> = { gap: 1 };
export const gap2: SxProps<Theme> = { gap: 2 };
export const gap3: SxProps<Theme> = { gap: 3 };
export const gap4: SxProps<Theme> = { gap: 4 };

// Full width/height
export const fullWidth: SxProps<Theme> = {
  width: '100%',
};

export const fullHeight: SxProps<Theme> = {
  height: '100%',
};

export const fullSize: SxProps<Theme> = {
  width: '100%',
  height: '100%',
};

// Page layout
export const pageContainer: SxProps<Theme> = {
  display: 'flex',
  flexDirection: 'column',
  minHeight: '100vh',
};

export const mainContent: SxProps<Theme> = {
  flex: 1,
  display: 'flex',
  flexDirection: 'column',
};

// Responsive utilities
export const hideOnMobile: SxProps<Theme> = {
  display: { xs: 'none', sm: 'block' },
};

export const hideOnDesktop: SxProps<Theme> = {
  display: { xs: 'block', sm: 'none' },
};

export const stackOnMobile: SxProps<Theme> = {
  flexDirection: { xs: 'column', sm: 'row' },
};

/**
 * Create a flex container with custom gap
 */
export const flexWithGap = (gap: number | string): SxProps<Theme> => ({
  display: 'flex',
  gap,
});

/**
 * Create a centered container with max width
 */
export const centeredContainer = (maxWidth: string | number = 'lg'): SxProps<Theme> => ({
  width: '100%',
  maxWidth,
  mx: 'auto',
  px: { xs: 2, sm: 3 },
});