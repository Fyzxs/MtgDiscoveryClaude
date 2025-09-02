import { DarkBadge } from '../shared/DarkBadge';
import type { StyledComponentProps } from '../../../types/components';

interface ArtistLinkProps extends StyledComponentProps {
  artistName: string;
  artistId?: string;
  onArtistClick?: (artistName: string, artistId?: string) => void;
}

export const ArtistLink = ({ 
  artistName,
  artistId,
  onArtistClick,
  className 
}: ArtistLinkProps) => {
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