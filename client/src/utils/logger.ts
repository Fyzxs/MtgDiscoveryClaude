/**
 * Production-safe logger utility
 * Only logs debug/info messages in development environment
 */

const isDevelopment = import.meta.env.DEV;

export const logger = {
  /**
   * Debug level logging - only shown in development
   */
  debug: (...args: unknown[]) => {
    if (isDevelopment) {
      console.debug('[DEBUG]', ...args);
    }
  },

  /**
   * Info level logging - only shown in development
   */
  info: (...args: unknown[]) => {
    if (isDevelopment) {
      console.info('[INFO]', ...args);
    }
  },

  /**
   * Warning level logging - always shown
   */
  warn: (...args: unknown[]) => {
    console.warn('[WARN]', ...args);
  },

  /**
   * Error level logging - always shown
   * Can be integrated with error tracking services
   */
  error: (...args: unknown[]) => {
    console.error('[ERROR]', ...args);
  },

  /**
   * Performance logging - only in development
   */
  perf: (label: string, fn: () => void) => {
    if (isDevelopment) {
      console.time(`[PERF] ${label}`);
      fn();
      console.timeEnd(`[PERF] ${label}`);
    } else {
      fn();
    }
  },

  /**
   * Create a context-specific logger with prefixed messages
   *
   * @param context - The context name (e.g., component name, module name)
   * @returns Logger instance with context prefix
   *
   * @example
   * ```typescript
   * const cardLogger = logger.createContext('CardGrid');
   * cardLogger.debug('Rendering cards:', cards.length);
   * // Output: [DEBUG] [CardGrid] Rendering cards: 42
   * ```
   */
  createContext: (context: string) => ({
    debug: (...args: unknown[]) => logger.debug(`[${context}]`, ...args),
    info: (...args: unknown[]) => logger.info(`[${context}]`, ...args),
    warn: (...args: unknown[]) => logger.warn(`[${context}]`, ...args),
    error: (...args: unknown[]) => logger.error(`[${context}]`, ...args),
  }),

  /**
   * GraphQL operation logging - only in development
   */
  graphql: {
    query: (operationName: string, variables?: unknown) => {
      if (isDevelopment) {
        console.debug('[GraphQL Query]', operationName, variables);
      }
    },
    mutation: (operationName: string, variables?: unknown) => {
      if (isDevelopment) {
        console.debug('[GraphQL Mutation]', operationName, variables);
      }
    },
    error: (operationName: string, error: unknown) => {
      console.error('[GraphQL Error]', operationName, error);
    },
  },
};

/**
 * Error tracking service stub
 * Currently logs using the logger utility, ready for integration with services like Sentry
 */
export const errorTracking = {
  /**
   * Capture an exception for error tracking
   * @param error - The error to track
   * @param context - Additional context information
   */
  captureException: (error: unknown, context?: Record<string, unknown>) => {
    logger.error('[ErrorTracking] Exception:', error);
    if (context) {
      logger.error('[ErrorTracking] Context:', context);
    }
    // Future: integrate with Sentry or similar service
    // Sentry.captureException(error, { extra: context });
  },

  /**
   * Capture a message for error tracking
   * @param message - The message to track
   * @param level - Severity level
   * @param context - Additional context information
   */
  captureMessage: (
    message: string,
    level: 'info' | 'warning' | 'error' = 'info',
    context?: Record<string, unknown>
  ) => {
    const contextStr = context ? JSON.stringify(context) : '';

    if (level === 'error') {
      logger.error(`[ErrorTracking] ${message}`, contextStr);
    } else if (level === 'warning') {
      logger.warn(`[ErrorTracking] ${message}`, contextStr);
    } else {
      logger.info(`[ErrorTracking] ${message}`, contextStr);
    }
    // Future: integrate with Sentry or similar service
    // Sentry.captureMessage(message, { level, extra: context });
  },
};
