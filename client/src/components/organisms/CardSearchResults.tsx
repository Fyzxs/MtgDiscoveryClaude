import React, { useMemo, useCallback } from 'react';
import { Box, Typography, Paper } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';

interface CardNameResult {
  name: string;
}

interface CardSearchResultsProps {
  cards: CardNameResult[];
  onCardClick: (cardName: string) => void;
  searchTerm?: string;
}

/**
 * CardSearchResults - Displays a list of card search results with sorting and navigation
 *
 * Features:
 * - Alphabetical sorting of results
 * - Hover effects and transitions
 * - Accessible navigation with keyboard support
 * - Result count display
 */
export const CardSearchResults: React.FC<CardSearchResultsProps> = React.memo(({
  cards,
  onCardClick,
  searchTerm
}) => {
  const sortedCards = useMemo(() =>
    [...cards].sort((a, b) => a.name.localeCompare(b.name)),
    [cards]
  );

  const resultCountText = useMemo(() =>
    `Found ${cards.length} card${cards.length !== 1 ? 's' : ''}${searchTerm ? ` matching "${searchTerm}"` : ''}`,
    [cards.length, searchTerm]
  );

  const cardPaperStyles = useMemo(() => ({
    px: 2,
    py: 1,
    cursor: 'pointer',
    border: '1px solid',
    borderColor: 'divider',
    borderRadius: '20px',
    transition: 'all 0.2s ease',
    '&:hover': {
      borderColor: 'primary.main',
      bgcolor: 'action.hover',
      transform: 'translateY(-2px)'
    }
  }), []);

  return (
    <Box>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        {resultCountText}
      </Typography>

      <Box sx={{
        display: 'flex',
        flexWrap: 'wrap',
        gap: 1
      }}>
        {sortedCards.map((card) => (
          <CardResult
            key={card.name}
            card={card}
            onCardClick={onCardClick}
            styles={cardPaperStyles}
          />
        ))}
      </Box>
    </Box>
  );
});

// Memoized individual card result component
const CardResult = React.memo<{
  card: CardNameResult;
  onCardClick: (cardName: string) => void;
  styles: SxProps<Theme>;
}>(({ card, onCardClick, styles }) => {
  const cardUrl = `/card/${encodeURIComponent(card.name)}`;

  const handleClick = useCallback((e: React.MouseEvent) => {
    // Only prevent default for left clicks to allow right-click context menu
    if (e.button === 0) {
      e.preventDefault();
      onCardClick(card.name);
    }
  }, [card.name, onCardClick]);

  return (
    <Paper
      elevation={0}
      component="a"
      href={cardUrl}
      onClick={handleClick}
      sx={{
        ...styles,
        textDecoration: 'none',
        color: 'inherit'
      }}
    >
      <Typography variant="body2">
        {card.name}
      </Typography>
    </Paper>
  );
});

CardResult.displayName = 'CardResult';
CardSearchResults.displayName = 'CardSearchResults';