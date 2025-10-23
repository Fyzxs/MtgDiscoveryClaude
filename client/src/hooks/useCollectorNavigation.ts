import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCollectorParam } from './useCollectorParam';

interface NavigationHelpers {
  /**
   * Builds a URL with collector parameter preserved
   */
  buildUrlWithCollector: (path: string, additionalParams?: Record<string, string>) => string;

  /**
   * Navigates to a path while preserving collector parameter
   * @param path - The path to navigate to
   * @param additionalParams - Additional query parameters to include
   * @param options - Navigation options (replace: true to replace history entry)
   */
  navigateWithCollector: (path: string, additionalParams?: Record<string, string>, options?: { replace?: boolean }) => void;

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
  const navigate = useNavigate();

  const buildUrlWithCollector = useCallback((path: string, additionalParams?: Record<string, string>): string => {
    // Parse the path to separate pathname from any existing search params
    const pathParts = path.split('?');
    const pathname = pathParts[0];

    // Create a clean URL with only the pathname
    const url = new URL(pathname, window.location.origin);

    // Clear any existing search params to ensure we start fresh
    url.search = '';

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

  const navigateWithCollector = useCallback((path: string, additionalParams?: Record<string, string>, options?: { replace?: boolean }): void => {
    const fullUrl = buildUrlWithCollector(path, additionalParams);
    // Use React Router navigation instead of window.location to preserve SPA behavior and cache
    navigate(fullUrl, options);
  }, [buildUrlWithCollector, navigate]);

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