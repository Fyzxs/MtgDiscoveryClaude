import React from 'react';
import { Box } from '@mui/material';
import type { Card, CardContext } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { PriceDisplay } from '../atoms/shared/PriceDisplay';
import { CollectorNumber } from '../atoms/Cards/CollectorNumber';
import { RarityBadge } from '../atoms/Cards/RarityBadge';
import { ArtistInfo } from '../molecules/Cards/ArtistInfo';
import { CardLinks } from '../molecules/Cards/CardLinks';

interface CardCompactProps {
  card: Card;
  context?: CardContext;
  onClick?: () => void;
  className?: string;
}

export const CardCompact: React.FC<CardCompactProps> = ({ 
  card, 
  context = {},
  onClick,
  className = ''
}) => {
  // Determine border glow based on rarity
  const getBorderGlow = (rarity?: string) => {
    switch (rarity?.toLowerCase()) {
      case 'common':
        return { '&:hover': { boxShadow: '0 0 20px rgba(156, 163, 175, 0.3)' } };
      case 'uncommon':
        return { '&:hover': { boxShadow: '0 0 20px rgba(156, 163, 175, 0.4)' } };
      case 'rare':
        return { '&:hover': { boxShadow: '0 0 20px rgba(217, 119, 6, 0.3)' } };
      case 'mythic':
        return { '&:hover': { boxShadow: '0 0 20px rgba(234, 88, 12, 0.4)' } };
      case 'special':
      case 'bonus':
        return { '&:hover': { boxShadow: '0 0 20px rgba(147, 51, 234, 0.3)' } };
      default:
        return {};
    }
  };

  return (
    <Box 
      sx={{
        position: 'relative',
        bgcolor: 'grey.900',
        borderRadius: 2,
        overflow: 'hidden',
        cursor: 'pointer',
        transition: 'all 0.3s',
        '&:hover': {
          boxShadow: 3
        },
        ...getBorderGlow(card.rarity)
      }}
      onClick={onClick}
      className={className}
    >
      {/* Card Image */}
      <Box sx={{ position: 'relative' }}>
        <CardImageDisplay 
          card={card}
          size="normal"
          showFlipButton={false}
          className="w-full"
        />
        
        {/* Overlay info at bottom of image */}
        <Box 
          sx={{ 
            position: 'absolute',
            bottom: 0,
            left: 0,
            right: 0,
            background: 'linear-gradient(to top, rgba(17, 24, 39, 1) 0%, rgba(17, 24, 39, 0.9) 50%, transparent 100%)',
            p: 3
          }}
        >
          {/* Collector info row */}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
            <CollectorNumber 
              number={card.collectorNumber} 
              setCode={card.setCode}
              sx={{ fontSize: '0.75rem' }}
            />
            {card.rarity && (
              <RarityBadge rarity={card.rarity} />
            )}
          </Box>

          {/* Artist */}
          <ArtistInfo
            artist={card.artist}
            artistIds={card.artistIds}
            context={context}
            className="text-xs mb-2"
          />

          {/* Card Name - conditional display */}
          {!context.isOnCardPage && (
            <h4 className="text-sm font-bold text-white mb-1 line-clamp-1">
              {card.name}
            </h4>
          )}

          {/* Set Name - conditional display */}
          {!context.isOnSetPage && card.setName && (
            <p className="text-xs text-gray-400 line-clamp-1">
              {card.setName}
            </p>
          )}

          {/* Price and Links */}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 2 }}>
            <PriceDisplay 
              price={card.prices?.usd} 
              currency="usd"
              className="text-sm"
            />
            <CardLinks
              scryfallUrl={card.scryfallUri}
              cardName={card.name}
              className="scale-75 origin-right"
            />
          </Box>
        </Box>
      </Box>
    </Box>
  );
};