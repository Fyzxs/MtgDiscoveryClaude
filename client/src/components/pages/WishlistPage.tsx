import React, { useState, useMemo, useCallback, useTransition } from 'react';
import { useQuery } from '@apollo/client/react';
import { Box, CircularProgress } from '../atoms';
import { Heading } from '../molecules/text';
import { GET_USER_WISHLIST } from '../../graphql/queries/wishlist';
import { EmptyState } from '../molecules/shared/EmptyState';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { MobileFilterBar } from '../molecules/shared/MobileFilterBar';
import { CardGridErrorBoundary, FilterErrorBoundary } from '../utils/ErrorBoundaries';
import { CardGrid } from '../organisms/Cards/CardGrid';
import { FilterPanel } from '../organisms/filters/FilterPanel';
import { FilterDrawer } from '../organisms/filters/FilterDrawer';
import { FilterControlsWithLoading } from '../molecules/shared/FilterControlsWithLoading';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { useUser } from '../../contexts/UserContext';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { WISHLIST_PAGE_SORT_OPTIONS, createCardSortOptions } from '../../config/cardSortOptions';
import type { Card } from '../../types/card';
import type { FilterPanelConfig } from '../../types/filters';

interface WishlistResponse {
  userWishlist: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

export const WishlistPage: React.FC = () => {
  const { hasCollector, collectorId } = useCollectorParam();
  const { userProfile } = useUser();
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const [isPending, startTransition] = useTransition();
  const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);

  // Filter/sort state
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('name-asc');
  const [selectedRarities, setSelectedRarities] = useState<string[]>([]);
  const [selectedArtists, setSelectedArtists] = useState<string[]>([]);
  const [selectedSets, setSelectedSets] = useState<string[]>([]);

  // Determine if we should use mobile layout
  const useMobileLayout = isMobile || isTablet;

  // Determine if viewing own wishlist
  const isOwnWishlist = userProfile?.userId === collectorId;

  // Single query: Get wishlist cards with full card data
  const { loading, error, data } = useQuery<WishlistResponse>(GET_USER_WISHLIST, {
    variables: { userId: collectorId },
    skip: !hasCollector
  });

  // Cards from wishlist query
  const cards = useMemo(() => {
    return data?.userWishlist?.data || [];
  }, [data?.userWishlist?.data]);

  // Extract unique filter options from cards
  const allRarities = useMemo(() => {
    const rarities = [...new Set(cards.map(c => c.rarity).filter(Boolean))] as string[];
    const rarityOrder: Record<string, number> = { common: 1, uncommon: 2, rare: 3, mythic: 4, special: 5, bonus: 6 };
    return rarities.sort((a, b) => (rarityOrder[a.toLowerCase()] || 99) - (rarityOrder[b.toLowerCase()] || 99));
  }, [cards]);

  const allArtists = useMemo(() => {
    const artists = [...new Set(cards.map(c => c.artist).filter(Boolean))] as string[];
    return artists.sort((a, b) => a.localeCompare(b));
  }, [cards]);

  const allSets = useMemo(() => {
    const sets = [...new Set(cards.map(c => c.setName).filter(Boolean))] as string[];
    return sets.sort((a, b) => a.localeCompare(b));
  }, [cards]);

  // Handlers with transitions for smooth updates
  const handleSearchChange = useCallback((value: string) => {
    startTransition(() => {
      setSearchTerm(value);
    });
  }, []);

  const handleSortChange = useCallback((value: string) => {
    startTransition(() => {
      setSortBy(value);
    });
  }, []);

  const handleRarityChange = useCallback((value: string[]) => {
    startTransition(() => {
      setSelectedRarities(value);
    });
  }, []);

  const handleArtistChange = useCallback((value: string[]) => {
    startTransition(() => {
      setSelectedArtists(value);
    });
  }, []);

  const handleSetChange = useCallback((value: string[]) => {
    startTransition(() => {
      setSelectedSets(value);
    });
  }, []);

  const handleClearFilters = useCallback(() => {
    startTransition(() => {
      setSearchTerm('');
      setSelectedRarities([]);
      setSelectedArtists([]);
      setSelectedSets([]);
    });
  }, []);

  const handleFilterDrawerToggle = useCallback(() => {
    setFilterDrawerOpen(prev => !prev);
  }, []);

  // Apply filters
  const filteredCards = useMemo(() => {
    let result = [...cards];

    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      result = result.filter(card =>
        card.name.toLowerCase().includes(term) ||
        card.typeLine?.toLowerCase().includes(term) ||
        card.oracleText?.toLowerCase().includes(term)
      );
    }

    if (selectedRarities.length > 0) {
      result = result.filter(card =>
        card.rarity && selectedRarities.includes(card.rarity)
      );
    }

    if (selectedArtists.length > 0) {
      result = result.filter(card =>
        card.artist && selectedArtists.includes(card.artist)
      );
    }

    if (selectedSets.length > 0) {
      result = result.filter(card =>
        card.setName && selectedSets.includes(card.setName)
      );
    }

    return result;
  }, [cards, searchTerm, selectedRarities, selectedArtists, selectedSets]);

  // Apply sorting
  const sortedCards = useMemo(() => {
    const sortFunctions = createCardSortOptions<Card>();
    const sortFn = sortFunctions[sortBy];
    if (sortFn) {
      return [...filteredCards].sort(sortFn);
    }
    return filteredCards;
  }, [filteredCards, sortBy]);

  // Page title based on whose wishlist we're viewing
  const pageTitle = isOwnWishlist ? 'My Wishlist' : 'Wishlist';

  // Card context for display
  const cardContext = {
    isOnSetPage: false,
    hasCollector: hasCollector
  };

  // Check if filters are active
  const hasActiveFilters = searchTerm || selectedRarities.length > 0 || selectedArtists.length > 0 || selectedSets.length > 0;

  // Calculate active filter count for mobile badge
  const activeFilterCount = useMemo(() => {
    let count = 0;
    if (searchTerm) count++;
    if (selectedRarities.length > 0) count++;
    if (selectedArtists.length > 0) count++;
    if (selectedSets.length > 0) count++;
    return count;
  }, [searchTerm, selectedRarities, selectedArtists, selectedSets]);

  // Build filter config (used by both desktop FilterPanel and mobile FilterDrawer)
  const filterConfig: FilterPanelConfig = useMemo(() => ({
    search: {
      value: searchTerm,
      onChange: handleSearchChange,
      placeholder: 'Search cards...',
      debounceMs: 300,
      minWidth: 200,
      fullWidth: useMobileLayout
    },
    multiSelects: [
      ...(allSets.length > 1 ? [{
        key: 'sets',
        value: selectedSets,
        onChange: handleSetChange,
        options: allSets.map(set => ({ value: set, label: set })),
        label: 'Set',
        placeholder: 'All Sets',
        minWidth: 200,
        searchable: true,
        fullWidth: useMobileLayout
      }] : []),
      ...(allRarities.length > 1 ? [{
        key: 'rarities',
        value: selectedRarities,
        onChange: handleRarityChange,
        options: allRarities.map(rarity => ({ value: rarity, label: rarity })),
        label: 'Rarity',
        placeholder: 'All Rarities',
        minWidth: 150,
        fullWidth: useMobileLayout
      }] : []),
      ...(allArtists.length > 1 ? [{
        key: 'artists',
        value: selectedArtists,
        onChange: handleArtistChange,
        options: allArtists.map(artist => ({ value: artist, label: artist })),
        label: 'Artist',
        placeholder: 'All Artists',
        minWidth: 200,
        searchable: true,
        fullWidth: useMobileLayout
      }] : [])
    ],
    sort: {
      value: sortBy,
      onChange: handleSortChange,
      options: WISHLIST_PAGE_SORT_OPTIONS,
      minWidth: 200,
      fullWidth: useMobileLayout
    }
  }), [
    searchTerm, handleSearchChange, selectedSets, handleSetChange, allSets,
    selectedRarities, handleRarityChange, allRarities,
    selectedArtists, handleArtistChange, allArtists,
    sortBy, handleSortChange, useMobileLayout
  ]);

  // Results summary text
  const resultsSummary = `${sortedCards.length} of ${cards.length} cards`;

  // No collector param in URL
  if (!hasCollector) {
    return (
      <Box sx={{ py: 4 }}>
        <Heading level={1} sx={{ mb: 4, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
          <span role="img" aria-label="heart">♡</span> Wishlist
        </Heading>
        <EmptyState
          message="No wishlist specified"
          suggestion="Add ?ctor=userId to the URL to view a wishlist"
        />
      </Box>
    );
  }

  // Loading state
  if (loading) {
    return (
      <Box sx={{ py: 4 }}>
        <Heading level={1} sx={{ mb: 4, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
          <span role="img" aria-label="heart">♡</span> {pageTitle}
        </Heading>
        <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Box>
    );
  }

  // Error state
  if (error) {
    return (
      <Box sx={{ py: 4 }}>
        <Heading level={1} sx={{ mb: 4, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
          <span role="img" aria-label="heart">♡</span> {pageTitle}
        </Heading>
        <EmptyState
          message="Failed to load wishlist"
          suggestion={error.message}
        />
      </Box>
    );
  }

  // Empty state
  if (cards.length === 0) {
    return (
      <Box sx={{ py: 4 }}>
        <Heading level={1} sx={{ mb: 4, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
          <span role="img" aria-label="heart">♡</span> {pageTitle}
        </Heading>
        <EmptyState
          message={isOwnWishlist ? "Your wishlist is empty" : "This wishlist is empty"}
          suggestion={isOwnWishlist ? "Press Q or Y while viewing cards to toggle wishlist mode, then add cards using the keyboard entry workflow" : ""}
        />
      </Box>
    );
  }

  return (
    <Box sx={{ py: 4 }}>
      {/* Header */}
      <Box sx={{ mb: 4 }}>
        <Heading level={1} sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1, m: 0 }}>
          <span role="img" aria-label="heart">♡</span> {pageTitle}
        </Heading>
      </Box>

      {/* Mobile Filter Bar (sticky) */}
      {useMobileLayout && cards.length > 1 && (
        <MobileFilterBar
          activeFilterCount={activeFilterCount}
          onFilterClick={handleFilterDrawerToggle}
          resultsSummary={resultsSummary}
        />
      )}

      {/* Desktop Filters */}
      {!useMobileLayout && cards.length > 1 && (
        <FilterErrorBoundary name="WishlistPageFilters">
          <FilterControlsWithLoading isLoading={isPending}>
            <FilterPanel
              config={filterConfig}
              layout="compact"
              sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}
            />
          </FilterControlsWithLoading>
        </FilterErrorBoundary>
      )}

      {/* Results summary for desktop */}
      {!useMobileLayout && hasActiveFilters && (
        <ResultsSummary
          current={sortedCards.length}
          total={cards.length}
          label="cards"
          textAlign="center"
        />
      )}

      {/* Empty filtered state */}
      {sortedCards.length === 0 && hasActiveFilters && (
        <EmptyState
          message="No cards match your filters"
          suggestion="Try adjusting your search or filter criteria"
        />
      )}

      {/* Wishlist Cards */}
      {sortedCards.length > 0 && (
        <CardGridErrorBoundary>
          <CardGrid
            cards={sortedCards}
            context={cardContext}
          />
        </CardGridErrorBoundary>
      )}

      <BackToTopFab />

      {/* Mobile Filter Drawer */}
      {useMobileLayout && (
        <FilterDrawer
          open={filterDrawerOpen}
          onClose={handleFilterDrawerToggle}
          config={filterConfig}
          title="Filter Wishlist"
          activeFilterCount={activeFilterCount}
          onClear={handleClearFilters}
        />
      )}
    </Box>
  );
};

export default WishlistPage;
