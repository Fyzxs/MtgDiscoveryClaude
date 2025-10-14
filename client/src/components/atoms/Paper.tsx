import { Paper as MuiPaper } from '@mui/material';
import type { PaperProps } from './types';

const Paper = (props: PaperProps) => {
  return <MuiPaper {...props} />;
};

export default Paper;
