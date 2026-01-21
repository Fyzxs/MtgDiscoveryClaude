import { useApolloClient } from '@apollo/client/react';
import { useCallback } from 'react';
import { useCollectorParam } from './useCollectorParam';
import { cardCacheManager } from '../services/CardCacheManager';
import {
  GET_CARDS_BY_SET_CODE,
  GET_CARDS_BY_NAME,
  GET_CARDS_BY_IDS,
  GET_CARDS_BY_ARTIST
} from '../graphql/queries/cards';
import type { Card } from '../types/card';
import type { CollectorCardData } from '../services/CardCacheManager';

/**
 * Hook that provides card cache operations with GraphQL integration
 * Encapsulates all Apollo Client calls so consumers don't need to handle them
 */
export function useCardCache() {
  const apolloClient = useApolloClient();
  const { hasCollector, collectorId } = useCollectorParam();

  /**
   * Fetch a single card by ID through the cache
   */
  const fetchCard = useCallback(async (cardId: string): Promise<Card> => {
    return cardCacheManager.fetchCard(
      cardId,
      async () => {
        const response = await apolloClient.query({
          query: GET_CARDS_BY_IDS,
          variables: {
            ids: {
              ids: [cardId],
              ...(hasCollector && collectorId ? { userId: collectorId } : {})
            }
          }
        });

        const data = (response.data as any)?.cardsById?.data;
        if (data?.[0]) {
          return data[0];
        }
        throw new Error(`Card ${cardId} not found`);
      },
      hasCollector ? collectorId : null
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch multiple cards by IDs through the cache
   */
  const fetchCards = useCallback(async (cardIds: string[]): Promise<Card[]> => {
    return cardCacheManager.fetchCards(
      cardIds,
      async (missingIds) => {
        const response = await apolloClient.query({
          query: GET_CARDS_BY_IDS,
          variables: {
            ids: {
              ids: missingIds,
              ...(hasCollector && collectorId ? { userId: collectorId } : {})
            }
          }
        });

        return (response.data as any)?.cardsById?.data || [];
      },
      hasCollector ? collectorId : null
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch all cards in a set through the cache
   */
  const fetchSetCards = useCallback(async (setCode: string): Promise<Card[]> => {
    return cardCacheManager.fetchSetCards(
      setCode,
      async () => {
        const response = await apolloClient.query({
          query: GET_CARDS_BY_SET_CODE,
          variables: {
            setCode: {
              setCode,
              ...(hasCollector && collectorId ? { userId: collectorId } : {})
            }
          }
        });

        const data = (response.data as any)?.cardsBySetCode?.data;
        if (data) {
          return data;
        }
        return [];
      },
      hasCollector ? collectorId : null
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch cards by name through the cache
   */
  const fetchCardsByName = useCallback(async (cardName: string): Promise<Card[]> => {
    return cardCacheManager.fetchCardsByName(
      cardName,
      async () => {
        const response = await apolloClient.query({
          query: GET_CARDS_BY_NAME,
          variables: {
            cardName: {
              cardName,
              ...(hasCollector && collectorId ? { userId: collectorId } : {})
            }
          }
        });

        return (response.data as any)?.cardsByName?.data || [];
      },
      hasCollector ? collectorId : null
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch cards by artist through the cache
   */
  const fetchCardsByArtist = useCallback(async (artistName: string): Promise<Card[]> => {
    return cardCacheManager.fetchCardsByArtist(
      artistName,
      async () => {
        const response = await apolloClient.query({
          query: GET_CARDS_BY_ARTIST,
          variables: {
            artistName: {
              artistName,
              ...(hasCollector && collectorId ? { userId: collectorId } : {})
            }
          }
        });

        return (response.data as any)?.cardsByArtistName?.data || [];
      },
      hasCollector ? collectorId : null
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Update collector card data (for collection management)
   * TODO: Implement when UPDATE_COLLECTION mutation is available
   */
  const updateCollectorCard = useCallback(async (
    collectorData: CollectorCardData
  ): Promise<Card> => {
    if (!hasCollector || !collectorId) {
      throw new Error('No collector context available');
    }

    return cardCacheManager.updateCollectorCardData(
      collectorData,
      collectorId,
      async () => {
        // TODO: Call UPDATE_COLLECTION mutation when available
        // const response = await apolloClient.mutate({
        //   mutation: UPDATE_COLLECTION,
        //   variables: {
        //     cardId: collectorData.cardId,
        //     collectorId,
        //     finish: collectorData.finish,
        //     special: collectorData.special,
        //     count: collectorData.count
        //   }
        // });
        // return response.data?.updateCollection?.card;

        throw new Error('UPDATE_COLLECTION mutation not yet implemented');
      }
    );
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Clear the entire cache (useful for logout, etc.)
   */
  const clearCache = useCallback(() => {
    cardCacheManager.clear();
  }, []);

  return {
    fetchCard,
    fetchCards,
    fetchSetCards,
    fetchCardsByName,
    fetchCardsByArtist,
    updateCollectorCard,
    clearCache,
    hasCollectorContext: hasCollector,
    collectorId
  };
}

// Re-export for convenience
export type { CollectorCardData } from '../services/CardCacheManager';