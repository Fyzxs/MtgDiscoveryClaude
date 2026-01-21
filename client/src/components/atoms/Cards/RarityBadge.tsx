import Chip from '../Chip';
import type { Rarity } from '../../../types/card';
import type { StyledComponentProps } from '../../../types/components';

interface RarityBadgeProps extends StyledComponentProps {
  rarity: Rarity | string;
  /** Size variant for responsive display */
  size?: 'small' | 'medium';
}

export const RarityBadge = ({ rarity, className = '', size = 'medium' }: RarityBadgeProps) => {
  const getRarityColors = (r: string): { background: string; color: string } => {
    switch (r.toLowerCase()) {
      case 'common':
        return { background: '#4B5563', color: '#F3F4F6' };
      case 'uncommon':
        return { background: '#9CA3AF', color: '#111827' };
      case 'rare':
        return { background: '#D97706', color: '#FEF3C7' };
      case 'mythic':
        return { background: '#EA580C', color: '#FFEDD5' };
      case 'special':
      case 'bonus':
        return { background: '#9333EA', color: '#F3E8FF' };
      default:
        return { background: '#374151', color: '#D1D5DB' };
    }
  };

  const getRaritySymbol = (r: string): string => {
    switch (r.toLowerCase()) {
      case 'common':
        return 'C';
      case 'uncommon':
        return 'U';
      case 'rare':
        return 'R';
      case 'mythic':
        return 'M';
      case 'special':
        return 'S';
      case 'bonus':
        return 'B';
      default:
        return '?';
    }
  };

  const colors = getRarityColors(rarity);
  const chipSize = size === 'small' ? 20 : 24;
  const fontSize = size === 'small' ? '0.625rem' : '0.75rem';

  return (
    <Chip
      label={getRaritySymbol(rarity)}
      className={className}
      sx={{
        width: chipSize,
        height: chipSize,
        minWidth: chipSize,
        backgroundColor: colors.background,
        color: colors.color,
        fontSize,
        fontWeight: 'bold',
        '& .MuiChip-label': {
          px: 0,
          py: 0
        }
      }}
      title={rarity}
    />
  );
};