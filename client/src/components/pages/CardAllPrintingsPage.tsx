import React, { useEffect, useState, useMemo, useCallback, useTransition } from 'react';
import { logger } from '../../utils/logger';
import { useParams } from 'react-router-dom';
import { useCardQueries } from '../../hooks/useCardQueries';
import type { Card } from '../../types/card';
import { PageContainer, Section } from '../molecules/layouts';
import { Heading, BodyText } from '../molecules/text';
import { LoadingIndicator, StatusMessage } from '../molecules/feedback';
import { AppButton, FilterControlsWithLoading } from '../molecules/shared';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { CardGrid } from '../organisms/Cards/CardGrid';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import { useCardFiltering } from '../../hooks/useCardFiltering';
import { useMinimumLoadingTime } from '../../hooks/useMinimumLoadingTime';
import { useOptimizedSort } from '../../hooks/useOptimizedSort';
import { useCollectionUpdates } from '../../hooks/useCollectionUpdates';
import { useWishlistUpdates } from '../../hooks/useWishlistUpdates';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { CardFilterPanel } from '../organisms/Cards/CardFilterPanel';
import { CARD_DETAIL_SORT_OPTIONS, CARD_DETAIL_COLLECTOR_SORT_OPTIONS, createCardSortOptions } from '../../config/cardSortOptions';
import { CollectorFiltersSection } from '../molecules/shared/CollectorFiltersSection';
import {
  getCollectionCountOptions,
  getSignedCardsOptions,
  getFormatOptions
} from '../../utils/cardUtils';
import { handleGraphQLError, globalLoadingManager } from '../../utils/networkErrorHandler';
import { AppErrorBoundary } from '../utils/ErrorBoundaries';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useCollectorNavigation } from '../../hooks/useCollectorNavigation';
import { useUrlState } from '../../hooks/useUrlState';
import { RefreshIcon } from '../atoms/Icons';
import { LinkParamsProvider } from '../../contexts/LinkParamsContext';
import type { FilterPanelConfig } from '../../types/filters';

// Stable empty array to prevent infinite re-renders
const EMPTY_CARDS_ARRAY: Card[] = [];

export const CardAllPrintingsPage: React.FC = () => {
  const { cardName } = useParams<{ cardName: string }>();
  const { navigateWithCollector } = useCollectorNavigation();
  const decodedCardName = decodeURIComponent(cardName || '');
  const [userFriendlyError, setUserFriendlyError] = useState<string | null>(null);
  const [retryCount, setRetryCount] = useState(0);

  // Check if we have a collector parameter
  const { hasCollector, collectorId } = useCollectorParam();
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);

  // Determine if we should use mobile layout
  const useMobileLayout = isMobile || isTablet;

  // Loading state with minimum display time
  const { isLoading: isFiltering, showLoading, hideLoading } = useMinimumLoadingTime(400);
  const [isPending, startFilterTransition] = useTransition();

  // Use card cache for fetching cards by name
  const { fetchCardsByName } = useCardQueries();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [cards, setCards] = useState<Card[]>(EMPTY_CARDS_ARRAY);

  // Listen for collection and wishlist updates via reusable hooks
  useCollectionUpdates(cards, setCards);
  useWishlistUpdates(cards, setCards);

  const refetch = useCallback(async () => {
    if (!cardName) return;

    setLoading(true);
    setError(null);
    try {
      const fetchedCards = await fetchCardsByName(decodedCardName);
      setCards(fetchedCards);
    } catch (err) {
      setError(err as Error);
      setCards(EMPTY_CARDS_ARRAY);
    } finally {
      setLoading(false);
    }
  }, [cardName, decodedCardName, fetchCardsByName]);

  useEffect(() => {
    refetch();
  }, [refetch]);

  useEffect(() => {
    const loadingKey = `card-detail-${decodedCardName}`;
    globalLoadingManager.setLoading(loadingKey, loading);
    
    return () => {
      globalLoadingManager.setLoading(loadingKey, false);
    };
  }, [loading, decodedCardName]);

  useEffect(() => {
    if (error) {
      try {
        const networkError = handleGraphQLError(error);
        setUserFriendlyError(networkError.userMessage);
      } catch {
        setUserFriendlyError('Failed to load card details. Please try again.');
      }
    } else {
      setUserFriendlyError(null);
    }
  }, [error]);

  const hasError = userFriendlyError || error;



  // handleBackClick removed - using href directly on buttons

  const handleRetry = async () => {
    setRetryCount(prev => prev + 1);
    setUserFriendlyError(null);
    try {
      await refetch();
    } catch (retryError) {
      logger.error('Retry failed:', retryError);
    }
  };

  const handleArtistClick = (artistName: string) => {
    navigateWithCollector(`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`, { formats: 'paper' });
  };

  // Read initial values from URL
  const urlStateConfig = useMemo(() => ({
    rarities: { default: [] },
    artists: { default: [] },
    formats: { default: [] },
    sort: { default: 'release-desc' },
    ...(hasCollector ? {
      counts: { default: [] },
      signed: { default: [] }
    } : {})
  }), [hasCollector]);

  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const initialUrlValues = useMemo(() => getInitialValues(), [getInitialValues]);

  // Memoize filtering options to prevent infinite re-renders
  const filteringOptions = useMemo(() => ({
    defaultSort: 'release-desc',
    initialSort: typeof initialUrlValues.sort === 'string' ? initialUrlValues.sort : 'release-desc',
    initialFilters: {
      rarities: Array.isArray(initialUrlValues.rarities) ? initialUrlValues.rarities : [],
      artists: Array.isArray(initialUrlValues.artists) ? initialUrlValues.artists : [],
      formats: Array.isArray(initialUrlValues.formats) ? initialUrlValues.formats : [],
      ...(hasCollector ? {
        collectionCounts: Array.isArray(initialUrlValues.counts) ? initialUrlValues.counts : [],
        signedCards: Array.isArray(initialUrlValues.signed) ? initialUrlValues.signed : []
      } : {})
    },
    includeSets: false,
    includeCollectorFilters: hasCollector
  }), [hasCollector, initialUrlValues]);

  // Use the shared card filtering hook (no search or sets filter for card detail page)
  const {
    filteredCards: unsortedFilteredCards,
    totalCards,
    sortBy,
    filters,
    setSortBy: setSortByOriginal,
    updateFilter: updateFilterOriginal,
    uniqueArtists,
    uniqueRarities,
    uniqueFormats,
    hasMultipleArtists,
    hasMultipleRarities,
    hasMultipleFormats
  } = useCardFiltering(cards, filteringOptions);

  // Create sort options and apply sorting
  const sortOptions = useMemo(() => createCardSortOptions<Card>(), []);
  const sortFunction = useMemo(() => {
    return sortOptions[sortBy] || sortOptions['release-desc'];
  }, [sortOptions, sortBy]);
  // Include collector context in sort key to disable caching when in collector mode
  const sortKey = hasCollector ? `${sortBy}-ctor-${collectorId}` : sortBy;
  const filteredCards = useOptimizedSort(unsortedFilteredCards, sortKey, sortFunction);

  // Wrapped filter handlers with loading state
  const setSortBy = useCallback((value: string) => {
    showLoading();
    startFilterTransition(() => {
      setSortByOriginal(value);
      hideLoading();
    });
  }, [setSortByOriginal, showLoading, hideLoading]);

  const updateFilter = useCallback((filterName: string, value: unknown) => {
    showLoading();
    startFilterTransition(() => {
      updateFilterOriginal(filterName, value);
      hideLoading();
    });
  }, [updateFilterOriginal, showLoading, hideLoading]);

  // Extract filter values for URL state
  const selectedRarities = (Array.isArray(filters.rarities) ? filters.rarities : []) as string[];
  const selectedArtists = (Array.isArray(filters.artists) ? filters.artists : []) as string[];
  const selectedFormats = (Array.isArray(filters.formats) ? filters.formats : []) as string[];

  // URL state sync for filters and sort
  useUrlState(
    {
      rarities: selectedRarities,
      artists: selectedArtists,
      formats: selectedFormats,
      sort: sortBy,
      ...(hasCollector ? {
        counts: (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[],
        signed: (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[]
      } : {})
    },
    urlStateConfig,
    { debounceMs: 0 }
  );

  // Calculate active filter count for mobile filter badge
  const activeFilterCount = useMemo(() => {
    let count = 0;
    if (selectedRarities.length > 0) count++;
    if (selectedArtists.length > 0) count++;
    if (selectedFormats.length > 0) count++;
    if (Array.isArray(filters.collectionCounts) && filters.collectionCounts.length > 0) count++;
    if (Array.isArray(filters.signedCards) && filters.signedCards.length > 0) count++;
    return count;
  }, [selectedRarities, selectedArtists, selectedFormats, filters]);

  // Toggle filter drawer for mobile
  const handleFilterDrawerToggle = useCallback(() => {
    setFilterDrawerOpen(prev => !prev);
  }, []);

  // Clear all filters handler
  const handleClearFilters = useCallback(() => {
    startFilterTransition(() => {
      updateFilterOriginal('rarities', []);
      updateFilterOriginal('artists', []);
      updateFilterOriginal('formats', []);
      if (hasCollector) {
        updateFilterOriginal('collectionCounts', []);
        updateFilterOriginal('signedCards', []);
      }
    });
  }, [updateFilterOriginal, hasCollector]);

  // Build filter config for mobile drawer
  const filterConfig: FilterPanelConfig = useMemo(() => {
    const collectionCounts = (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[];
    const signedCardsFilter = (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[];

    return {
      multiSelects: [
        // Rarities filter
        ...(hasMultipleRarities ? [{
          key: 'rarities',
          value: selectedRarities,
          onChange: (value: string[]) => updateFilter('rarities', value),
          options: uniqueRarities.map(rarity => ({ value: rarity, label: rarity })),
          label: 'Rarity',
          placeholder: 'All Rarities',
          fullWidth: true
        }] : []),
        // Artists filter
        ...(hasMultipleArtists ? [{
          key: 'artists',
          value: selectedArtists,
          onChange: (value: string[]) => updateFilter('artists', value),
          options: uniqueArtists.map(artist => ({ value: artist, label: artist })),
          label: 'Artist',
          placeholder: 'All Artists',
          fullWidth: true,
          searchable: true
        }] : []),
        // Formats filter
        ...(hasMultipleFormats ? [{
          key: 'formats',
          value: selectedFormats,
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
        options: hasCollector ? CARD_DETAIL_COLLECTOR_SORT_OPTIONS : CARD_DETAIL_SORT_OPTIONS,
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
    selectedRarities, selectedArtists, selectedFormats,
    uniqueRarities, uniqueArtists, hasMultipleRarities, hasMultipleArtists, hasMultipleFormats,
    filters.collectionCounts, filters.signedCards,
    sortBy, setSortBy, hasCollector, updateFilter
  ]);

  // Build results summary text for mobile bar
  const resultsSummary = `${filteredCards.length} of ${totalCards} printings`;

  if (loading) {
    return (
      <PageContainer maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <Section asSection={false} sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
          <LoadingIndicator />
        </Section>
      </PageContainer>
    );
  }

  if (hasError) {
    return (
      <PageContainer maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <StatusMessage
          severity="error"
          sx={{ mb: 2 }}
          action={
            <Section asSection={false} sx={{ display: 'flex', gap: 1 }}>
              <AppButton
                color="inherit"
                size="small"
                startIcon={<RefreshIcon />}
                onClick={handleRetry}
                loading={loading}
              >
                {loading ? 'Retrying...' : 'Retry'}
              </AppButton>
            </Section>
          }
        >
          <BodyText variant="body1">
            {userFriendlyError || error?.message || 'Failed to load card details'}
          </BodyText>
          {retryCount > 0 && (
            <BodyText variant="caption" display="block" sx={{ mt: 1 }}>
              Retry attempts: {retryCount}
            </BodyText>
          )}
        </StatusMessage>
      </PageContainer>
    );
  }

  if (cards.length === 0) {
    return (
      <PageContainer maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <BodyText>No cards found with name "{decodedCardName}"</BodyText>
      </PageContainer>
    );
  }

  return (
    <LinkParamsProvider additionalParams={{ formats: 'paper' }}>
      <PageContainer maxWidth={false} sx={{ mt: { xs: 1, sm: 2 }, mb: 4, px: { xs: 1, sm: 2, md: 3 } }}>
        {/* Centered card name */}
        <Section asSection={false} sx={{ textAlign: 'center', mb: { xs: 2, md: 4 } }}>
          <Heading variant="h2" fontWeight="bold">
            {decodedCardName}
          </Heading>
        </Section>

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
          <FilterControlsWithLoading isLoading={isFiltering || isPending}>
            <CardFilterPanel
              sortBy={sortBy}
              filters={filters}
              onSortChange={setSortBy}
              onFilterChange={updateFilter}
              uniqueArtists={uniqueArtists}
              uniqueRarities={uniqueRarities}
              uniqueFormats={uniqueFormats}
              hasMultipleArtists={hasMultipleArtists}
              hasMultipleRarities={hasMultipleRarities}
              hasMultipleFormats={hasMultipleFormats}
              filteredCount={filteredCards.length}
              totalCount={totalCards}
              showSearch={false}
              sortOptions={hasCollector ? CARD_DETAIL_COLLECTOR_SORT_OPTIONS : CARD_DETAIL_SORT_OPTIONS}
              centerControls={true}
            />

            {/* Collector-specific filters */}
            {hasCollector && (
              <CollectorFiltersSection
                config={{
                  collectionCounts: {
                    key: 'collectionCounts',
                    value: (filters.collectionCounts as string[]) || [],
                    onChange: (value: string[]) => updateFilter('collectionCounts', value),
                    options: getCollectionCountOptions(),
                    label: 'Collection Count',
                    placeholder: 'All Counts',
                    minWidth: 180
                  },
                  signedCards: {
                    key: 'signedCards',
                    value: (filters.signedCards as string[]) || [],
                    onChange: (value: string[]) => updateFilter('signedCards', value),
                    options: getSignedCardsOptions(),
                    label: 'Signed Cards',
                    placeholder: 'All Cards',
                    minWidth: 150
                  }
                }}
                sx={{ mt: 2, mb: 3, mx: 'auto', maxWidth: '800px' }}
              />
            )}
          </FilterControlsWithLoading>
        )}

        {/* Results Summary - only show on desktop (mobile has it in the bar) */}
        {useMobileLayout === false && (
          <ResultsSummary
            current={filteredCards.length}
            total={totalCards}
            label="printings"
            textAlign="center"
          />
        )}

        {/* Cards Grid */}
        <AppErrorBoundary variant="card-grid" name="CardDetailGrid">
          <CardGrid
            cards={filteredCards}
            groupId="all-printings"
            context={{
              isOnCardPage: true,
              hasCollector,
              showCollectorInfo: hasCollector
            }}
            isLoading={false}
            onArtistClick={handleArtistClick}
          />
        </AppErrorBoundary>

        {/* Back to Top Button */}
        <BackToTopFab />
      </PageContainer>

      {/* Mobile Filter Drawer */}
      {useMobileLayout && (
        <FilterDrawer
          open={filterDrawerOpen}
          onClose={handleFilterDrawerToggle}
          config={filterConfig}
          title="Filter Printings"
          activeFilterCount={activeFilterCount}
          onClear={handleClearFilters}
        />
      )}
    </LinkParamsProvider>
  );
};

export default CardAllPrintingsPage;
