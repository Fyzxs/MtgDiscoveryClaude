import React, { useEffect, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter, setTokenReadyState } from '../../graphql/apollo-client';
import { logger } from '../../utils/logger';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect } = useAuth0();
  const tokenGetterRegistered = useRef(false);

  useEffect(() => {
    const initializeAuth = async () => {
      if (isLoading) {
        return;
      }

      if (isAuthenticated === false) {
        setTokenReadyState(false);
        return;
      }

      // Step 1: Register the token getter FIRST (before signaling ready)
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

      // Step 2: Verify we can get a token before signaling ready
      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: "api://mtg-discovery"
          }
        });

        if (accessToken) {
          logger.debug('Auth0TokenProvider - Access token verified, signaling ready');
          setTokenReadyState(true);
        }
      } catch (error) {
        // Check if this is a missing refresh token error
        const errorMessage = error instanceof Error ? error.message : String(error);
        if (errorMessage.includes('Missing Refresh Token') || errorMessage.includes('login_required')) {
          logger.warn('Auth0TokenProvider - Session expired, re-authenticating...');
          loginWithRedirect();
          return;
        }
        logger.error('Auth0TokenProvider - Failed to get access token:', error);
        setTokenReadyState(false);
      }
    };

    initializeAuth();
  }, [getAccessTokenSilently, isAuthenticated, isLoading, loginWithRedirect]);

  return <>{children}</>;
};