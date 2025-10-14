import { CircularProgress as MuiCircularProgress } from '@mui/material';
import type { CircularProgressProps } from './types';

const CircularProgress = (props: CircularProgressProps) => {
  return <MuiCircularProgress {...props} />;
};

export default CircularProgress;
