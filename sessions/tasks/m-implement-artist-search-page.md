---
task: m-implement-artist-search-page
branch: feature/implement-artist-search-page
status: pending
created: 2025-01-14
modules: [client/pages, client/components, client/hooks, client/graphql]
---

# Implement Artist Search Page

## Problem/Goal
Create a dedicated artist search page that allows users to search for and browse Magic: The Gathering cards by artist. This page should provide a comprehensive artist-focused search experience with filtering, sorting, and detailed results display.

## Success Criteria
- [ ] Artist search page accessible at `/search/artists` route
- [ ] Search input with autocomplete for artist names
- [ ] Artist profile display with basic info and card count
- [ ] Grid view of all cards by selected artist
- [ ] Filtering options (sets, rarity, card type, etc.)
- [ ] Sorting options (release date, name, etc.)
- [ ] Responsive design matching existing page patterns
- [ ] Loading states and error handling
- [ ] Artist link integration from existing card components
- [ ] Navigation integration in header/menu
- [ ] Accessibility features (ARIA labels, keyboard navigation)
- [ ] Comprehensive error boundaries and network error handling

## Context Manifest

### How This Currently Works: Page Architecture and Artist Integration System

**Page Architecture Pattern:**
When implementing pages in this React application, they follow a consistent architectural pattern established by AllSetsPage, SetPage, and CardDetailPage. Each page uses a layered structure: the main page component handles routing parameters via useParams, manages state through custom hooks (useFilterState, useUrlFilterState), executes GraphQL queries with Apollo Client's useQuery hook, and wraps everything in error boundaries for resilience.

The data flow starts with URL parameters being parsed and synchronized with component state. GraphQL queries execute automatically based on route parameters, returning structured responses that follow a union type pattern (SuccessResponse | FailureResponse). The FilterPanel component provides unified filtering/searching/sorting controls, while ResponsiveGrid layouts display results. Error boundaries at multiple levels (page, section, component) catch and handle failures gracefully.

**Current Artist Integration:**
Artist functionality is already partially implemented throughout the application. The Card data structure includes `artist: string` and `artistIds: string[]` fields from GraphQL. Artist parsing logic in cardUtils.ts splits multiple artists using the pattern `/\s+(?:&|and)\s+/i`, handling cases like "Artist 1 & Artist 2". The ArtistLink component creates navigable links to `/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`, while ArtistInfo displays artist information with context awareness (hiding current artist on artist pages).

Filter systems already support artist filtering through the createCardFilterFunctions utility, which performs case-insensitive matching against selected artists. The useCardFiltering hook includes getUniqueArtists functionality to extract all unique artist names from card collections. ArtistLinks component handles multiple artist display with proper ampersand separation.

**Navigation and URL Structure:**
The Header component contains a "Search" dropdown menu that currently shows "Artists (Coming Soon)" as disabled. The application uses React Router with nested routing: main routes in App.tsx include `/sets`, `/set/:setCode`, `/search/cards`, and `/card/:cardName`. The artist links already generate URLs following the pattern `/artists/artist-name-slug`, expecting an ArtistSearchPage to handle the `/artists/:artistName` route.

URL state management is handled by useUrlState and useUrlFilterState hooks, which synchronize filter states with query parameters. Search terms get debounced (300ms), while filters sync immediately. The pattern supports complex state like `?search=term&rarities=common,rare&artists=artist1,artist2&sort=name-asc`.

### For New Artist Search Page Implementation: Integration Points and Patterns

**GraphQL Integration Requirements:**
Currently, there are no artist-specific GraphQL queries. The new page will likely need a `GET_CARDS_BY_ARTIST` query similar to existing `GET_CARDS_BY_NAME` or `GET_CARDS_BY_SET_CODE`. The query should return the same Card data structure used throughout the application, ensuring compatibility with existing components like CardCompact, MtgCard, and filtering systems.

Artist search functionality may also benefit from an artist autocomplete/search query for the search input, similar to the existing `CARD_NAME_SEARCH` query. This would enable responsive artist name suggestions as users type.

**State Management Integration:**
The new ArtistSearchPage should follow the established pattern using useCardFiltering or useUrlFilterState hooks. The page will need to extract the artist name from useParams, handle URL decoding (`decodeURIComponent`), and execute artist-based card queries. Filter state should include all existing card filters (sets, rarities, digital cards) while excluding artist filtering when viewing a specific artist's cards.

URL state synchronization should follow the same pattern as SetPage: immediate sync for dropdowns, debounced sync for search (300ms). The artist context should be passed to card components to hide redundant artist information when viewing cards by a specific artist.

**Component Architecture Integration:**
The page should use the same component hierarchy: Container → FilterPanel → ResultsSummary → ResponsiveGrid → CardCompact/MtgCard. Error boundaries should wrap major sections (FilterErrorBoundary, CardGridErrorBoundary). The FilterPanel should exclude artist filtering but include all other standard filters.

Card components should receive a context prop indicating they're on an artist page: `context={{ isOnArtistPage: true, currentArtist: artistName }}`. This enables ArtistInfo component to hide redundant artist display or show collaborating artists only.

**Header Navigation Integration:**
The Header component's search dropdown needs updating to enable the "Artists" menu item and navigate to `/search/artists` (a general artist search) or handle direct artist links. The existing `handleSearchMenuClick` function can be extended to support artist search routes.

Consider whether the page should support both `/artists/:artistName` (direct artist links) and `/search/artists` (general artist search with autocomplete). The latter would provide a search interface similar to CardSearchPage but for artists.

### Technical Reference Details

#### Required Route Addition
```typescript
// Add to App.tsx Routes
<Route path="/artists/:artistName" element={
  <PageErrorBoundary name="ArtistSearchPage">
    <ArtistSearchPage />
  </PageErrorBoundary>
} />
```

#### Artist Data Structure (from existing GraphQL schema)
```typescript
interface Card {
  artist: string;           // "Artist Name" or "Artist 1 & Artist 2"
  artistIds: string[];      // Array of artist IDs
  // ... all other card fields
}
```

#### Required GraphQL Query Pattern
```graphql
query GetCardsByArtist($artistName: ArtistNameArgEntityInput!) {
  cardsByArtist(artistName: $artistName) {
    __typename
    ... on SuccessCardsResponse {
      data { /* same card fields as existing queries */ }
    }
    ... on FailureResponse {
      status { message }
    }
  }
}
```

#### Hook Usage Pattern
```typescript
const { 
  filteredCards, 
  uniqueRarities, 
  uniqueSets,
  sortBy,
  filters,
  setSortBy,
  updateFilter 
} = useCardFiltering(cards, {
  defaultSort: 'release-desc',
  includeSets: true,  // Enable set filtering for artist pages
  initialFilters: { showDigital: false }
});
```

#### Component Configuration
```typescript
const filterConfig: FilterPanelConfig = {
  search: {
    value: searchTerm,
    onChange: setSearchTerm,
    placeholder: 'Search cards by this artist...'
  },
  autocompletes: [
    // Rarities and Sets, but NOT Artists
    { key: 'rarities', /* config */ },
    { key: 'sets', /* config */ }
  ],
  sort: { /* standard card sort options */ }
};

const cardContext: CardContext = {
  isOnArtistPage: true,
  currentArtist: decodedArtistName,
  hideArtistInfo: false  // Show collaborating artists
};
```

#### File Locations
- **Implementation**: `/mnt/d/src/MtgDiscoveryVibeWorkspace/WebMtgSet/client/src/pages/ArtistSearchPage.tsx`
- **GraphQL Queries**: `/mnt/d/src/MtgDiscoveryVibeWorkspace/WebMtgSet/client/src/graphql/queries/artists.ts` (new file)
- **Route Integration**: `/mnt/d/src/MtgDiscoveryVibeWorkspace/WebMtgSet/client/src/App.tsx`
- **Header Update**: `/mnt/d/src/MtgDiscoveryVibeWorkspace/WebMtgSet/client/src/components/organisms/Header.tsx`
- **Tests**: Following project patterns, create test files alongside components

#### Error Handling Requirements
The page must implement the comprehensive error handling pattern recently added to CardDetailPage:
- Network error handling with userFriendlyError state
- GraphQL error detection and retry mechanisms  
- Loading state management with globalLoadingManager
- Proper error boundary integration at multiple levels
- User-friendly error messages with retry functionality

## User Notes
**KEY REQUIREMENT: This search page should be very similar to the card search page**

- **Primary Pattern**: Copy and adapt CardSearchPage structure as the main template
- Use identical layout, filtering panel, and result display patterns
- Replace card name search with artist name search functionality
- Leverage existing useCardFiltering hook with artist-specific queries
- Keep same navigation, sorting, and pagination patterns
- Reuse CardFilterPanel, ResultsSummary, and ResponsiveGridAutoFit components
- Maintain identical styling and responsive behavior
- Only difference: search by artist instead of card name

## Work Log
<!-- Updated as work progresses -->
- [2025-01-14] Created task for artist search page implementation