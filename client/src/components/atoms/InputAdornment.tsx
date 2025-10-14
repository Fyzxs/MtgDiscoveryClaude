import { InputAdornment as MuiInputAdornment } from '@mui/material';
import type { InputAdornmentProps } from './types';

const InputAdornment = (props: InputAdornmentProps) => {
  return <MuiInputAdornment {...props} />;
};

export default InputAdornment;
