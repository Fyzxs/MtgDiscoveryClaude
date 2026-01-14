import React, { useState } from 'react';
import {
  Box,
  Typography,
  Popover,
} from '../../atoms';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

interface SealedCollectionSummaryProps {
  quantity: number;
  size?: 'small' | 'medium' | 'large';
  /** Force interactive mode even on mobile (for use in detail sheets) */
  forceInteractive?: boolean;
}

export const SealedCollectionSummary: React.FC<SealedCollectionSummaryProps> = ({
  quantity,
  size = 'medium',
  forceInteractive = false
}) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const [isHovered, setIsHovered] = useState(false);
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // On mobile/tablet: disable clicks (unless forceInteractive)
  const isTouchDevice = isMobile || isTablet;
  const disableClicks = isTouchDevice && !forceInteractive;

  // Show empty state for products with 0 quantity
  if (quantity === 0) {
    return (
      <Box
        sx={{
          display: 'inline-flex',
          alignItems: 'center',
          bgcolor: 'rgba(0, 0, 0, 0.8)',
          borderRadius: 1,
          px: 1,
          py: 0.5
        }}
      >
        <Typography
          variant="body2"
          sx={{
            fontSize: size === 'small' ? '0.75rem' : size === 'large' ? '1rem' : '0.875rem',
            color: 'white',
            fontWeight: 500
          }}
        >
          ⭕
        </Typography>
      </Box>
    );
  }

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (disableClicks) return;
    event.stopPropagation();
    event.preventDefault();
    // Show popover on click (desktop or forceInteractive touch device)
    setAnchorEl(event.currentTarget);
  };

  const handleMouseEnter = () => {
    if (disableClicks) return;
    setIsHovered(true);
  };

  const handleMouseLeave = () => {
    if (disableClicks) return;
    setIsHovered(false);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
    setIsHovered(false);
  };

  const open = Boolean(anchorEl);

  return (
    <Box
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        bgcolor: 'rgba(0, 0, 0, 0.8)',
        borderRadius: 1,
        px: 1,
        py: 0.5
      }}
    >
      <Typography
        variant="body2"
        onClick={disableClicks ? undefined : handleClick}
        onMouseEnter={disableClicks ? undefined : handleMouseEnter}
        onMouseLeave={disableClicks ? undefined : handleMouseLeave}
        sx={{
          fontSize: size === 'small' ? '0.75rem' : size === 'large' ? '1rem' : '0.875rem',
          fontWeight: 500,
          color: 'white',
          cursor: disableClicks ? 'default' : 'pointer',
          userSelect: 'none',
          minWidth: 'max-content',
          whiteSpace: 'nowrap',
          ...(!disableClicks && {
            '&:hover': {
              color: 'primary.light'
            }
          })
        }}
      >
        [{quantity}]
      </Typography>

      <Popover
        open={open}
        anchorEl={anchorEl}
        onClose={handlePopoverClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        sx={{ mt: 0.5 }}
      >
        <Box sx={{ p: 2, minWidth: 200 }}>
          <Typography variant="h6" gutterBottom sx={{ fontSize: '1rem', fontWeight: 600, mb: 1 }}>
            Collection
          </Typography>
          <Typography variant="body2">
            Quantity: {quantity}
          </Typography>
        </Box>
      </Popover>
    </Box>
  );
};
