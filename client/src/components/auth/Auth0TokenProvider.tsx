import React, { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter } from '../../graphql/apollo-client';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();

  useEffect(() => {
    const getToken = async (): Promise<string | null> => {
      if (!isAuthenticated) {
        return null;
      }
      
      try {
        const token = await getAccessTokenSilently();
        return token;
      } catch (error) {
        console.error('Failed to get Auth0 access token:', error);
        return null;
      }
    };

    // Set the token getter in Apollo Client
    setAuth0TokenGetter(getToken);
  }, [getAccessTokenSilently, isAuthenticated]);


  return <>{children}</>;
};