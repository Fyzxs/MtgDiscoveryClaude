import React, { type ReactNode } from 'react';
import { Modal, Box, IconButton } from '../../atoms';
import { CloseIcon } from '../../atoms/Icons';

interface ModalContainerProps {
  open: boolean;
  onClose: () => void;
  children: ReactNode;
  width?: string | number;
  maxWidth?: string | number;
  height?: string | number;
  maxHeight?: string | number;
  showCloseButton?: boolean;
  disableEscapeKeyDown?: boolean;
  disableBackdropClick?: boolean;
}

/**
 * Reusable modal wrapper with consistent styling and close functionality
 */
export const ModalContainer: React.FC<ModalContainerProps> = ({
  open,
  onClose,
  children,
  width = '90vw',
  maxWidth = 1400,
  height = '90vh',
  maxHeight = undefined,
  showCloseButton = true,
  disableEscapeKeyDown = false,
  disableBackdropClick = false
}) => {

  const handleBackdropClick = () => {
    if (!disableBackdropClick) {
      onClose();
    }
  };

  return (
    <Modal
      open={open}
      onClose={disableEscapeKeyDown ? () => {} : onClose}
      sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        bgcolor: 'rgba(0, 0, 0, 0.9)'
      }}
      onClick={handleBackdropClick}
    >
      <Box
        sx={{
          width,
          maxWidth,
          height,
          maxHeight,
          bgcolor: 'background.paper',
          borderRadius: 2,
          display: 'flex',
          flexDirection: 'column',
          overflow: 'hidden',
          outline: 'none',
          position: 'relative'
        }}
        onClick={(e) => e.stopPropagation()}
      >
        {showCloseButton && (
          <IconButton
            onClick={onClose}
            sx={{
              position: 'absolute',
              top: 8,
              right: 8,
              zIndex: 1,
              bgcolor: 'background.paper',
              '&:hover': {
                bgcolor: 'action.hover'
              }
            }}
          >
            <CloseIcon />
          </IconButton>
        )}
        {children}
      </Box>
    </Modal>
  );
};