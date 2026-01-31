import React, { useEffect, useRef } from 'react';
import { Box, CircularProgress, Typography } from '../atoms';
import { useAuthState } from '../../contexts/AuthStateContext';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

/**
 * A route wrapper that requires authentication.
 *
 * Behavior:
 * - While initializing: Shows loading indicator
 * - If unauthenticated: Auto-triggers login with return URL
 * - If authenticated: Renders children
 *
 * The return URL is preserved in the Auth0 appState so users are
 * redirected back to their intended destination after login.
 */
export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { state: authState, login } = useAuthState();
  const hasTriggeredLogin = useRef(false);

  // Auto-trigger login when unauthenticated
  useEffect(() => {
    if (authState.status === 'unauthenticated' && hasTriggeredLogin.current === false) {
      hasTriggeredLogin.current = true;
      // login() captures window.location.pathname for return redirect
      login();
    }
  }, [authState.status, login]);

  // Reset login trigger when auth status changes from unauthenticated
  useEffect(() => {
    if (authState.status !== 'unauthenticated') {
      hasTriggeredLogin.current = false;
    }
  }, [authState.status]);

  // Show loading while auth state is being initialized or login is in progress
  if (
    authState.status === 'initializing' ||
    authState.status === 'syncing' ||
    authState.status === 'authenticating'
  ) {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: '60vh',
          gap: 2,
        }}
      >
        <CircularProgress />
        <Typography variant="body2" color="text.secondary">
          Checking authentication...
        </Typography>
      </Box>
    );
  }

  // If unauthenticated, show redirecting message (login effect will trigger redirect)
  if (authState.status === 'unauthenticated') {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: '60vh',
          gap: 2,
        }}
      >
        <CircularProgress />
        <Typography variant="body2" color="text.secondary">
          Redirecting to sign in...
        </Typography>
      </Box>
    );
  }

  // User is authenticated
  return <>{children}</>;
};

export default ProtectedRoute;
