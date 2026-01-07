import React, { startTransition, useState, useMemo, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { PageContainer } from '../molecules/layouts';
import { StatusMessage } from '../molecules/feedback';
import { ArtistPageHeader } from '../organisms/Artists/ArtistPageHeader';
import { ArtistPageFilters } from '../organisms/Artists/ArtistPageFilters';
import { ArtistPageCardDisplay } from '../organisms/Artists/ArtistPageCardDisplay';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { FilterControlsWithLoading } from '../molecules/shared/FilterControlsWithLoading';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import { SectionErrorBoundary } from '../utils/ErrorBoundaries';
import { useArtistCardsData } from '../../hooks/useArtistCardsData';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { LinkParamsProvider } from '../../contexts/LinkParamsContext';
import { ARTIST_PAGE_SORT_OPTIONS, ARTIST_PAGE_COLLECTOR_SORT_OPTIONS } from '../../config/cardSortOptions';
import {
  getCollectionCountOptions,
  getSignedCardsOptions,
  getFormatOptions
} from '../../utils/cardUtils';
import type { FilterPanelConfig } from '../../types/filters';

/**
 * ArtistCardsPage - Display all cards by a specific artist with filtering
 * Refactored to use useArtistCardsData hook for reduced complexity
 */
export const ArtistCardsPage: React.FC = () => {
  const { artistName } = useParams<{ artistName: string }>();
  const decodedArtistName = decodeURIComponent(artistName || '').replace(/-/g, ' ');
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);

  // All data and state management extracted to custom hook
  const {
    cards,
    cardsLoading,
    cardsError,
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedSets,
    allRarities,
    allSets,
    allFormats,
    setLabelMap,
    filteredCards,
    displayArtistName,
    alternateNames,
    setSearchTerm,
    setSortBy,
    updateFilter,
    isFilteringOrSorting,
    hasCollector
  } = useArtistCardsData(artistName, decodedArtistName);

  // Determine if we should use mobile layout
  const useMobileLayout = isMobile || isTablet;

  // Calculate active filter count for mobile filter badge
  const activeFilterCount = useMemo(() => {
    let count = 0;
    if (searchTerm) count++;
    if (selectedRarities.length > 0) count++;
    if (selectedSets.length > 0) count++;
    if (Array.isArray(filters.formats) && filters.formats.length > 0) count++;
    if (Array.isArray(filters.collectionCounts) && filters.collectionCounts.length > 0) count++;
    if (Array.isArray(filters.signedCards) && filters.signedCards.length > 0) count++;
    return count;
  }, [searchTerm, selectedRarities, selectedSets, filters]);

  // Toggle filter drawer for mobile
  const handleFilterDrawerToggle = useCallback(() => {
    setFilterDrawerOpen(prev => !prev);
  }, []);

  // Handler functions
  const handleClearFilters = useCallback(() => {
    startTransition(() => {
      setSearchTerm('');
      updateFilter('rarities', []);
      updateFilter('sets', []);
      updateFilter('formats', []);
      if (hasCollector) {
        updateFilter('collectionCounts', []);
        updateFilter('signedCards', []);
      }
    });
  }, [setSearchTerm, updateFilter, hasCollector]);

  const handleSearchChange = useCallback((value: string) => {
    startTransition(() => setSearchTerm(value));
  }, [setSearchTerm]);

  // Build filter config for mobile drawer
  const filterConfig: FilterPanelConfig = useMemo(() => {
    const collectionCounts = (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[];
    const signedCardsFilter = (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[];
    const formatsFilter = (Array.isArray(filters.formats) ? filters.formats : []) as string[];

    return {
      search: {
        value: searchTerm,
        onChange: handleSearchChange,
        placeholder: `Search ${displayArtistName}'s cards...`,
        debounceMs: 300,
        fullWidth: true
      },
      multiSelects: [
        // Rarities filter
        ...(allRarities.length > 1 ? [{
          key: 'rarities',
          value: selectedRarities,
          onChange: (value: string[]) => updateFilter('rarities', value),
          options: allRarities.map(rarity => ({ value: rarity, label: rarity })),
          label: 'Rarity',
          placeholder: 'All Rarities',
          fullWidth: true
        }] : []),
        // Sets filter
        ...(allSets.length > 1 ? [{
          key: 'sets',
          value: selectedSets,
          onChange: (value: string[]) => updateFilter('sets', value),
          options: allSets.map(setCode => ({
            value: setCode,
            label: setLabelMap.get(setCode) || setCode.toUpperCase()
          })),
          label: 'Set',
          placeholder: 'All Sets',
          fullWidth: true
        }] : []),
        // Formats filter
        ...(allFormats.length > 1 ? [{
          key: 'formats',
          value: formatsFilter,
          onChange: (value: string[]) => updateFilter('formats', value),
          options: getFormatOptions(),
          label: 'Format',
          placeholder: 'All Formats',
          fullWidth: true
        }] : [])
      ],
      sort: {
        value: sortBy,
        onChange: setSortBy,
        options: hasCollector ? ARTIST_PAGE_COLLECTOR_SORT_OPTIONS : ARTIST_PAGE_SORT_OPTIONS,
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
    searchTerm, handleSearchChange, displayArtistName,
    selectedRarities, selectedSets, allRarities, allSets, allFormats,
    setLabelMap, filters.collectionCounts, filters.signedCards, filters.formats,
    sortBy, setSortBy, hasCollector, updateFilter
  ]);

  // Build results summary text for mobile bar
  const resultsSummary = `${filteredCards.length} of ${cards.length} cards`;

  // Error handling
  if (!artistName) {
    return (
      <StatusMessage severity="error" sx={{ m: 4 }}>
        No artist name provided. Please provide an artist name in the URL.
      </StatusMessage>
    );
  }

  if (cardsError) {
    return (
      <StatusMessage severity="error" sx={{ m: 4 }}>
        {cardsError.message || 'Failed to load cards'}
      </StatusMessage>
    );
  }

  // Render using template composition pattern (similar to SetPageTemplate)
  return (
    <LinkParamsProvider additionalParams={{ formats: 'paper' }}>
      <QueryStateContainer
        loading={cardsLoading}
        error={cardsError}
        containerProps={{ maxWidth: false }}
      >
        <PageContainer maxWidth={false} sx={{ mt: { xs: 1, sm: 2 }, mb: 4, px: { xs: 1, sm: 2, md: 3 } }}>
          {/* Header Section */}
          <SectionErrorBoundary name="ArtistPageHeader">
            <ArtistPageHeader
              displayArtistName={displayArtistName}
              alternateNames={alternateNames}
            />
          </SectionErrorBoundary>

          {/* Mobile Filter Bar (sticky) */}
          {useMobileLayout && (
            <MobileFilterBar
              activeFilterCount={activeFilterCount}
              onFilterClick={handleFilterDrawerToggle}
              resultsSummary={resultsSummary}
            />
          )}

          {/* Desktop Filters Section (inline) */}
          {useMobileLayout === false && (
            <FilterControlsWithLoading isLoading={isFilteringOrSorting}>
              <ArtistPageFilters
                totalCards={cards.length}
                displayArtistName={displayArtistName}
                hasCollector={hasCollector}
                filters={{
                  search: {
                    value: searchTerm,
                    onChange: handleSearchChange
                  },
                  rarities: {
                    value: selectedRarities,
                    onChange: (value: string[]) => updateFilter('rarities', value),
                    options: allRarities,
                    shouldShow: allRarities.length > 1
                  },
                  sets: {
                    value: selectedSets,
                    onChange: (value: string[]) => updateFilter('sets', value),
                    options: allSets,
                    shouldShow: allSets.length > 1,
                    getOptionLabel: (setCode: string) => setLabelMap.get(setCode) || setCode.toUpperCase()
                  },
                  sort: {
                    value: sortBy,
                    onChange: setSortBy
                  },
                  collectionCounts: hasCollector ? {
                    value: (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[],
                    onChange: (value: string[]) => updateFilter('collectionCounts', value)
                  } : undefined,
                  signedCards: hasCollector ? {
                    value: (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[],
                    onChange: (value: string[]) => updateFilter('signedCards', value)
                  } : undefined,
                  formats: {
                    value: (Array.isArray(filters.formats) ? filters.formats : []) as string[],
                    onChange: (value: string[]) => updateFilter('formats', value),
                    shouldShow: allFormats.length > 1
                  }
                }}
              />
            </FilterControlsWithLoading>
          )}

          {/* Results Summary - only show on desktop (mobile has it in the bar) */}
          {useMobileLayout === false && (
            <ResultsSummary
              current={filteredCards.length}
              total={cards.length}
              label="cards"
              textAlign="center"
            />
          )}

          {/* Card Display Section */}
          <ArtistPageCardDisplay
            filteredCards={filteredCards}
            displayArtistName={displayArtistName}
            hasCollector={hasCollector}
            isLoading={cardsLoading}
            onClearFilters={handleClearFilters}
          />

          {/* Back to Top Button */}
          <BackToTopFab />
        </PageContainer>

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
      </QueryStateContainer>
    </LinkParamsProvider>
  );
};

export default ArtistCardsPage;
