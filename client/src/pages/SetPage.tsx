import React, { useState, useEffect, useCallback } from 'react';
import { useQuery } from '@apollo/client/react';
import { 
  Container, 
  Typography, 
  Box, 
  CircularProgress, 
  Alert,
  TextField,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Chip,
  Stack,
  Autocomplete,
  Fab,
  Zoom,
  Divider
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import { GET_CARDS_BY_SET_CODE } from '../graphql/queries/cards';
import { GET_SETS_BY_CODE } from '../graphql/queries/sets';
import { MtgCard } from '../components/organisms/MtgCard';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { CardGroup } from '../components/organisms/CardGroup';
import { DebouncedSearchInput } from '../components/atoms/shared/DebouncedSearchInput';
import { ResultsSummary } from '../components/atoms/shared/ResultsSummary';
import type { CardGroupConfig } from '../types/cardGroup';
import type { Card } from '../types/card';
import type { MtgSet } from '../types/set';

interface CardsResponse {
  cardsBySetCode: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

interface SetsResponse {
  setsByCode: {
    __typename: string;
    data?: MtgSet[];
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

const RARITIES = ['common', 'uncommon', 'rare', 'mythic', 'special', 'bonus'];

export const SetPage: React.FC = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const setCode = urlParams.get('set') || '';
  const initialSearch = urlParams.get('search') || '';
  const initialRarities = urlParams.get('rarities')?.split(',').filter(Boolean) || [];
  const initialArtists = urlParams.get('artists')?.split(',').filter(Boolean) || [];
  const initialSort = urlParams.get('sort') || 'collector-asc';
  const { loading: cardsLoading, error: cardsError, data: cardsData } = useQuery<CardsResponse>(GET_CARDS_BY_SET_CODE, {
    variables: { setCode: { setCode } },
    skip: !setCode
  });

  const { loading: setLoading, error: setError, data: setData } = useQuery<SetsResponse>(GET_SETS_BY_CODE, {
    variables: { codes: { setCodes: [setCode] } },
    skip: !setCode
  });

  const [searchTerm, setSearchTerm] = useState(initialSearch);
  const [selectedRarities, setSelectedRarities] = useState<string[]>(initialRarities);
  const [selectedArtists, setSelectedArtists] = useState<string[]>(initialArtists);
  const [selectedCardTypes, setSelectedCardTypes] = useState<string[]>([]);
  const [sortBy, setSortBy] = useState<string>(initialSort);
  const [filteredCards, setFilteredCards] = useState<Card[]>([]);
  const [cardGroups, setCardGroups] = useState<CardGroupConfig[]>([]);
  const [visibleGroupIds, setVisibleGroupIds] = useState<Set<string>>(new Set());
  const [showDigital, setShowDigital] = useState(false);
  const [showBackToTop, setShowBackToTop] = useState(false);
  const [selectedCardId, setSelectedCardId] = useState<string | null>(null);

  // Get unique artists from cards
  const getUniqueArtists = (cards: Card[]): string[] => {
    const artistSet = new Set<string>();
    cards.forEach(card => {
      if (card.artist) {
        // Split multiple artists (e.g., "Artist 1 & Artist 2")
        const artists = card.artist.split(/\s+(?:&|and)\s+/i);
        artists.forEach(artist => artistSet.add(artist.trim()));
      }
    });
    return Array.from(artistSet).sort((a, b) => a.toLowerCase().localeCompare(b.toLowerCase()));
  };
  
  // Format card type labels for display
  const formatCardTypeLabel = (type: string): string => {
    // Handle null/undefined
    if (!type) return 'Unknown';
    
    // Normalize the type for comparison (remove spaces, underscores, make uppercase)
    const normalizedType = type.replace(/[\s_-]/g, '').toUpperCase();
    
    // Special cases mapping - use normalized keys
    const specialCases: Record<string, string> = {
      'INBOOSTERS': 'In Boosters',
      'CARDVARIATIONS': 'Card Variations', 
      'FOILONLYBOOSTER': 'Foil Only Booster Cards',
      'PROMOCARDS': 'Promo Cards',
      'OTHERCARDS': 'Other Cards'
    };
    
    // Check normalized version
    if (specialCases[normalizedType]) {
      return specialCases[normalizedType];
    }
    
    // General formatting: replace underscores with spaces and capitalize words
    return type.replace(/[_-]/g, ' ').replace(/\b\w/g, c => c.toUpperCase());
  };
  
  // Determine which type group a card belongs to (matching the grouping logic)
  const getCardType = (card: Card): string => {
    // Check for foil-only booster cards (7th Edition specific)
    if (card.booster && card.foil && !card.nonFoil) {
      return 'FOIL_ONLY_BOOSTER';
    }
    // Check for variations
    else if (card.variation) {
      return 'CARD_VARIATIONS';
    }
    // Check if it's in boosters (this is the "regular" cards)
    else if (card.booster) {
      return 'IN_BOOSTERS';
    }
    // Check for promos
    else if (card.promo) {
      return 'PROMO_CARDS';
    }
    // Default for cards that don't fit other categories
    return 'OTHER_CARDS';
  };
  
  // Get unique card types from cards (includes promo types, boosters, variations, etc.)
  const getUniqueCardTypes = (cards: Card[]): string[] => {
    const cardTypeSet = new Set<string>();
    
    cards.forEach(card => {
      const cardType = getCardType(card);
      if (cardType !== 'OTHER_CARDS' || cards.some(c => getCardType(c) === 'OTHER_CARDS')) {
        // Only add OTHER_CARDS if at least one card actually belongs to it
        cardTypeSet.add(cardType);
      }
    });
    
    return Array.from(cardTypeSet)
      .sort((a, b) => formatCardTypeLabel(a).localeCompare(formatCardTypeLabel(b)));
  };


  // Handle scroll to show/hide back to top button
  useEffect(() => {
    const handleScroll = () => {
      setShowBackToTop(window.scrollY > 300);
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // Get the currently selected card by querying the DOM
  // This avoids React state updates on every selection change (which cause lag)
  // Instead, we query the DOM only when we actually need to know which card is selected
  const getSelectedCardId = useCallback(() => {
    const selectedCard = document.querySelector('[data-mtg-card="true"].selected');
    if (selectedCard) {
      return selectedCard.getAttribute('data-card-id');
    }
    return null;
  }, []);
  
  // Example: When adding to collection (to be implemented)
  // const handleAddToCollection = () => {
  //   const selectedId = getSelectedCardId();
  //   if (selectedId) {
  //     // Add card with selectedId to collection
  //   }
  // };
  
  // This is no longer used for click handling, but kept for potential future use
  const handleCardSelection = useCallback((cardId: string, selected: boolean) => {
    setSelectedCardId(selected ? cardId : null);
  }, []);

  useEffect(() => {
    const params = new URLSearchParams();
    params.set('page', 'set');
    params.set('set', setCode);
    
    if (searchTerm) {
      params.set('search', searchTerm);
    }
    if (selectedRarities.length > 0) {
      params.set('rarities', selectedRarities.join(','));
    }
    if (selectedArtists.length > 0) {
      params.set('artists', selectedArtists.join(','));
    }
    if (sortBy !== 'collector-asc') {
      params.set('sort', sortBy);
    }
    
    const newUrl = `${window.location.pathname}?${params.toString()}`;
    window.history.replaceState(null, '', newUrl);
  }, [setCode, searchTerm, selectedRarities, selectedArtists, sortBy]);

  useEffect(() => {
    if (cardsData?.cardsBySetCode?.data) {
      let filtered = [...cardsData.cardsBySetCode.data];

      if (!showDigital) {
        filtered = filtered.filter(card => !card.digital);
      }

      if (searchTerm) {
        filtered = filtered.filter(card => 
          card.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          card.oracleText?.toLowerCase().includes(searchTerm.toLowerCase()) ||
          card.typeLine?.toLowerCase().includes(searchTerm.toLowerCase()) ||
          card.artist?.toLowerCase().includes(searchTerm.toLowerCase())
        );
      }

      if (selectedRarities.length > 0) {
        filtered = filtered.filter(card => selectedRarities.includes(card.rarity));
      }

      if (selectedArtists.length > 0) {
        filtered = filtered.filter(card => {
          if (!card.artist) return false;
          // Split multiple artists and check if any match
          const cardArtists = card.artist.split(/\s+(?:&|and)\s+/i).map(a => a.trim());
          return cardArtists.some(artist => selectedArtists.includes(artist));
        });
      }
      
      // Note: Card type filtering is now handled by showing/hiding groups, not filtering cards

      const parseCollectorNumber = (num: string): number => {
        const match = num.match(/^(\d+)/);
        return match ? parseInt(match[1], 10) : 999999;
      };

      switch (sortBy) {
        case 'collector-asc':
          filtered.sort((a, b) => parseCollectorNumber(a.collectorNumber) - parseCollectorNumber(b.collectorNumber));
          break;
        case 'collector-desc':
          filtered.sort((a, b) => parseCollectorNumber(b.collectorNumber) - parseCollectorNumber(a.collectorNumber));
          break;
        case 'name-asc':
          filtered.sort((a, b) => a.name.localeCompare(b.name));
          break;
        case 'name-desc':
          filtered.sort((a, b) => b.name.localeCompare(a.name));
          break;
        case 'rarity':
          const rarityOrder: Record<string, number> = { common: 0, uncommon: 1, rare: 2, mythic: 3, special: 4, bonus: 5 };
          filtered.sort((a, b) => (rarityOrder[a.rarity] || 99) - (rarityOrder[b.rarity] || 99));
          break;
        case 'price-desc':
          filtered.sort((a, b) => {
            const priceA = parseFloat(a.prices?.usd || '0');
            const priceB = parseFloat(b.prices?.usd || '0');
            return priceB - priceA;
          });
          break;
        case 'price-asc':
          filtered.sort((a, b) => {
            const priceA = parseFloat(a.prices?.usd || '0');
            const priceB = parseFloat(b.prices?.usd || '0');
            return priceA - priceB;
          });
          break;
        case 'release-desc':
          filtered.sort((a, b) => {
            if (!a.releasedAt && !b.releasedAt) return 0;
            if (!a.releasedAt) return 1;
            if (!b.releasedAt) return -1;
            return new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime();
          });
          break;
        case 'release-asc':
          filtered.sort((a, b) => {
            if (!a.releasedAt && !b.releasedAt) return 0;
            if (!a.releasedAt) return 1;
            if (!b.releasedAt) return -1;
            return new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime();
          });
          break;
      }

      setFilteredCards(filtered);
      
      // Group cards by their characteristics
      const groupsMap = new Map<string, CardGroupConfig>();
      
      filtered.forEach(card => {
        const cardType = getCardType(card);
        const groupId = cardType;
        const displayName = formatCardTypeLabel(cardType);
        
        if (!groupsMap.has(groupId)) {
          groupsMap.set(groupId, {
            id: groupId,
            name: cardType,
            displayName: displayName,
            cards: [],
            isVisible: true,
            isFoilOnly: cardType === 'FOIL_ONLY_BOOSTER',
            isVariation: cardType === 'CARD_VARIATIONS',
            isBooster: cardType === 'IN_BOOSTERS',
            isPromo: cardType === 'PROMO_CARDS'
          });
        }
        
        groupsMap.get(groupId)!.cards.push(card);
      });
      
      // Convert to array and sort by display name
      const groupsArray = Array.from(groupsMap.values())
        .sort((a, b) => {
          // Put "In Boosters" first as it's the regular cards
          if (a.id === 'IN_BOOSTERS') return -1;
          if (b.id === 'IN_BOOSTERS') return 1;
          return a.displayName.localeCompare(b.displayName);
        });
      
      setCardGroups(groupsArray);
      
      // Initially all groups are visible
      const allGroupIds = new Set(groupsArray.map(g => g.id));
      setVisibleGroupIds(allGroupIds);
    }
    // Note: selectedCardId is intentionally removed from dependencies as it doesn't affect filtering/sorting
  }, [cardsData, searchTerm, selectedRarities, selectedArtists, sortBy, showDigital]);
  
  // Handle card type filtering by showing/hiding groups
  useEffect(() => {
    if (selectedCardTypes.length === 0) {
      // Show all groups if no filter selected
      setVisibleGroupIds(new Set(cardGroups.map(g => g.id)));
    } else {
      // Only show selected groups
      setVisibleGroupIds(new Set(selectedCardTypes));
    }
  }, [selectedCardTypes, cardGroups]);

  const handleRarityChange = (event: SelectChangeEvent<string[]>) => {
    const value = event.target.value;
    if (Array.isArray(value) && value.includes('CLEAR_ALL')) {
      setSelectedRarities([]);
      return;
    }
    setSelectedRarities(typeof value === 'string' ? value.split(',') : value);
  };

  const handleSortChange = (event: SelectChangeEvent) => {
    setSortBy(event.target.value);
  };

  if (!setCode) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          No set code provided. Please provide a set code in the URL (e.g., ?set=lea)
        </Alert>
      </Container>
    );
  }

  if (cardsLoading || setLoading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (cardsError) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          Error loading cards: {cardsError.message}
        </Alert>
      </Container>
    );
  }

  if (setError) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          Error loading set information: {setError.message}
        </Alert>
      </Container>
    );
  }

  if (cardsData?.cardsBySetCode?.__typename === 'FailureResponse') {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          {cardsData.cardsBySetCode.status?.message || 'Failed to load cards'}
        </Alert>
      </Container>
    );
  }

  const cards = cardsData?.cardsBySetCode?.data || [];
  const setInfo = setData?.setsByCode?.data?.[0];
  const setName = setInfo?.name || cards[0]?.setName || setCode.toUpperCase();
  const uniqueArtists = getUniqueArtists(cards);
  const uniqueCardTypes = getUniqueCardTypes(cards);
  
  // Check if all cards have the same release date
  const allSameReleaseDate = cards.length > 0 && 
    cards.every(card => card.releasedAt === cards[0].releasedAt);

  return (
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
      {/* Set Information Card */}
      {setInfo && (
        <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}>
          <MtgSetCard set={setInfo} />
        </Box>
      )}
      
      {/* Fallback title if no set info */}
      {!setInfo && (
        <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
          {setName} ({setCode.toUpperCase()})
        </Typography>
      )}

      <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}>
        <Stack spacing={2}>
          <Stack direction="row" spacing={2} flexWrap="wrap" sx={{ rowGap: 2 }}>
            <DebouncedSearchInput
              value={initialSearch}
              onChange={setSearchTerm}
              placeholder="Search cards..."
              debounceMs={1000}
              minWidth={300}
            />
            
            <FormControl sx={{ minWidth: 150 }}>
              <InputLabel>Rarity</InputLabel>
              <Select
                multiple
                value={selectedRarities}
                onChange={handleRarityChange}
                label="Rarity"
                renderValue={(selected) => {
                  if (selected.length === 0) {
                    return <Typography variant="body2" color="text.secondary">All Rarities</Typography>;
                  }
                  if (selected.length <= 2) {
                    return selected.map(r => r.charAt(0).toUpperCase() + r.slice(1)).join(', ');
                  }
                  return `${selected.length} selected`;
                }}
              >
                <MenuItem value="CLEAR_ALL" sx={{ borderBottom: '1px solid rgba(255,255,255,0.12)' }}>
                  <Typography variant="body2" color="text.secondary">Clear All</Typography>
                </MenuItem>
                {RARITIES.map(rarity => (
                  <MenuItem key={rarity} value={rarity}>
                    <Chip
                      size="small"
                      label={rarity.charAt(0).toUpperCase() + rarity.slice(1)}
                      color={selectedRarities.includes(rarity) ? "primary" : "default"}
                      sx={{ mr: 1 }}
                    />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <Autocomplete
              multiple
              sx={{ minWidth: 250 }}
              options={uniqueArtists}
              value={selectedArtists}
              onChange={(event, newValue) => {
                setSelectedArtists(newValue);
              }}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="Artist"
                  placeholder={selectedArtists.length === 0 ? "All Artists" : "Add more..."}
                  size="medium"
                />
              )}
              renderTags={(value, getTagProps) =>
                value.map((option, index) => {
                  const { key, ...chipProps } = getTagProps({ index });
                  return (
                    <Chip
                      key={key}
                      size="small"
                      label={option}
                      {...chipProps}
                    />
                  );
                })
              }
              renderOption={(props, option) => {
                const { key, ...otherProps } = props;
                return (
                  <Box key={key} component="li" {...otherProps}>
                    <Typography variant="body2">{option}</Typography>
                  </Box>
                );
              }}
              ListboxProps={{
                style: {
                  maxHeight: 300
                }
              }}
              disableCloseOnSelect
              filterSelectedOptions
              size="small"
            />
            
            {/* Card Type Filter - only show if there are more than 1 card type */}
            {uniqueCardTypes.length > 1 && (
              <Autocomplete
                multiple
                sx={{ minWidth: 250 }}
                options={uniqueCardTypes}
                value={selectedCardTypes}
                onChange={(event, newValue) => {
                  setSelectedCardTypes(newValue);
                }}
                getOptionLabel={(option) => formatCardTypeLabel(option)}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Card Type"
                    placeholder={selectedCardTypes.length === 0 ? "All Types" : "Add more..."}
                    size="medium"
                  />
                )}
                renderTags={(value, getTagProps) =>
                  value.map((option, index) => {
                    const { key, ...chipProps } = getTagProps({ index });
                    return (
                      <Chip
                        key={key}
                        size="small"
                        label={formatCardTypeLabel(option)}
                        {...chipProps}
                      />
                    );
                  })
                }
                renderOption={(props, option) => {
                  const { key, ...otherProps } = props;
                  return (
                    <Box key={key} component="li" {...otherProps}>
                      <Typography variant="body2">
                        {formatCardTypeLabel(option)}
                      </Typography>
                    </Box>
                  );
                }}
                ListboxProps={{
                  style: {
                    maxHeight: 300
                  }
                }}
                disableCloseOnSelect
                filterSelectedOptions
                size="small"
              />
            )}

            <FormControl sx={{ minWidth: 180 }}>
              <InputLabel>Sort By</InputLabel>
              <Select
                value={sortBy}
                onChange={handleSortChange}
                label="Sort By"
              >
                <MenuItem value="collector-asc">Collector # (Low-High)</MenuItem>
                <MenuItem value="collector-desc">Collector # (High-Low)</MenuItem>
                <MenuItem value="name-asc">Name (A-Z)</MenuItem>
                <MenuItem value="name-desc">Name (Z-A)</MenuItem>
                <MenuItem value="rarity">Rarity</MenuItem>
                <MenuItem value="price-desc">Price (High-Low)</MenuItem>
                <MenuItem value="price-asc">Price (Low-High)</MenuItem>
                {cards.some(c => c.releasedAt !== cards[0]?.releasedAt) && (
                  <>
                    <MenuItem value="release-desc">Release Date (Newest)</MenuItem>
                    <MenuItem value="release-asc">Release Date (Oldest)</MenuItem>
                  </>
                )}
              </Select>
            </FormControl>
          </Stack>
        </Stack>
      </Box>

      <ResultsSummary 
        showing={cardGroups
          .filter(g => visibleGroupIds.has(g.id))
          .reduce((sum, g) => sum + g.cards.length, 0)} 
        total={filteredCards.length} 
        itemType="cards"
        textAlign="center"
      />

      {/* Card Groups */}
      {cardGroups.map((group) => (
        <CardGroup
          key={group.id}
          groupId={group.id}
          groupName={group.displayName.toUpperCase()}
          cards={group.cards}
          isVisible={visibleGroupIds.has(group.id)}
          showHeader={cardGroups.length > 1}
          context={{
            isOnSetPage: true,
            currentSet: setCode,
            hideSetInfo: true,
            hideReleaseDate: allSameReleaseDate
          }}
          onCardSelection={handleCardSelection}
          selectedCardId={selectedCardId}
        />
      ))}

      {filteredCards.length === 0 && (
        <Box sx={{ mt: 4, textAlign: 'center' }}>
          <Typography variant="h6" color="text.secondary">
            No cards found matching your criteria
          </Typography>
        </Box>
      )}

      {/* Back to Top Button */}
      <Zoom in={showBackToTop}>
        <Fab
          color="primary"
          size="medium"
          onClick={scrollToTop}
          sx={{
            position: 'fixed',
            bottom: 16,
            right: 16,
            zIndex: 1000
          }}
          aria-label="scroll back to top"
        >
          <KeyboardArrowUpIcon />
        </Fab>
      </Zoom>
    </Container>
  );
};