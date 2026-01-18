/**
 * Component Props Contracts
 *
 * React component prop interfaces
 *
 * Constitution Alignment:
 * - Principle I: Explicit interface for every component's props
 * - Principle VI: Material-UI sx props, TypeScript interfaces
 * - Frontend Standards: React 19 patterns, atomic design
 */

import { type SxProps, type Theme } from '@mui/material';
import { type AuthCallbackResult } from './hook-contracts';

// ============================================================================
// AuthCallbackPage Component
// ============================================================================

/**
 * File: client/src/components/pages/AuthCallbackPage.tsx
 * Phase: 2 (Callback Separation)
 *
 * Fast callback page that routes users based on localStorage state.
 */

/**
 * AuthCallbackPage props.
 *
 * Note: No props needed - uses hooks internally.
 */
export interface AuthCallbackPageProps {
  // No props - page uses useAuthCallback() and useNavigate() internally
}

/**
 * Auth callback page component.
 *
 * Behavior:
 * 1. Calls useAuthCallback() hook
 * 2. Shows loading indicator while processing
 * 3. Shows "Welcome back!" message for returning users
 * 4. Navigates to appropriate route based on status
 *
 * Status Messages:
 * - processing: "Completing sign-in..."
 * - returning: "Welcome back!"
 * - new-user: "Completing sign-in..." (brief, then navigate)
 * - error: "Completing sign-in..." (navigate to home for re-auth)
 *
 * @param props - Component props (empty)
 * @returns React element
 *
 * Dependencies:
 * - useAuthCallback() hook
 * - useNavigate() from react-router-dom
 * - Material-UI components (Box, CircularProgress, Typography)
 *
 * Constitution:
 * - Material-UI sx props for styling (not Tailwind)
 * - TypeScript props interface
 * - Composition over complexity
 */
export function AuthCallbackPage(props: AuthCallbackPageProps): JSX.Element;

// ============================================================================
// SignInRedirectPage Component (Modified)
// ============================================================================

/**
 * File: client/src/components/pages/SignInRedirectPage.tsx
 * Phase: 1 (Quick Win - localStorage detection)
 *
 * Modified to check localStorage before registration.
 */

/**
 * SignInRedirectPage props.
 *
 * Note: No props needed - uses hooks internally.
 */
export interface SignInRedirectPageProps {
  // No props - page uses useAuth0(), useNavigate(), etc.
}

/**
 * Sign-in redirect page modifications (Phase 1).
 *
 * New Logic (added at start of handleUserSetup):
 * ```typescript
 * if (isReturningUser(user.sub ?? '')) {
 *   logger.debug('Returning user detected, skipping registration');
 *   updateLastLogin();
 *   setSetupStatus('complete');
 *   setStatusMessage('Welcome back! Redirecting...');
 *   setTimeout(() => {
 *     navigate('/', { replace: true });
 *   }, 500);
 *   return;
 * }
 * ```
 *
 * Existing logic continues for new users (wait for token, register, save data).
 *
 * Constitution:
 * - Additive change (no breaking modifications)
 * - Early return pattern (guard clause)
 * - Clear status messages
 */

// ============================================================================
// AuthButton Component (Modified)
// ============================================================================

/**
 * File: client/src/components/auth/AuthButton.tsx
 * Phase: 1 (Quick Win - localStorage detection)
 *
 * Modified to clear localStorage on logout.
 */

/**
 * AuthButton props.
 *
 * Note: Props unchanged - internal logout logic modified.
 */
export interface AuthButtonProps {
  // Existing props (not modified)
}

/**
 * AuthButton modifications (Phase 1).
 *
 * Logout handler modification:
 * ```typescript
 * onClick={() => {
 *   clearUserData(); // NEW: Clear localStorage
 *   logout({ logoutParams: { returnTo: window.location.origin } });
 * }}
 * ```
 *
 * Constitution:
 * - Single responsibility: Clear data before logout
 * - No side effects after logout (data already cleared)
 */

// ============================================================================
// Auth0TokenProvider Component (Modified)
// ============================================================================

/**
 * File: client/src/components/auth/Auth0TokenProvider.tsx
 * Phase: 3 (Token Simplification)
 *
 * Simplified to remove subscription system.
 */

/**
 * Auth0TokenProvider props.
 *
 * Note: Props unchanged.
 */
export interface Auth0TokenProviderProps {
  children: React.ReactNode;
}

/**
 * Auth0TokenProvider modifications (Phase 3).
 *
 * Changes:
 * - Remove BehaviorSubject subscription system (lines 11-56)
 * - Keep only token getter registration
 * - Simpler implementation (no global state)
 *
 * Preserved:
 * - setAuth0TokenGetter() call
 * - Error handling for missing refresh token
 * - loginWithRedirect() on auth errors
 *
 * @param props - Component props
 * @returns React element wrapping children
 *
 * Constitution:
 * - Simplified (removes complexity)
 * - Component-local logic only
 * - No global subscriptions
 */
export function Auth0TokenProvider(props: Auth0TokenProviderProps): JSX.Element;

// ============================================================================
// Material-UI Styling Contracts
// ============================================================================

/**
 * Material-UI sx prop type for components.
 *
 * Constitution Principle VI:
 * - Use sx props (NOT className or Tailwind)
 * - Reference theme colors via theme.palette
 * - Responsive breakpoints via sx={{ width: { xs: '100%', sm: 'auto' } }}
 */
export type MuiSxProp = SxProps<Theme>;

/**
 * Example MUI sx prop usage:
 *
 * ```typescript
 * const loadingBoxSx: MuiSxProp = {
 *   display: 'flex',
 *   flexDirection: 'column',
 *   alignItems: 'center',
 *   justifyContent: 'center',
 *   minHeight: '60vh',
 *   gap: 2,
 *   bgcolor: 'background.default',
 *   color: 'text.primary'
 * };
 * ```
 */

/**
 * Common sx patterns for auth pages.
 */
export const AuthPageSxPatterns = {
  /** Centered loading container */
  loadingContainer: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '60vh',
    gap: 2
  } as MuiSxProp,

  /** Status message typography */
  statusMessage: {
    mt: 2,
    color: 'text.secondary'
  } as MuiSxProp,

  /** Error message typography */
  errorMessage: {
    mt: 2,
    color: 'error.main'
  } as MuiSxProp,

  /** Success message typography */
  successMessage: {
    mt: 2,
    color: 'success.main'
  } as MuiSxProp
} as const;

// ============================================================================
// Component Testing Contracts
// ============================================================================

/**
 * Test utilities for components.
 *
 * Frontend testing pattern (Vitest + React Testing Library):
 * - render() component with mocked dependencies
 * - screen queries to find elements
 * - userEvent for interactions
 * - waitFor() for async assertions
 */

/**
 * Mock navigate function for React Router tests.
 *
 * Usage:
 * ```typescript
 * const mockNavigate = vi.fn();
 * vi.mock('react-router-dom', () => ({
 *   useNavigate: () => mockNavigate
 * }));
 * ```
 */
export type MockNavigate = (to: string, options?: { replace?: boolean }) => void;

/**
 * Test helper: Render component with providers.
 *
 * Wraps component with necessary providers:
 * - Auth0Provider (mocked)
 * - RouterProvider (mocked)
 * - ThemeProvider (Material-UI)
 *
 * @param component - Component to render
 * @param options - Test options (mocks, initial route, etc.)
 * @returns Render result from @testing-library/react
 */
export function renderWithProviders(
  component: React.ReactElement,
  options?: {
    auth0Mock?: Partial<MockAuth0Result>;
    initialRoute?: string;
  }
): RenderResult;

/**
 * Render result from React Testing Library.
 *
 * Note: This is a re-export for type convenience.
 * Actual type comes from @testing-library/react.
 */
export interface RenderResult {
  container: HTMLElement;
  baseElement: HTMLElement;
  debug: (element?: HTMLElement) => void;
  rerender: (ui: React.ReactElement) => void;
  unmount: () => void;
  asFragment: () => DocumentFragment;
}

/**
 * Mock Auth0 result for component tests.
 *
 * Same as hook testing mock.
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
