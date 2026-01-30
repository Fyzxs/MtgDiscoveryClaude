import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import type { ReactNode } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useQuery } from '@apollo/client/react';
import { GET_MY_COLLECTIONS } from '../graphql/queries/getMyCollections';
import type { GetMyCollectionsQuery } from '../generated/graphql';
import { isNetworkAvailable, resetNetworkStatus } from '../graphql/apollo-client';

interface Collection {
  collectionId: string;
  ownerId: string;
  name: string;
  type: string;
  visibility: string;
  isDefault: boolean;
  createdAt: string;
  updatedAt: string;
}

interface CollectionManagementContextType {
  collections: Collection[];
  activeCollectionId: string | null;
  activeCollection: Collection | null;
  isLoading: boolean;
  error: Error | null;
  setActiveCollection: (collectionId: string) => void;
  refetchCollections: () => void;
}

const ACTIVE_COLLECTION_KEY = 'mtg-discovery-active-collection';

const CollectionManagementContext = createContext<CollectionManagementContextType | null>(null);

interface CollectionManagementProviderProps {
  children: ReactNode;
}

export function CollectionManagementProvider({ children }: CollectionManagementProviderProps) {
  const { isAuthenticated } = useAuth0();
  const [activeCollectionId, setActiveCollectionIdState] = useState<string | null>(() => {
    return localStorage.getItem(ACTIVE_COLLECTION_KEY);
  });

  const [networkAvailable, setNetworkAvailable] = useState(() => isNetworkAvailable());

  const { data, loading, error: queryError, refetch } = useQuery<GetMyCollectionsQuery>(GET_MY_COLLECTIONS, {
    skip: !isAuthenticated || !networkAvailable,
    fetchPolicy: networkAvailable ? 'cache-and-network' : 'cache-only',
    notifyOnNetworkStatusChange: true,
  });

  // Update network available state when query errors occur
  useEffect(() => {
    if (queryError) {
      // Check if network is now unavailable
      const available = isNetworkAvailable();
      if (available !== networkAvailable) {
        setNetworkAvailable(available);
      }
    }
  }, [queryError, networkAvailable]);

  // Extract collections from query response
  const collections: Collection[] = (() => {
    if (data?.myCollections?.__typename === 'CollectionsSuccessResponse') {
      return data.myCollections.data.map(c => ({
        collectionId: c.collectionId,
        ownerId: c.ownerId,
        name: c.name,
        type: c.type,
        visibility: c.visibility,
        isDefault: c.isDefault,
        createdAt: c.createdAt,
        updatedAt: c.updatedAt,
      }));
    }
    return [];
  })();

  const isLoading = loading;
  const error = queryError ?? (
    data?.myCollections?.__typename === 'FailureResponse'
      ? new Error(data.myCollections.status?.message ?? 'Failed to load collections')
      : null
  );

  const setActiveCollection = (collectionId: string) => {
    setActiveCollectionIdState(collectionId);
    localStorage.setItem(ACTIVE_COLLECTION_KEY, collectionId);
  };

  const refetchCollections = useCallback(() => {
    // Reset network status to allow retries
    resetNetworkStatus();
    setNetworkAvailable(true);
    refetch();
  }, [refetch]);

  // Find active collection from list, fallback to default
  const activeCollection = collections.find(c => c.collectionId === activeCollectionId)
    ?? collections.find(c => c.isDefault) ?? null;

  // If no active collection is set but we have collections, set the default
  useEffect(() => {
    if (!activeCollectionId && collections.length > 0) {
      const defaultCollection = collections.find(c => c.isDefault);
      if (defaultCollection) {
        setActiveCollection(defaultCollection.collectionId);
      }
    }
  }, [collections, activeCollectionId]);

  // Fallback to default collection if active collection no longer exists (deleted/revoked)
  useEffect(() => {
    if (activeCollectionId && collections.length > 0) {
      const activeExists = collections.some(c => c.collectionId === activeCollectionId);
      if (!activeExists) {
        const defaultCollection = collections.find(c => c.isDefault);
        if (defaultCollection) {
          setActiveCollection(defaultCollection.collectionId);
        } else if (collections.length > 0) {
          // Fall back to first collection if no default exists
          setActiveCollection(collections[0].collectionId);
        } else {
          // No collections available, clear the selection
          setActiveCollectionIdState(null);
          localStorage.removeItem(ACTIVE_COLLECTION_KEY);
        }
      }
    }
  }, [activeCollectionId, collections]);

  return (
    <CollectionManagementContext.Provider value={{
      collections,
      activeCollectionId,
      activeCollection,
      isLoading,
      error,
      setActiveCollection,
      refetchCollections,
    }}>
      {children}
    </CollectionManagementContext.Provider>
  );
}

export function useCollectionManagement() {
  const context = useContext(CollectionManagementContext);
  if (!context) {
    throw new Error('useCollectionManagement must be used within CollectionManagementProvider');
  }
  return context;
}
