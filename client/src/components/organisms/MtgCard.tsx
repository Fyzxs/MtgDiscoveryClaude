import React, { useState, useCallback, useMemo, useEffect, useRef } from 'react';
import { Card as MuiCard, Box, keyframes } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import type { Card, CardContext, UserCardData } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { ZoomIndicator } from '../atoms/Cards/ZoomIndicator';
import { CardOverlay } from '../molecules/Cards/CardOverlay';
import { CardDetailsModal } from './CardDetailsModal';
import { CardBadges } from '../atoms/Cards/CardBadges';
import { getRarityGlowStyles } from '../../utils/rarityStyles';
import { srOnly } from '../../styles/cardStyles';
import { useCardCollectionEntry } from '../../hooks/useCardCollectionEntry3';
import { useCollection } from '../../contexts/CollectionContext';
import type { CardFinish } from '../../types/collection';
import type { StyledComponentProps } from '../../types/components';

interface MtgCardProps extends StyledComponentProps {
  card: Card;
  context?: CardContext;
  collectionData?: UserCardData | UserCardData[];
  index: number;
  groupId: string;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
}

// Flash animation for success/error feedback
const flashSuccess = keyframes`
  0%, 100% { opacity: 0; }
  50% { opacity: 0.5; }
`;

const flashError = keyframes`
  0%, 100% { opacity: 0; }
  50% { opacity: 0.6; }
`;

// Subtle pulse for submitting state
const submitPulse = keyframes`
  0%, 100% { opacity: 0.15; }
  50% { opacity: 0.25; }
`;

const MtgCardComponent: React.FC<MtgCardProps> = ({
  card,
  context = {},
  collectionData: _collectionData,
  index,
  groupId: _groupId,
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  const [modalOpen, setModalOpen] = useState(false);
  const [isSelected, setIsSelected] = useState(false);
  // REMOVED: Flash state moved to DOM data attribute for performance
  const theme = useTheme();
  const cardRef = useRef<HTMLDivElement>(null);
  const { submitCollectionUpdate } = useCollection();

  // Memoize aria label to avoid recalculating on every render
  const ariaLabel = useMemo(() => {
    return `${card.name} - ${card.rarity} ${card.typeLine || 'card'} from ${card.setName}. Artist: ${card.artist}`;
  }, [card.name, card.rarity, card.typeLine, card.setName, card.artist]);

  // Memoize hover styles to avoid recalculating rarity styles
  const hoverStyles = useMemo(() => {
    return getRarityGlowStyles(card.rarity, false, true);
  }, [card.rarity]);

  // Determine available finishes
  const availableFinishes = useMemo<CardFinish[]>(() => {
    const finishes: CardFinish[] = [];
    if (card.nonFoil) finishes.push('non-foil');
    if (card.foil) finishes.push('foil');
    if (card.finishes?.includes('etched')) finishes.push('etched');
    return finishes.length > 0 ? finishes : ['non-foil']; // Default to non-foil if none specified
  }, [card.nonFoil, card.foil, card.finishes]);

  // Handle collection update submission
  const handleCollectionSubmit = useCallback(async (update: any) => {
    const cardElement = cardRef.current;
    if (!cardElement) return;

    // Mark as submitting for instant visual feedback (but keep selected for rapid entry)
    cardElement.setAttribute('data-submitting', 'true');

    try {
      // Fire-and-forget submission
      await submitCollectionUpdate({ ...update, setId: card.setId }, card.name);
      // Success flash via DOM (after mutation succeeds)
      cardElement.removeAttribute('data-submitting');
      cardElement.setAttribute('data-flash', 'success');
      setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
    } catch (error) {
      // Error flash via DOM
      cardElement.removeAttribute('data-submitting');
      cardElement.setAttribute('data-flash', 'error');
      setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
    }
  }, [submitCollectionUpdate, card.name, card.setId]);

  // Collection entry hook
  useCardCollectionEntry({
    cardId: card.id,
    isSelected,
    availableFinishes,
    onSubmit: handleCollectionSubmit
  });

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

  const handleCardClick = useCallback((e: React.MouseEvent) => {
    // Don't trigger selection if clicking on links or zoom indicator
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');
    const clickedZoom = target.closest('.zoom-indicator');

    // If clicking a link or zoom, let it handle itself - don't prevent default
    if (clickedLink || clickedZoom) {
      return;
    }

    // Only prevent default for card selection clicks
    e.preventDefault();
    e.stopPropagation();

    const cardElement = e.currentTarget as HTMLElement;

    // Clear ALL selections across ALL card groups on the page
    const allSelected = document.querySelectorAll('[data-selected="true"]');
    allSelected.forEach(selected => {
      if (selected !== cardElement) {
        selected.setAttribute('data-selected', 'false');
      }
    });

    // Always select the clicked card (don't toggle)
    cardElement.setAttribute('data-selected', 'true');

    // Remove focus from any other focused element to clear blue borders
    const focused = document.querySelector(':focus');
    if (focused && focused !== cardElement) {
      (focused as HTMLElement).blur();
    }

    // Focus this card to enable keyboard navigation from here
    cardElement.focus();
  }, []);

  const handleZoomClick = useCallback((e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setModalOpen(true);
  }, []);

  const handleModalClose = useCallback(() => {
    setModalOpen(false);
  }, []);

  return (
    <MuiCard
      ref={cardRef}
      elevation={4}
      onClick={handleCardClick}
      data-mtg-card="true"
      data-card-id={card.id}
      data-selected="false"
      data-card-index={index}
      tabIndex={-1}
      role="button"
      aria-label={ariaLabel}
      aria-describedby={`card-details-${card.id}`}
      sx={{
        position: 'relative',
        width: '280px',
        bgcolor: 'grey.800',
        borderRadius: '12px',
        border: '3px solid',
        borderColor: 'grey.700',
        overflow: 'hidden',
        boxShadow: theme.mtg.shadows.card.normal,
        transition: 'transform 0.05s ease-out',
        transform: 'scale(1)',
        cursor: 'pointer',
        outline: 'none',
        // Instant deselection when submitting (no animation lag)
        '&[data-submitting="true"]': {
          transition: 'none !important',
          transform: 'scale(1) !important',
          border: '3px solid',
          borderColor: 'grey.700',
          boxShadow: theme.mtg.shadows.card.normal,
          '&::before': {
            content: '""',
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: '#1976d2',
            opacity: 0.2,
            animation: `${submitPulse} 0.6s ease-in-out infinite`,
            pointerEvents: 'none',
            zIndex: 999,
          }
        },
        // Flash overlay using pseudo-element (CSS-only, no React state)
        '&[data-flash="success"]::after, &[data-flash="error"]::after': {
          content: '""',
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          pointerEvents: 'none',
          zIndex: 9999,
        },
        '&[data-flash="success"]::after': {
          backgroundColor: '#4caf50',
          animation: `${flashSuccess} 0.3s ease-in-out 3`,
        },
        '&[data-flash="error"]::after': {
          backgroundColor: '#f44336',
          animation: `${flashError} 0.3s ease-in-out 3`,
        },
        '&:focus': {
          outline: 'none'
        },
        '&:focus-visible': {
          outline: 'none'
        },
        // CSS-only selection styles
        '&[data-selected="true"]': {
          border: '4px solid',
          borderColor: '#1976d2',
          boxShadow: `${theme.mtg.shadows.card.selected}, ${theme.mtg.shadows.card.normal}`,
          transform: 'scale(1.05)',
          '& .zoom-indicator': {
            opacity: 1,
            transform: 'scale(1)'
          }
        },
        '&:hover:not([data-selected="true"])': {
          ...hoverStyles,
          transform: 'scale(1.01)',
          '& .zoom-indicator': {
            opacity: 1,
            transform: 'scale(1)'
          }
        }
      }}
      className={className}
    >
      <Box sx={{
        width: '100%',
        aspectRatio: '745 / 1040',
        position: 'relative'
      }}>
        <CardImageDisplay
          card={card}
          size="normal"
          showFlipButton={true}
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%'
          }}
        />
      </Box>

      <CardBadges
        foil={card.foil}
        nonfoil={card.nonFoil}
        promoTypes={card.promoTypes}
        frameEffects={card.frameEffects}
        isPromo={card.promo}
        digital={card.digital}
      />

      <ZoomIndicator
        onZoomClick={handleZoomClick}
      />


      <CardOverlay
        card={card}
        isSelected={false}
        context={context}
        onCardClick={undefined}
        onArtistClick={onArtistClick}
        onSetClick={onSetClick}
      />

      {/* Hidden element for screen reader description */}
      <Box
        id={`card-details-${card.id}`}
        sx={srOnly}
        aria-hidden="true"
      >
        {card.oracleText && `Card text: ${card.oracleText}`}
        {card.manaCost && `Mana cost: ${card.manaCost}`}
        {card.power && card.toughness && `Power/Toughness: ${card.power}/${card.toughness}`}
        {card.prices?.usd && `Price: $${card.prices.usd}`}
      </Box>

      <CardDetailsModal
        open={modalOpen}
        onClose={handleModalClose}
        card={card}
      />
    </MuiCard>
  );
};

// Memoized component with custom comparison for optimal performance
export const MtgCard = React.memo(MtgCardComponent, (prevProps, nextProps) => {
  // Custom comparison to prevent unnecessary re-renders

  // Card ID is the most important - if different, always re-render
  if (prevProps.card.id !== nextProps.card.id) return false;

  // Index or group changes should re-render
  if (prevProps.index !== nextProps.index) return false;
  if (prevProps.groupId !== nextProps.groupId) return false;

  // Context changes that affect display
  if (prevProps.context?.isOnSetPage !== nextProps.context?.isOnSetPage) return false;
  if (prevProps.context?.isOnArtistPage !== nextProps.context?.isOnArtistPage) return false;
  if (prevProps.context?.currentArtist !== nextProps.context?.currentArtist) return false;
  if (prevProps.context?.currentSet !== nextProps.context?.currentSet) return false;
  if (prevProps.context?.showCollectorInfo !== nextProps.context?.showCollectorInfo) return false;

  // Callback function references (usually stable but worth checking)
  if (prevProps.onSetClick !== nextProps.onSetClick) return false;
  if (prevProps.onArtistClick !== nextProps.onArtistClick) return false;

  // Class name changes
  if (prevProps.className !== nextProps.className) return false;

  // Card properties that affect visual display
  if (prevProps.card.imageUris !== nextProps.card.imageUris) return false;
  if (prevProps.card.cardFaces !== nextProps.card.cardFaces) return false;
  if (prevProps.card.rarity !== nextProps.card.rarity) return false;
  if (prevProps.card.finishes !== nextProps.card.finishes) return false;
  if (prevProps.card.promoTypes !== nextProps.card.promoTypes) return false;
  if (prevProps.card.frameEffects !== nextProps.card.frameEffects) return false;
  if (prevProps.card.promo !== nextProps.card.promo) return false;
  if (prevProps.card.name !== nextProps.card.name) return false;
  if (prevProps.card.collectorNumber !== nextProps.card.collectorNumber) return false;
  if (prevProps.card.artist !== nextProps.card.artist) return false;
  if (prevProps.card.setCode !== nextProps.card.setCode) return false;
  if (prevProps.card.setName !== nextProps.card.setName) return false;
  if (prevProps.card.scryfallUri !== nextProps.card.scryfallUri) return false;

  // Price changes (for display)
  if (prevProps.card.prices?.usd !== nextProps.card.prices?.usd) return false;
  if (prevProps.card.purchaseUris?.tcgplayer !== nextProps.card.purchaseUris?.tcgplayer) return false;

  // Collection data changes (critical for collector view)
  if ((prevProps.card as any).userCollection !== (nextProps.card as any).userCollection) return false;

  // If we get here, props are effectively the same
  return true;
});