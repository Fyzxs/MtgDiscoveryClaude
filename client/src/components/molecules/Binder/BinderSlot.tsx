import React, { useState, useRef, useCallback, useEffect } from 'react';
import { Box } from '../../atoms';
import { CardImageDisplay } from '../../organisms/Cards/CardImageDisplay';
import { CardDetailsModal } from '../../organisms/Cards/CardDetailsModal';
import { ZoomIndicator } from '../../atoms/Cards/ZoomIndicator';
import { DeltaBadge } from '../shared/badges';
import { CollectionEntryOverlay } from '../Cards/CollectionEntryOverlay';
import { CollectionSummary } from '../Cards/CollectionSummary';
import { WishlistSummary } from '../Cards/WishlistSummary';
import { useMtgCardCollectionActions } from '../../../hooks/useMtgCardCollectionActions';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';
import { useCardCollectionFromCache } from '../../../hooks/useCardCollectionFromCache';
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
 * Desktop: Click selects card for collection modification, zoom button opens details.
 * Mobile: Click opens details modal directly.
 */
export const BinderSlot: React.FC<BinderSlotProps> = ({
  card,
  isCollected,
  index,
  hasCollector = true
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [isSelected, setIsSelected] = useState(false);
  const cardRef = useRef<HTMLDivElement>(null);
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // Determine opacity: full opacity if no collector, collected, or hovered; otherwise transparent
  const cardOpacity = !hasCollector || isCollected || isHovered ? 1 : 0.3;

  // Track selection state from DOM attribute
  useEffect(() => {
    if (!cardRef.current) return;

    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.attributeName === 'data-selected') {
          const selected = cardRef.current?.getAttribute('data-selected') === 'true';
          setIsSelected(selected || false);
        }
      });
    });

    observer.observe(cardRef.current, { attributes: true, attributeFilter: ['data-selected'] });

    return () => observer.disconnect();
  }, []);

  // Read collection/wishlist data from Apollo cache for reactive badge updates
  const { userCollection, userWishlist } = useCardCollectionFromCache(card?.id ?? '');

  // Enable collection actions via keyboard when card is selected (only for non-empty slots)
  const { overlayState } = useMtgCardCollectionActions({
    card: card ?? undefined,
    isSelected: card ? isSelected : false,
    cardRef
  });

  // Handle zoom button click - opens modal
  const handleZoomClick = useCallback((e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setModalOpen(true);
  }, []);

  // Handle card click - select on desktop, open modal on mobile
  const handleCardClick = useCallback((e: React.MouseEvent) => {
    if (!card) return;

    // Don't trigger selection if clicking on zoom indicator
    const target = e.target as HTMLElement;
    const clickedZoom = target.closest('.zoom-indicator');

    if (clickedZoom) {
      return;
    }

    e.preventDefault();
    e.stopPropagation();

    const cardElement = e.currentTarget as HTMLElement;

    // Mobile/tablet behavior - tap opens modal
    if (isMobile || isTablet) {
      setModalOpen(true);
      return;
    }

    // Desktop behavior - select card for collection modification
    // Clear ALL selections across ALL binder slots
    const allSelected = document.querySelectorAll('[data-binder-card="true"][data-selected="true"]');
    allSelected.forEach(selected => {
      if (selected !== cardElement) {
        selected.setAttribute('data-selected', 'false');
      }
    });

    // Select the clicked card
    cardElement.setAttribute('data-selected', 'true');

    // Remove focus from any other focused element
    const focused = document.querySelector(':focus');
    if (focused && focused !== cardElement) {
      (focused as HTMLElement).blur();
    }

    // Focus this card to enable keyboard navigation
    cardElement.focus();
  }, [card, isMobile, isTablet]);

  const handleModalClose = useCallback(() => {
    setModalOpen(false);
  }, []);

  // Desktop only: deselect when clicking outside
  useEffect(() => {
    if (isMobile || isTablet) return;

    const handleDocumentClick = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      // Check if click was inside any binder card
      const clickedCard = target.closest('[data-binder-card="true"]');
      if (clickedCard) return;

      // Click was outside all cards - deselect all
      const allSelected = document.querySelectorAll('[data-binder-card="true"][data-selected="true"]');
      allSelected.forEach(selected => {
        selected.setAttribute('data-selected', 'false');
      });
    };

    document.addEventListener('click', handleDocumentClick);
    return () => document.removeEventListener('click', handleDocumentClick);
  }, [isMobile, isTablet]);

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
        ref={cardRef}
        data-binder-card="true"
        data-card-id={card?.id}
        data-card-index={index}
        data-selected="false"
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
          border: '2px solid',
          borderColor: 'grey.800',
          transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out, border-color 0.2s ease-in-out',
          '&:hover': card ? {
            transform: 'scale(1.02)',
            boxShadow: 6,
            zIndex: 1,
            // Show zoom indicator on hover (desktop)
            '& .zoom-indicator': {
              opacity: 1,
              transform: 'scale(1)'
            }
          } : undefined,
          // Selected state styling
          '&[data-selected="true"]': {
            borderColor: 'primary.main',
            borderWidth: 3,
            boxShadow: (theme) => `0 0 0 2px ${theme.palette.primary.main}`,
            zIndex: 2
          }
        }}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
        onClick={handleCardClick}
        role={card ? 'button' : undefined}
        tabIndex={card ? 0 : -1}
        aria-label={card ? `${card.name}${isCollected ? '' : ' (not collected)'}` : `Empty slot ${index + 1}`}
        onKeyDown={(e) => {
          if (card && (e.key === 'Enter' || e.key === ' ')) {
            e.preventDefault();
            handleCardClick(e as any);
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

        {/* Zoom indicator - desktop only */}
        {card && !isMobile && !isTablet && (
          <ZoomIndicator onZoomClick={handleZoomClick} />
        )}

        {/* Last modification delta badge */}
        {card && <DeltaBadge itemId={card.id} positioned />}

        {/* Collection and Wishlist badges - bottom corners */}
        {/* Show counts if there's a collector AND a collector number, or just if there's a collector number */}
        {card && (hasCollector || card.collectorNumber) && (
          <Box
            sx={{
              position: 'absolute',
              bottom: 8,
              left: 8,
              right: 8,
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'flex-end',
              pointerEvents: 'auto',
              zIndex: 20
            }}
          >
            {userCollection && (
              <CollectionSummary
                collectionData={userCollection}
                size="small"
              />
            )}
            {userWishlist && (
              <WishlistSummary
                wishlistData={userWishlist}
                size="small"
              />
            )}
          </Box>
        )}

        {/* Collection indicator for missing cards (only when there's a collector and not selected) */}
        {card && hasCollector && !isCollected && !isHovered && !isSelected && (
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

        {/* Collection entry overlay - always mounted for pre-mount pattern */}
        {card && (
          <CollectionEntryOverlay
            cardId={isSelected ? card.id : undefined}
            visible={overlayState.visible}
            count={overlayState.count}
            isNegative={overlayState.isNegative}
            finish={overlayState.finish}
            special={overlayState.special}
            variant="card"
            flash={overlayState.flash}
          />
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
