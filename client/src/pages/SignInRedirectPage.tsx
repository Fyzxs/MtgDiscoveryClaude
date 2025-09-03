import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useNavigate } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Box,
  CircularProgress,
  Alert
} from '@mui/material';

export const SignInRedirectPage: React.FC = () => {
  const { isLoading, isAuthenticated, user, error } = useAuth0();
  const navigate = useNavigate();
  const [setupStatus, setSetupStatus] = useState<'loading' | 'configuring' | 'complete' | 'error'>('loading');
  const [statusMessage, setStatusMessage] = useState('Completing sign-in...');

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

      try {
        setSetupStatus('configuring');
        setStatusMessage('Setting up your account...');

        // Store user information locally
        const userData = {
          sub: user.sub,
          email: user.email,
          name: user.name || user.email,
          picture: user.picture,
          // Add collector number or other preferences here
          collectorNumber: null, // This could be pulled from user metadata
          lastLoginAt: new Date().toISOString(),
        };

        localStorage.setItem('mtg-user-data', JSON.stringify(userData));
        
        // Here you could also make API calls to:
        // 1. Register the user in your backend if they don't exist
        // 2. Sync user preferences
        // 3. Check for any pending collections or settings

        // Simulate some setup time
        await new Promise(resolve => setTimeout(resolve, 1000));

        setSetupStatus('complete');
        setStatusMessage('Setup complete! Redirecting...');

        // Redirect to home page after a brief delay
        setTimeout(() => {
          navigate('/', { replace: true });
        }, 1500);

      } catch (setupError) {
        console.error('User setup error:', setupError);
        setSetupStatus('error');
        setStatusMessage('Error setting up your account. Please try again.');
      }
    };

    handleUserSetup();
  }, [isLoading, isAuthenticated, user, error, navigate]);

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