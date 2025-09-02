import React, { useEffect, useRef } from 'react';
import { Modal, Box, CardMedia } from '@mui/material';
import type { StyledComponentProps } from '../../../types/components';

interface CardModalProps extends StyledComponentProps {
  open: boolean;
  onClose: () => void;
  imageUrl?: string;
  cardName?: string;
}

export const CardModal: React.FC<CardModalProps> = ({ 
  open,
  onClose,
  imageUrl,
  cardName,
  className 
}) => {
  const modalRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (open && modalRef.current) {
      modalRef.current.focus();
    }
  }, [open]);

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Escape') {
      onClose();
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      onKeyDown={handleKeyDown}
      aria-labelledby="card-modal-title"
      aria-describedby="card-modal-description"
      sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        bgcolor: 'rgba(0, 0, 0, 0.8)'
      }}
      onClick={onClose}
      className={className}
    >
      <Box
        ref={modalRef}
        tabIndex={-1}
        role="dialog"
        aria-modal="true"
        aria-labelledby="card-modal-title"
        aria-describedby="card-modal-description"
        sx={{
          outline: 'none',
          maxWidth: '90vw',
          maxHeight: '90vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          '&:focus': {
            outline: '2px solid',
            outlineColor: 'primary.main'
          }
        }}
        onClick={(e) => e.stopPropagation()}
      >
        <Box sx={{ display: 'none' }}>
          <div id="card-modal-title">{cardName ? `Viewing ${cardName}` : 'Viewing Magic card'}</div>
          <div id="card-modal-description">Large view of the card image. Press Escape or click outside to close.</div>
        </Box>
        <CardMedia
          component="img"
          image={imageUrl || ''}
          alt={cardName ? `Large view of ${cardName}` : 'Large view of Magic card'}
          sx={{
            maxWidth: '100%',
            maxHeight: '100%',
            width: 'auto',
            height: 'auto',
            borderRadius: 12,
            boxShadow: '0 25px 50px -12px rgba(0, 0, 0, 0.8)',
            cursor: 'pointer'
          }}
          onClick={onClose}
          onKeyDown={(e) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              onClose();
            }
          }}
        />
      </Box>
    </Modal>
  );
};