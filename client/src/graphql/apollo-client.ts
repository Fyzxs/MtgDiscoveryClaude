import { ApolloClient, InMemoryCache, createHttpLink, ApolloLink } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';

const httpLink = createHttpLink({
  uri: 'https://localhost:65203/graphql',
});

// Auth0 token getter - will be set by Auth0Provider
let getAuth0Token: (() => Promise<string | null>) | null = null;
let isTokenReady = false;

// Function to set the token getter from React context
export const setAuth0TokenGetter = (tokenGetter: () => Promise<string | null>) => {
  getAuth0Token = tokenGetter;
};

// Function to set the token ready state
export const setTokenReadyState = (ready: boolean) => {
  isTokenReady = ready;
  console.log('Apollo Client - Token ready state:', ready);
};

// Function to check if token is ready
export const getTokenReadyState = (): boolean => {
  return isTokenReady;
};

const authLink = setContext(async (_, { headers }) => {
  if (getAuth0Token) {
    try {
      const idToken = await getAuth0Token();
      if (idToken) {
        return {
          headers: {
            ...headers,
            authorization: `Bearer ${idToken}`
          }
        };
      }
    } catch (error) {
      console.error('Auth0 ID token acquisition failed:', error);
    }
  }

  return { headers };
});

export const apolloClient = new ApolloClient({
  link: ApolloLink.from([authLink, httpLink]),
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
    },
  }),
  defaultOptions: {
    watchQuery: {
      fetchPolicy: 'cache-and-network',
      errorPolicy: 'all'
    },
    query: {
      errorPolicy: 'all'
    }
  },
});