import React from 'react';
import { Box, Typography, Collapse } from '../../atoms';
import { useTheme } from '../../atoms';
import { RarityCollectorBadge } from './RarityCollectorBadge';
import { ArtistLinks } from './ArtistLinks';
import { CardName, SetLink, PriceDisplay } from '../../atoms';
import { CardLinks } from './CardLinks';
import { CollectionSummary } from './CollectionSummary';
import { formatReleaseDate } from '../../../utils/dateFormatters';
import { formatCollectionCount } from '../../../utils/collectionFormatters';
import { touchTargetStyles } from '../../../styles/touchTargets';
import type { Card, CardContext } from '../../../types/card';
import type { OverlayVariant } from '../../../hooks/useCardDisplaySettings';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

interface CardOverlayProps {
  card: Card;
  isSelected?: boolean;
  context?: CardContext;
  onCardClick?: (cardId?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  className?: string;
  // Responsive props
  variant?: OverlayVariant;
  expanded?: boolean;
  onExpandToggle?: () => void;
  collectionCount?: number;
}

/**
 * CardOverlay with responsive variants:
 * - minimal: Mobile collapsed - shows collection count + card name only
 * - compact: Mobile expanded/Tablet - shows count + name, artist, set, price
 * - full: Desktop - shows all information (current behavior)
 */
export const CardOverlay: React.FC<CardOverlayProps> = React.memo(({
  card,
  isSelected = false,
  context = {},
  onCardClick,
  onArtistClick,
  onSetClick,
  className,
  variant = 'full',
  expanded = false,
  collectionCount = 0
}) => {
  const theme = useTheme();
  const { isMobile } = useResponsiveBreakpoints();

  // Extract properties from card object
  const cardId = card.id;
  const cardName = card.name;
  const rarity = card.rarity;
  const collectorNumber = card.collectorNumber;
  const reserved = card.reserved;
  const releaseDate = card.releasedAt;
  const artist = card.artist;
  const artistIds = card.artistIds;
  const setCode = card.setCode;
  const setName = card.setName;
  const price = card.prices?.usd;
  const scryfallUrl = card.scryfallUri;
  const tcgplayerUrl = card.purchaseUris?.tcgplayer;
  const collectionData = card.userCollection;

  // Shared gradient for consistent look across all breakpoints
  const overlayGradient = 'linear-gradient(to top, rgba(0,0,0,0.95) 0%, rgba(0,0,0,0.8) 40%, rgba(0,0,0,0.5) 70%, rgba(0,0,0,0.2) 85%, transparent 100%)';

  // Minimal variant - mobile/tablet: simplified overlay
  if (variant === 'minimal') {
    return (
      <Box
        sx={{
          position: 'absolute',
          bottom: 0,
          left: 0,
          right: 0,
          zIndex: 10,
          background: overlayGradient,
          p: 1,
        }}
        className={className}
      >
        {/* Rarity/Collector # + Collection info row - tablet only */}
        {!isMobile && (
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 0.5 }}>
            <RarityCollectorBadge
              rarity={rarity}
              collectorNumber={collectorNumber}
              reserved={reserved}
            />
            {context.hasCollector && collectionData && (
              <CollectionSummary
                collectionData={collectionData}
                size="small"
              />
            )}
          </Box>
        )}

        {/* Card name */}
        <CardName
          cardId={cardId}
          cardName={cardName}
          onCardClick={onCardClick}
          sx={{
            overflow: 'hidden',
            '& .MuiTypography-root': {
              fontSize: '0.75rem',
              fontWeight: 600,
              whiteSpace: 'nowrap',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
            }
          }}
        />

        {/* Artist */}
        <ArtistLinks
          artist={artist}
          artistIds={artistIds}
          context={context}
          onArtistClick={onArtistClick}
        />

        {/* Collection info - mobile only (tablet has it in top row) */}
        {isMobile && context.hasCollector && collectionData && (
          <Box sx={{ mt: 0.5 }}>
            <CollectionSummary
              collectionData={collectionData}
              size="small"
            />
          </Box>
        )}
      </Box>
    );
  }

  // Compact variant - mobile expanded / tablet
  if (variant === 'compact') {
    return (
      <Box
        sx={{
          position: 'absolute',
          bottom: 0,
          left: 0,
          right: 0,
          zIndex: 10,
          background: overlayGradient,
        }}
        className={className}
      >
        <Box sx={{ p: 1.5, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
          {/* Collection count + Card name */}
          <Box sx={{
            display: 'flex',
            alignItems: 'center',
            gap: 1,
          }}>
            {context.hasCollector && (
              <Typography
                component="span"
                sx={{
                  fontWeight: 'bold',
                  minWidth: 20,
                  fontSize: '0.875rem',
                }}
              >
                {formatCollectionCount(collectionCount)}
              </Typography>
            )}
            {!context.isOnCardPage && (
              <CardName
                cardId={cardId}
                cardName={cardName}
                onCardClick={onCardClick}
              />
            )}
          </Box>

          {/* Artist */}
          <ArtistLinks
            artist={artist}
            artistIds={artistIds}
            context={context}
            onArtistClick={onArtistClick}
          />

          {/* Set + Collector # + Price row */}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              {!context.hideSetInfo && (
                <SetLink
                  setCode={setCode}
                  setName={setName}
                  rarity={rarity}
                  onSetClick={onSetClick}
                />
              )}
              <Typography variant="caption" sx={{ color: 'grey.400' }}>
                #{collectorNumber}
              </Typography>
            </Box>
            <PriceDisplay
              price={price}
              currency="usd"
              sx={{ fontSize: '0.875rem' }}
            />
          </Box>
        </Box>
      </Box>
    );
  }

  // Full variant - desktop (original implementation)
  return (
    <Box
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
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
          background: overlayGradient,
          opacity: isSelected ? 0 : 1,
          transition: 'none',
          pointerEvents: 'none',
          zIndex: -1
        },
        '.MuiCard-root:not(.selected):hover &::before': {
          opacity: 0
        },
        '.MuiCard-root:focus &::before, .MuiCard-root:focus-within &::before': {
          opacity: 0
        },
        '.MuiCard-root.selected &::before': {
          opacity: '0 !important'
        }
      }}
      className={className}
    >
      <Box sx={{ px: 2, pt: 1, pb: 1, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        {/* Release Date Row - now at the top */}
        {releaseDate && !context.hideReleaseDate && (
          <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
            <Typography
              variant="caption"
              sx={{
                fontSize: '0.625rem',
                color: 'grey.300',
                bgcolor: 'rgba(0, 0, 0, 0.6)',
                px: 0.75,
                py: 0.25,
                borderRadius: 1
              }}
            >
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
              reserved={reserved}
            />
          </Box>
          {context.hasCollector && collectionData && (
            <CollectionSummary
              collectionData={collectionData}
              size="small"
            />
          )}
        </Box>

        {/* Artist(s) */}
        <ArtistLinks
          artist={artist}
          artistIds={artistIds}
          context={context}
          onArtistClick={onArtistClick}
        />

        {/* Card Name */}
        {!context.isOnCardPage && (
          <Box sx={{ mb: -0.5 }}>
            <CardName
              cardId={cardId}
              cardName={cardName}
              onCardClick={onCardClick}
            />
          </Box>
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
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <PriceDisplay
            price={price}
            currency="usd"
            sx={{ fontSize: '0.875rem' }}
          />
          <CardLinks
            scryfallUrl={scryfallUrl}
            tcgplayerUrl={tcgplayerUrl}
            cardName={cardName}
            setCode={setCode}
            collectorNumber={collectorNumber}
          />
        </Box>
      </Box>
    </Box>
  );
});
