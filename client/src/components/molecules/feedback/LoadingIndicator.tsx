import React from 'react';
import { Box, CircularProgress, type CircularProgressProps } from '../../atoms';
import { Section } from '../layouts/Section';

interface LoadingIndicatorProps extends CircularProgressProps {
  /** Optional message to display below the spinner */
  message?: string;
  /** Whether to center the indicator (default: true) */
  centered?: boolean;
  /** Whether to wrap in a Section with proper spacing (default: true) */
  withContainer?: boolean;
}

/**
 * LoadingIndicator - Feedback molecule for loading states
 *
 * Displays a centered loading spinner with optional message.
 * Should be used by pages and templates instead of importing CircularProgress atom directly.
 *
 * Note: For query loading states, prefer using QueryStateContainer.
 *
 * @example
 * <LoadingIndicator />
 *
 * @example
 * <LoadingIndicator message="Loading cards..." />
 *
 * @example
 * // Inline loading without container
 * <LoadingIndicator withContainer={false} centered={false} size={20} />
 */
export const LoadingIndicator: React.FC<LoadingIndicatorProps> = ({
  message,
  centered = true,
  withContainer = true,
  size = 40,
  ...props
}) => {
  const spinner = (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: centered ? 'center' : 'flex-start',
        gap: message ? 2 : 0
      }}
    >
      <CircularProgress size={size} {...props} />
      {message && (
        <Box component="span" sx={{ color: 'text.secondary' }}>
          {message}
        </Box>
      )}
    </Box>
  );

  if (withContainer) {
    return (
      <Section asSection={false} sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        {spinner}
      </Section>
    );
  }

  return spinner;
};
