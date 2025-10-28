import { useState, useCallback, useRef } from 'react';

/**
 * Hook to manage loading state with a minimum display time
 * Ensures loading indicators don't flash too quickly
 *
 * @param minimumMs - Minimum time to show loading state (default: 400ms)
 * @returns Object with isLoading state and show/hide functions
 */
export function useMinimumLoadingTime(minimumMs: number = 400) {
  const [isLoading, setIsLoading] = useState(false);
  const loadingStartTimeRef = useRef<number | null>(null);
  const timeoutRef = useRef<NodeJS.Timeout | null>(null);
  const showTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  const showLoading = useCallback(() => {
    // Small delay to allow dropdowns/selects to close and lose focus
    // This prevents aria-hidden accessibility warnings
    if (showTimeoutRef.current) {
      clearTimeout(showTimeoutRef.current);
    }
    showTimeoutRef.current = setTimeout(() => {
      loadingStartTimeRef.current = Date.now();
      setIsLoading(true);
      showTimeoutRef.current = null;
    }, 50);
  }, []);

  const hideLoading = useCallback(() => {
    // Cancel pending show if it hasn't happened yet
    if (showTimeoutRef.current) {
      clearTimeout(showTimeoutRef.current);
      showTimeoutRef.current = null;
    }

    if (loadingStartTimeRef.current === null) {
      setIsLoading(false);
      return;
    }

    const elapsed = Date.now() - loadingStartTimeRef.current;
    const remaining = minimumMs - elapsed;

    if (remaining > 0) {
      // Wait for the remaining time before hiding
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
      timeoutRef.current = setTimeout(() => {
        setIsLoading(false);
        loadingStartTimeRef.current = null;
        timeoutRef.current = null;
      }, remaining);
    } else {
      // Minimum time has passed, hide immediately
      setIsLoading(false);
      loadingStartTimeRef.current = null;
    }
  }, [minimumMs]);

  return {
    isLoading,
    showLoading,
    hideLoading
  };
}
