import { MenuItem as MuiMenuItem } from '@mui/material';
import type { MenuItemProps } from './types';

const MenuItem = (props: MenuItemProps) => {
  return <MuiMenuItem {...props} />;
};

export default MenuItem;
