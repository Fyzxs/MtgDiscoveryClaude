# MTG Card Component System

This is a comprehensive component library for displaying Magic: The Gathering cards following Atomic Design principles.

## Component Hierarchy

### Atoms (Basic Building Blocks)
- **RarityBadge** - Displays card rarity with color-coded badges
- **ManaSymbol** - Individual mana symbol representation
- **CardImage** - Card image with lazy loading and error states
- **PriceDisplay** - Color-coded price display (green < $5, yellow $5-25, red > $25)
- **ExternalLinkIcon** - Icons for external services (Scryfall, TCGPlayer, etc.)
- **CollectorNumber** - Shows collector number with optional set code

### Molecules (Component Combinations)
- **ManaCost** - Full mana cost display using ManaSymbol atoms
- **ArtistInfo** - Artist attribution with context-aware display
- **CardLinks** - External link group for card resources
- **CollectorInfo** - Collector number + rarity badge combination
- **CardMetadata** - Card name, type, set, and date information

### Organisms (Complex Components)
- **CardDisplay** - Full card display with all information and hover state
- **CardCompact** - Compact card view optimized for grid layouts

## Usage

### Basic Card Display
```tsx
import { CardDisplay } from './components/organisms/CardDisplay';

<CardDisplay 
  card={cardData} 
  context={{
    isOnSetPage: false,
    showCollectorInfo: true
  }}
/>
```

### Compact Grid View
```tsx
import { CardCompact } from './components/organisms/CardCompact';

<div className="grid grid-cols-3 gap-4">
  {cards.map(card => (
    <CardCompact key={card.id} card={card} />
  ))}
</div>
```

## Context-Aware Display

Components adapt based on context:
- **On Set Page**: Hides set name, shows date only if different from set release
- **On Artist Page**: Shows artist only if multiple artists, displays other artists
- **On Card Page**: Hides card name
- **Collector Info**: Optional display based on context

## Styling

All components use Tailwind CSS with:
- Dark theme optimized for MTG aesthetic
- Rarity-based border glows
- Price-based color coding
- Responsive design
- Hover states and transitions

## Demo Page

Access the interactive demo at the root path to see all components with live data and context controls.