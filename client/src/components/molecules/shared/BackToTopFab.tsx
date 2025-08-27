import React, { useState, useEffect } from 'react';
import { Fab, Zoom } from '@mui/material';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import type { FabProps } from '@mui/material';

interface BackToTopFabProps {
  /**
   * The scroll threshold (in pixels) before showing the button
   * @default 300
   */
  threshold?: number;
  /**
   * Size of the FAB button
   * @default 'medium'
   */
  size?: FabProps['size'];
  /**
   * Color of the FAB button
   * @default 'primary'
   */
  color?: FabProps['color'];
  /**
   * Position offset from bottom of viewport (in pixels)
   * @default 16
   */
  bottom?: number;
  /**
   * Position offset from right of viewport (in pixels)
   * @default 16
   */
  right?: number;
  /**
   * Z-index for the FAB
   * @default 1000
   */
  zIndex?: number;
  /**
   * Custom icon component
   * @default KeyboardArrowUpIcon
   */
  icon?: React.ReactNode;
  /**
   * Aria label for accessibility
   * @default 'scroll back to top'
   */
  ariaLabel?: string;
  /**
   * Scroll behavior
   * @default 'smooth'
   */
  scrollBehavior?: ScrollBehavior;
}

/**
 * A floating action button that appears when the user scrolls down
 * and scrolls back to top when clicked
 */
export const BackToTopFab: React.FC<BackToTopFabProps> = ({
  threshold = 300,
  size = 'medium',
  color = 'primary',
  bottom = 16,
  right = 16,
  zIndex = 1000,
  icon,
  ariaLabel = 'scroll back to top',
  scrollBehavior = 'smooth'
}) => {
  const [isVisible, setIsVisible] = useState(false);

  // Handle scroll to show/hide button
  useEffect(() => {
    const handleScroll = () => {
      setIsVisible(window.scrollY > threshold);
    };

    // Check initial scroll position
    handleScroll();

    // Add scroll listener
    window.addEventListener('scroll', handleScroll, { passive: true });
    
    // Cleanup
    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, [threshold]);

  const handleClick = () => {
    window.scrollTo({ 
      top: 0, 
      behavior: scrollBehavior 
    });
  };

  return (
    <Zoom in={isVisible}>
      <Fab
        color={color}
        size={size}
        onClick={handleClick}
        sx={{
          position: 'fixed',
          bottom,
          right,
          zIndex
        }}
        aria-label={ariaLabel}
      >
        {icon || <KeyboardArrowUpIcon />}
      </Fab>
    </Zoom>
  );
};