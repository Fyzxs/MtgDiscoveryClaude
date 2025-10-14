import { Box as MuiBox } from '@mui/material';
import type { BoxProps } from './types';

const Box = (props: BoxProps) => {
  return <MuiBox {...props} />;
};

export default Box;
