# Tasks: Homepage Redesign

**Input**: Design documents from `/specs/003-homepage-redesign/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Not requested in the feature specification. Test tasks are omitted.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story. User stories are ordered to respect component dependencies (US3 depends on anonymous sections from US1/US6 existing first).

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Frontend**: `client/src/` at repository root
- **Homepage components**: `client/src/components/pages/HomePage/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create directory structure and static data files used across multiple stories

- [X] T001 Create homepage directory structure: `mkdir -p client/src/components/pages/HomePage/{sections,components,data}`
- [X] T002 [P] Create FeatureCardData interface and feature cards static data array (4 cards: Browse Sets, Search Cards, Discover Artists, Convention Signing) in `client/src/components/pages/HomePage/data/featureCards.ts`
- [X] T003 [P] Create barrel export with default export of HomePage in `client/src/components/pages/HomePage/index.ts`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Create shared sub-components and wire up routing — MUST be complete before any user story section can render

**CRITICAL**: No user story work can begin until this phase is complete

- [X] T004 Create FeatureCard sub-component with Props interface (icon, title, description, ctaLabel, route, authRequired, isAuthenticated), hover animation (translateY -4px, elevation 2→8), lock icon with Tooltip for auth-required features, CTA button navigating to route in `client/src/components/pages/HomePage/components/FeatureCard.tsx`
- [X] T005 Create HomePage orchestrator skeleton — imports useAuth0/useUser, renders placeholder Box for each of the 7 sections with TODO comments, default export for React.lazy compatibility in `client/src/components/pages/HomePage/HomePage.tsx`
- [X] T006 Update App.tsx: replace inline HomePage function (lines 51-96) with `React.lazy(() => import('./components/pages/HomePage'))`, keep existing route and PageErrorBoundary wrapper in `client/src/App.tsx`

**Checkpoint**: Homepage renders with placeholder content, routing works, build passes

---

## Phase 3: User Story 1 — Anonymous Visitor First Impression (Priority: P1) MVP

**Goal**: First-time visitors land on the homepage and immediately understand what MTG Discovery offers, can see key features, and can navigate to explore without signing up.

**Independent Test**: Load homepage in incognito browser. Verify hero headline is visible in first viewport, 4 feature cards display with correct icons/descriptions, Convention Signing shows lock icon, all CTA buttons navigate correctly.

### Implementation for User Story 1

- [X] T007 [US1] Create HeroSection (anonymous version only) with gradient background using `theme.mtg.gradients.header`, h1 headline "Track Your Collection. Discover Every Card.", subtitle, "Browse Sets" outlined button navigating to `/sets`, "Start Collecting" contained button triggering Auth0 signup via `loginWithRedirect({ authorizationParams: { screen_hint: 'signup' } })`, responsive layout (minHeight xs:80vh md:60vh), `<section aria-labelledby="hero-heading">` in `client/src/components/pages/HomePage/sections/HeroSection.tsx`
- [X] T008 [US1] Create FeatureHighlights section — renders 4 FeatureCard components from featureCards data in responsive Grid (xs:12, sm:6, md:3), section heading "Everything You Need to Manage Your Collection", `<section aria-labelledby="features-heading">`, each card has `role="article"` and `aria-labelledby="feature-{id}-title"` in `client/src/components/pages/HomePage/sections/FeatureHighlights.tsx`
- [X] T009 [US1] Wire HeroSection and FeatureHighlights into HomePage orchestrator — replace placeholder boxes for sections 1 and 3, pass `isAuthenticated` to FeatureHighlights in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Anonymous visitors see hero with value proposition and 4 feature cards. Convention Signing shows lock icon. All CTAs navigate correctly.

---

## Phase 4: User Story 2 — Quick Search from Homepage (Priority: P1)

**Goal**: Visitors can search for cards directly from the homepage via a centered search bar that navigates to the existing card search results page.

**Independent Test**: Type a card name in the search field, press Enter, verify navigation to `/search/cards?q={term}`. Verify search field has proper ARIA label and placeholder text.

### Implementation for User Story 2

- [X] T010 [US2] Create QuickSearchSection — centered TextField (maxWidth 800px) with SearchIcon InputAdornment, placeholder "Search cards, sets, or artists...", form onSubmit navigates to `/search/cards?q=${encodeURIComponent(searchTerm)}`, visually-hidden h2 heading, `<section aria-labelledby="search-heading">`, `aria-label="Search cards, sets, or artists"` on input in `client/src/components/pages/HomePage/sections/QuickSearchSection.tsx`
- [X] T011 [US2] Wire QuickSearchSection into HomePage orchestrator — replace placeholder box for section 2 in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Search bar renders between hero and features. Submitting a search term navigates to card search page with query parameter.

---

## Phase 5: User Story 6 — Conversion CTA for Anonymous Users (Priority: P2)

**Goal**: Anonymous users who scroll through the homepage see a compelling sign-up prompt at the bottom. Authenticated users see a pro tip instead.

**Independent Test**: As anonymous user, scroll to bottom, verify sign-up CTA is visible with "Create Free Account" button that triggers Auth0 signup and "Sign In" link. As authenticated user, verify pro tip displays instead.

### Implementation for User Story 6

- [X] T012 [P] [US6] Create proTips static data array (5-10 tips with title, body, ctaLabel, ctaRoute; tip selected by date-based index) in `client/src/components/pages/HomePage/data/proTips.ts`
- [X] T013 [US6] Create BottomCTA section with both variants: anonymous version (Paper with gradient, "Ready to Start Your Collection Journey?" headline, description, "Create Free Account" contained button triggering `loginWithRedirect({ authorizationParams: { screen_hint: 'signup' } })`, "Sign In" link triggering `loginWithRedirect()`); authenticated version (Paper with subtle styling, pro tip of the day selected by `Math.floor(Date.now() / 86400000) % tips.length`, contextual CTA button navigating to tip route). Props: `isAuthenticated: boolean` in `client/src/components/pages/HomePage/sections/BottomCTA.tsx`
- [X] T014 [US6] Wire BottomCTA into HomePage orchestrator — replace placeholder box for section 7, pass `isAuthenticated` prop in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Anonymous users see sign-up CTA at bottom. "Create Free Account" triggers Auth0 signup flow. Authenticated users see rotating pro tips with contextual CTA.

---

## Phase 6: User Story 4 — Featured Sets Discovery (Priority: P2)

**Goal**: Visitors discover recent MTG set releases through a horizontal carousel and can click through to any set's detail page.

**Independent Test**: View homepage, scroll to carousel, verify latest expansion/core sets are displayed sorted by release date. On desktop verify arrow navigation on hover. On mobile verify horizontal swipe. Click a set card and verify navigation to `/set/{code}`.

### Implementation for User Story 4

- [X] T015 [P] [US4] Create SetPreviewCard sub-component — Card with set icon (img from iconSvgUri), set name, card count, formatted release date (using date-fns `format`), click handler calling `onClick(code)`, hover effect (scale 1.02, `theme.mtg.shadows.card.hover`). Props: code, name, cardCount, releasedAt, iconSvgUri, onClick in `client/src/components/pages/HomePage/components/SetPreviewCard.tsx`
- [X] T016 [US4] Create FeaturedSetsCarousel section — uses `useQuery(GET_ALL_SETS)`, filters to expansion + core setType, sorts by releasedAt descending, takes first 12. CSS scroll-snap horizontal container (`scrollSnapType: 'x mandatory'`, hidden scrollbar), SetPreviewCard for each set, desktop arrow IconButtons (visible on hover, scroll via `scrollBy`), "Latest Releases" heading with "See All" Link to `/sets`, loading state shows skeleton shimmer cards, error/empty state hides section entirely. ARIA: `role="region"`, `aria-roledescription="carousel"`, arrow buttons `aria-label="Previous/Next sets"`. Keyboard: Arrow Left/Right navigate between cards, Home/End jump to first/last card, Enter/Space activate focused card in `client/src/components/pages/HomePage/sections/FeaturedSetsCarousel.tsx`
- [X] T017 [P] [US4] Create CollectionPreview section (anonymous version only) — static decorative mockup showing styled boxes representing a card grid, LinearProgress at 67%, "See Your Collection Come to Life" heading, "Start Tracking Your Collection" contained Button triggering Auth0 signup. Props: `isAuthenticated: boolean` (renders anonymous variant when false) in `client/src/components/pages/HomePage/sections/CollectionPreview.tsx`
- [X] T018 [US4] Wire FeaturedSetsCarousel and CollectionPreview into HomePage orchestrator — replace placeholder boxes for sections 4 and 5, pass `isAuthenticated` to CollectionPreview in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Carousel displays latest sets with horizontal scroll-snap. Desktop has arrow buttons on hover. Mobile supports touch swipe. Set cards navigate to set detail pages. Anonymous collection preview shows decorative mockup.

---

## Phase 7: User Story 5 — Artist Spotlight Engagement (Priority: P3)

**Goal**: Visitors discover a featured MTG artist with sample card artwork and can navigate to explore their full card portfolio.

**Independent Test**: View homepage, scroll to artist spotlight, verify featured artist name/description/card count display. Verify 4 sample card names are shown. Click "View All Cards" and verify navigation to `/artists/{artistName}`.

### Implementation for User Story 5

- [X] T019 [P] [US5] Create featuredArtists static data array (2-3 artists with name, description, cardCount, sampleCardNames array of 4 names, featuredCardName) in `client/src/components/pages/HomePage/data/featuredArtists.ts`
- [X] T020 [US5] Create ArtistSpotlight section — selects first artist from featuredArtists array, split layout (Grid xs:12 stacked, md:6-6 side by side): left side featured card image placeholder, right side artist name (h4), description (body1), card count, "View All {cardCount} Cards" Button navigating to `/artists/${encodeURIComponent(artistName)}`. Below: 4 sample card name Typography items. `<section aria-labelledby="artist-heading">` in `client/src/components/pages/HomePage/sections/ArtistSpotlight.tsx`
- [X] T021 [US5] Wire ArtistSpotlight into HomePage orchestrator — replace placeholder box for section 6 in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Artist spotlight displays with name, description, sample works, and working navigation to artist page.

---

## Phase 8: User Story 3 — Authenticated User Dashboard (Priority: P1, depends on US1/US4/US6)

**Goal**: Returning authenticated users see a personalized dashboard with collection stats, recent set activity, wishlist preview, and pro tips instead of generic anonymous content.

**Independent Test**: Sign in, visit homepage, verify "Welcome back, {name}!" heading, 3 stat boxes load independently with spinners then show real numbers, collection preview shows recent sets with completion percentages and wishlist card thumbnails, bottom section shows pro tip instead of sign-up CTA. For new user with zero data: verify stat boxes show "0" with encouraging CTAs. For fetch error: verify fallback to anonymous hero.

### Implementation for User Story 3

- [X] T022 [P] [US3] Create StatBox sub-component — Card with elevation={0} and border, centered content. When `value === null`: shows CircularProgress (size 24). When `value === 0 && ctaText`: shows "0" with encouraging CTA text below. When `value > 0`: shows formatted number (Intl.NumberFormat with comma separators). Optional onClick handler for navigation. Props: value (number|null), label (string), ctaText (string?), onClick (()=>void?) in `client/src/components/pages/HomePage/components/StatBox.tsx`
- [X] T023 [US3] Update HeroSection to add authenticated variant — when `isAuthenticated && !statsError`: show "Welcome back, {userName}!" (h2), Grid of 3 StatBox components (xs:1col, sm:3col) for Cards Tracked, Sets In Progress, Wishlist Items with independent loading spinners, "Continue: {lastSetName}" button when recent set exists. When `isAuthenticated && statsError`: fall back to anonymous hero. Empty state: zero values show "0" with CTAs like "Start tracking!". Update HeroSectionProps to include isAuthenticated, userName, stats (UserHomepageStats|null), statsError (boolean) in `client/src/components/pages/HomePage/sections/HeroSection.tsx`
- [X] T024 [US3] Update CollectionPreview to add authenticated variant — when `isAuthenticated === true`: show "Continue Where You Left Off" heading, up to 3 recent sets user is collecting (from sets data with userCollection) with CircularProgress completion percentage and "Continue" button navigating to `/set/{code}`, "Wishlist Preview" subsection showing first 4-6 cards from `GET_USER_WISHLIST` query as thumbnails with "View All" Link to `/wishlist` in `client/src/components/pages/HomePage/sections/CollectionPreview.tsx`
- [X] T025 [US3] Update HomePage orchestrator to compute authenticated stats: use `useQuery(GET_ALL_SETS, { variables: { args: { userId: userProfile?.userId } } })` to derive cardsTracked (sum userCollection.totalCards) and setsInProgress (count sets with collecting entries), use `useQuery(GET_USER_WISHLIST, { variables: { userId: userProfile?.userId }, skip: !isAuthenticated })` to derive wishlistCount (filtered data length) and wishlist preview cards. Track statsError boolean (any query error). Pass stats, statsError, userName to HeroSection. Pass isAuthenticated to CollectionPreview in `client/src/components/pages/HomePage/HomePage.tsx`

**Checkpoint**: Authenticated users see personalized hero with real stats, collection preview with recent sets + wishlist thumbnails, and pro tips. New users see zeros with CTAs. Fetch errors fall back to anonymous hero.

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Animations, accessibility refinements, and performance optimization across all sections

- [X] T026 [P] Create useInView custom hook — uses IntersectionObserver to track element visibility, returns `{ ref, isInView }`, triggers once (does not reset), threshold 0.1 in `client/src/components/pages/HomePage/hooks/useInView.ts`
- [X] T027 Add page load staggered fade-in animation to HeroSection — hero headline 0ms, subtext 100ms, buttons 200ms using CSS `@keyframes fadeInUp` (opacity 0→1, translateY 20px→0) with `animation-delay` per element in `client/src/components/pages/HomePage/sections/HeroSection.tsx`
- [X] T028 Add scroll-triggered fade-in to all remaining sections — wrap each section body in a Box that applies opacity/translateY transition based on useInView `isInView` state, update QuickSearchSection (300ms delay), FeatureHighlights (400ms + 50ms stagger per card), FeaturedSetsCarousel, CollectionPreview, ArtistSpotlight, BottomCTA in all `client/src/components/pages/HomePage/sections/*.tsx` files
- [X] T029 [P] Add reduced motion support — CSS media query `@media (prefers-reduced-motion: reduce)` that sets `animation-duration: 0s !important` and `transition-duration: 0s !important`, apply globally to HomePage wrapper or via a shared sx style object in `client/src/components/pages/HomePage/HomePage.tsx`
- [X] T030 Add hover state refinements to FeatureCard (transition 0.2s ease-in-out for translateY + box-shadow + border) and SetPreviewCard (transition for scale + box-shadow) using `theme.mtg.transitions.card` in `client/src/components/pages/HomePage/components/FeatureCard.tsx` and `client/src/components/pages/HomePage/components/SetPreviewCard.tsx`
- [X] T031 Performance optimization — add React.memo to StatBox, FeatureCard, SetPreviewCard; add useMemo for latest sets filtering/sorting in FeaturedSetsCarousel; verify lazy loading via React.lazy works correctly with `npm run build`; verify no layout shifts during load; run Lighthouse audit and verify performance score > 90 and accessibility score > 90 (address any flagged issues) in `client/src/components/pages/HomePage/` (multiple files)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies — can start immediately
- **Foundational (Phase 2)**: Depends on Setup (T001 for directory) — BLOCKS all user stories
- **US1 (Phase 3)**: Depends on Foundational (T004 FeatureCard, T005 HomePage skeleton, T006 routing)
- **US2 (Phase 4)**: Depends on Foundational — can run in PARALLEL with US1
- **US6 (Phase 5)**: Depends on Foundational — can run in PARALLEL with US1/US2
- **US4 (Phase 6)**: Depends on Foundational — can run in PARALLEL with US1/US2/US6
- **US5 (Phase 7)**: Depends on Foundational — can run in PARALLEL with US1/US2/US4/US6
- **US3 (Phase 8)**: Depends on US1 (HeroSection exists), US4 (CollectionPreview exists), US6 (BottomCTA exists) — MUST wait for these
- **Polish (Phase 9)**: Depends on ALL user stories being complete

### User Story Dependencies

- **US1 (P1)**: After Foundational → Independent
- **US2 (P1)**: After Foundational → Independent
- **US6 (P2)**: After Foundational → Independent
- **US4 (P2)**: After Foundational → Independent
- **US5 (P3)**: After Foundational → Independent
- **US3 (P1)**: After US1 + US4 + US6 → Modifies existing sections to add auth variants

### Within Each User Story

- Data files before components that use them
- Sub-components before sections that compose them
- Sections before HomePage wiring
- Story complete before checkpoint validation

### Parallel Opportunities

- **Setup**: T002 and T003 can run in parallel (different files)
- **Foundational**: T004 can run in parallel with T005 (different files)
- **After Foundational**: US1, US2, US4, US5, US6 can ALL start in parallel (different files, independent sections)
- **Within US4**: T015 (SetPreviewCard) and T017 (CollectionPreview) can run in parallel
- **Within US5**: T019 (data) can run in parallel with other stories
- **Within US3**: T022 (StatBox) can run in parallel with T012 (proTips — already done in US6)
- **Polish**: T026 (useInView) and T029 (reduced motion) can run in parallel

---

## Parallel Example: After Foundational Completes

```bash
# Launch all independent user stories in parallel:
Task: "[US1] HeroSection anonymous in sections/HeroSection.tsx"
Task: "[US2] QuickSearchSection in sections/QuickSearchSection.tsx"
Task: "[US6] BottomCTA in sections/BottomCTA.tsx"
Task: "[US4] SetPreviewCard in components/SetPreviewCard.tsx"
Task: "[US5] featuredArtists data in data/featuredArtists.ts"
```

```bash
# After US1+US4+US6 complete, launch US3:
Task: "[US3] StatBox sub-component in components/StatBox.tsx"
Task: "[US3] Update HeroSection with auth variant"
Task: "[US3] Update CollectionPreview with auth variant"
Task: "[US3] Update HomePage stats computation"
```

---

## Implementation Strategy

### MVP First (US1 + US2 Only)

1. Complete Phase 1: Setup (T001-T003)
2. Complete Phase 2: Foundational (T004-T006)
3. Complete Phase 3: US1 — Anonymous hero + features (T007-T009)
4. Complete Phase 4: US2 — Search bar (T010-T011)
5. **STOP and VALIDATE**: Homepage renders with hero, search, features. All navigation works.
6. Deploy/demo if ready — anonymous users can understand and explore the platform

### Incremental Delivery

1. Setup + Foundational → Foundation ready
2. US1 + US2 → Anonymous hero, search, features → **MVP Deploy** (validates core homepage)
3. US6 → Bottom CTA for anonymous → Deploy (adds conversion path)
4. US4 → Featured sets carousel + collection mockup → Deploy (adds content depth)
5. US5 → Artist spotlight → Deploy (adds engagement content)
6. US3 → Authenticated personalization → Deploy (adds personalized dashboard)
7. Polish → Animations, reduced motion, perf → Deploy (final polish)
8. Each increment adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers after Foundational completes:

- **Developer A**: US1 (hero + features) → then US3 (auth variants)
- **Developer B**: US2 (search) + US6 (bottom CTA) + US4 (carousel + collection preview)
- **Developer C**: US5 (artist spotlight) → then Polish phase

---

## Notes

- [P] tasks = different files, no dependencies on incomplete tasks
- [Story] label maps task to specific user story for traceability
- Each user story is independently completable and testable (except US3 which modifies existing sections)
- Post-plan clarifications applied: no PopularSearchChip/popularSearches.ts, no 4th stat box (upcoming signings removed), wishlist count from GET_USER_WISHLIST query, 3 stat boxes only
- **Deferred**: Analytics integration (success metrics from spec.md) is deferred to a follow-up feature. No analytics instrumentation is included in these tasks.
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Run `npm run build` after each phase to verify no compilation errors
- Run `npm run lint` periodically to catch style issues early
