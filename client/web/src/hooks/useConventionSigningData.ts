import { useState, useCallback, useMemo, useEffect } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { useUrlState } from './useUrlState';
import { useCollectorParam } from './useCollectorParam';
import { USER_CARDS_FOR_SIGNING } from '../graphql/queries/conventionSigning';

interface ArtistSearchResult {
  artistId: string;
  name: string;
}

interface CollectedDetail {
  finish: string;
  special: string;
  count: number;
}

interface SigningCard {
  cardId: string;
  cardName: string;
  imageUri: string;
  unsignedCopies: number;
  isPartiallySigned: boolean;
  collectedDetails: CollectedDetail[];
}

interface SigningArtistGroup {
  artistId: string;
  artistName: string;
  unsignedCount: number;
  partiallySignedCount: number;
  cards: SigningCard[];
}

interface SigningSetGroup {
  setId: string;
  setCode: string;
  setName: string;
  artistCount: number;
  unsignedCardCount: number;
  artists: SigningArtistGroup[];
}

interface SigningResult {
  sets: SigningSetGroup[];
}

interface SigningResponse {
  userCardsForSigning: {
    __typename: string;
    data?: SigningResult;
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

export interface UseConventionSigningDataReturn {
  // Selected artists
  selectedArtists: ArtistSearchResult[];
  addArtist: (artist: ArtistSearchResult) => void;
  removeArtist: (artistId: string) => void;
  clearArtists: () => void;

  // Signing data
  signingData: SigningResult | null;
  loading: boolean;
  error: Error | null;
  hasFetched: boolean;

  // Fetch trigger
  fetchSigningData: () => void;

  // State
  hasCollector: boolean;
  collectorId: string | null;
}

/**
 * Custom hook to manage convention signing planner data
 * Uses URL state for artist persistence (shareable links)
 */
export const useConventionSigningData = (): UseConventionSigningDataReturn => {
  const { hasCollector, collectorId: userId } = useCollectorParam();

  // URL state configuration for artists
  const urlStateConfig = useMemo(() => ({
    artists: {
      default: '',
      serialize: (value: unknown) => {
        const artists = value as ArtistSearchResult[];
        return artists.length > 0
          ? artists.map(a => `${a.artistId}:${encodeURIComponent(a.name)}`).join(',')
          : null;
      },
      deserialize: (value: string): ArtistSearchResult[] => {
        if (!value) return [];
        return value.split(',').filter(Boolean).map(entry => {
          const [artistId, encodedName] = entry.split(':');
          return {
            artistId,
            name: decodeURIComponent(encodedName || '')
          };
        });
      }
    }
  }), []);

  const { getInitialValues } = useUrlState({}, urlStateConfig);

  // Initialize selected artists from URL
  const [selectedArtists, setSelectedArtists] = useState<ArtistSearchResult[]>(() => {
    const initial = getInitialValues();
    return (initial.artists as ArtistSearchResult[]) || [];
  });

  // Error state
  const [error, setError] = useState<Error | null>(null);

  // Track if we've fetched (to distinguish "not fetched yet" from "no results")
  const [hasFetched, setHasFetched] = useState(false);

  // GraphQL lazy query
  const [fetchQuery, { data, loading }] = useLazyQuery<SigningResponse>(USER_CARDS_FOR_SIGNING, {
    fetchPolicy: 'network-only',
    onError: (err) => setError(err)
  });

  // Add artist
  const addArtist = useCallback((artist: ArtistSearchResult) => {
    setSelectedArtists(prev => {
      // Don't add duplicates
      if (prev.some(a => a.artistId === artist.artistId)) {
        return prev;
      }
      return [...prev, artist];
    });
  }, []);

  // Remove artist
  const removeArtist = useCallback((artistId: string) => {
    setSelectedArtists(prev => prev.filter(a => a.artistId !== artistId));
  }, []);

  // Clear all artists
  const clearArtists = useCallback(() => {
    setSelectedArtists([]);
    setHasFetched(false);
  }, []);

  // Fetch signing data - only called when user clicks button
  const fetchSigningData = useCallback(() => {
    if (!userId || selectedArtists.length === 0) {
      return;
    }

    setError(null);
    setHasFetched(true);
    fetchQuery({
      variables: {
        forSigningArgs: {
          userId,
          artistIds: selectedArtists.map(a => a.artistId)
        }
      }
    });
  }, [userId, selectedArtists, fetchQuery]);

  // Sync URL state
  useUrlState(
    { artists: selectedArtists },
    urlStateConfig,
    { debounceMs: 0 }
  );

  // Extract signing result from response
  const signingData = useMemo(() => {
    if (!data?.userCardsForSigning) return null;

    const response = data.userCardsForSigning;
    if (response.__typename === 'SigningResultSuccessResponse' && response.data) {
      return response.data;
    }

    // Handle failure
    if (response.__typename === 'FailureResponse' && response.status) {
      setError(new Error(response.status.message));
    }

    return null;
  }, [data]);

  return {
    // Selected artists
    selectedArtists,
    addArtist,
    removeArtist,
    clearArtists,

    // Signing data
    signingData,
    loading,
    error,
    hasFetched,

    // Fetch trigger
    fetchSigningData,

    // State
    hasCollector,
    collectorId: userId ?? null
  };
};
