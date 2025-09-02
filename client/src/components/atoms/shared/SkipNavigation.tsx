import React from 'react';
import { Button, Box } from '@mui/material';

interface SkipNavigationProps {
  links?: Array<{
    href: string;
    label: string;
  }>;
}

const defaultLinks = [
  { href: '#main-content', label: 'Skip to main content' },
  { href: '#filter-panel', label: 'Skip to filters' },
  { href: '#search-results', label: 'Skip to search results' }
];

/**
 * Skip navigation links for keyboard accessibility
 * These links are visually hidden until focused, allowing keyboard users
 * to quickly jump to important sections of the page
 */
export const SkipNavigation: React.FC<SkipNavigationProps> = ({ 
  links = defaultLinks 
}) => {
  const handleSkipClick = (href: string) => {
    const element = document.querySelector(href) as HTMLElement;
    if (element) {
      element.focus();
      element.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <Box
      sx={{
        position: 'fixed',
        top: 0,
        left: 0,
        zIndex: 9999,
        backgroundColor: 'background.paper',
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 1,
        p: 1,
        transform: 'translateY(-100%)',
        transition: 'transform 0.2s ease-in-out',
        '&:focus-within': {
          transform: 'translateY(0)'
        }
      }}
      role="navigation"
      aria-label="Skip navigation"
    >
      {links.map((link, index) => (
        <Button
          key={index}
          variant="outlined"
          size="small"
          onClick={() => handleSkipClick(link.href)}
          onKeyDown={(e) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              handleSkipClick(link.href);
            }
          }}
          sx={{ 
            mr: 1,
            mb: index < links.length - 1 ? 1 : 0,
            '&:focus': {
              outline: '2px solid',
              outlineColor: 'primary.main',
              outlineOffset: '2px'
            }
          }}
        >
          {link.label}
        </Button>
      ))}
    </Box>
  );
};