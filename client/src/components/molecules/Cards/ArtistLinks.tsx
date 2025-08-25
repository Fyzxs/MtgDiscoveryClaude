import React from 'react';
import { Typography } from '@mui/material';
import { ArtistLink } from '../../atoms/Cards/ArtistLink';

interface ArtistLinksProps {
  artists: string[];
  artistIds?: string[];
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const ArtistLinks: React.FC<ArtistLinksProps> = ({ 
  artists,
  artistIds,
  onArtistClick,
  className 
}) => {
  if (!artists || artists.length === 0) return null;

  return (
    <Typography variant="caption" sx={{ fontSize: '0.75rem' }} className={className}>
      {artists.map((artistName, index) => (
        <React.Fragment key={index}>
          {index > 0 && (
            <Typography component="span" sx={{ color: 'grey.500' }}>
              {' & '}
            </Typography>
          )}
          <ArtistLink
            artistName={artistName}
            artistId={artistIds?.[index]}
            onArtistClick={onArtistClick}
          />
        </React.Fragment>
      ))}
    </Typography>
  );
};