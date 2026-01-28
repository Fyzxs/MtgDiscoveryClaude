# Research: Homepage Redesign

**Feature**: 003-homepage-redesign | **Date**: 2026-01-27

## Research Tasks

### 1. Carousel Implementation Approach

**Decision**: CSS scroll-snap with native scrolling (no third-party carousel library)

**Rationale**: The project has a constraint of no new npm dependencies where possible, and Material-UI provides all the building blocks needed. CSS `scroll-snap-type` with `scroll-snap-align` provides smooth, native-feeling horizontal scrolling on both mobile (touch swipe) and desktop. Arrow buttons for desktop are simple IconButton overlays.

**Alternatives Considered**:
- **Swiper.js** - Feature-rich but adds ~45KB bundle weight and introduces a non-MUI styling dependency. Rejected for bundle size and styling conflict.
- **react-slick** - Popular but dated, requires separate CSS file, and has known accessibility issues. Rejected.
- **Embla Carousel** - Lightweight and modern, but still an external dependency when CSS scroll-snap handles the requirements (no autoplay, no infinite loop needed). Rejected as unnecessary.

**Implementation Pattern**:
```tsx
<Box sx={{
  display: 'flex',
  overflowX: 'auto',
  scrollSnapType: 'x mandatory',
  gap: 2,
  scrollbarWidth: 'none',  // Firefox
  '&::-webkit-scrollbar': { display: 'none' },  // Chrome/Safari
}}>
  {sets.map(set => (
    <Box key={set.code} sx={{ scrollSnapAlign: 'start', flexShrink: 0 }}>
      <SetPreviewCard set={set} />
    </Box>
  ))}
</Box>
```

Arrow navigation uses `scrollBy({ left: cardWidth, behavior: 'smooth' })` via a ref to the scroll container.

---

### 2. Animation Strategy

**Decision**: CSS transitions and `@keyframes` for page load; Intersection Observer API for scroll-triggered reveals. No animation library.

**Rationale**: The spec requires staggered fade-in on page load and scroll-triggered section reveals. CSS animations handle the load sequence via `animation-delay`. The Intersection Observer API is a browser native for scroll detection with no dependencies. `prefers-reduced-motion` is trivially handled with a CSS media query that sets `animation-duration: 0s` and `transition-duration: 0s`.

**Alternatives Considered**:
- **Framer Motion** - Powerful animation library with `whileInView` and layout animations. Adds ~30KB and is overkill for fade-in/translateY effects. The spec mentions it as an option but CSS handles the requirements. Rejected for bundle size.
- **react-intersection-observer** - Convenience wrapper around Intersection Observer. Adds minimal weight but unnecessary since the raw API is simple. Rejected as unnecessary dependency.
- **GSAP** - Enterprise animation library. Massive overkill for this use case. Rejected.

**Implementation Pattern**:
```tsx
// Custom hook for scroll-triggered visibility
const useInView = (options?: IntersectionObserverInit) => {
  const ref = useRef<HTMLElement>(null);
  const [isInView, setIsInView] = useState(false);
  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => { if (entry.isIntersecting) setIsInView(true); },
      { threshold: 0.1, ...options }
    );
    if (ref.current) observer.observe(ref.current);
    return () => observer.disconnect();
  }, []);
  return { ref, isInView };
};
```

**Reduced Motion Support**:
```css
@media (prefers-reduced-motion: reduce) {
  * { animation-duration: 0s !important; transition-duration: 0s !important; }
}
```

---

### 3. Search Navigation Pattern

**Decision**: Simple navigation to existing search pages (no autocomplete for MVP)

**Rationale**: The spec recommends simple navigation for MVP. The existing `/search/cards` and `/search/artists` pages already handle search functionality with filters. Adding autocomplete would require new GraphQL queries and significant frontend complexity. The homepage search bar simply navigates with the search term as a URL parameter.

**Alternatives Considered**:
- **Autocomplete with live results** - Would require a new lightweight search GraphQL endpoint and real-time debounced queries. Deferred to post-MVP enhancement. Rejected for scope.
- **Unified search dropdown** - Combining cards, sets, and artists in a single autocomplete. Complex UX and backend requirements. Rejected for scope.

**Implementation Pattern**:
```tsx
// On submit: navigate to search page with query
const handleSearch = (searchTerm: string) => {
  navigate(`/search/cards?q=${encodeURIComponent(searchTerm)}`);
};
```

---

### 4. Featured Sets Data Source

**Decision**: Use existing `GET_ALL_SETS` GraphQL query, sort by `releasedAt` descending, take first 8-12 sets

**Rationale**: The `allSets` query already returns all set data including `releasedAt`, `cardCount`, `iconSvgUri`, `name`, and `code`. Sorting client-side by release date and slicing gives the "latest releases" carousel data with no backend work. The AllSetsPage already uses this query, so Apollo Client may have it cached.

**Alternatives Considered**:
- **New "featured sets" endpoint** - Would allow server-side curation but adds backend work. Rejected as unnecessary for MVP.
- **Hardcoded set codes** - Simple but requires code changes for each new release. Rejected as unmaintainable.

**Implementation Pattern**:
```tsx
const { data, loading } = useQuery(GET_ALL_SETS, { variables: { args: null } });
const latestSets = useMemo(() => {
  if (!data?.allSets?.data) return [];
  return [...data.allSets.data]
    .filter(set => set.setType === 'expansion' || set.setType === 'core')
    .sort((a, b) => new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime())
    .slice(0, 12);
}, [data]);
```

---

### 5. Authenticated User Stats Computation

**Decision**: Compute stats from existing Apollo cache and contexts. Each stat loads independently with its own CircularProgress spinner.

**Rationale**: Per clarification sessions, all user data is computed client-side from existing sources (3 stat boxes only — upcoming signings removed):
- **Cards tracked**: Sum of `userCollection` counts from Apollo cache (cards with any `userCollection` entry)
- **Sets In Progress**: Count of sets where `userCollection.collecting` has entries (from `GET_ALL_SETS` with userId)
- **Wishlist count**: Count of cards returned by `GET_USER_WISHLIST` query (filtered to those with `totalCount > 0`)

If any stat fetch fails, the entire hero falls back to the anonymous version.

**Alternatives Considered**:
- **New aggregated stats endpoint** - Better performance but requires backend work. Deferred to post-implementation evaluation.
- **Single loading state for all stats** - Simpler but blocks entire dashboard for slowest query. Rejected per clarification.

**Data Source Mapping**:
| Stat | Source | Query/Context |
|------|--------|---------------|
| Display name | Auth0 user object | `useAuth0().user.name` or `user.nickname` |
| Cards tracked | Sets with user data | `GET_ALL_SETS` with userId - sum `userCollection.totalCards` across sets |
| Sets In Progress | Sets with user data | `GET_ALL_SETS` with userId - count sets where `userCollection.collecting.length > 0` |
| Wishlist count | Wishlist query | `GET_USER_WISHLIST` with userId - count results after filtering `totalCount > 0` |

**Wishlist Data Reuse**: The `GET_USER_WISHLIST` query serves double duty — the count populates the stat box, and the first 4-6 cards provide thumbnails for the CollectionPreview wishlist section. This avoids a separate query. Flagged for post-implementation backend API evaluation (a lightweight count-only endpoint would reduce data transfer).

---

### 6. Existing Component Reuse Assessment

**Decision**: Reuse the following existing components; create new homepage-specific components for the rest.

| Existing Component | Reuse For |
|-------------------|-----------|
| `Section` (molecule) | Semantic section wrappers for each homepage section |
| `AppButton` (molecule) | All CTA buttons (Browse Sets, Start Collecting, etc.) |
| `AppCard` (molecule) | Feature cards, set preview cards |
| `EmptyState` (molecule) | Potential reuse for empty authenticated states |
| `useAuth0` hook | Authentication state detection |
| `useUser` hook | User profile data (display name, ID) |
| `useNavigate` (React Router) | All navigation actions |
| Theme values | `theme.mtg.gradients.header`, `theme.mtg.shadows.card.*`, `theme.palette.rarity.*` |

**New Components Required** (homepage-specific, co-located):
- `HomePage.tsx` - Orchestrator
- 7 section components in `sections/`
- 3 sub-components in `components/` (FeatureCard, SetPreviewCard, StatBox)
- `useInView` custom hook (if not creating shared utility)
- 3 data files in `data/` (featureCards, featuredArtists, proTips)

---

### 7. Routing Integration

**Decision**: Replace the inline `HomePage` function in `App.tsx` with a lazy-loaded import from the new `HomePage` module.

**Rationale**: The current `HomePage` is defined inline in `App.tsx` (lines 51-96). All other pages are lazy-loaded via `React.lazy()`. The new HomePage should follow the same pattern for consistency and code-splitting.

**Implementation**:
```tsx
// App.tsx - change from inline to lazy import
const HomePage = React.lazy(() => import('./components/pages/HomePage'));
// Remove the inline HomePage function (lines 51-96)
```

The route definition stays the same: `<Route path="/" element={<PageErrorBoundary name="HomePage"><HomePage /></PageErrorBoundary>} />`
