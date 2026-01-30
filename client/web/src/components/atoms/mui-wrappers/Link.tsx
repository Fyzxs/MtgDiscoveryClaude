import { Link as MuiLink } from '@mui/material';
import type { LinkProps } from './types';

const Link = (props: LinkProps) => {
  return <MuiLink {...props} />;
};

export default Link;
