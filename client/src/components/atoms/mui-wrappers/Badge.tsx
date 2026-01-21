import { Badge as MuiBadge } from '@mui/material';
import type { BadgeProps } from './types';

const Badge = (props: BadgeProps) => {
  return <MuiBadge {...props} />;
};

export default Badge;
