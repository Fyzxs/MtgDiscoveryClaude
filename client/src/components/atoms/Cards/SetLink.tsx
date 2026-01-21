import Link from '../Link';
import Typography from '../Typography';
import Box from '../Box';
import { SetIcon } from '../Sets/SetIcon';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useLinkParams } from '../../../contexts/LinkParamsContext';
import type { StyledComponentProps } from '../../../types/components';

interface SetLinkProps extends StyledComponentProps {
  setCode?: string;
  setName?: string;
  rarity?: string;
  onSetClick?: (setCode?: string) => void;
  /** Additional URL parameters to include in the link */
  additionalParams?: Record<string, string>;
}

export const SetLink = ({
  setCode,
  setName,
  rarity,
  onSetClick,
  className,
  additionalParams
}: SetLinkProps) => {
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const contextParams = useLinkParams();

  // Use explicit prop if provided, otherwise fall back to context
  const params = additionalParams ?? contextParams;

  if (!setName) return null;

  const setPath = `/set/${setCode?.toLowerCase()}`;
  const href = buildUrlWithCollector(setPath, params);

  return (
    <Box className={className}>
      <Link
        href={href}
        tabIndex={0}
        onKeyDown={(e: React.KeyboardEvent) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.stopPropagation();
            if (onSetClick) {
              e.preventDefault();
              onSetClick(setCode);
            }
          }
        }}
        onClick={(e) => {
          e.stopPropagation();
          // Allow browser's default behavior for modifier keys (CTRL/CMD+click = new tab, etc.)
          if (e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) {
            return;
          }
          if (onSetClick) {
            e.preventDefault();
            onSetClick(setCode);
          } else {
            // Use collector navigation for regular clicks
            createCollectorClickHandler(setPath, params)(e);
          }
        }}
        sx={{
          display: 'inline-flex',
          alignItems: 'center',
          gap: 0.5,
          color: 'white',
          textDecoration: 'none',
          fontSize: '0.75rem',
          px: 0.75,
          py: 0.25,
          borderRadius: 1,
          bgcolor: 'rgba(0, 0, 0, 0.6)',
          '&:hover': {
            bgcolor: 'rgba(0, 0, 0, 0.8)',
            color: 'primary.main'
          },
          transition: 'all 0.2s ease'
        }}
        aria-label={`View all cards from ${setName}`}
      >
        {setCode && (
          <SetIcon
            setCode={setCode}
            rarity={rarity}
            size="small"
          />
        )}
        <Typography 
          variant="caption" 
          sx={{ 
            fontSize: 'inherit',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            whiteSpace: 'nowrap'
          }}
        >
          {setName}{setCode && ` (${setCode})`}
        </Typography>
      </Link>
    </Box>
  );
};