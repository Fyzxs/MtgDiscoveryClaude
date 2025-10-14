import { CardActionArea as MuiCardActionArea } from '@mui/material';
import type { CardActionAreaProps } from './types';

const CardActionArea = (props: CardActionAreaProps) => {
  return <MuiCardActionArea {...props} />;
};

export default CardActionArea;
