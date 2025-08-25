import React from 'react';
import { Box, Typography } from '@mui/material';
import { RarityCollectorBadge } from './RarityCollectorBadge';
import { ArtistLinks } from './ArtistLinks';
import { CardName } from '../../atoms/Cards/CardName';
import { SetLink } from '../../atoms/Cards/SetLink';
import { PriceDisplay } from '../../atoms/shared/PriceDisplay';
import { CardLinks } from './CardLinks';
import type { CardContext } from '../../../types/card';

interface CardOverlayProps {
  cardId?: string;
  cardName?: string;
  rarity?: string;
  collectorNumber?: string;
  releaseDate?: string;
  artists: string[];
  artistIds?: string[];
  setCode?: string;
  setName?: string;
  price?: string | null;
  scryfallUrl?: string;
  tcgplayerUrl?: string;
  isSelected?: boolean;
  context?: CardContext;
  onCardClick?: (cardId?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  className?: string;
}

export const CardOverlay: React.FC<CardOverlayProps> = ({ 
  cardId,
  cardName,
  rarity,
  collectorNumber,
  releaseDate,
  artists,
  artistIds,
  setCode,
  setName,
  price,
  scryfallUrl,
  tcgplayerUrl,
  isSelected = false,
  context = {},
  onCardClick,
  onArtistClick,
  onSetClick,
  className 
}) => {
  // Format date
  const formatDate = (dateString?: string): string => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
  };

  return (
    <Box
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        background: isSelected 
          ? 'linear-gradient(to top, rgba(1,32,96,1) 0%, rgba(1,32,96,0.95) 60%, rgba(1,32,96,0) 100%)'
          : 'linear-gradient(to top, rgba(0,0,0,1) 0%, rgba(0,0,0,0.95) 60%, rgba(0,0,0,0) 100%)',
        pt: 7,
        zIndex: 10,
        opacity: 1,
        transition: 'all 0.3s ease-in-out'
      }}
      className={`card-overlay ${className || ''}`}
    >
      <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        {/* Collector Info Row */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <RarityCollectorBadge 
            rarity={rarity}
            collectorNumber={collectorNumber}
          />
          {/* Date moved to top row */}
          {releaseDate && (!context.isOnSetPage || context.currentSetCode !== setCode) && (
            <Typography variant="caption" sx={{ fontSize: '0.625rem', color: 'grey.400' }}>
              {formatDate(releaseDate)}
            </Typography>
          )}
        </Box>

        {/* Artist(s) */}
        {artists.length > 0 && (
          <ArtistLinks
            artists={artists}
            artistIds={artistIds}
            onArtistClick={onArtistClick}
          />
        )}

        {/* Card Name */}
        {!context.isOnCardPage && (
          <CardName
            cardId={cardId}
            cardName={cardName}
            onCardClick={onCardClick}
          />
        )}

        {/* Set Name with Icon */}
        {!context.isOnSetPage && (
          <SetLink
            setCode={setCode}
            setName={setName}
            rarity={rarity}
            onSetClick={onSetClick}
          />
        )}

        {/* Price and Links Row */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', pt: 0.5 }}>
          <PriceDisplay 
            price={price} 
            currency="usd"
            className="text-sm"
          />
          <CardLinks
            scryfallUrl={scryfallUrl}
            tcgplayerUrl={tcgplayerUrl}
            cardName={cardName}
          />
        </Box>
      </Box>
    </Box>
  );
};