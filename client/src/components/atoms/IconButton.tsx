import { IconButton as MuiIconButton } from '@mui/material';
import type { IconButtonProps } from './types';

const IconButton = (props: IconButtonProps) => {
  return <MuiIconButton {...props} />;
};

export default IconButton;
