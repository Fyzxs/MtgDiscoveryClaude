import React from 'react';
import { Box, Typography } from '@mui/material';

interface SeparatedListProps {
  /** Array of React nodes to display with separators */
  items: React.ReactNode[];
  /** Separator character or string (default: "|") */
  separator?: string;
  /** Color for the separator (default: "text.secondary") */
  separatorColor?: string;
  /** Typography variant for separator (default: "body2") */
  variant?: 'body1' | 'body2' | 'caption';
  /** Spacing around separator in theme units (default: 1) */
  spacing?: number;
}

/**
 * Renders a list of items with separators between them.
 * Commonly used for displaying links or tags in a horizontal layout.
 *
 * @example
 * <SeparatedList
 *   items={[
 *     <Link onClick={() => navigate('/card/A')}>Card A</Link>,
 *     <Link onClick={() => navigate('/card/B')}>Card B</Link>,
 *   ]}
 *   separator="|"
 * />
 */
export const SeparatedList: React.FC<SeparatedListProps> = ({
  items,
  separator = '|',
  separatorColor = 'text.secondary',
  variant = 'body2',
  spacing = 1,
}) => {
  if (items.length === 0) {
    return null;
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexWrap: 'wrap',
        alignItems: 'center',
      }}
    >
      {items.map((item, index) => (
        <Box key={index} sx={{ display: 'flex', alignItems: 'center' }}>
          {item}
          {index < items.length - 1 && (
            <Typography
              variant={variant}
              component="span"
              sx={{ mx: spacing, color: separatorColor }}
            >
              {separator}
            </Typography>
          )}
        </Box>
      ))}
    </Box>
  );
};
