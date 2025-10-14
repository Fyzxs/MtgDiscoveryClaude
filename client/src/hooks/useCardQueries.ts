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
 * Hook that provides card fetching operations using Apollo Client
 * All queries use cache-first policy to minimize network requests
 */
export function useCardQueries() {
  const apolloClient = useApolloClient();
  const { hasCollector, collectorId } = useCollectorParam();

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

  return {
    fetchCards,
    fetchSetCards,
    fetchCardsByName,
    fetchCardsByArtist,
    hasCollectorContext: hasCollector,
    collectorId
  };
}
