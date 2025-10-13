import React, { useState, useCallback, startTransition } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { Typography, Box, CircularProgress } from '@mui/material';
import { SearchTemplate } from '../templates/pages/SearchTemplate';
import { DebouncedSearchInput } from '../atoms/shared/DebouncedSearchInput';
import { ArtistSearchResults } from '../organisms/ArtistSearchResults';
import { ARTIST_NAME_SEARCH } from '../../graphql/queries/artistSearch';
import { useCollectorNavigation } from '../../hooks/useCollectorNavigation';

interface ArtistNameResult {
  artistId: string;
  name: string;
}

interface ArtistNameSearchResponse {
  artistSearch: {
    __typename: string;
    data?: ArtistNameResult[];
    status?: {
      message: string;
      statusCode: string;
    };
  };
}

export const ArtistSearchPage: React.FC = React.memo(() => {
  const { navigateWithCollector } = useCollectorNavigation();
  const [searchTerm, setSearchTerm] = useState('');

  const [searchArtists, { loading, data }] = useLazyQuery<ArtistNameSearchResponse>(
    ARTIST_NAME_SEARCH,
    {
      fetchPolicy: 'cache-and-network'
    }
  );

  const handleArtistClick = useCallback((artistName: string) => {
    navigateWithCollector(`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`);
  }, [navigateWithCollector]);

  const handleSearchChange = useCallback((value: string) => {
    startTransition(() => {
      setSearchTerm(value);
      if (value.length >= 3) {
        searchArtists({
          variables: {
            searchTerm: {
              searchTerm: value
            }
          }
        });
      }
    });
  }, [searchArtists]);

  // Determine display states
  const hasSearched = searchTerm.length >= 3;
  const hasResults = hasSearched && data?.artistSearch?.data && data.artistSearch.data.length > 0;
  const isEmpty = hasSearched && !loading && data?.artistSearch?.data?.length === 0;
  const showInitialState = searchTerm.length === 0;

  // Search input slot
  const searchInput = (
    <Box sx={{ width: '100%', maxWidth: 600 }}>
      <Typography variant="h4" gutterBottom sx={{ textAlign: 'center', mb: 3 }}>
        Artist Search
      </Typography>
      <DebouncedSearchInput
        value={searchTerm}
        onChange={handleSearchChange}
        placeholder="Enter artist name..."
        debounceMs={500}
        fullWidth
        disabled={loading}
      />
    </Box>
  );

  // Results summary slot
  const resultsSummary = hasSearched && (
    <Typography variant="body2" color="text.secondary">
      {searchTerm.length < 3 ? (
        <CharacterCountMessage remainingChars={3 - searchTerm.length} />
      ) : hasResults ? (
        `Search results for "${searchTerm}"`
      ) : null}
    </Typography>
  );

  // Results content slot
  const resultsContent = (() => {
    if (showInitialState) {
      return (
        <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
          Search by Artist Name - Enter at least 3 characters to search
        </Typography>
      );
    }

    if (hasResults) {
      return (
        <ArtistSearchResults
          artists={data!.artistSearch.data!}
          onArtistClick={handleArtistClick}
          searchTerm={searchTerm}
        />
      );
    }

    return null;
  })();

  // Loading state slot
  const loadingState = (
    <CircularProgress size={40} />
  );

  // Empty state slot
  const emptyState = (
    <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
      No artists found matching "{searchTerm}"
    </Typography>
  );

  return (
    <SearchTemplate
      searchInput={searchInput}
      resultsSummary={resultsSummary}
      resultsContent={resultsContent}
      loadingState={loadingState}
      emptyState={emptyState}
      isLoading={loading}
      isEmpty={isEmpty}
      showInitialState={showInitialState}
      containerPadding={{ mt: 2, mb: 4, px: 3 }}
    />
  );
});

// Memoized component for character count message
const CharacterCountMessage = React.memo<{ remainingChars: number }>(({ remainingChars }) => (
  <>
    Minimum 3 characters required - Enter {remainingChars} more character{remainingChars === 1 ? '' : 's'}
  </>
));

CharacterCountMessage.displayName = 'CharacterCountMessage';export default ArtistSearchPage;
