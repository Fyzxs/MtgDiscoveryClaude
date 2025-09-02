# MTG Card Display Components

## Overview
Clean, properly-sized Magic: The Gathering card display components built with Material-UI following atomic design principles.

## Primary Components

### CardDisplayResponsive
The main responsive card display component at `src/components/organisms/CardDisplayResponsive.tsx`.
- **Responsive layouts**: Horizontal on mobile, vertical on desktop
- **Context-aware display**: Adapts based on CardContext
- **Hover interactions**: Desktop-only overlay with additional card details

### CardCompact  
Grid-optimized component at `src/components/organisms/CardCompact.tsx`.
- **MUI sx styling**: Converted from Tailwind to Material-UI sx props
- **Rarity-based glows**: Using theme.mtg.shadows.rarity colors
- **Fixed proportions**: Maintains MTG card aspect ratio

### Features
- **Theme-based colors**: Rarity colors defined in `src/theme/index.ts:160-168`
- **Responsive design**: Uses MUI breakpoint system
- **Clean overlay design**: Information displayed with theme gradients
- **Context-aware display**: Hides/shows elements based on page context
- **Interactive links**: Artist names, card names, and set names are clickable

### Usage
Reference `organisms/CardDisplayResponsive.tsx:11-19` for props interface.
Reference `App.tsx:8-11` for App* component import pattern.

## Display Information

The card shows the following information in the overlay:
1. **Collector Number** (#5 • LEA)
2. **Rarity Badge** (C/U/R/M indicator)
3. **Artist Name(s)** (clickable links)
4. **Card Name** (clickable link, hidden on card pages)
5. **Set Name with Icon** (clickable, hidden on set pages)
6. **Release Date** (conditional display)
7. **Price** (color-coded: green <$5, yellow $5-25, red >$25)
8. **External Links** (Scryfall, TCGPlayer, CardMarket)

## Grid Layouts with MUI

Reference existing page implementations:
- `pages/AllSetsPage.tsx` for set grid layouts
- `pages/CardSearchPage.tsx` for card search results
- `organisms/CardCompact.tsx:44-58` for MUI sx grid styling patterns

Use MUI Grid2 component or Box with sx props for responsive layouts.

## Context Settings

Reference `types/card.ts` for CardContext interface definition.
Reference existing page components for context usage patterns.

## GraphQL Integration

- **Generated Types**: `src/generated/` directory contains all GraphQL types
- **Apollo Client**: Configured in `src/config/apollo.ts` 
- **Schema**: Located in GraphQL API backend project

## Material-UI Theme Integration

- **Custom MTG Theme**: `src/theme/index.ts` extends MUI with MTG-specific colors
- **Rarity Colors**: `theme.palette.rarity` for common/uncommon/rare/mythic
- **Card Shadows**: `theme.mtg.shadows.rarity` for glow effects  
- **Responsive Dimensions**: `theme.mtg.dimensions.cardWidth` for breakpoint sizes

## Component Architecture

Reference `src/components/README.md` for complete atomic design structure.

Key file locations:
- **Atoms**: `atoms/shared/App*.tsx` for base UI components
- **Molecules**: `molecules/Cards/*` for card-specific composed components  
- **Organisms**: `organisms/Card*.tsx` for complete card displays
- **Theme**: `theme/index.ts:137-368` for complete styling system