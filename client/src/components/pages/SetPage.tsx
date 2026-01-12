import React, { useState, useMemo, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { Box, Tabs, Tab, Chip } from '@mui/material';
import { ViewListIcon, ViewModuleIcon } from '../atoms/Icons';
import { PageContainer } from '../molecules/layouts';
import { StatusMessage } from '../molecules/feedback';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { SetPageTemplate } from '../templates/SetPageTemplate';
import { SetPageHeader } from '../organisms/Sets/SetPageHeader';
import { SetPageFilters } from '../organisms/Sets/SetPageFilters';
import { SetPageCardDisplay } from '../organisms/Sets/SetPageCardDisplay';
import { SealedProductGrid } from '../organisms/Sealed';
import { FilterControlsWithLoading } from '../molecules/shared/FilterControlsWithLoading';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import { useSetPageData } from '../../hooks/useSetPageData';
import { useSealedProductsData } from '../../hooks/useSealedProductsData';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { useUrlState } from '../../hooks/useUrlState';
import { SET_PAGE_SORT_OPTIONS, SET_PAGE_COLLECTOR_SORT_OPTIONS } from '../../config/cardSortOptions';
import { getCollectionCountOptions, getSignedCardsOptions } from '../../utils/cardUtils';
import type { FilterPanelConfig } from '../../types/filters';

type SetPageTab = 'cards' | 'sealed';

/**
 * SetPage - Display a specific Magic: The Gathering set with filtering and grouping
 * Refactored to use useSetPageData hook for reduced complexity
 */
export const SetPage: React.FC = () => {
  const { setCode } = useParams<{ setCode: string }>();
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);

  // URL state for tab selection
  const tabUrlConfig = useMemo(() => ({
    tab: { default: 'cards' as SetPageTab }
  }), []);

  const { getInitialValues } = useUrlState({}, tabUrlConfig);
  const initialTabValue = useMemo(() => {
    const values = getInitialValues();
    const tabValue = values.tab as string;
    return (tabValue === 'cards' || tabValue === 'sealed') ? tabValue as SetPageTab : 'cards';
  }, [getInitialValues]);

  const [activeTab, setActiveTab] = useState<SetPageTab>(initialTabValue);

  // Sync active tab to URL
  useUrlState({ tab: activeTab }, tabUrlConfig);

  // All data and state management extracted to custom hook
  const {
    cards,
    setInfo,
    setName,
    isLoading,
    firstError,
    cardsLoading,
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedArtists,
    selectedGroupIds,
    allArtists,
    allRarities,
    allFinishes,
    filteredCards,
    sortedCards,
    cardGroups,
    allCardGroups,
    visibleGroupIds,
    allSameReleaseDate,
    currentCount,
    handleSearchChange,
    handleSortChange,
    handleRarityChange,
    handleClearFilters,
    updateFilter,
    isFilteringOrSorting,
    hasCollector
  } = useSetPageData(setCode);

  // Sealed products data - only fetch when sealed tab is active
  const {
    sealedProducts,
    loading: sealedLoading,
    error: sealedError
  } = useSealedProductsData(setCode, activeTab === 'sealed');

  // Tab change handler
  const handleTabChange = useCallback((_event: React.SyntheticEvent, newValue: SetPageTab) => {
    setActiveTab(newValue);
  }, []);

  // Calculate active filter count for mobile filter badge
  const activeFilterCount = useMemo(() => {
    let count = 0;
    if (searchTerm) count++;
    if (selectedRarities.length > 0) count++;
    if (selectedArtists.length > 0) count++;
    if (selectedGroupIds.length > 0) count++;
    if (Array.isArray(filters.collectionCounts) && filters.collectionCounts.length > 0) count++;
    if (Array.isArray(filters.signedCards) && filters.signedCards.length > 0) count++;
    if (Array.isArray(filters.finishes) && filters.finishes.length > 0) count++;
    return count;
  }, [searchTerm, selectedRarities, selectedArtists, selectedGroupIds, filters]);

  // Determine if we should use mobile layout (mobile or tablet)
  const useMobileLayout = isMobile || isTablet;

  // Check if there are multiple card groups (for showing groups toggle)
  const hasMultipleGroups = cardGroups.filter(g => g.cards.length > 0).length > 1;

  // Toggle filter drawer for mobile
  const handleFilterDrawerToggle = useCallback(() => {
    setFilterDrawerOpen(prev => !prev);
  }, []);

  // Handler for show groups change (used by both desktop and mobile)
  const handleShowGroupsChange = useCallback((value: boolean) => {
    updateFilter('showGroups', value);
  }, [updateFilter]);

  // Build filter config for mobile drawer (mirrors SetPageFilters logic)
  const filterConfig: FilterPanelConfig = useMemo(() => {
    const showGroups = filters.showGroups !== false;
    const collectionCounts = (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[];
    const signedCardsFilter = (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[];
    const finishesFilter = (Array.isArray(filters.finishes) ? filters.finishes : []) as string[];

    return {
      search: {
        value: searchTerm,
        onChange: handleSearchChange,
        placeholder: 'Search cards...',
        debounceMs: 300,
        fullWidth: true
      },
      multiSelects: [
        // Card Groups (if multiple groups exist)
        ...(hasMultipleGroups && showGroups ? [{
          key: 'cardGroups',
          value: selectedGroupIds,
          onChange: (value: string[]) => updateFilter('groups', value),
          options: cardGroups
            .filter(g => g.cards.length > 0)
            .map(g => ({
              value: g.id,
              label: `${g.displayName} (${g.cards.length})`
            })),
          label: 'Card Group',
          placeholder: 'All Groups',
          fullWidth: true
        }] : []),
        // Rarities filter
        ...(allRarities.length > 1 ? [{
          key: 'rarities',
          value: selectedRarities,
          onChange: handleRarityChange,
          options: allRarities.map(rarity => ({ value: rarity, label: rarity })),
          label: 'Rarity',
          placeholder: 'All Rarities',
          fullWidth: true
        }] : []),
        // Artists filter
        ...(allArtists.length > 1 ? [{
          key: 'artists',
          value: selectedArtists,
          onChange: (value: string[]) => updateFilter('artists', value),
          options: allArtists.map(artist => ({ value: artist, label: artist })),
          label: 'Artist',
          placeholder: 'All Artists',
          fullWidth: true
        }] : []),
        // Finishes filter
        ...(allFinishes.length > 1 ? [{
          key: 'finishes',
          value: finishesFilter,
          onChange: (value: string[]) => updateFilter('finishes', value),
          options: allFinishes.map(finish => ({ value: finish, label: finish })),
          label: 'Finish',
          placeholder: 'All Finishes',
          fullWidth: true
        }] : [])
      ],
      sort: {
        value: sortBy,
        onChange: handleSortChange,
        options: (hasCollector ? SET_PAGE_COLLECTOR_SORT_OPTIONS : SET_PAGE_SORT_OPTIONS).map(opt => {
          if (opt.value === 'release-desc' || opt.value === 'release-asc') {
            return {
              ...opt,
              condition: cards.some(c => c.releasedAt !== cards[0]?.releasedAt)
            };
          }
          return opt;
        }),
        fullWidth: true
      },
      collectorFilters: hasCollector ? {
        collectionCounts: {
          key: 'collectionCounts',
          value: collectionCounts,
          onChange: (value: string[]) => updateFilter('collectionCounts', value),
          options: getCollectionCountOptions(),
          label: 'Collection Count',
          placeholder: 'All Counts',
          fullWidth: true
        },
        signedCards: {
          key: 'signedCards',
          value: signedCardsFilter,
          onChange: (value: string[]) => updateFilter('signedCards', value),
          options: getSignedCardsOptions(),
          label: 'Signed Cards',
          placeholder: 'All Cards',
          fullWidth: true
        }
      } : undefined
    };
  }, [
    searchTerm, handleSearchChange, selectedGroupIds, selectedRarities, selectedArtists,
    handleRarityChange, allRarities, allArtists, allFinishes, cardGroups, hasMultipleGroups,
    filters.showGroups, filters.collectionCounts, filters.signedCards, filters.finishes,
    sortBy, handleSortChange, hasCollector, cards, updateFilter
  ]);

  // Error handling
  if (!setCode) {
    return (
      <PageContainer maxWidth="lg" sx={{ mt: 4 }}>
        <StatusMessage severity="error">
          No set code provided. Please provide a set code in the URL (e.g., ?set=lea)
        </StatusMessage>
      </PageContainer>
    );
  }

  if (firstError) {
    return (
      <PageContainer maxWidth="lg" sx={{ mt: 4 }}>
        <StatusMessage severity="error">
          {firstError.message || 'Failed to load cards'}
        </StatusMessage>
      </PageContainer>
    );
  }

  // Build results summary text for mobile bar
  const resultsSummary = `${currentCount} of ${cards.length} cards`;

  // Render using template composition pattern
  return (
    <>
      <SetPageTemplate
      isLoading={isLoading}
      error={firstError}
      currentCount={activeTab === 'cards' ? currentCount : sealedProducts.length}
      totalCount={activeTab === 'cards' ? cards.length : sealedProducts.length}
      // Disable template's mobile layout - we'll handle it manually inside Cards tab
      useMobileLayout={false}
      filterDrawerOpen={filterDrawerOpen}
      onFilterDrawerToggle={handleFilterDrawerToggle}
      filterConfig={filterConfig}
      activeFilterCount={activeFilterCount}
      onClearFilters={handleClearFilters}
      header={
        <Box>
          <SetPageHeader
            setInfo={setInfo}
            setName={setName}
            setCode={setCode}
            availableGroupIds={allCardGroups.map(g => g.id)}
          />

          {/* Tab Navigation - Right after header */}
          <Box sx={{ borderBottom: 1, borderColor: 'divider', mt: 2, mb: 3, display: 'flex', justifyContent: 'center' }}>
            <Tabs
              value={activeTab}
              onChange={handleTabChange}
              sx={{
                '& .MuiTab-root': {
                  textTransform: 'none',
                  fontWeight: 600,
                  fontSize: '0.95rem',
                  minWidth: 120,
                },
              }}
            >
              <Tab label="Cards" value="cards" />
              <Tab label="Sealed" value="sealed" />
            </Tabs>
          </Box>
        </Box>
      }
      cardDisplay={
        <Box>
          {/* Cards Tab Content */}
          {activeTab === 'cards' && (
            <Box>
              {/* Mobile Filter Bar (sticky) - inside Cards tab */}
              {useMobileLayout && (
                <MobileFilterBar
                  activeFilterCount={activeFilterCount}
                  onFilterClick={handleFilterDrawerToggle}
                  showGroups={filters.showGroups !== false}
                  onShowGroupsChange={handleShowGroupsChange}
                  showGroupsToggle={hasMultipleGroups}
                  resultsSummary={resultsSummary}
                />
              )}

              {/* Binder View Button - Desktop only, above filters */}
              {useMobileLayout === false && hasMultipleGroups && (
                <Box sx={{ display: 'flex', justifyContent: 'center', mb: 2 }}>
                  <Chip
                    icon={filters.showGroups !== false ? <ViewModuleIcon /> : <ViewListIcon />}
                    label={filters.showGroups !== false ? 'Grouped' : 'Flat'}
                    onClick={() => handleShowGroupsChange(!(filters.showGroups !== false))}
                    variant={filters.showGroups !== false ? 'filled' : 'outlined'}
                    color={filters.showGroups !== false ? 'primary' : 'default'}
                    sx={{
                      minHeight: 36,
                      '& .MuiChip-icon': {
                        fontSize: '1.1rem'
                      }
                    }}
                  />
                </Box>
              )}

              {/* Filters Section */}
              {useMobileLayout === false && (
                <FilterControlsWithLoading isLoading={isFilteringOrSorting}>
                  <SetPageFilters
                    searchTerm={searchTerm}
                    onSearchChange={handleSearchChange}
                    sortBy={sortBy}
                    onSortChange={handleSortChange}
                    selectedRarities={selectedRarities}
                    selectedArtists={selectedArtists}
                    selectedGroupIds={selectedGroupIds}
                    showGroups={filters.showGroups !== false}
                    onRarityChange={handleRarityChange}
                    onArtistChange={(value: string[]) => updateFilter('artists', value)}
                    onGroupChange={(groupIds: string[]) => updateFilter('groups', groupIds)}
                    onShowGroupsChange={handleShowGroupsChange}
                    allRarities={allRarities}
                    allArtists={allArtists}
                    allFinishes={allFinishes}
                    cardGroups={cardGroups}
                    cards={cards}
                    hasCollector={hasCollector}
                    collectionCounts={(Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[]}
                    signedCards={(Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[]}
                    finishes={(Array.isArray(filters.finishes) ? filters.finishes : []) as string[]}
                    onCollectionCountsChange={(value: string[]) => updateFilter('collectionCounts', value)}
                    onSignedCardsChange={(value: string[]) => updateFilter('signedCards', value)}
                    onFinishesChange={(value: string[]) => updateFilter('finishes', value)}
                  />
                </FilterControlsWithLoading>
              )}

              {/* Card Display */}
              <SetPageCardDisplay
                cardsLoading={cardsLoading}
                sortedCards={sortedCards}
                filteredCards={filteredCards}
                cardGroups={cardGroups}
                setInfo={setInfo}
                showGroups={filters.showGroups !== false}
                visibleGroupIds={visibleGroupIds}
                allSameReleaseDate={allSameReleaseDate}
                setCode={setCode}
                hasCollector={hasCollector}
                onClearFilters={handleClearFilters}
              />
            </Box>
          )}

          {/* Sealed Tab Content - Centered */}
          {activeTab === 'sealed' && (
            <Box sx={{ display: 'flex', justifyContent: 'center' }}>
              <Box sx={{ width: '100%', maxWidth: '1400px' }}>
                <SealedProductGrid
                  products={sealedProducts}
                  loading={sealedLoading}
                  error={sealedError}
                />
              </Box>
            </Box>
          )}
        </Box>
      }
    />

      {/* Mobile Filter Drawer */}
      {useMobileLayout && (
        <FilterDrawer
          open={filterDrawerOpen}
          onClose={handleFilterDrawerToggle}
          config={filterConfig}
          title="Filter Cards"
          activeFilterCount={activeFilterCount}
          onClear={handleClearFilters}
        />
      )}
    </>
  );
};

export default SetPage;
