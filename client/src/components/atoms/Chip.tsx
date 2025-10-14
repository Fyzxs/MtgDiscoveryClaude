import { Chip as MuiChip } from '@mui/material';
import type { ChipProps } from './types';

const Chip = (props: ChipProps) => {
  return <MuiChip {...props} />;
};

export default Chip;
