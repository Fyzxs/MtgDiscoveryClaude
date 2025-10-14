import Chip from '../Chip';

interface FoilOnlyBadgeProps {
  show: boolean;
}

export const FoilOnlyBadge = ({ show }: FoilOnlyBadgeProps) => {
  if (!show) {
    return null;
  }

  return (
    <Chip
      label="Foil"
      size="small"
      variant="filled"
      sx={{
        fontWeight: 500,
        backgroundColor: '#ffe96aff',
        color: '#000000'
      }}
    />
  );
};