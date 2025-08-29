import React, { useState, useEffect } from 'react';
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

export const CardSearchPage: React.FC = () => {
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
  
  const handleCardClick = (cardName: string) => {
    navigate(`/card/${encodeURIComponent(cardName)}`);
  };

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
          <Typography>Minimum 3 characters required - Enter {3 - searchTerm.length} more character{3 - searchTerm.length === 1 ? '' : 's'}</Typography>
        )}
        
        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.cardNameSearch?.data && (
          <Box>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
              Found {data.cardNameSearch.data.length} card{data.cardNameSearch.data.length !== 1 ? 's' : ''}
            </Typography>
            
            <Box sx={{ 
              display: 'flex', 
              flexWrap: 'wrap', 
              gap: 1
            }}>
              {[...data.cardNameSearch.data].sort((a, b) => a.name.localeCompare(b.name)).map((card) => (
                <Paper
                  key={card.name}
                  elevation={0}
                  onClick={() => handleCardClick(card.name)}
                  sx={{
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
                  }}
                >
                  <Typography variant="body2">
                    {card.name}
                  </Typography>
                </Paper>
              ))}
            </Box>
          </Box>
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.cardNameSearch?.data?.length === 0 && (
          <Typography>No cards found matching "{debouncedSearchTerm}"</Typography>
        )}
      </Box>
    </Container>
  );
};