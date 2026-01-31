import React, { useCallback } from 'react';
import { Button, Box, Typography, CircularProgress } from '../atoms';
import { useAuth0 } from '@auth0/auth0-react';
import { useApolloClient } from '@apollo/client/react';
import { MyCollectionButton } from '../molecules/ui/MyCollectionButton';
import { useAuthState } from '../../contexts/AuthStateContext';
import { logger } from '../../utils/logger';

interface AuthButtonProps {
  /** Whether the button should take full width */
  fullWidth?: boolean;
}

export const AuthButton: React.FC<AuthButtonProps> = ({ fullWidth = false }) => {
  const { isAuthenticated, user, isLoading } = useAuth0();
  const { login, logout } = useAuthState();
  const apolloClient = useApolloClient();

  // Handle logout with Apollo cache clear
  const handleLogout = useCallback(async () => {
    try {
      // Clear Apollo cache before logging out to prevent stale data
      await apolloClient.clearStore();
      logger.debug('AuthButton - Apollo cache cleared');
    } catch (error) {
      logger.error('AuthButton - Error clearing Apollo cache:', error);
    }
    // Use auth state logout which dispatches LOGOUT action and calls Auth0 logout
    logout();
  }, [apolloClient, logout]);

  if (isLoading) {
    return (
      <Button
        disabled
        fullWidth={fullWidth}
        startIcon={<CircularProgress size={20} />}
        sx={{ minHeight: 44 }}
      >
        Loading...
      </Button>
    );
  }

  if (isAuthenticated && user) {
    // Mobile/fullWidth layout - vertical stack
    if (fullWidth) {
      return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, width: '100%' }}>
          <MyCollectionButton />
          <Typography
            variant="body2"
            sx={{
              color: 'text.primary',
              textAlign: 'center'
            }}
          >
            Welcome, {user.name || user.email}
          </Typography>
          <Button
            onClick={handleLogout}
            variant="outlined"
            fullWidth
            sx={{ minHeight: 44 }}
          >
            Logout
          </Button>
        </Box>
      );
    }

    // Desktop layout - horizontal row
    return (
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
        <MyCollectionButton />
        <Typography
          variant="body2"
          sx={{ color: 'text.primary' }}
        >
          Welcome, {user.name || user.email}
        </Typography>
        <Button
          onClick={handleLogout}
          variant="outlined"
          size="small"
          sx={{ minHeight: 44 }}
        >
          Logout
        </Button>
      </Box>
    );
  }

  return (
    <Button
      onClick={login}
      variant="contained"
      size={fullWidth ? 'medium' : 'small'}
      fullWidth={fullWidth}
      sx={{ minHeight: 44 }}
    >
      Login
    </Button>
  );
};