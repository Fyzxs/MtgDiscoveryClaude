import React from 'react';
import { FormControlLabel, Switch } from '../../atoms';
import { FilterPanel } from './filters/FilterPanel';
import { FilterErrorBoundary } from '../ErrorBoundaries';
import { SET_PAGE_SORT_OPTIONS, SET_PAGE_COLLECTOR_SORT_OPTIONS } from '../../config/cardSortOptions';
import {
  getCollectionCountOptions,
  getSignedCardsOptions,
} from '../../utils/cardUtils';
import type { Card } from '../../types/card';

interface CardGroupConfig {
  id: string;
  displayName: string;
  cards: Card[];
}

interface SetPageFiltersProps {
  // Search state
  searchTerm: string;
  onSearchChange: (value: string) => void;

  // Sort state
  sortBy: string;
  onSortChange: (value: string) => void;

  // Filter state
  selectedRarities: string[];
  selectedArtists: string[];
  selectedGroupIds: string[];
  showGroups: boolean;
  onRarityChange: (value: string[]) => void;
  onArtistChange: (value: string[]) => void;
  onGroupChange: (value: string[]) => void;
  onShowGroupsChange: (value: boolean) => void;

  // Data for filter options
  allRarities: string[];
  allArtists: string[];
  allFinishes: string[];
  cardGroups: CardGroupConfig[];
  cards: Card[];

  // Collector-specific filters
  hasCollector: boolean;
  collectionCounts?: string[];
  signedCards?: string[];
  finishes?: string[];
  onCollectionCountsChange?: (value: string[]) => void;
  onSignedCardsChange?: (value: string[]) => void;
  onFinishesChange?: (value: string[]) => void;
}

export const SetPageFilters: React.FC<SetPageFiltersProps> = ({
  searchTerm,
  onSearchChange,
  sortBy,
  onSortChange,
  selectedRarities,
  selectedArtists,
  selectedGroupIds,
  showGroups,
  onRarityChange,
  onArtistChange,
  onGroupChange,
  onShowGroupsChange,
  allRarities,
  allArtists,
  allFinishes,
  cardGroups,
  cards,
  hasCollector,
  collectionCounts = [],
  signedCards = [],
  finishes = [],
  onCollectionCountsChange,
  onSignedCardsChange,
  onFinishesChange
}) => {
  // Only show filters if there are multiple cards
  if (cards.length <= 1) {
    return null;
  }

  return (
    <FilterErrorBoundary name="SetPageFilters">
      <FilterPanel
        config={{
          search: {
            value: searchTerm,
            onChange: onSearchChange,
            placeholder: 'Search cards...',
            debounceMs: 300,
            minWidth: 300
          },
          multiSelects: [
            // Card Groups (if multiple groups exist)
            ...(cardGroups.filter(g => g.cards.length > 0).length > 1 && showGroups !== false ? [{
              key: 'cardGroups',
              value: selectedGroupIds,
              onChange: onGroupChange,
              options: cardGroups
                .filter(g => g.cards.length > 0) // Only show groups with cards
                .map(g => ({
                  value: g.id,
                  label: `${g.displayName} (${g.cards.length})`
                })),
              label: 'Card Group',
              placeholder: 'All Groups',
              minWidth: 200
            }] : []),
            // Rarities filter
            ...(allRarities.length > 1 ? [{
              key: 'rarities',
              value: selectedRarities,
              onChange: onRarityChange,
              options: allRarities.map(rarity => ({ value: rarity, label: rarity })),
              label: 'Rarity',
              placeholder: 'All Rarities',
              minWidth: 150
            }] : []),
            // Artists filter
            ...(allArtists.length > 1 ? [{
              key: 'artists',
              value: selectedArtists,
              onChange: onArtistChange,
              options: allArtists.map(artist => ({ value: artist, label: artist })),
              label: 'Artist',
              placeholder: 'All Artists',
              minWidth: 200
            }] : []),
            // Finishes filter (general filter, shown for all users)
            ...(allFinishes.length > 1 ? [{
              key: 'finishes',
              value: finishes,
              onChange: onFinishesChange || (() => {}),
              options: allFinishes.map(finish => ({ value: finish, label: finish })),
              label: 'Finish',
              placeholder: 'All Finishes',
              minWidth: 150
            }] : [])
          ],
          sort: {
            value: sortBy,
            onChange: onSortChange,
            options: (hasCollector ? SET_PAGE_COLLECTOR_SORT_OPTIONS : SET_PAGE_SORT_OPTIONS).map(opt => {
              // Add conditional display for release date options
              if (opt.value === 'release-desc' || opt.value === 'release-asc') {
                return {
                  ...opt,
                  condition: cards.some(c => c.releasedAt !== cards[0]?.releasedAt)
                };
              }
              return opt;
            }),
            minWidth: 200
          },
          customFilters: cardGroups.filter(g => g.cards.length > 0).length > 1 ? [
            <FormControlLabel
              key="show-groups-toggle"
              control={
                <Switch
                  checked={showGroups !== false}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => onShowGroupsChange(e.target.checked)}
                  size="small"
                />
              }
              label="Show Card Groups"
              sx={{ minWidth: 150 }}
            />
          ] : [],
          collectorFilters: hasCollector ? {
            collectionCounts: {
              key: 'collectionCounts',
              value: collectionCounts,
              onChange: onCollectionCountsChange || (() => {}),
              options: getCollectionCountOptions(),
              label: 'Collection Count',
              placeholder: 'All Counts',
              minWidth: 180
            },
            signedCards: {
              key: 'signedCards',
              value: signedCards,
              onChange: onSignedCardsChange || (() => {}),
              options: getSignedCardsOptions(),
              label: 'Signed Cards',
              placeholder: 'All Cards',
              minWidth: 150
            }
          } : undefined
        }}
        layout="compact"
        sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}
      />
    </FilterErrorBoundary>
  );
};