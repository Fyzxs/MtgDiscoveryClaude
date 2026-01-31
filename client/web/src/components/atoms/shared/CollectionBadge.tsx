import { Chip } from '@mui/material';
import type { ChipProps, SxProps, Theme } from '@mui/material';

interface CollectionBadgeProps {
  type: string;
  isDefault?: boolean;
  size?: ChipProps['size'];
  sx?: SxProps<Theme>;
}

const typeColors: Record<string, { bgcolor: string; color: string }> = {
  default: { bgcolor: 'grey.700', color: 'grey.100' },
  custom: { bgcolor: 'primary.main', color: 'primary.contrastText' },
  cube: { bgcolor: 'secondary.main', color: 'secondary.contrastText' },
  trade: { bgcolor: 'success.main', color: 'success.contrastText' },
};

export function CollectionBadge({ type, isDefault = false, size = 'small', sx }: CollectionBadgeProps) {
  const displayType = isDefault ? 'default' : type;
  const colors = typeColors[displayType] || typeColors.custom;

  return (
    <Chip
      label={displayType.charAt(0).toUpperCase() + displayType.slice(1)}
      size={size}
      sx={{
        bgcolor: colors.bgcolor,
        color: colors.color,
        fontWeight: 500,
        ...sx
      }}
    />
  );
}
