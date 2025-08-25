import React from 'react';
import { Link, Typography, Box } from '@mui/material';

interface CardNameProps {
  cardId?: string;
  cardName?: string;
  onCardClick?: (cardId?: string) => void;
  className?: string;
}

export const CardName: React.FC<CardNameProps> = ({ 
  cardId,
  cardName,
  onCardClick,
  className 
}) => {
  if (!cardName) return null;

  return (
    <Box className={className}>
      <Link
        href={`/cards/${cardId}`}
        tabIndex={-1}
        onClick={(e) => {
          e.stopPropagation();
          if (onCardClick) {
            e.preventDefault();
            onCardClick(cardId);
          }
        }}
        sx={{
          color: 'white',
          textDecoration: 'none',
          display: 'inline-block',
          px: 0.5,
          py: 0.25,
          borderRadius: 1,
          '&:hover': {
            bgcolor: 'rgba(0, 0, 0, 1)',
            color: 'primary.main'
          },
          transition: 'all 0.2s ease'
        }}
        aria-label={`View all versions of ${cardName}`}
      >
        <Typography 
          variant="subtitle2" 
          component="span" 
          sx={{ 
            fontWeight: 'bold', 
            lineHeight: 1.2,
            fontSize: '0.875rem'
          }}
        >
          {cardName}
        </Typography>
      </Link>
    </Box>
  );
};