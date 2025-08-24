import React from 'react';
import type { CardContext } from '../../types/card';

interface ArtistInfoProps {
  artist?: string;
  artistIds?: string[];
  context?: CardContext;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const ArtistInfo: React.FC<ArtistInfoProps> = ({ 
  artist, 
  artistIds,
  context = {},
  onArtistClick,
  className = '' 
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
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a !== context.currentArtist)
    : artists;

  if (displayArtists.length === 0) return null;

  const handleArtistClick = (artistName: string, index: number) => {
    const artistId = artistIds?.[index];
    if (onArtistClick) {
      onArtistClick(artistName, artistId);
    }
  };

  return (
    <div className={`text-xs sm:text-sm text-gray-400 ${className}`}>
      <span className="text-gray-500">Illus.</span>{' '}
      {displayArtists.map((artistName, index) => (
        <React.Fragment key={index}>
          {index > 0 && <span className="text-gray-500"> & </span>}
          <a
            href={`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`}
            onClick={(e) => {
              if (onArtistClick) {
                e.preventDefault();
                handleArtistClick(artistName, index);
              }
            }}
            className="text-gray-300 hover:text-white cursor-pointer transition-colors underline-offset-2 hover:underline"
          >
            {artistName}
          </a>
        </React.Fragment>
      ))}
    </div>
  );
};