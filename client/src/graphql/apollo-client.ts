import { ApolloClient, InMemoryCache, createHttpLink, ApolloLink } from '@apollo/client';
import { onError } from '@apollo/client/link/error';
import { logger } from '../utils/logger';
import { setContext } from '@apollo/client/link/context';

// In development, use the Vite proxy to avoid CORS issues with self-signed certs
// In production, use the environment variable or default endpoint
const httpLink = createHttpLink({
  uri: import.meta.env.VITE_GRAPHQL_ENDPOINT ||
       (import.meta.env.DEV ? '/graphql' : 'https://localhost:65203/graphql'),
});

// Auth0 token getter - will be set by Auth0Provider
let getAuth0Token: (() => Promise<string | null>) | null = null;
let isTokenReady = false;

// Event subscribers for token ready state changes
type TokenReadyCallback = (ready: boolean) => void;
const tokenReadySubscribers: Set<TokenReadyCallback> = new Set();

// Function to set the token getter from React context
export const setAuth0TokenGetter = (tokenGetter: () => Promise<string | null>) => {
  getAuth0Token = tokenGetter;
  logger.debug('Apollo Client - Token getter registered');
};

// Function to set the token ready state
export const setTokenReadyState = (ready: boolean) => {
  const previousState = isTokenReady;
  isTokenReady = ready;
  logger.debug('Apollo Client - Token ready state:', ready);

  // Notify all subscribers if state changed
  if (previousState !== ready) {
    tokenReadySubscribers.forEach(callback => {
      try {
        callback(ready);
      } catch (error) {
        logger.error('Apollo Client - Error in token ready callback:', error);
      }
    });
  }
};

// Function to check if token is ready
export const getTokenReadyState = (): boolean => {
  return isTokenReady;
};

// Subscribe to token ready state changes
export const subscribeToTokenReady = (callback: TokenReadyCallback): (() => void) => {
  tokenReadySubscribers.add(callback);
  // Immediately call with current state
  callback(isTokenReady);
  // Return unsubscribe function
  return () => {
    tokenReadySubscribers.delete(callback);
  };
};

const authLink = setContext(async (operation, { headers }) => {
  const operationName = operation.operationName || 'unknown';

  if (getAuth0Token === null) {
    logger.warn(`[AUTH] Request "${operationName}" - No token getter registered, sending without auth`);
    return { headers };
  }

  try {
    const tokenStart = performance.now();
    const idToken = await getAuth0Token();
    const tokenEnd = performance.now();
    logger.debug(`[AUTH] Request "${operationName}" - Token acquisition took ${tokenEnd - tokenStart}ms, got token: ${idToken ? 'yes' : 'no'}`);

    if (idToken) {
      logger.debug(`[AUTH] Request "${operationName}" - Attaching Bearer token`);
      return {
        headers: {
          ...headers,
          authorization: `Bearer ${idToken}`
        }
      };
    }

    logger.warn(`[AUTH] Request "${operationName}" - Token getter returned null/empty`);
  } catch (error) {
    logger.error(`[AUTH] Request "${operationName}" - Token acquisition failed:`, error);
  }

  return { headers };
});

// Track network connectivity to avoid spamming requests when backend is down
let networkErrorCount = 0;
let lastNetworkErrorTime = 0;
const NETWORK_ERROR_THRESHOLD = 3;
const NETWORK_ERROR_WINDOW_MS = 10000; // 10 seconds

// Export for components that need to check network status
export const isNetworkAvailable = (): boolean => {
  const now = Date.now();
  // Reset counter if enough time has passed
  if (now - lastNetworkErrorTime > NETWORK_ERROR_WINDOW_MS) {
    networkErrorCount = 0;
  }
  return networkErrorCount < NETWORK_ERROR_THRESHOLD;
};

// Reset network status (call when user explicitly retries)
export const resetNetworkStatus = (): void => {
  networkErrorCount = 0;
  lastNetworkErrorTime = 0;
  logger.debug('[Network] Status reset - will retry connections');
};

// Error link to handle and enhance GraphQL errors
const errorLink = onError(({ graphQLErrors, networkError, operation, forward }) => {
  if (graphQLErrors) {
    graphQLErrors.forEach((err) => {
      const extensionMessage = err.extensions?.message as string | undefined;

      // Detect Cosmos DB emulator errors
      if (extensionMessage?.toLowerCase().includes('localhost:8081') &&
          (extensionMessage.toLowerCase().includes('connection') ||
           extensionMessage.toLowerCase().includes('refused'))) {

        // Enhance the error message
        err.message = '🔌 Database connection failed. Did you forget to start the Cosmos DB emulator?';
        logger.error('[GraphQL Error] Cosmos DB emulator not running:', {
          operation: operation.operationName,
          originalMessage: extensionMessage
        });
      } else {
        logger.error('[GraphQL Error]:', {
          message: err.message,
          operation: operation.operationName,
          path: err.path,
          extensions: err.extensions
        });
      }
    });
  }

  if (networkError) {
    const now = Date.now();
    // Reset counter if enough time has passed since last error
    if (now - lastNetworkErrorTime > NETWORK_ERROR_WINDOW_MS) {
      networkErrorCount = 0;
    }
    networkErrorCount++;
    lastNetworkErrorTime = now;

    // Only log the first few errors to avoid spam
    if (networkErrorCount <= NETWORK_ERROR_THRESHOLD) {
      logger.error('[Network Error]:', networkError);
    } else if (networkErrorCount === NETWORK_ERROR_THRESHOLD + 1) {
      logger.warn('[Network] Backend appears to be down - suppressing further connection error logs');
    }
  }

  return forward(operation);
});

export const apolloClient = new ApolloClient({
  link: ApolloLink.from([errorLink, authLink, httpLink]),
  cache: new InMemoryCache({
    typePolicies: {
      Query: {
        fields: {
          cards: {
            merge(existing = [], incoming) {
              return [...existing, ...incoming];
            },
          },
          sets: {
            merge(existing = [], incoming) {
              return [...existing, ...incoming];
            },
          },
          collection: {
            merge(existing = [], incoming) {
              return [...existing, ...incoming];
            },
          },
        },
      },
      Card: {
        fields: {
          userCollection: {
            // Replace the entire array when mutation returns new collection data
            merge(_existing, incoming) {
              return incoming;
            },
          },
        },
      },
      // Prevent normalization of Set-related nested objects to avoid cache merging issues
      // These types don't have unique IDs, so they should be embedded in their parent
      SetGrouping: {
        keyFields: false,
      },
      SetGroupingFinishCounts: {
        keyFields: false,
      },
      SetInformationOutEntity: {
        keyFields: false,
      },
      UserSetCardCollecting: {
        keyFields: false,
      },
      FinishCounts: {
        keyFields: false,
      },
      UserSetCardCollectionGroup: {
        keyFields: false,
      },
      UserSetCardGroup: {
        keyFields: false,
      },
      UserSetCardFinishGroup: {
        keyFields: false,
      },
    },
  }),
  defaultOptions: {
    watchQuery: {
      fetchPolicy: 'cache-and-network',
      errorPolicy: 'all'
    },
    query: {
      errorPolicy: 'all'
    },
    mutate: {
      errorPolicy: 'all',
      awaitRefetchQueries: false,  // Don't wait for queries to refetch
      refetchQueries: []  // Don't automatically refetch any queries
    }
  },
});