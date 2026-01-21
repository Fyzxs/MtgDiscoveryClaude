import { useEffect, useState } from 'react';
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';

export interface ResponsiveBreakpoints {
  isMobile: boolean;
  isTablet: boolean;
  isDesktop: boolean;
  isWide: boolean;
  current: 'mobile' | 'tablet' | 'desktop' | 'wide';
  screenWidth: number;
}

/**
 * Hook for responsive breakpoint detection using MUI breakpoints
 * Aligned with MUI defaults: xs (0-599), sm (600-899), md (900-1199), lg (1200+)
 */
export const useResponsiveBreakpoints = (): ResponsiveBreakpoints => {
  const theme = useTheme();
  const [screenWidth, setScreenWidth] = useState(0);

  // Use MUI's breakpoint helpers for consistent behavior
  // down('sm') = 0-599px, between('sm','md') = 600-899px, etc.
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));      // 0-599px (xs)
  const isTablet = useMediaQuery(theme.breakpoints.between('sm', 'md')); // 600-899px (sm)
  const isDesktop = useMediaQuery(theme.breakpoints.between('md', 'lg')); // 900-1199px (md)
  const isWide = useMediaQuery(theme.breakpoints.up('lg'));          // 1200px+ (lg+)

  // Update screen width on resize
  useEffect(() => {
    const updateScreenWidth = () => {
      setScreenWidth(window.innerWidth);
    };

    updateScreenWidth();
    window.addEventListener('resize', updateScreenWidth);

    return () => {
      window.removeEventListener('resize', updateScreenWidth);
    };
  }, []);

  // Determine current breakpoint
  const getCurrentBreakpoint = (): 'mobile' | 'tablet' | 'desktop' | 'wide' => {
    if (isWide) return 'wide';
    if (isDesktop) return 'desktop';
    if (isTablet) return 'tablet';
    return 'mobile';
  };

  return {
    isMobile,
    isTablet,
    isDesktop,
    isWide,
    current: getCurrentBreakpoint(),
    screenWidth,
  };
};