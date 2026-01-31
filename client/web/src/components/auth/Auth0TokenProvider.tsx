import React, { useEffect, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter, setTokenReadyState } from '../../graphql/apollo-client';
import { logger } from '../../utils/logger';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

/**
 * Provides Auth0 token getter to Apollo Client for authenticated requests.
 *
 * This component:
 * - Registers a token getter with Apollo Client when authenticated
 * - Signals token ready state for legacy useUserSync hook
 * - Uses Auth0's useRefreshTokens for silent token renewal (FR-011)
 * - Token acquisition is handled per-request by Apollo's auth link
 *
 * Note: Auth state management is handled by AuthStateContext, not this provider.
 */
export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading } = useAuth0();
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
      }
    };

    initializeAuth();
  }, [getAccessTokenSilently, isAuthenticated, isLoading]);

  return <>{children}</>;
};