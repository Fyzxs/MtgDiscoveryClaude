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
 * Hook for responsive breakpoint detection using both MUI and custom MTG breakpoints
 * Provides real-time screen size information for responsive behavior
 */
export const useResponsiveBreakpoints = (): ResponsiveBreakpoints => {
  const theme = useTheme();
  const [screenWidth, setScreenWidth] = useState(0);

  // MUI breakpoint queries
  const isMuiXs = useMediaQuery(theme.breakpoints.down('sm'));
  const isMuiSm = useMediaQuery(theme.breakpoints.between('sm', 'md'));
  const isMuiMd = useMediaQuery(theme.breakpoints.between('md', 'lg'));
  const isMuiLg = useMediaQuery(theme.breakpoints.up('lg'));

  // Custom MTG breakpoint queries
  const isMobile = useMediaQuery(`(max-width: ${theme.mtg.breakpoints.tablet})`);
  const isTablet = useMediaQuery(
    `(min-width: ${theme.mtg.breakpoints.tablet}) and (max-width: ${theme.mtg.breakpoints.desktop})`
  );
  const isDesktop = useMediaQuery(
    `(min-width: ${theme.mtg.breakpoints.desktop}) and (max-width: ${theme.mtg.breakpoints.wide})`
  );
  const isWide = useMediaQuery(`(min-width: ${theme.mtg.breakpoints.wide})`);

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