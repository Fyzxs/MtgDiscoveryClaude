import React from 'react';
import { Typography, type TypographyProps } from '../../atoms';

/**
 * Heading - Text molecule for page headings
 *
 * Wraps MUI Typography with semantic heading defaults.
 * Should be used by pages and templates instead of importing Typography atom directly.
 *
 * @example
 * <Heading variant="h1">Magic: The Gathering Collection</Heading>
 *
 * @example
 * <Heading variant="h2" sx={{ color: 'rarity.mythic' }}>
 *   Rare Cards
 * </Heading>
 */
export const Heading: React.FC<TypographyProps> = ({
  children,
  variant = 'h1',
  ...props
}) => {
  return (
    <Typography variant={variant} {...props}>
      {children}
    </Typography>
  );
};
