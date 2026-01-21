import { CardContent as MuiCardContent } from '@mui/material';
import type { CardContentProps } from './types';

const CardContent = (props: CardContentProps) => {
  return <MuiCardContent {...props} />;
};

export default CardContent;
