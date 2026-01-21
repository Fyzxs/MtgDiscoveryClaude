import React from 'react';
import { Box, type BoxProps } from '../../atoms';

interface SectionProps extends BoxProps {
  /** Semantic section name for aria-label */
  label?: string;
  /** Whether to use <section> tag (default: true) */
  asSection?: boolean;
}

/**
 * Section - Semantic page section molecule
 *
 * Wraps MUI Box with semantic HTML section tag and proper ARIA labels.
 * Should be used by templates and pages for structural layout instead of Box atom.
 *
 * @example
 * <Section label="Filters and search" sx={{ mb: 3 }}>
 *   <FilterPanel />
 * </Section>
 *
 * @example
 * // Non-semantic Box for layout only
 * <Section asSection={false} sx={{ display: 'flex', gap: 2 }}>
 *   <Sidebar />
 *   <MainContent />
 * </Section>
 */
export const Section: React.FC<SectionProps> = ({
  children,
  label,
  asSection = true,
  component,
  sx = {},
  ...props
}) => {
  return (
    <Box
      component={component || (asSection ? 'section' : 'div')}
      aria-label={label}
      sx={sx}
      {...props}
    >
      {children}
    </Box>
  );
};
