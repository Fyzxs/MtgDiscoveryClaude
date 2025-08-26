import React, { useState, useEffect, useRef } from 'react';
import { useQuery } from '@apollo/client/react';
import { 
  Container, 
  Typography, 
  Grid,
  Box, 
  CircularProgress, 
  Alert,
  TextField,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Chip,
  Stack,
  InputAdornment,
  IconButton
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import { GET_ALL_SETS } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { ResultsSummary } from '../components/atoms/shared/ResultsSummary';
import { EmptyState } from '../components/atoms/shared/EmptyState';
import type { MtgSet } from '../types/set';

interface SetsResponse {
  allSets: {
    __typename: string;
    data?: MtgSet[];
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

export const AllSetsPage: React.FC = () => {
  // Get initial state from URL
  const urlParams = new URLSearchParams(window.location.search);
  const initialSearch = urlParams.get('search') || '';
  const initialTypes = urlParams.get('types')?.split(',').filter(Boolean) || [];
  const initialSort = urlParams.get('sort') || 'release-desc';

  const { loading, error, data } = useQuery<SetsResponse>(GET_ALL_SETS);
  const [searchTerm, setSearchTerm] = useState(initialSearch);
  const [selectedSetTypes, setSelectedSetTypes] = useState<string[]>(initialTypes);
  const [sortBy, setSortBy] = useState<string>(initialSort);
  const [filteredSets, setFilteredSets] = useState<MtgSet[]>([]);
  const [hasSearchText, setHasSearchText] = useState(!!initialSearch);
  
  // Use ref to store the search input value without causing re-renders
  const searchInputRef = useRef<HTMLInputElement>(null);
  const debounceTimer = useRef<NodeJS.Timeout>();

  // Get unique set types from data
  const getUniqueSetTypes = (sets: MtgSet[]): string[] => {
    const types = new Set(sets.map(set => set.setType));
    return Array.from(types).sort();
  };

  // Cleanup timer on unmount
  useEffect(() => {
    return () => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }
    };
  }, []);

  // Update URL when filters change
  useEffect(() => {
    const params = new URLSearchParams();
    params.set('page', 'all-sets');
    
    if (searchTerm) {
      params.set('search', searchTerm);
    }
    if (selectedSetTypes.length > 0) {
      params.set('types', selectedSetTypes.join(','));
    }
    if (sortBy !== 'release-desc') {
      params.set('sort', sortBy);
    }
    
    const newUrl = `${window.location.pathname}?${params.toString()}`;
    window.history.replaceState(null, '', newUrl);
  }, [searchTerm, selectedSetTypes, sortBy]);

  // Filter and sort sets
  useEffect(() => {
    if (data?.allSets?.data) {
      let filtered = [...data.allSets.data];

      // Filter by search term
      if (searchTerm) {
        filtered = filtered.filter(set => 
          set.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          set.code.toLowerCase().includes(searchTerm.toLowerCase())
        );
      }

      // Filter by set types (multi-select)
      if (selectedSetTypes.length > 0) {
        filtered = filtered.filter(set => selectedSetTypes.includes(set.setType));
      }

      // Sort sets
      switch (sortBy) {
        case 'release-desc':
          filtered.sort((a, b) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime());
          break;
        case 'release-asc':
          filtered.sort((a, b) => new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime());
          break;
        case 'name-asc':
          filtered.sort((a, b) => a.name.localeCompare(b.name));
          break;
        case 'name-desc':
          filtered.sort((a, b) => b.name.localeCompare(a.name));
          break;
        case 'cards-desc':
          filtered.sort((a, b) => b.cardCount - a.cardCount);
          break;
        case 'cards-asc':
          filtered.sort((a, b) => a.cardCount - b.cardCount);
          break;
      }

      setFilteredSets(filtered);
    }
  }, [data, searchTerm, selectedSetTypes, sortBy]);

  const handleSetClick = (setCode?: string) => {
    if (setCode) {
      window.location.href = `?page=set&set=${setCode}`;
    }
  };

  const handleSetTypeChange = (event: SelectChangeEvent<string[]>) => {
    const value = event.target.value;
    // Check if the last selected item is our special "CLEAR_ALL" value
    if (Array.isArray(value) && value.includes('CLEAR_ALL')) {
      setSelectedSetTypes([]);
      return;
    }
    setSelectedSetTypes(typeof value === 'string' ? value.split(',') : value);
  };

  const handleSortChange = (event: SelectChangeEvent) => {
    setSortBy(event.target.value);
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          Error loading sets: {error.message}
        </Alert>
      </Container>
    );
  }

  if (data?.allSets?.__typename === 'FailureResponse') {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          {data.allSets.status?.message || 'Failed to load sets'}
        </Alert>
      </Container>
    );
  }

  const sets = data?.allSets?.data || [];
  const setTypes = getUniqueSetTypes(sets);

  return (
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
      <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4 }}>
        All Magic Sets
      </Typography>

      {/* Filters and Search */}
      <Box sx={{ mb: 4 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={4}>
            <TextField
              inputRef={searchInputRef}
              fullWidth
              variant="outlined"
              placeholder="Search sets..."
              defaultValue={initialSearch}
              onChange={(e) => {
                const value = e.target.value;
                setHasSearchText(!!value);
                
                // Clear any existing timer
                if (debounceTimer.current) {
                  clearTimeout(debounceTimer.current);
                }
                
                // Set new timer - only update state after 1 second
                debounceTimer.current = setTimeout(() => {
                  setSearchTerm(value);
                }, 1000);
              }}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
                endAdornment: hasSearchText ? (
                  <InputAdornment position="end">
                    <IconButton
                      size="small"
                      onClick={() => {
                        if (searchInputRef.current) {
                          searchInputRef.current.value = '';
                          setHasSearchText(false);
                          setSearchTerm('');
                          // Clear any pending timer
                          if (debounceTimer.current) {
                            clearTimeout(debounceTimer.current);
                          }
                        }
                      }}
                      edge="end"
                    >
                      <ClearIcon fontSize="small" />
                    </IconButton>
                  </InputAdornment>
                ) : null,
              }}
            />
          </Grid>
          
          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth>
              <InputLabel>Set Types</InputLabel>
              <Select
                multiple
                value={selectedSetTypes}
                onChange={handleSetTypeChange}
                label="Set Types"
                sx={{ minWidth: '150px' }}
                renderValue={(selected) => {
                  if (selected.length === 0) {
                    return <Typography variant="body2" color="text.secondary">All Types</Typography>;
                  }
                  if (selected.length <= 2) {
                    return selected.map(s => s.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase())).join(', ');
                  }
                  return `${selected.length} types selected`;
                }}
              >
                <MenuItem 
                  value="CLEAR_ALL"
                  sx={{ borderBottom: '1px solid rgba(255,255,255,0.12)' }}
                >
                  <Typography variant="body2" color="text.secondary">
                    Clear All
                  </Typography>
                </MenuItem>
                {setTypes.map(type => (
                  <MenuItem key={type} value={type}>
                    <Chip
                      size="small"
                      label={type.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase())}
                      color={selectedSetTypes.includes(type) ? "primary" : "default"}
                      sx={{ mr: 1 }}
                    />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth>
              <InputLabel>Sort By</InputLabel>
              <Select
                value={sortBy}
                onChange={handleSortChange}
                label="Sort By"
              >
                <MenuItem value="release-desc">Release Date (Newest)</MenuItem>
                <MenuItem value="release-asc">Release Date (Oldest)</MenuItem>
                <MenuItem value="name-asc">Name (A-Z)</MenuItem>
                <MenuItem value="name-desc">Name (Z-A)</MenuItem>
                <MenuItem value="cards-desc">Card Count (High-Low)</MenuItem>
                <MenuItem value="cards-asc">Card Count (Low-High)</MenuItem>
              </Select>
            </FormControl>
          </Grid>

        </Grid>
      </Box>

      {/* Results Summary */}
      <ResultsSummary 
        showing={filteredSets.length} 
        total={sets.length} 
        itemType="sets" 
      />

      {/* Sets Grid */}
      <Box sx={{ 
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, 240px)',
        gap: 3,
        justifyContent: 'center'
      }}>
        {filteredSets.map((set) => (
          <MtgSetCard
            key={set.id}
            set={set}
            onSetClick={handleSetClick}
          />
        ))}
      </Box>

      {filteredSets.length === 0 && (
        <EmptyState
          message="No sets found matching your criteria"
          description="Try adjusting your filters or search terms"
        />
      )}
    </Container>
  );
};