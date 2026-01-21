import React, { useLayoutEffect, useRef } from 'react';
import { Box, Typography, keyframes, useTheme } from '@mui/material';
import type { CardFinish, CardSpecial } from '../../../types/collection';
import { FINISH_DISPLAY_NAMES, SPECIAL_DISPLAY_NAMES } from '../../../types/collection';

const fadeIn = keyframes`
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
`;

const fadeOut = keyframes`
  from {
    opacity: 1;
  }
  to {
    opacity: 0;
  }
`;

const flashAnimation = keyframes`
  0%, 100% {
    background-color: rgba(0, 0, 0, 0.95);
  }
  50% {
    background-color: rgba(211, 47, 47, 0.95);
  }
`;

const flashAnimationWishlist = keyframes`
  0%, 100% {
    background-color: rgba(45, 20, 45, 0.95);
  }
  50% {
    background-color: rgba(211, 47, 47, 0.95);
  }
`;

interface CollectionEntryOverlayProps {
  visible: boolean;
  count: number;
  isNegative: boolean;
  mode: 'collection' | 'wishlist';
  variant: 'card' | 'sealed';
  flash?: boolean;

  // Card-specific props (required when variant='card')
  finish?: CardFinish;
  special?: CardSpecial;
}

const CollectionEntryOverlayComponent: React.FC<CollectionEntryOverlayProps> = ({
  visible,
  count,
  isNegative,
  mode,
  variant,
  finish,
  special,
  flash = false
}) => {
  const theme = useTheme();
  const overlayRef = useRef<HTMLDivElement>(null);
  const isWishlistMode = mode === 'wishlist';
  const isCardVariant = variant === 'card';

  // Card variant validation
  if (isCardVariant && (finish === undefined || finish === null)) {
    console.error('CollectionEntryOverlay: finish is required when variant="card"');
  }

  // IMPERATIVE: Directly manipulate visibility styles to bypass React render cycle
  // This runs synchronously before browser paint for instant visibility changes
  useLayoutEffect(() => {
    if (overlayRef.current === null) {
      return;
    }

    if (visible) {
      overlayRef.current.style.opacity = '1';
      overlayRef.current.style.visibility = 'visible';
      overlayRef.current.style.pointerEvents = 'auto';
    } else {
      overlayRef.current.style.opacity = '0';
      overlayRef.current.style.visibility = 'hidden';
      overlayRef.current.style.pointerEvents = 'none';
    }
  }, [visible]);

  const hasSpecial = isCardVariant && special && special !== 'none';
  const specialDisplayName = hasSpecial ? SPECIAL_DISPLAY_NAMES[special as CardSpecial] : '';

  const displayCount = isNegative ? `-${count}` : count.toString();

  const modeLabel = isWishlistMode ? '♡ WISHLIST ♡' : 'COLLECTION UPDATE';
  const modeLabelColor = isWishlistMode ? '#f48fb1' : 'rgba(255, 255, 255, 0.6)';

  const baseBackground = isWishlistMode
    ? 'rgba(45, 20, 45, 0.95)'
    : 'rgba(0, 0, 0, 0.95)';

  const borderColor = isWishlistMode ? '#e91e63' : '#1976d2';
  const countColor = isNegative
    ? '#f44336'
    : (isWishlistMode ? '#e91e63' : '#1976d2');

  return (
    <Box
      ref={overlayRef}
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        width: '100%',
        height: '33%',
        // Responsive padding: tighter on small cards
        padding: { xs: 0.5, sm: 1 },
        background: baseBackground,
        backdropFilter: 'blur(4px)',
        borderTop: `2px solid ${borderColor}`,
        zIndex: 1000,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'space-between',
        fontFamily: '"Roboto","Helvetica","Arial",sans-serif',
        boxSizing: 'border-box',
        // Initial state - useLayoutEffect will override these instantly
        opacity: 0,
        visibility: 'hidden',
        pointerEvents: 'none',
        // CSS transitions for smooth opacity changes
        transition: 'opacity 150ms ease-in-out',
        // Performance optimizations
        contain: 'layout style paint',
        willChange: visible ? 'opacity' : 'auto',
        transform: 'translateZ(0)',
        // Flash animation still uses CSS animation
        animation: flash
          ? `${isWishlistMode ? flashAnimationWishlist : flashAnimation} 150ms ease-in-out`
          : 'none'
      }}
    >
      {/* Mode Label */}
      <Typography
        sx={{
          fontSize: { xs: '0.45rem', sm: '0.55rem', md: '0.65rem', lg: '0.75rem' },
          fontWeight: 500,
          textTransform: 'uppercase',
          letterSpacing: '0.05em',
          color: modeLabelColor,
          textAlign: 'center',
          width: '100%',
          flexShrink: 0,
          lineHeight: 1
        }}
      >
        {modeLabel}
      </Typography>

      {/* Main Content */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'row',
          alignItems: 'stretch',
          justifyContent: 'space-between',
          width: '100%',
          flex: '1 1 auto',
          // Responsive gap: tighter on small cards
          gap: { xs: 0.25, sm: 0.5 }
        }}
      >
        {/* Details Section (Left) - Always shown for card variant */}
        {isCardVariant && finish && (
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'flex-start',
              justifyContent: 'center',
              // Responsive gap: tighter on small cards
              gap: { xs: 0.125, sm: 0.25 },
              flex: '0 0 auto'
            }}
          >
            {/* Finish - ALWAYS VISIBLE */}
            <Typography
              sx={{
                fontSize: { xs: '0.6rem', sm: '0.75rem', md: '0.9rem', lg: '1.1rem' },
                fontWeight: 500,
                color: 'rgba(255, 255, 255, 0.87)',
                lineHeight: 1.2
              }}
            >
              {FINISH_DISPLAY_NAMES[finish]}
            </Typography>

            {/* Special - ALWAYS VISIBLE (even if 'none', to maintain layout) */}
            <Typography
              sx={{
                fontSize: { xs: '0.6rem', sm: '0.75rem', md: '0.9rem', lg: '1.1rem' },
                fontWeight: 500,
                color: '#ff9800',
                lineHeight: 1.2,
                visibility: hasSpecial ? 'visible' : 'hidden',
                minHeight: { xs: '0.72rem', sm: '0.9rem', md: '1.08rem', lg: '1.32rem' }
              }}
            >
              {specialDisplayName || '\u00A0'}
            </Typography>
          </Box>
        )}

        {/* Count Display (Right) */}
        <Box
          sx={{
            fontSize: { xs: '1.5rem', sm: '2rem', md: '3rem', lg: '4rem' },
            fontWeight: 'bold',
            color: countColor,
            lineHeight: 1,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            flexShrink: 1,
            minWidth: 0,
            willChange: 'contents',
            // Sealed products get full width, cards share with details
            flex: isCardVariant ? '0 0 auto' : '1 1 auto'
          }}
        >
          {displayCount}
        </Box>
      </Box>
    </Box>
  );
};

export const CollectionEntryOverlay = React.memo(
  CollectionEntryOverlayComponent,
  (prev, next) => {
    return (
      prev.visible === next.visible &&
      prev.count === next.count &&
      prev.isNegative === next.isNegative &&
      prev.mode === next.mode &&
      prev.variant === next.variant &&
      prev.flash === next.flash &&
      prev.finish === next.finish &&
      prev.special === next.special
    );
  }
);
