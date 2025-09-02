import React, { useState, useCallback, useRef } from 'react';
import { Card as MuiCard, Box } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import type { Card, CardContext } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { ZoomIndicator } from '../atoms/Cards/ZoomIndicator';
import { CardOverlay } from '../molecules/Cards/CardOverlay';
import { CardDetailsModal } from './CardDetailsModal';
import { CardBadges } from '../atoms/Cards/CardBadges';
import { getRarityGlowStyles } from '../../utils/rarityStyles';
import { srOnly } from '../../styles/cardStyles';
import type { StyledComponentProps, SelectionProps } from '../../types/components';

interface MtgCardProps extends StyledComponentProps, SelectionProps {
  card: Card;
  context?: CardContext;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
}

export const MtgCard: React.FC<MtgCardProps> = React.memo(({ 
  card, 
  context = {},
  isSelected = false,
  onCardClick,
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  const [modalOpen, setModalOpen] = useState(false);
  const theme = useTheme();
  const cardRef = useRef<HTMLDivElement>(null);

  // Parse multiple artists
  const parseArtists = (artistString?: string): string[] => {
    if (!artistString) return [];
    return artistString.split(/\s+(?:&|and)\s+/i);
  };

  const artists = parseArtists(card.artist);
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a !== context.currentArtist)
    : artists;

  const selectCard = useCallback((cardElement: HTMLElement) => {
    // Only remove selected class from currently selected card (faster)
    const currentlySelected = document.querySelector('[data-mtg-card="true"].selected');
    if (currentlySelected && currentlySelected !== cardElement) {
      currentlySelected.classList.remove('selected');
      currentlySelected.setAttribute('data-selected', 'false');
      currentlySelected.setAttribute('aria-selected', 'false');
    }
    
    // Add selected class to this card
    cardElement.classList.add('selected');
    cardElement.setAttribute('data-selected', 'true');
    cardElement.setAttribute('aria-selected', 'true');
  }, []);

  const handleCardClick = useCallback((e: React.MouseEvent) => {
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
    selectCard(cardElement);
  }, [selectCard]);

  const handleKeyDown = useCallback((e: React.KeyboardEvent<HTMLDivElement>) => {
    const cardElement = e.currentTarget as HTMLElement;
    
    switch (e.key) {
      case 'Enter':
      case ' ':
        e.preventDefault();
        e.stopPropagation();
        selectCard(cardElement);
        break;
        
      case 'Escape':
        e.preventDefault();
        // Deselect card
        cardElement.classList.remove('selected');
        cardElement.setAttribute('data-selected', 'false');
        cardElement.setAttribute('aria-selected', 'false');
        break;
    }
  }, [selectCard]);

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
      onKeyDown={handleKeyDown}
      data-mtg-card="true"
      data-card-id={card.id}
      data-selected={isSelected ? "true" : "false"}
      tabIndex={0}
      role="button"
      aria-label={`${card.name} - ${card.rarity} ${card.typeLine || 'card'} from ${card.setName}. Artist: ${card.artist}. ${isSelected ? 'Selected' : 'Not selected'}`}
      aria-selected={isSelected}
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
        transition: 'transform 0.05s ease-in-out',
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
        onZoomClick={handleZoomClick}
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
});