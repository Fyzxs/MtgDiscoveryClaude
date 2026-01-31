import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Paper, Stack, Button } from '../../../atoms';
import { LightbulbIcon } from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';
import { proTips } from '../data/proTips';
import { getDailyRotatedItem } from '../../../../utils/rotation';

interface BottomCTAProps {
  isAuthenticated: boolean;
}

/**
 * Bottom call-to-action section with two variants:
 * - Anonymous: Sign-up promotion with gradient background
 * - Authenticated: Daily rotating pro tip with action button
 */
export const BottomCTA: React.FC<BottomCTAProps> = ({ isAuthenticated }) => {
  const { loginWithRedirect } = useAuth0();
  const navigate = useNavigate();

  const currentTip = getDailyRotatedItem(proTips);

  const handleSignIn = async (): Promise<void> => {
    await loginWithRedirect();
  };

  const handleSignUp = async (): Promise<void> => {
    await loginWithRedirect({
      authorizationParams: { screen_hint: 'signup' },
    });
  };

  if (isAuthenticated === false) {
    return (
      <Box
        component="section"
        aria-labelledby="cta-heading"
        sx={{
          py: { xs: 4, md: 8 },
          mb: { xs: 4, md: 6 },
        }}
      >
        <Box sx={{ maxWidth: '1200px', mx: 'auto', px: { xs: 2, sm: 3, md: 4 } }}>
          <Paper
            sx={{
              background: (theme) => theme.mtg.gradients.header,
              p: { xs: 4, md: 6 },
              textAlign: 'center',
              borderRadius: 2,
            }}
          >
            <Typography
              id="cta-heading"
              variant="h4"
              component="h2"
              sx={{
                mb: 2,
                fontWeight: 600,
                color: 'common.white',
              }}
            >
              Ready to Start Your Collection Journey?
            </Typography>
            <Typography
              variant="body1"
              sx={{
                mb: 4,
                color: 'grey.100',
                maxWidth: '600px',
                mx: 'auto',
              }}
            >
              Create a free account to track your cards, build wishlists, and plan convention signings.
            </Typography>
            <Stack spacing={2} alignItems="center">
              <AppButton
                variant="contained"
                size="large"
                onClick={handleSignUp}
                sx={{
                  bgcolor: 'common.white',
                  color: 'primary.main',
                  '&:hover': {
                    bgcolor: 'grey.100',
                  },
                }}
              >
                Create Free Account
              </AppButton>
              <Typography variant="body2" sx={{ color: 'grey.200' }}>
                Already have an account?{' '}
                <Button
                  variant="text"
                  onClick={handleSignIn}
                  sx={{
                    color: 'common.white',
                    textDecoration: 'underline',
                    fontWeight: 600,
                    p: 0,
                    minWidth: 'auto',
                    verticalAlign: 'baseline',
                    '&:hover': {
                      color: 'grey.100',
                      bgcolor: 'transparent',
                      textDecoration: 'underline',
                    },
                  }}
                >
                  Sign In
                </Button>
              </Typography>
            </Stack>
          </Paper>
        </Box>
      </Box>
    );
  }

  if (currentTip === undefined) {
    return null;
  }

  return (
    <Box
      component="section"
      aria-labelledby="protip-heading"
      sx={{
        py: { xs: 4, md: 8 },
        mb: { xs: 4, md: 6 },
      }}
    >
      <Box sx={{ maxWidth: '1200px', mx: 'auto', px: { xs: 2, sm: 3, md: 4 } }}>
        <Paper
          sx={{
            bgcolor: 'background.paper',
            border: 1,
            borderColor: 'divider',
            p: { xs: 3, md: 4 },
            borderRadius: 2,
          }}
        >
          <Stack direction="row" spacing={2} alignItems="flex-start" sx={{ mb: 2 }}>
            <LightbulbIcon sx={{ color: 'primary.main', fontSize: 32 }} />
            <Box sx={{ flex: 1 }}>
              <Typography
                id="protip-heading"
                variant="h5"
                component="h2"
                sx={{
                  mb: 1,
                  fontWeight: 600,
                }}
              >
                {currentTip.title}
              </Typography>
              <Typography
                variant="body1"
                sx={{
                  color: 'text.secondary',
                  mb: 3,
                }}
              >
                {currentTip.body}
              </Typography>
              <AppButton
                variant="contained"
                onClick={() => navigate(currentTip.ctaRoute)}
              >
                {currentTip.ctaLabel}
              </AppButton>
            </Box>
          </Stack>
        </Paper>
      </Box>
    </Box>
  );
};
