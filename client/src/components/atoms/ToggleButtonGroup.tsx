import { ToggleButtonGroup as MuiToggleButtonGroup } from '@mui/material';
import type { ToggleButtonGroupProps } from './types';

const ToggleButtonGroup = (props: ToggleButtonGroupProps) => {
  return <MuiToggleButtonGroup {...props} />;
};

export default ToggleButtonGroup;
