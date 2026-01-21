import Typography from '../mui-wrappers/Typography';
import Box from '../mui-wrappers/Box';
import { DarkBadge } from '../shared/DarkBadge';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useLinkParams } from '../../../contexts/LinkParamsContext';
import type { StyledComponentProps } from '../../../types/components';

interface CardNameProps extends StyledComponentProps {
  cardId?: string;
  cardName?: string;
  onCardClick?: (cardId?: string) => void;
  /** Additional URL parameters to include in the link */
  additionalParams?: Record<string, string>;
}

export const CardName = ({
  cardId,
  cardName,
  onCardClick,
  className,
  additionalParams
}: CardNameProps) => {
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const contextParams = useLinkParams();

  // Use explicit prop if provided, otherwise fall back to context
  const params = additionalParams ?? contextParams;

  if (!cardName) return null;

  const cardPath = `/card/${encodeURIComponent(cardName)}`;
  const href = buildUrlWithCollector(cardPath, params);

  return (
    <Box className={className} sx={{ overflow: 'hidden', maxWidth: '100%' }}>
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
          // Allow browser's default behavior for modifier keys (CTRL/CMD+click = new tab, etc.)
          if (e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) {
            return;
          }
          if (onCardClick) {
            e.preventDefault();
            onCardClick(cardId);
          } else {
            // Use collector navigation for regular clicks
            createCollectorClickHandler(cardPath, params)(e);
          }
        }}
        aria-label={`View all versions of ${cardName}`}
        sx={{
          px: 1,
          py: 0.5,
          maxWidth: '100%',
          overflow: 'hidden'
        }}
      >
        <Typography
          variant="subtitle2"
          component="span"
          sx={{
            fontWeight: 'bold',
            lineHeight: 1.2,
            fontSize: '0.875rem',
            whiteSpace: 'nowrap',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            display: 'block'
          }}
        >
          {cardName}
        </Typography>
      </DarkBadge>
    </Box>
  );
};