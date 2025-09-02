import React from 'react';
import { Box } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';
import type { StyledComponentProps } from '../../../types/components';

interface DarkBadgeProps extends StyledComponentProps {
  children: React.ReactNode;
  onClick?: (e: React.MouseEvent) => void;
  onKeyDown?: (e: React.KeyboardEvent) => void;
  href?: string;
  tabIndex?: number;
  component?: 'div' | 'a' | 'span';
  'aria-label'?: string;
}

/**
 * Reusable badge component with semi-transparent dark background
 * Used for artist names, card names, and price displays on cards
 */
export const DarkBadge: React.FC<DarkBadgeProps> = ({
  children,
  onClick,
  onKeyDown,
  href,
  tabIndex,
  className,
  component = 'div',
  'aria-label': ariaLabel,
  sx = {}
}) => {
  const isLink = component === 'a' || href;
  
  const baseStyles: SxProps<Theme> = {
    color: 'white',
    textDecoration: 'none',
    display: 'inline-block',
    px: 0.75,
    py: 0.25,
    borderRadius: 2,
    bgcolor: 'rgba(0, 0, 0, 0.5)',
    backdropFilter: 'blur(4px)',
    border: '1px solid rgba(255, 255, 255, 0.1)',
    transition: 'all 0.2s ease',
    ...(isLink && {
      cursor: 'pointer',
      '&:hover': {
        bgcolor: 'rgba(0, 0, 0, 0.9)',
        color: 'primary.light',
        borderColor: 'primary.dark',
        transform: 'translateY(-1px)',
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.5)',
        textDecoration: 'none'
      }
    }),
    ...sx
  };

  if (isLink) {
    return (
      <Box
        component="a"
        href={href}
        onClick={onClick}
        onKeyDown={onKeyDown}
        tabIndex={tabIndex}
        className={className}
        aria-label={ariaLabel}
        sx={baseStyles}
      >
        {children}
      </Box>
    );
  }

  return (
    <Box
      component={component}
      onClick={onClick}
      onKeyDown={onKeyDown}
      tabIndex={tabIndex}
      className={className}
      aria-label={ariaLabel}
      sx={baseStyles}
    >
      {children}
    </Box>
  );
};