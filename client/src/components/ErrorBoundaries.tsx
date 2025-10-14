import React from 'react';
import { ErrorBoundary } from './ErrorBoundary';
import type { ReactNode, ErrorInfo } from 'react';
import { Box, Typography } from './atoms';
import { logger } from '../utils/logger';

type ErrorBoundaryLevel = 'page' | 'section' | 'component';
type ErrorBoundaryVariant = 'default' | 'modal' | 'card-grid';

interface AppErrorBoundaryProps {
  children: ReactNode;
  name?: string;
  level?: ErrorBoundaryLevel;
  variant?: ErrorBoundaryVariant;
  resetKeys?: Array<string | number>;
  onClose?: () => void;
  customFallback?: ReactNode;
}

/**
 * Unified error boundary component that replaces all specialized variants.
 * Supports different levels (page/section/component) and variants for special behaviors.
 */
export const AppErrorBoundary: React.FC<AppErrorBoundaryProps> = ({
  children,
  name,
  level = 'component',
  variant = 'default',
  resetKeys,
  onClose,
  customFallback
}) => {
  // Configure props based on level and variant
  const getErrorBoundaryProps = () => {
    const baseProps = {
      level,
      name: name || level.charAt(0).toUpperCase() + level.slice(1),
      resetKeys,
      isolate: level !== 'page',
      resetOnPropsChange: level !== 'component'
    };

    // Handle variant-specific behavior
    switch (variant) {
      case 'modal':
        return {
          ...baseProps,
          level: 'component' as const,
          isolate: true,
          onError: (error: Error, errorInfo: ErrorInfo) => {
            logger.error('Modal error:', error, errorInfo);
            if (onClose) {
              setTimeout(onClose, 2000);
            }
          }
        };
        
      case 'card-grid':
        return {
          ...baseProps,
          level: 'section' as const,
          isolate: true,
          fallback: customFallback || (
            <Box sx={{ p: 4, textAlign: 'center', color: 'text.secondary' }}>
              <Typography variant="body1">
                Unable to load cards. Please try refreshing the page.
              </Typography>
            </Box>
          )
        };
        
      default:
        return {
          ...baseProps,
          fallback: customFallback
        };
    }
  };

  return <ErrorBoundary {...getErrorBoundaryProps()}>{children}</ErrorBoundary>;
};

// Legacy exports for backward compatibility
export const PageErrorBoundary: React.FC<{ children: ReactNode; name?: string }> = (props) => (
  <AppErrorBoundary level="page" {...props} />
);

export const SectionErrorBoundary: React.FC<{ children: ReactNode; name?: string }> = (props) => (
  <AppErrorBoundary level="section" {...props} />
);

export const ComponentErrorBoundary: React.FC<{ children: ReactNode; name?: string }> = (props) => (
  <AppErrorBoundary level="component" {...props} />
);

export const FilterErrorBoundary: React.FC<{ children: ReactNode; name?: string }> = (props) => (
  <AppErrorBoundary level="component" {...props} />
);

export const CardGridErrorBoundary: React.FC<{ children: ReactNode; name?: string }> = (props) => (
  <AppErrorBoundary level="section" variant="card-grid" {...props} />
);

export const ModalErrorBoundary: React.FC<{ children: ReactNode; name?: string; onClose?: () => void }> = (props) => (
  <AppErrorBoundary level="component" variant="modal" {...props} />
);

export const DataErrorBoundary: React.FC<{ children: ReactNode; name?: string; resetKeys?: Array<string | number> }> = (props) => (
  <AppErrorBoundary level="section" {...props} />
);