import { Icon as MuiIcon } from '@mui/material';
import type { IconProps } from './types';

const Icon = (props: IconProps) => {
  return <MuiIcon {...props} />;
};

export default Icon;
