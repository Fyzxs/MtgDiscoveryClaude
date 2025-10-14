# Logging Guide

## Overview

This project uses a custom logger utility that automatically removes debug logs from production builds while keeping them available during development.

## Quick Start

```typescript
import { logger } from '../utils/logger';

// Development only (removed in production):
logger.debug('User clicked card:', cardId);
logger.info('Cards loaded successfully');

// Always logged (even in production):
logger.warn('Using deprecated API');
logger.error('Failed to fetch data:', error);
```

## Available Methods

### `logger.debug(...args)`
**Use for:** Detailed debugging information
**Production:** Removed (tree-shaken)
```typescript
logger.debug('Component rendered', { props, state });
```

### `logger.info(...args)`
**Use for:** General informational messages
**Production:** Removed (tree-shaken)
```typescript
logger.info('User logged in:', username);
```

### `logger.warn(...args)`
**Use for:** Warnings that should be monitored
**Production:** Always logged
```typescript
logger.warn('Deprecated prop used:', propName);
```

### `logger.error(...args)`
**Use for:** Errors that need attention
**Production:** Always logged (can integrate with error tracking)
```typescript
logger.error('API call failed:', error);
```

### `logger.perf(label, fn)`
**Use for:** Performance measurements
**Production:** No-op wrapper (still executes fn)
```typescript
logger.perf('card-rendering', () => {
  renderCards(cards);
});
```

## Advanced Usage

### Context-Specific Logger

Create a logger with automatic context prefix:

```typescript
// At top of component/module
const cardLogger = logger.createContext('CardGrid');

// Later in code
cardLogger.debug('Rendering cards:', cards.length);
// Output: [DEBUG] [CardGrid] Rendering cards: 42

cardLogger.error('Failed to load:', error);
// Output: [ERROR] [CardGrid] Failed to load: ...
```

**Benefits:**
- Easy to filter logs by component
- Helps track log sources
- No performance overhead in production

### GraphQL Operation Logging

```typescript
// Log queries (development only)
logger.graphql.query('GetCards', { setCode: 'MH3' });

// Log mutations (development only)
logger.graphql.mutation('UpdateCollection', { cardId, count });

// Log errors (always)
logger.graphql.error('GetCards', error);
```

## Production Behavior

### What Gets Removed

In production builds, the following are **completely removed** through tree-shaking and terser:

- All `logger.debug()` calls
- All `logger.info()` calls
- All `logger.graphql.query()` calls
- All `logger.graphql.mutation()` calls
- Direct `console.log()`, `console.debug()`, `console.info()`, `console.trace()`

### What Stays

- `logger.warn()` - Important warnings should be monitored
- `logger.error()` - Errors need tracking
- `logger.graphql.error()` - GraphQL errors
- `console.warn()` - Direct console warnings
- `console.error()` - Direct console errors

## ESLint Enforcement

The project uses ESLint to enforce logger usage:

```javascript
// ❌ ESLint Warning
console.log('User clicked:', cardId);
console.debug('Data:', data);

// ✅ Correct
logger.debug('User clicked:', cardId);
logger.debug('Data:', data);

// ✅ Allowed (direct console for errors)
console.error('Critical error:', error);
console.warn('Warning:', message);
```

## Build Configuration

### Vite Terser Configuration

The Vite build automatically removes console statements:

```typescript
// vite.config.ts
build: {
  minify: 'terser',
  terserOptions: {
    compress: {
      drop_console: false,        // Keep warn/error
      drop_debugger: true,
      pure_funcs: [
        'console.log',            // Remove
        'console.debug',          // Remove
        'console.info',           // Remove
        'console.trace',          // Remove
      ],
    },
  },
}
```

### Tree-Shaking

The logger utility uses environment checks that TypeScript can optimize away:

```typescript
// In source code
logger.debug('Test');

// In production build (completely removed)
// (nothing - tree-shaken away)

// In development build
if (true) {
  console.debug('[DEBUG]', 'Test');
}
```

## Best Practices

### ✅ Do's

1. **Use logger for all debugging**
   ```typescript
   logger.debug('Card state:', card);
   ```

2. **Create context loggers for components**
   ```typescript
   const log = logger.createContext('SetPage');
   log.debug('Filters changed:', filters);
   ```

3. **Use appropriate log levels**
   ```typescript
   logger.debug('Detailed state');    // Development details
   logger.info('Operation complete'); // General info
   logger.warn('Deprecated usage');   // Important warnings
   logger.error('Request failed');    // Critical errors
   ```

4. **Log meaningful context**
   ```typescript
   logger.error('Failed to load cards', {
     setCode,
     error,
     timestamp: Date.now(),
   });
   ```

### ❌ Don'ts

1. **Don't use console.log directly**
   ```typescript
   console.log('Test'); // ❌ ESLint warning
   ```

2. **Don't log sensitive data**
   ```typescript
   logger.debug('User password:', password); // ❌ Security risk
   ```

3. **Don't over-log in hot paths**
   ```typescript
   // ❌ Bad: logs on every render
   cards.forEach(card => logger.debug('Card:', card));
   
   // ✅ Good: log once
   logger.debug('Rendering cards:', cards.length);
   ```

4. **Don't use logger for user-facing messages**
   ```typescript
   // ❌ Bad: logs not shown to users
   logger.error('Failed to save');
   
   // ✅ Good: use UI notifications
   toast.error('Failed to save collection');
   logger.error('Collection save failed:', error);
   ```

## Verification

### Check Build Output

Verify logs are removed in production:

```bash
# Build for production
npm run build

# Check if debug logs are in bundle (should find none)
grep -r "console.debug" dist/assets/*.js
grep -r "\[DEBUG\]" dist/assets/*.js

# Should return empty
```

### Check ESLint

Verify ESLint catches console usage:

```bash
npm run lint
```

## Migration Guide

### Replacing Existing console.log

```typescript
// Before
console.log('Card clicked:', cardId);
console.debug('State:', state);
console.info('Loaded:', count);

// After
logger.debug('Card clicked:', cardId);
logger.debug('State:', state);
logger.info('Loaded:', count);
```

### Batch Replacement (Example)

```bash
# Find all console.log usage
grep -r "console.log" src/

# Use find and replace in your editor:
# Find:    console.log(
# Replace: logger.debug(
```

## FAQ

**Q: Why not just use console.log?**
A: Direct console.log statements remain in production bundles, increasing bundle size and potentially exposing sensitive information.

**Q: What about console.warn and console.error?**
A: These are intentionally kept in production for monitoring real issues.

**Q: Does this affect performance?**
A: No. In production, debug/info logs are completely removed via tree-shaking. Zero runtime overhead.

**Q: Can I temporarily enable debug logs in production?**
A: No. They're completely removed from the bundle. Use `logger.warn()` or `logger.error()` for persistent production logs.

**Q: How do I debug production issues?**
A: 
1. Use browser DevTools to set breakpoints
2. Integrate error tracking (Sentry, LogRocket)
3. Use `logger.error()` for critical paths
4. Build with `npm run build -- --mode development` for testing

**Q: Why terser instead of esbuild?**
A: Terser supports `pure_funcs` option to remove specific function calls. Esbuild doesn't have this feature yet.

## Future Enhancements

### Error Tracking Integration

```typescript
// In logger.ts error method
export const logger = {
  error: (...args: unknown[]) => {
    console.error('[ERROR]', ...args);
    
    // Send to error tracking service
    if (import.meta.env.PROD) {
      Sentry.captureException(args[0]);
    }
  },
};
```

### Log Buffering

For offline-first apps:

```typescript
// Buffer logs when offline, send when online
const logBuffer = [];

export const bufferedLogger = {
  debug: (...args) => {
    if (isDevelopment) {
      console.debug(...args);
    }
    if (!navigator.onLine) {
      logBuffer.push({ level: 'debug', args, timestamp: Date.now() });
    }
  },
};
```

### Remote Logging

For production debugging:

```typescript
export const remoteLogger = {
  critical: (message: string, context?: unknown) => {
    console.error('[CRITICAL]', message, context);
    
    // Send to logging service
    fetch('/api/logs', {
      method: 'POST',
      body: JSON.stringify({ message, context, timestamp: Date.now() }),
    });
  },
};
```
