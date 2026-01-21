import React from 'react';
import { Modal, Box, CircularProgress, Typography } from '../../atoms';
import type { SxProps, Theme } from '../../atoms';

interface LoadingModalProps {
  open: boolean;
  message?: string;
}

/**
 * LoadingModal - Displays a centered loading spinner in a modal overlay
 *
 * Features:
 * - Semi-transparent backdrop
 * - Centered loading spinner
 * - Optional message text
 * - Prevents interaction with background content
 */
export const LoadingModal: React.FC<LoadingModalProps> = React.memo(({
  open,
  message = 'Loading...'
}) => {
  const backdropStyles: SxProps<Theme> = {
    backgroundColor: 'rgba(0, 0, 0, 0.7)',
    backdropFilter: 'blur(4px)'
  };

  const contentStyles: SxProps<Theme> = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    gap: 2,
    bgcolor: 'background.paper',
    borderRadius: 2,
    boxShadow: 24,
    p: 4,
    minWidth: 200,
    outline: 'none'
  };

  return (
    <Modal
      open={open}
      slotProps={{
        backdrop: {
          sx: backdropStyles
        }
      }}
      disableAutoFocus
      disableEnforceFocus
    >
      <Box sx={contentStyles}>
        <CircularProgress size={48} />
        {message && (
          <Typography variant="body1" color="text.primary">
            {message}
          </Typography>
        )}
      </Box>
    </Modal>
  );
});

LoadingModal.displayName = 'LoadingModal';
