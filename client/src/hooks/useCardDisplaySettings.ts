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

    // Determine display mode
    const displayMode: DisplayMode = explicitMode ?? (isMobile ? 'compact' : 'full');

    // Determine overlay behavior
    let overlayBehavior: OverlayBehavior;
    if (isMobile) {
      overlayBehavior = 'tap';
    } else if (isTablet) {
      overlayBehavior = 'tap'; // Also tap on tablet for touch support
    } else {
      overlayBehavior = 'hover';
    }

    // Determine overlay variant
    let overlayVariant: OverlayVariant;
    if (displayMode === 'compact') {
      overlayVariant = 'minimal';
    } else if (isTablet) {
      overlayVariant = 'compact';
    } else {
      overlayVariant = 'full';
    }

    // Determine Scryfall image size
    let imageScryfallSize: 'small' | 'normal' | 'large';
    if (isMobile) {
      imageScryfallSize = 'small';
    } else if (isTablet) {
      imageScryfallSize = 'normal';
    } else {
      imageScryfallSize = 'normal';
    }

    // Determine what to show
    const showBadges = !isMobile; // Hide badges on mobile to reduce clutter
    const showZoomIndicator = !isMobile; // Hide on mobile - tap opens details

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
