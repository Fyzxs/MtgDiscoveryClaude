import { LinearProgress as MuiLinearProgress } from '@mui/material';
import type { LinearProgressProps } from './types';

const LinearProgress = (props: LinearProgressProps) => {
  return <MuiLinearProgress {...props} />;
};

export default LinearProgress;
