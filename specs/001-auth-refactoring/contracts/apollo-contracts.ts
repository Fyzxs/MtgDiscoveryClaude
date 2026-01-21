/**
 * Apollo Client Configuration Contracts
 *
 * Updates to Apollo Client for caching optimization
 *
 * Constitution Alignment:
 * - Principle II (Layered Architecture → React): Apollo as data layer
 * - Performance optimization (cache-first policy)
 * - No behavioral changes (only caching strategy)
 */

import { type InMemoryCacheConfig, type TypePolicy } from '@apollo/client';

// ============================================================================
// Apollo Client Modifications
// ============================================================================

/**
 * File: client/src/graphql/apollo-client.ts
 * Phases: 3 (Token Simplification), 4 (Caching Optimization)
 */

// ----------------------------------------------------------------------------
// Phase 3: Remove Token Subscription System
// ----------------------------------------------------------------------------

/**
 * Token getter function type.
 *
 * Simplified from old subscription system.
 */
export type TokenGetter = () => Promise<string | null>;

/**
 * Global token getter (set by Auth0TokenProvider).
 *
 * Usage:
 * ```typescript
 * let getAuth0Token: TokenGetter | null = null;
 *
 * export const setAuth0TokenGetter = (tokenGetter: TokenGetter) => {
 *   getAuth0Token = tokenGetter;
 * };
 * ```
 *
 * Changes in Phase 3:
 * - Remove BehaviorSubject (lines 11-56)
 * - Keep only tokenGetter registration
 * - Auth link uses getAuth0Token directly (unchanged)
 */
export function setAuth0TokenGetter(tokenGetter: TokenGetter): void;

/**
 * Note: authLink usage remains unchanged.
 * Only token readiness notification system is removed.
 */

// ----------------------------------------------------------------------------
// Phase 4: Cache Type Policies
// ----------------------------------------------------------------------------

/**
 * User info type policy for Apollo cache.
 *
 * Purpose:
 * - Define merge strategy for userInfo query
 * - Always prefer fresh data over cached data
 * - Enable cache-first policy without stale data risk
 */
export const userInfoTypePolicy: TypePolicy = {
  merge(existing, incoming) {
    // Always prefer incoming (fresh) data
    return incoming ?? existing;
  },
};

/**
 * Complete InMemoryCache configuration.
 *
 * Includes type policies for all queries/mutations.
 */
export interface ApolloInMemoryCacheConfig extends InMemoryCacheConfig {
  typePolicies: {
    Query: {
      fields: {
        /** User info query caching policy */
        userInfo: TypePolicy;
      };
    };
  };
}

/**
 * Apollo InMemoryCache configuration with type policies.
 *
 * Usage:
 * ```typescript
 * const cache = new InMemoryCache({
 *   typePolicies: {
 *     Query: {
 *       fields: {
 *         userInfo: {
 *           merge(existing, incoming) {
 *             return incoming ?? existing;
 *           },
 *         },
 *       },
 *     },
 *   },
 * });
 * ```
 *
 * Constitution:
 * - Optimization only (no breaking changes)
 * - Explicit merge strategy (not implicit)
 */

// ============================================================================
// Query Options Modifications
// ============================================================================

/**
 * File: client/src/hooks/user/useUserSync.ts
 * Phase: 4 (Caching Optimization)
 */

/**
 * User info query options with cache-first policy.
 *
 * Changes:
 * - fetchPolicy: 'cache-first' (was: default or 'network-only')
 * - nextFetchPolicy: 'cache-and-network' (background refresh)
 * - errorPolicy: 'all' (unchanged)
 *
 * Usage:
 * ```typescript
 * const { data, loading, error } = useQuery<UserInfoQueryData>(GET_USER_INFO, {
 *   skip: shouldQueryUserInfo === false,
 *   fetchPolicy: 'cache-first',
 *   nextFetchPolicy: 'cache-and-network',
 *   errorPolicy: 'all'
 * });
 * ```
 */
export interface UserInfoQueryOptions {
  skip?: boolean;
  fetchPolicy: 'cache-first';
  nextFetchPolicy: 'cache-and-network';
  errorPolicy: 'all';
}

// ============================================================================
// GraphQL Query/Mutation Contracts
// ============================================================================

/**
 * Note: GraphQL queries and mutations are generated via codegen.
 *
 * Constitution Principle VI (Frontend Standards):
 * - DO NOT manually write GraphQL types
 * - Use npm run codegen after schema changes
 * - Import from client/src/generated/
 *
 * Example:
 * ```typescript
 * import { useGetUserInfoQuery } from '../generated/graphql';
 * ```
 */

/**
 * GraphQL codegen workflow:
 * 1. Update .graphql files in client/src/graphql/
 * 2. Run: npm run codegen
 * 3. Import generated hooks from client/src/generated/
 * 4. Do NOT manually create GraphQL types
 *
 * Relevant queries for this feature:
 * - GET_USER_INFO (existing, modified query options only)
 * - REGISTER_USER (existing, unchanged)
 */

// ============================================================================
// Testing Contracts
// ============================================================================

/**
 * Mock Apollo Client for testing.
 *
 * Usage in tests:
 * ```typescript
 * import { MockedProvider } from '@apollo/client/testing';
 *
 * const mocks = [
 *   {
 *     request: { query: GET_USER_INFO },
 *     result: { data: { userInfo: {...} } }
 *   }
 * ];
 *
 * render(
 *   <MockedProvider mocks={mocks}>
 *     <ComponentUnderTest />
 *   </MockedProvider>
 * );
 * ```
 */

/**
 * Mock query result for GET_USER_INFO.
 *
 * Used in component/hook tests.
 */
export interface MockUserInfoQueryResult {
  userInfo: {
    userId: string;
    displayName?: string;
    // ...other fields
  };
}

/**
 * Test helper: Create mock Apollo query result.
 *
 * @param overrides - Partial overrides for mock data
 * @returns Complete mock query result
 */
export function createMockUserInfoResult(
  overrides?: Partial<MockUserInfoQueryResult>
): MockUserInfoQueryResult;

// ============================================================================
// Performance Monitoring
// ============================================================================

/**
 * Apollo Client performance monitoring.
 *
 * Track cache hit/miss rates for optimization validation.
 */

/**
 * Cache performance metrics.
 */
export interface CachePerformanceMetrics {
  /** Total cache reads */
  cacheReads: number;

  /** Cache hits (data found in cache) */
  cacheHits: number;

  /** Cache misses (network fetch required) */
  cacheMisses: number;

  /** Cache hit rate (hits / reads) */
  hitRate: number;
}

/**
 * Log cache performance for debugging.
 *
 * Usage:
 * ```typescript
 * cache.watch({
 *   query: GET_USER_INFO,
 *   callback: (result) => {
 *     logger.debug('Cache access', {
 *       fromCache: result.fromCache,
 *       query: 'GET_USER_INFO'
 *     });
 *   }
 * });
 * ```
 *
 * Note: Implementation details may vary.
 * Primary goal: Verify cache-first policy reduces network calls.
 */
