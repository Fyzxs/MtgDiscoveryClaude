import { useState, useEffect } from 'react';
import { useApolloClient } from '@apollo/client/react';
import { getCollectionManager } from '../services/collectionManager';
import type { UserCardData } from '../types/card';

interface UseCollectionDataOptions {
  cardId: string;
  userId: string;
  enabled?: boolean;
}

interface UseCollectionDataResult {
  collectionData: UserCardData | null;
  loading: boolean;
  error: Error | null;
  refetch: () => Promise<void>;
}

/**
 * Hook for requesting collection data for a single card
 * Automatically handles batching and caching via CollectionManager
 */
export const useCollectionData = ({
  cardId,
  userId,
  enabled = true
}: UseCollectionDataOptions): UseCollectionDataResult => {
  const [collectionData, setCollectionData] = useState<UserCardData | null>(null);
  const [loading, setLoading] = useState(enabled);
  const [error, setError] = useState<Error | null>(null);
  const apolloClient = useApolloClient();

  const fetchCollectionData = async () => {
    if (!enabled || !cardId || !userId) {
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const manager = getCollectionManager(apolloClient);
      const data = await manager.requestCollectionData(cardId, userId);

      setCollectionData(data);
    } catch (err) {
      setError(err as Error);
      setCollectionData(null);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCollectionData();
  }, [cardId, userId, enabled]);

  const refetch = async () => {
    // Invalidate cache and refetch
    const manager = getCollectionManager(apolloClient);
    manager.invalidateCard(cardId, userId);
    await fetchCollectionData();
  };

  return {
    collectionData,
    loading,
    error,
    refetch
  };
};

interface UseSetCollectionDataOptions {
  setId: string;
  userId: string;
  enabled?: boolean;
}

interface UseSetCollectionDataResult {
  collectionMap: Map<string, UserCardData>;
  loading: boolean;
  error: Error | null;
  refetch: () => Promise<void>;
  getCardData: (cardId: string) => UserCardData | null;
}

/**
 * Hook for requesting collection data for an entire set
 * More efficient than individual card requests when you need many cards from the same set
 */
export const useSetCollectionData = ({
  setId,
  userId,
  enabled = true
}: UseSetCollectionDataOptions): UseSetCollectionDataResult => {
  const [collectionMap, setCollectionMap] = useState<Map<string, UserCardData>>(new Map());
  const [loading, setLoading] = useState(enabled);
  const [error, setError] = useState<Error | null>(null);
  const apolloClient = useApolloClient();

  const fetchSetCollectionData = async () => {
    if (!enabled || !setId || !userId) {
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const manager = getCollectionManager(apolloClient);
      const data = await manager.requestSetCollectionData(setId, userId);

      setCollectionMap(data);
    } catch (err) {
      setError(err as Error);
      setCollectionMap(new Map());
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSetCollectionData();
  }, [setId, userId, enabled]);

  const refetch = async () => {
    await fetchSetCollectionData();
  };

  const getCardData = (cardId: string): UserCardData | null => {
    return collectionMap.get(cardId) || null;
  };

  return {
    collectionMap,
    loading,
    error,
    refetch,
    getCardData
  };
};

interface UseBulkCollectionDataOptions {
  cardIds: string[];
  userId: string;
  enabled?: boolean;
}

interface UseBulkCollectionDataResult {
  collectionMap: Map<string, UserCardData>;
  loading: boolean;
  error: Error | null;
  refetch: () => Promise<void>;
  getCardData: (cardId: string) => UserCardData | null;
  loadingCards: Set<string>;
}

/**
 * Hook for requesting collection data for multiple specific cards
 * Useful for pages with mixed cards from different sets (like Artist pages)
 */
export const useBulkCollectionData = ({
  cardIds,
  userId,
  enabled = true
}: UseBulkCollectionDataOptions): UseBulkCollectionDataResult => {
  const [collectionMap, setCollectionMap] = useState<Map<string, UserCardData>>(new Map());
  const [loadingCards, setLoadingCards] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const apolloClient = useApolloClient();


  const fetchBulkCollectionData = async () => {
    if (!enabled || !cardIds.length || !userId) {
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setLoadingCards(new Set(cardIds));
      setError(null);

      const manager = getCollectionManager(apolloClient);
      const newCollectionMap = new Map<string, UserCardData>();

      // Request all cards (will be batched automatically by manager)
      const promises = cardIds.map(async (cardId) => {
        try {
          const data = await manager.requestCollectionData(cardId, userId);
          if (data) {
            newCollectionMap.set(cardId, data);
          }

          // Remove from loading set
          setLoadingCards(prev => {
            const next = new Set(prev);
            next.delete(cardId);
            return next;
          });
        } catch (err) {
          console.warn(`Failed to fetch collection data for card ${cardId}:`, err);
          setLoadingCards(prev => {
            const next = new Set(prev);
            next.delete(cardId);
            return next;
          });
        }
      });

      await Promise.all(promises);
      setCollectionMap(newCollectionMap);

      // Force re-render by updating collection map with all cached data
      const updatedMap = new Map<string, UserCardData>();
      cardIds.forEach(cardId => {
        const cachedData = manager.getCachedCollectionData(cardId, userId);
        if (cachedData) {
          updatedMap.set(cardId, cachedData);
        }
      });
      setCollectionMap(updatedMap);
    } catch (err) {
      setError(err as Error);
    } finally {
      setLoading(false);
      setLoadingCards(new Set());
    }
  };

  useEffect(() => {
    fetchBulkCollectionData();
  }, [JSON.stringify(cardIds), userId, enabled]);

  const refetch = async () => {
    // Invalidate cache for all cards and refetch
    const manager = getCollectionManager(apolloClient);
    cardIds.forEach(cardId => manager.invalidateCard(cardId, userId));
    await fetchBulkCollectionData();
  };

  const getCardData = (cardId: string): UserCardData | null => {
    // Get data directly from CollectionManager cache for real-time updates
    const manager = getCollectionManager(apolloClient);
    return manager.getCachedCollectionData(cardId, userId) || collectionMap.get(cardId) || null;
  };

  return {
    collectionMap,
    loading,
    error,
    refetch,
    getCardData,
    loadingCards
  };
};