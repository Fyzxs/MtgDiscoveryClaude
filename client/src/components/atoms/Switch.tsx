import { Switch as MuiSwitch } from '@mui/material';
import type { SwitchProps } from './types';

const Switch = (props: SwitchProps) => {
  return <MuiSwitch {...props} />;
};

export default Switch;
