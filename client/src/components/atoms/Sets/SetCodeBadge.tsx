import { Chip } from '@mui/material';

interface SetCodeBadgeProps {
  code: string;
}

export const SetCodeBadge = ({ code }: SetCodeBadgeProps) => {
  return (
    <Chip
      label={code.toUpperCase()}
      size="small"
      variant="filled"
      color="primary"
      sx={{
        fontFamily: 'monospace',
        fontWeight: 600,
        fontSize: '0.875rem',
      }}
    />
  );
};