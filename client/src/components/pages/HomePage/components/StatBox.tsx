import React from 'react';
import { Box, Card, CircularProgress, Typography } from '../../../atoms';

interface StatBoxProps {
  value: number | null;
  label: string;
  ctaText?: string;
  onClick?: () => void;
}

export const StatBox: React.FC<StatBoxProps> = React.memo(({
  value,
  label,
  ctaText,
  onClick,
}) => {
  const isClickable = onClick !== undefined;

  return (
    <Card
      elevation={0}
      onClick={onClick}
      sx={{
        border: 1,
        borderColor: 'divider',
        p: 3,
        textAlign: 'center',
        cursor: isClickable ? 'pointer' : 'default',
        transition: 'border-color 0.2s ease-in-out',
        '&:hover': isClickable
          ? { borderColor: 'primary.main' }
          : undefined,
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          gap: 1,
          minHeight: 80,
          justifyContent: 'center',
        }}
      >
        {value === null ? (
          <CircularProgress size={24} />
        ) : (
          <Typography
            variant="h3"
            component="span"
            sx={{ fontWeight: 700 }}
          >
            {new Intl.NumberFormat('en-US').format(value)}
          </Typography>
        )}

        <Typography variant="body2" color="text.secondary">
          {label}
        </Typography>

        {value === 0 && ctaText !== undefined && (
          <Typography
            variant="caption"
            color="primary.main"
            sx={{ fontWeight: 500 }}
          >
            {ctaText}
          </Typography>
        )}
      </Box>
    </Card>
  );
});
