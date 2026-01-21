import React from 'react';
import { CardGroup } from './CardGroup';
import { SearchEmptyState } from '../molecules/shared/EmptyState';
import { CardGridErrorBoundary } from '../ErrorBoundaries';
import type { Card } from '../../types/card';
import type { MtgSet } from '../../types/set';

interface CardGroupConfig {
  id: string;
  displayName: string;
  cards: Card[];
  totalCards: number;
}

interface CardContext {
  isOnSetPage: boolean;
  currentSet?: string;
  hideSetInfo: boolean;
  hideReleaseDate: boolean;
  hasCollector: boolean;
}

interface SetPageCardDisplayProps {
  // Loading state
  cardsLoading: boolean;

  // Data
  sortedCards: Card[];
  filteredCards: Card[];
  cardGroups: CardGroupConfig[];
  setInfo?: MtgSet;

  // Display state
  showGroups: boolean;
  visibleGroupIds: Set<string>;
  allSameReleaseDate: boolean;

  // Context
  setCode: string;
  hasCollector: boolean;

  // Handlers
  onClearFilters: () => void;
}

export const SetPageCardDisplay: React.FC<SetPageCardDisplayProps> = ({
  cardsLoading,
  sortedCards,
  filteredCards,
  cardGroups,
  setInfo,
  showGroups,
  visibleGroupIds,
  allSameReleaseDate,
  setCode,
  hasCollector,
  onClearFilters
}) => {
  const cardContext: CardContext = {
    isOnSetPage: true,
    currentSet: setCode,
    hideSetInfo: true,
    hideReleaseDate: allSameReleaseDate,
    hasCollector
  };

  return (
    <CardGridErrorBoundary name="SetPageCardGroups">
      {cardsLoading && (
        <>
          {/* If we have setInfo with groupings, show loading skeleton for each group */}
          {setInfo?.groupings && setInfo.groupings.length > 0 ? (
            setInfo.groupings.map((grouping) => (
              <CardGroup
                key={`loading-${grouping.id}`}
                groupId={grouping.id}
                groupName={grouping.displayName.toUpperCase()}
                cards={[]}
                isVisible={true}
                showHeader={true}
                isLoading={true}
                context={cardContext}
              />
            ))
          ) : (
            /* Otherwise show a single loading group */
            <CardGroup
              key="loading-default"
              groupId="default-cards"
              groupName="LOADING CARDS"
              cards={[]}
              isVisible={true}
              showHeader={false}
              isLoading={true}
              context={cardContext}
            />
          )}
        </>
      )}

      {!cardsLoading && (
        showGroups === false ? (
          // Flat display: show all sorted cards in a single group
          <CardGroup
            key="all-cards"
            groupId="all-cards"
            groupName="ALL CARDS"
            cards={sortedCards}
            isVisible={true}
            showHeader={false}
            context={cardContext}
          />
        ) : (
          // Grouped display: show cards organized by groups (filter out empty groups)
          cardGroups
            .filter(group => group.cards.length > 0) // Only show groups with cards
            .map((group) => (
              <CardGroup
                key={group.id}
                groupId={group.id}
                groupName={group.displayName.toUpperCase()}
                cards={group.cards}
                totalCards={group.totalCards}
                isVisible={visibleGroupIds.has(group.id)}
                showHeader={cardGroups.length > 1}
                context={cardContext}
              />
            ))
        )
      )}

      {filteredCards.length === 0 && (
        <SearchEmptyState
          itemType="cards"
          onClear={onClearFilters}
        />
      )}
    </CardGridErrorBoundary>
  );
};