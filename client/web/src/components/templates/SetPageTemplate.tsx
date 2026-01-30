import React from 'react';
import { PageContainer } from '../molecules/layouts';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import type { FilterPanelConfig } from '../../types/filters';

interface SetPageTemplateProps {
  // Query state
  isLoading: boolean;
  error?: Error | null;

  // Content sections
  header: React.ReactNode;
  filters?: React.ReactNode;
  cardDisplay: React.ReactNode;

  // Results summary
  currentCount: number;
  totalCount: number;

  // Mobile layout props
  useMobileLayout?: boolean;
  filterDrawerOpen?: boolean;
  onFilterDrawerToggle?: () => void;
  filterConfig?: FilterPanelConfig;
  activeFilterCount?: number;
  onClearFilters?: () => void;

  // Groups toggle (shown in mobile bar)
  showGroupsToggle?: boolean;
  showGroups?: boolean;
  onShowGroupsChange?: (value: boolean) => void;

  children?: never; // Prevent accidental children
}

export const SetPageTemplate: React.FC<SetPageTemplateProps> = ({
  isLoading,
  error,
  header,
  filters,
  cardDisplay,
  currentCount,
  totalCount,
  useMobileLayout = false,
  filterDrawerOpen = false,
  onFilterDrawerToggle,
  filterConfig,
  activeFilterCount = 0,
  onClearFilters,
  showGroupsToggle = false,
  showGroups = true,
  onShowGroupsChange
}) => {
  // Build results summary text for mobile bar
  const resultsSummary = `${currentCount} of ${totalCount} cards`;

  return (
    <QueryStateContainer
      loading={isLoading}
      error={error}
      containerProps={{ maxWidth: false }}
    >
      <PageContainer maxWidth={false} sx={{ mt: { xs: 1, sm: 2 }, mb: 4, px: { xs: 1, sm: 2, md: 3 } }}>
        {/* Header Section */}
        {header}

        {/* Mobile Filter Bar (sticky) */}
        {useMobileLayout && onFilterDrawerToggle && onShowGroupsChange && (
          <MobileFilterBar
            activeFilterCount={activeFilterCount}
            onFilterClick={onFilterDrawerToggle}
            showGroups={showGroups}
            onShowGroupsChange={onShowGroupsChange}
            showGroupsToggle={showGroupsToggle}
            resultsSummary={resultsSummary}
          />
        )}

        {/* Desktop Filters Section (inline) */}
        {useMobileLayout === false && filters}

        {/* Card Display Section */}
        {cardDisplay}

        {/* Back to Top Button */}
        <BackToTopFab />
      </PageContainer>

      {/* Mobile Filter Drawer */}
      {useMobileLayout && filterConfig && onFilterDrawerToggle && (
        <FilterDrawer
          open={filterDrawerOpen}
          onClose={onFilterDrawerToggle}
          config={filterConfig}
          title="Filter Cards"
          activeFilterCount={activeFilterCount}
          onClear={onClearFilters}
        />
      )}
    </QueryStateContainer>
  );
};