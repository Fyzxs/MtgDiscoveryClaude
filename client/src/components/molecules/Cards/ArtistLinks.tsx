import React from 'react';
import { Typography } from '../../atoms';
import { ArtistLink } from '../../atoms';
import type { CardContext } from '../../../types/card';

interface ArtistLinksProps {
  artists: string[];
  artistIds?: string[];
  context?: CardContext;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const ArtistLinks: React.FC<ArtistLinksProps> = ({
  artists,
  artistIds,
  context = {},
  onArtistClick,
  className
}) => {
  if (!artists || artists.length === 0) return null;

  // Don't show artist on artist page unless there are multiple artists
  const isMultipleArtists = artists.length > 1;
  if (context.isOnArtistPage && !isMultipleArtists) {
    return null;
  }

  // On artist page with multiple artists, show only the other artists
  // Use case-insensitive comparison to handle name variations
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a.trim().toLowerCase() !== context.currentArtist?.trim().toLowerCase())
    : artists;

  if (displayArtists.length === 0) return null;

  return (
    <Typography variant="caption" sx={{ fontSize: '0.75rem', display: 'flex', gap: 0.5, flexWrap: 'wrap', alignItems: 'center' }} className={className}>
      {displayArtists.map((artistName, index) => {
        // Find the original index to get the correct artistId
        const originalIndex = artists.indexOf(artistName);
        return (
          <React.Fragment key={index}>
            {index > 0 && (
              <Typography component="span" sx={{ color: 'grey.400', fontSize: '0.7rem', mx: 0.25 }}>
                &
              </Typography>
            )}
            <ArtistLink
              artistName={artistName}
              artistId={artistIds?.[originalIndex]}
              onArtistClick={onArtistClick}
            />
          </React.Fragment>
        );
      })}
    </Typography>
  );
};