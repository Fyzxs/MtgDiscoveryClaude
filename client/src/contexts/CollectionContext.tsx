import React, { createContext, useContext, type ReactNode } from 'react';
import { useApolloClient } from '@apollo/client/react';
import { getCollectionManager, CollectionManager } from '../services/collectionManager';

interface CollectionContextValue {
  collectionManager: CollectionManager;
}

const CollectionContext = createContext<CollectionContextValue | null>(null);

interface CollectionProviderProps {
  children: ReactNode;
}

/**
 * Provider for the CollectionManager singleton
 * Should be placed high in the component tree to ensure same instance is used everywhere
 */
export const CollectionProvider: React.FC<CollectionProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const collectionManager = getCollectionManager(apolloClient);

  return (
    <CollectionContext.Provider value={{ collectionManager }}>
      {children}
    </CollectionContext.Provider>
  );
};

/**
 * Hook to access the CollectionManager from context
 */
export const useCollectionManager = (): CollectionManager => {
  const context = useContext(CollectionContext);

  if (!context) {
    throw new Error('useCollectionManager must be used within a CollectionProvider');
  }

  return context.collectionManager;
};

/**
 * Hook to get collection cache statistics
 * Useful for debugging and monitoring cache performance
 */
export const useCollectionCacheStats = () => {
  const collectionManager = useCollectionManager();

  // This could be made reactive with state updates if needed
  const getStats = () => collectionManager.getCacheStats();

  return { getStats };
};