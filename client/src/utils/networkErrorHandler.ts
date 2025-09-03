/**
 * Comprehensive network error handling utilities
 * Provides retry logic, user-friendly error messages, and error classification
 */

export interface NetworkErrorOptions {
  retries?: number;
  retryDelay?: number;
  retryBackoff?: boolean;
  timeout?: number;
  onRetry?: (attemptNumber: number, error: Error) => void;
}

export interface NetworkError extends Error {
  code?: string;
  status?: number;
  isNetworkError: boolean;
  isRetryable: boolean;
  userMessage: string;
}

/**
 * Creates a NetworkError with user-friendly messaging
 */
export const createNetworkError = (
  originalError: Error, 
  status?: number
): NetworkError => {
  const networkError = originalError as NetworkError;
  networkError.isNetworkError = true;
  networkError.status = status;

  // Determine if the error is retryable
  networkError.isRetryable = isRetryableError(status, originalError);

  // Generate user-friendly message
  networkError.userMessage = getUserFriendlyErrorMessage(status, originalError);

  return networkError;
};

/**
 * Determines if an error should be retried
 */
export const isRetryableError = (status?: number, error?: Error): boolean => {
  // Network/connection errors are retryable
  if (error?.name === 'TypeError' && error.message.includes('fetch')) {
    return true;
  }

  // Specific HTTP status codes that are retryable
  if (status) {
    return [408, 429, 500, 502, 503, 504].includes(status);
  }

  return false;
};

/**
 * Generates user-friendly error messages based on error type
 */
export const getUserFriendlyErrorMessage = (status?: number, error?: Error): string => {
  // Network connectivity issues
  if (error?.name === 'TypeError' && error.message.includes('fetch')) {
    return 'Unable to connect to the server. Please check your internet connection and try again.';
  }

  // HTTP status codes
  if (status) {
    switch (status) {
      case 400:
        return 'The request was invalid. Please refresh the page and try again.';
      case 401:
        return 'Authentication required. Please log in and try again.';
      case 403:
        return 'You do not have permission to access this resource.';
      case 404:
        return 'The requested information could not be found.';
      case 408:
        return 'The request timed out. Please try again.';
      case 429:
        return 'Too many requests. Please wait a moment and try again.';
      case 500:
        return 'A server error occurred. Please try again later.';
      case 502:
      case 503:
      case 504:
        return 'The service is temporarily unavailable. Please try again in a few moments.';
      default:
        return `An error occurred (${status}). Please try again.`;
    }
  }

  // Generic fallback
  return error?.message || 'An unexpected error occurred. Please try again.';
};

/**
 * Delays execution for the specified number of milliseconds
 */
const delay = (ms: number): Promise<void> => 
  new Promise(resolve => setTimeout(resolve, ms));

/**
 * Calculates retry delay with optional exponential backoff
 */
const calculateRetryDelay = (
  attemptNumber: number, 
  baseDelay: number, 
  useBackoff: boolean
): number => {
  return useBackoff ? baseDelay * Math.pow(2, attemptNumber - 1) : baseDelay;
};

/**
 * Fetches data with retry logic and comprehensive error handling
 */
export const fetchWithRetry = async <T>(
  url: string,
  options: RequestInit & NetworkErrorOptions = {}
): Promise<T> => {
  const {
    retries = 3,
    retryDelay = 1000,
    retryBackoff = true,
    timeout = 10000,
    onRetry,
    ...fetchOptions
  } = options;

  // Add timeout to fetch options
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeout);

  const fetchOptionsWithTimeout: RequestInit = {
    ...fetchOptions,
    signal: controller.signal
  };

  for (let attempt = 1; attempt <= retries + 1; attempt++) {
    try {
      const response = await fetch(url, fetchOptionsWithTimeout);
      clearTimeout(timeoutId);

      if (!response.ok) {
        const error = createNetworkError(
          new Error(`HTTP ${response.status}: ${response.statusText}`),
          response.status
        );

        // If not retryable or on last attempt, throw the error
        if (!error.isRetryable || attempt > retries) {
          throw error;
        }

        // Otherwise, continue to retry logic below
        throw error;
      }

      // Success - parse and return response
      const contentType = response.headers.get('content-type');
      if (contentType?.includes('application/json')) {
        return await response.json();
      } else {
        return await response.text() as unknown as T;
      }

    } catch (error) {
      clearTimeout(timeoutId);
      
      const networkError = error instanceof Error 
        ? createNetworkError(error)
        : createNetworkError(new Error('Unknown error occurred'));

      // On last attempt, throw the error
      if (attempt > retries) {
        throw networkError;
      }

      // Check if error is retryable
      if (!networkError.isRetryable) {
        throw networkError;
      }

      // Call retry callback if provided
      if (onRetry) {
        onRetry(attempt, networkError);
      }

      // Wait before retrying
      const delayTime = calculateRetryDelay(attempt, retryDelay, retryBackoff);
      await delay(delayTime);
    }
  }

  // This should never be reached, but TypeScript requires it
  throw createNetworkError(new Error('Maximum retry attempts exceeded'));
};

/**
 * GraphQL-specific error handling
 */
export const handleGraphQLError = (error: any): NetworkError => {
  // Handle Apollo Client errors
  if (error.networkError) {
    return createNetworkError(
      error.networkError,
      error.networkError.statusCode
    );
  }

  // Handle GraphQL errors
  if (error.graphQLErrors?.length > 0) {
    const graphQLError = error.graphQLErrors[0];
    const networkError = createNetworkError(new Error(graphQLError.message));
    networkError.userMessage = 'Failed to load data. Please try refreshing the page.';
    networkError.isRetryable = false; // GraphQL errors are typically not retryable
    return networkError;
  }

  // Generic error
  return createNetworkError(new Error(error.message || 'GraphQL query failed'));
};

/**
 * Enhanced loading state manager
 */
export class LoadingStateManager {
  private loadingStates = new Map<string, boolean>();
  private subscribers = new Set<(loadingStates: Record<string, boolean>) => void>();

  setLoading(key: string, isLoading: boolean): void {
    this.loadingStates.set(key, isLoading);
    this.notifySubscribers();
  }

  isLoading(key: string): boolean {
    return this.loadingStates.get(key) || false;
  }

  isAnyLoading(): boolean {
    return Array.from(this.loadingStates.values()).some(Boolean);
  }

  subscribe(callback: (loadingStates: Record<string, boolean>) => void): () => void {
    this.subscribers.add(callback);
    return () => this.subscribers.delete(callback);
  }

  private notifySubscribers(): void {
    const loadingStates = Object.fromEntries(this.loadingStates);
    this.subscribers.forEach(callback => callback(loadingStates));
  }

  clear(): void {
    this.loadingStates.clear();
    this.notifySubscribers();
  }
}

/**
 * Default loading state manager instance
 */
export const globalLoadingManager = new LoadingStateManager();