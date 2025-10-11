import React, { useMemo, useCallback } from 'react';
import { useQuery } from '@apollo/client/react';
import { useNavigate } from 'react-router-dom';
import { Typography } from '@mui/material';
import { GET_ALL_SETS } from '../../graphql/queries/sets';
import { MtgSetCard } from '../molecules/Sets/MtgSetCard';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { EmptyState } from '../atoms/shared/EmptyState';
import type { SortOption } from '../atoms/shared/SortDropdown';
import { useUrlFilterState, createUrlFilterConfig } from '../../hooks/useUrlFilterState';
import { GraphQLQueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { FilterPanel } from '../organisms/filters/FilterPanel';
import { ResponsiveGridAutoFit } from '../atoms/layouts/ResponsiveGrid';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { CardGridErrorBoundary } from '../ErrorBoundaries';
import { BrowseTemplate } from '../templates/pages/BrowseTemplate';
import type { MtgSet } from '../../types/set';

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
  const { loading, error, data } = useQuery<SetsResponse>(GET_ALL_SETS);
  
  // Simplified filter state with URL synchronization
  const filterConfig = useMemo(() => 
    createUrlFilterConfig('sets', {
      urlParams: {
        search: 'search',
        sort: 'sort', 
        filters: { setTypes: 'types' }
      }
    }), []);

  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: filteredSets,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useUrlFilterState(
    data?.allSets?.data,
    filterConfig,
    {
      search: '',
      sort: 'release-desc',
      filters: { setTypes: [] }
    }
  );
  
  const selectedSetTypes = filters.setTypes || [];

  // Get unique set types from data
  const getUniqueSetTypes = (sets: MtgSet[]): string[] => {
    const types = new Set(sets.map(set => set.setType));
    return Array.from(types).sort();
  };

  // URL synchronization is handled automatically by useUrlFilterState

  const navigate = useNavigate();
  
  const handleSetClick = (setCode?: string) => {
    if (setCode) {
      navigate(`/set/${setCode}`);
    }
  };

  const handleSetTypeChange = useCallback((value: string[]) => {
    updateFilter('setTypes', value);
  }, [updateFilter]);

  const handleSortChange = useCallback((value: string) => {
    setSortBy(value);
  }, [setSortBy]);

  const sortOptions: SortOption[] = [
    { value: 'release-desc', label: 'Release Date (Newest)' },
    { value: 'release-asc', label: 'Release Date (Oldest)' },
    { value: 'name-asc', label: 'Name (A-Z)' },
    { value: 'name-desc', label: 'Name (Z-A)' },
    { value: 'cards-desc', label: 'Card Count (High-Low)' },
    { value: 'cards-asc', label: 'Card Count (Low-High)' }
  ];

  const sets = data?.allSets?.data || EMPTY_SETS_ARRAY;
  const setTypes = getUniqueSetTypes(sets);

  return (
    <GraphQLQueryStateContainer
      loading={loading}
      error={error}
      data={data?.allSets}
      failureTypeName="FailureResponse"
    >
      <BrowseTemplate
        maxWidth={false}
        header={
          <Typography variant="h3" component="h1" gutterBottom sx={{ textAlign: 'center' }}>
            All Sets
          </Typography>
        }
        filters={
          <FilterPanel
            config={{
              search: {
                value: searchTerm,
                onChange: setSearchTerm,
                placeholder: 'Search sets...',
                debounceMs: 300
              },
              multiSelects: [
                {
                  key: 'setTypes',
                  value: selectedSetTypes,
                  onChange: handleSetTypeChange,
                  options: setTypes,
                  label: 'Set Types',
                  placeholder: 'All Types'
                }
              ],
              sort: {
                value: sortBy,
                onChange: handleSortChange,
                options: sortOptions
              }
            }}
            layout="horizontal"
          />
        }
        summary={
          <ResultsSummary
            current={filteredSets.length}
            total={sets.length}
            label="sets"
            textAlign="center"
          />
        }
        content={
          <>
            <CardGridErrorBoundary name="AllSetsGrid">
              <ResponsiveGridAutoFit minItemWidth={240} spacing={1.5}>
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

      {/* Back to Top Button - positioned fixed outside template */}
      <BackToTopFab />
    </GraphQLQueryStateContainer>
  );
};