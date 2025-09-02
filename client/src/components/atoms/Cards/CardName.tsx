import { Typography, Box } from '@mui/material';
import { DarkBadge } from '../shared/DarkBadge';
import type { StyledComponentProps } from '../../../types/components';

interface CardNameProps extends StyledComponentProps {
  cardId?: string;
  cardName?: string;
  onCardClick?: (cardId?: string) => void;
}

export const CardName = ({ 
  cardId,
  cardName,
  onCardClick,
  className 
}: CardNameProps) => {
  if (!cardName) return null;

  return (
    <Box className={className}>
      <DarkBadge
        component="a"
        href={`/card/${encodeURIComponent(cardName)}`}
        tabIndex={0}
        onKeyDown={(e: React.KeyboardEvent) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.stopPropagation();
            if (onCardClick) {
              e.preventDefault();
              onCardClick(cardId);
            }
          }
        }}
        onClick={(e) => {
          e.stopPropagation();
          if (onCardClick) {
            e.preventDefault();
            onCardClick(cardId);
          }
        }}
        aria-label={`View all versions of ${cardName}`}
        sx={{
          px: 1,
          py: 0.5
        }}
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
      </DarkBadge>
    </Box>
  );
};