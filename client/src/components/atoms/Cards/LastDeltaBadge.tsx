import React from 'react';
import Chip from '../Chip';
import { useLastDelta } from '../../../hooks/useLastDelta';

interface LastDeltaBadgeProps {
  cardId: string;
}

/**
 * Displays the last modification delta for a card.
 * Shows +X (green) for additions, -X (red) for removals.
 * Only visible if the card was modified in the current session.
 */
export const LastDeltaBadge: React.FC<LastDeltaBadgeProps> = ({ cardId }) => {
  const delta = useLastDelta(cardId);

  if (delta === undefined) {
    return null;
  }

  const isPositive = delta > 0;
  const displayText = isPositive ? `+${delta}` : `${delta}`;
  const bgColor = isPositive
    ? 'rgba(76, 175, 80, 0.9)'
    : 'rgba(244, 67, 54, 0.9)';

  return (
    <Chip
      label={displayText}
      size="small"
      sx={{
        position: 'absolute',
        top: 8,
        left: 8,
        zIndex: 15,
        height: 24,
        fontSize: '0.75rem',
        fontWeight: 700,
        backgroundColor: bgColor,
        color: 'white',
        backdropFilter: 'blur(4px)',
        border: '1px solid rgba(255, 255, 255, 0.2)',
        boxShadow: '0 2px 4px rgba(0,0,0,0.2)',
        '& .MuiChip-label': {
          px: 1
        }
      }}
    />
  );
};
