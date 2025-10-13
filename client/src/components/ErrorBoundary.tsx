import { Component } from 'react';
import type { ErrorInfo, ReactNode } from 'react';
import { Box, Alert, Button, Typography, Stack } from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { logger, errorTracking } from '../utils/logger';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
  onError?: (error: Error, errorInfo: ErrorInfo) => void;
  resetKeys?: Array<string | number>;
  resetOnPropsChange?: boolean;
  isolate?: boolean;  // If true, only shows error for this section
  level?: 'page' | 'section' | 'component';
  name?: string; // For better error tracking
}

interface State {
  hasError: boolean;
  error: Error | null;
  errorInfo: ErrorInfo | null;
  errorCount: number;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
    error: null,
    errorInfo: null,
    errorCount: 0
  };
  
  private resetTimeoutId?: NodeJS.Timeout;

  public static getDerivedStateFromError(error: Error): Partial<State> {
    return { 
      hasError: true, 
      error
    };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    const { onError, name, level } = this.props;

    // Enhanced logging with context
    logger.error(`[ErrorBoundary${name ? ` - ${name}` : ''}]:`, error, errorInfo);
    
    // Store error info and increment count
    this.setState((prevState) => ({ 
      errorInfo,
      errorCount: prevState.errorCount + 1 
    }));
    
    // Call custom error handler if provided
    if (onError) {
      onError(error, errorInfo);
    }

    // Send to error tracking service
    errorTracking.captureException(error, {
      errorBoundary: name || 'unknown',
      level,
      componentStack: errorInfo.componentStack,
      errorCount: this.state.errorCount + 1
    });
    
    // Auto-retry for network errors after delay
    if (error.message?.includes('fetch') || error.message?.includes('network')) {
      this.resetTimeoutId = setTimeout(() => {
        this.handleReset();
      }, 5000);
    }
  }

  public componentDidUpdate(prevProps: Props) {
    const { resetKeys, resetOnPropsChange } = this.props;
    const { hasError } = this.state;
    
    // Reset on prop changes if configured
    if (hasError && prevProps.resetKeys !== resetKeys) {
      if (resetKeys?.some((key, idx) => key !== prevProps.resetKeys?.[idx])) {
        this.handleReset();
      }
    }
    
    // Reset when children change significantly
    if (hasError && resetOnPropsChange && prevProps.children !== this.props.children) {
      this.handleReset();
    }
  }
  
  public componentWillUnmount() {
    if (this.resetTimeoutId) {
      clearTimeout(this.resetTimeoutId);
    }
  }
  
  private handleReset = () => {
    this.setState({ 
      hasError: false, 
      error: null,
      errorInfo: null 
    });
  };

  public render() {
    const { hasError, error, errorInfo, errorCount } = this.state;
    const { fallback, isolate, level = 'component', children } = this.props;
    
    if (hasError) {
      // Use custom fallback if provided
      if (fallback) {
        return <>{fallback}</>;
      }
      
      // Different error displays based on level
      if (level === 'page') {
        return (
          <Box 
            sx={{ 
              minHeight: '100vh',
              bgcolor: 'background.default',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              p: 4
            }}
          >
            <Stack spacing={3} alignItems="center" maxWidth="md">
              <ErrorOutlineIcon sx={{ fontSize: 64, color: 'error.main' }} />
              <Typography variant="h4" component="h1" gutterBottom>
                Something went wrong
              </Typography>
              <Typography variant="body1" color="text.secondary" align="center">
                {error?.message || 'An unexpected error occurred'}
              </Typography>
              {process.env.NODE_ENV !== 'production' && errorInfo && (
                <Alert severity="error" sx={{ width: '100%', mt: 2 }}>
                  <Typography variant="caption" component="pre" sx={{ fontFamily: 'monospace', whiteSpace: 'pre-wrap' }}>
                    {errorInfo.componentStack}
                  </Typography>
                </Alert>
              )}
              <Stack direction="row" spacing={2}>
                <Button 
                  variant="contained" 
                  startIcon={<RefreshIcon />}
                  onClick={this.handleReset}
                >
                  Try Again
                </Button>
                <Button 
                  variant="outlined"
                  onClick={() => window.location.href = '/'}
                >
                  Go Home
                </Button>
              </Stack>
              {errorCount > 2 && (
                <Alert severity="warning">
                  This error has occurred {errorCount} times. Please refresh the page if the issue persists.
                </Alert>
              )}
            </Stack>
          </Box>
        );
      }
      
      if (level === 'section') {
        return (
          <Box sx={{ p: 3 }}>
            <Alert 
              severity="error" 
              action={
                <Button color="inherit" size="small" onClick={this.handleReset}>
                  Retry
                </Button>
              }
            >
              <Typography variant="subtitle2">
                Failed to load this section
              </Typography>
              {process.env.NODE_ENV !== 'production' && (
                <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                  {error?.message}
                </Typography>
              )}
            </Alert>
          </Box>
        );
      }
      
      // Component level - inline error
      if (isolate) {
        return (
          <Alert 
            severity="error" 
            sx={{ m: 1 }}
            action={
              <Button color="inherit" size="small" onClick={this.handleReset}>
                Retry
              </Button>
            }
          >
            Component failed to load
          </Alert>
        );
      }
      
      // Default error display
      return (
        <Box sx={{ p: 2 }}>
          <Alert severity="error">
            {error?.message || 'An error occurred'}
          </Alert>
        </Box>
      );
    }

    return children;
  }
}