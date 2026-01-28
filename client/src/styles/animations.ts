import { keyframes, type SxProps, type Theme } from '@mui/material';

/**
 * Shared animation keyframes for fade-in-up effect
 */
export const fadeInUpKeyframes = keyframes`
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
`;

/**
 * Creates sx props for fade-in-up animation with configurable delay
 * Respects prefers-reduced-motion accessibility setting
 *
 * @param delayMs - Delay in milliseconds before animation starts
 * @returns SxProps object with animation and reduced-motion support
 */
export const getFadeInUpStyle = (delayMs: number): SxProps<Theme> => ({
  opacity: 0,
  animation: `${fadeInUpKeyframes} 0.6s ease-out ${delayMs}ms forwards`,
  '@media (prefers-reduced-motion: reduce)': {
    animation: 'none',
    opacity: 1,
  },
});

/**
 * Creates sx props for scroll-triggered fade-in effect
 * Used with IntersectionObserver-based visibility detection
 *
 * @param isInView - Whether the element is in the viewport
 * @param delayMs - Optional delay in milliseconds (default: 0)
 * @returns SxProps object with transition-based animation
 */
export const getScrollFadeInStyle = (isInView: boolean, delayMs = 0): SxProps<Theme> => ({
  opacity: isInView ? 1 : 0,
  transform: isInView ? 'translateY(0)' : 'translateY(20px)',
  transition: `opacity 0.6s ease-out ${delayMs}ms, transform 0.6s ease-out ${delayMs}ms`,
  '@media (prefers-reduced-motion: reduce)': {
    transition: 'none',
    opacity: 1,
    transform: 'none',
  },
});
