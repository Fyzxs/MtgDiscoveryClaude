import { Select as MuiSelect } from '@mui/material';
import type { SelectProps } from './types';

const Select = (props: SelectProps) => {
  return <MuiSelect {...props} />;
};

export default Select;
