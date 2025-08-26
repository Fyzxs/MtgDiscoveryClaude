import React from 'react';
import { Box, SxProps, Theme } from '@mui/material';

export interface BadgeGroupProps {
  children: React.ReactNode;
  direction?: 'row' | 'column';
  spacing?: number;
  justify?: 'flex-start' | 'flex-end' | 'center' | 'space-between' | 'space-around' | 'space-evenly';
  align?: 'flex-start' | 'flex-end' | 'center' | 'stretch' | 'baseline';
  wrap?: boolean;
  maxWidth?: string | number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Container component for grouping badges with consistent spacing and alignment
 * Replaces TopBadges, BottomBadges patterns with a unified approach
 */
export const BadgeGroup: React.FC<BadgeGroupProps> = React.memo(({
  children,
  direction = 'row',
  spacing = 1,
  justify = 'flex-start',
  align = 'center',
  wrap = true,
  maxWidth,
  sx = {},
  className = ''
}) => {
  return (
    <Box
      className={className}
      sx={{
        display: 'flex',
        flexDirection: direction,
        gap: spacing,
        justifyContent: justify,
        alignItems: align,
        flexWrap: wrap ? 'wrap' : 'nowrap',
        maxWidth,
        width: '100%',
        ...sx
      }}
    >
      {children}
    </Box>
  );
});

interface TopBadgeGroupProps {
  children: React.ReactNode;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Specialized badge group for top positioning (replaces TopBadges pattern)
 */
export const TopBadgeGroup: React.FC<TopBadgeGroupProps> = React.memo(({
  children,
  sx = {},
  className = ''
}) => {
  return (
    <BadgeGroup
      direction="row"
      justify="space-between"
      align="center"
      spacing={1}
      className={className}
      sx={{
        mb: 1,
        ...sx
      }}
    >
      {children}
    </BadgeGroup>
  );
});

interface BottomBadgeGroupProps {
  children: React.ReactNode;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Specialized badge group for bottom positioning (replaces BottomBadges pattern)
 */
export const BottomBadgeGroup: React.FC<BottomBadgeGroupProps> = React.memo(({
  children,
  sx = {},
  className = ''
}) => {
  return (
    <BadgeGroup
      direction="row"
      justify="flex-start"
      align="center"
      spacing={1}
      maxWidth="180px"
      className={className}
      sx={{
        mb: 1,
        ...sx
      }}
    >
      {children}
    </BadgeGroup>
  );
});

interface InlineBadgeGroupProps {
  children: React.ReactNode;
  spacing?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Badge group for inline display within text or compact areas
 */
export const InlineBadgeGroup: React.FC<InlineBadgeGroupProps> = React.memo(({
  children,
  spacing = 0.5,
  sx = {},
  className = ''
}) => {
  return (
    <BadgeGroup
      direction="row"
      justify="flex-start"
      align="center"
      spacing={spacing}
      wrap={false}
      className={className}
      sx={{
        display: 'inline-flex',
        verticalAlign: 'middle',
        ...sx
      }}
    >
      {children}
    </BadgeGroup>
  );
});

BadgeGroup.displayName = 'BadgeGroup';
TopBadgeGroup.displayName = 'TopBadgeGroup';
BottomBadgeGroup.displayName = 'BottomBadgeGroup';
InlineBadgeGroup.displayName = 'InlineBadgeGroup';