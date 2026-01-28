# Implementation Plan: Homepage Redesign

**Branch**: `003-homepage-redesign` | **Date**: 2026-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-homepage-redesign/spec.md`

## Summary

Redesign the MTG Discovery homepage from a minimal developer-focused landing page to a 7-section, user-centric experience. The implementation is frontend-only (React 19 + Material-UI), requires no backend changes, and is phased across 4 releases: MVP core structure, content sections, authenticated personalization, and polish/animations. All authenticated user stats are computed client-side from existing GraphQL queries and contexts.

## Technical Context

**Language/Version**: TypeScript (React 19.1.1, ES2020+)
**Primary Dependencies**: Material-UI 7.3.1, Apollo Client 4.0.0, Auth0 React SDK 2.4.0, React Router DOM 7.9.1
**Storage**: N/A (frontend-only; data from existing GraphQL API and Auth0)
**Testing**: Vitest / React Testing Library (frontend component tests)
**Target Platform**: Web (desktop + mobile responsive, xs through xl breakpoints)
**Project Type**: Web application (frontend SPA)
**Performance Goals**: < 3s time to first meaningful interaction; hero text loads immediately without skeleton
**Constraints**: No new backend endpoints; no new npm dependencies for carousel (use CSS scroll-snap); Material-UI sx props only (no Tailwind); follow atomic design structure
**Scale/Scope**: 7 homepage sections, ~13 new component files, 4 implementation phases

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I. MicroObjects Architecture | N/A | Frontend feature - MicroObjects applies to .NET backend only |
| II. Layered Architecture Flow | N/A | No backend changes |
| III. Test-First Development | PASS | Component tests will use existing patterns (Vitest + RTL) |
| IV. Null Boundary Guards | N/A | Frontend - uses TypeScript null checks |
| V. Scope and Access Control | N/A | Frontend - no .NET scope rules |
| VI. Code Style Consistency | PASS | MUI sx props, atomic design, Props interfaces, generated GraphQL types |
| VII. NoArgsEntity Pattern | N/A | No backend changes |

**Frontend-Specific Constitution Gates:**

| Gate | Status | Notes |
|------|--------|-------|
| Material-UI only (no Tailwind) | PASS | All styling via sx props |
| Atomic design folder structure | PASS | New components follow atoms/molecules/organisms/pages pattern |
| Props interface for each component | PASS | All new components will have explicit Props interfaces |
| Generated GraphQL types only | PASS | Using existing generated hooks; no new manual type defs |
| Auth0 React SDK for auth | PASS | Using existing useAuth0 and useUser hooks |

**No violations requiring justification.**

## Project Structure

### Documentation (this feature)

```text
specs/003-homepage-redesign/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
│   └── component-contracts.md
└── tasks.md             # Phase 2 output (via /speckit.tasks)
```

### Source Code (repository root)

```text
client/src/
├── components/
│   └── pages/
│       └── HomePage/
│           ├── HomePage.tsx              # Main page orchestrator
│           ├── index.ts                  # Barrel export
│           ├── sections/                 # Section-level organisms
│           │   ├── HeroSection.tsx       # Hero with anonymous/auth/empty/error variants
│           │   ├── QuickSearchSection.tsx # Search bar
│           │   ├── FeatureHighlights.tsx  # 4 feature cards grid
│           │   ├── FeaturedSetsCarousel.tsx  # Horizontal scroll carousel
│           │   ├── CollectionPreview.tsx  # Demo (anon) / real data (auth)
│           │   ├── ArtistSpotlight.tsx    # Hardcoded featured artist
│           │   └── BottomCTA.tsx          # Sign-up (anon) / tips (auth)
│           ├── components/               # Homepage-specific sub-components
│           │   ├── FeatureCard.tsx        # Individual feature highlight card
│           │   ├── SetPreviewCard.tsx     # Set card for carousel
│           │   └── StatBox.tsx           # Stat display with independent loading
│           └── data/                     # Static data for homepage
│               ├── featuredArtists.ts    # Hardcoded artist spotlight data
│               ├── featureCards.ts       # Feature card configuration
│               └── proTips.ts            # Authenticated user tips
├── App.tsx                               # Updated: replace inline HomePage with import
```

**Canonical Source**: The authoritative file structure is defined in [spec.md](./spec.md) under "Technical Implementation > File Structure". This plan mirrors it for convenience but spec.md is the source of truth.

**Structure Decision**: Homepage components are co-located under `components/pages/HomePage/` following the pattern of complex pages having their own directory. Section-level components go in `sections/`, reusable sub-components in `components/`, and static data in `data/`. This avoids polluting the shared atoms/molecules/organisms folders with homepage-specific components while maintaining clear organization.

## Complexity Tracking

No violations to justify. This feature:
- Uses existing infrastructure (Auth0, Apollo Client, MUI theme)
- Adds no new backend endpoints
- Adds no new npm dependencies
- Follows established frontend patterns
