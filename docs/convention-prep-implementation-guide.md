# Convention Prep Feature - Implementation Guide

## Feature Overview

**Purpose:** Help users prepare for MTG conventions by identifying which card sets to pull from their collection based on which artists will be attending.

**User Workflow:**
1. Search for artists attending the convention
2. Add artists to a selected list
3. Generate a list of sets sorted by number of attending artists
4. Click sets to view cards filtered by all selected artists

**Key Insight:** Users store cards in boxes by set, so they want to minimize re-pulling the same sets. Sorting by artist count helps them prioritize which sets to pull first.

---

## Route & URL Structure

### Primary Route
**Path:** `/convention-prep`

**Query Parameters:**
- `artists` - Comma-separated, URL-encoded artist names
- Format: `?artists=Dave+Rapoza%2CDavid+Baldeon%2CEd+Hannigan`
- Encoding: Comma as `%2C`, space as `+`

**Example URLs:**
```
/convention-prep
/convention-prep?artists=Mark+Tedin
/convention-prep?artists=Mark+Tedin%2CMark+Poole%2CRebecca+Guay
```

### Navigation Target
When clicking a set, navigate to:
```
/set/{setCode}?artistFilter={Artist1}&artistFilter={Artist2}&artistFilter={Artist3}...
```

**Example:**
```
/set/fdn?artistFilter=Mark+Tedin&artistFilter=Mark+Poole&artistFilter=Rebecca+Guay
```

**Important:** Include ALL selected artists in the filter, even if they don't have cards in that specific set.

---

## Component Architecture

### Page Component
**File:** `client/src/pages/ConventionPrepPage.tsx`

**Responsibilities:**
- URL state management (read/write artists query param)
- Coordinate child components
- Handle navigation

### Child Components

#### 1. ArtistSearchSection
**File:** `client/src/components/organisms/ConventionPrep/ArtistSearchSection.tsx`

**Props:**
```typescript
interface ArtistSearchSectionProps {
  onArtistAdd: (artistName: string) => void;
  selectedArtists: string[];
}
```

**Responsibilities:**
- Search input with debounced query
- Display search results as clickable badges with (+) indicator
- Handle artist selection (click anywhere on badge)
- Empty state: "Search for artists to add to your convention list"

**Behavior:**
- Search results remain visible after adding artist
- No auto-clear of search input
- Visual indicator (+) on badges
- Search uses existing artist search functionality
- Clicking anywhere on badge adds artist (not just the +)
- Can search for partial names to add multiple artists (e.g., "Mark" → Mark Tedin, Mark Poole)

#### 2. SelectedArtistsSection
**File:** `client/src/components/organisms/ConventionPrep/SelectedArtistsSection.tsx`

**Props:**
```typescript
interface SelectedArtistsSectionProps {
  artists: string[];
  onArtistRemove: (artistName: string) => void;
}
```

**Responsibilities:**
- Display selected artists as chips/pills with X button
- Handle artist removal on click
- Empty state: "No artists selected. Search and add artists above to begin."
- Grow to fit all artists (no scrolling/height limit)

**UI Specs:**
- Header: "Selected Artists ({count})"
- Artist chips: Material-UI `Chip` component with `onDelete` prop
- Layout: Flex wrap to handle multiple rows
- No "Clear All" button
- Always visible (even when empty)

#### 3. GenerateSetListButton
**File:** `client/src/components/atoms/ConventionPrep/GenerateSetListButton.tsx`

**Props:**
```typescript
interface GenerateSetListButtonProps {
  disabled: boolean;
  loading: boolean;
  onClick: () => void;
}
```

**States:**
- Disabled when `selectedArtists.length === 0`
- Loading state shows spinner
- Disabled during loading

**Behavior:**
- If user adds more artists after generating, must click Generate again to refresh results
- Does NOT auto-update set list when artists added/removed

#### 4. SetResultsList
**File:** `client/src/components/organisms/ConventionPrep/SetResultsList.tsx`

**Props:**
```typescript
interface SetResultsListProps {
  sets: ConventionPrepSet[];
  loading: boolean;
  error?: string;
  onSetClick: (setCode: string) => void;
  selectedArtists: string[];
}

interface ConventionPrepSet {
  setCode: string;
  setName: string;
  artistCount: number;
  cardCount: number;
}
```

**Responsibilities:**
- Display sets sorted by artist count (desc), then card count (desc)
- Format: `Set Name [X artists, Y cards]`
- Handle set click → navigate to set page with artist filters
- Loading state (spinner)
- Error state with retry option
- Empty state: "No sets found with cards by the selected artists"

**UI Specs:**
- List item format: `{setName} [{artistCount} artists, {cardCount} cards]`
- Clickable list items (Material-UI `ListItem` with button behavior)
- Loading: Show spinner overlay
- Error: Alert box with error message and "Retry" button

---

## State Management

### Page-Level State

**File:** `client/src/pages/ConventionPrepPage.tsx`

```typescript
const ConventionPrepPage: React.FC = () => {
  // URL state - sync with query params
  const [searchParams, setSearchParams] = useSearchParams();

  // Parse artists from URL on mount
  const [selectedArtists, setSelectedArtists] = useState<string[]>(() => {
    const artistsParam = searchParams.get('artists');
    return artistsParam ? artistsParam.split(',').map(decodeURIComponent) : [];
  });

  // Set results state
  const [sets, setSets] = useState<ConventionPrepSet[]>([]);
  const [setsLoading, setSetsLoading] = useState(false);
  const [setsError, setSetsError] = useState<string | null>(null);

  // Sync selectedArtists to URL
  useEffect(() => {
    if (selectedArtists.length > 0) {
      const encodedArtists = selectedArtists.map(encodeURIComponent).join(',');
      setSearchParams({ artists: encodedArtists });
    } else {
      setSearchParams({});
    }
  }, [selectedArtists, setSearchParams]);

  // Handler functions...
};
```

### URL Encoding/Decoding

**Helper Functions:**
```typescript
// Encode artists for URL
const encodeArtistsForUrl = (artists: string[]): string => {
  return artists.map(encodeURIComponent).join(',');
};

// Decode artists from URL
const decodeArtistsFromUrl = (artistsParam: string | null): string[] => {
  if (!artistsParam) return [];
  return artistsParam.split(',').map(decodeURIComponent);
};
```

---

## Data Fetching Strategy

### Client-Side Aggregation (Recommended for MVP)

**Approach:**
1. For each selected artist, fetch all sets containing their cards
2. Aggregate on client to count artists per set
3. Calculate card counts per set
4. Sort results

**Implementation:**
```typescript
const generateSetList = async () => {
  setSetsLoading(true);
  setSetsError(null);

  try {
    // Fetch cards for all selected artists
    const artistCardsPromises = selectedArtists.map(artistName =>
      apolloClient.query({
        query: GET_CARDS_BY_ARTIST,
        variables: { artistName }
      })
    );

    const results = await Promise.all(artistCardsPromises);

    // Aggregate by set
    const setMap = new Map<string, {
      setCode: string;
      setName: string;
      artistNames: Set<string>;
      cardIds: Set<string>;
    }>();

    results.forEach((result, index) => {
      const artistName = selectedArtists[index];
      const cards = result.data.cardsByArtist.data.cards;

      cards.forEach(card => {
        if (!setMap.has(card.setCode)) {
          setMap.set(card.setCode, {
            setCode: card.setCode,
            setName: card.setName,
            artistNames: new Set(),
            cardIds: new Set()
          });
        }

        const setData = setMap.get(card.setCode)!;
        setData.artistNames.add(artistName);
        setData.cardIds.add(card.id);
      });
    });

    // Convert to array and calculate counts
    const setsArray = Array.from(setMap.values()).map(set => ({
      setCode: set.setCode,
      setName: set.setName,
      artistCount: set.artistNames.size,
      cardCount: set.cardIds.size
    }));

    // Sort: primary by artist count (desc), secondary by card count (desc)
    setsArray.sort((a, b) => {
      if (b.artistCount !== a.artistCount) {
        return b.artistCount - a.artistCount;
      }
      return b.cardCount - a.cardCount;
    });

    setSets(setsArray);
  } catch (error) {
    setSetsError('Failed to generate set list. Please try again.');
    console.error('Error generating set list:', error);
  } finally {
    setSetsLoading(false);
  }
};
```

### Future Backend Optimization

**New GraphQL Query (Post-MVP):**
```graphql
query GetSetsByArtists($artistNames: [String!]!) {
  setsByArtists(artistNames: $artistNames) {
    setCode
    setName
    artistCount
    cardCount
  }
}
```

**Backend Implementation Notes:**
- Aggregate cards by set across all provided artists
- Count unique artists per set
- Count unique cards per set
- Sort server-side by artist count (desc), card count (desc)

---

## GraphQL Queries

### Existing Query to Use
```graphql
query GetCardsByArtist($artistName: String!) {
  cardsByArtist(artistName: $artistName) {
    ... on SuccessDataResponseModel {
      data {
        cards {
          id
          name
          setCode
          setName
        }
      }
    }
    ... on FailureResponseModel {
      message
    }
  }
}
```

### Artist Search Query
Use existing artist search functionality from `/search/artists` page.

---

## Navigation & Routing

### Route Configuration

**File:** `client/src/App.tsx`

Add route:
```typescript
<Route
  path="/convention-prep"
  element={
    <PageErrorBoundary name="ConventionPrepPage">
      <ConventionPrepPage />
    </PageErrorBoundary>
  }
/>
```

### Navigation Menu

**File:** `client/src/components/organisms/Header.tsx`

Add menu item:
```typescript
<MenuItem onClick={() => navigate('/convention-prep')}>
  Convention Prep
</MenuItem>
```

### Set Click Navigation

```typescript
const handleSetClick = (setCode: string) => {
  const artistFilters = selectedArtists
    .map(artist => `artistFilter=${encodeURIComponent(artist)}`)
    .join('&');

  navigate(`/set/${setCode}?${artistFilters}`);
};
```

---

## UI/UX Specifications

### Layout Structure

```
┌───────────────────────────────────────────────────┐
│  Convention Prep                                  │
├───────────────────────────────────────────────────┤
│                                                   │
│  Artist Search                                    │
│  ┌─────────────────────────────────────────────┐ │
│  │ [Search for artists...]              [🔍]  │ │
│  └─────────────────────────────────────────────┘ │
│                                                   │
│  Search Results:                                  │
│  [Artist A (+)] [Artist B (+)] [Artist C (+)]     │
│                                                   │
│  ─────────────────────────────────────────────   │
│                                                   │
│  Selected Artists (3)                             │
│  ┌─────────────────────────────────────────────┐ │
│  │ [Artist A ✕] [Artist B ✕] [Artist C ✕]      │ │
│  └─────────────────────────────────────────────┘ │
│                                                   │
│  [Generate Set List]                              │
│                                                   │
│  ─────────────────────────────────────────────   │
│                                                   │
│  Sets with Selected Artists                       │
│  • Foundations [3 artists, 12 cards]              │
│  • Wilds of Eldraine [2 artists, 8 cards]         │
│  • March of the Machine [2 artists, 5 cards]      │
│  • Phyrexia: All Will Be One [1 artist, 3 cards]  │
│                                                   │
└───────────────────────────────────────────────────┘
```

### Material-UI Components

**Artist Search:**
- `TextField` with `InputAdornment` for search icon
- `Chip` components for search results with custom `icon` prop showing "+"
- `Box` with flex layout for results

**Selected Artists:**
- `Typography` variant="h6" for section header
- `Box` with flexWrap for chip container
- `Chip` with `onDelete` for each artist

**Generate Button:**
- `Button` variant="contained" color="primary"
- `CircularProgress` size={20} for loading state

**Set Results:**
- `List` and `ListItem` components
- `ListItemButton` for clickable items
- `ListItemText` with primary text formatted as specified

### Spacing & Layout
- Use Material-UI `spacing` units (theme.spacing())
- Section spacing: 3-4 units between major sections
- Chip spacing: 1 unit gap in flex layout
- Container: `maxWidth="lg"` centered

### Colors & Typography
- Follow existing theme
- Artist count badge: Use theme primary color
- Card count: Use theme text.secondary color
- Error messages: theme.palette.error.main

---

## Error Handling

### Error States

**1. Network Error (Set Generation)**
```typescript
if (error) {
  return (
    <Alert severity="error" action={
      <Button color="inherit" size="small" onClick={handleRetry}>
        Retry
      </Button>
    }>
      Failed to generate set list. Please try again.
    </Alert>
  );
}
```

**2. No Sets Found**
```typescript
if (sets.length === 0 && !loading) {
  return (
    <Box sx={{ textAlign: 'center', py: 4 }}>
      <Typography variant="body1" color="text.secondary">
        No sets found with cards by the selected artists.
      </Typography>
    </Box>
  );
}
```

**3. Invalid URL Artists**
- Gracefully handle malformed URL params
- Filter out empty strings from artist array
- Show warning if URL contained invalid data

### Loading States

**Set Generation Loading:**
```typescript
{loading && (
  <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
    <CircularProgress />
  </Box>
)}
```

---

## TypeScript Types

### Create Types File
**File:** `client/src/types/conventionPrep.ts`

```typescript
export interface ConventionPrepSet {
  setCode: string;
  setName: string;
  artistCount: number;
  cardCount: number;
}

export interface ArtistSearchResult {
  name: string;
  cardCount?: number;
}
```

---

## Testing Considerations

### Unit Tests
- URL encoding/decoding helpers
- Set aggregation logic
- Sorting logic (artist count primary, card count secondary)

### Integration Tests
- Add artist → updates URL
- Remove artist → updates URL
- Load page with artists in URL → populates selected artists
- Generate set list → fetches and displays results

### E2E Tests
- Full workflow: search → add → generate → click set → verify navigation
- Multiple artists scenario
- Empty states
- Error states

---

## Implementation Steps

### Phase 1: Basic Page Structure
1. Create route `/convention-prep`
2. Create `ConventionPrepPage.tsx` with basic layout
3. Implement URL state management for artists
4. Add to navigation menu

### Phase 2: Artist Selection
5. Create `ArtistSearchSection` component
6. Integrate existing artist search functionality
7. Implement add artist handler
8. Create `SelectedArtistsSection` component
9. Implement remove artist handler
10. Test URL sync with browser back/forward

### Phase 3: Set Generation
11. Create `GenerateSetListButton` component
12. Implement client-side aggregation logic
13. Create `SetResultsList` component
14. Implement sorting (artist count, then card count)
15. Add loading and error states

### Phase 4: Navigation
16. Implement set click navigation with artist filters
17. Verify set page receives and applies all artist filters
18. Test navigation flow end-to-end

### Phase 5: Polish
19. Add empty states for all sections
20. Refine loading states and transitions
21. Add error handling and retry logic
22. Responsive design adjustments
23. Accessibility review (keyboard navigation, ARIA labels)

### Phase 6: Testing
24. Write unit tests
25. Write integration tests
26. Manual testing across browsers
27. Performance testing with large artist lists

---

## Future Enhancements (Post-MVP)

### Backend Optimization
- Add dedicated GraphQL query for set aggregation
- Server-side sorting
- Caching layer for convention prep queries

### Additional Features
- Save/name convention events ("GP Vegas 2025")
- Share convention prep URLs with friends
- Export set list to printable format
- Integration with collection tracking (show owned vs. not owned)
- Estimated value of cards to bring
- Filter sets by format (Standard, Modern, etc.)

### UX Improvements
- Drag-and-drop artist ordering
- Bulk artist import (paste list)
- Auto-complete in artist search
- Recent conventions history
- Mobile-optimized view

---

## Files to Create/Modify

### New Files
```
client/src/pages/ConventionPrepPage.tsx
client/src/components/organisms/ConventionPrep/ArtistSearchSection.tsx
client/src/components/organisms/ConventionPrep/SelectedArtistsSection.tsx
client/src/components/organisms/ConventionPrep/SetResultsList.tsx
client/src/components/atoms/ConventionPrep/GenerateSetListButton.tsx
client/src/types/conventionPrep.ts
client/src/utils/conventionPrepHelpers.ts
```

### Modified Files
```
client/src/App.tsx (add route)
client/src/components/organisms/Header.tsx (add nav item)
```

---

## Acceptance Criteria

- [ ] User can search for artists and add them to selected list
- [ ] Selected artists persist in URL
- [ ] Page loads with artists from URL
- [ ] User can remove artists from selected list
- [ ] Generate button disabled when no artists selected
- [ ] Generate button shows loading state
- [ ] Set list displays with correct format: `Set Name [X artists, Y cards]`
- [ ] Sets sorted by artist count (desc), then card count (desc)
- [ ] Clicking set navigates to set page with all selected artists as filters
- [ ] All empty states display correctly
- [ ] Error states display with retry option
- [ ] URL can be bookmarked and shared
- [ ] Feature works on mobile/tablet/desktop
- [ ] Keyboard navigation works
- [ ] Screen reader accessible

---

## Estimated Implementation Effort

**Total: 8-12 hours**
- Phase 1-2: 3-4 hours
- Phase 3-4: 3-4 hours
- Phase 5-6: 2-4 hours

---

## Notes

- **Card Count Definition:** "Unique cards" and "total printings" are the same in this system - we list by unique card ID, not by card name
- **Artist Filter Compatibility:** The set page already supports multiple artist filters via URL params
- **No Re-generation:** User must manually click "Generate Set List" again if artists are added/removed after initial generation
