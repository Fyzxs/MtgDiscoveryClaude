import React, { useState } from 'react';
import type { Card, CardContext } from '../../types/card';
import { CardImage } from '../atoms/CardImage';
import { PriceDisplay } from '../atoms/PriceDisplay';
import { CollectorInfo } from '../molecules/CollectorInfo';
import { CardMetadata } from '../molecules/CardMetadata';
import { ArtistInfo } from '../molecules/ArtistInfo';
import { CardLinks } from '../molecules/CardLinks';
import { ManaCost } from '../molecules/ManaCost';

interface CardDisplayProps {
  card: Card;
  context?: CardContext;
  showHover?: boolean;
  className?: string;
}

export const CardDisplay: React.FC<CardDisplayProps> = ({ 
  card, 
  context = {},
  showHover = true,
  className = ''
}) => {
  const [isHovered, setIsHovered] = useState(false);

  // Determine border color based on rarity
  const getBorderColor = (rarity?: string): string => {
    switch (rarity?.toLowerCase()) {
      case 'common':
        return 'border-gray-600 shadow-gray-600/50';
      case 'uncommon':
        return 'border-gray-400 shadow-gray-400/50';
      case 'rare':
        return 'border-yellow-600 shadow-yellow-600/50';
      case 'mythic':
        return 'border-orange-600 shadow-orange-600/50';
      case 'special':
      case 'bonus':
        return 'border-purple-600 shadow-purple-600/50';
      default:
        return 'border-gray-700 shadow-gray-700/50';
    }
  };

  const borderClass = getBorderColor(card.rarity);

  return (
    <div 
      className={`
        relative 
        bg-gray-900 
        rounded-xl 
        border-2 
        ${borderClass}
        ${isHovered ? 'shadow-lg' : 'shadow-md'}
        transition-all 
        duration-300
        ${className}
      `}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      <div className="p-4 space-y-4">
        {/* Card Image */}
        <div className="flex justify-center">
          <CardImage 
            imageUris={card.imageUris} 
            cardName={card.name}
            size="normal"
          />
        </div>

        {/* Card Info */}
        <div className="space-y-3">
          {/* Header with name and mana cost */}
          <div className="flex justify-between items-start">
            <CardMetadata
              name={card.name}
              typeLine={card.typeLine}
              setName={card.setName}
              releasedAt={card.releasedAt}
              context={context}
              className="flex-1"
            />
            {card.manaCost && (
              <ManaCost manaCost={card.manaCost} size="small" />
            )}
          </div>

          {/* Collector Info */}
          {context.showCollectorInfo && (
            <CollectorInfo
              collectorNumber={card.collectorNumber}
              setCode={card.setCode}
              rarity={card.rarity}
            />
          )}

          {/* Artist Info */}
          <ArtistInfo
            artist={card.artist}
            artistIds={card.artistIds}
            context={context}
          />

          {/* Price and Links */}
          <div className="flex justify-between items-center pt-2 border-t border-gray-800">
            <PriceDisplay 
              price={card.prices?.usd} 
              currency="usd"
              className="text-lg"
            />
            <CardLinks
              scryfallUri={card.scryfallUri}
              purchaseUris={card.purchaseUris}
              relatedUris={card.relatedUris}
              setCode={card.setCode}
              collectorNumber={card.collectorNumber}
              cardName={card.name}
            />
          </div>
        </div>

        {/* Hover overlay with additional info */}
        {showHover && isHovered && (
          <div className="absolute inset-x-0 bottom-0 bg-gray-900/95 backdrop-blur-sm rounded-b-xl p-4 border-t border-gray-700">
            <div className="space-y-2">
              {card.oracleText && (
                <p className="text-sm text-gray-300 italic">
                  {card.oracleText}
                </p>
              )}
              {card.flavorText && (
                <p className="text-xs text-gray-500 italic">
                  "{card.flavorText}"
                </p>
              )}
              {(card.power || card.toughness) && (
                <div className="text-right text-lg font-bold text-gray-400">
                  {card.power}/{card.toughness}
                </div>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};