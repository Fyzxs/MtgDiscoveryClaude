# Component Contracts: Homepage Redesign

**Feature**: 003-homepage-redesign | **Date**: 2026-01-27

## Overview

This document defines the component API contracts for the homepage redesign. Since this is a frontend-only feature with no new backend endpoints, contracts are defined as React component interfaces and their expected behaviors.

---

## Page Component

### HomePage

**File**: `client/src/components/pages/HomePage/HomePage.tsx`
**Export**: Default export (for React.lazy compatibility)

```typescript
// No props - top-level page component
const HomePage: React.FC = () => { ... }
export default HomePage;
```

**Responsibilities**:
- Orchestrates all 7 homepage sections
- Reads auth state via `useAuth0()` and `useUser()`
- Computes `UserHomepageStats` for authenticated users
- Passes `isAuthenticated` and derived data to child sections
- Handles stats fetch error → sets `statsError` flag for hero fallback

**Dependencies**:
- `useAuth0` from `@auth0/auth0-react`
- `useUser` from `contexts/UserContext`
- `useQuery` from `@apollo/client` (for sets data)
- All 7 section components

---

## Section Components

### HeroSection

**File**: `sections/HeroSection.tsx`

```typescript
interface HeroSectionProps {
  isAuthenticated: boolean;
  userName: string | undefined;
  stats: UserHomepageStats | null;
  statsError: boolean;
}
```

**Behavior Contract**:
| Condition | Renders |
|-----------|---------|
| `isAuthenticated === false` | Anonymous hero: headline, subtitle, Browse Sets + Start Collecting buttons |
| `isAuthenticated && statsError` | Falls back to anonymous hero |
| `isAuthenticated && stats === null` | Authenticated layout with 3 StatBox spinners |
| `isAuthenticated && stats.cardsTracked === 0` (all zero) | Authenticated layout with "0" values + encouraging CTAs |
| `isAuthenticated && stats.cardsTracked > 0` | Authenticated dashboard with real numbers |

**Accessibility**:
- `<section aria-labelledby="hero-heading">`
- `<h1>` for anonymous, `<h2>` for authenticated
- Buttons have descriptive text (no icon-only)

---

### QuickSearchSection

**File**: `sections/QuickSearchSection.tsx`

```typescript
// No props
```

**Behavior Contract**:
- Renders a centered TextField with SearchIcon adornment
- On form submit: navigates to `/search/cards?q={searchTerm}`

**Accessibility**:
- `<section aria-labelledby="search-heading">`
- Search field: `aria-label="Search cards, sets, or artists"`
- Visually-hidden `<h2>` heading

---

### FeatureHighlights

**File**: `sections/FeatureHighlights.tsx`

```typescript
interface FeatureHighlightsProps {
  isAuthenticated: boolean;
}
```

**Behavior Contract**:
- Renders 4 FeatureCard components in a responsive grid (xs:12, sm:6, md:3)
- Auth-required features show lock icon with tooltip when `isAuthenticated === false`
- Auth-required feature CTAs navigate to route regardless of auth (protected route handles redirect)

**Accessibility**:
- `<section aria-labelledby="features-heading">`
- Each card: `role="article"`, `aria-labelledby="feature-{id}-title"`
- Lock icon: `aria-label="Sign in required to access this feature"`

---

### FeaturedSetsCarousel

**File**: `sections/FeaturedSetsCarousel.tsx`

```typescript
// No props - fetches own data
```

**Behavior Contract**:
- Fetches `GET_ALL_SETS` via Apollo
- Filters to `expansion` and `core` set types
- Sorts by `releasedAt` descending, takes first 12
- Renders horizontal scrollable container with CSS scroll-snap
- Desktop: arrow buttons on left/right (visible on hover)
- Mobile: native touch swipe, no arrows
- Loading: skeleton shimmer cards
- Empty/error: section hidden entirely
- "See All" link navigates to `/sets`

**Accessibility**:
- `<section aria-labelledby="sets-heading">`
- Container: `role="region"`, `aria-label="Featured sets carousel"`, `aria-roledescription="carousel"`
- Arrow buttons: `aria-label="Previous sets"` / `aria-label="Next sets"`
- Keyboard: Arrow Left/Right navigates, Enter/Space activates

---

### CollectionPreview

**File**: `sections/CollectionPreview.tsx`

```typescript
interface CollectionPreviewProps {
  isAuthenticated: boolean;
}
```

**Behavior Contract**:
| Condition | Renders |
|-----------|---------|
| `isAuthenticated === false` | Decorative mockup with progress bar and "Start Tracking" CTA |
| `isAuthenticated === true` | "Continue Where You Left Off" with recent sets progress + wishlist preview |

**Anonymous**: Static content, no data fetching.
**Authenticated**: Uses sets data (already fetched by parent) to show up to 3 sets the user is collecting, with completion percentages.

---

### ArtistSpotlight

**File**: `sections/ArtistSpotlight.tsx`

```typescript
// No props - uses hardcoded data
```

**Behavior Contract**:
- Selects one artist from hardcoded `featuredArtists` array
- Renders split layout: featured card image (left) + artist info (right) on desktop; stacked on mobile
- Shows 4 sample card names/images below
- "View All Cards" button navigates to `/artists/{artistName}`

**Accessibility**:
- `<section aria-labelledby="artist-heading">`
- Featured image has descriptive alt text

---

### BottomCTA

**File**: `sections/BottomCTA.tsx`

```typescript
interface BottomCTAProps {
  isAuthenticated: boolean;
}
```

**Behavior Contract**:
| Condition | Renders |
|-----------|---------|
| `isAuthenticated === false` | Sign-up CTA with headline, description, "Create Free Account" button, "Sign In" link |
| `isAuthenticated === true` | Pro tip of the day with tip text and contextual CTA button |

**Anonymous CTA**: "Create Free Account" triggers Auth0 sign-up flow via `loginWithRedirect({ authorizationParams: { screen_hint: 'signup' } })`.
**Authenticated Tips**: Selects tip from hardcoded array using date-based index.

---

## Sub-Components

### StatBox

```typescript
interface StatBoxProps {
  value: number | null;
  label: string;
  ctaText?: string;
  onClick?: () => void;
}
```

**Behavior**:
- `value === null`: Shows `<CircularProgress size={24} />`
- `value === 0 && ctaText`: Shows "0" with encouraging CTA text below
- `value > 0`: Shows formatted number (with comma separators)
- Click handler optional for navigation

### FeatureCard

```typescript
interface FeatureCardProps {
  icon: React.ComponentType;
  title: string;
  description: string;
  ctaLabel: string;
  route: string;
  authRequired: boolean;
  isAuthenticated: boolean;
}
```

**Behavior**:
- Renders MUI Card with icon, title, description, CTA button
- `authRequired && !isAuthenticated`: Shows lock icon in top-right with tooltip
- Hover: `translateY(-4px)`, elevation 2 → 8, border highlight
- CTA button navigates to `route`

### SetPreviewCard

```typescript
interface SetPreviewCardProps {
  code: string;
  name: string;
  cardCount: number;
  releasedAt: string;
  iconSvgUri: string;
  onClick: (code: string) => void;
}
```

**Behavior**:
- Renders card with set icon (from SVG URI), name, card count, formatted release date
- Click navigates to `/set/{code}`
- Hover: `scale(1.02)`, `theme.mtg.shadows.card.hover`

---

## Navigation Contracts

| User Action | Target Route | Method |
|------------|--------------|--------|
| Click "Browse Sets" (hero) | `/sets` | `navigate('/sets')` |
| Click "Start Collecting" (hero) | Auth0 signup | `loginWithRedirect({ screen_hint: 'signup' })` |
| Submit search | `/search/cards?q={term}` | `navigate(...)` |
| Click feature card CTA | Feature route | `navigate(route)` |
| Click set in carousel | `/set/{setCode}` | `navigate(...)` |
| Click "See All" sets | `/sets` | `navigate('/sets')` |
| Click "View All Cards" (artist) | `/artists/{name}` | `navigate(...)` |
| Click "Create Free Account" | Auth0 signup | `loginWithRedirect(...)` |
| Click "Sign In" | Auth0 login | `loginWithRedirect()` |
| Click "Continue" (set) | `/set/{setCode}` | `navigate(...)` |
| Click "View All" (wishlist) | `/wishlist` | `navigate('/wishlist')` |
