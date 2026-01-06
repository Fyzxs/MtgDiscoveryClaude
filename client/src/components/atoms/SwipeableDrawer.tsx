import { SwipeableDrawer as MuiSwipeableDrawer } from '@mui/material';
import type { SwipeableDrawerProps } from '@mui/material';

const SwipeableDrawer = (props: SwipeableDrawerProps) => {
  return <MuiSwipeableDrawer {...props} />;
};

export default SwipeableDrawer;
export type { SwipeableDrawerProps };
