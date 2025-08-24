# MTG Card Display Components

## Overview
Clean, properly-sized Magic: The Gathering card display components that match the visual style of the existing application.

## Main Component: `MtgCard`

The primary card display component located at `src/components/organisms/MtgCard.tsx`.

### Features
- **Fixed card width**: 280px (proper MTG card proportions)
- **Rarity-based shadow glow**: Common (gray), Uncommon (silver), Rare (gold), Mythic (orange)
- **Clean overlay design**: Information displayed at bottom with gradient background
- **Context-aware display**: Hides/shows elements based on page context
- **Interactive links**: Artist names, card names, and set names are all clickable

### Usage
```tsx
import { MtgCard } from './components/organisms/MtgCard';

<MtgCard 
  card={cardData}
  context={{
    isOnSetPage: false,
    isOnArtistPage: false,
    isOnCardPage: false
  }}
  onCardClick={(id) => navigateToCard(id)}
  onSetClick={(code) => navigateToSet(code)}
  onArtistClick={(name) => navigateToArtist(name)}
/>
```

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

## Responsive Grid Layouts

### Desktop (5 columns)
```tsx
<div className="grid grid-cols-5 gap-6">
  {cards.map(card => <MtgCard key={card.id} card={card} />)}
</div>
```

### Tablet (3 columns)
```tsx
<div className="grid grid-cols-3 gap-6">
  {cards.map(card => <MtgCard key={card.id} card={card} />)}
</div>
```

### Mobile (1-2 columns)
```tsx
<div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
  {cards.map(card => <MtgCard key={card.id} card={card} />)}
</div>
```

## Context Settings

Control what information is displayed based on the current page:

```tsx
const context: CardContext = {
  isOnSetPage: true,      // Hides set name
  isOnArtistPage: true,   // Shows only other artists if multiple
  isOnCardPage: true,     // Hides card name
  currentArtist: "Dan Frazier",  // For filtering on artist pages
  currentSetCode: "LEA"   // For date comparison
}
```

## GraphQL Configuration

- **Endpoint**: `https://localhost:65203/graphql`
- **Queries**: Located in `src/graphql/queries/cards.ts`
- **Types**: Located in `src/types/card.ts`

## Demo Page

Access the demo at `/card-demo` (or click "View Card Component Demo" from home).

The demo page includes:
- Single card display
- Grid layout (set page style)
- Context controls to test conditional rendering
- Card ID input for testing with real data
- Mobile view examples

## External Dependencies

- **Keyrune**: MTG set icons loaded from CDN
  ```html
  <link href="https://cdn.jsdelivr.net/npm/keyrune@latest/css/keyrune.min.css" rel="stylesheet" />
  ```

## Component Structure

```
src/components/
├── atoms/
│   ├── RarityBadge.tsx      # Rarity indicators
│   ├── PriceDisplay.tsx     # Color-coded prices
│   ├── CollectorNumber.tsx  # Collector info
│   ├── SetIcon.tsx          # Keyrune set icons
│   └── ExternalLinkIcon.tsx # External link icons
├── organisms/
│   └── MtgCard.tsx          # Main card component
└── pages/
    └── CardDemoPage.tsx     # Demo page
```