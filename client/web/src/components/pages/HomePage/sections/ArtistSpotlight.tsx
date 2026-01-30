import React, { useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useQuery } from '@apollo/client/react';
import { Box, Typography, Grid, Button, SeparatedList, Skeleton } from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';
import { MtgCard } from '../../../organisms/Cards/MtgCard';
import { featuredArtists } from '../data/featuredArtists';
import { getDailyRotatedItem } from '../../../../utils/rotation';
import { GET_CARDS_BY_NAME } from '../../../../graphql/queries/cards';
import type { Card } from '../../../../types/card';

/**
 * Featured artist spotlight section with daily rotation.
 * Showcases a different classic MTG artist each day with their iconic card artwork.
 */
export const ArtistSpotlight: React.FC = () => {
  const navigate = useNavigate();
  const artist = getDailyRotatedItem(featuredArtists);

  const { data, loading } = useQuery(GET_CARDS_BY_NAME, {
    variables: {
      cardName: { cardName: artist?.featuredCardName ?? '' },
    },
    skip: artist === undefined,
  });

  const featuredCard = useMemo((): Card | undefined => {
    if (artist === undefined || data?.cardsByName?.__typename !== 'CardsSuccessResponse') {
      return undefined;
    }

    const cards = data.cardsByName.data ?? [];
    if (cards.length === 0) {
      return undefined;
    }

    // Find the first printing by this artist
    const matchedCard = cards.find(
      (card: Card) => card.artist?.toLowerCase() === artist.name.toLowerCase()
    );

    // Fall back to first card if no exact artist match (handles name variations)
    return matchedCard ?? cards[0];
  }, [artist, data]);

  if (artist === undefined) {
    return null;
  }

  const handleViewAllCards = (): void => {
    navigate(`/artists/${encodeURIComponent(artist.name)}`);
  };

  const sampleWorkLinks = artist.sampleCardNames.map((cardName) => (
    <Button
      key={cardName}
      variant="text"
      onClick={() => navigate(`/card/${encodeURIComponent(cardName)}?artists=${encodeURIComponent(artist.name)}`)}
      sx={{
        color: 'primary.main',
        textDecoration: 'none',
        p: 0,
        minWidth: 'auto',
        verticalAlign: 'baseline',
        '&:hover': {
          textDecoration: 'underline',
          bgcolor: 'transparent',
        },
      }}
    >
      <Typography variant="body2" component="span">
        {cardName}
      </Typography>
    </Button>
  ));

  return (
    <Box
      component="section"
      aria-labelledby="artist-heading"
      sx={{
        px: { xs: 4, md: 8 },
        py: { xs: 4, md: 8 },
        maxWidth: 1200,
        mx: 'auto',
      }}
    >
      <Grid container spacing={2}>
        <Grid size={{ xs: 12, md: 6 }}>
          <Box
            sx={{
              maxWidth: 350,
              mx: { xs: 'auto' },
              ml: { md: 'auto' },
              mr: { md: 0 },
            }}
          >
            {loading && (
              <Skeleton
                variant="rectangular"
                sx={{
                  width: '100%',
                  aspectRatio: '0.72',
                  borderRadius: 2,
                }}
              />
            )}
            {featuredCard !== undefined && loading === false && (
              <MtgCard
                card={featuredCard}
                context={{ isOnArtistPage: true, hidePrice: true, hideLinks: true }}
                index={0}
                groupId="artist-spotlight"
                onArtistClick={(artistName) => navigate(`/artists/${encodeURIComponent(artistName)}`)}
              />
            )}
            {featuredCard === undefined && loading === false && (
              <Box
                sx={{
                  width: '100%',
                  aspectRatio: '0.72',
                  borderRadius: 2,
                  bgcolor: 'grey.800',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                }}
              >
                <Typography color="text.secondary">
                  Card not found
                </Typography>
              </Box>
            )}
          </Box>
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Typography variant="h4" component="h2" id="artist-heading">
              {artist.name}
            </Typography>

            <Typography variant="body1" sx={{ color: 'text.secondary' }}>
              {artist.description}
            </Typography>

            <AppButton variant="contained" onClick={handleViewAllCards} sx={{ alignSelf: 'flex-start' }}>
              View {artist.name} Cards
            </AppButton>

            <Box sx={{ mt: 2 }}>
              <Typography variant="h6" component="h3" sx={{ mb: 1 }}>
                Sample Works
              </Typography>
              <SeparatedList items={sampleWorkLinks} separator="|" />
            </Box>
          </Box>
        </Grid>
      </Grid>
    </Box>
  );
};
