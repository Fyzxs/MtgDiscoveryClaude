import { Skeleton as MuiSkeleton } from '@mui/material';
import type { SkeletonProps } from './types';

const Skeleton = (props: SkeletonProps) => {
  return <MuiSkeleton {...props} />;
};

export default Skeleton;
