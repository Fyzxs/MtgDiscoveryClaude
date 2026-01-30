import { useState, useCallback } from 'react';
import type { Breakpoint } from '@mui/material/styles';

export type CardSizePref = 'compact' | 'normal' | 'large';

export interface CardSizePreference {
  size: CardSizePref;
  setSize: (size: CardSizePref) => void;
  getMinItemWidth: (breakpoint: Breakpoint) => number;
  getImageSize: () => 'small' | 'normal' | 'large';
}

const STORAGE_KEY = 'cardSizePreference';

const WIDTH_MAP: Record<CardSizePref, Record<Breakpoint, number>> = {
  compact: { xs: 100, sm: 120, md: 140, lg: 160, xl: 180 },
  normal: { xs: 110, sm: 150, md: 180, lg: 220, xl: 250 },
  large: { xs: 130, sm: 180, md: 220, lg: 260, xl: 280 },
};

const IMAGE_SIZE_MAP: Record<CardSizePref, 'small' | 'normal' | 'large'> = {
  compact: 'small',
  normal: 'normal',
  large: 'normal',
};

/**
 * Hook for managing user's card size preference
 * Persists to localStorage and provides width/image size mappings per breakpoint
 */
export function useCardSizePreference(): CardSizePreference {
  const [size, setSizeState] = useState<CardSizePref>(() => {
    if (typeof window === 'undefined') return 'normal';
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored === 'compact' || stored === 'normal' || stored === 'large') {
      return stored;
    }
    return 'normal';
  });

  const setSize = useCallback((newSize: CardSizePref) => {
    setSizeState(newSize);
    localStorage.setItem(STORAGE_KEY, newSize);
  }, []);

  const getMinItemWidth = useCallback((breakpoint: Breakpoint): number => {
    return WIDTH_MAP[size][breakpoint];
  }, [size]);

  const getImageSize = useCallback((): 'small' | 'normal' | 'large' => {
    return IMAGE_SIZE_MAP[size];
  }, [size]);

  return { size, setSize, getMinItemWidth, getImageSize };
}
