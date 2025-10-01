import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { useMutation } from '@apollo/client/react';
import { 
  Container, 
  Typography, 
  Box,
  CircularProgress,
  Alert
} from '@mui/material';
import { REGISTER_USER } from '../../graphql/mutations/user';
import { getTokenReadyState } from '../../graphql/apollo-client';

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
        console.log('SignInRedirectPage - Token ready state changed:', ready);
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

        if ((result.data as any)?.registerUserInfo?.__typename === 'SuccessUserRegistrationResponse') {
          // Registration successful
          const registrationData = (result.data as any).registerUserInfo.data;
          
          // Store user information locally for quick access
          const userData = {
            sub: user.sub,
            email: user.email,
            name: user.name || user.email,
            picture: user.picture,
            userId: registrationData.userId,
            displayName: registrationData.displayName,
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
          const errorMsg = (result.data as any)?.registerUserInfo?.status?.message || 'Unknown registration error';
          setSetupStatus('error');
          setStatusMessage(`Registration failed: ${errorMsg}`);
        }

      } catch (registrationError) {
        console.error('User registration error:', registrationError);
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
    <Container maxWidth="sm" sx={{ py: 12 }}>
      <Box 
        sx={{ 
          display: 'flex', 
          flexDirection: 'column', 
          alignItems: 'center',
          textAlign: 'center',
          gap: 3
        }}
      >
        <Typography variant="h4" component="h1" gutterBottom>
          Welcome to MTG Discovery
        </Typography>

        {setupStatus !== 'error' && (
          <CircularProgress size={48} />
        )}

        <Alert 
          severity={getStatusColor()}
          sx={{ width: '100%' }}
        >
          {statusMessage}
        </Alert>

        {user && setupStatus !== 'error' && (
          <Box sx={{ mt: 2 }}>
            <Typography variant="body1" color="text.secondary">
              Hello, {user.name || user.email}!
            </Typography>
            {setupStatus === 'waiting-token' && (
              <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                Preparing secure authentication...
              </Typography>
            )}
          </Box>
        )}

        {setupStatus === 'error' && (
          <Box sx={{ mt: 2 }}>
            <Typography variant="body2" color="text.secondary">
              You can try refreshing the page or{' '}
              <a href="/" style={{ color: 'inherit' }}>return to the home page</a>.
            </Typography>
          </Box>
        )}
      </Box>
    </Container>
  );
};