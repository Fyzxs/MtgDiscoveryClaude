import React, { useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { PageContainer, Section } from '../molecules/layouts';
import { Heading, BodyText } from '../molecules/text';
import { LoadingIndicator, StatusMessage } from '../molecules/feedback';
import { useAuthState } from '../../contexts/AuthStateContext';
import { useAuthCallback } from '../../hooks/auth/useAuthCallback';
import type { AlertColor } from '@mui/material';

/**
 * Auth callback page that handles the OAuth redirect flow.
 *
 * This page:
 * 1. Receives the callback from Auth0
 * 2. Triggers backend sync via useAuthCallback
 * 3. Shows appropriate loading/error states
 * 4. Redirects to intended destination on success
 *
 * The welcome toast ("Welcome to MtgDiscovery" or "Welcome back") is
 * handled by AuthStateContext when sync succeeds.
 */
export const AuthCallbackPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { state: authState } = useAuthState();
  const { isProcessing, error } = useAuthCallback();

  // Get the return URL from Auth0 appState or default to home
  // Validates that returnTo is a safe relative path to prevent open redirect attacks
  const getReturnUrl = (): string => {
    const searchParams = new URLSearchParams(location.search);
    const returnTo = searchParams.get('returnTo');

    // Security: Only allow relative paths starting with /
    // Reject absolute URLs, protocol-relative URLs, and other potentially malicious inputs
    if (returnTo && returnTo.startsWith('/') && returnTo.startsWith('//') === false) {
      return returnTo;
    }
    return '/';
  };

  // Redirect when authentication completes successfully
  useEffect(() => {
    if (authState.status === 'authenticated') {
      const returnUrl = getReturnUrl();
      // Small delay to allow welcome toast to show
      const timeoutId = setTimeout(() => {
        navigate(returnUrl, { replace: true });
      }, 500);
      return () => { clearTimeout(timeoutId); };
    }
  }, [authState.status, navigate, location.search]);

  const getStatusSeverity = (): AlertColor => {
    if (error) {
      return 'error';
    }
    if (authState.status === 'authenticated') {
      return 'success';
    }
    return 'info';
  };

  const getStatusMessage = (): string => {
    if (error) {
      return error.message;
    }
    switch (authState.status) {
      case 'initializing':
        return 'Initializing authentication...';
      case 'authenticating':
        return 'Completing sign-in...';
      case 'syncing':
        return 'Setting up your account...';
      case 'authenticated':
        return 'Success! Redirecting...';
      case 'unauthenticated':
        return 'Authentication failed. Please try signing in again.';
      default:
        return 'Processing...';
    }
  };

  const showSpinner = isProcessing || authState.status === 'authenticated';
  const showError = Boolean(error) || authState.status === 'unauthenticated';

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

        {showSpinner && (
          <LoadingIndicator withContainer={false} size={48} />
        )}

        <StatusMessage
          severity={getStatusSeverity()}
          sx={{ width: '100%' }}
        >
          {getStatusMessage()}
        </StatusMessage>

        {authState.user && authState.status === 'authenticated' && (
          <Section asSection={false} sx={{ mt: 2 }}>
            <BodyText variant="body1" color="text.secondary">
              Hello, {authState.user.displayName}!
            </BodyText>
          </Section>
        )}

        {showError && (
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
};

export default AuthCallbackPage;
