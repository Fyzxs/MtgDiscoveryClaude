import React, { useState } from 'react';
import { Box, useTheme } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';
import type { Card, CardContext } from '../../types/card';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { PriceDisplay } from '../atoms/shared/PriceDisplay';
import { CollectorInfo } from '../molecules/Cards/CollectorInfo';
import { CardMetadata } from '../molecules/Cards/CardMetadata';
import { ArtistInfo } from '../molecules/Cards/ArtistInfo';
import { CardLinks } from '../molecules/Cards/CardLinks';
import { ManaCost } from '../molecules/Cards/ManaCost';
import { useLongPress } from '../../hooks/useLongPress';
import { useSwipeGesture } from '../../hooks/useSwipeGesture';
import { useHapticFeedback } from '../../hooks/useHapticFeedback';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { getRarityColor } from '../../theme';

interface CardDisplayProps {
  card: Card;
  context?: CardContext;
  showHover?: boolean;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onCardLongPress?: (cardId?: string) => void;
  onSwipe?: (direction: 'left' | 'right' | 'up' | 'down', cardId?: string) => void;
  enableHapticFeedback?: boolean;
  enableSwipeGestures?: boolean;
  enableLongPress?: boolean;
  sx?: SxProps<Theme>;
}

export const CardDisplay: React.FC<CardDisplayProps> = ({
  card,
  context = {},
  showHover = true,
  onCardClick,
  onSetClick,
  onArtistClick,
  onCardLongPress,
  onSwipe,
  enableHapticFeedback = true,
  enableSwipeGestures = false,
  enableLongPress = false,
  sx = {},
}) => {
  const theme = useTheme();
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const { triggerHaptic } = useHapticFeedback({ enabled: enableHapticFeedback });

  const [isHovered, setIsHovered] = useState(false);
  const [isTouched, setIsTouched] = useState(false);

  // Handle card click with haptic feedback
  const handleCardClick = () => {
    if (enableHapticFeedback) {
      triggerHaptic('light');
    }
    onCardClick?.(card.id);
  };

  // Handle long press with haptic feedback
  const handleLongPress = () => {
    if (enableHapticFeedback) {
      triggerHaptic('medium');
    }
    onCardLongPress?.(card.id);
  };

  // Handle swipe gestures
  const handleSwipe = (direction: 'left' | 'right' | 'up' | 'down') => {
    if (enableHapticFeedback) {
      triggerHaptic('selection');
    }
    onSwipe?.(direction, card.id);
  };

  // Long press handlers
  const longPressHandlers = useLongPress({
    onLongPress: handleLongPress,
    onClick: handleCardClick,
    threshold: 500,
    onStart: () => {
      setIsTouched(true);
      if (enableHapticFeedback) {
        triggerHaptic('selection');
      }
    },
    onFinish: () => setIsTouched(false),
    onCancel: () => setIsTouched(false),
  });

  // Swipe gesture handlers
  const swipeHandlers = useSwipeGesture({
    onSwipe: handleSwipe,
    threshold: 50,
    velocityThreshold: 0.3,
    preventDefaultTouchmove: false,
  });

  // Get rarity-based styling
  const getRarityBorderColor = (rarity?: string): string => {
    return getRarityColor(rarity);
  };

  // Get container styling based on responsive state
  const getContainerSx = (): SxProps<Theme> => {
    const rarityColor = getRarityBorderColor(card.rarity);

    return {
      position: 'relative',
      bgcolor: 'grey.900',
      borderRadius: 3,
      border: 2,
      borderColor: rarityColor,
      boxShadow: isHovered ? theme.mtg.shadows.card.hover : theme.mtg.shadows.card.normal,
      transition: theme.mtg.transitions.card,
      cursor: onCardClick ? 'pointer' : 'default',
      overflow: 'hidden',

      // Touch feedback
      transform: isTouched ? 'scale(0.98)' : 'none',

      // Hover effects for non-touch devices
      '@media (hover: hover)': {
        '&:hover': {
          transform: 'translateY(-2px)',
          boxShadow: theme.mtg.shadows.card.hover,
        },
      },

      // Remove tap highlight on mobile
      WebkitTapHighlightColor: 'transparent',

      // Focus styles for accessibility
      '&:focus-visible': {
        outline: `2px solid ${theme.palette.primary.main}`,
        outlineOffset: '2px',
      },

      ...sx,
    };
  };

  // Combine all event handlers
  const eventHandlers = {
    ...(enableLongPress ? longPressHandlers : { onClick: handleCardClick }),
    ...(enableSwipeGestures ? swipeHandlers : {}),
    onMouseEnter: () => setIsHovered(true),
    onMouseLeave: () => setIsHovered(false),
    tabIndex: onCardClick ? 0 : undefined,
    role: onCardClick ? 'button' : undefined,
    'aria-label': onCardClick ? `View ${card.name} details` : undefined,
  };

  return (
    <Box
      component="div"
      sx={getContainerSx()}
      {...eventHandlers}
    >
      {/* Mobile Layout - Horizontal */}
      <Box sx={{ display: { xs: 'block', sm: 'none' } }}>
        <Box
          sx={{
            display: 'flex',
            gap: 3,
            p: 3,
          }}
        >
          {/* Card Image - Smaller on mobile */}
          <Box sx={{ flexShrink: 0 }}>
            <CardImageDisplay
              card={card}
              size="small"
              showFlipButton={false}
              sx={{ width: 80 }}
            />
          </Box>

          {/* Card Info */}
          <Box
            sx={{
              flex: 1,
              minWidth: 0,
              display: 'flex',
              flexDirection: 'column',
              gap: 1,
            }}
          >
            <CardMetadata
              name={card.name}
              cardId={card.id}
              typeLine={card.typeLine}
              setName={card.setName}
              setCode={card.setCode}
              rarity={card.rarity}
              releasedAt={card.releasedAt}
              context={context}
              onCardClick={onCardClick}
              onSetClick={onSetClick}
              sx={{ flex: 1 }}
            />

            {card.manaCost && (
              <ManaCost manaCost={card.manaCost} size="small" />
            )}

            <ArtistInfo
              artist={card.artist}
              artistIds={card.artistIds}
              context={context}
              onArtistClick={onArtistClick}
            />

            <Box
              sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                pt: 1,
              }}
            >
              <PriceDisplay
                price={card.prices?.usd}
                currency="usd"
                sx={{
                  fontSize: '0.875rem',
                  fontWeight: 'bold',
                }}
              />
              <CardLinks
                scryfallUrl={card.scryfallUri}
                cardName={card.name}
                sx={{
                  transform: 'scale(0.75)',
                  transformOrigin: 'right',
                }}
              />
            </Box>
          </Box>
        </Box>
      </Box>

      {/* Desktop Layout - Vertical */}
      <Box sx={{ display: { xs: 'none', sm: 'block' }, p: 4 }}>
        {/* Card Image */}
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            mb: 4,
          }}
        >
          <CardImageDisplay
            card={card}
            size="normal"
            showFlipButton={true}
            sx={{
              width: '100%',
              maxWidth: { sm: '250px', lg: '300px' },
            }}
          />
        </Box>

        {/* Card Info */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          {/* Header with name and mana cost */}
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'flex-start',
              gap: 2,
            }}
          >
            <CardMetadata
              name={card.name}
              cardId={card.id}
              typeLine={card.typeLine}
              setName={card.setName}
              setCode={card.setCode}
              rarity={card.rarity}
              releasedAt={card.releasedAt}
              context={context}
              onCardClick={onCardClick}
              onSetClick={onSetClick}
              sx={{ flex: 1, minWidth: 0 }}
            />
            {card.manaCost && (
              <ManaCost
                manaCost={card.manaCost}
                size="small"
                sx={{ flexShrink: 0 }}
              />
            )}
          </Box>

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
            onArtistClick={onArtistClick}
          />

          {/* Price and Links */}
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              pt: 2,
              borderTop: 1,
              borderColor: 'grey.800',
            }}
          >
            <PriceDisplay
              price={card.prices?.usd}
              currency="usd"
              sx={{ fontSize: '1.125rem' }}
            />
            <CardLinks
              scryfallUrl={card.scryfallUri}
              cardName={card.name}
            />
          </Box>
        </Box>

        {/* Hover overlay with additional info - Desktop only */}
        {showHover && isHovered && (
          <Box
            sx={{
              position: 'absolute',
              left: 0,
              right: 0,
              bottom: 0,
              bgcolor: 'rgba(17, 24, 39, 0.95)',
              backdropFilter: 'blur(4px)',
              borderRadius: '0 0 24px 24px',
              p: 4,
              borderTop: 1,
              borderColor: 'grey.700',
            }}
          >
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              {card.oracleText && (
                <Box
                  component="p"
                  sx={{
                    fontSize: '0.875rem',
                    color: 'grey.300',
                    fontStyle: 'italic',
                    m: 0,
                  }}
                >
                  {card.oracleText}
                </Box>
              )}
              {card.flavorText && (
                <Box
                  component="p"
                  sx={{
                    fontSize: '0.75rem',
                    color: 'grey.500',
                    fontStyle: 'italic',
                    m: 0,
                  }}
                >
                  "{card.flavorText}"
                </Box>
              )}
              {(card.power || card.toughness) && (
                <Box
                  sx={{
                    textAlign: 'right',
                    fontSize: '1.125rem',
                    fontWeight: 'bold',
                    color: 'grey.400',
                  }}
                >
                  {card.power}/{card.toughness}
                </Box>
              )}
            </Box>
          </Box>
        )}
      </Box>
    </Box>
  );
};