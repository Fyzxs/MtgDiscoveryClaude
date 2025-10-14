import React from 'react';
import { Box, Typography, Link } from '../../atoms';
import type { SxProps, Theme } from '../../atoms';
import type { CardContext } from '../../../types/card';

interface ArtistInfoProps {
  artist?: string;
  artistIds?: string[];
  context?: CardContext;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
  sx?: SxProps<Theme>;
}

export const ArtistInfo: React.FC<ArtistInfoProps> = ({
  artist,
  artistIds,
  context = {},
  onArtistClick,
  className = '',
  sx
}) => {
  if (!artist) return null;

  // Parse multiple artists (separated by & or and)
  const parseArtists = (artistString: string): string[] => {
    return artistString.split(/\s+(?:&|and)\s+/i);
  };

  const artists = parseArtists(artist);
  const isMultipleArtists = artists.length > 1;

  // Don't show artist on artist page unless there are multiple artists
  if (context.isOnArtistPage && !isMultipleArtists) {
    return null;
  }

  // On artist page with multiple artists, show only the other artists
  // Use case-insensitive comparison to handle name variations
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a.trim().toLowerCase() !== context.currentArtist?.trim().toLowerCase())
    : artists;

  if (displayArtists.length === 0) return null;

  const handleArtistClick = (artistName: string, index: number) => {
    const artistId = artistIds?.[index];
    if (onArtistClick) {
      onArtistClick(artistName, artistId);
    }
  };

  return (
    <Box className={className} sx={{ display: 'flex', alignItems: 'center', flexWrap: 'wrap', ...sx }}>
      <Typography 
        variant="caption" 
        sx={{ 
          color: 'text.secondary',
          fontSize: { xs: '0.75rem', sm: '0.875rem' },
          mr: 0.5
        }}
      >
        Illus.
      </Typography>
      {displayArtists.map((artistName, index) => (
        <React.Fragment key={index}>
          {index > 0 && (
            <Typography 
              variant="caption" 
              component="span"
              sx={{ 
                color: 'text.secondary',
                mx: 0.5,
                fontSize: { xs: '0.75rem', sm: '0.875rem' }
              }}
            >
              &
            </Typography>
          )}
          <Link
            href={`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`}
            onClick={(e: React.MouseEvent) => {
              if (onArtistClick) {
                e.preventDefault();
                handleArtistClick(artistName, index);
              }
            }}
            sx={{
              color: 'grey.300',
              fontSize: { xs: '0.75rem', sm: '0.875rem' },
              textDecorationOffset: '2px',
              cursor: 'pointer',
              transition: 'color 0.2s',
              '&:hover': {
                color: 'white',
                textDecoration: 'underline'
              }
            }}
          >
            {artistName}
          </Link>
        </React.Fragment>
      ))}
    </Box>
  );
};