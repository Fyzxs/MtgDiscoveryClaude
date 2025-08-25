import React from 'react';
import { DarkBadge } from '../shared/DarkBadge';

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
    <DarkBadge
      component="a"
      href={`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`}
      tabIndex={-1}
      onClick={(e) => {
        e.stopPropagation();
        if (onArtistClick) {
          e.preventDefault();
          onArtistClick(artistName, artistId);
        }
      }}
      className={className}
      aria-label={`View cards by ${artistName}`}
    >
      {artistName}
    </DarkBadge>
  );
};