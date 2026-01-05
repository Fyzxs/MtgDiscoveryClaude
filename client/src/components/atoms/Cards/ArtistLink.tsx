import { DarkBadge } from '../shared/DarkBadge';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useLinkParams } from '../../../contexts/LinkParamsContext';
import type { StyledComponentProps } from '../../../types/components';

interface ArtistLinkProps extends StyledComponentProps {
  artistName: string;
  artistId?: string;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  /** Additional URL parameters to include in the link */
  additionalParams?: Record<string, string>;
}

export const ArtistLink = ({
  artistName,
  artistId,
  onArtistClick,
  className,
  additionalParams
}: ArtistLinkProps) => {
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const contextParams = useLinkParams();

  // Use explicit prop if provided, otherwise fall back to context
  const params = additionalParams ?? contextParams;

  const artistPath = `/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`;
  const href = buildUrlWithCollector(artistPath, params);

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
        // Allow browser's default behavior for modifier keys (CTRL/CMD+click = new tab, etc.)
        if (e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) {
          return;
        }
        if (onArtistClick) {
          e.preventDefault();
          onArtistClick(artistName, artistId);
        } else {
          // Use collector navigation for regular clicks
          createCollectorClickHandler(artistPath, params)(e);
        }
      }}
      className={className}
      aria-label={`View cards by ${artistName}`}
    >
      {artistName}
    </DarkBadge>
  );
};