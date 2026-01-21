import { Card as MuiCard } from '@mui/material';
import type { CardProps } from './types';

const Card = (props: CardProps) => {
  return <MuiCard {...props} />;
};

export default Card;
