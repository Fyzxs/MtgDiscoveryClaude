import React, { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setAuth0TokenGetter } from '../../graphql/apollo-client';

interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

export const Auth0TokenProvider: React.FC<Auth0TokenProviderProps> = ({ children }) => {
  const { getIdTokenClaims, isAuthenticated } = useAuth0();

  useEffect(() => {
    const getToken = async (): Promise<string | null> => {
      if (!isAuthenticated) {
        return null;
      }
      
      try {
        const idTokenClaims = await getIdTokenClaims();
        const idToken = idTokenClaims?.__raw;
        return idToken || null;
      } catch (error) {
        console.error('Failed to get Auth0 ID token:', error);
        return null;
      }
    };

    // Set the token getter in Apollo Client
    setAuth0TokenGetter(getToken);
  }, [getIdTokenClaims, isAuthenticated]);


  return <>{children}</>;
};