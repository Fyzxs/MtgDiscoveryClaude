import { useMemo } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useQuery } from '@apollo/client/react';
import { useUser } from '../contexts/UserContext';
import { GET_ALL_SETS } from '../graphql/queries/sets';
import { GET_USER_WISHLIST } from '../graphql/queries/wishlist';

/**
 * User stats displayed on the authenticated homepage hero
 */
export interface UserHomepageStats {
  cardsTracked: number | null;
  setsInProgress: number | null;
  wishlistCount: number | null;
}

/**
 * Recent set data for the collection preview
 */
export interface RecentSetData {
  code: string;
  name: string;
  iconSvgUri: string;
  completionPercent: number;
}

/**
 * Wishlist card data for the collection preview
 */
export interface WishlistCardData {
  id: string;
  name: string;
  imageUri: string | undefined;
}

interface UseHomePageDataResult {
  /** User stats for authenticated hero section */
  stats: UserHomepageStats | null;
  /** Whether stats failed to load */
  statsError: boolean;
  /** Recent sets user is collecting */
  recentSets: RecentSetData[] | undefined;
  /** Preview of wishlist cards */
  wishlistCards: WishlistCardData[] | undefined;
  /** Whether collection data is loading */
  isLoading: boolean;
  /** Auth0 authenticated state */
  isAuthenticated: boolean;
  /** User's nickname from Auth0 */
  userName: string | undefined;
}

/**
 * Calculates user statistics from sets data
 */
function calculateUserStats(
  isAuthenticated: boolean,
  setsData: unknown,
  wishlistData: unknown
): UserHomepageStats | null {
  if (isAuthenticated === false) {
    return null;
  }

  let cardsTracked: number | null = null;
  let setsInProgress: number | null = null;

  const setsResponse = setsData as { allSets?: { __typename?: string; data?: Array<{
    userCollection?: { totalCards?: number; collecting?: unknown[] } | null;
  }> } } | undefined;

  if (setsResponse?.allSets?.__typename === 'SetsSuccessResponse') {
    const sets = setsResponse.allSets.data ?? [];
    cardsTracked = 0;
    setsInProgress = 0;

    for (const set of sets) {
      const collection = set.userCollection;
      if (collection !== undefined && collection !== null) {
        const totalCards = collection.totalCards ?? 0;
        if (totalCards > 0) {
          cardsTracked += totalCards;
        }
        const collecting = collection.collecting;
        if (collecting !== undefined && collecting !== null && collecting.length > 0) {
          setsInProgress += 1;
        }
      }
    }
  }

  let wishlistCount: number | null = null;
  const wishlistResponse = wishlistData as { userWishlist?: { __typename?: string; data?: unknown[] } } | undefined;
  if (wishlistResponse?.userWishlist?.__typename === 'CardsSuccessResponse') {
    wishlistCount = wishlistResponse.userWishlist.data?.length ?? 0;
  }

  return { cardsTracked, setsInProgress, wishlistCount };
}

/**
 * Extracts recent sets user is collecting
 */
function extractRecentSets(
  isAuthenticated: boolean,
  setsData: unknown
): RecentSetData[] | undefined {
  if (isAuthenticated === false) {
    return undefined;
  }

  const setsResponse = setsData as { allSets?: { __typename?: string; data?: Array<{
    code: string;
    name: string;
    iconSvgUri: string;
    cardCount: number;
    releasedAt: string;
    userCollection?: { totalCards?: number; collecting?: unknown[] } | null;
  }> } } | undefined;

  if (setsResponse?.allSets?.__typename !== 'SetsSuccessResponse') {
    return undefined;
  }

  const sets = setsResponse.allSets.data ?? [];
  return sets
    .filter((set) => {
      const collecting = set.userCollection?.collecting;
      return collecting !== undefined && collecting !== null && Array.isArray(collecting) && collecting.length > 0;
    })
    .sort((a, b) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime())
    .slice(0, 3)
    .map((set) => {
      const totalCollected = set.userCollection?.totalCards ?? 0;
      const completionPercent = set.cardCount > 0
        ? Math.round((totalCollected / set.cardCount) * 100)
        : 0;
      return {
        code: set.code,
        name: set.name,
        iconSvgUri: set.iconSvgUri,
        completionPercent,
      };
    });
}

/**
 * Extracts wishlist cards for preview
 */
function extractWishlistCards(
  isAuthenticated: boolean,
  wishlistData: unknown
): WishlistCardData[] | undefined {
  if (isAuthenticated === false) {
    return undefined;
  }

  const wishlistResponse = wishlistData as { userWishlist?: { __typename?: string; data?: Array<{
    id: string;
    name: string;
    imageUris?: { small?: string };
    cardFaces?: Array<{ imageUris?: { small?: string } }>;
  }> } } | undefined;

  if (wishlistResponse?.userWishlist?.__typename !== 'CardsSuccessResponse') {
    return undefined;
  }

  const cards = wishlistResponse.userWishlist.data ?? [];
  return cards.slice(0, 6).map((card) => ({
    id: card.id,
    name: card.name,
    imageUri: card.imageUris?.small ?? card.cardFaces?.[0]?.imageUris?.small,
  }));
}

/**
 * Hook that provides all data needed for the HomePage component.
 * Handles authentication state, user stats, recent sets, and wishlist preview.
 *
 * @returns Object containing all homepage data and loading states
 *
 * @example
 * const { stats, recentSets, wishlistCards, isLoading, isAuthenticated, userName } = useHomePageData();
 */
export function useHomePageData(): UseHomePageDataResult {
  const { isAuthenticated } = useAuth0();
  const { auth0User, userProfile } = useUser();

  const { data: setsData, error: setsError } = useQuery(GET_ALL_SETS, {
    variables: { args: isAuthenticated ? { userId: userProfile?.id } : null },
  });

  const { data: wishlistData, error: wishlistError } = useQuery(GET_USER_WISHLIST, {
    variables: { userId: userProfile?.id ?? '' },
    skip: isAuthenticated === false || userProfile?.id === undefined,
  });

  const statsError = isAuthenticated && (setsError !== undefined || wishlistError !== undefined);

  const stats = useMemo(
    () => calculateUserStats(isAuthenticated, setsData, wishlistData),
    [isAuthenticated, setsData, wishlistData]
  );

  const recentSets = useMemo(
    () => extractRecentSets(isAuthenticated, setsData),
    [isAuthenticated, setsData]
  );

  const wishlistCards = useMemo(
    () => extractWishlistCards(isAuthenticated, wishlistData),
    [isAuthenticated, wishlistData]
  );

  const isLoading = isAuthenticated && setsData === undefined && setsError === undefined;

  return {
    stats,
    statsError,
    recentSets,
    wishlistCards,
    isLoading,
    isAuthenticated,
    userName: auth0User?.nickname,
  };
}
