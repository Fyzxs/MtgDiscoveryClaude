import React, { useState } from 'react';
import { Card as MuiCard } from '@mui/material';
import type { Card, CardContext } from '../../types/card';
import { CardImage } from '../atoms/Cards/CardImage';
import { ZoomIndicator } from '../atoms/Cards/ZoomIndicator';
import { CardOverlay } from '../molecules/Cards/CardOverlay';
import { CardModal } from '../atoms/Cards/CardModal';

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

export const MtgCard: React.FC<MtgCardProps> = ({ 
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
  const [isZoomClicked, setIsZoomClicked] = useState(false);

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
    // Don't trigger selection if zoom was just clicked
    if (isZoomClicked) {
      setIsZoomClicked(false);
      return;
    }
    
    // Don't trigger selection if clicking on links
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');
    
    if (!clickedLink && onSelectionChange) {
      e.preventDefault();
      e.stopPropagation();
      // Only select, never deselect when clicking the card
      if (!isSelected) {
        onSelectionChange(card.id || '', true);
      }
    }
  };

  return (
    <MuiCard 
      elevation={4}
      onClick={handleCardClick}
      onFocus={() => {
        // Select card when it gains focus
        if (onSelectionChange && !isSelected) {
          onSelectionChange(card.id || '', true);
        }
      }}
      onBlur={() => {
        // Deselect card when it loses focus
        if (onSelectionChange && isSelected) {
          onSelectionChange(card.id || '', false);
        }
      }}
      data-mtg-card="true"
      tabIndex={0}
      sx={{
        position: 'relative',
        width: '280px',
        bgcolor: 'grey.800',
        borderRadius: 6,
        border: isSelected ? '4px solid' : '3px solid',
        borderColor: isSelected ? '#2196F3' : 'grey.700',
        overflow: 'hidden',
        boxShadow: isSelected 
          ? '0 0 50px rgba(33, 150, 243, 0.8), 0 0 30px rgba(33, 150, 243, 0.6), 0 25px 50px -12px rgba(0, 0, 0, 0.25)'
          : '0 25px 50px -12px rgba(0, 0, 0, 0.25)',
        transition: 'all 0.3s ease-in-out',
        transform: isSelected ? 'scale(1.03)' : 'scale(1)',
        cursor: onSelectionChange ? 'pointer' : 'default',
        outline: 'none',
        '&:focus': {
          outline: '2px solid',
          outlineColor: 'primary.light',
          outlineOffset: '2px'
        },
        // Combined hover effect with rarity-based glow and overlay fade
        ...(card.rarity ? {
          '&:hover': {
            boxShadow: isSelected 
              ? '0 0 50px rgba(33, 150, 243, 0.8), 0 25px 50px -12px rgba(0, 0, 0, 0.5)'
              : card.rarity.toLowerCase() === 'common' ? '0 0 40px rgba(156, 163, 175, 0.5), 0 0 20px rgba(156, 163, 175, 0.3)' :
                card.rarity.toLowerCase() === 'uncommon' ? '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)' :
                card.rarity.toLowerCase() === 'rare' ? '0 0 40px rgba(217, 119, 6, 0.5), 0 0 20px rgba(217, 119, 6, 0.3)' :
                card.rarity.toLowerCase() === 'mythic' ? '0 0 40px rgba(234, 88, 12, 0.6), 0 0 20px rgba(234, 88, 12, 0.4)' :
                card.rarity.toLowerCase() === 'special' || card.rarity.toLowerCase() === 'bonus' ? '0 0 40px rgba(147, 51, 234, 0.5), 0 0 20px rgba(147, 51, 234, 0.3)' :
                '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)',
            transform: isSelected ? 'scale(1.03)' : 'scale(1.01)',
            '& .card-overlay': {
              opacity: 0.5
            },
            '& .zoom-indicator': {
              opacity: 1,
              transform: 'scale(1)'
            }
          }
        } : {
          '&:hover': {
            boxShadow: isSelected 
              ? '0 0 50px rgba(33, 150, 243, 0.8), 0 25px 50px -12px rgba(0, 0, 0, 0.5)'
              : '0 0 40px rgba(156, 163, 175, 0.4), 0 25px 50px -12px rgba(0, 0, 0, 0.5)',
            transform: isSelected ? 'scale(1.03)' : 'scale(1.01)',
            '& .card-overlay': {
              opacity: 0.5
            },
            '& .zoom-indicator': {
              opacity: 1,
              transform: 'scale(1)'
            }
          }
        })
      }}
      className={className}
    >
      <CardImage
        imageUrl={card.imageUris?.normal || card.imageUris?.large}
        cardName={card.name}
      />

      <ZoomIndicator
        onZoomClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          setIsZoomClicked(true);
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

      <CardModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        imageUrl={card.imageUris?.large || card.imageUris?.png || card.imageUris?.normal}
        cardName={card.name}
      />
    </MuiCard>
  );
};