import { useMemo } from 'react';
import { useLocation } from 'react-router-dom';

interface CollectorParamResult {
  hasCollector: boolean;
  collectorId: string | null;
}

/**
 * Hook to detect and extract the collector parameter from URL query string
 * Looks for 'ctor' parameter containing a GUID
 *
 * @returns Object with hasCollector boolean and collectorId string
 */
export function useCollectorParam(): CollectorParamResult {
  const location = useLocation();

  return useMemo(() => {
    const searchParams = new URLSearchParams(location.search);
    const collectorId = searchParams.get('ctor');

    return {
      hasCollector: collectorId !== null && collectorId.trim() !== '',
      collectorId: collectorId?.trim() || null
    };
  }, [location.search]);
}