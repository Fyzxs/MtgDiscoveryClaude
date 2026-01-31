import React from 'react';
import { Alert, type AlertProps } from '../../atoms';

/**
 * StatusMessage - Feedback molecule for displaying status messages
 *
 * Wraps MUI Alert for status, error, warning, and info messages.
 * Should be used by pages and templates instead of importing Alert atom directly.
 *
 * Note: For query loading/error states, prefer using QueryStateContainer.
 *
 * @example
 * <StatusMessage severity="error">
 *   Failed to load cards. Please try again.
 * </StatusMessage>
 *
 * @example
 * <StatusMessage severity="info" sx={{ mb: 2 }}>
 *   This set is not yet released.
 * </StatusMessage>
 */
export const StatusMessage: React.FC<AlertProps> = ({
  children,
  severity = 'info',
  ...props
}) => {
  return (
    <Alert severity={severity} {...props}>
      {children}
    </Alert>
  );
};
