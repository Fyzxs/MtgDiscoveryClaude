import Typography from '../Typography';
import Box from '../Box';
import { DarkBadge } from '../shared/DarkBadge';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
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
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();

  if (!cardName) return null;

  const cardPath = `/card/${encodeURIComponent(cardName)}`;
  const href = buildUrlWithCollector(cardPath);

  return (
    <Box className={className}>
      <DarkBadge
        component="a"
        href={href}
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
          } else {
            // Use collector navigation for regular clicks
            createCollectorClickHandler(cardPath)(e);
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