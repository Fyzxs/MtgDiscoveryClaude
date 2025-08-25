import React from 'react';
import { Link } from '@mui/material';

interface ArtistLinkProps {
  artistName: string;
  artistId?: string;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const ArtistLink: React.FC<ArtistLinkProps> = ({ 
  artistName,
  artistId,
  onArtistClick,
  className 
}) => {
  return (
    <Link
      href={`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`}
      tabIndex={-1}
      onClick={(e) => {
        e.stopPropagation();
        if (onArtistClick) {
          e.preventDefault();
          onArtistClick(artistName, artistId);
        }
      }}
      sx={{
        color: 'white',
        textDecoration: 'none',
        px: 0.5,
        py: 0.25,
        borderRadius: 1,
        display: 'inline-block',
        '&:hover': {
          bgcolor: 'rgba(0, 0, 0, 1)',
          color: 'primary.main',
          textDecoration: 'none'
        },
        transition: 'all 0.2s ease'
      }}
      className={className}
      aria-label={`View cards by ${artistName}`}
    >
      {artistName}
    </Link>
  );
};