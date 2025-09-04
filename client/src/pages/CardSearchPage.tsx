import React, { useState, useEffect, useCallback, useMemo } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  TextField,
  InputAdornment,
  CircularProgress,
  Paper
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { CARD_NAME_SEARCH } from '../graphql/queries/cardNameSearch';

interface CardNameResult {
  name: string;
}

interface CardNameSearchResponse {
  cardNameSearch: {
    __typename: string;
    data?: CardNameResult[];
    status?: {
      message: string;
    };
  };
}

export const CardSearchPage: React.FC = React.memo(() => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState('');
  const searchInputRef = React.useRef<HTMLInputElement>(null);
  
  const [searchCards, { loading, data }] = useLazyQuery<CardNameSearchResponse>(
    CARD_NAME_SEARCH,
    {
      fetchPolicy: 'cache-and-network'
    }
  );
  
  const handleCardClick = useCallback((cardName: string) => {
    navigate(`/card/${encodeURIComponent(cardName)}`);
  }, [navigate]);

  // Focus input on mount
  useEffect(() => {
    searchInputRef.current?.focus();
  }, []);

  // Refocus after search completes
  useEffect(() => {
    if (!loading && data) {
      searchInputRef.current?.focus();
    }
  }, [loading, data]);

  // Debounce the search term
  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchTerm]);

  // Execute search when debounced term changes
  useEffect(() => {
    if (debouncedSearchTerm.length >= 3) {
      searchCards({
        variables: {
          searchTerm: {
            searchTerm: debouncedSearchTerm
          }
        }
      });
    }
  }, [debouncedSearchTerm, searchCards]);

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Card Search
      </Typography>

      <TextField
        fullWidth
        variant="outlined"
        placeholder="Enter card name..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        disabled={loading}
        inputRef={searchInputRef}
        sx={{ mb: 3 }}
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <SearchIcon />
            </InputAdornment>
          ),
          endAdornment: loading && (
            <InputAdornment position="end">
              <CircularProgress size={20} />
            </InputAdornment>
          )
        }}
      />

      <Box>
        {searchTerm.length === 0 && (
          <Typography>Search by Card Name - Enter at least 3 characters to search</Typography>
        )}
        {searchTerm.length > 0 && searchTerm.length < 3 && (
          <CharacterCountMessage remainingChars={3 - searchTerm.length} />
        )}
        
        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.cardNameSearch?.data && (
          <SearchResults 
            cards={data.cardNameSearch.data} 
            onCardClick={handleCardClick}
          />
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.cardNameSearch?.data?.length === 0 && (
          <Typography>No cards found matching "{debouncedSearchTerm}"</Typography>
        )}
      </Box>
    </Container>
  );
});

// Memoized component for character count message
const CharacterCountMessage = React.memo<{ remainingChars: number }>(({ remainingChars }) => (
  <Typography>
    Minimum 3 characters required - Enter {remainingChars} more character{remainingChars === 1 ? '' : 's'}
  </Typography>
));

// Memoized component for search results
const SearchResults = React.memo<{
  cards: CardNameResult[];
  onCardClick: (cardName: string) => void;
}>(({ cards, onCardClick }) => {
  const sortedCards = useMemo(() => 
    [...cards].sort((a, b) => a.name.localeCompare(b.name)),
    [cards]
  );

  const resultCountText = useMemo(() => 
    `Found ${cards.length} card${cards.length !== 1 ? 's' : ''}`,
    [cards.length]
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
  styles: any;
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

CharacterCountMessage.displayName = 'CharacterCountMessage';
SearchResults.displayName = 'SearchResults';
CardResult.displayName = 'CardResult';