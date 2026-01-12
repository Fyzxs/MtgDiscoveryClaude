import React, { useState } from 'react';
import { Box } from '../../atoms';
import { CardImageDisplay } from '../../organisms/Cards/CardImageDisplay';
import { CardDetailsModal } from '../../organisms/Cards/CardDetailsModal';
import type { Card } from '../../../types/card';

interface BinderSlotProps {
  /** Card to display, or null for empty slot */
  card: Card | null;
  /** Whether this card is in the user's collection */
  isCollected: boolean;
  /** Slot index for accessibility */
  index: number;
  /** Whether there is a collector viewing (affects opacity) */
  hasCollector?: boolean;
}

/**
 * Individual binder slot displaying a card with transparency for missing cards.
 * Card back shows through when card is not collected.
 * Hover/click reveals full card and opens details modal.
 */
export const BinderSlot: React.FC<BinderSlotProps> = ({
  card,
  isCollected,
  index,
  hasCollector = true
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  // Determine opacity: full opacity if no collector, collected, or hovered; otherwise transparent
  const cardOpacity = !hasCollector || isCollected || isHovered ? 1 : 0.3;

  const handleClick = () => {
    if (card) {
      setModalOpen(true);
    }
  };

  const handleModalClose = () => {
    setModalOpen(false);
  };

  return (
    <Box
      sx={{
        // Grid cell container - centers the card within the cell
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: '100%',
        height: '100%'
      }}
    >
      <Box
        sx={{
          position: 'relative',
          // Maintain card aspect ratio - width-based sizing
          aspectRatio: '745 / 1040',
          width: '100%',
          maxHeight: '100%',
          borderRadius: '4.55%',
          overflow: 'hidden',
          cursor: card ? 'pointer' : 'default',
          bgcolor: 'grey.900',
          // Subtle border for slot definition
          border: '1px solid',
          borderColor: 'grey.800',
          transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
          '&:hover': card ? {
            transform: 'scale(1.02)',
            boxShadow: 6,
            zIndex: 1
          } : undefined
        }}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
        onClick={handleClick}
        role={card ? 'button' : undefined}
        tabIndex={card ? 0 : -1}
        aria-label={card ? `${card.name}${isCollected ? '' : ' (not collected)'}` : `Empty slot ${index + 1}`}
        onKeyDown={(e) => {
          if (card && (e.key === 'Enter' || e.key === ' ')) {
            e.preventDefault();
            handleClick();
          }
        }}
      >
        {/* Card back - only visible when there's a card */}
        {card && (
          <Box
            component="img"
            src="/cardback.jpeg"
            alt="Card back"
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              objectFit: 'cover',
              borderRadius: '4.55%'
            }}
          />
        )}

        {/* Card image with conditional opacity */}
        {card && (
          <Box
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              opacity: cardOpacity,
              transition: 'opacity 0.3s ease-in-out'
            }}
          >
            <CardImageDisplay
              card={card}
              size="normal"
              showFlipButton={false}
              sx={{
                width: '100%',
                height: '100%'
              }}
            />
          </Box>
        )}

        {/* Collection indicator for missing cards (only when there's a collector) */}
        {card && hasCollector && !isCollected && !isHovered && (
          <Box
            sx={{
              position: 'absolute',
              bottom: 8,
              left: '50%',
              transform: 'translateX(-50%)',
              bgcolor: 'rgba(0, 0, 0, 0.8)',
              color: 'error.light',
              px: 1,
              py: 0.5,
              borderRadius: 1,
              fontSize: '0.625rem',
              fontWeight: 600,
              textTransform: 'uppercase',
              letterSpacing: '0.05em'
            }}
          >
            Missing
          </Box>
        )}
      </Box>

      {/* Card details modal */}
      {card && (
        <CardDetailsModal
          open={modalOpen}
          onClose={handleModalClose}
          card={card}
        />
      )}
    </Box>
  );
};
