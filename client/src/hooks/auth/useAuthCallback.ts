import { useEffect, useCallback, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useMutation } from '@apollo/client/react';
import { SYNC_USER } from '../../graphql/mutations/user';
import { useAuthState } from '../../contexts/AuthStateContext';
import type { UserProfile, AuthError } from '../../types/auth';
import type { RegisterUserMutation } from '../../generated/graphql';

/**
 * Hook that handles the Auth0 callback flow and syncs user with backend.
 * This hook should be used in the AuthCallbackPage component.
 *
 * Flow:
 * 1. Auth0 redirects back after login
 * 2. This hook detects authenticated state
 * 3. Calls backend to sync user (creates or updates)
 * 4. Backend returns isFirstLogin flag
 * 5. Updates AuthStateContext with sync result
 */
export const useAuthCallback = () => {
  const { isAuthenticated, isLoading: auth0Loading, error: auth0Error, getAccessTokenSilently } = useAuth0();
  const { state, handleSyncSuccess, handleSyncFailure, dispatch } = useAuthState();
  const [syncUserMutation] = useMutation<RegisterUserMutation>(SYNC_USER);
  const hasSyncedRef = useRef(false);

  // Check if an error indicates session expiry
  const isSessionExpiredError = (errorMessage: string): boolean => {
    const sessionExpiredPatterns = [
      'Missing Refresh Token',
      'login_required',
      'Token Refresh Error',
      'consent_required',
      'interaction_required',
      'session expired',
      'Session Expired',
    ];
    return sessionExpiredPatterns.some((pattern) =>
      errorMessage.toLowerCase().includes(pattern.toLowerCase())
    );
  };

  const syncWithBackend = useCallback(async () => {
    // Prevent duplicate sync calls
    if (hasSyncedRef.current) {
      return;
    }

    try {
      // Ensure we have a valid token before making the mutation
      await getAccessTokenSilently();

      hasSyncedRef.current = true;
      dispatch({ type: 'SYNC_START' });

      const result = await syncUserMutation();
      const response = result.data?.registerUserInfo;

      if (response?.__typename === 'UserRegistrationSuccessResponse') {
        const userData = response.data;
        const userProfile: UserProfile = {
          id: userData.userId,
          displayName: userData.displayName,
          email: userData.email,
          // Explicitly convert DateTime scalars to strings for type safety
          createdAt: String(userData.createdAt),
          lastLoginAt: String(userData.lastLoginAt),
        };
        handleSyncSuccess(userProfile, userData.isFirstLogin);
      } else if (response?.__typename === 'FailureResponse') {
        const errorMessage = response.status?.message || 'Failed to sync user with backend';
        const error: AuthError = {
          code: 'sync_failed',
          message: errorMessage,
        };
        handleSyncFailure(error);
      } else {
        const error: AuthError = {
          code: 'sync_failed',
          message: 'Received unexpected response from server',
        };
        handleSyncFailure(error);
      }
    } catch (err) {
      hasSyncedRef.current = false; // Allow retry on error
      const errorMessage = err instanceof Error ? err.message : 'An unexpected error occurred';

      // Check for session expiry errors (US5: Session Expiry Handling)
      if (isSessionExpiredError(errorMessage)) {
        dispatch({ type: 'SESSION_EXPIRED' });
        return;
      }

      const error: AuthError = {
        code: 'sync_failed',
        message: errorMessage,
      };
      handleSyncFailure(error);
    }
  }, [dispatch, getAccessTokenSilently, handleSyncFailure, handleSyncSuccess, syncUserMutation]);

  // Handle Auth0 authentication errors (FR-012) and session expiry (US5)
  useEffect(() => {
    if (auth0Error) {
      const errorMessage = auth0Error.message || 'Authentication failed';

      // Check for session expiry errors from Auth0
      if (isSessionExpiredError(errorMessage)) {
        dispatch({ type: 'SESSION_EXPIRED' });
        return;
      }

      const error: AuthError = {
        code: 'auth_failed',
        message: errorMessage,
      };
      handleSyncFailure(error);
    }
  }, [auth0Error, dispatch, handleSyncFailure]);

  // Trigger sync when Auth0 authentication completes
  useEffect(() => {
    // Only sync when:
    // 1. Auth0 has finished loading
    // 2. User is authenticated
    // 3. We're in the syncing state (meaning we're expecting to sync)
    // 4. We haven't already synced
    if (
      auth0Loading === false &&
      isAuthenticated &&
      state.status === 'syncing' &&
      hasSyncedRef.current === false
    ) {
      syncWithBackend();
    }
  }, [auth0Loading, isAuthenticated, state.status, syncWithBackend]);

  // Reset sync flag when user logs out
  useEffect(() => {
    if (state.status === 'unauthenticated') {
      hasSyncedRef.current = false;
    }
  }, [state.status]);

  return {
    isProcessing: state.status === 'syncing' || state.status === 'authenticating',
    error: state.error,
    syncWithBackend,
  };
};
