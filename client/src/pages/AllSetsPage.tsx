import React, { useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import { 
  Container, 
  Typography, 
  Grid,
  Box
} from '@mui/material';
import { GET_ALL_SETS } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { ResultsSummary } from '../components/atoms/shared/ResultsSummary';
import { EmptyState } from '../components/atoms/shared/EmptyState';
import { SortDropdown } from '../components/atoms/shared/SortDropdown';
import type { SortOption } from '../components/atoms/shared/SortDropdown';
import { MultiSelectDropdown } from '../components/atoms/shared/MultiSelectDropdown';
import { DebouncedSearchInput } from '../components/atoms/shared/DebouncedSearchInput';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState, commonFilters, commonSorts } from '../hooks/useFilterState';
import { GraphQLQueryStateContainer } from '../components/molecules/shared/QueryStateContainer';
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
  // URL state configuration (don't manage 'page' here as it's handled by routing)
  const urlStateConfig = {
    search: { default: '' },
    types: { default: [] },
    sort: { default: 'release-desc' }
  };

  // Get initial values from URL
  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const initialValues = getInitialValues();

  const { loading, error, data } = useQuery<SetsResponse>(GET_ALL_SETS);
  
  // Configure filter state (memoized to prevent recreating on every render)
  const filterConfig = useMemo(() => ({
    searchFields: ['name', 'code'] as (keyof MtgSet)[],
    sortOptions: {
      'release-desc': (a: MtgSet, b: MtgSet) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime(),
      'release-asc': (a: MtgSet, b: MtgSet) => new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime(),
      'name-asc': commonSorts.alphabetical<MtgSet>('name', true),
      'name-desc': commonSorts.alphabetical<MtgSet>('name', false),
      'cards-desc': commonSorts.numeric<MtgSet>('cardCount', false),
      'cards-asc': commonSorts.numeric<MtgSet>('cardCount', true)
    },
    filterFunctions: {
      setTypes: commonFilters.multiSelect<MtgSet>('setType')
    },
    defaultSort: 'release-desc'
  }), []);

  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: filteredSets,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useFilterState(
    data?.allSets?.data,
    filterConfig,
    {
      search: initialValues.search || '',
      sort: initialValues.sort || 'release-desc',
      filters: {
        setTypes: initialValues.types || []
      }
    }
  );
  
  const selectedSetTypes = filters.setTypes || [];

  // Get unique set types from data
  const getUniqueSetTypes = (sets: MtgSet[]): string[] => {
    const types = new Set(sets.map(set => set.setType));
    return Array.from(types).sort();
  };

  // Sync state with URL (don't manage 'page' here)
  useUrlState(
    {
      search: searchTerm,
      types: selectedSetTypes,
      sort: sortBy
    },
    {
      search: { default: '' },
      types: { default: [] },
      sort: { default: 'release-desc' }
    }
  );

  const handleSetClick = (setCode?: string) => {
    if (setCode) {
      window.location.href = `?page=set&set=${setCode}`;
    }
  };

  const handleSetTypeChange = (value: string[]) => {
    updateFilter('setTypes', value);
  };

  const handleSortChange = (value: string) => {
    setSortBy(value);
  };

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
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
      <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4 }}>
        All Magic Sets
      </Typography>

      {/* Filters and Search */}
      <Box sx={{ mb: 4 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={4}>
            <DebouncedSearchInput
              value={initialValues.search || ''}
              onChange={setSearchTerm}
              placeholder="Search sets..."
              debounceMs={1000}
              fullWidth
            />
          </Grid>
          
          <Grid item xs={12} sm={6} md={3}>
            <MultiSelectDropdown
              value={selectedSetTypes}
              onChange={handleSetTypeChange}
              options={setTypes}
              label="Set Types"
              placeholder="All Types"
              fullWidth
            />
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <SortDropdown
              value={sortBy}
              onChange={handleSortChange}
              options={sortOptions}
              fullWidth
            />
          </Grid>

        </Grid>
      </Box>

      {/* Results Summary */}
      <ResultsSummary 
        showing={filteredSets.length} 
        total={sets.length} 
        itemType="sets" 
      />

      {/* Sets Grid */}
      <Box sx={{ 
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, 240px)',
        gap: 3,
        justifyContent: 'center'
      }}>
        {filteredSets.map((set) => (
          <MtgSetCard
            key={set.id}
            set={set}
            onSetClick={handleSetClick}
          />
        ))}
      </Box>

      {filteredSets.length === 0 && (
        <EmptyState
          message="No sets found matching your criteria"
          description="Try adjusting your filters or search terms"
        />
      )}
    </Container>
    </GraphQLQueryStateContainer>
  );
};