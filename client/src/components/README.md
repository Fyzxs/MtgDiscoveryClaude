# MTG Card Component System

This is a comprehensive component library for displaying Magic: The Gathering cards following Atomic Design principles with Material-UI components.

## Component Hierarchy

### Atoms (Basic Building Blocks)
#### Shared Components
- **AppButton** (`atoms/shared/AppButton.tsx`) - MUI Button wrapper with loading state
- **AppCard** (`atoms/shared/AppCard.tsx`) - MUI Card wrapper with consistent styling
- **AppInput** (`atoms/shared/AppInput.tsx`) - MUI TextField wrapper for forms
- **PriceDisplay** (`atoms/shared/PriceDisplay.tsx`) - Color-coded price display
- **DebouncedSearchInput** (`atoms/shared/DebouncedSearchInput.tsx`) - Search input with debouncing

#### Card-Specific Atoms
- **RarityBadge** - Displays card rarity with color-coded badges  
- **ManaSymbol** - Individual mana symbol representation
- **CardImage** - Card image with lazy loading and error states
- **ExternalLinkIcon** - Icons for external services (Scryfall, TCGPlayer, etc.)
- **CollectorNumber** - Shows collector number with optional set code

### Molecules (Component Combinations)
- **ManaCost** - Full mana cost display using ManaSymbol atoms
- **ArtistInfo** - Artist attribution with context-aware display
- **CardLinks** - External link group for card resources
- **CollectorInfo** - Collector number + rarity badge combination
- **CardMetadata** - Card name, type, set, and date information
- **CardImageDisplay** - Enhanced card image with flip functionality

### Organisms (Complex Components)
- **CardDisplayResponsive** - Full card display with responsive mobile/desktop layouts
- **CardCompact** - Compact card view optimized for grid layouts (converted to MUI sx props)
- **MtgCard** - Alternative card display component
- **CardDetailsModal** - Modal for detailed card information

## Usage

### Basic Card Display
Reference: `organisms/CardDisplayResponsive.tsx:21-28`

### Compact Grid View  
Reference: `organisms/CardCompact.tsx:11-16`

### App Components
Reference: `atoms/shared/AppButton.tsx:5-8` for props interface pattern

### MUI Styling Pattern
Reference: `organisms/CardCompact.tsx:44-58` for sx props usage

## Context-Aware Display

Components adapt based on context:
- **On Set Page**: Hides set name, shows date only if different from set release
- **On Artist Page**: Shows artist only if multiple artists, displays other artists
- **On Card Page**: Hides card name
- **Collector Info**: Optional display based on context

## Styling

All components use Material-UI with:
- Dark theme optimized for MTG aesthetic (`theme/index.ts:137-183`)
- Rarity-based colors and shadows (`theme/index.ts:160-168`)
- Custom MTG theme extensions (`theme/index.ts:266-305`)
- Responsive design using MUI breakpoints
- Hover states via sx props

### Migration from Tailwind
Components are being converted from Tailwind classes to MUI sx props:
- `className="bg-gray-900"` → `sx={{ bgcolor: 'grey.900' }}`
- `className="flex gap-4"` → `sx={{ display: 'flex', gap: 2 }}`
- Theme-based colors preferred over hardcoded values

## Demo Page

Access the interactive demo at the root path to see all components with live data and context controls.