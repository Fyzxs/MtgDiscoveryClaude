import { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useAuthState } from '../../contexts/AuthStateContext';
import type { AuthError } from '../../types/auth';

/**
 * Hook that handles Auth0 callback-page-specific concerns.
 *
 * The actual backend sync is performed by AuthStateProvider whenever
 * the auth state enters 'syncing'. This hook only handles:
 * - Auth0 redirect errors (e.g., consent_required, login_required)
 * - Provides processing/error status for the callback page UI
 */
export const useAuthCallback = () => {
  const { error: auth0Error } = useAuth0();
  const { state, handleSyncFailure, dispatch } = useAuthState();

  // Handle Auth0 authentication errors (FR-012) and session expiry (US5)
  useEffect(() => {
    if (auth0Error) {
      const errorMessage = auth0Error.message || 'Authentication failed';

      const sessionExpiredPatterns = [
        'Missing Refresh Token', 'login_required', 'Token Refresh Error',
        'consent_required', 'interaction_required', 'session expired',
      ];
      const isSessionExpired = sessionExpiredPatterns.some((p) =>
        errorMessage.toLowerCase().includes(p.toLowerCase())
      );

      if (isSessionExpired) {
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

  return {
    isProcessing: state.status === 'syncing' || state.status === 'authenticating',
    error: state.error,
  };
};
