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
import { ARTIST_NAME_SEARCH } from '../graphql/queries/artistSearch';

interface ArtistNameResult {
  name: string;
}

interface ArtistNameSearchResponse {
  artistNameSearch: {
    __typename: string;
    data?: ArtistNameResult[];
    status?: {
      message: string;
    };
  };
}

export const ArtistSearchPage: React.FC = React.memo(() => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState('');
  const searchInputRef = React.useRef<HTMLInputElement>(null);
  
  const [searchArtists, { loading, data }] = useLazyQuery<ArtistNameSearchResponse>(
    ARTIST_NAME_SEARCH,
    {
      fetchPolicy: 'cache-and-network'
    }
  );
  
  const handleArtistClick = useCallback((artistName: string) => {
    navigate(`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`);
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
      searchArtists({
        variables: {
          searchTerm: {
            searchTerm: debouncedSearchTerm
          }
        }
      });
    }
  }, [debouncedSearchTerm, searchArtists]);

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Artist Search
      </Typography>

      <TextField
        fullWidth
        variant="outlined"
        placeholder="Enter artist name..."
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
          <Typography>Search by Artist Name - Enter at least 3 characters to search</Typography>
        )}
        {searchTerm.length > 0 && searchTerm.length < 3 && (
          <CharacterCountMessage remainingChars={3 - searchTerm.length} />
        )}
        
        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.artistNameSearch?.data && (
          <SearchResults 
            artists={data.artistNameSearch.data} 
            onArtistClick={handleArtistClick}
          />
        )}
        
        {!loading && debouncedSearchTerm.length >= 3 && data?.artistNameSearch?.data?.length === 0 && (
          <Typography>No artists found matching "{debouncedSearchTerm}"</Typography>
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
  artists: ArtistNameResult[];
  onArtistClick: (artistName: string) => void;
}>(({ artists, onArtistClick }) => {
  const sortedArtists = useMemo(() => 
    [...artists].sort((a, b) => a.name.localeCompare(b.name)),
    [artists]
  );

  const resultCountText = useMemo(() => 
    `Found ${artists.length} artist${artists.length !== 1 ? 's' : ''}`,
    [artists.length]
  );

  const artistPaperStyles = useMemo(() => ({
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
        {sortedArtists.map((artist) => (
          <ArtistResult
            key={artist.name}
            artist={artist}
            onArtistClick={onArtistClick}
            styles={artistPaperStyles}
          />
        ))}
      </Box>
    </Box>
  );
});

// Memoized individual artist result component
const ArtistResult = React.memo<{
  artist: ArtistNameResult;
  onArtistClick: (artistName: string) => void;
  styles: any;
}>(({ artist, onArtistClick, styles }) => {
  const handleClick = useCallback(() => {
    onArtistClick(artist.name);
  }, [artist.name, onArtistClick]);

  return (
    <Paper
      elevation={0}
      onClick={handleClick}
      sx={styles}
    >
      <Typography variant="body2">
        {artist.name}
      </Typography>
    </Paper>
  );
});

CharacterCountMessage.displayName = 'CharacterCountMessage';
SearchResults.displayName = 'SearchResults';
ArtistResult.displayName = 'ArtistResult';