import { ToggleButton as MuiToggleButton } from '@mui/material';
import type { ToggleButtonProps } from './types';

const ToggleButton = (props: ToggleButtonProps) => {
  return <MuiToggleButton {...props} />;
};

export default ToggleButton;
