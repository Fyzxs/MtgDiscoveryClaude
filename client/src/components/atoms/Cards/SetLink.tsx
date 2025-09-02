import { Link, Typography, Box } from '@mui/material';
import { SetIcon } from '../Sets/SetIcon';
import type { StyledComponentProps } from '../../../types/components';

interface SetLinkProps extends StyledComponentProps {
  setCode?: string;
  setName?: string;
  rarity?: string;
  onSetClick?: (setCode?: string) => void;
}

export const SetLink = ({ 
  setCode,
  setName,
  rarity,
  onSetClick,
  className 
}: SetLinkProps) => {
  if (!setName) return null;

  return (
    <Box className={className}>
      <Link
        href={`/set/${setCode?.toLowerCase()}`}
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
          if (onSetClick) {
            e.preventDefault();
            onSetClick(setCode);
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
            className="group-hover/set:opacity-100"
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