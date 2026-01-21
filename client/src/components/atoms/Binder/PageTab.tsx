import React from 'react';
import { Box, Typography } from '../../atoms';

interface PageTabProps {
  /** Page number */
  pageNumber: number;
  /** Whether this page is currently displayed */
  isActive: boolean;
  /** Whether this page has cards missing from collection */
  hasMissingCards?: boolean;
  /** Callback when tab is clicked */
  onClick: () => void;
  /** Which side the tab is on */
  side: 'left' | 'right';
  /** Vertical position from top in pixels */
  topOffset: number;
}

/**
 * Individual page tab positioned along the binder edge.
 */
export const PageTab: React.FC<PageTabProps> = ({
  pageNumber,
  isActive,
  hasMissingCards = false,
  onClick,
  side,
  topOffset
}) => {
  // Determine background color based on state
  const getBackgroundColor = () => {
    if (isActive) return 'primary.main';
    if (hasMissingCards) return 'warning.dark';
    return 'grey.800';
  };

  // Determine border color based on state
  // Active tabs with missing cards get amber border to indicate incomplete
  const getBorderColor = () => {
    if (isActive && hasMissingCards) return 'warning.main';
    if (isActive) return 'primary.light';
    if (hasMissingCards) return 'warning.main';
    return 'grey.700';
  };

  return (
    <Box
      component="button"
      onClick={onClick}
      sx={{
        position: 'absolute',
        top: topOffset,
        left: side === 'left' ? 0 : undefined,
        right: side === 'right' ? 0 : undefined,
        width: 28,
        height: 24,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        bgcolor: getBackgroundColor(),
        border: isActive && hasMissingCards ? '2px solid' : '1px solid',
        borderColor: getBorderColor(),
        borderRadius: side === 'right' ? '0 4px 4px 0' : '4px 0 0 4px',
        cursor: 'pointer',
        '&:hover': {
          bgcolor: isActive ? 'primary.dark' : hasMissingCards ? 'warning.main' : 'grey.700',
        },
        '&:focus': {
          outline: 'none',
          boxShadow: theme => `0 0 0 2px ${theme.palette.primary.main}`,
        }
      }}
      aria-label={`Go to page ${pageNumber}${hasMissingCards ? ' (has missing cards)' : ''}`}
      aria-current={isActive ? 'page' : undefined}
    >
      <Typography
        variant="caption"
        sx={{
          fontSize: '0.65rem',
          fontWeight: isActive ? 700 : 500,
          color: isActive ? 'primary.contrastText' : hasMissingCards ? 'warning.contrastText' : 'text.secondary',
          lineHeight: 1
        }}
      >
        {pageNumber}
      </Typography>
    </Box>
  );
};
