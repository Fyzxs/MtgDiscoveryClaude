import React, { useState } from 'react';
import { Card as MuiCard, Box } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import type { Card, CardContext } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { ZoomIndicator } from '../atoms/Cards/ZoomIndicator';
import { CardOverlay } from '../molecules/Cards/CardOverlay';
import { CardDetailsModal } from './CardDetailsModal';
import { CardBadges } from '../atoms/Cards/CardBadges';
import { getRarityGlowStyles, getCardTransform } from '../../utils/rarityStyles';

interface MtgCardProps {
  card: Card;
  context?: CardContext;
  isSelected?: boolean;
  onSelectionChange?: (cardId: string, selected: boolean) => void;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const MtgCard: React.FC<MtgCardProps> = React.memo(({ 
  card, 
  context = {},
  isSelected = false,
  onSelectionChange,
  onCardClick,
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  const [modalOpen, setModalOpen] = useState(false);
  const theme = useTheme();

  // Parse multiple artists
  const parseArtists = (artistString?: string): string[] => {
    if (!artistString) return [];
    return artistString.split(/\s+(?:&|and)\s+/i);
  };

  const artists = parseArtists(card.artist);
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a !== context.currentArtist)
    : artists;

  // Debug: Log what we're receiving
  // console.log('MtgCard Debug:', {
  //   cardName: card?.name,
  //   artist: card?.artist,
  //   collectorNumber: card?.collectorNumber,
  //   rarity: card?.rarity,
  //   displayArtists,
  //   context
  // });



  const handleCardClick = (e: React.MouseEvent) => {
    // Don't trigger selection if clicking on links or zoom indicator
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');
    const clickedZoom = target.closest('.zoom-indicator');
    
    // If clicking a link, don't do anything - let the link handle itself
    if (clickedLink || clickedZoom) {
      return;
    }
    
    e.preventDefault();
    e.stopPropagation();
    
    // Simple CSS class manipulation - no React state needed
    const cardElement = e.currentTarget as HTMLElement;
    
    // Only remove selected class from currently selected card (faster)
    const currentlySelected = document.querySelector('[data-mtg-card="true"].selected');
    if (currentlySelected && currentlySelected !== cardElement) {
      currentlySelected.classList.remove('selected');
      currentlySelected.setAttribute('data-selected', 'false');
    }
    
    // Add selected class to this card
    cardElement.classList.add('selected');
    cardElement.setAttribute('data-selected', 'true');
    
    // Don't update React state - it causes lag even with setTimeout
    // We'll query the DOM when we need to know which card is selected
  };

  return (
    <MuiCard 
      elevation={4}
      onClick={handleCardClick}
      data-mtg-card="true"
      data-card-id={card.id}
      data-selected={isSelected ? "true" : "false"}
      tabIndex={0}
      sx={{
        position: 'relative',
        width: '280px',
        bgcolor: 'grey.800',
        borderRadius: '12px',
        border: '3px solid',
        borderColor: 'grey.700',
        overflow: 'hidden',
        boxShadow: theme.mtg.shadows.card.normal,
        transition: 'transform 0.15s ease-in-out',
        transform: 'scale(1)',
        cursor: 'pointer',
        outline: 'none',
        // Selected state using CSS class
        '&.selected': {
          border: '4px solid',
          borderColor: '#1976d2 !important',
          boxShadow: `${theme.mtg.shadows.card.selected}, ${theme.mtg.shadows.card.normal} !important`,
          transform: 'scale(1.05) !important',
          transition: 'none !important',
          '& .zoom-indicator': {
            opacity: 1,
            transform: 'scale(1)'
          }
        },
        '&:focus': {
          outline: '2px solid',
          outlineColor: 'primary.light',
          outlineOffset: '2px'
        },
        '&:hover:not(.selected)': {
          ...getRarityGlowStyles(card.rarity, false, true),
          transform: 'scale(1.01)',
          '& .zoom-indicator': {
            opacity: 1,
            transform: 'scale(1)'
          }
        }
      }}
      className={`${className} ${isSelected ? 'selected' : ''}`}
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
        finishes={card.finishes}
        promoTypes={card.promoTypes}
        frameEffects={card.frameEffects}
        isPromo={card.promo}
      />

      <ZoomIndicator
        onZoomClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          setModalOpen(true);
        }}
      />
      
      <CardOverlay
        cardId={card.id}
        cardName={card.name}
        rarity={card.rarity}
        collectorNumber={card.collectorNumber}
        releaseDate={card.releasedAt}
        artists={displayArtists}
        artistIds={card.artistIds}
        setCode={card.setCode}
        setName={card.setName}
        price={card.prices?.usd}
        scryfallUrl={card.scryfallUri}
        tcgplayerUrl={card.purchaseUris?.tcgplayer}
        isSelected={isSelected}
        context={context}
        onCardClick={onCardClick}
        onArtistClick={onArtistClick}
        onSetClick={onSetClick}
      />

      <CardDetailsModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        card={card}
      />
    </MuiCard>
  );
});