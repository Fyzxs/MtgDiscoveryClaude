import React, { useMemo, useCallback } from 'react';
import { Box, Typography, Paper } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';

interface ArtistNameResult {
  artistId: string;
  name: string;
}

interface ArtistSearchResultsProps {
  artists: ArtistNameResult[];
  onArtistClick: (artistName: string) => void;
  searchTerm?: string;
}

/**
 * ArtistSearchResults - Displays a list of artist search results with sorting and navigation
 *
 * Features:
 * - Alphabetical sorting of results
 * - Hover effects and transitions
 * - Accessible navigation with keyboard support
 * - Result count display
 */
export const ArtistSearchResults: React.FC<ArtistSearchResultsProps> = React.memo(({
  artists,
  onArtistClick,
  searchTerm
}) => {
  const sortedArtists = useMemo(() =>
    [...artists].sort((a, b) => a.name.localeCompare(b.name)),
    [artists]
  );

  const resultCountText = useMemo(() =>
    `Found ${artists.length} artist${artists.length !== 1 ? 's' : ''}${searchTerm ? ` matching "${searchTerm}"` : ''}`,
    [artists.length, searchTerm]
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
            key={artist.artistId}
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
  styles: SxProps<Theme>;
}>(({ artist, onArtistClick, styles }) => {
  const artistUrl = `/artists/${encodeURIComponent(artist.name.toLowerCase().replace(/\s+/g, '-'))}`;

  const handleClick = useCallback((e: React.MouseEvent) => {
    // Only prevent default for left clicks to allow right-click context menu
    if (e.button === 0) {
      e.preventDefault();
      onArtistClick(artist.name);
    }
  }, [artist.name, onArtistClick]);

  return (
    <Paper
      elevation={0}
      component="a"
      href={artistUrl}
      onClick={handleClick}
      sx={{
        ...styles,
        textDecoration: 'none',
        color: 'inherit'
      }}
    >
      <Typography variant="body2">
        {artist.name}
      </Typography>
    </Paper>
  );
});

ArtistResult.displayName = 'ArtistResult';
ArtistSearchResults.displayName = 'ArtistSearchResults';