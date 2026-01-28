# Feature Specification: Homepage Redesign

**Feature Branch**: `003-homepage-redesign`
**Created**: 2026-01-19
**Status**: Draft
**Input**: UX design for improved homepage experience

## Overview

This feature redesigns the MTG Discovery homepage from a minimal developer-focused landing page to a user-centric experience that communicates value, showcases features, and provides personalized content for authenticated users.

### Current State

The existing homepage (`App.tsx:51-96`) consists of:
- A centered card with "Welcome to MTG Discovery" heading
- Developer-focused copy about "atomic design principles" and "component system"
- Single "Browse All Sets" button
- No visual appeal, no MTG theming, no personalization

### Target State

A modern, engaging homepage that:
- Clearly communicates the value proposition to new visitors
- Provides quick access to all major features
- Personalizes content for authenticated users
- Showcases the depth of the platform (sets, cards, artists)
- Follows Material-UI design patterns with MTG theming

---

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Anonymous Visitor First Impression (Priority: P1)

A first-time visitor lands on MTG Discovery and immediately understands what the platform offers, can explore without signing up, and knows how to get started.

**Why this priority**: First impressions determine whether visitors explore further or leave. The homepage must communicate value within seconds.

**Independent Test**: Can be tested by loading the homepage in an incognito browser and verifying the value proposition is clear, features are visible, and exploration paths are obvious.

**Acceptance Scenarios**:

1. **Given** a first-time visitor, **When** they land on the homepage, **Then** they see a clear headline explaining what MTG Discovery does within the first viewport.

2. **Given** an anonymous visitor, **When** they view the feature highlights, **Then** they can identify at least 3 key features (browse sets, search cards, artist discovery) without scrolling extensively.

3. **Given** an anonymous visitor, **When** they want to explore, **Then** they can navigate to browse sets or search cards without signing up.

4. **Given** an anonymous visitor viewing auth-required features, **When** they see Convention Signing or Wishlist, **Then** a lock icon indicates sign-in is required with a tooltip explanation.

---

### User Story 2 - Quick Search from Homepage (Priority: P1)

A visitor (anonymous or authenticated) can quickly search for cards, sets, or artists directly from the homepage without navigating to separate search pages.

**Why this priority**: Search is a primary user action. Reducing friction to search improves engagement and demonstrates platform value immediately.

**Independent Test**: Can be tested by entering a search term on the homepage and verifying navigation to appropriate results.

**Acceptance Scenarios**:

1. **Given** a visitor on the homepage, **When** they type a card name in the search field, **Then** they are directed to the card search results page.

2. **Given** a visitor on the homepage, **When** they submit a search term, **Then** they navigate to the card search results page with their query pre-filled.

3. **Given** a visitor on the homepage, **When** they view the search section, **Then** the search field is prominently displayed with clear placeholder text indicating searchable content types (cards, sets, artists).

---

### User Story 3 - Authenticated User Dashboard (Priority: P1)

A returning authenticated user sees a personalized dashboard with their collection stats, recent activity, and quick actions instead of the generic hero.

**Why this priority**: Authenticated users are the most valuable. Personalizing their experience increases engagement and demonstrates the value of having an account.

**Independent Test**: Can be tested by signing in and verifying the hero section transforms to show user-specific data.

**Acceptance Scenarios**:

1. **Given** an authenticated user, **When** they visit the homepage, **Then** they see "Welcome back, [name]!" instead of the generic value proposition.

2. **Given** an authenticated user with tracked cards, **When** they view the dashboard, **Then** they see stats including cards tracked, sets in progress, and wishlist count.

3. **Given** an authenticated user, **When** they view quick actions, **Then** they can continue to recently viewed sets with one click.

4. **Given** an authenticated user with wishlist items, **When** they view the collection preview section, **Then** they see a preview of their actual wishlist items.

---

### User Story 4 - Featured Sets Discovery (Priority: P2)

Visitors can discover new and popular MTG sets through a featured carousel, encouraging exploration of the platform's content.

**Why this priority**: Showcasing content demonstrates platform depth and encourages exploration. This drives engagement but is secondary to core navigation.

**Independent Test**: Can be tested by viewing the homepage and verifying the carousel displays recent sets with navigation controls.

**Acceptance Scenarios**:

1. **Given** a visitor on the homepage, **When** they scroll to the featured sets section, **Then** they see a carousel of recent MTG set releases.

2. **Given** a visitor viewing the carousel, **When** they click a set card, **Then** they navigate to that set's detail page.

3. **Given** a visitor on mobile, **When** they interact with the carousel, **Then** they can swipe horizontally to browse sets.

4. **Given** a visitor on desktop, **When** they interact with the carousel, **Then** they can use arrow buttons to navigate.

---

### User Story 5 - Artist Spotlight Engagement (Priority: P3)

Visitors discover featured MTG artists and can explore their card portfolios, supporting the convention signing use case.

**Why this priority**: Artist discovery is a differentiating feature but serves a specific subset of users. Important for engagement but lower priority than core features.

**Independent Test**: Can be tested by viewing the homepage artist section and clicking through to an artist's portfolio.

**Acceptance Scenarios**:

1. **Given** a visitor on the homepage, **When** they scroll to the artist spotlight, **Then** they see a featured artist with sample card artwork.

2. **Given** a visitor viewing artist spotlight, **When** they click "View All Cards", **Then** they navigate to that artist's portfolio page.

3. **Given** a visitor viewing artist spotlight, **When** they see sample cards, **Then** at least 3-4 cards by that artist are displayed.

---

### User Story 6 - Conversion CTA for Anonymous Users (Priority: P2)

Anonymous users are presented with compelling calls-to-action to create an account after experiencing the platform's value.

**Why this priority**: Conversion is a key business goal. CTAs should appear after users have seen value, not immediately on landing.

**Independent Test**: Can be tested by scrolling through the homepage as an anonymous user and verifying a sign-up CTA appears near the bottom.

**Acceptance Scenarios**:

1. **Given** an anonymous visitor who has scrolled through the homepage, **When** they reach the bottom CTA section, **Then** they see a compelling sign-up prompt.

2. **Given** an anonymous visitor viewing the CTA, **When** they click "Create Free Account", **Then** they are directed to the Auth0 sign-up flow.

3. **Given** an authenticated user, **When** they view the bottom section, **Then** they see tips/tutorials instead of sign-up prompts.

---

## Information Architecture

### Section Order (Top to Bottom)

| # | Section | Purpose | Auth Variant |
|---|---------|---------|--------------|
| 1 | Hero | Value prop / Dashboard | Yes - full replacement |
| 2 | Quick Search | Search bar | No |
| 3 | Feature Highlights | 4 key features | Yes - personalized order |
| 4 | Featured Sets | Latest releases carousel | Yes - includes user's sets |
| 5 | Collection Preview | Demo / User activity | Yes - actual user data |
| 6 | Artist Spotlight | Featured artist | Optional personalization |
| 7 | Bottom CTA | Sign-up / Tips | Yes - full replacement |

### Navigation Paths

```
Homepage
├── Hero CTA → /sets (Browse Sets)
├── Hero CTA → Auth0 (Start Collecting)
├── Search → /search/cards, /search/artists, /set/:code
├── Feature: Browse Sets → /sets
├── Feature: Search Cards → /search/cards
├── Feature: Discover Artists → /search/artists
├── Feature: Convention Signing → /convention-signing (auth required)
├── Featured Set Card → /set/:setCode
├── Artist Spotlight → /artists/:artistName
└── Bottom CTA → Auth0 (Create Account)
```

---

## Wireframes

### Section 1: Hero Section

#### Anonymous Version

```
┌─────────────────────────────────────────────────────────────────────┐
│  [Background: MTG gradient using theme.mtg.gradients.header]        │
│                                                                     │
│                                                                     │
│         "Track Your Collection. Discover Every Card."               │
│                        (h1, centered)                               │
│                                                                     │
│      "The complete toolkit for Magic: The Gathering collectors.     │
│       Browse 25,000+ cards, track your collection, and never        │
│                      miss a signing."                               │
│                   (subtitle, text.secondary)                        │
│                                                                     │
│          ┌─────────────────┐  ┌─────────────────┐                   │
│          │  Browse Sets    │  │ Start Collecting│                   │
│          │  (outlined)     │  │ (contained)     │                   │
│          └─────────────────┘  └─────────────────┘                   │
│                                                                     │
│              "No account needed to explore" (link)                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Container: minHeight { xs: '80vh', md: '60vh' }
- Flex column, center justified
- Max content width: 800px
- Padding: { xs: 3, md: 6 }

Components:
- Typography variant="h1" (responsive: h3 on mobile)
- Typography variant="h5" for subtitle
- Stack direction={{ xs: 'column', sm: 'row' }} for buttons
- Button variant="outlined" + Button variant="contained"
```

#### Authenticated Version

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│              "Welcome back, {displayName}!"                         │
│                        (h2, centered)                               │
│                                                                     │
│   ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐  │
│   │      1,234       │ │        12        │ │        45        │  │
│   │      Cards       │ │       Sets       │ │     Wishlist     │  │
│   │     Tracked      │ │    In Progress   │ │      Items       │  │
│   └──────────────────┘ └──────────────────┘ └──────────────────┘  │
│                                                                     │
│          ┌─────────────────────────────────────────┐                │
│          │  Continue: [Last Set Name] →           │                │
│          └─────────────────────────────────────────┘                │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Same container as anonymous
- Grid for stat boxes: { xs: 1 column, sm: 3 columns }

Components:
- StatBox: Card with elevation={0}, border, centered content
- Typography variant="h4" for numbers (or CircularProgress while loading)
- Typography variant="caption" for labels
- Each StatBox loads independently with its own spinner
- Button or Card for "Continue" action

Empty State (new user with zero data):
- Display authenticated hero with all stat boxes showing "0"
- Each zero-stat box includes an encouraging CTA (e.g., "0 Cards — Start tracking!")
- "Continue" action hidden when no recent sets exist

Error State:
- If any stat data fetch fails, fall back to the anonymous hero section
```

### Section 2: Quick Search

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│   ┌─────────────────────────────────────────────────────────────┐   │
│   │  🔍  Search cards, sets, or artists...                  [▼] │   │
│   └─────────────────────────────────────────────────────────────┘   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Max width: 800px, centered
- Padding: { xs: 2, md: 4 }

Components:
- TextField with InputAdornment (SearchIcon)
- Optional: Select for search type (cards/sets/artists)
- Autocomplete for enhanced UX (future enhancement, see post-impl GraphQL notes)
```

### Section 3: Feature Highlights

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│          "Everything You Need to Manage Your Collection"            │
│                                                                     │
│   ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌────────────┐│
│   │     📚       │ │     🔍       │ │     🎨       │ │    ✍️  🔒  ││
│   │              │ │              │ │              │ │            ││
│   │ BROWSE SETS  │ │ SEARCH CARDS │ │  DISCOVER    │ │ CONVENTION ││
│   │              │ │              │ │  ARTISTS     │ │  SIGNING   ││
│   │ View every   │ │ Find cards   │ │ Explore      │ │ Plan which ││
│   │ MTG set ever │ │ by name,     │ │ cards by     │ │ cards to   ││
│   │ made. Track  │ │ artist, or   │ │ your         │ │ bring for  ││
│   │ progress.    │ │ mana cost.   │ │ favorite     │ │ signatures ││
│   │              │ │              │ │ artists.     │ │            ││
│   │ [Explore →]  │ │ [Search →]   │ │ [Browse →]   │ │ [Plan →]   ││
│   └──────────────┘ └──────────────┘ └──────────────┘ └────────────┘│
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Grid container with spacing={3}
- Columns: { xs: 12, sm: 6, md: 3 }

Components:
- Card with CardContent and CardActions
- Avatar or Box for icon (with gradient background)
- Typography variant="h6" for title
- Typography variant="body2" for description
- Button variant="text" for CTA
- Lock icon (Tooltip) for auth-required features
```

#### Feature Card Data

| Feature | Icon | Title | Description | CTA | Route | Auth Required |
|---------|------|-------|-------------|-----|-------|---------------|
| 1 | CollectionsBookmark | Browse Sets | View every MTG set ever made. Track completion and view card checklists. | Explore Sets | /sets | No |
| 2 | Search | Search Cards | Find any card by name, type, color, or text. View all printings across sets. | Search Now | /search/cards | No |
| 3 | Brush | Discover Artists | Explore cards by your favorite artists. Perfect for collecting signatures. | Browse Artists | /search/artists | No |
| 4 | EventNote | Convention Signing | Plan which cards to bring for artist signings at conventions and events. | Plan Signings | /convention-signing | Yes |

### Section 4: Featured Sets Carousel

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│   "Latest Releases"                                   [See All →]   │
│                                                                     │
│  ◀  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐  ▶ │
│     │ [SET ICON] │ │ [SET ICON] │ │ [SET ICON] │ │ [SET ICON] │    │
│     │            │ │            │ │            │ │            │    │
│     │  Murders   │ │  Outlaws   │ │  Bloomburr.│ │  Duskmourn │    │
│     │  at Karlov │ │  of Thunder│ │            │ │            │    │
│     │            │ │            │ │            │ │            │    │
│     │ 286 cards  │ │ 312 cards  │ │ 271 cards  │ │ 298 cards  │    │
│     │ Feb 2024   │ │ Apr 2024   │ │ Aug 2024   │ │ Sep 2024   │    │
│     └────────────┘ └────────────┘ └────────────┘ └────────────┘    │
│                                                                     │
│                      ● ○ ○ ○ ○ (pagination)                        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Full width container
- Horizontal scroll with scroll-snap or carousel library
- Visible cards: { xs: 1.2, sm: 2.5, md: 4 }

Components:
- Section header with Typography + Link
- IconButton for navigation arrows (desktop only)
- SetPreviewCard: Card with set icon, name, card count, release date
- Pagination dots (optional)

Responsive:
- Mobile: Swipe navigation, no arrows
- Desktop: Arrow buttons visible on hover
```

### Section 5: Collection Preview

#### Anonymous Version

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│              "See Your Collection Come to Life"                     │
│                                                                     │
│   ┌─────────────────────────────────────────────────────────────┐   │
│   │  [Decorative mockup of collection tracking UI]              │   │
│   │                                                             │   │
│   │   ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐ ┌─────┐                 │   │
│   │   │ ✓   │ │ ✓   │ │     │ │ ✓   │ │     │  (card grid)   │   │
│   │   └─────┘ └─────┘ └─────┘ └─────┘ └─────┘                 │   │
│   │                                                             │   │
│   │   Progress: ████████████░░░░░░░░ 67% complete              │   │
│   │                                                             │   │
│   └─────────────────────────────────────────────────────────────┘   │
│                                                                     │
│              [Start Tracking Your Collection]                       │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Components:
- Paper with decorative content (static image or styled boxes)
- LinearProgress showing example completion
- Button variant="contained" for CTA
```

#### Authenticated Version

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│              "Continue Where You Left Off"                          │
│                                                                     │
│   RECENT SETS                                                       │
│   ┌────────────┐ ┌────────────┐ ┌────────────┐                     │
│   │    MKM     │ │    OTJ     │ │    BLB     │                     │
│   │    67%     │ │    23%     │ │    89%     │                     │
│   │ [Continue] │ │ [Continue] │ │ [Continue] │                     │
│   └────────────┘ └────────────┘ └────────────┘                     │
│                                                                     │
│   WISHLIST PREVIEW                                      [View All]  │
│   ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐                  │
│   │ [card]  │ │ [card]  │ │ [card]  │ │ [card]  │  ...             │
│   └─────────┘ └─────────┘ └─────────┘ └─────────┘                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Components:
- Typography for section headers
- Card components for recent sets with CircularProgress
- Horizontal scroll for wishlist card thumbnails
- Link for "View All"

Data Requirements:
- Recent sets: Last 3 sets user interacted with
- Wishlist preview: First 4-6 wishlist items
```

### Section 6: Artist Spotlight

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│   "Artist Spotlight"                                                │
│                                                                     │
│   ┌────────────────────────┐ ┌──────────────────────────────────┐  │
│   │                        │ │                                  │  │
│   │   [Featured Card Art   │ │  MAGALI VILLENEUVE               │  │
│   │    or Artist Photo]    │ │                                  │  │
│   │                        │ │  One of Magic's most prolific    │  │
│   │                        │ │  artists with over 150 card      │  │
│   │                        │ │  illustrations including iconic  │  │
│   │                        │ │  planeswalkers and legends.      │  │
│   │                        │ │                                  │  │
│   │                        │ │  [View All 156 Cards →]          │  │
│   └────────────────────────┘ └──────────────────────────────────┘  │
│                                                                     │
│   SAMPLE WORKS                                                      │
│   ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐                  │
│   │ [card]  │ │ [card]  │ │ [card]  │ │ [card]  │                  │
│   └─────────┘ └─────────┘ └─────────┘ └─────────┘                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘

Layout:
- Grid: { xs: 12 (stacked), md: 6-6 (side by side) }

Components:
- Featured image (card art or placeholder)
- Typography for artist name (h4) and description (body1)
- Button linking to /artists/{artistName}
- Card thumbnail row for sample works

Data:
- Hardcoded array of featured artists in code (name, description, card count, sample card IDs)
- Rotated manually via code changes with deployments
```

### Section 7: Bottom CTA

#### Anonymous Version

```
┌─────────────────────────────────────────────────────────────────────┐
│   ┌─────────────────────────────────────────────────────────────┐   │
│   │  [Background: Gradient or subtle pattern]                   │   │
│   │                                                             │   │
│   │         "Ready to Start Your Collection Journey?"           │   │
│   │                                                             │   │
│   │    Create a free account to track your cards, build        │   │
│   │    wishlists, and plan convention signings.                │   │
│   │                                                             │   │
│   │              [Create Free Account]                          │   │
│   │                                                             │   │
│   │         Already have an account? [Sign In]                  │   │
│   │                                                             │   │
│   └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘

Components:
- Paper with gradient background
- Typography variant="h4" for headline
- Typography variant="body1" for description
- Button variant="contained" for primary CTA
- Link for sign-in
```

#### Authenticated Version

```
┌─────────────────────────────────────────────────────────────────────┐
│   ┌─────────────────────────────────────────────────────────────┐   │
│   │                                                             │   │
│   │              "Pro Tip of the Day" 💡                       │   │
│   │                                                             │   │
│   │    Use Binder View to quickly flip through your           │   │
│   │    collection like a physical binder. Try it on            │   │
│   │    any set you're tracking!                                │   │
│   │                                                             │   │
│   │              [Try Binder View]                              │   │
│   │                                                             │   │
│   └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘

Components:
- Paper with subtle styling
- Rotating tips/tutorials (can be hardcoded array initially)
- Contextual CTA based on tip content
```

---

## Interaction Patterns

### Hover States

**Feature Cards**:
```
Rest → Hover transition (0.2s ease-in-out):
- transform: translateY(-4px)
- box-shadow: elevation 2 → elevation 8
- border: transparent → primary.main
```

**Set Cards in Carousel**:
```
Rest → Hover transition:
- transform: scale(1.02)
- box-shadow: theme.mtg.shadows.card.hover
```

**Buttons**: Default MUI hover states

### Animations

**Page Load Sequence** (staggered fade-in):
| Element | Delay |
|---------|-------|
| Hero headline | 0ms |
| Hero subtext | 100ms |
| Hero buttons | 200ms |
| Search bar | 300ms |
| Feature cards | 400ms + 50ms stagger each |

**Scroll-Triggered**:
- Sections fade in as they enter viewport
- Use Intersection Observer or framer-motion whileInView

**Reduced Motion**:
- Respect `prefers-reduced-motion` media query
- Set all animation durations to 0 when enabled

### Loading States

- Skeleton screens for feature cards during data fetch
- Shimmer effect on set carousel
- Hero text loads immediately (no skeleton)

---

## Accessibility Requirements

### Semantic Structure

```html
<main>
  <section aria-labelledby="hero-heading">
    <h1 id="hero-heading">Track Your Collection...</h1>
  </section>

  <section aria-labelledby="search-heading">
    <h2 id="search-heading" class="visually-hidden">Search</h2>
  </section>

  <section aria-labelledby="features-heading">
    <h2 id="features-heading">Everything You Need</h2>
  </section>

  <section aria-labelledby="sets-heading">
    <h2 id="sets-heading">Latest Releases</h2>
  </section>

  <!-- Continue pattern... -->
</main>
```

### ARIA Requirements

| Element | ARIA Attributes |
|---------|-----------------|
| Search field | `aria-label="Search cards, sets, or artists"`, `role="searchbox"` |
| Feature cards | `role="article"`, `aria-labelledby="feature-{id}-title"` |
| Carousel | `role="region"`, `aria-label="Featured sets carousel"`, `aria-roledescription="carousel"` |
| Carousel slides | `role="group"`, `aria-roledescription="slide"`, `aria-label="{n} of {total}"` |
| Lock icons | `aria-label="Sign in required to access this feature"` |
| Navigation arrows | `aria-label="Previous sets"` / `aria-label="Next sets"` |

### Keyboard Navigation

**Tab Order**:
1. Skip to main content link
2. Hero CTAs
3. Search field
4. Feature cards (in reading order)
5. Carousel navigation buttons
6. Set cards within carousel
7. Remaining sections in order

**Carousel Keyboard Support**:
- Arrow Left/Right: Navigate between cards
- Enter/Space: Activate focused card
- Home: First card
- End: Last card

### Focus Indicators

```tsx
sx={{
  '&:focus-visible': {
    outline: '2px solid',
    outlineColor: 'primary.main',
    outlineOffset: 2,
  },
}}
```

### Color Contrast

- All text meets WCAG AA (4.5:1 normal, 3:1 large)
- Do not rely solely on color for information
- Lock icons include tooltip text explanation

---

## Technical Implementation

### File Structure

```
client/src/
├── components/
│   └── pages/
│       └── HomePage/
│           ├── HomePage.tsx              # Main page component
│           ├── index.ts                  # Export
│           ├── sections/
│           │   ├── HeroSection.tsx       # Hero with auth switching
│           │   ├── QuickSearchSection.tsx
│           │   ├── FeatureHighlights.tsx
│           │   ├── FeaturedSetsCarousel.tsx
│           │   ├── CollectionPreview.tsx
│           │   ├── ArtistSpotlight.tsx
│           │   └── BottomCTA.tsx
│           └── components/
│               ├── FeatureCard.tsx
│               ├── SetPreviewCard.tsx
│               └── StatBox.tsx
```

### Data Requirements

**Anonymous User**: No API calls required for MVP (static content)

**Authenticated User** (all computed client-side from existing data sources):
| Data | Source | Loading |
|------|--------|---------|
| User display name | Auth0/UserContext | Already available (no spinner) |
| Cards tracked count | Existing UserCards query | Individual spinner per stat box |
| Sets in progress count | Existing UserSetCards query | Individual spinner per stat box |
| Wishlist count | Existing `GET_USER_WISHLIST` query (count of results) | Individual spinner per stat box |
| Wishlist preview | Existing `GET_USER_WISHLIST` query (first 4-6 cards) | Shared spinner with wishlist count (same query) |

### Component Dependencies

| Component | MUI Components | Custom Components |
|-----------|---------------|-------------------|
| HeroSection | Box, Typography, Button, Stack, Grid | StatBox |
| QuickSearchSection | TextField, InputAdornment | - |
| FeatureHighlights | Grid, Card, CardContent, CardActions, Avatar, Tooltip | FeatureCard |
| FeaturedSetsCarousel | Box, IconButton, Typography, Link | SetPreviewCard |
| CollectionPreview | Box, Typography, LinearProgress, CircularProgress | Card thumbnails |
| ArtistSpotlight | Grid, Typography, Button | Card thumbnails |
| BottomCTA | Paper, Typography, Button, Link | - |

### State Management

```tsx
// HomePage.tsx
const HomePage: React.FC = () => {
  const { isAuthenticated, user } = useAuth0();
  const { userProfile } = useUser();

  return (
    <Box>
      <HeroSection
        isAuthenticated={isAuthenticated}
        user={user}
        userStats={userProfile?.stats}
      />
      <QuickSearchSection />
      <FeatureHighlights isAuthenticated={isAuthenticated} />
      <FeaturedSetsCarousel />
      <CollectionPreview isAuthenticated={isAuthenticated} />
      <ArtistSpotlight />
      <BottomCTA isAuthenticated={isAuthenticated} />
    </Box>
  );
};
```

---

## Implementation Phases

### Phase 1: MVP (Core Structure)

**Scope**:
- Hero section (anonymous version only)
- Quick search (basic - navigate to existing search pages)
- Feature highlights (4 cards, static)
- Bottom CTA (anonymous version only)

**Excludes**:
- Authenticated user personalization
- Featured sets carousel
- Collection preview
- Artist spotlight
- Animations

**Acceptance**: Anonymous users can understand the platform and navigate to key features.

### Phase 2: Content Sections

**Scope**:
- Featured sets carousel (using existing set data)
- Collection preview (mockup for anonymous)
- Artist spotlight (hardcoded featured artist)

**Excludes**:
- Authenticated user personalization
- Real user data in collection preview

**Acceptance**: Homepage showcases platform depth with engaging content.

### Phase 3: Personalization

**Scope**:
- Authenticated hero variant with stats dashboard
- Real user data in collection preview
- Recent sets for authenticated users
- Wishlist preview
- Authenticated bottom CTA (tips)

**Excludes**:
- User stats API (may need backend work)

**Acceptance**: Authenticated users see personalized, relevant content.

### Phase 4: Polish

**Scope**:
- Page load animations
- Scroll-triggered animations
- Hover state refinements
- Reduced motion support
- Performance optimization

**Acceptance**: Smooth, polished experience that respects user preferences.

### Post-Implementation: Backend API Evaluation

After all phases are complete, evaluate the final homepage implementation to identify opportunities for dedicated backend APIs that improve performance. Candidates include:
- Aggregated user stats endpoint (single query instead of multiple client-side computations)
- Featured content endpoint (artist spotlight, curated sets)
- Homepage dashboard endpoint (combined payload for authenticated users)
- Lightweight wishlist count endpoint (avoid fetching full card data just for a count)
- Review all homepage queries for optimization (e.g., `GET_USER_WISHLIST` returns full card data when only count + thumbnails are needed)
- **GraphQL paging, sorting, and filtering**: Evaluate adding these capabilities to existing endpoints (e.g., `allSets`, `userWishlist`) rather than creating new dedicated endpoints. This could enable homepage autocomplete search, more efficient carousel data, and lightweight stat queries through existing infrastructure.

### Post-Implementation: Code Cleanup

- **Remove `CardDisplayResponsive` dead code**: The `CardDisplay` component in `src/components/organisms/Cards/CardDisplayResponsive.tsx` is not used anywhere in the application. This file can be safely deleted along with any orphaned imports.
- **Remove `CardCompact` dead code**: The `CardCompact` component in `src/components/organisms/Cards/CardCompact.tsx` is not used anywhere in the application. The actual card display is handled by `MtgCard` (via `CardGrid`). This file can be safely deleted along with any orphaned imports.

---

## Clarifications

### Session 2026-01-27

- Q: How should the hero section handle empty state for brand-new authenticated users with zero data? → A: Show the authenticated hero with zero stats and encouraging CTAs (e.g., "0 Cards — Start tracking!")
- Q: Should user statistics (cards tracked, sets in progress, wishlist count) come from a new backend endpoint or be computed client-side? → A: Compute client-side from existing queries/contexts, enabling individualized loading spinners per stat box while each data source loads independently.
- Q: How should the featured artist be selected for the artist spotlight? → A: Hardcoded array in code, rotated manually with deployments. Post-task: evaluate the final homepage implementation to design backend APIs that improve homepage performance (e.g., aggregated stats endpoint, featured content endpoint).
- Q: What should happen when one or more authenticated stat data fetches fail? → A: Fall back to the anonymous hero if any stat fetch fails.

### Session 2026-01-27 (post-plan)

- Q: How should the Wishlist stat box get its count, given WishlistContext is a mutation dispatcher not a data store? → A: Use existing `GET_USER_WISHLIST` query — count for stat box, first 4-6 cards for CollectionPreview wishlist thumbnails, with CTA linking to wishlist page. Add to post-implementation backend API evaluation for query optimization review.
- Q: The authenticated hero wireframe shows "Upcoming Signings" (4th stat box) but no data source exists for signing events. How should the 4th stat box behave? → A: Remove the 4th stat box entirely. Show only 3 stats: Cards Tracked, Sets In Progress, Wishlist Items.
- Q: Should the featured sets carousel show latest by release date or a curated list? → A: Latest by release date, filtered to expansion + core set types.
- Q: Should homepage search use autocomplete or simple navigation? → A: Simple navigation to `/search/cards?q={term}` for MVP. Post-impl note: adding paging, sorting, and filtering capabilities to existing GraphQL endpoints could enable autocomplete and other homepage enhancements without creating new endpoints.
- Q: Should popular search suggestion chips be hardcoded or dynamic? → A: Remove popular search suggestions entirely. The search bar stands alone without suggestion chips.

---

## Open Questions

1. ~~**Featured Artist Data**~~: Resolved — hardcoded array in code, rotated manually with deployments.

2. ~~**User Stats API**~~: Resolved — compute client-side from existing queries/contexts with individual loading spinners per stat box.

3. ~~**Set Carousel Data**~~: Resolved — latest by release date, filtered to expansion + core set types.

4. ~~**Search Behavior**~~: Resolved — simple navigation to existing search pages for MVP.

5. ~~**Popular Searches**~~: Resolved — removed entirely. Search bar stands alone.

---

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Time to first meaningful interaction | < 3 seconds | Analytics |
| Bounce rate reduction | -20% from current | Analytics |
| Feature discovery | 50%+ visitors click a feature card | Analytics |
| Search usage from homepage | 30%+ visitors use search | Analytics |
| Conversion (anon → auth) | Track sign-up clicks | Analytics |

---

## References

- [Material-UI Components](https://mui.com/material-ui/)
- [Existing theme configuration](client/src/theme/index.ts)
- [Auth0 React SDK](https://auth0.com/docs/libraries/auth0-react)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
