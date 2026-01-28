import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, LinearProgress, Paper, CircularProgress, CardActionArea } from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';
import { useAuth0 } from '@auth0/auth0-react';
import type { RecentSetData, WishlistCardData } from '../../../../hooks/useHomePageData';

/** Maximum number of recent sets to display */
const MAX_RECENT_SETS = 3;

/** Maximum number of wishlist cards to display */
const MAX_WISHLIST_CARDS = 6;

interface CollectionPreviewProps {
  isAuthenticated: boolean;
  recentSets?: RecentSetData[];
  wishlistCards?: WishlistCardData[];
  isLoading?: boolean;
}

/**
 * Collection preview section with two variants:
 * - Authenticated: Shows recent sets in progress and wishlist preview
 * - Anonymous: Shows mockup with sign-up CTA
 */
export const CollectionPreview: React.FC<CollectionPreviewProps> = ({
  isAuthenticated,
  recentSets,
  wishlistCards,
  isLoading,
}) => {
  const { loginWithRedirect } = useAuth0();
  const navigate = useNavigate();

  const handleSignup = async (): Promise<void> => {
    await loginWithRedirect({
      authorizationParams: {
        screen_hint: 'signup',
      },
    });
  };

  if (isAuthenticated) {
    return (
      <Box
        component="section"
        aria-labelledby="collection-heading"
        sx={{
          maxWidth: 1200,
          mx: 'auto',
          px: { xs: 2, md: 4 },
          py: { xs: 4, md: 8 },
        }}
      >
        <Typography
          id="collection-heading"
          variant="h5"
          sx={{ fontWeight: 600, mb: 3 }}
        >
          Continue Where You Left Off
        </Typography>

        {isLoading === true ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 4 }}>
            {recentSets !== undefined && recentSets.length > 0 && (
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                {recentSets.slice(0, MAX_RECENT_SETS).map((set) => (
                  <Paper
                    key={set.code}
                    sx={{
                      flex: '1 1 250px',
                      maxWidth: 350,
                      transition: 'border-color 0.2s ease-in-out',
                      border: 1,
                      borderColor: 'divider',
                      overflow: 'hidden',
                      '&:hover': { borderColor: 'primary.main' },
                    }}
                    elevation={0}
                  >
                    <CardActionArea
                      onClick={() => navigate(`/set/${set.code}`)}
                      sx={{ p: 2 }}
                    >
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 2 }}>
                        <img
                          src={set.iconSvgUri}
                          alt=""
                          aria-hidden="true"
                          style={{ width: 24, height: 24 }}
                        />
                        <Typography variant="subtitle2" sx={{ fontWeight: 600 }}>
                          {set.name}
                        </Typography>
                      </Box>
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                        <Box sx={{ flex: 1 }}>
                          <LinearProgress
                            variant="determinate"
                            value={set.completionPercent}
                            sx={{ height: 6, borderRadius: 1 }}
                          />
                        </Box>
                        <Typography variant="caption" color="text.secondary">
                          {set.completionPercent}%
                        </Typography>
                      </Box>
                      <Typography
                        variant="body2"
                        sx={{ mt: 1.5, color: 'primary.main', fontWeight: 500 }}
                      >
                        Continue
                      </Typography>
                    </CardActionArea>
                  </Paper>
                ))}
              </Box>
            )}

            {wishlistCards !== undefined && wishlistCards.length > 0 && (
              <Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                  <Typography variant="h6" sx={{ fontWeight: 600 }}>
                    Wishlist Preview
                  </Typography>
                  <AppButton
                    variant="text"
                    size="small"
                    onClick={() => navigate('/wishlist')}
                  >
                    View All
                  </AppButton>
                </Box>
                <Box sx={{ display: 'flex', gap: 1.5, flexWrap: 'wrap' }}>
                  {wishlistCards.slice(0, MAX_WISHLIST_CARDS).map((card) => (
                    <Box
                      key={card.id}
                      sx={{
                        width: { xs: 80, sm: 100 },
                        aspectRatio: '5 / 7',
                        borderRadius: 1,
                        overflow: 'hidden',
                        bgcolor: 'grey.800',
                      }}
                    >
                      {card.imageUri !== undefined && (
                        <img
                          src={card.imageUri}
                          alt={`${card.name} card`}
                          style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                          loading="lazy"
                        />
                      )}
                    </Box>
                  ))}
                </Box>
              </Box>
            )}

            {(recentSets === undefined || recentSets.length === 0) &&
              (wishlistCards === undefined || wishlistCards.length === 0) && (
                <Box sx={{ textAlign: 'center', py: 4 }}>
                  <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
                    Start building your collection by browsing sets.
                  </Typography>
                  <AppButton
                    variant="contained"
                    onClick={() => navigate('/sets')}
                  >
                    Browse Sets
                  </AppButton>
                </Box>
              )}
          </Box>
        )}
      </Box>
    );
  }

  return (
    <Box
      component="section"
      aria-labelledby="collection-heading"
      sx={{
        maxWidth: 1200,
        mx: 'auto',
        px: { xs: 4, md: 8 },
        py: { xs: 4, md: 8 },
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
          gap: 4,
        }}
      >
        <Typography
          id="collection-heading"
          variant="h4"
          sx={{ fontWeight: 600, maxWidth: 600 }}
        >
          See Your Collection Come to Life
        </Typography>

        {/* TODO: Replace with real collection showcase image */}
        <Box
          sx={{
            maxWidth: 600,
            width: '100%',
            aspectRatio: '16 / 9',
            bgcolor: 'grey.800',
            borderRadius: 2,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            border: '2px dashed',
            borderColor: 'grey.600',
          }}
        >
          <Typography variant="body1" color="text.secondary">
            Collection showcase image placeholder
          </Typography>
        </Box>

        <Typography variant="body1" color="text.secondary" sx={{ maxWidth: 500 }}>
          Track your cards, monitor set completion, and discover what you need next.
        </Typography>

        <AppButton
          variant="contained"
          size="large"
          onClick={handleSignup}
        >
          Start Tracking Your Collection
        </AppButton>
      </Box>
    </Box>
  );
};
