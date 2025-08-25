import React from 'react';
import { Box, Typography, Link, Stack } from '@mui/material';
import { SetIcon } from '../../atoms/Sets/SetIcon';
import type { CardContext } from '../../../types/card';

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
    <Stack spacing={1} className={className}>
      {showName && (
        <Typography 
          variant="subtitle2" 
          component="h3"
          sx={{ 
            fontWeight: 'bold',
            color: 'white',
            fontSize: { xs: '0.875rem', sm: '1rem', lg: '1.125rem' }
          }}
        >
          <Link
            href={`/cards/${cardId || encodeURIComponent(name.toLowerCase().replace(/\s+/g, '-'))}`}
            onClick={handleCardClick}
            sx={{
              color: 'inherit',
              textDecoration: 'none',
              transition: 'color 0.2s',
              '&:hover': {
                color: 'grey.300'
              }
            }}
          >
            {name}
          </Link>
        </Typography>
      )}
      {typeLine && (
        <Typography 
          variant="caption" 
          sx={{ 
            color: 'text.secondary',
            fontSize: { xs: '0.75rem', sm: '0.875rem' },
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            whiteSpace: 'nowrap'
          }}
        >
          {typeLine}
        </Typography>
      )}
      <Box sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        gap: { xs: 1, sm: 2 },
        flexWrap: 'wrap'
      }}>
        {showSetName && (
          <>
            <Link
              href={`/sets/${setCode?.toLowerCase()}`}
              onClick={handleSetClick}
              sx={{
                display: 'flex',
                alignItems: 'center',
                gap: 0.5,
                color: 'text.secondary',
                textDecoration: 'none',
                transition: 'color 0.2s',
                '&:hover': {
                  color: 'white'
                }
              }}
            >
              {setCode && (
                <SetIcon 
                  setCode={setCode} 
                  rarity={rarity}
                  size="small"
                  className="inline-block"
                />
              )}
              <Typography 
                variant="caption"
                component="span"
                sx={{ 
                  overflow: 'hidden',
                  textOverflow: 'ellipsis',
                  maxWidth: { xs: '150px', sm: 'none' }
                }}
              >
                {setName}
              </Typography>
            </Link>
            {(showDate && formattedDate) && (
              <Typography 
                variant="caption" 
                component="span"
                sx={{ 
                  color: 'text.disabled',
                  display: { xs: 'none', sm: 'inline' }
                }}
              >
                •
              </Typography>
            )}
          </>
        )}
        {showDate && formattedDate && (
          <Typography 
            variant="caption"
            sx={{ 
              color: 'text.disabled',
              fontSize: { xs: '0.625rem', sm: '0.75rem' }
            }}
          >
            {formattedDate}
          </Typography>
        )}
      </Box>
    </Stack>
  );
};