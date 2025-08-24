import React from 'react';
import { SetIcon } from '../atoms/SetIcon';
import type { CardContext } from '../../types/card';

interface CardMetadataProps {
  name?: string;
  cardId?: string;
  typeLine?: string;
  setName?: string;
  setCode?: string;
  rarity?: string;
  releasedAt?: string;
  context?: CardContext;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  className?: string;
}

export const CardMetadata: React.FC<CardMetadataProps> = ({ 
  name,
  cardId,
  typeLine,
  setName,
  setCode,
  rarity,
  releasedAt,
  context = {},
  onCardClick,
  onSetClick,
  className = ''
}) => {
  // Don't show card name on card page
  const showName = !context.isOnCardPage && name;
  
  // Don't show set name on set page
  const showSetName = !context.isOnSetPage && setName;

  // Format date
  const formatDate = (dateString?: string): string => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      year: 'numeric' 
    });
  };

  const formattedDate = formatDate(releasedAt);

  // Only show date on set page if it differs from set release date
  const showDate = !context.isOnSetPage || context.currentSetCode !== releasedAt;

  const handleCardClick = (e: React.MouseEvent) => {
    if (onCardClick) {
      e.preventDefault();
      onCardClick(cardId);
    }
  };

  const handleSetClick = (e: React.MouseEvent) => {
    if (onSetClick) {
      e.preventDefault();
      onSetClick(setCode);
    }
  };

  return (
    <div className={`space-y-1 ${className}`}>
      {showName && (
        <h3 className="text-sm sm:text-base lg:text-lg font-bold text-white">
          <a
            href={`/cards/${cardId || encodeURIComponent(name.toLowerCase().replace(/\s+/g, '-'))}`}
            onClick={handleCardClick}
            className="hover:text-gray-300 transition-colors"
          >
            {name}
          </a>
        </h3>
      )}
      {typeLine && (
        <p className="text-xs sm:text-sm text-gray-400 line-clamp-1">
          {typeLine}
        </p>
      )}
      <div className="flex items-center gap-1 sm:gap-2 text-xs text-gray-500 flex-wrap">
        {showSetName && (
          <>
            <a
              href={`/sets/${setCode?.toLowerCase()}`}
              onClick={handleSetClick}
              className="flex items-center gap-1 text-gray-400 hover:text-white transition-colors"
            >
              {setCode && (
                <SetIcon 
                  setCode={setCode} 
                  rarity={rarity}
                  size="small"
                  className="inline-block"
                />
              )}
              <span className="truncate max-w-[150px] sm:max-w-none">{setName}</span>
            </a>
            {(showDate && formattedDate) && <span className="hidden sm:inline">•</span>}
          </>
        )}
        {showDate && formattedDate && (
          <span className="text-[10px] sm:text-xs">{formattedDate}</span>
        )}
      </div>
    </div>
  );
};