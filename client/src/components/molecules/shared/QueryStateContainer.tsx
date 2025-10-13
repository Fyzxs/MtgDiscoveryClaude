import React from 'react';
import { Container, CircularProgress, Alert, Box, type ContainerProps } from '@mui/material';
import { handleGraphQLError } from '../../../utils/networkErrorHandler';

interface QueryStateContainerProps {
  loading?: boolean;
  error?: Error | null;
  errorMessage?: string;
  children: React.ReactNode;
  loadingComponent?: React.ReactNode;
  errorComponent?: React.ReactNode;
  containerProps?: Omit<ContainerProps, 'children'>;
  showContainer?: boolean;
}

/**
 * Container that handles loading, error, and success states for queries
 * Reduces boilerplate for handling Apollo Client query states
 */
export const QueryStateContainer: React.FC<QueryStateContainerProps> = ({
  loading = false,
  error = null,
  errorMessage,
  children,
  loadingComponent,
  errorComponent,
  containerProps = {},
  showContainer = true
}) => {
  // Loading state
  if (loading) {
    if (loadingComponent) {
      return <>{loadingComponent}</>;
    }
    
    const loadingContent = (
      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <CircularProgress />
      </Box>
    );

    return showContainer ? (
      <Container maxWidth="lg" {...containerProps}>
        {loadingContent}
      </Container>
    ) : loadingContent;
  }

  // Error state
  if (error || errorMessage) {
    if (errorComponent) {
      return <>{errorComponent}</>;
    }

    let displayMessage = errorMessage;
    if (!displayMessage && error) {
      try {
        const networkError = handleGraphQLError(error);
        displayMessage = networkError.userMessage;
      } catch {
        displayMessage = error.message || 'An error occurred';
      }
    }
    displayMessage = displayMessage || 'An error occurred';

    const errorContent = (
      <Box sx={{ mt: 4 }}>
        <Alert severity="error">
          {displayMessage}
        </Alert>
      </Box>
    );

    return showContainer ? (
      <Container maxWidth="lg" {...containerProps}>
        {errorContent}
      </Container>
    ) : errorContent;
  }

  // Success state - render children
  return <>{children}</>;
};

/**
 * Specialized version for GraphQL responses with __typename checking
 */
interface GraphQLQueryStateContainerProps<T> extends Omit<QueryStateContainerProps, 'errorMessage'> {
  data?: {
    __typename?: string;
    status?: {
      message?: string;
      statusCode?: number;
    };
    data?: T;
  } | null;
  failureTypeName?: string;
  getErrorMessage?: (data: GraphQLQueryStateContainerProps<T>['data']) => string;
}

export function GraphQLQueryStateContainer<T>({
  data,
  loading = false,
  error = null,
  failureTypeName = 'FailureResponse',
  getErrorMessage,
  children,
  ...props
}: GraphQLQueryStateContainerProps<T>) {
  // Check for GraphQL failure response
  let errorMsg: string | undefined;
  
  if (data?.__typename === failureTypeName) {
    errorMsg = getErrorMessage 
      ? getErrorMessage(data)
      : data.status?.message || 'Request failed';
  }

  return (
    <QueryStateContainer
      loading={loading}
      error={error}
      errorMessage={errorMsg}
      {...props}
    >
      {children}
    </QueryStateContainer>
  );
}

/**
 * Hook for managing multiple query states
 */
// eslint-disable-next-line react-refresh/only-export-components -- Utility hook related to QueryStateContainer
export function useQueryStates(queries: Array<{ loading?: boolean; error?: Error | null }>) {
  const isLoading = queries.some(q => q.loading);
  const errors = queries.filter(q => q.error).map(q => q.error);
  const firstError = errors[0] || null;
  
  return {
    isLoading,
    hasError: errors.length > 0,
    firstError,
    errors
  };
}