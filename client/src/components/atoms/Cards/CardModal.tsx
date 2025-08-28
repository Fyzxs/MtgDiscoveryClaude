import React from 'react';
import { Modal, Box, CardMedia } from '@mui/material';

interface CardModalProps {
  open: boolean;
  onClose: () => void;
  imageUrl?: string;
  cardName?: string;
  className?: string;
}

export const CardModal: React.FC<CardModalProps> = ({ 
  open,
  onClose,
  imageUrl,
  cardName,
  className 
}) => {
  return (
    <Modal
      open={open}
      onClose={onClose}
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
        sx={{
          outline: 'none',
          maxWidth: '90vw',
          maxHeight: '90vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}
        onClick={(e) => e.stopPropagation()}
      >
        <CardMedia
          component="img"
          image={imageUrl || ''}
          alt={cardName || 'Magic card'}
          sx={{
            maxWidth: '100%',
            maxHeight: '100%',
            width: 'auto',
            height: 'auto',
            borderRadius: 12,
            boxShadow: '0 25px 50px -12px rgba(0, 0, 0, 0.8)',
            cursor: 'default'
          }}
          onClick={onClose}
        />
      </Box>
    </Modal>
  );
};