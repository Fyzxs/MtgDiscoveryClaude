import React, { useState } from 'react';
import {
  TextField,
  InputAdornment,
  CircularProgress,
  Typography,
  Box,
  Chip,
  Collapse,
  Button,
  Paper,
  FormControl,
  InputLabel,
  Select,
  MenuItem
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import FilterListIcon from '@mui/icons-material/FilterList';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { SearchTemplate } from './SearchTemplate';

/**
 * Example: Card Search Implementation
 *
 * Demonstrates a complete card search interface with:
 * - Prominent search input with loading indicator
 * - Collapsible advanced filters
 * - Results summary with search context
 * - Quick filter chips for common refinements
 * - Search results grid
 * - Empty state handling
 */
export const CardSearchExample: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showAdvancedFilters, setShowAdvancedFilters] = useState(false);
  const [selectedRarity, setSelectedRarity] = useState('');
  const [selectedType, setSelectedType] = useState('');
  const [results] = useState([
    { id: '1', name: 'Lightning Bolt', rarity: 'common' },
    { id: '2', name: 'Lightning Strike', rarity: 'common' },
    { id: '3', name: 'Lightning Helix', rarity: 'uncommon' }
  ]);

  const quickFilters = ['Instant', 'Sorcery', 'Creature', 'Artifact'];
  const hasResults = results.length > 0;
  const hasSearchTerm = searchTerm.length >= 3;

  return (
    <SearchTemplate
      searchInput={
        <TextField
          fullWidth
          variant="outlined"
          placeholder="Enter card name..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          disabled={isLoading}
          sx={{ maxWidth: 600 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
            endAdornment: isLoading && (
              <InputAdornment position="end">
                <CircularProgress size={20} />
              </InputAdornment>
            )
          }}
        />
      }

      advancedFilters={
        <Box sx={{ width: '100%', maxWidth: 600 }}>
          <Button
            startIcon={<FilterListIcon />}
            endIcon={showAdvancedFilters ? <ExpandLessIcon /> : <ExpandMoreIcon />}
            onClick={() => setShowAdvancedFilters(!showAdvancedFilters)}
            sx={{ mb: 2 }}
          >
            Advanced Filters
          </Button>

          <Collapse in={showAdvancedFilters}>
            <Paper sx={{ p: 3, bgcolor: 'grey.50' }}>
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <FormControl sx={{ minWidth: 120 }}>
                  <InputLabel>Rarity</InputLabel>
                  <Select
                    value={selectedRarity}
                    label="Rarity"
                    onChange={(e) => setSelectedRarity(e.target.value)}
                  >
                    <MenuItem value="">Any</MenuItem>
                    <MenuItem value="common">Common</MenuItem>
                    <MenuItem value="uncommon">Uncommon</MenuItem>
                    <MenuItem value="rare">Rare</MenuItem>
                    <MenuItem value="mythic">Mythic</MenuItem>
                  </Select>
                </FormControl>

                <FormControl sx={{ minWidth: 120 }}>
                  <InputLabel>Type</InputLabel>
                  <Select
                    value={selectedType}
                    label="Type"
                    onChange={(e) => setSelectedType(e.target.value)}
                  >
                    <MenuItem value="">Any</MenuItem>
                    <MenuItem value="creature">Creature</MenuItem>
                    <MenuItem value="instant">Instant</MenuItem>
                    <MenuItem value="sorcery">Sorcery</MenuItem>
                    <MenuItem value="artifact">Artifact</MenuItem>
                  </Select>
                </FormControl>
              </Box>
            </Paper>
          </Collapse>
        </Box>
      }

      resultsSummary={
        hasSearchTerm && (
          <Typography variant="body2" color="text.secondary">
            Found {results.length} card{results.length !== 1 ? 's' : ''} matching "{searchTerm}"
          </Typography>
        )
      }

      quickFilters={
        hasResults && (
          <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap', justifyContent: 'center' }}>
            {quickFilters.map((filter) => (
              <Chip
                key={filter}
                label={filter}
                variant="outlined"
                clickable
                size="small"
                sx={{
                  '&:hover': {
                    bgcolor: 'primary.main',
                    color: 'primary.contrastText'
                  }
                }}
              />
            ))}
          </Box>
        )
      }

      loadingState={
        <CircularProgress size={40} />
      }

      emptyState={
        hasSearchTerm && (
          <Typography variant="body1" color="text.secondary">
            No cards found matching "{searchTerm}"
          </Typography>
        )
      }

      resultsContent={
        searchTerm.length === 0 ? (
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
            Search by Card Name - Enter at least 3 characters to search
          </Typography>
        ) : searchTerm.length < 3 ? (
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
            Minimum 3 characters required - Enter {3 - searchTerm.length} more character{3 - searchTerm.length === 1 ? '' : 's'}
          </Typography>
        ) : (
          <Box sx={{
            display: 'flex',
            flexWrap: 'wrap',
            gap: 1,
            justifyContent: 'center'
          }}>
            {results.map((card) => (
              <Paper
                key={card.id}
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
        )
      }

      isLoading={isLoading}
      isEmpty={hasSearchTerm && !isLoading && results.length === 0}
      showInitialState={searchTerm.length === 0 || searchTerm.length < 3}
    />
  );
};

/**
 * Example: Artist Search Implementation
 *
 * Demonstrates a simpler search interface for artists with:
 * - Basic search input
 * - No advanced filters (artists don't need complex filtering)
 * - Results summary
 * - Simple results display
 */
export const ArtistSearchExample: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [results] = useState([
    { id: '1', name: 'Rebecca Guay' },
    { id: '2', name: 'John Avon' },
    { id: '3', name: 'Terese Nielsen' }
  ]);

  const hasResults = results.length > 0;
  const hasSearchTerm = searchTerm.length >= 3;

  return (
    <SearchTemplate
      searchInput={
        <TextField
          fullWidth
          variant="outlined"
          placeholder="Enter artist name..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          disabled={isLoading}
          sx={{ maxWidth: 600 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
            endAdornment: isLoading && (
              <InputAdornment position="end">
                <CircularProgress size={20} />
              </InputAdornment>
            )
          }}
        />
      }

      resultsSummary={
        hasSearchTerm && (
          <Typography variant="body2" color="text.secondary">
            Found {results.length} artist{results.length !== 1 ? 's' : ''} matching "{searchTerm}"
          </Typography>
        )
      }

      loadingState={
        <CircularProgress size={40} />
      }

      emptyState={
        hasSearchTerm && (
          <Typography variant="body1" color="text.secondary">
            No artists found matching "{searchTerm}"
          </Typography>
        )
      }

      resultsContent={
        searchTerm.length === 0 ? (
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
            Search by Artist Name - Enter at least 3 characters to search
          </Typography>
        ) : searchTerm.length < 3 ? (
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
            Minimum 3 characters required - Enter {3 - searchTerm.length} more character{3 - searchTerm.length === 1 ? '' : 's'}
          </Typography>
        ) : (
          <Box sx={{
            display: 'flex',
            flexWrap: 'wrap',
            gap: 1,
            justifyContent: 'center'
          }}>
            {results.map((artist) => (
              <Paper
                key={artist.id}
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
                  {artist.name}
                </Typography>
              </Paper>
            ))}
          </Box>
        )
      }

      isLoading={isLoading}
      isEmpty={hasSearchTerm && !isLoading && results.length === 0}
      showInitialState={searchTerm.length === 0 || searchTerm.length < 3}
    />
  );
};

/**
 * Example: Minimal Search Interface
 *
 * Demonstrates the simplest possible search implementation with:
 * - Just search input and results
 * - No filters or advanced features
 * - Basic loading and empty states
 */
export const MinimalSearchExample: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  return (
    <SearchTemplate
      searchInput={
        <TextField
          fullWidth
          variant="outlined"
          placeholder="Search..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          disabled={isLoading}
          sx={{ maxWidth: 400 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            )
          }}
        />
      }

      loadingState={<CircularProgress />}

      emptyState={
        <Typography>No results found</Typography>
      }

      resultsContent={
        <Typography>Enter a search term to see results</Typography>
      }

      isLoading={isLoading}
      isEmpty={false}
      showInitialState={searchTerm.length === 0}
    />
  );
};

// Component display names for debugging
CardSearchExample.displayName = 'CardSearchExample';
ArtistSearchExample.displayName = 'ArtistSearchExample';
MinimalSearchExample.displayName = 'MinimalSearchExample';