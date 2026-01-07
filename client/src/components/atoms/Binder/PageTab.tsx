import React from 'react';
import { Box, Typography } from '../../atoms';

interface PageTabProps {
  /** Page number */
  pageNumber: number;
  /** Whether this page is currently displayed */
  isActive: boolean;
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
  onClick,
  side,
  topOffset
}) => {
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
        bgcolor: isActive ? 'primary.main' : 'grey.800',
        border: '1px solid',
        borderColor: isActive ? 'primary.light' : 'grey.700',
        borderRadius: side === 'right' ? '0 4px 4px 0' : '4px 0 0 4px',
        cursor: 'pointer',
        '&:hover': {
          bgcolor: isActive ? 'primary.dark' : 'grey.700',
        },
        '&:focus': {
          outline: 'none',
          boxShadow: theme => `0 0 0 2px ${theme.palette.primary.main}`,
        }
      }}
      aria-label={`Go to page ${pageNumber}`}
      aria-current={isActive ? 'page' : undefined}
    >
      <Typography
        variant="caption"
        sx={{
          fontSize: '0.65rem',
          fontWeight: isActive ? 700 : 500,
          color: isActive ? 'primary.contrastText' : 'text.secondary',
          lineHeight: 1
        }}
      >
        {pageNumber}
      </Typography>
    </Box>
  );
};
