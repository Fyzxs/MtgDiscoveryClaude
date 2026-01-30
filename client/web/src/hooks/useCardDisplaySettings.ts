import { useMemo } from 'react';
import { useResponsiveBreakpoints } from './useResponsiveBreakpoints';

export type CardSize = 'xs' | 'sm' | 'md' | 'lg' | 'xl';
export type DisplayMode = 'full' | 'compact';
export type OverlayBehavior = 'always' | 'hover' | 'tap';
export type OverlayVariant = 'full' | 'compact' | 'minimal';

export interface CardDisplaySettings {
  size: CardSize;
  displayMode: DisplayMode;
  overlayBehavior: OverlayBehavior;
  overlayVariant: OverlayVariant;
  imageScryfallSize: 'small' | 'normal' | 'large';
  showBadges: boolean;
  showZoomIndicator: boolean;
  isMobile: boolean;
  isTablet: boolean;
  breakpoints: ReturnType<typeof useResponsiveBreakpoints>;
}

interface UseCardDisplaySettingsOptions {
  explicitSize?: CardSize;
  explicitMode?: DisplayMode;
}

/**
 * Hook that determines card display settings based on breakpoint
 * Returns appropriate sizes, overlay behaviors, and image sizes for each breakpoint
 */
export function useCardDisplaySettings(
  options: UseCardDisplaySettingsOptions = {}
): CardDisplaySettings {
  const { explicitSize, explicitMode } = options;
  const breakpoints = useResponsiveBreakpoints();
  const { isMobile, isTablet, current } = breakpoints;

  const settings = useMemo((): Omit<CardDisplaySettings, 'breakpoints'> => {
    // Determine card size based on breakpoint or explicit override
    let size: CardSize;
    if (explicitSize) {
      size = explicitSize;
    } else {
      switch (current) {
        case 'mobile':
          size = 'xs';
          break;
        case 'tablet':
          size = 'sm';
          break;
        case 'desktop':
          size = 'md';
          break;
        case 'wide':
          size = 'lg';
          break;
        default:
          size = 'md';
      }
    }

    // Determine display mode - compact for mobile and tablet
    const isTouchDevice = isMobile || isTablet;
    const displayMode: DisplayMode = explicitMode ?? (isTouchDevice ? 'compact' : 'full');

    // Determine overlay behavior
    let overlayBehavior: OverlayBehavior;
    if (isTouchDevice) {
      overlayBehavior = 'tap';
    } else {
      overlayBehavior = 'hover';
    }

    // Determine overlay variant - minimal for mobile/tablet, full for desktop
    let overlayVariant: OverlayVariant;
    if (isTouchDevice) {
      overlayVariant = 'minimal';
    } else {
      overlayVariant = 'full';
    }

    // Determine Scryfall image size
    let imageScryfallSize: 'small' | 'normal' | 'large';
    if (isMobile) {
      imageScryfallSize = 'small';
    } else {
      imageScryfallSize = 'normal';
    }

    // Determine what to show - hide badges/zoom on touch devices
    const showBadges = !isTouchDevice;
    const showZoomIndicator = !isTouchDevice;

    return {
      size,
      displayMode,
      overlayBehavior,
      overlayVariant,
      imageScryfallSize,
      showBadges,
      showZoomIndicator,
      isMobile,
      isTablet,
    };
  }, [current, isMobile, isTablet, explicitSize, explicitMode]);

  return {
    ...settings,
    breakpoints,
  };
}
