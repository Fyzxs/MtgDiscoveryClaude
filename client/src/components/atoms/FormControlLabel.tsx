import { FormControlLabel as MuiFormControlLabel } from '@mui/material';
import type { FormControlLabelProps } from './types';

const FormControlLabel = (props: FormControlLabelProps) => {
  return <MuiFormControlLabel {...props} />;
};

export default FormControlLabel;
