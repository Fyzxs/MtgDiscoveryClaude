import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter, setTokenReadyState } from '../../graphql/apollo-client';
import { logger } from '../../utils/logger';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated, isLoading } = useAuth0();
  const [tokenReady, setTokenReady] = useState(false);

  useEffect(() => {
    const initializeToken = async () => {
      if (isLoading) {
        return;
      }

      if (isAuthenticated === false) {
        setTokenReady(false);
        setTokenReadyState(false);
        return;
      }

      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
            audience: "api://mtg-discovery"
          }
        });

        if (accessToken) {
          logger.debug('Auth0TokenProvider - Access token acquired successfully');
          setTokenReady(true);
          setTokenReadyState(true);
        }
      } catch (error) {
        logger.error('Auth0TokenProvider - Failed to get access token:', error);
        setTokenReady(false);
        setTokenReadyState(false);
      }
    };

    initializeToken();
  }, [getAccessTokenSilently, isAuthenticated, isLoading]);

  useEffect(() => {
    const getToken = async (): Promise<string | null> => {
      if (tokenReady === false) {
        return null;
      }

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
  }, [getAccessTokenSilently, tokenReady]);

  return <>{children}</>;
};