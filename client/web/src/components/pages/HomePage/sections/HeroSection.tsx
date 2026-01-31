import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { Box, Container, Grid, Typography, useTheme } from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';
import { StatBox } from '../components/StatBox';
import { getFadeInUpStyle } from '../../../../styles/animations';
import type { UserHomepageStats } from '../../../../hooks/useHomePageData';

interface HeroSectionProps {
  isAuthenticated: boolean;
  userName: string | undefined;
  stats: UserHomepageStats | null;
  statsError: boolean;
}

/**
 * Hero section for the homepage with authenticated and anonymous variants.
 * - Authenticated: Shows welcome message with user stats
 * - Anonymous: Shows value proposition with sign-up CTA
 */
export const HeroSection: React.FC<HeroSectionProps> = ({
  isAuthenticated,
  userName,
  stats,
  statsError,
}) => {
  const { loginWithRedirect } = useAuth0();
  const navigate = useNavigate();
  const theme = useTheme();

  const handleBrowseSets = (): void => {
    navigate('/sets');
  };

  const handleStartCollecting = async (): Promise<void> => {
    await loginWithRedirect({
      authorizationParams: {
        screen_hint: 'signup',
      },
    });
  };

  const showAuthenticatedHero = isAuthenticated && statsError === false;

  if (showAuthenticatedHero) {
    return (
      <Box
        component="section"
        aria-labelledby="hero-heading"
        sx={{
          background: theme.mtg.gradients.header,
          py: { xs: 4, md: 6 },
        }}
      >
        <Container maxWidth="md">
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
              id="hero-heading"
              variant="h2"
              component="h1"
              sx={{
                fontSize: { xs: theme.typography.h4.fontSize, md: theme.typography.h2.fontSize },
                fontWeight: 700,
                color: 'common.white',
              }}
            >
              Welcome back, {userName ?? 'Collector'}!
            </Typography>

            <Grid container spacing={2} sx={{ maxWidth: 600 }}>
              <Grid size={{ xs: 12, sm: 4 }}>
                <StatBox
                  value={stats?.cardsTracked ?? null}
                  label="Cards Tracked"
                  ctaText="Start tracking!"
                  onClick={() => navigate('/sets')}
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 4 }}>
                <StatBox
                  value={stats?.setsInProgress ?? null}
                  label="Sets in Progress"
                  ctaText="Pick a set!"
                  onClick={() => navigate('/sets')}
                />
              </Grid>
              <Grid size={{ xs: 12, sm: 4 }}>
                <StatBox
                  value={stats?.wishlistCount ?? null}
                  label="Wishlist Items"
                  ctaText="Add cards!"
                  onClick={() => navigate('/wishlist')}
                />
              </Grid>
            </Grid>

            <AppButton
              variant="outlined"
              size="large"
              onClick={handleBrowseSets}
              sx={{
                color: 'common.white',
                borderColor: 'common.white',
                '&:hover': {
                  borderColor: 'common.white',
                  bgcolor: 'rgba(255, 255, 255, 0.1)',
                },
              }}
            >
              Browse Sets
            </AppButton>
          </Box>
        </Container>
      </Box>
    );
  }

  return (
    <Box
      component="section"
      aria-labelledby="hero-heading"
      sx={{
        background: theme.mtg.gradients.header,
        py: { xs: 4, md: 5 },
      }}
    >
      <Container maxWidth="md">
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            textAlign: 'center',
            maxWidth: '800px',
            mx: 'auto',
          }}
        >
          <Typography
            id="hero-heading"
            variant="h1"
            component="h1"
            gutterBottom
            sx={{
              fontSize: { xs: '1.25rem', md: '1.75rem' },
              fontWeight: 700,
              color: 'common.white',
              mb: 3,
              ...getFadeInUpStyle(0),
            }}
          >
            Track Your Collection. Discover Every Card.
          </Typography>

          <Typography
            variant="h5"
            component="p"
            sx={{
              color: 'grey.100',
              mb: 4,
              maxWidth: '700px',
              lineHeight: 1.6,
              fontSize: { xs: '0.875rem', md: '1rem' },
              ...getFadeInUpStyle(100),
            }}
          >
            The complete toolkit for Magic: The Gathering collectors. Browse 25,000+ cards, track
            your collection, and never miss a signing.
          </Typography>

          <AppButton
            variant="contained"
            size="large"
            onClick={handleStartCollecting}
            sx={{
              bgcolor: 'common.white',
              color: 'primary.main',
              minWidth: { xs: '100%', sm: '180px' },
              mb: 2,
              '&:hover': {
                bgcolor: 'grey.100',
              },
              ...getFadeInUpStyle(200),
            }}
          >
            Start Collecting
          </AppButton>

          <Typography
            variant="body2"
            sx={{
              color: 'grey.300',
              fontStyle: 'italic',
              ...getFadeInUpStyle(300),
            }}
          >
            No account needed to explore
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};
