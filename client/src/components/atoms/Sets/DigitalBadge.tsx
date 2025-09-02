import { Chip } from '@mui/material';

interface DigitalBadgeProps {
  show: boolean;
}

export const DigitalBadge = ({ show }: DigitalBadgeProps) => {
  if (!show) {
    return null;
  }

  return (
    <Chip
      label="Digital"
      size="small"
      color="info"
      variant="filled"
    />
  );
};