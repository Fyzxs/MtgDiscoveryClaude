import React from 'react';
import { Box, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { RarityCollectorBadge } from './RarityCollectorBadge';
import { ArtistLinks } from './ArtistLinks';
import { CardName } from '../../atoms/Cards/CardName';
import { SetLink } from '../../atoms/Cards/SetLink';
import { PriceDisplay } from '../../atoms/shared/PriceDisplay';
import { CardLinks } from './CardLinks';
import { CollectionSummary } from './CollectionSummary';
import { formatReleaseDate } from '../../../utils/dateFormatters';
import type { CardContext, UserCardData } from '../../../types/card';

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
  collectionData?: UserCardData | UserCardData[];
  onCardClick?: (cardId?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  className?: string;
}

export const CardOverlay: React.FC<CardOverlayProps> = React.memo(({
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
  collectionData,
  onCardClick,
  onArtistClick,
  onSetClick,
  className
}) => {
  const theme = useTheme();

  return (
    <Box
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        pt: 7,
        zIndex: 10,
        opacity: 1,
        transition: 'none',
        '&::before': {
          content: '""',
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          background: theme.mtg.gradients.cardOverlay,
          opacity: isSelected ? 0 : 1,
          transition: 'none',
          pointerEvents: 'none',
          zIndex: -1
        },
        '.MuiCard-root:not(.selected):hover &::before': {
          opacity: 0
        },
        '.MuiCard-root.selected &::before': {
          opacity: '0 !important'
        }
      }}
      className={`card-overlay ${className || ''}`}
    >
      <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        {/* Release Date Row - now at the top */}
        {releaseDate && !context.hideReleaseDate && (
          <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
            <Typography variant="caption" sx={{ fontSize: '0.625rem', color: 'grey.400' }}>
              {formatReleaseDate(releaseDate)}
            </Typography>
          </Box>
        )}

        {/* Collector Info Row */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            <RarityCollectorBadge
              rarity={rarity}
              collectorNumber={collectorNumber}
            />
          </Box>
          {context.hasCollector && (
            <CollectionSummary
              collectionData={collectionData}
              size="small"
            />
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
        {!context.hideSetInfo && (
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
});