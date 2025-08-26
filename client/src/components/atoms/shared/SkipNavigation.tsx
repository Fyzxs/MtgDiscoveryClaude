import React from 'react';
import { Link, SxProps, Theme } from '@mui/material';

interface SkipNavigationProps {
  targetId?: string;
  label?: string;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Skip navigation link for accessibility
 * Allows keyboard users to quickly jump to main content
 */
export const SkipNavigation: React.FC<SkipNavigationProps> = React.memo(({
  targetId = 'main-content',
  label = 'Skip to main content',
  sx = {},
  className = ''
}) => {
  const handleClick = (event: React.MouseEvent<HTMLAnchorElement>) => {
    event.preventDefault();
    const target = document.getElementById(targetId);
    if (target) {
      target.scrollIntoView({ behavior: 'smooth', block: 'start' });
      target.focus();
    }
  };

  return (
    <Link
      href={`#${targetId}`}
      onClick={handleClick}
      className={className}
      sx={{
        position: 'absolute',
        left: '-9999px',
        zIndex: 999999,
        padding: '8px 16px',
        backgroundColor: 'primary.main',
        color: 'primary.contrastText',
        textDecoration: 'none',
        borderRadius: 1,
        fontSize: '0.875rem',
        fontWeight: 'bold',
        border: '2px solid',
        borderColor: 'primary.main',
        transition: 'all 0.2s ease',
        '&:focus': {
          left: '8px',
          top: '8px',
          outline: '2px solid',
          outlineColor: 'secondary.main',
          outlineOffset: '2px'
        },
        '&:hover:focus': {
          backgroundColor: 'primary.dark',
          borderColor: 'primary.dark'
        },
        ...sx
      }}
    >
      {label}
    </Link>
  );
});

interface SkipNavigationGroupProps {
  links: Array<{
    targetId: string;
    label: string;
  }>;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Group of skip navigation links for multiple sections
 */
export const SkipNavigationGroup: React.FC<SkipNavigationGroupProps> = React.memo(({
  links,
  sx = {},
  className = ''
}) => {
  if (links.length === 0) return null;

  return (
    <nav
      className={className}
      aria-label="Skip navigation"
      sx={{
        position: 'absolute',
        top: 0,
        left: 0,
        zIndex: 999999,
        ...sx
      }}
    >
      {links.map((link, index) => (
        <SkipNavigation
          key={link.targetId}
          targetId={link.targetId}
          label={link.label}
          sx={{
            top: `${8 + index * 44}px`, // Stack vertically with 44px spacing
          }}
        />
      ))}
    </nav>
  );
});

SkipNavigation.displayName = 'SkipNavigation';
SkipNavigationGroup.displayName = 'SkipNavigationGroup';