import React from 'react';
import Typography from '../Typography';

interface CardCountDisplayProps {
  count: number;
}

export const CardCountDisplay: React.FC<CardCountDisplayProps> = ({ count }) => {
  return (
    <Typography variant="body2" color="text.secondary">
      {count} cards
    </Typography>
  );
};