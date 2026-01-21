import React, { useMemo, useCallback, startTransition, useTransition, useState } from 'react';
import { useQuery } from '@apollo/client/react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Box } from '../atoms';
import { Heading } from '../molecules/text';
import { GET_ALL_SETS } from '../../graphql/queries/sets';
import { MtgSetCard } from '../molecules/Sets/MtgSetCard';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { EmptyState } from '../molecules/shared/EmptyState';
import { CollectionStatsSummary } from '../molecules/shared/CollectionStatsSummary';
import { CollapsibleStatsSummary } from '../molecules/shared/CollapsibleStatsSummary';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { FilterControlsWithLoading } from '../molecules/shared/FilterControlsWithLoading';
import type { SortOption } from '../molecules/shared/SortDropdown';
import { useUrlFilterState, createUrlFilterConfig } from '../../hooks/useUrlFilterState';
import { useMinimumLoadingTime } from '../../hooks/useMinimumLoadingTime';
import { GraphQLQueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { FilterPanel } from '../organisms/filters/FilterPanel';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import { ResponsiveGridAutoFit } from '../molecules/layouts/ResponsiveGrid';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { CardGridErrorBoundary } from '../utils/ErrorBoundaries';
import { BrowseTemplate } from '../templates/pages/BrowseTemplate';
import type { MtgSet } from '../../types/set';
import type { FilterPanelConfig } from '../../types/filters';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';

interface SetsResponse {
  allSets: {
    __typename: string;
    data?: MtgSet[];
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

// Stable empty array to prevent infinite re-renders
const EMPTY_SETS_ARRAY: MtgSet[] = [];

export const AllSetsPage: React.FC = () => {
  const { hasCollector, collectorId } = useCollectorParam();
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  // Use compact/drawer UI for both mobile and tablet (xs + sm breakpoints)
  const useCompactUI = isMobile || isTablet;
  const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);
  const { loading, error, data } = useQuery<SetsResponse>(GET_ALL_SETS, {
    variables: {
      args: hasCollector && collectorId ? { userId: collectorId } : {}
    }
  });

  // Loading state with minimum display time
  const { isLoading: isFiltering, showLoading, hideLoading } = useMinimumLoadingTime(400);
  const [isPending, startFilterTransition] = useTransition();

  // Simplified filter state with URL synchronization
  const filterConfig = useMemo(() =>
    createUrlFilterConfig('sets', {
      urlParams: {
        search: 'search',
        sort: 'sort',
        filters: {
          setTypes: 'types',
          collectionStatus: 'status'
        }
      }
    }), []);

  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: unsortedSets,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useUrlFilterState(
    data?.allSets?.data,
    filterConfig,
    {
      search: '',
      sort: 'release-desc',
      filters: {
        setTypes: [],
        collectionStatus: []
      }
    }
  );

  const selectedSetTypes = (Array.isArray(filters.setTypes) ? filters.setTypes : []) as string[];
  const selectedCollectionStatuses = (Array.isArray(filters.collectionStatus) ? filters.collectionStatus : []) as string[];

  // Apply sorting to filtered sets
  const filteredSets = useMemo(() => {
    if (!unsortedSets || unsortedSets.length === 0) return [];
    const sortFn = filterConfig.sortOptions?.[sortBy as keyof typeof filterConfig.sortOptions];
    if (!sortFn) return unsortedSets;
    return [...unsortedSets].sort(sortFn);
  }, [unsortedSets, sortBy, filterConfig.sortOptions]);

  // Get unique set types from data
  const getUniqueSetTypes = (sets: MtgSet[]): string[] => {
    const types = new Set(sets.map(set => set.setType));
    return Array.from(types).sort();
  };

  // URL synchronization is handled automatically by useUrlFilterState

  const navigate = useNavigate();
  const location = useLocation();

  const handleSetClick = (setCode?: string) => {
    if (setCode) {
      // Preserve existing query parameters (like ctor) when navigating
      navigate(`/set/${setCode}${location.search}`);
    }
  };

  const handleSetTypeChange = useCallback((value: string[]) => {
    showLoading();
    startFilterTransition(() => {
      updateFilter('setTypes', value);
      hideLoading();
    });
  }, [updateFilter, showLoading, hideLoading]);

  const handleCollectionStatusChange = useCallback((value: string[]) => {
    showLoading();
    startFilterTransition(() => {
      updateFilter('collectionStatus', value);
      hideLoading();
    });
  }, [updateFilter, showLoading, hideLoading]);

  const handleSortChange = useCallback((value: string) => {
    showLoading();
    startFilterTransition(() => {
      setSortBy(value);
      hideLoading();
    });
  }, [setSortBy, showLoading, hideLoading]);

  const sortOptions: SortOption[] = useMemo(() => {
    const baseSortOptions: SortOption[] = [
      { value: 'release-desc', label: 'Release Date (Newest)' },
      { value: 'release-asc', label: 'Release Date (Oldest)' },
      { value: 'name-asc', label: 'Name (A-Z)' },
      { value: 'name-desc', label: 'Name (Z-A)' },
      { value: 'cards-desc', label: 'Card Count (High-Low)' },
      { value: 'cards-asc', label: 'Card Count (Low-High)' }
    ];

    if (hasCollector) {
      baseSortOptions.push(
        { value: 'completion-desc', label: 'Percent Collected', isCollectorOption: true },
        { value: 'collected-desc', label: 'Cards Collected', isCollectorOption: true }
      );
    }

    return baseSortOptions;
  }, [hasCollector]);

  const sets = data?.allSets?.data || EMPTY_SETS_ARRAY;
  const setTypes = getUniqueSetTypes(sets);

  // Calculate active filter count for mobile drawer badge
  const activeFilterCount = useMemo(() => {
    let count = 0;
    if (searchTerm) count++;
    if (selectedSetTypes.length > 0) count++;
    if (selectedCollectionStatuses.length > 0) count++;
    return count;
  }, [searchTerm, selectedSetTypes, selectedCollectionStatuses]);

  // Build filter config for both desktop and mobile drawer
  const filterPanelConfig: FilterPanelConfig = useMemo(() => ({
    search: {
      value: searchTerm,
      onChange: (value: string) => {
        startTransition(() => {
          setSearchTerm(value);
        });
      },
      placeholder: 'Search sets...',
      debounceMs: 300,
      fullWidth: useCompactUI
    },
    multiSelects: [
      {
        key: 'setTypes',
        value: selectedSetTypes,
        onChange: handleSetTypeChange,
        options: setTypes,
        label: 'Set Types',
        placeholder: 'All Types',
        fullWidth: useCompactUI
      }
    ],
    sort: {
      value: sortBy,
      onChange: handleSortChange,
      options: sortOptions,
      fullWidth: useCompactUI
    },
    collectorFilters: hasCollector ? {
      collectionStatus: {
        key: 'collectionStatus',
        value: selectedCollectionStatuses,
        onChange: handleCollectionStatusChange,
        options: ['not-started', 'in-progress', 'completed'],
        label: 'Collection Status',
        placeholder: 'All Statuses',
        minWidth: useCompactUI ? undefined : 180,
        fullWidth: useCompactUI
      }
    } : undefined
  }), [searchTerm, setSearchTerm, selectedSetTypes, handleSetTypeChange, setTypes, sortBy, handleSortChange, sortOptions, hasCollector, selectedCollectionStatuses, handleCollectionStatusChange, useCompactUI]);

  // Toggle filter drawer
  const handleFilterDrawerToggle = useCallback(() => {
    setFilterDrawerOpen(prev => !prev);
  }, []);

  // Clear all filters
  const handleClearFilters = useCallback(() => {
    setSearchTerm('');
    updateFilter('setTypes', []);
    updateFilter('collectionStatus', []);
  }, [setSearchTerm, updateFilter]);

  // Results summary text for mobile bar
  const resultsSummary = `${filteredSets.length} of ${sets.length} sets`;

  return (
    <GraphQLQueryStateContainer
      loading={loading}
      error={error}
      data={data?.allSets}
      failureTypeName="FailureResponse"
    >
      {/* Mobile/Tablet: Sticky filter bar at top */}
      {useCompactUI && (
        <MobileFilterBar
          activeFilterCount={activeFilterCount}
          onFilterClick={handleFilterDrawerToggle}
          showGroups={false}
          onShowGroupsChange={() => {}}
          showGroupsToggle={false}
          resultsSummary={resultsSummary}
        />
      )}

      <BrowseTemplate
        maxWidth={false}
        header={
          <Box sx={{ width: '100%' }}>
            {/* Hide title on mobile/tablet - it's obvious from context */}
            <Heading
              variant="h3"
              component="h1"
              gutterBottom
              sx={{ textAlign: 'center', display: { xs: 'none', md: 'block' } }}
            >
              All Sets
            </Heading>
            {hasCollector && (
              useCompactUI ? (
                <CollapsibleStatsSummary sets={sets} />
              ) : (
                <CollectionStatsSummary sets={sets} />
              )
            )}
          </Box>
        }
        filters={
          useCompactUI ? (
            // Mobile/Tablet: No inline filters - using sticky bar + drawer
            null
          ) : (
            // Desktop: Full FilterPanel inline
            <FilterControlsWithLoading isLoading={isFiltering || isPending}>
              <FilterPanel
                config={filterPanelConfig}
                layout="horizontal"
              />
            </FilterControlsWithLoading>
          )
        }
        summary={
          // Only show summary on desktop (mobile has it in the sticky bar)
          useCompactUI ? null : (
            <ResultsSummary
              current={filteredSets.length}
              total={sets.length}
              label="sets"
              textAlign="center"
            />
          )
        }
        content={
          <>
            <CardGridErrorBoundary name="AllSetsGrid">
              <ResponsiveGridAutoFit
                minItemWidth={{
                  xs: 140,  // sm card size (140x115)
                  sm: 160,  // md card size (160x130)
                  md: 200,  // lg card size (200x260)
                  lg: 200,  // lg card size
                  xl: 200,  // lg card size
                }}
                maxItemWidth={{
                  xs: 140,
                  sm: 160,
                  md: 220,
                  lg: 220,
                  xl: 220,
                }}
                spacing={{ xs: 1, sm: 1.5, md: 2 }}
              >
                {filteredSets.map((set) => (
                  <MtgSetCard
                    key={set.id}
                    set={set}
                    onSetClick={handleSetClick}
                  />
                ))}
              </ResponsiveGridAutoFit>
            </CardGridErrorBoundary>

            {filteredSets.length === 0 && (
              <EmptyState
                message="No sets found matching your criteria"
                description="Try adjusting your filters or search terms"
              />
            )}
          </>
        }
      />

      {/* Mobile/Tablet: Bottom sheet filter drawer */}
      {useCompactUI && (
        <FilterDrawer
          open={filterDrawerOpen}
          onClose={handleFilterDrawerToggle}
          config={filterPanelConfig}
          title="Filter Sets"
          activeFilterCount={activeFilterCount}
          onClear={handleClearFilters}
        />
      )}

      {/* Back to Top Button - positioned fixed outside template */}
      <BackToTopFab />
    </GraphQLQueryStateContainer>
  );
};

export default AllSetsPage;
