import { Checkbox as MuiCheckbox } from '@mui/material';
import type { CheckboxProps } from './types';

const Checkbox = (props: CheckboxProps) => {
  return <MuiCheckbox {...props} />;
};

export default Checkbox;
