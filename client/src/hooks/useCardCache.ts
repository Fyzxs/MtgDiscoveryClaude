import { useApolloClient } from '@apollo/client/react';
import { useCallback } from 'react';
import { useCollectorParam } from './useCollectorParam';
import {
  GET_CARDS_BY_SET_CODE,
  GET_CARDS_BY_NAME,
  GET_CARDS_BY_IDS,
  GET_CARDS_BY_ARTIST
} from '../graphql/queries/cards';
import type { Card } from '../types/card';

interface CardQueryResponse {
  cardsById?: {
    data?: Card[];
  };
  cardsBySetCode?: {
    data?: Card[];
  };
  cardsByName?: {
    data?: Card[];
  };
  cardsByArtistName?: {
    data?: Card[];
  };
}

/**
 * Collection card data interface for updates
 */
export interface CollectorCardData {
  cardId: string;
  finish: 'nonfoil' | 'foil' | 'etched';
  special: 'none' | 'artist_proof' | 'signed' | 'altered';
  count: number;
}

/**
 * Hook that provides card fetching operations using Apollo Client cache
 * All queries use cache-first policy to minimize network requests
 */
export function useCardCache() {
  const apolloClient = useApolloClient();
  const { hasCollector, collectorId } = useCollectorParam();

  /**
   * Fetch a single card by ID through Apollo cache
   */
  const fetchCard = useCallback(async (cardId: string): Promise<Card> => {
    const response = await apolloClient.query({
      query: GET_CARDS_BY_IDS,
      variables: {
        ids: {
          cardIds: [cardId],
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      fetchPolicy: 'cache-first'
    });

    const data = (response.data as CardQueryResponse)?.cardsById?.data;
    if (data?.[0]) {
      return data[0];
    }
    throw new Error(`Card ${cardId} not found`);
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch multiple cards by IDs through Apollo cache
   */
  const fetchCards = useCallback(async (cardIds: string[]): Promise<Card[]> => {
    const response = await apolloClient.query({
      query: GET_CARDS_BY_IDS,
      variables: {
        ids: {
          cardIds,
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      fetchPolicy: 'cache-first'
    });

    return (response.data as CardQueryResponse)?.cardsById?.data || [];
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch all cards in a set through Apollo cache
   */
  const fetchSetCards = useCallback(async (setCode: string): Promise<Card[]> => {
    const response = await apolloClient.query({
      query: GET_CARDS_BY_SET_CODE,
      variables: {
        setCode: {
          setCode,
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      fetchPolicy: 'cache-first'
    });

    const data = (response.data as CardQueryResponse)?.cardsBySetCode?.data;
    if (data) {
      return data;
    }
    return [];
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch cards by name through Apollo cache
   */
  const fetchCardsByName = useCallback(async (cardName: string): Promise<Card[]> => {
    const response = await apolloClient.query({
      query: GET_CARDS_BY_NAME,
      variables: {
        cardName: {
          cardName,
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      fetchPolicy: 'cache-first'
    });

    return (response.data as CardQueryResponse)?.cardsByName?.data || [];
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Fetch cards by artist through Apollo cache
   */
  const fetchCardsByArtist = useCallback(async (artistName: string): Promise<Card[]> => {
    const response = await apolloClient.query({
      query: GET_CARDS_BY_ARTIST,
      variables: {
        artistName: {
          artistName,
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      fetchPolicy: 'cache-first'
    });

    return (response.data as CardQueryResponse)?.cardsByArtistName?.data || [];
  }, [apolloClient, hasCollector, collectorId]);

  /**
   * Update collector card data (for collection management)
   * TODO: Implement when UPDATE_COLLECTION mutation is available
   */
  const updateCollectorCard = useCallback(async (
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    _collectorData: CollectorCardData
  ): Promise<Card> => {
    if (!hasCollector || !collectorId) {
      throw new Error('No collector context available');
    }

    // TODO: Call UPDATE_COLLECTION mutation when available
    throw new Error('UPDATE_COLLECTION mutation not yet implemented');
  }, [hasCollector, collectorId]);

  /**
   * Clear the Apollo cache (useful for logout, etc.)
   */
  const clearCache = useCallback(() => {
    apolloClient.cache.reset();
  }, [apolloClient]);

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