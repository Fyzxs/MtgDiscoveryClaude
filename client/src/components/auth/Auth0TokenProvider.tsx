import React, { useEffect, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter, setTokenReadyState } from '../../graphql/apollo-client';
import { logger } from '../../utils/logger';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

/**
 * Error messages indicating the Auth0 session has expired and local cached
 * state is stale. When detected, the stale localStorage cache is cleared
 * so the UI shows "Login" instead of a broken half-authenticated state.
 */
const SESSION_EXPIRED_PATTERNS = [
  'missing refresh token',
  'login_required',
  'token refresh error',
  'consent_required',
  'interaction_required',
  'invalid_grant',
];

const isSessionExpiredError = (error: unknown): boolean => {
  const message = error instanceof Error ? error.message : String(error);
  const lower = message.toLowerCase();
  return SESSION_EXPIRED_PATTERNS.some((pattern) => lower.includes(pattern));
};

/**
 * Provides Auth0 token getter to Apollo Client for authenticated requests.
 *
 * This component:
 * - Registers a token getter with Apollo Client when authenticated
 * - Signals token ready state for legacy useUserSync hook
 * - Uses Auth0's useRefreshTokens for silent token renewal (FR-011)
 * - Token acquisition is handled per-request by Apollo's auth link
 * - Detects stale sessions (expired refresh tokens) and clears cached auth state
 *
 * Note: Auth state management is handled by AuthStateContext, not this provider.
 */
export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading, logout } = useAuth0();
  const tokenGetterRegistered = useRef(false);

  useEffect(() => {
    const initializeAuth = async () => {
      // Skip if still loading Auth0 state
      if (isLoading) {
        return;
      }

      // Clear token getter if not authenticated
      if (isAuthenticated === false) {
        tokenGetterRegistered.current = false;
        setTokenReadyState(false);
        return;
      }

      // Only register once per authentication session
      if (tokenGetterRegistered.current) {
        return;
      }

      // Register token getter for Apollo Client
      const getToken = async (): Promise<string | null> => {
        try {
          const accessToken = await getAccessTokenSilently({
            authorizationParams: {
              audience: "api://mtg-discovery"
            }
          });
          return accessToken || null;
        } catch (error) {
          logger.error('Failed to get Auth0 access token:', error);
          return null;
        }
      };

      setAuth0TokenGetter(getToken);
      tokenGetterRegistered.current = true;
      logger.debug('Auth0TokenProvider - Token getter registered');

      // Verify token and signal ready state for legacy useUserSync
      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: "api://mtg-discovery"
          }
        });
        if (accessToken) {
          logger.debug('Auth0TokenProvider - Token verified, signaling ready');
          setTokenReadyState(true);
        }
      } catch (error) {
        logger.error('Auth0TokenProvider - Token verification failed:', error);
        setTokenReadyState(false);

        // Auth0 reports isAuthenticated=true from cached ID token in localStorage,
        // but the access token can't be refreshed. Clear the stale cached state
        // so the UI shows "Login" instead of a broken half-authenticated state.
        if (isSessionExpiredError(error)) {
          logger.warn('Auth0TokenProvider - Session expired, clearing stale auth cache');
          tokenGetterRegistered.current = false;
          await logout({ openUrl: false });
        }
      }
    };

    initializeAuth();
  }, [getAccessTokenSilently, isAuthenticated, isLoading, logout]);

  return <>{children}</>;
};