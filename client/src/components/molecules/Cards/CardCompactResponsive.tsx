import React from 'react';
import type { Card, CardContext } from '../../../types/card';
import { CardImageDisplay } from './CardImageDisplay';
import { PriceDisplay } from '../../atoms';
import { CollectorNumber } from '../../atoms';
import { RarityBadge } from '../../atoms';
import { ArtistInfo } from './ArtistInfo';
import { CardLinks } from './CardLinks';
import { SetIcon } from '../../atoms';

interface CardCompactProps {
  card: Card;
  context?: CardContext;
  onClick?: () => void;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const CardCompact: React.FC<CardCompactProps> = ({ 
  card, 
  context = {},
  onClick,
  onCardClick,
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  // Determine border glow based on rarity
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

  const handleCardNameClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (onCardClick) {
      onCardClick(card.id);
    }
  };

  const handleSetNameClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (onSetClick) {
      onSetClick(card.setCode);
    }
  };

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
      <div className="relative aspect-[745/1040]">
        <CardImageDisplay 
          card={card}
          size="normal"
          showFlipButton={false}
          className="w-full h-full object-cover"
        />
        
        {/* Overlay info at bottom of image */}
        <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-gray-900 via-gray-900/90 to-transparent p-2 sm:p-3">
          {/* Collector info row */}
          <div className="flex justify-between items-center mb-1 sm:mb-2">
            <CollectorNumber 
              number={card.collectorNumber} 
              setCode={card.setCode}
              className="text-[10px] sm:text-xs"
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
            onArtistClick={onArtistClick}
            className="text-[10px] sm:text-xs mb-1 sm:mb-2"
          />

          {/* Card Name - conditional display */}
          {!context.isOnCardPage && (
            <h4 className="text-xs sm:text-sm font-bold text-white mb-0.5 sm:mb-1 line-clamp-1">
              <a
                href={`/cards/${card.id}`}
                onClick={handleCardNameClick}
                className="hover:text-gray-300 transition-colors"
              >
                {card.name}
              </a>
            </h4>
          )}

          {/* Set Name with icon - conditional display */}
          {!context.isOnSetPage && card.setName && (
            <div className="flex items-center gap-1 mb-1 sm:mb-2">
              <a
                href={`/set/${card.setCode?.toLowerCase()}`}
                onClick={handleSetNameClick}
                className="flex items-center gap-1 text-[10px] sm:text-xs text-gray-400 hover:text-white transition-colors line-clamp-1"
              >
                {card.setCode && (
                  <SetIcon 
                    setCode={card.setCode} 
                    rarity={card.rarity}
                    size="small"
                    className="flex-shrink-0"
                  />
                )}
                <span className="truncate">{card.setName}</span>
              </a>
            </div>
          )}

          {/* Price and Links */}
          <div className="flex justify-between items-center">
            <PriceDisplay 
              price={card.prices?.usd} 
              currency="usd"
              className="text-xs sm:text-sm font-bold"
            />
            <CardLinks
              scryfallUrl={card.scryfallUri}
              cardName={card.name}
              className="scale-[0.6] sm:scale-75 origin-right"
            />
          </div>
        </div>
      </div>
    </div>
  );
};