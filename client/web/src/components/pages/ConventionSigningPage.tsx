import React from 'react';
import { Typography, Box, Alert, Button } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { BrowseTemplate } from '../templates/pages/BrowseTemplate';
import { ConventionArtistSelector } from '../organisms/Convention/ConventionArtistSelector';
import { ConventionSigningResults } from '../organisms/Convention/ConventionSigningResults';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { useConventionSigningData } from '../../hooks/useConventionSigningData';
import { LinkParamsProvider } from '../../contexts/LinkParamsContext';

/**
 * ConventionSigningPage - Plan which cards to get signed at conventions
 *
 * Allows users to:
 * 1. Search and add artists attending a convention
 * 2. View their cards grouped by Set -> Artist
 * 3. See unsigned copies and partially signed cards
 */
export const ConventionSigningPage: React.FC = () => {
  const {
    selectedArtists,
    addArtist,
    removeArtist,
    clearArtists,
    signingData,
    loading,
    error,
    hasFetched,
    fetchSigningData,
    hasCollector,
    collectorId
  } = useConventionSigningData();

  // Auth guard - require collector mode
  if (!hasCollector) {
    return (
      <Box sx={{ p: 4, textAlign: 'center' }}>
        <Alert severity="info" sx={{ maxWidth: 600, mx: 'auto' }}>
          <Typography variant="h6" gutterBottom>
            Sign In Required
          </Typography>
          <Typography>
            Please sign in and enable Collector Mode to access the Convention Signing Planner.
            This feature uses your card collection data to show which cards you can get signed.
          </Typography>
        </Alert>
      </Box>
    );
  }

  return (
    <LinkParamsProvider additionalParams={{ formats: 'paper' }}>
      <QueryStateContainer
        loading={false}
        error={null}
        containerProps={{ maxWidth: false }}
      >
        <BrowseTemplate
        header={
          <Box sx={{ textAlign: 'center' }}>
            <Typography variant="h3" component="h1" gutterBottom>
              Convention Signing Planner
            </Typography>
            <Typography variant="body1" color="text.secondary" sx={{ maxWidth: 600, mx: 'auto' }}>
              Add artists attending a convention to see which cards from your collection you can get signed.
              Results are grouped by set showing unsigned copies.
            </Typography>
          </Box>
        }
        filters={
          <ConventionArtistSelector
            selectedArtists={selectedArtists}
            onAddArtist={addArtist}
            onRemoveArtist={removeArtist}
            onClearAll={clearArtists}
          />
        }
        summary={
          selectedArtists.length > 0 ? (
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 2, flexWrap: 'wrap' }}>
              <Typography variant="body2" color="text.secondary">
                {selectedArtists.length} artist{selectedArtists.length !== 1 ? 's' : ''} selected
                {signingData && signingData.sets.length > 0 && (
                  <> - {signingData.sets.reduce((sum, set) => sum + set.unsignedCardCount, 0)} unsigned cards across {signingData.sets.length} sets</>
                )}
              </Typography>
              <Button
                variant="contained"
                size="small"
                startIcon={<SearchIcon />}
                onClick={fetchSigningData}
                disabled={loading}
              >
                {hasFetched ? 'Refresh' : 'Find Cards to Sign'}
              </Button>
            </Box>
          ) : null
        }
        content={
          <Box>
            {error && (
              <Alert severity="error" sx={{ mb: 3 }}>
                {error.message}
              </Alert>
            )}

            {selectedArtists.length === 0 ? (
              <Box sx={{ textAlign: 'center', py: 8 }}>
                <Typography variant="h6" color="text.secondary">
                  No artists selected
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Use the search above to add artists attending your convention
                </Typography>
              </Box>
            ) : loading ? (
              <Box sx={{ textAlign: 'center', py: 8 }}>
                <Typography variant="body1" color="text.secondary">
                  Loading your cards...
                </Typography>
              </Box>
            ) : hasFetched && signingData ? (
              <ConventionSigningResults data={signingData} collectorId={collectorId} />
            ) : hasFetched ? (
              <Box sx={{ textAlign: 'center', py: 8 }}>
                <Typography variant="body1" color="text.secondary">
                  No cards found for the selected artists in your collection.
                </Typography>
              </Box>
            ) : (
              <Box sx={{ textAlign: 'center', py: 8 }}>
                <Typography variant="h6" color="text.secondary" gutterBottom>
                  Ready to find your cards
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Click "Find Cards to Sign" above to search your collection
                </Typography>
              </Box>
            )}
          </Box>
        }
        />
        <BackToTopFab />
      </QueryStateContainer>
    </LinkParamsProvider>
  );
};

export default ConventionSigningPage;
