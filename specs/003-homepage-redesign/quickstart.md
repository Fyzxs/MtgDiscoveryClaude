# Quickstart: Homepage Redesign

**Feature**: 003-homepage-redesign | **Date**: 2026-01-27

## Prerequisites

- Node.js installed
- Frontend dependencies installed: `cd client && npm install`
- Backend GraphQL API running (for sets data in carousel): `dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj`
- Auth0 environment variables configured in `client/.env.local`

## Implementation Order

### Phase 1: MVP (Core Structure)

1. **Create directory structure**
   ```bash
   mkdir -p client/src/components/pages/HomePage/{sections,components,data}
   ```

2. **Create static data files** (no dependencies)
   - `data/featureCards.ts` - 4 feature card configs

3. **Create sub-components** (depend on data files)
   - `components/FeatureCard.tsx`

4. **Create MVP sections** (depend on sub-components)
   - `sections/HeroSection.tsx` - Anonymous version only
   - `sections/QuickSearchSection.tsx`
   - `sections/FeatureHighlights.tsx`
   - `sections/BottomCTA.tsx` - Anonymous version only

5. **Create page orchestrator**
   - `HomePage.tsx` - Compose MVP sections
   - `index.ts` - Default export

6. **Update routing**
   - `App.tsx` - Replace inline HomePage with lazy import

### Phase 2: Content Sections

7. **Create remaining sub-components**
   - `components/SetPreviewCard.tsx`

8. **Create remaining data files**
   - `data/featuredArtists.ts`

9. **Create content sections**
   - `sections/FeaturedSetsCarousel.tsx` - With CSS scroll-snap
   - `sections/CollectionPreview.tsx` - Anonymous mockup version
   - `sections/ArtistSpotlight.tsx` - Hardcoded artist data

10. **Update HomePage** - Add Phase 2 sections

### Phase 3: Personalization

11. **Create authenticated sub-components**
    - `components/StatBox.tsx` - With independent loading spinner

12. **Create remaining data files**
    - `data/proTips.ts`

13. **Update existing sections for auth variants**
    - `sections/HeroSection.tsx` - Add authenticated dashboard + empty + error states
    - `sections/CollectionPreview.tsx` - Add real user data variant
    - `sections/BottomCTA.tsx` - Add pro tips variant

14. **Update HomePage** - Add stats computation, error handling, auth-aware props

### Phase 4: Polish

15. **Create animation utilities**
    - `useInView` hook (or inline in sections)
    - CSS keyframes for staggered fade-in

16. **Add animations to all sections**
    - Page load staggered fade-in
    - Scroll-triggered section reveals
    - Hover state refinements

17. **Add reduced motion support**
    - `prefers-reduced-motion` media query

18. **Performance optimization**
    - React.memo on static components
    - useMemo on computed data
    - Verify lazy loading works correctly

## Development Commands

```bash
# Start dev server
cd client && npm run dev

# Build to verify no errors
cd client && npm run build

# Run linting
cd client && npm run lint

# Generate GraphQL types (only if schema changes)
cd client && npm run codegen
```

## Verification Checklist

### Phase 1 (MVP)
- [ ] Homepage loads with hero, search, features, bottom CTA
- [ ] Hero shows value proposition text
- [ ] Search navigates to /search/cards with query
- [ ] Feature cards display with correct icons and descriptions
- [ ] Convention Signing shows lock icon when not authenticated
- [ ] Feature card CTAs navigate to correct routes
- [ ] Bottom CTA "Create Free Account" triggers Auth0 signup
- [ ] Responsive layout works on mobile and desktop
- [ ] Accessibility: semantic HTML, ARIA labels, keyboard navigation

### Phase 2 (Content)
- [ ] Featured sets carousel shows latest expansion/core sets
- [ ] Carousel scrolls horizontally with snap
- [ ] Desktop shows arrow navigation on hover
- [ ] Mobile supports touch swipe
- [ ] Set cards navigate to /set/{code}
- [ ] Collection preview shows static mockup
- [ ] Artist spotlight shows hardcoded artist with sample works
- [ ] "View All Cards" navigates to artist page

### Phase 3 (Personalization)
- [ ] Authenticated hero shows "Welcome back, {name}!"
- [ ] Stat boxes load independently with individual spinners
- [ ] Zero stats show "0" with encouraging CTAs
- [ ] Stats fetch error falls back to anonymous hero
- [ ] Collection preview shows real user set data
- [ ] Bottom CTA shows pro tip for authenticated users
- [ ] All authenticated features degrade gracefully when logged out

### Phase 4 (Polish)
- [ ] Page load fade-in animation plays
- [ ] Sections fade in on scroll
- [ ] Hover states smooth on feature cards and set cards
- [ ] prefers-reduced-motion disables all animations
- [ ] No layout shifts during load
- [ ] Lighthouse performance score > 90

## Key Files Reference

| File | Purpose |
|------|---------|
| `client/src/App.tsx` | Route definition (update import) |
| `client/src/theme/index.ts` | Theme values (gradients, shadows, rarity) |
| `client/src/contexts/UserContext.tsx` | Auth/user state |
| `client/src/graphql/queries/sets.ts` | `GET_ALL_SETS` query for carousel |
| `client/src/components/molecules/layouts/Section.tsx` | Semantic section wrapper |
| `client/src/components/molecules/shared/AppButton.tsx` | Reusable button with loading |
| `client/src/components/molecules/shared/AppCard.tsx` | Reusable card component |
