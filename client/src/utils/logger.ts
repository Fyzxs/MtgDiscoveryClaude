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

    // TODO: Integrate with error tracking service (e.g., Sentry)
    // Sentry.captureException(args[0]);
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
  }
};
