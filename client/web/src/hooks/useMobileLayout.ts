import { useState, useCallback } from 'react';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useResponsiveBreakpoints } from './useResponsiveBreakpoints';

export interface MobileLayoutState {
  isMobile: boolean;
  isTablet: boolean;
  isTouch: boolean;
  showFilterDrawer: boolean;
  toggleFilterDrawer: () => void;
  openFilterDrawer: () => void;
  closeFilterDrawer: () => void;
}

/**
 * Hook for mobile layout state management
 * Provides breakpoint detection, touch detection, and filter drawer state
 */
export function useMobileLayout(): MobileLayoutState {
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const isTouch = useMediaQuery('(pointer: coarse)');
  const [showFilterDrawer, setShowFilterDrawer] = useState(false);

  const toggleFilterDrawer = useCallback(() => {
    setShowFilterDrawer(prev => !prev);
  }, []);

  const openFilterDrawer = useCallback(() => {
    setShowFilterDrawer(true);
  }, []);

  const closeFilterDrawer = useCallback(() => {
    setShowFilterDrawer(false);
  }, []);

  return {
    isMobile,
    isTablet,
    isTouch,
    showFilterDrawer,
    toggleFilterDrawer,
    openFilterDrawer,
    closeFilterDrawer,
  };
}
