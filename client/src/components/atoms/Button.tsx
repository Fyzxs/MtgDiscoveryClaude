import { Button as MuiButton } from '@mui/material';
import type { ButtonProps } from './types';

const Button = (props: ButtonProps) => {
  return <MuiButton {...props} />;
};

export default Button;
