import { Zoom as MuiZoom } from '@mui/material';
import type { ZoomProps } from './types';

const Zoom = (props: ZoomProps) => {
  return <MuiZoom {...props} />;
};

export default Zoom;
