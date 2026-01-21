import React from 'react';
import { Container, Box, type SxProps, type Theme } from '../../atoms';

interface SearchTemplateProps {
  /** Search input component - the primary interaction */
  searchInput: React.ReactNode;

  /** Advanced filters section (collapsible or secondary) */
  advancedFilters?: React.ReactNode;

  /** Search results summary (count, search terms, etc.) */
  resultsSummary?: React.ReactNode;

  /** Quick filters/facets for refining search */
  quickFilters?: React.ReactNode;

  /** Main search results content OR empty state */
  resultsContent: React.ReactNode;

  /** Loading state component when search is in progress */
  loadingState?: React.ReactNode;

  /** Empty state component when no results found */
  emptyState?: React.ReactNode;

  /** Pagination controls (optional) */
  pagination?: React.ReactNode;

  /** Container max width (defaults to 'md' for search focus) */
  maxWidth?: false | 'xs' | 'sm' | 'md' | 'lg' | 'xl';

  /** Container padding overrides */
  containerPadding?: {
    mt?: number;
    mb?: number;
    px?: number;
  };

  /** Additional container styles */
  containerSx?: SxProps<Theme>;

  /** Whether to show loading state (controls which content is displayed) */
  isLoading?: boolean;

  /** Whether to show empty state (controls which content is displayed) */
  isEmpty?: boolean;

  /** Whether to show search instructions/initial state */
  showInitialState?: boolean;
}

/**
 * SearchTemplate - A reusable template for search result pages
 *
 * Provides a consistent layout structure for search interfaces with prominent
 * search input, optional filters, results display, and empty states.
 *
 * Key differences from BrowseTemplate:
 * - Search input is primary and prominent at top
 * - Advanced filters are optional and can be collapsible
 * - Handles empty states and loading states specifically for search
 * - Results summary includes search context
 * - Quick filters for search refinement
 *
 * Layout Structure:
 * [Search Input - Prominent]
 * [Advanced Filters - Optional/Collapsible]
 * [Results Summary - With search context]
 * [Quick Filters - Optional facets]
 * [Results Content OR Loading OR Empty State]
 * [Pagination - Optional]
 *
 * @example
 * <SearchTemplate
 *   searchInput={
 *     <TextField
 *       placeholder="Search cards..."
 *       value={searchTerm}
 *       onChange={handleSearch}
 *       InputProps={{
 *         startAdornment: <SearchIcon />,
 *         endAdornment: loading && <CircularProgress size={20} />
 *       }}
 *     />
 *   }
 *   resultsSummary={
 *     <Typography>Found {results.length} cards matching "{searchTerm}"</Typography>
 *   }
 *   resultsContent={
 *     <ResponsiveGrid>
 *       {results.map(card => <CardDisplay key={card.id} card={card} />)}
 *     </ResponsiveGrid>
 *   }
 *   emptyState={
 *     <Typography>No cards found matching "{searchTerm}"</Typography>
 *   }
 *   isLoading={loading}
 *   isEmpty={results.length === 0}
 * />
 */
export const SearchTemplate: React.FC<SearchTemplateProps> = ({
  searchInput,
  advancedFilters,
  resultsSummary,
  quickFilters,
  resultsContent,
  loadingState,
  emptyState,
  pagination,
  maxWidth = 'md',
  containerPadding = { mt: 2, mb: 4, px: 3 },
  containerSx = {},
  isLoading = false,
  isEmpty = false,
  showInitialState = false
}) => {
  const hasAdvancedFilters = Boolean(advancedFilters);
  const hasResultsSummary = Boolean(resultsSummary);
  const hasQuickFilters = Boolean(quickFilters);
  const hasPagination = Boolean(pagination);

  // Determine which content to show based on state
  const shouldShowLoading = isLoading && loadingState;
  const shouldShowEmpty = !isLoading && isEmpty && emptyState;
  const shouldShowResults = !isLoading && !isEmpty && !showInitialState;
  const shouldShowInitialContent = showInitialState && !isLoading;

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
      {/* Search Input - Primary and Prominent */}
      <Box
        component="section"
        aria-label="Search input"
        sx={{
          mb: hasAdvancedFilters ? 3 : 4,
          display: 'flex',
          justifyContent: 'center'
        }}
      >
        {searchInput}
      </Box>

      {/* Advanced Filters - Optional/Collapsible */}
      {hasAdvancedFilters && (
        <Box
          component="section"
          aria-label="Advanced search filters"
          sx={{
            mb: hasResultsSummary || hasQuickFilters ? 3 : 4,
            display: 'flex',
            justifyContent: 'center'
          }}
        >
          {advancedFilters}
        </Box>
      )}

      {/* Results Summary - Search Context */}
      {hasResultsSummary && (shouldShowResults || shouldShowEmpty) && (
        <Box
          component="section"
          aria-label="Search results summary"
          sx={{
            mb: hasQuickFilters ? 2 : 3,
            display: 'flex',
            justifyContent: 'flex-start'
          }}
        >
          {resultsSummary}
        </Box>
      )}

      {/* Quick Filters/Facets - Search Refinement */}
      {hasQuickFilters && shouldShowResults && (
        <Box
          component="section"
          aria-label="Search filters"
          sx={{
            mb: 3,
            display: 'flex',
            justifyContent: 'center'
          }}
        >
          {quickFilters}
        </Box>
      )}

      {/* Main Content Area - Results, Loading, or Empty State */}
      <Box
        component="section"
        aria-label="Search results"
        sx={{
          mb: hasPagination ? 4 : 0,
          minHeight: 200, // Prevent layout shift during loading
          display: 'flex',
          flexDirection: 'column'
        }}
      >
        {/* Loading State */}
        {shouldShowLoading && (
          <Box sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            flex: 1,
            py: 4
          }}>
            {loadingState}
          </Box>
        )}

        {/* Empty State */}
        {shouldShowEmpty && (
          <Box sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            flex: 1,
            py: 4
          }}>
            {emptyState}
          </Box>
        )}

        {/* Search Results */}
        {shouldShowResults && resultsContent}

        {/* Initial State Content */}
        {shouldShowInitialContent && (
          <Box sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            flex: 1,
            py: 4
          }}>
            {resultsContent}
          </Box>
        )}
      </Box>

      {/* Pagination - Bottom Navigation */}
      {hasPagination && shouldShowResults && (
        <Box
          component="nav"
          aria-label="Search results pagination"
          sx={{
            mt: 4,
            display: 'flex',
            justifyContent: 'center'
          }}
        >
          {pagination}
        </Box>
      )}
    </Container>
  );
};