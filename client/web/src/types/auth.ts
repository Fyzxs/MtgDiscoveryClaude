/**
 * Authentication type definitions for MtgDiscovery
 * Based on data-model.md from 002-auth-flow-refactor spec
 */

/**
 * Authentication status states for the auth state machine
 */
export type AuthStatus =
  | 'initializing'
  | 'unauthenticated'
  | 'authenticating'
  | 'syncing'
  | 'authenticated';

/**
 * Error codes for authentication failures
 */
export type AuthErrorCode =
  | 'auth_failed'
  | 'sync_failed'
  | 'network_error'
  | 'session_expired';

/**
 * Error information for authentication failures
 */
export interface AuthError {
  code: AuthErrorCode;
  message: string;
}

/**
 * Client-side representation of the authenticated user
 * Mapped from backend UserSyncResponse
 */
export interface UserProfile {
  id: string;
  displayName: string;
  email?: string;
  avatarUrl?: string;
  createdAt: string;
  lastLoginAt: string;
}

/**
 * Authentication state for the React context
 */
export interface AuthState {
  status: AuthStatus;
  user: UserProfile | null;
  isFirstLogin: boolean;
  error: AuthError | null;
}

/**
 * Actions for the auth state reducer
 */
export type AuthAction =
  | { type: 'INITIALIZE_START' }
  | { type: 'INITIALIZE_AUTHENTICATED'; user: UserProfile }
  | { type: 'INITIALIZE_UNAUTHENTICATED' }
  | { type: 'LOGIN_START' }
  | { type: 'CALLBACK_RECEIVED' }
  | { type: 'SYNC_START' }
  | { type: 'SYNC_SUCCESS'; user: UserProfile; isFirstLogin: boolean }
  | { type: 'SYNC_FAILURE'; error: AuthError }
  | { type: 'LOGOUT' }
  | { type: 'SESSION_EXPIRED' }
  | { type: 'CLEAR_ERROR' };

/**
 * Initial auth state
 */
export const initialAuthState: AuthState = {
  status: 'initializing',
  user: null,
  isFirstLogin: false,
  error: null,
};

/**
 * Type guard to check if auth status allows navigation
 */
export function isAuthenticatedStatus(status: AuthStatus): boolean {
  return status === 'authenticated';
}

/**
 * Type guard to check if auth is still loading
 */
export function isAuthLoadingStatus(status: AuthStatus): boolean {
  return status === 'initializing' || status === 'authenticating' || status === 'syncing';
}
