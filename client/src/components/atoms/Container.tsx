import { Container as MuiContainer } from '@mui/material';
import type { ContainerProps } from './types';

const Container = (props: ContainerProps) => {
  return <MuiContainer {...props} />;
};

export default Container;
