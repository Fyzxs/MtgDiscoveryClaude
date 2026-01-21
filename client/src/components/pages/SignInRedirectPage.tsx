import React, { useEffect, useState } from 'react';
import { logger } from '../../utils/logger';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { useMutation } from '@apollo/client/react';
import { PageContainer, Section } from '../molecules/layouts';
import { Heading, BodyText } from '../molecules/text';
import { LoadingIndicator, StatusMessage } from '../molecules/feedback';
import { REGISTER_USER } from '../../graphql/mutations/user';
import { getTokenReadyState } from '../../graphql/apollo-client';

interface RegistrationData {
  userId: string;
  displayName: string;
}

interface RegisterUserResponse {
  registerUserInfo?: {
    __typename: string;
    data?: RegistrationData;
    status?: {
      message: string;
    };
  };
}

export const SignInRedirectPage: React.FC = () => {
  const { isLoading, isAuthenticated, user, error } = useAuth0();
  const navigate = useNavigate();
  const [registerUser] = useMutation(REGISTER_USER);
  const [setupStatus, setSetupStatus] = useState<'loading' | 'waiting-token' | 'registering' | 'complete' | 'error'>('loading');
  const [statusMessage, setStatusMessage] = useState('Completing sign-in...');
  const [tokenReady, setTokenReady] = useState(false);

  // Monitor token ready state
  useEffect(() => {
    const checkTokenReady = () => {
      const ready = getTokenReadyState();
      if (ready !== tokenReady) {
        logger.debug('SignInRedirectPage - Token ready state changed:', ready);
        setTokenReady(ready);
      }
    };

    if (isAuthenticated && isLoading === false) {
      const interval = setInterval(checkTokenReady, 100);
      return () => clearInterval(interval);
    }
  }, [isAuthenticated, isLoading, tokenReady]);

  useEffect(() => {
    const handleUserSetup = async () => {
      if (isLoading) {
        setSetupStatus('loading');
        setStatusMessage('Completing sign-in...');
        return;
      }

      if (error) {
        setSetupStatus('error');
        setStatusMessage(`Authentication error: ${error.message}`);
        return;
      }

      if (!isAuthenticated || !user) {
        setSetupStatus('error');
        setStatusMessage('Authentication failed. Please try signing in again.');
        return;
      }

      // Wait for Auth0 token to be ready before making GraphQL call
      if (!tokenReady) {
        setSetupStatus('waiting-token');
        setStatusMessage('Preparing authentication token...');
        return;
      }

      try {
        setSetupStatus('registering');
        setStatusMessage('Registering your account...');

        // Call GraphQL mutation to register user - now with token ready
        const result = await registerUser();

        if ((result.data as RegisterUserResponse)?.registerUserInfo?.__typename === 'UserRegistrationSuccessResponse') {
          // Registration successful
          const registrationData = (result.data as RegisterUserResponse).registerUserInfo?.data;
          
          // Store user information locally for quick access
          const userData = {
            sub: user.sub,
            email: user.email,
            name: user.name || user.email,
            picture: user.picture,
            userId: registrationData?.userId,
            displayName: registrationData?.displayName,
            lastLoginAt: new Date().toISOString(),
          };

          localStorage.setItem('mtg-user-data', JSON.stringify(userData));

          setSetupStatus('complete');
          setStatusMessage('Registration complete! Redirecting...');

          // Redirect to home page after a brief delay
          setTimeout(() => {
            navigate('/', { replace: true });
          }, 1500);

        } else {
          // Registration failed
          const errorMsg = (result.data as RegisterUserResponse)?.registerUserInfo?.status?.message || 'Unknown registration error';
          setSetupStatus('error');
          setStatusMessage(`Registration failed: ${errorMsg}`);
        }

      } catch (registrationError) {
        logger.error('User registration error:', registrationError);
        setSetupStatus('error');
        setStatusMessage('Error registering your account. Please try again.');
      }
    };

    handleUserSetup();
  }, [isLoading, isAuthenticated, user, error, tokenReady, navigate, registerUser]);

  const getStatusColor = () => {
    switch (setupStatus) {
      case 'error': return 'error';
      case 'complete': return 'success';
      default: return 'info';
    }
  };

  return (
    <PageContainer maxWidth="sm" sx={{ py: 12 }}>
      <Section
        asSection={false}
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
          gap: 3
        }}
      >
        <Heading variant="h4" component="h1" gutterBottom>
          Welcome to MTG Discovery
        </Heading>

        {setupStatus !== 'error' && (
          <LoadingIndicator withContainer={false} size={48} />
        )}

        <StatusMessage
          severity={getStatusColor()}
          sx={{ width: '100%' }}
        >
          {statusMessage}
        </StatusMessage>

        {user && setupStatus !== 'error' && (
          <Section asSection={false} sx={{ mt: 2 }}>
            <BodyText variant="body1" color="text.secondary">
              Hello, {user.name || user.email}!
            </BodyText>
            {setupStatus === 'waiting-token' && (
              <BodyText variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                Preparing secure authentication...
              </BodyText>
            )}
          </Section>
        )}

        {setupStatus === 'error' && (
          <Section asSection={false} sx={{ mt: 2 }}>
            <BodyText variant="body2" color="text.secondary">
              You can try refreshing the page or{' '}
              <a href="/" style={{ color: 'inherit' }}>return to the home page</a>.
            </BodyText>
          </Section>
        )}
      </Section>
    </PageContainer>
  );
};export default SignInRedirectPage;
