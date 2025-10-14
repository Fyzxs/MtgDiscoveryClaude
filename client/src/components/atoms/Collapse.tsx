import { Collapse as MuiCollapse } from '@mui/material';
import type { CollapseProps } from './types';

const Collapse = (props: CollapseProps) => {
  return <MuiCollapse {...props} />;
};

export default Collapse;
