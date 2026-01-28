# Data Model: Homepage Redesign

**Feature**: 003-homepage-redesign | **Date**: 2026-01-27

## Overview

The homepage is a frontend-only feature. All data models are TypeScript interfaces used within React components. No new backend entities or database changes are required.

## Entities

### 1. FeatureCardData

Static configuration for the 4 feature highlight cards.

```typescript
interface FeatureCardData {
  id: string;                    // Unique identifier (e.g., 'browse-sets')
  icon: React.ComponentType;     // MUI icon component reference
  title: string;                 // Display title (e.g., 'Browse Sets')
  description: string;           // Short description text
  ctaLabel: string;              // Button label (e.g., 'Explore Sets')
  route: string;                 // Navigation route (e.g., '/sets')
  authRequired: boolean;         // Whether feature requires authentication
}
```

**Source**: Hardcoded array in `data/featureCards.ts`
**Cardinality**: Exactly 4 items (fixed)

---

### 2. FeaturedArtistData

Static configuration for the artist spotlight section.

```typescript
interface FeaturedArtistData {
  name: string;                  // Artist name (e.g., 'Magali Villeneuve')
  description: string;           // Short bio/description
  cardCount: number;             // Approximate number of MTG cards illustrated
  sampleCardNames: string[];     // 4 card names for sample works display
  featuredCardName: string;      // Card name for the large featured image
}
```

**Source**: Hardcoded array in `data/featuredArtists.ts`
**Cardinality**: Array of artists; one displayed at a time

---

### 3. ProTip

Configuration for authenticated user tips in the bottom CTA section.

```typescript
interface ProTip {
  title: string;                 // Tip headline (e.g., 'Pro Tip of the Day')
  body: string;                  // Tip description text
  ctaLabel: string;              // Action button text
  ctaRoute: string;              // Navigation target
}
```

**Source**: Hardcoded array in `data/proTips.ts`
**Cardinality**: 5-10 tips; one shown per visit (index rotated by day)

---

### 4. UserHomepageStats

Computed from existing data sources for the authenticated hero dashboard (3 stat boxes).

```typescript
interface UserHomepageStats {
  cardsTracked: number | null;   // null = loading
  setsInProgress: number | null; // null = loading
  wishlistCount: number | null;  // null = loading; from GET_USER_WISHLIST query
}
```

**Source**: Computed client-side
- `cardsTracked`: Sum of `userCollection.totalCards` from sets query
- `setsInProgress`: Count of sets where `userCollection.collecting.length > 0`
- `wishlistCount`: Count of cards returned by `GET_USER_WISHLIST` with `totalCount > 0`

**Lifecycle**: Computed on each homepage render for authenticated users. Falls back to anonymous hero on fetch failure.

---

### 5. SetPreview (derived from existing Set type)

Subset of existing Set data used in the featured sets carousel.

```typescript
// No new interface needed - uses existing generated Set type from GraphQL
// Relevant fields from allSets query response:
interface SetPreviewFields {
  id: string;
  code: string;
  name: string;
  releasedAt: string;
  cardCount: number;
  iconSvgUri: string;
  setType: string;
}
```

**Source**: Existing `GET_ALL_SETS` GraphQL query
**Filter**: `setType === 'expansion' || setType === 'core'`
**Sort**: `releasedAt` descending
**Limit**: First 12 results

---

## Component Props Interfaces

### HeroSectionProps

```typescript
interface HeroSectionProps {
  isAuthenticated: boolean;
  userName: string | undefined;
  stats: UserHomepageStats | null;  // null = not loaded or error
  statsError: boolean;              // true triggers fallback to anonymous hero
}
```

### QuickSearchSectionProps

```typescript
interface QuickSearchSectionProps {
  // No props needed - self-contained with internal state
}
```

### FeatureHighlightsProps

```typescript
interface FeatureHighlightsProps {
  isAuthenticated: boolean;  // Controls lock icon visibility
}
```

### FeaturedSetsCarouselProps

```typescript
interface FeaturedSetsCarouselProps {
  // No props needed - fetches own data via Apollo
}
```

### CollectionPreviewProps

```typescript
interface CollectionPreviewProps {
  isAuthenticated: boolean;
}
```

### ArtistSpotlightProps

```typescript
interface ArtistSpotlightProps {
  // No props needed - uses hardcoded data
}
```

### BottomCTAProps

```typescript
interface BottomCTAProps {
  isAuthenticated: boolean;
}
```

### StatBoxProps

```typescript
interface StatBoxProps {
  value: number | null;          // null shows CircularProgress spinner
  label: string;                 // Caption below the number
  ctaText?: string;              // Optional encouraging CTA for zero state
  onClick?: () => void;          // Optional click handler for navigation
}
```

### FeatureCardProps

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

### SetPreviewCardProps

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

---

## Data Flow Diagram

```
Auth0 (useAuth0)          Apollo Client (GET_ALL_SETS)
      │                           │
      ▼                           ▼
  UserContext              Sets Data (cache)
      │                           │
      ├── isAuthenticated         ├── Latest sets → FeaturedSetsCarousel
      ├── user.name               └── User collection data → Stats computation
      │                                     │
      ▼                                     ▼
  HomePage.tsx ──────────────────── UserHomepageStats
      │
      ├── HeroSection (anonymous OR authenticated+stats)
      ├── QuickSearchSection (static)
      ├── FeatureHighlights (static, auth-aware for lock icons)
      ├── FeaturedSetsCarousel (Apollo query)
      ├── CollectionPreview (auth-dependent)
      ├── ArtistSpotlight (hardcoded data)
      └── BottomCTA (auth-dependent)
```

---

## State Transitions

### Hero Section State Machine

```
                    ┌─────────────────┐
                    │   Anonymous     │
                    │   (default)     │
                    └────────┬────────┘
                             │ isAuthenticated = true
                             ▼
                    ┌─────────────────┐
                    │   Loading       │
                    │  (stat spinners)│
                    └────────┬────────┘
                             │
                    ┌────────┴────────┐
                    │                 │
              stats loaded      any fetch error
                    │                 │
                    ▼                 ▼
           ┌────────────────┐  ┌─────────────────┐
           │  Authenticated │  │  Fallback to     │
           │  Dashboard     │  │  Anonymous Hero  │
           └────────────────┘  └─────────────────┘
                    │
           stats all zero?
                    │
                    ▼
           ┌────────────────┐
           │  Empty State   │
           │  (zero + CTAs) │
           └────────────────┘
```

Note: "Empty State" is a visual variant of "Authenticated Dashboard" (same component, different display when values are 0), not a separate state.
