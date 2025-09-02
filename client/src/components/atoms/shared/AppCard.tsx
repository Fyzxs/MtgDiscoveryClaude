import React from 'react';
import { Card, CardContent } from '@mui/material';
import type { CardProps } from '@mui/material';

interface AppCardProps extends CardProps {
  children: React.ReactNode;
  padding?: boolean;
}

export const AppCard: React.FC<AppCardProps> = ({ 
  children, 
  padding = true,
  elevation = 2,
  ...props 
}) => {
  return (
    <Card elevation={elevation} {...props}>
      {padding ? (
        <CardContent>
          {children}
        </CardContent>
      ) : (
        children
      )}
    </Card>
  );
};