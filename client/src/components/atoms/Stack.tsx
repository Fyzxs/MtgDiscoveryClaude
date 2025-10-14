import { Stack as MuiStack } from '@mui/material';
import type { StackProps } from './types';

const Stack = (props: StackProps) => {
  return <MuiStack {...props} />;
};

export default Stack;
