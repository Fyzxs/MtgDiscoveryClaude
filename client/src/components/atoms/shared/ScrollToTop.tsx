import React, { useState, useEffect } from 'react';
import { Fab, Zoom, SxProps, Theme } from '@mui/material';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';

interface ScrollToTopProps {
  showAfter?: number;
  scrollTarget?: Element | Window | null;
  smooth?: boolean;
  position?: 'bottom-right' | 'bottom-left' | 'bottom-center';
  offset?: { bottom: number; right?: number; left?: number };
  size?: 'small' | 'medium' | 'large';
  color?: 'default' | 'primary' | 'secondary';
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Scroll to top button that appears when user scrolls past a threshold
 * Provides smooth scrolling back to the top of the page or specific element
 */
export const ScrollToTop: React.FC<ScrollToTopProps> = React.memo(({
  showAfter = 300,
  scrollTarget = window,
  smooth = true,
  position = 'bottom-right',
  offset = { bottom: 16, right: 16 },
  size = 'medium',
  color = 'primary',
  sx = {},
  className = ''
}) => {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (!scrollTarget) return;

      const scrollTop = scrollTarget instanceof Window 
        ? scrollTarget.scrollY 
        : scrollTarget.scrollTop;

      setIsVisible(scrollTop > showAfter);
    };

    if (scrollTarget) {
      scrollTarget.addEventListener('scroll', handleScroll);
      // Check initial scroll position
      handleScroll();
    }

    return () => {
      if (scrollTarget) {
        scrollTarget.removeEventListener('scroll', handleScroll);
      }
    };
  }, [scrollTarget, showAfter]);

  const handleClick = () => {
    if (!scrollTarget) return;

    if (scrollTarget instanceof Window) {
      scrollTarget.scrollTo({
        top: 0,
        behavior: smooth ? 'smooth' : 'auto'
      });
    } else {
      scrollTarget.scrollTo({
        top: 0,
        behavior: smooth ? 'smooth' : 'auto'
      });
    }
  };

  const getPositionStyles = () => {
    const baseStyles = {
      position: 'fixed' as const,
      zIndex: 1000,
    };

    switch (position) {
      case 'bottom-left':
        return {
          ...baseStyles,
          bottom: offset.bottom,
          left: offset.left || offset.right || 16,
        };
      case 'bottom-center':
        return {
          ...baseStyles,
          bottom: offset.bottom,
          left: '50%',
          transform: 'translateX(-50%)',
        };
      case 'bottom-right':
      default:
        return {
          ...baseStyles,
          bottom: offset.bottom,
          right: offset.right || 16,
        };
    }
  };

  return (
    <Zoom in={isVisible} timeout={{ enter: 200, exit: 200 }}>
      <Fab
        onClick={handleClick}
        color={color}
        size={size}
        aria-label="Scroll to top"
        className={className}
        sx={{
          ...getPositionStyles(),
          '&:hover': {
            transform: position === 'bottom-center' 
              ? 'translateX(-50%) scale(1.1)' 
              : 'scale(1.1)',
          },
          transition: 'transform 0.2s ease-in-out',
          ...sx,
        }}
      >
        <KeyboardArrowUpIcon />
      </Fab>
    </Zoom>
  );
});

interface ScrollToTopTriggerProps {
  children: (scrollToTop: () => void, isVisible: boolean) => React.ReactNode;
  showAfter?: number;
  scrollTarget?: Element | Window | null;
  smooth?: boolean;
}

/**
 * Render prop version of ScrollToTop for custom implementations
 */
export const ScrollToTopTrigger: React.FC<ScrollToTopTriggerProps> = React.memo(({
  children,
  showAfter = 300,
  scrollTarget = window,
  smooth = true,
}) => {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (!scrollTarget) return;

      const scrollTop = scrollTarget instanceof Window 
        ? scrollTarget.scrollY 
        : scrollTarget.scrollTop;

      setIsVisible(scrollTop > showAfter);
    };

    if (scrollTarget) {
      scrollTarget.addEventListener('scroll', handleScroll);
      handleScroll();
    }

    return () => {
      if (scrollTarget) {
        scrollTarget.removeEventListener('scroll', handleScroll);
      }
    };
  }, [scrollTarget, showAfter]);

  const scrollToTop = () => {
    if (!scrollTarget) return;

    if (scrollTarget instanceof Window) {
      scrollTarget.scrollTo({
        top: 0,
        behavior: smooth ? 'smooth' : 'auto'
      });
    } else {
      scrollTarget.scrollTo({
        top: 0,
        behavior: smooth ? 'smooth' : 'auto'
      });
    }
  };

  return <>{children(scrollToTop, isVisible)}</>;
});

ScrollToTop.displayName = 'ScrollToTop';
ScrollToTopTrigger.displayName = 'ScrollToTopTrigger';