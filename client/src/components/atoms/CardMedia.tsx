import { CardMedia as MuiCardMedia } from '@mui/material';
import type { CardMediaProps } from './types';

const CardMedia = (props: CardMediaProps) => {
  return <MuiCardMedia {...props} />;
};

export default CardMedia;
