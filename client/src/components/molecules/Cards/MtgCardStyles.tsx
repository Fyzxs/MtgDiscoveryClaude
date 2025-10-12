import { keyframes } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { useMemo } from 'react';
import { getRarityGlowStyles } from '../../../utils/rarityStyles';
import type { Card } from '../../../types/card';

// Flash animation for success/error feedback
export const flashSuccess = keyframes`
  0%, 100% { opacity: 0; }
  50% { opacity: 0.5; }
`;

export const flashError = keyframes`
  0%, 100% { opacity: 0; }
  50% { opacity: 0.6; }
`;

// Subtle pulse for submitting state
export const submitPulse = keyframes`
  0%, 100% { opacity: 0.15; }
  50% { opacity: 0.25; }
`;

interface MtgCardStylesProps {
  card: Card;
}

export const useMtgCardStyles = ({ card }: MtgCardStylesProps) => {
  const theme = useTheme();

  // Memoize hover styles to avoid recalculating rarity styles
  const hoverStyles = useMemo(() => {
    return getRarityGlowStyles(card.rarity, false, true);
  }, [card.rarity]);

  const cardStyles = useMemo(() => ({
    position: 'relative',
    width: '280px',
    bgcolor: 'grey.800',
    borderRadius: '12px',
    border: '3px solid',
    borderColor: 'grey.700',
    overflow: 'hidden',
    boxShadow: theme.mtg.shadows.card.normal,
    transition: 'transform 0.05s ease-out',
    transform: 'scale(1)',
    cursor: 'pointer',
    outline: 'none',
    // Instant deselection when submitting (no animation lag)
    '&[data-submitting="true"]': {
      transition: 'none !important',
      transform: 'scale(1) !important',
      border: '3px solid',
      borderColor: 'grey.700',
      boxShadow: theme.mtg.shadows.card.normal,
      '&::before': {
        content: '""',
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: '#1976d2',
        opacity: 0.2,
        animation: `${submitPulse} 0.6s ease-in-out infinite`,
        pointerEvents: 'none',
        zIndex: 999,
      }
    },
    // Flash overlay using pseudo-element (CSS-only, no React state)
    '&[data-flash="success"]::after, &[data-flash="error"]::after': {
      content: '""',
      position: 'absolute',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      pointerEvents: 'none',
      zIndex: 9999,
    },
    '&[data-flash="success"]::after': {
      backgroundColor: '#4caf50',
      animation: `${flashSuccess} 0.3s ease-in-out 3`,
    },
    '&[data-flash="error"]::after': {
      backgroundColor: '#f44336',
      animation: `${flashError} 0.3s ease-in-out 3`,
    },
    '&:focus': {
      outline: 'none'
    },
    '&:focus-visible': {
      outline: 'none'
    },
    // CSS-only selection styles
    '&[data-selected="true"]': {
      border: '4px solid',
      borderColor: '#1976d2',
      boxShadow: `${theme.mtg.shadows.card.selected}, ${theme.mtg.shadows.card.normal}`,
      transform: 'scale(1.05)',
      '& .zoom-indicator': {
        opacity: 1,
        transform: 'scale(1)'
      }
    },
    '&:hover:not([data-selected="true"])': {
      ...hoverStyles,
      transform: 'scale(1.01)',
      '& .zoom-indicator': {
        opacity: 1,
        transform: 'scale(1)'
      }
    }
  }), [theme, hoverStyles]);

  return {
    cardStyles,
    hoverStyles
  };
};