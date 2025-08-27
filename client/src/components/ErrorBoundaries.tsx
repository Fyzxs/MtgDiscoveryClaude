import React from 'react';
import { ErrorBoundary } from './ErrorBoundary';
import type { ReactNode } from 'react';

interface ErrorBoundaryWrapperProps {
  children: ReactNode;
  name?: string;
}

/**
 * Page-level error boundary for entire page components
 */
export const PageErrorBoundary: React.FC<ErrorBoundaryWrapperProps> = ({ 
  children, 
  name 
}) => (
  <ErrorBoundary 
    level="page" 
    name={name || 'Page'}
    resetOnPropsChange
  >
    {children}
  </ErrorBoundary>
);

/**
 * Section-level error boundary for major page sections
 */
export const SectionErrorBoundary: React.FC<ErrorBoundaryWrapperProps> = ({ 
  children, 
  name 
}) => (
  <ErrorBoundary 
    level="section" 
    name={name || 'Section'}
    isolate
    resetOnPropsChange
  >
    {children}
  </ErrorBoundary>
);

/**
 * Component-level error boundary for individual components
 */
export const ComponentErrorBoundary: React.FC<ErrorBoundaryWrapperProps> = ({ 
  children, 
  name 
}) => (
  <ErrorBoundary 
    level="component" 
    name={name || 'Component'}
    isolate
  >
    {children}
  </ErrorBoundary>
);

/**
 * Filter-specific error boundary with auto-retry
 */
export const FilterErrorBoundary: React.FC<ErrorBoundaryWrapperProps> = ({ 
  children, 
  name 
}) => (
  <ErrorBoundary 
    level="component" 
    name={name || 'Filter'}
    isolate
    resetOnPropsChange
  >
    {children}
  </ErrorBoundary>
);

/**
 * Card grid error boundary with custom fallback
 */
export const CardGridErrorBoundary: React.FC<ErrorBoundaryWrapperProps> = ({ 
  children, 
  name 
}) => (
  <ErrorBoundary 
    level="section" 
    name={name || 'CardGrid'}
    isolate
    fallback={
      <div style={{ 
        padding: '2rem', 
        textAlign: 'center',
        color: '#999'
      }}>
        <p>Unable to load cards. Please try refreshing the page.</p>
      </div>
    }
  >
    {children}
  </ErrorBoundary>
);

/**
 * Modal error boundary that closes on error
 */
export const ModalErrorBoundary: React.FC<ErrorBoundaryWrapperProps & {
  onClose?: () => void;
}> = ({ 
  children, 
  name,
  onClose
}) => (
  <ErrorBoundary 
    level="component" 
    name={name || 'Modal'}
    isolate
    onError={(error, errorInfo) => {
      console.error('Modal error:', error, errorInfo);
      // Close modal on error after delay
      if (onClose) {
        setTimeout(onClose, 2000);
      }
    }}
  >
    {children}
  </ErrorBoundary>
);

/**
 * API/Data fetching error boundary with retry logic
 */
export const DataErrorBoundary: React.FC<ErrorBoundaryWrapperProps & {
  resetKeys?: Array<string | number>;
}> = ({ 
  children, 
  name,
  resetKeys = []
}) => (
  <ErrorBoundary 
    level="section" 
    name={name || 'Data'}
    isolate
    resetKeys={resetKeys}
    resetOnPropsChange
  >
    {children}
  </ErrorBoundary>
);