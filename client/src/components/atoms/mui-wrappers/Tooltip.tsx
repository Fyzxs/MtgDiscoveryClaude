import { Tooltip as MuiTooltip } from '@mui/material';
import type { TooltipProps } from './types';

const Tooltip = (props: TooltipProps) => {
  return <MuiTooltip {...props} />;
};

export default Tooltip;
