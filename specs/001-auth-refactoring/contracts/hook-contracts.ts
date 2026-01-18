/**
 * Custom Hook Contracts
 *
 * Hooks for authentication state management
 *
 * Constitution Alignment:
 * - Principle II (Layered Architecture → React): Hooks abstract Auth0 SDK, Apollo, routing
 * - Principle I: Explicit types for all hook return values
 * - Component-local state (no global state races)
 */

import { type StoredUserData } from './utility-contracts';

// ============================================================================
// useAuthCallback Hook
// ============================================================================

/**
 * File: client/src/hooks/auth/useAuthCallback.ts
 * Phase: 2 (Callback Separation)
 *
 * Determines routing after Auth0 callback based on localStorage state.
 */

/**
 * Auth callback status states.
 *
 * Explicit union type (no magic strings).
 */
export type CallbackStatus = 'processing' | 'returning' | 'new-user' | 'error';

/**
 * Result of auth callback processing.
 *
 * Immutable: Hook returns new object on each render.
 */
export interface AuthCallbackResult {
  /** Current authentication status */
  status: CallbackStatus;

  /** Target route for navigation */
  redirectTo: string;
}

/**
 * Processes Auth0 callback and determines user routing.
 *
 * Logic:
 * 1. Wait for Auth0 SDK to load (isLoading)
 * 2. Check authentication status (isAuthenticated)
 * 3. Check localStorage for returning user
 * 4. Return appropriate status and redirect path
 *
 * State Transitions:
 * - Initial: { status: 'processing', redirectTo: '/' }
 * - Error: { status: 'error', redirectTo: '/' }
 * - Returning: { status: 'returning', redirectTo: '/' }
 * - New User: { status: 'new-user', redirectTo: '/signin-redirect' }
 *
 * @returns Auth callback result with status and redirect path
 *
 * Dependencies:
 * - useAuth0() from @auth0/auth0-react
 * - isReturningUser() from userStorage
 * - useState, useEffect from React
 *
 * Constitution:
 * - Component-local state (no global subscription)
 * - useEffect with proper dependencies (no race conditions)
 * - Explicit error handling
 */
export function useAuthCallback(): AuthCallbackResult;

// ============================================================================
// useTokenReady Hook
// ============================================================================

/**
 * File: client/src/hooks/auth/useTokenReady.ts
 * Phase: 3 (Token Simplification)
 *
 * Replaces global token subscription with component-local verification.
 */

/**
 * Token readiness state.
 *
 * Mutually exclusive states:
 * - isWaiting=true: Verifying token
 * - isReady=true: Token verified, ready for GraphQL
 * - error!=null: Verification failed
 */
export interface TokenReadyState {
  /** Token successfully verified and ready for use */
  isReady: boolean;

  /** Token verification in progress */
  isWaiting: boolean;

  /** Error message if verification failed, null otherwise */
  error: string | null;
}

/**
 * Verifies Auth0 token readiness for component.
 *
 * Logic:
 * 1. Check if user is authenticated
 * 2. If authenticated, call getAccessTokenSilently() to verify token
 * 3. Update state based on success/failure
 *
 * State Machine:
 * - Initial: { isReady: false, isWaiting: true, error: null }
 * - Not Authenticated: { isReady: false, isWaiting: false, error: null }
 * - Token Ready: { isReady: true, isWaiting: false, error: null }
 * - Token Failed: { isReady: false, isWaiting: false, error: "message" }
 *
 * @returns Current token readiness state
 *
 * Dependencies:
 * - useAuth0() from @auth0/auth0-react
 * - useState, useEffect from React
 *
 * Constitution:
 * - Component-local (each component verifies independently)
 * - Auth0 SDK handles token caching (no performance penalty)
 * - Replaces global BehaviorSubject subscription
 */
export function useTokenReady(): TokenReadyState;

// ============================================================================
// useUserSync Hook (Modified)
// ============================================================================

/**
 * File: client/src/hooks/user/useUserSync.ts
 * Phase: 4 (Caching Optimization)
 *
 * Modified to use cache-first Apollo policy.
 */

/**
 * User sync hook query options (partial - relevant changes only).
 *
 * Changes in Phase 4:
 * - fetchPolicy: 'cache-first' (was: default/network-only)
 * - nextFetchPolicy: 'cache-and-network' (background refresh)
 * - errorPolicy: 'all' (unchanged)
 */
export interface UserSyncQueryOptions {
  /** Skip query if user info not needed */
  skip?: boolean;

  /** Fetch policy - use cache first for performance */
  fetchPolicy: 'cache-first';

  /** Next fetch policy - refresh in background */
  nextFetchPolicy: 'cache-and-network';

  /** Error policy - return partial data on error */
  errorPolicy: 'all';
}

/**
 * Note: Full useUserSync contract not shown here as only Apollo query options change.
 * The hook signature and return type remain unchanged from existing implementation.
 * See existing client/src/hooks/user/useUserSync.ts for full contract.
 *
 * Constitution:
 * - Optimization only (no behavioral changes)
 * - Reduces network requests for returning users
 * - Apollo handles cache invalidation
 */

// ============================================================================
// Hook Testing Contracts
// ============================================================================

/**
 * Test utilities for hooks.
 *
 * Note: Frontend testing uses Vitest + React Testing Library (not MSTest).
 * Constitution Principle III applies differently to frontend:
 * - Self-contained tests (no shared state)
 * - Arrange-Act-Assert pattern
 * - Mock Auth0/Apollo with test utilities
 */

/**
 * Mock Auth0 hook return for testing.
 *
 * Usage in tests:
 * ```typescript
 * vi.mock('@auth0/auth0-react', () => ({
 *   useAuth0: () => mockAuth0Result
 * }));
 * ```
 */
export interface MockAuth0Result {
  isLoading: boolean;
  isAuthenticated: boolean;
  user?: {
    sub?: string;
    email?: string;
    name?: string;
    picture?: string;
  };
  error?: Error;
  getAccessTokenSilently: () => Promise<string>;
  loginWithRedirect: () => Promise<void>;
}

/**
 * Test helper: Create mock Auth0 result.
 *
 * @param overrides - Partial overrides for default mock
 * @returns Complete mock Auth0 result
 */
export function createMockAuth0Result(overrides?: Partial<MockAuth0Result>): MockAuth0Result;

/**
 * Test helper: Create mock stored user data.
 *
 * @param overrides - Partial overrides for default data
 * @returns Complete StoredUserData for tests
 */
export function createMockStoredUserData(overrides?: Partial<StoredUserData>): StoredUserData;
