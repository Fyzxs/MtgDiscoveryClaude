import { useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { COLLECTION_PARAM_NAME } from './useCollectionParam';

interface CollectorParamResult {
  hasCollector: boolean;
  collectorId: string | null;
}

/**
 * Hook to detect and extract the collection parameter from URL query string
 * Note: 'ctor' now represents collectionId, but this hook maintains 'collectorId'
 * naming for backwards compatibility with existing components.
 *
 * @returns Object with hasCollector boolean and collectorId string
 */
export function useCollectorParam(): CollectorParamResult {
  const location = useLocation();

  return useMemo(() => {
    const searchParams = new URLSearchParams(location.search);
    const collectorId = searchParams.get(COLLECTION_PARAM_NAME);

    return {
      hasCollector: collectorId !== null && collectorId.trim() !== '',
      collectorId: collectorId?.trim() || null
    };
  }, [location.search]);
}