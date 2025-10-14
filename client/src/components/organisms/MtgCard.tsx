import React, { useRef } from 'react';
import { Card as MuiCard, Box } from '@mui/material';
import type { Card, CardContext, UserCardData } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { ZoomIndicator } from '../atoms/Cards/ZoomIndicator';
import { CardOverlay } from '../molecules/Cards/CardOverlay';
import { CardDetailsModal } from './CardDetailsModal';
import { CardBadges } from '../molecules/Cards/CardBadges';
import { srOnly } from '../../styles/cardStyles';
import type { StyledComponentProps } from '../../types/components';
// Extracted molecules
import { useMtgCardStyles } from '../molecules/Cards/MtgCardStyles';
import { useMtgCardCollectionActions } from '../molecules/Cards/MtgCardCollectionActions';
import { useMtgCardInteractions } from '../molecules/Cards/MtgCardInteractions';
import { useMtgCardMemo, mtgCardPropsComparison } from '../molecules/Cards/MtgCardMemo';

interface MtgCardProps extends StyledComponentProps {
  card: Card;
  context?: CardContext;
  collectionData?: UserCardData | UserCardData[];
  index: number;
  groupId: string;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
}


const MtgCardComponent: React.FC<MtgCardProps> = ({
  card,
  context = {},
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  collectionData: _collectionData, // Used for memoization comparison
  index,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  groupId: _groupId, // Used for memoization comparison
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  const cardRef = useRef<HTMLDivElement>(null);

  // Use extracted hooks
  const { ariaLabel } = useMtgCardMemo({ card });
  const { cardStyles } = useMtgCardStyles({ card });
  const { modalOpen, isSelected, handleCardClick, handleZoomClick, handleModalClose } = useMtgCardInteractions({ cardRef });
  useMtgCardCollectionActions({ card, isSelected, cardRef });

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
      sx={cardStyles}
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
        etched={card.finishes?.includes('etched')}
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
export const MtgCard = React.memo(MtgCardComponent, mtgCardPropsComparison);