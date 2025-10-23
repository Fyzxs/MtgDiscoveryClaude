import React from 'react';
import { Typography, type TypographyProps } from '../../atoms';

/**
 * BodyText - Text molecule for body content
 *
 * Wraps MUI Typography with body text defaults.
 * Should be used by pages and templates instead of importing Typography atom directly.
 *
 * @example
 * <BodyText>This set contains 271 cards.</BodyText>
 *
 * @example
 * <BodyText variant="body2" color="text.secondary">
 *   Released: March 2024
 * </BodyText>
 */
export const BodyText: React.FC<TypographyProps> = ({
  children,
  variant = 'body1',
  ...props
}) => {
  return (
    <Typography variant={variant} {...props}>
      {children}
    </Typography>
  );
};
