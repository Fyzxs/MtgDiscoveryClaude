import { useLocation, useNavigate } from 'react-router-dom';
import { useCallback } from 'react';

export const COLLECTION_PARAM_NAME = 'ctor';

export function useCollectionParam() {
  const location = useLocation();
  const navigate = useNavigate();

  const searchParams = new URLSearchParams(location.search);
  const collectionId = searchParams.get(COLLECTION_PARAM_NAME);

  const setCollectionId = useCallback((id: string | null) => {
    const params = new URLSearchParams(location.search);
    if (id) {
      params.set(COLLECTION_PARAM_NAME, id);
    } else {
      params.delete(COLLECTION_PARAM_NAME);
    }
    const newUrl = params.toString()
      ? `${location.pathname}?${params.toString()}`
      : location.pathname;
    navigate(newUrl);
  }, [location.search, location.pathname, navigate]);

  const navigateWithCollection = useCallback((pathname: string, id: string) => {
    navigate(`${pathname}?${COLLECTION_PARAM_NAME}=${id}`);
  }, [navigate]);

  return {
    collectionId,
    hasCollection: collectionId !== null,
    setCollectionId,
    navigateWithCollection,
    clearCollection: useCallback(() => setCollectionId(null), [setCollectionId]),
  };
}
