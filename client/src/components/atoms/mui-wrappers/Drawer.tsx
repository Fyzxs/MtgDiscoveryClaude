import { Drawer as MuiDrawer } from '@mui/material';
import type { DrawerProps } from './types';

const Drawer = (props: DrawerProps) => {
  return <MuiDrawer {...props} />;
};

export default Drawer;
