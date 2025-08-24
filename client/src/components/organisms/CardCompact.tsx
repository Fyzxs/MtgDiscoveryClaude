import React from 'react';
import type { Card, CardContext } from '../../types/card';
import { CardImage } from '../atoms/CardImage';
import { PriceDisplay } from '../atoms/PriceDisplay';
import { CollectorNumber } from '../atoms/CollectorNumber';
import { RarityBadge } from '../atoms/RarityBadge';
import { ArtistInfo } from '../molecules/ArtistInfo';
import { CardLinks } from '../molecules/CardLinks';

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
  // Determine border color based on rarity
  const getBorderGlow = (rarity?: string): string => {
    switch (rarity?.toLowerCase()) {
      case 'common':
        return 'hover:shadow-gray-600/30';
      case 'uncommon':
        return 'hover:shadow-gray-400/30';
      case 'rare':
        return 'hover:shadow-yellow-600/30';
      case 'mythic':
        return 'hover:shadow-orange-600/30';
      case 'special':
      case 'bonus':
        return 'hover:shadow-purple-600/30';
      default:
        return 'hover:shadow-gray-700/30';
    }
  };

  const glowClass = getBorderGlow(card.rarity);

  return (
    <div 
      className={`
        relative 
        bg-gray-900 
        rounded-lg 
        overflow-hidden
        hover:shadow-lg
        ${glowClass}
        transition-all 
        duration-300
        cursor-pointer
        ${className}
      `}
      onClick={onClick}
    >
      {/* Card Image */}
      <div className="relative">
        <CardImage 
          imageUris={card.imageUris} 
          cardName={card.name}
          size="normal"
          className="w-full"
        />
        
        {/* Overlay info at bottom of image */}
        <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-gray-900 via-gray-900/90 to-transparent p-3">
          {/* Collector info row */}
          <div className="flex justify-between items-center mb-2">
            <CollectorNumber 
              number={card.collectorNumber} 
              setCode={card.setCode}
              className="text-xs"
            />
            {card.rarity && (
              <RarityBadge rarity={card.rarity} />
            )}
          </div>

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
          <div className="flex justify-between items-center mt-2">
            <PriceDisplay 
              price={card.prices?.usd} 
              currency="usd"
              className="text-sm"
            />
            <CardLinks
              scryfallUri={card.scryfallUri}
              purchaseUris={card.purchaseUris}
              relatedUris={card.relatedUris}
              setCode={card.setCode}
              collectorNumber={card.collectorNumber}
              cardName={card.name}
              className="scale-75 origin-right"
            />
          </div>
        </div>
      </div>
    </div>
  );
};