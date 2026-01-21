import { Typography as MuiTypography } from '@mui/material';
import type { TypographyProps } from './types';

const Typography = (props: TypographyProps) => {
  return <MuiTypography {...props} />;
};

export default Typography;
