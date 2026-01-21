import React, { useState } from 'react';
import { Box, useTheme, Typography } from '../../atoms';
import type { SxProps, Theme } from '../../atoms';
import type { Card, CardContext } from '../../../types/card';
import { CardImageDisplay } from './CardImageDisplay';
import { PriceDisplay } from '../../atoms';
import { CollectorNumber, RarityBadge } from '../../atoms';
import { ArtistInfo } from './ArtistInfo';
import { CardLinks } from './CardLinks';
import { useLongPress } from '../../../hooks/useLongPress';
import { useSwipeGesture } from '../../../hooks/useSwipeGesture';
import { useHapticFeedback } from '../../../hooks/useHapticFeedback';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';
import { getRarityColor, getRarityShadow } from '../../../theme';

interface CardCompactProps {
  card: Card;
  context?: CardContext;
  onClick?: () => void;
  onLongPress?: () => void;
  onSwipe?: (direction: 'left' | 'right' | 'up' | 'down') => void;
  enableHapticFeedback?: boolean;
  enableSwipeGestures?: boolean;
  enableLongPress?: boolean;
  sx?: SxProps<Theme>;
}

export const CardCompact: React.FC<CardCompactProps> = ({
  card,
  context = {},
  onClick,
  onLongPress,
  onSwipe,
  enableHapticFeedback = true,
  enableSwipeGestures = false,
  enableLongPress = false,
  sx = {},
}) => {
  const theme = useTheme();
  const { isMobile } = useResponsiveBreakpoints();
  const { triggerHaptic } = useHapticFeedback({ enabled: enableHapticFeedback });

  const [isHovered, setIsHovered] = useState(false);
  const [isTouched, setIsTouched] = useState(false);

  // Handle interactions with haptic feedback
  const handleClick = () => {
    if (enableHapticFeedback) {
      triggerHaptic('light');
    }
    onClick?.();
  };

  const handleLongPress = () => {
    if (enableHapticFeedback) {
      triggerHaptic('medium');
    }
    onLongPress?.();
  };

  const handleSwipe = (direction: 'left' | 'right' | 'up' | 'down') => {
    if (enableHapticFeedback) {
      triggerHaptic('selection');
    }
    onSwipe?.(direction);
  };

  // Touch interaction hooks
  const longPressHandlers = useLongPress({
    onLongPress: handleLongPress,
    onClick: handleClick,
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

  const swipeHandlers = useSwipeGesture({
    onSwipe: handleSwipe,
    threshold: 50,
    velocityThreshold: 0.3,
    preventDefaultTouchmove: false,
  });

  // Get rarity-based styling
  const rarityColor = getRarityColor(card.rarity);
  const rarityShadow = getRarityShadow(card.rarity);

  // Container styling
  const getContainerSx = (): SxProps<Theme> => ({
    position: 'relative',
    bgcolor: 'grey.900',
    borderRadius: 2,
    overflow: 'hidden',
    cursor: onClick ? 'pointer' : 'default',
    transition: theme.mtg.transitions.card,
    border: `1px solid ${rarityColor}`,

    // Touch feedback
    transform: isTouched ? 'scale(0.98)' : isHovered ? 'scale(1.02)' : 'none',

    // Hover effects for non-touch devices
    '@media (hover: hover)': {
      '&:hover': {
        boxShadow: rarityShadow,
        transform: 'translateY(-2px) scale(1.02)',
      },
    },

    // Mobile-specific touch optimizations
    ...(isMobile && {
      minHeight: theme.mtg.dimensions.touch.minHeight,
      '&:active': {
        transform: 'scale(0.95)',
        transition: theme.mtg.transitions.touch,
      },
    }),

    // Remove tap highlight
    WebkitTapHighlightColor: 'transparent',

    // Focus styles for accessibility
    '&:focus-visible': {
      outline: `2px solid ${theme.palette.primary.main}`,
      outlineOffset: '2px',
    },

    ...sx,
  });

  // Combine event handlers
  const eventHandlers = {
    ...(enableLongPress ? longPressHandlers : { onClick: handleClick }),
    ...(enableSwipeGestures ? swipeHandlers : {}),
    onMouseEnter: () => setIsHovered(true),
    onMouseLeave: () => setIsHovered(false),
    tabIndex: onClick ? 0 : undefined,
    role: onClick ? 'button' : undefined,
    'aria-label': onClick ? `View ${card.name} card` : undefined,
  };

  return (
    <Box
      sx={getContainerSx()}
      {...(eventHandlers as Partial<React.DOMAttributes<HTMLDivElement>>)}
    >
      {/* Card Image */}
      <Box sx={{ position: 'relative' }}>
        <CardImageDisplay
          card={card}
          size="normal"
          showFlipButton={false}
          sx={{ width: '100%', display: 'block' }}
        />

        {/* Overlay info at bottom of image */}
        <Box
          sx={{
            position: 'absolute',
            bottom: 0,
            left: 0,
            right: 0,
            background: theme.mtg.gradients.mobileOverlay,
            p: { xs: 2, sm: 3 },
          }}
        >
          {/* Collector info row */}
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              mb: 2,
            }}
          >
            <CollectorNumber
              number={card.collectorNumber}
              setCode={card.setCode}
              sx={{ fontSize: '0.75rem' }}
            />
            {card.rarity && <RarityBadge rarity={card.rarity} />}
          </Box>

          {/* Artist */}
          <ArtistInfo
            artist={card.artist}
            artistIds={card.artistIds}
            context={context}
            sx={{
              fontSize: '0.75rem',
              mb: 2,
              '& a': {
                color: 'text.secondary',
                textDecoration: 'none',
                '&:hover': {
                  color: 'primary.main',
                },
              },
            }}
          />

          {/* Card Name - conditional display */}
          {!context.isOnCardPage && (
            <Typography
              variant="body2"
              component="h4"
              sx={{
                fontSize: '0.875rem',
                fontWeight: 'bold',
                color: 'text.primary',
                mb: 1,
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
              }}
            >
              {card.name}
            </Typography>
          )}

          {/* Set Name - conditional display */}
          {!context.isOnSetPage && card.setName && (
            <Typography
              variant="caption"
              sx={{
                fontSize: '0.75rem',
                color: 'text.secondary',
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                display: 'block',
                mb: 1,
              }}
            >
              {card.setName}
            </Typography>
          )}

          {/* Price and Links */}
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              mt: 2,
            }}
          >
            <PriceDisplay
              price={card.prices?.usd}
              currency="usd"
              sx={{ fontSize: '0.875rem' }}
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
  );
};