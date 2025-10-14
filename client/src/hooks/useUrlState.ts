import { useEffect, useCallback, useRef } from 'react';

export interface UrlStateConfig {
  [key: string]: {
    default?: unknown;
    serialize?: (value: unknown) => string | null;
    deserialize?: (value: string) => unknown;
  };
}

/**
 * Hook to synchronize component state with URL parameters
 * 
 * @param state - Current state object to sync with URL
 * @param config - Configuration for each state property
 * @param options - Additional options for URL handling
 */
export function useUrlState(
  state: Record<string, unknown>,
  config: UrlStateConfig,
  options: {
    replace?: boolean; // Use replaceState instead of pushState
    debounceMs?: number; // Debounce URL updates
  } = {}
) {
  const { replace = true, debounceMs = 0 } = options; // Default to no debounce
  const debounceTimerRef = useRef<NodeJS.Timeout | undefined>(undefined);
  const isInitialMount = useRef(true);

  // Get initial values from URL
  const getInitialValues = useCallback(() => {
    const params = new URLSearchParams(window.location.search);
    const initialValues: Record<string, unknown> = {};

    Object.entries(config).forEach(([key, keyConfig]) => {
      const urlValue = params.get(key);
      
      if (urlValue !== null) {
        // Use custom deserializer if provided
        if (keyConfig.deserialize) {
          initialValues[key] = keyConfig.deserialize(urlValue);
        } else if (urlValue.includes(',')) {
          // Default: treat comma-separated as array
          initialValues[key] = urlValue.split(',').filter(Boolean);
        } else {
          initialValues[key] = urlValue;
        }
      } else if (keyConfig.default !== undefined) {
        initialValues[key] = keyConfig.default;
      }
    });

    return initialValues;
  }, [config]);

  // Update URL when state changes
  useEffect(() => {
    // Skip the initial mount to avoid replacing URL immediately
    if (isInitialMount.current) {
      isInitialMount.current = false;
      return;
    }

    const updateUrl = () => {
      const params = new URLSearchParams(window.location.search);

      Object.entries(state).forEach(([key, value]) => {
        const keyConfig = config[key];
        if (!keyConfig) return;

        // Use custom serializer if provided
        let serializedValue: string | null = null;
        
        if (keyConfig.serialize) {
          serializedValue = keyConfig.serialize(value);
        } else if (Array.isArray(value)) {
          // Default: join arrays with comma
          serializedValue = value.length > 0 ? value.join(',') : null;
        } else if (value !== keyConfig.default && value !== null && value !== undefined && value !== '') {
          serializedValue = String(value);
        }

        // Update or remove parameter
        if (serializedValue !== null) {
          params.set(key, serializedValue);
        } else {
          params.delete(key);
        }
      });

      // Keep any extra params that aren't managed by this hook
      const managedKeys = Object.keys(config);
      const currentParams = new URLSearchParams(window.location.search);
      currentParams.forEach((value, key) => {
        if (!managedKeys.includes(key) && !params.has(key)) {
          params.set(key, value);
        }
      });

      const newUrl = `${window.location.pathname}?${params.toString()}`;
      
      if (replace) {
        window.history.replaceState(null, '', newUrl);
      } else {
        window.history.pushState(null, '', newUrl);
      }
    };

    // Clear any existing timer
    if (debounceTimerRef.current) {
      clearTimeout(debounceTimerRef.current);
    }

    if (debounceMs > 0) {
      debounceTimerRef.current = setTimeout(updateUrl, debounceMs);
    } else {
      updateUrl();
    }

    return () => {
      if (debounceTimerRef.current) {
        clearTimeout(debounceTimerRef.current);
      }
    };
  }, [state, config, replace, debounceMs]);

  return {
    getInitialValues,
    // Expose current URL params for reading
    getUrlParams: () => new URLSearchParams(window.location.search),
    // Manual URL update if needed
    updateUrl: (updates: Record<string, unknown>) => {
      const params = new URLSearchParams(window.location.search);
      Object.entries(updates).forEach(([key, value]) => {
        if (value) {
          params.set(key, String(value));
        } else {
          params.delete(key);
        }
      });
      const newUrl = `${window.location.pathname}?${params.toString()}`;
      if (replace) {
        window.history.replaceState(null, '', newUrl);
      } else {
        window.history.pushState(null, '', newUrl);
      }
    }
  };
}

/**
 * Simplified hook for common use cases
 */
export function useSimpleUrlState<T extends Record<string, unknown>>(
  state: T,
  defaults: Partial<T> = {},
  options: { replace?: boolean; debounceMs?: number } = {}
) {
  const config: UrlStateConfig = {};
  
  Object.keys(state).forEach(key => {
    config[key] = {
      default: defaults[key]
    };
  });

  return useUrlState(state, config, options);
}