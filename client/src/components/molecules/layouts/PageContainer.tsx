import React from 'react';
import { Container, type ContainerProps } from '../../atoms';

/**
 * PageContainer - Layout molecule for page-level container
 *
 * Wraps MUI Container with page-appropriate defaults.
 * Should be used by templates and pages instead of importing Container atom directly.
 *
 * @example
 * <PageContainer maxWidth="lg">
 *   <PageContent />
 * </PageContainer>
 *
 * @example
 * // Full-width container
 * <PageContainer maxWidth={false} sx={{ px: 3 }}>
 *   <SetGrid />
 * </PageContainer>
 */
export const PageContainer: React.FC<ContainerProps> = ({
  children,
  maxWidth = 'lg',
  sx = {},
  ...props
}) => {
  return (
    <Container
      maxWidth={maxWidth}
      sx={{
        mx: 'auto',
        ...sx
      }}
      {...props}
    >
      {children}
    </Container>
  );
};
