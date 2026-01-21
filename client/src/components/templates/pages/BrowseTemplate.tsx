import React from 'react';
import { Container, Box, type SxProps, type Theme } from '../../atoms';

interface BrowseTemplateProps {
  /** Page header content (title, hero section, etc.) */
  header?: React.ReactNode;

  /** Filter panel for search and filtering options */
  filters?: React.ReactNode;

  /** Results summary showing counts and pagination info */
  summary?: React.ReactNode;

  /** Main content area for displaying items (grid, list, etc.) */
  content: React.ReactNode;

  /** Pagination controls (optional) */
  pagination?: React.ReactNode;

  /** Optional sidebar content */
  sidebar?: React.ReactNode;

  /** Container max width (defaults to false for full width) */
  maxWidth?: false | 'xs' | 'sm' | 'md' | 'lg' | 'xl';

  /** Container padding overrides */
  containerPadding?: {
    mt?: number;
    mb?: number;
    px?: number;
  };

  /** Additional container styles */
  containerSx?: SxProps<Theme>;
}

/**
 * BrowseTemplate - A reusable template for browse/listing pages
 *
 * Provides a consistent layout structure for pages that display collections
 * of items (cards, sets, artists, etc.) with filtering and pagination.
 *
 * @example
 * <BrowseTemplate
 *   header={<Typography variant="h3">All Sets</Typography>}
 *   filters={<FilterPanel config={filterConfig} />}
 *   summary={<ResultsSummary current={10} total={617} label="sets" />}
 *   content={<ResponsiveGrid>{setCards}</ResponsiveGrid>}
 *   pagination={<Pagination count={10} />}
 * />
 */
export const BrowseTemplate: React.FC<BrowseTemplateProps> = ({
  header,
  filters,
  summary,
  content,
  pagination,
  sidebar,
  maxWidth = false,
  containerPadding = { mt: 2, mb: 4, px: 3 },
  containerSx = {}
}) => {
  const hasHeader = Boolean(header);
  const hasFilters = Boolean(filters);
  const hasSummary = Boolean(summary);
  const hasPagination = Boolean(pagination);
  const hasSidebar = Boolean(sidebar);

  return (
    <Container
      maxWidth={maxWidth}
      sx={{
        mt: containerPadding.mt,
        mb: containerPadding.mb,
        px: containerPadding.px,
        mx: 'auto',
        ...containerSx
      }}
    >
      {/* Page Header Section */}
      {hasHeader && (
        <Box
          component="header"
          sx={{
            mb: hasFilters ? 4 : 3,
            display: 'flex',
            justifyContent: 'center'
          }}
        >
          {header}
        </Box>
      )}

      {/* Main Layout Container */}
      <Box sx={{
        display: hasSidebar ? 'flex' : 'block',
        gap: hasSidebar ? 4 : 0,
        alignItems: 'flex-start'
      }}>

        {/* Sidebar (if present) */}
        {hasSidebar && (
          <Box
            component="aside"
            sx={{
              flexShrink: 0,
              width: { xs: '100%', md: 280 },
              order: { xs: 2, md: 1 },
              mt: { xs: 4, md: 0 }
            }}
          >
            {sidebar}
          </Box>
        )}

        {/* Main Content Area */}
        <Box sx={{
          flex: hasSidebar ? 1 : 'none',
          order: { xs: 1, md: 2 },
          width: hasSidebar ? 'auto' : '100%'
        }}>

          {/* Filters Section */}
          {hasFilters && (
            <Box
              component="section"
              aria-label="Filters and search"
              sx={{
                mb: hasSummary ? 3 : 4,
                display: 'flex',
                justifyContent: 'center'
              }}
            >
              {filters}
            </Box>
          )}

          {/* Results Summary */}
          {hasSummary && (
            <Box
              component="section"
              aria-label="Results summary"
              sx={{ mb: 3 }}
            >
              {summary}
            </Box>
          )}

          {/* Main Content */}
          <Box
            component="section"
            aria-label="Browse results"
            sx={{ mb: hasPagination ? 4 : 0 }}
          >
            {content}
          </Box>

          {/* Pagination */}
          {hasPagination && (
            <Box
              component="nav"
              aria-label="Pagination"
              sx={{
                mt: 4,
                display: 'flex',
                justifyContent: 'center'
              }}
            >
              {pagination}
            </Box>
          )}
        </Box>
      </Box>
    </Container>
  );
};