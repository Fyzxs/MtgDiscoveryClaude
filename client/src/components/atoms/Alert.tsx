import { Alert as MuiAlert } from '@mui/material';
import type { AlertProps } from './types';

const Alert = (props: AlertProps) => {
  return <MuiAlert {...props} />;
};

export default Alert;
