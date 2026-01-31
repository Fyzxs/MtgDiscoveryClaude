import { useEffect, useCallback } from 'react';

interface UseBinderNavigationOptions {
  /** Current page number */
  currentPage: number;
  /** Total number of pages */
  totalPages: number;
  /** Navigate to next page */
  nextPage: () => void;
  /** Navigate to previous page */
  prevPage: () => void;
  /** Navigate to specific page */
  goToPage: (page: number) => void;
  /** Whether keyboard navigation is enabled */
  enabled?: boolean;
}

/**
 * Hook for keyboard navigation in the binder view.
 * Handles arrow keys, Home, End, Page Up/Down for navigation.
 */
export const useBinderNavigation = ({
  currentPage,
  totalPages,
  nextPage,
  prevPage,
  goToPage,
  enabled = true
}: UseBinderNavigationOptions) => {
  const handleKeyDown = useCallback((e: KeyboardEvent) => {
    if (!enabled) return;

    // Don't interfere with input fields or text areas
    const target = e.target as HTMLElement;
    if (
      target instanceof HTMLInputElement ||
      target instanceof HTMLTextAreaElement ||
      target.isContentEditable
    ) {
      return;
    }

    switch (e.key) {
      case 'ArrowRight':
      case 'ArrowDown':
      case 'x':
      case 'X':
      case '.':
        e.preventDefault();
        nextPage();
        break;

      case 'ArrowLeft':
      case 'ArrowUp':
      case 'z':
      case 'Z':
      case ',':
        e.preventDefault();
        prevPage();
        break;

      case 'Home':
        e.preventDefault();
        goToPage(1);
        break;

      case 'End':
        e.preventDefault();
        goToPage(totalPages);
        break;

      case 'PageDown':
        e.preventDefault();
        // Jump 5 pages forward
        goToPage(Math.min(currentPage + 5, totalPages));
        break;

      case 'PageUp':
        e.preventDefault();
        // Jump 5 pages back
        goToPage(Math.max(currentPage - 5, 1));
        break;
    }
  }, [enabled, currentPage, totalPages, nextPage, prevPage, goToPage]);

  useEffect(() => {
    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [handleKeyDown]);
};
