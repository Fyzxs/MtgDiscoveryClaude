import { useCallback } from 'react';
import { useCollectorParam } from './useCollectorParam';

interface NavigationHelpers {
  /**
   * Builds a URL with collector parameter preserved
   */
  buildUrlWithCollector: (path: string, additionalParams?: Record<string, string>) => string;

  /**
   * Navigates to a path while preserving collector parameter
   */
  navigateWithCollector: (path: string, additionalParams?: Record<string, string>) => void;

  /**
   * Creates event handler that preserves collector parameter
   */
  createCollectorClickHandler: (path: string, additionalParams?: Record<string, string>) => (e: React.MouseEvent) => void;

  /**
   * Current collector parameter info
   */
  collectorParam: {
    hasCollector: boolean;
    collectorId: string | null;
  };
}

/**
 * Hook that provides utilities for navigating while preserving the collector (ctor) parameter
 *
 * This ensures that when users have a collector context active (via ?ctor=<id>),
 * all navigation maintains that context so they continue to see collection information
 */
export const useCollectorNavigation = (): NavigationHelpers => {
  const collectorParam = useCollectorParam();

  const buildUrlWithCollector = useCallback((path: string, additionalParams?: Record<string, string>): string => {
    const url = new URL(path, window.location.origin);

    // Add collector parameter if it exists
    if (collectorParam.hasCollector && collectorParam.collectorId) {
      url.searchParams.set('ctor', collectorParam.collectorId);
    }

    // Add any additional parameters
    if (additionalParams) {
      Object.entries(additionalParams).forEach(([key, value]) => {
        url.searchParams.set(key, value);
      });
    }

    return url.pathname + url.search;
  }, [collectorParam.hasCollector, collectorParam.collectorId]);

  const navigateWithCollector = useCallback((path: string, additionalParams?: Record<string, string>): void => {
    const fullUrl = buildUrlWithCollector(path, additionalParams);
    window.location.href = fullUrl;
  }, [buildUrlWithCollector]);

  const createCollectorClickHandler = useCallback((path: string, additionalParams?: Record<string, string>) => {
    return (e: React.MouseEvent) => {
      e.preventDefault();
      navigateWithCollector(path, additionalParams);
    };
  }, [navigateWithCollector]);

  return {
    buildUrlWithCollector,
    navigateWithCollector,
    createCollectorClickHandler,
    collectorParam
  };
};