import { useState, useEffect, useMemo, useCallback } from 'react';
import { useQuery } from '@apollo/client/react';
import { useCardQueries } from './useCardQueries';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { useCollectorParam } from './useCollectorParam';
import { useCollectionUpdates } from './useCollectionUpdates';
import { useQueryStates } from '../components/molecules/shared/QueryStateContainer';
import { parseCollectorNumber } from '../config/cardSortOptions';
import type { Card } from '../types/card';
import type { MtgSet } from '../types/set';

interface SetsResponse {
  setsByCode: {
    __typename: string;
    data?: MtgSet[];
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

export type BinderSortBy = 'collector' | 'alphabetical';

const CARDS_PER_PAGE = 9;
const EMPTY_CARDS_ARRAY: Card[] = [];

/**
 * Custom hook to manage Binder Page data, filtering, and navigation
 * Filters cards to only those in collection groups the user is actively collecting
 */
export const useBinderPageData = (setCode: string | undefined) => {
  const { hasCollector, collectorId } = useCollectorParam();

  // Pagination and sort state
  const [currentPage, setCurrentPage] = useState(1);
  const [sortBy, setSortBy] = useState<BinderSortBy>('collector');

  // Card data fetching
  const { fetchSetCards } = useCardQueries();
  const [cardsLoading, setCardsLoading] = useState(false);
  const [cardsError, setCardsError] = useState<Error | null>(null);
  const [cards, setCards] = useState<Card[]>(EMPTY_CARDS_ARRAY);

  // Listen for collection updates
  useCollectionUpdates(cards, setCards);

  // Load cards effect
  useEffect(() => {
    if (!setCode) return;

    const loadCards = async () => {
      setCardsLoading(true);
      setCardsError(null);

      try {
        const fetchedCards = await fetchSetCards(setCode);
        setCards(fetchedCards);
      } catch (error) {
        setCardsError(error as Error);
        setCards(EMPTY_CARDS_ARRAY);
      } finally {
        setCardsLoading(false);
      }
    };

    loadCards();
  }, [setCode, collectorId, fetchSetCards]);

  // Fetch set metadata with collection data
  const { loading: setLoading, error: setError, data: setData } = useQuery<SetsResponse>(
    GET_SET_BY_CODE_WITH_GROUPINGS,
    {
      variables: {
        codes: {
          setCodes: [setCode],
          ...(hasCollector && collectorId ? { userId: collectorId } : {})
        }
      },
      skip: !setCode
    }
  );

  const setInfo = setData?.setsByCode?.data?.[0];

  // Get the group IDs that the user is actively collecting
  const collectingGroupIds = useMemo(() => {
    if (!setInfo?.userCollection?.collecting) return new Set<string>();

    return new Set(
      setInfo.userCollection.collecting
        .filter(g => g.collecting)
        .map(g => g.setGroupId)
    );
  }, [setInfo?.userCollection?.collecting]);

  // Build set of collected card IDs across all finishes
  const collectedCardIds = useMemo(() => {
    const ids = new Set<string>();

    if (!setInfo?.userCollection?.groups) return ids;

    for (const group of setInfo.userCollection.groups) {
      // Add all card IDs from each finish type
      if (group.group.nonFoil?.cards) {
        group.group.nonFoil.cards.forEach(id => ids.add(id));
      }
      if (group.group.foil?.cards) {
        group.group.foil.cards.forEach(id => ids.add(id));
      }
      if (group.group.etched?.cards) {
        group.group.etched.cards.forEach(id => ids.add(id));
      }
    }

    return ids;
  }, [setInfo?.userCollection?.groups]);

  // Filter cards to collecting groups if collector with groups, otherwise show all
  const binderCards = useMemo(() => {
    let filtered: Card[];

    if (hasCollector && collectingGroupIds.size > 0) {
      // Filter cards that belong to a collecting group
      filtered = cards.filter(card => {
        const cardGroupId = card.setGroupId || 'default-cards';
        return collectingGroupIds.has(cardGroupId);
      });
    } else {
      // No collector OR collector with no groups selected - show all cards
      filtered = cards;
    }

    // Sort cards
    const sorted = [...filtered].sort((a, b) => {
      if (sortBy === 'alphabetical') {
        return a.name.localeCompare(b.name);
      }
      // Default: collector number
      return parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || '');
    });

    return sorted;
  }, [cards, collectingGroupIds, sortBy, hasCollector]);

  // Pagination calculations
  const totalPages = useMemo(() => {
    return Math.ceil(binderCards.length / CARDS_PER_PAGE) || 1;
  }, [binderCards.length]);

  // Reset to page 1 when cards change significantly
  useEffect(() => {
    if (currentPage > totalPages) {
      setCurrentPage(Math.max(1, totalPages));
    }
  }, [totalPages, currentPage]);

  // Get cards for a specific page (with null padding for empty slots)
  const getPageCards = useCallback((pageNum: number): (Card | null)[] => {
    const startIndex = (pageNum - 1) * CARDS_PER_PAGE;
    const pageCards = binderCards.slice(startIndex, startIndex + CARDS_PER_PAGE);

    // Pad with null for empty slots
    const result: (Card | null)[] = [...pageCards];
    while (result.length < CARDS_PER_PAGE) {
      result.push(null);
    }

    return result;
  }, [binderCards]);

  // Navigation functions
  const goToPage = useCallback((page: number) => {
    const validPage = Math.max(1, Math.min(page, totalPages));
    setCurrentPage(validPage);
  }, [totalPages]);

  const nextPage = useCallback(() => {
    if (currentPage < totalPages) {
      setCurrentPage(prev => prev + 1);
    }
  }, [currentPage, totalPages]);

  const prevPage = useCallback(() => {
    if (currentPage > 1) {
      setCurrentPage(prev => prev - 1);
    }
  }, [currentPage]);

  // Book mode navigation (skip 2 pages at a time)
  const nextSpread = useCallback(() => {
    const increment = currentPage === 1 ? 1 : 2;
    const nextPage = currentPage + increment;
    if (nextPage <= totalPages) {
      setCurrentPage(nextPage);
    } else if (currentPage < totalPages) {
      setCurrentPage(totalPages);
    }
  }, [currentPage, totalPages]);

  const prevSpread = useCallback(() => {
    if (currentPage <= 2) {
      setCurrentPage(1);
    } else {
      // Go back 2 pages, ensuring we land on an even page for spread view
      const newPage = currentPage - 2;
      setCurrentPage(Math.max(2, newPage));
    }
  }, [currentPage]);

  // Combined query states
  const { isLoading, firstError } = useQueryStates([
    { loading: cardsLoading, error: cardsError },
    { loading: setLoading, error: setError }
  ]);

  // Computed values
  const setName = setInfo?.name || cards[0]?.setName || setCode?.toUpperCase() || '';
  const hasCollectingGroups = collectingGroupIds.size > 0;

  return {
    // Set data
    setInfo,
    setName,

    // Binder cards
    binderCards,
    collectedCardIds,
    getPageCards,

    // Pagination
    currentPage,
    totalPages,
    cardsPerPage: CARDS_PER_PAGE,

    // Navigation
    goToPage,
    nextPage,
    prevPage,
    nextSpread,
    prevSpread,

    // Sort
    sortBy,
    setSortBy,

    // Loading states
    isLoading,
    firstError,

    // Collector state
    hasCollector,
    collectorId,
    hasCollectingGroups
  };
};
