import React, { useRef } from 'react';
import { Card as MuiCard, Box } from '../../atoms';
import type { Card, CardContext } from '../../../types/card';
import { CardImageDisplay } from './CardImageDisplay';
import { ZoomIndicator } from '../../atoms/Cards/ZoomIndicator';
import { DeltaBadge } from '../../molecules/shared/badges';
import { CardOverlay } from '../../molecules/Cards/CardOverlay';
import { CollectionEntryOverlay } from '../../molecules/Cards/CollectionEntryOverlay';
import { CardDetailsModal } from './CardDetailsModal';
import { CardBadges } from '../../molecules/Cards/CardBadges';
import { srOnly } from '../../../styles/cardStyles';
import type { StyledComponentProps } from '../../../types/components';
// Extracted hooks
import { useMtgCardStyles } from '../../../hooks/useMtgCardStyles';
import { useMtgCardCollectionActions } from '../../../hooks/useMtgCardCollectionActions';
import { useMtgCardInteractions } from '../../../hooks/useMtgCardInteractions';
import { useMtgCardMemo, mtgCardPropsComparison } from '../../../hooks/useMtgCardMemo';
import { useCardDisplaySettings, type CardSize, type DisplayMode } from '../../../hooks/useCardDisplaySettings';

interface MtgCardProps extends StyledComponentProps {
  card: Card;
  context?: CardContext;
  index: number;
  groupId: string;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  // Responsive props
  size?: CardSize;
  displayMode?: DisplayMode;
}


const MtgCardComponent: React.FC<MtgCardProps> = ({
  card,
  context = {},
  index,
  // groupId is passed for memoization comparison in mtgCardPropsComparison
  groupId: _groupId,
  onSetClick,
  onArtistClick,
  className = '',
  size: explicitSize,
  displayMode: explicitMode
}) => {
  const cardRef = useRef<HTMLDivElement>(null);

  // Get responsive display settings
  const displaySettings = useCardDisplaySettings({
    explicitSize,
    explicitMode
  });

  // Use extracted hooks
  const { ariaLabel } = useMtgCardMemo({ card });
  const { cardStyles } = useMtgCardStyles({ card });
  const {
    modalOpen,
    isSelected,
    overlayExpanded,
    handleCardClick,
    handleZoomClick,
    handleModalClose,
    handleOverlayToggle
  } = useMtgCardInteractions({
    cardRef,
    overlayBehavior: displaySettings.overlayBehavior
  });
  const { overlayState, isWishlistMode } = useMtgCardCollectionActions({ card, isSelected, cardRef });

  // Calculate collection count for mobile display
  const collectionCount = card.userCollection?.totalCount ?? 0;

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
        ...cardStyles,
        // Ensure minimum touch target on mobile
        minHeight: displaySettings.isMobile ? 44 : undefined,
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
          size={displaySettings.imageScryfallSize}
          showFlipButton={!displaySettings.isMobile && !displaySettings.isTablet}
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%'
          }}
        />
      </Box>

      {displaySettings.showBadges && (
        <CardBadges
          foil={card.foil}
          nonfoil={card.nonFoil}
          etched={card.finishes?.includes('etched')}
          promoTypes={card.promoTypes}
          frameEffects={card.frameEffects}
          isPromo={card.promo}
          digital={card.digital}
        />
      )}

      {/* Last modification delta badge */}
      <DeltaBadge itemId={card.id} positioned />

      {displaySettings.showZoomIndicator && (
        <ZoomIndicator
          onZoomClick={handleZoomClick}
        />
      )}


      <CardOverlay
        card={card}
        isSelected={false}
        context={context}
        onCardClick={undefined}
        onArtistClick={onArtistClick}
        onSetClick={onSetClick}
        variant={displaySettings.overlayVariant}
        expanded={overlayExpanded}
        onExpandToggle={handleOverlayToggle}
        collectionCount={collectionCount}
      />

      {/* Collection entry overlay - always mounted for pre-mount pattern */}
      <CollectionEntryOverlay
        visible={overlayState.visible}
        count={overlayState.count}
        isNegative={overlayState.isNegative}
        finish={overlayState.finish}
        special={overlayState.special}
        mode={isWishlistMode ? 'wishlist' : 'collection'}
        variant="card"
        flash={overlayState.flash}
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
export const MtgCard = React.memo(MtgCardComponent, mtgCardPropsComparison);
