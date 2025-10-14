import { DarkBadge } from '../shared/DarkBadge';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
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
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();

  const artistPath = `/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`;
  const href = buildUrlWithCollector(artistPath);

  return (
    <DarkBadge
      component="a"
      href={href}
      tabIndex={0}
      onKeyDown={(e: React.KeyboardEvent) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.stopPropagation();
          if (onArtistClick) {
            e.preventDefault();
            onArtistClick(artistName, artistId);
          }
        }
      }}
      onClick={(e) => {
        e.stopPropagation();
        if (onArtistClick) {
          e.preventDefault();
          onArtistClick(artistName, artistId);
        } else {
          // Use collector navigation for regular clicks
          createCollectorClickHandler(artistPath)(e);
        }
      }}
      className={className}
      aria-label={`View cards by ${artistName}`}
    >
      {artistName}
    </DarkBadge>
  );
};