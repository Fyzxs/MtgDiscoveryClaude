import Chip from '../Chip';

interface SetCodeBadgeProps {
  code: string;
  /** Compact mode for smaller cards */
  compact?: boolean;
}

export const SetCodeBadge = ({ code, compact = false }: SetCodeBadgeProps) => {
  return (
    <Chip
      label={code.toUpperCase()}
      size="small"
      variant="filled"
      color="primary"
      sx={{
        fontFamily: 'monospace',
        fontWeight: 600,
        fontSize: compact ? '0.5625rem' : '0.875rem',
        height: compact ? 18 : undefined,
        '& .MuiChip-label': {
          px: compact ? 0.5 : 1,
        },
      }}
    />
  );
};