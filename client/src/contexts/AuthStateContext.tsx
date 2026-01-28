import React, { createContext, useContext, useReducer, useCallback, useEffect, useRef } from 'react';
import type { ReactNode } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useMutation } from '@apollo/client/react';
import { SYNC_USER } from '../graphql/mutations/user';
import type { AuthState, AuthAction, UserProfile, AuthError } from '../types/auth';
import type { RegisterUserMutation } from '../generated/graphql';
import { initialAuthState } from '../types/auth';
import { useToast } from './ToastContext';

/**
 * Auth state reducer implementing the auth state machine
 */
function authReducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'INITIALIZE_START':
      return {
        ...state,
        status: 'initializing',
        error: null,
      };

    case 'INITIALIZE_AUTHENTICATED':
      return {
        ...state,
        status: 'authenticated',
        user: action.user,
        error: null,
      };

    case 'INITIALIZE_UNAUTHENTICATED':
      return {
        ...state,
        status: 'unauthenticated',
        user: null,
        isFirstLogin: false,
        error: null,
      };

    case 'LOGIN_START':
      return {
        ...state,
        status: 'authenticating',
        error: null,
      };

    case 'CALLBACK_RECEIVED':
      return {
        ...state,
        status: 'syncing',
        error: null,
      };

    case 'SYNC_START':
      return {
        ...state,
        status: 'syncing',
        error: null,
      };

    case 'SYNC_SUCCESS':
      return {
        ...state,
        status: 'authenticated',
        user: action.user,
        isFirstLogin: action.isFirstLogin,
        error: null,
      };

    case 'SYNC_FAILURE':
      return {
        ...state,
        status: 'unauthenticated',
        user: null,
        isFirstLogin: false,
        error: action.error,
      };

    case 'LOGOUT':
      return {
        ...initialAuthState,
        status: 'unauthenticated',
      };

    case 'SESSION_EXPIRED':
      return {
        ...state,
        status: 'unauthenticated',
        user: null,
        isFirstLogin: false,
        error: {
          code: 'session_expired',
          message: 'Your session has expired. Please sign in again.',
        },
      };

    case 'CLEAR_ERROR':
      return {
        ...state,
        error: null,
      };

    default:
      return state;
  }
}

interface AuthStateContextType {
  state: AuthState;
  dispatch: React.Dispatch<AuthAction>;
  login: () => void;
  logout: () => void;
  handleSyncSuccess: (user: UserProfile, isFirstLogin: boolean) => void;
  handleSyncFailure: (error: AuthError) => void;
  clearError: () => void;
}

const AuthStateContext = createContext<AuthStateContextType | undefined>(undefined);

interface AuthStateProviderProps {
  children: ReactNode;
}

export const AuthStateProvider: React.FC<AuthStateProviderProps> = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, initialAuthState);
  const { loginWithRedirect, logout: auth0Logout, isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
  const { showSuccess, showError } = useToast();
  const hasShownWelcomeToast = useRef(false);
  const [syncUserMutation] = useMutation<RegisterUserMutation>(SYNC_USER);
  const hasSyncedRef = useRef(false);

  // Initialize auth state based on Auth0 status
  useEffect(() => {
    if (isLoading) {
      dispatch({ type: 'INITIALIZE_START' });
    } else if (isAuthenticated && state.status === 'initializing') {
      // Auth0 says authenticated but we need to sync with backend
      dispatch({ type: 'SYNC_START' });
    } else if (isAuthenticated === false && state.status === 'initializing') {
      dispatch({ type: 'INITIALIZE_UNAUTHENTICATED' });
    }
  }, [isLoading, isAuthenticated, state.status]);

  // Sync with backend when status enters 'syncing'
  useEffect(() => {
    if (state.status !== 'syncing' || hasSyncedRef.current) {
      return;
    }

    const syncWithBackend = async () => {
      try {
        await getAccessTokenSilently();
        hasSyncedRef.current = true;

        const result = await syncUserMutation();
        const response = result.data?.registerUserInfo;

        if (response?.__typename === 'UserRegistrationSuccessResponse') {
          const userData = response.data;
          const userProfile: UserProfile = {
            id: userData.userId,
            displayName: userData.displayName,
            email: userData.email,
            createdAt: String(userData.createdAt),
            lastLoginAt: String(userData.lastLoginAt),
          };
          dispatch({ type: 'SYNC_SUCCESS', user: userProfile, isFirstLogin: userData.isFirstLogin });
        } else if (response?.__typename === 'FailureResponse') {
          const errorMessage = response.status?.message || 'Failed to sync user with backend';
          dispatch({ type: 'SYNC_FAILURE', error: { code: 'sync_failed', message: errorMessage } });
        } else {
          dispatch({ type: 'SYNC_FAILURE', error: { code: 'sync_failed', message: 'Received unexpected response from server' } });
        }
      } catch (err) {
        hasSyncedRef.current = false;
        const errorMessage = err instanceof Error ? err.message : 'An unexpected error occurred';

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

        dispatch({ type: 'SYNC_FAILURE', error: { code: 'sync_failed', message: errorMessage } });
      }
    };

    syncWithBackend();
  }, [state.status, getAccessTokenSilently, syncUserMutation]);

  // Show welcome toast when sync succeeds (only once per authentication)
  useEffect(() => {
    if (state.status === 'authenticated' && state.user && hasShownWelcomeToast.current === false) {
      hasShownWelcomeToast.current = true;
      const message = state.isFirstLogin
        ? `Welcome to MtgDiscovery, ${state.user.displayName}!`
        : `Welcome back, ${state.user.displayName}!`;
      showSuccess(message);
    }
  }, [state.status, state.user, state.isFirstLogin, showSuccess]);

  // Reset flags on logout
  useEffect(() => {
    if (state.status === 'unauthenticated') {
      hasShownWelcomeToast.current = false;
      hasSyncedRef.current = false;
    }
  }, [state.status]);

  // Show error toast when auth fails
  useEffect(() => {
    if (state.error) {
      showError(state.error.message);
    }
  }, [state.error, showError]);

  const login = useCallback(() => {
    dispatch({ type: 'LOGIN_START' });
    loginWithRedirect({
      appState: {
        returnTo: window.location.pathname,
      },
    });
  }, [loginWithRedirect]);

  const logout = useCallback(() => {
    dispatch({ type: 'LOGOUT' });
    auth0Logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    });
  }, [auth0Logout]);

  const handleSyncSuccess = useCallback((user: UserProfile, isFirstLogin: boolean) => {
    dispatch({ type: 'SYNC_SUCCESS', user, isFirstLogin });
  }, []);

  const handleSyncFailure = useCallback((error: AuthError) => {
    dispatch({ type: 'SYNC_FAILURE', error });
  }, []);

  const clearError = useCallback(() => {
    dispatch({ type: 'CLEAR_ERROR' });
  }, []);

  const contextValue: AuthStateContextType = {
    state,
    dispatch,
    login,
    logout,
    handleSyncSuccess,
    handleSyncFailure,
    clearError,
  };

  return (
    <AuthStateContext.Provider value={contextValue}>
      {children}
    </AuthStateContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components -- Standard pattern: export hook with Provider
export const useAuthState = (): AuthStateContextType => {
  const context = useContext(AuthStateContext);
  if (context === undefined) {
    throw new Error('useAuthState must be used within an AuthStateProvider');
  }
  return context;
};
