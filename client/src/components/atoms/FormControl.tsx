import { FormControl as MuiFormControl } from '@mui/material';
import type { FormControlProps } from './types';

const FormControl = (props: FormControlProps) => {
  return <MuiFormControl {...props} />;
};

export default FormControl;
