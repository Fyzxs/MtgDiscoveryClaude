import React, { useMemo, useCallback } from 'react';
import { useQuery } from '@apollo/client/react';
import { useNavigate } from 'react-router-dom';
import { 
  Container, 
  Typography
} from '@mui/material';
import { GET_ALL_SETS } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { EmptyState } from '../components/atoms/shared/EmptyState';
import type { SortOption } from '../components/atoms/shared/SortDropdown';
import { useUrlFilterState, createUrlFilterConfig } from '../hooks/useUrlFilterState';
import { GraphQLQueryStateContainer } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { ResponsiveGrid } from '../components/atoms/layouts/ResponsiveGrid';
import { BackToTopFab } from '../components/molecules/shared/BackToTopFab';
import { CardGridErrorBoundary } from '../components/ErrorBoundaries';
import type { MtgSet } from '../types/set';

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

  const sets = data?.allSets?.data || [];
  const setTypes = getUniqueSetTypes(sets);

  return (
    <GraphQLQueryStateContainer
      loading={loading}
      error={error}
      data={data?.allSets}
      failureTypeName="FailureResponse"
    >
    <Container maxWidth="xl" sx={{ mt: 2, mb: 4, px: 3, mx: 'auto' }}>
      <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
        All Magic Sets
      </Typography>

      {/* Filters and Search */}
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

      {/* Results Summary */}
      <ResultsSummary 
        current={filteredSets.length} 
        total={sets.length} 
        label="sets"
        textAlign="center"
      />

      {/* Sets Grid */}
      <CardGridErrorBoundary name="AllSetsGrid">
        <ResponsiveGrid minItemWidth={240} spacing={1.5}>
          {filteredSets.map((set) => (
          <MtgSetCard
            key={set.id}
            set={set}
            onSetClick={handleSetClick}
          />
          ))}
        </ResponsiveGrid>
      </CardGridErrorBoundary>

      {filteredSets.length === 0 && (
        <EmptyState
          message="No sets found matching your criteria"
          description="Try adjusting your filters or search terms"
        />
      )}

      {/* Back to Top Button */}
      <BackToTopFab />
    </Container>
    </GraphQLQueryStateContainer>
  );
};