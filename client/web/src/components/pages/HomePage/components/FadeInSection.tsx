import React from 'react';
import { Box } from '../../../atoms';
import { useInView } from '../../../../hooks/useInView';
import { getScrollFadeInStyle } from '../../../../styles/animations';

interface FadeInSectionProps {
  children: React.ReactNode;
  /** Delay in milliseconds before animation starts (default: 0) */
  delay?: number;
}

/**
 * Wrapper component that fades in its children when they enter the viewport.
 * Uses IntersectionObserver for efficient scroll-triggered animations.
 * Respects prefers-reduced-motion accessibility setting.
 *
 * @example
 * <FadeInSection delay={100}>
 *   <MySection />
 * </FadeInSection>
 */
export const FadeInSection: React.FC<FadeInSectionProps> = ({
  children,
  delay = 0,
}) => {
  const { ref, isInView } = useInView(0.1);

  return (
    <Box ref={ref} sx={getScrollFadeInStyle(isInView, delay)}>
      {children}
    </Box>
  );
};
