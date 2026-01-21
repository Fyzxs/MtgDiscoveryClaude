import { TextField as MuiTextField } from '@mui/material';
import type { TextFieldProps } from './types';

const TextField = (props: TextFieldProps) => {
  return <MuiTextField {...props} />;
};

export default TextField;
