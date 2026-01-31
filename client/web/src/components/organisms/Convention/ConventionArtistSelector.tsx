import React, { useState, useCallback } from 'react';
import {
  Box,
  TextField,
  Chip,
  Paper,
  Typography,
  IconButton,
  Autocomplete,
  CircularProgress
} from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import { useLazyQuery } from '@apollo/client/react';
import { ARTIST_NAME_SEARCH } from '../../../graphql/queries/artistSearch';

interface ArtistSearchResult {
  artistId: string;
  name: string;
}

interface ConventionArtistSelectorProps {
  selectedArtists: ArtistSearchResult[];
  onAddArtist: (artist: ArtistSearchResult) => void;
  onRemoveArtist: (artistId: string) => void;
  onClearAll: () => void;
}

interface ArtistSearchResponse {
  artistSearch: {
    __typename: string;
    data?: ArtistSearchResult[];
  };
}

/**
 * ConventionArtistSelector - Search and select artists for convention signing
 */
export const ConventionArtistSelector: React.FC<ConventionArtistSelectorProps> = ({
  selectedArtists,
  onAddArtist,
  onRemoveArtist,
  onClearAll
}) => {
  const [inputValue, setInputValue] = useState('');

  const [searchArtists, { loading, data }] = useLazyQuery<ArtistSearchResponse>(ARTIST_NAME_SEARCH, {
    fetchPolicy: 'network-only'
  });

  // Derive options from query data, filtering out already selected artists
  const options = React.useMemo(() => {
    if (data?.artistSearch?.__typename === 'ArtistSearchResultsSuccessResponse' && data.artistSearch.data) {
      return data.artistSearch.data.filter(
        artist => !selectedArtists.some(selected => selected.artistId === artist.artistId)
      );
    }
    return [];
  }, [data, selectedArtists]);

  const handleInputChange = useCallback((_event: React.SyntheticEvent, value: string) => {
    setInputValue(value);

    if (value.length >= 2) {
      searchArtists({
        variables: {
          searchTerm: { searchTerm: value }
        }
      });
    }
  }, [searchArtists]);

  const handleSelect = useCallback((_event: React.SyntheticEvent, value: ArtistSearchResult | null) => {
    if (value) {
      onAddArtist(value);
      // Keep the input value so user can continue selecting from results
    }
  }, [onAddArtist]);

  return (
    <Box sx={{ width: '100%', maxWidth: 800, mx: 'auto' }}>
      {/* Search Input */}
      <Autocomplete<ArtistSearchResult>
        freeSolo={false}
        options={options}
        loading={loading}
        inputValue={inputValue}
        onInputChange={handleInputChange}
        onChange={handleSelect}
        getOptionLabel={(option) => option.name}
        isOptionEqualToValue={(option, value) => option.artistId === value.artistId}
        filterOptions={(x) => x} // Disable local filtering, use server results
        renderInput={(params) => (
          <TextField
            {...params}
            label="Search for artists"
            placeholder="Type at least 2 characters to search..."
            variant="outlined"
            fullWidth
            InputProps={{
              ...params.InputProps,
              endAdornment: (
                <>
                  {loading && <CircularProgress color="inherit" size={20} />}
                  {params.InputProps.endAdornment}
                </>
              )
            }}
          />
        )}
        renderOption={(props, option) => {
          const { key, ...rest } = props;
          return (
            <li key={key} {...rest}>
              <Typography>{option.name}</Typography>
            </li>
          );
        }}
        noOptionsText={inputValue.length < 2 ? 'Type at least 2 characters...' : 'No artists found'}
      />

      {/* Selected Artists */}
      {selectedArtists.length > 0 && (
        <Paper
          elevation={0}
          sx={{
            mt: 2,
            p: 2,
            bgcolor: 'grey.900',
            borderRadius: 2
          }}
        >
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
            <Typography variant="subtitle2" color="text.secondary">
              Selected Artists ({selectedArtists.length})
            </Typography>
            <IconButton size="small" onClick={onClearAll} title="Clear all artists">
              <ClearIcon fontSize="small" />
            </IconButton>
          </Box>

          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
            {selectedArtists.map((artist) => (
              <Chip
                key={artist.artistId}
                label={artist.name}
                onDelete={() => onRemoveArtist(artist.artistId)}
                color="primary"
                variant="outlined"
                sx={{
                  borderColor: 'primary.main',
                  '& .MuiChip-deleteIcon': {
                    color: 'primary.main',
                    '&:hover': {
                      color: 'primary.light'
                    }
                  }
                }}
              />
            ))}
          </Box>
        </Paper>
      )}
    </Box>
  );
};
