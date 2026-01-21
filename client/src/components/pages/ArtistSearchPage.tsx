import React, { useState, useCallback, startTransition } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { Heading, BodyText } from '../molecules/text';
import { Section } from '../molecules/layouts';
import { LoadingIndicator } from '../molecules/feedback';
import { SearchTemplate } from '../templates/pages/SearchTemplate';
import { DebouncedSearchInput } from '../molecules/shared/DebouncedSearchInput';
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
    <Section asSection={false} sx={{ width: '100%', maxWidth: 600 }}>
      <Heading variant="h4" gutterBottom sx={{ textAlign: 'center', mb: 3 }}>
        Artist Search
      </Heading>
      <DebouncedSearchInput
        value={searchTerm}
        onChange={handleSearchChange}
        placeholder="Enter artist name..."
        debounceMs={500}
        fullWidth
        disabled={loading}
      />
    </Section>
  );

  // Results summary slot
  const resultsSummary = hasSearched && (
    <BodyText variant="body2" color="text.secondary">
      {searchTerm.length < 3 ? (
        <CharacterCountMessage remainingChars={3 - searchTerm.length} />
      ) : hasResults ? (
        `Search results for "${searchTerm}"`
      ) : null}
    </BodyText>
  );

  // Results content slot
  const resultsContent = (() => {
    if (showInitialState) {
      return (
        <BodyText variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
          Search by Artist Name - Enter at least 3 characters to search
        </BodyText>
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
    <LoadingIndicator withContainer={false} centered={false} size={40} />
  );

  // Empty state slot
  const emptyState = (
    <BodyText variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
      No artists found matching "{searchTerm}"
    </BodyText>
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
