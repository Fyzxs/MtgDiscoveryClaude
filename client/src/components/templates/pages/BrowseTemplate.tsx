import React from 'react';
import { type SxProps, type Theme } from '../../atoms';
import { PageContainer, Section } from '../../molecules/layouts';

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
    <PageContainer
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
        <Section
          component="header"
          asSection={false}
          sx={{
            mb: hasFilters ? 4 : 3,
            display: 'flex',
            justifyContent: 'center'
          }}
        >
          {header}
        </Section>
      )}

      {/* Main Layout Container */}
      <Section asSection={false} sx={{
        display: hasSidebar ? 'flex' : 'block',
        gap: hasSidebar ? 4 : 0,
        alignItems: 'flex-start'
      }}>

        {/* Sidebar (if present) */}
        {hasSidebar && (
          <Section
            component="aside"
            asSection={false}
            sx={{
              flexShrink: 0,
              width: { xs: '100%', md: 280 },
              order: { xs: 2, md: 1 },
              mt: { xs: 4, md: 0 }
            }}
          >
            {sidebar}
          </Section>
        )}

        {/* Main Content Area */}
        <Section asSection={false} sx={{
          flex: hasSidebar ? 1 : 'none',
          order: { xs: 1, md: 2 },
          width: hasSidebar ? 'auto' : '100%'
        }}>

          {/* Filters Section */}
          {hasFilters && (
            <Section
              label="Filters and search"
              sx={{
                mb: hasSummary ? 3 : 4,
                display: 'flex',
                justifyContent: 'center'
              }}
            >
              {filters}
            </Section>
          )}

          {/* Results Summary */}
          {hasSummary && (
            <Section
              label="Results summary"
              sx={{ mb: 3 }}
            >
              {summary}
            </Section>
          )}

          {/* Main Content */}
          <Section
            label="Browse results"
            sx={{ mb: hasPagination ? 4 : 0 }}
          >
            {content}
          </Section>

          {/* Pagination */}
          {hasPagination && (
            <Section
              component="nav"
              label="Pagination"
              asSection={false}
              sx={{
                mt: 4,
                display: 'flex',
                justifyContent: 'center'
              }}
            >
              {pagination}
            </Section>
          )}
        </Section>
      </Section>
    </PageContainer>
  );
};