# CollectionEntryOverlay

A responsive overlay component that displays collection entry information for both MTG cards and sealed products. The overlay appears at the bottom 33% of a card/product image and shows the entry mode, count, and variant-specific details.

## Design Principle

**Never sacrifice data visibility for aesthetics.** Users MUST see finish/special information to add the correct card to their collection. This component always shows all critical information, scaling it down on smaller cards rather than hiding it.

## Features

- **Two content-based variants** (card vs sealed)
- **Always shows all critical information** (never hides finish/special data)
- **Intelligent responsive scaling** based on container size
- **Flash animation** for error states
- **Collection and wishlist mode** support
- **Smooth fade animations**

## Variants

### Card Variant (`variant='card'`)

Shows mode, count, finish, and special information. Used for MTG cards.

**Display:**
- Mode label at top
- Left side: Finish and special information (ALWAYS VISIBLE)
- Right side: Count number
- Both finish and special are always rendered to maintain consistent layout

**Required props:**
- `finish` - MUST be provided ('non-foil' | 'foil' | 'etched')
- `special` - Optional ('none' | 'signed' | 'artist-proof' | 'altered')

### Sealed Variant (`variant='sealed'`)

Shows mode and count only. Used for sealed products (booster boxes, etc.).

**Display:**
- Mode label at top
- Full-width count number (no left side details)
- Simpler layout since sealed products only track quantity

**Required props:**
- None beyond base props

## Props Interface

```typescript
interface CollectionEntryOverlayProps {
  // Required for all variants
  visible: boolean;              // Controls overlay visibility
  count: number;                 // Number to display (1-9 typically)
  isNegative: boolean;           // Shows count with minus sign (e.g., -2)
  mode: 'collection' | 'wishlist'; // Entry mode
  variant: 'card' | 'sealed';    // Content variant type
  flash?: boolean;               // Triggers red flash animation

  // Card-specific props (required when variant='card')
  finish?: CardFinish;           // 'non-foil' | 'foil' | 'etched'
  special?: CardSpecial;         // 'none' | 'signed' | 'artist-proof' | 'altered'
}
```

## Usage Examples

### Card Variant - Basic

```typescript
<CollectionEntryOverlay
  visible={true}
  count={3}
  isNegative={false}
  mode="collection"
  variant="card"
  finish="foil"
  special="signed"
/>
```

### Card Variant - Small Binder Card

```typescript
// Same props, but component automatically scales down text/spacing
<CollectionEntryOverlay
  visible={true}
  count={1}
  isNegative={false}
  mode="collection"
  variant="card"
  finish="non-foil"
  special="none"
/>
// Finish and special are ALWAYS visible, just smaller
```

### Sealed Variant

```typescript
<CollectionEntryOverlay
  visible={true}
  count={2}
  isNegative={false}
  mode="collection"
  variant="sealed"
/>
// No finish/special props needed
```

### Wishlist Mode

```typescript
<CollectionEntryOverlay
  visible={true}
  count={2}
  isNegative={false}
  mode="wishlist"
  variant="card"
  finish="etched"
  special="none"
/>
```

### Error Flash

```typescript
<CollectionEntryOverlay
  visible={true}
  count={0}
  isNegative={false}
  mode="collection"
  variant="card"
  finish="non-foil"
  special="none"
  flash={true}  // Red background flash
/>
```

### Removing from Collection

```typescript
<CollectionEntryOverlay
  visible={true}
  count={2}
  isNegative={true}  // Shows "-2" in red
  mode="collection"
  variant="card"
  finish="foil"
  special="none"
/>
```

## Responsive Scaling

The component automatically scales all elements based on screen size breakpoints:

| Breakpoint | Mode Label | Details Text | Count Number | Padding | Gap |
|------------|-----------|--------------|--------------|---------|-----|
| xs (0px+)  | 0.45rem   | 0.6rem       | 1.5rem       | 4px     | 2px |
| sm (600px+)| 0.55rem   | 0.75rem      | 2rem         | 8px     | 4px |
| md (900px+)| 0.65rem   | 0.9rem       | 3rem         | 8px     | 4px |
| lg (1200px+)| 0.75rem  | 1.1rem       | 4rem         | 8px     | 4px |

### Scaling Strategy

1. **Never hide information** - All critical data is always visible
2. **Tighter spacing on small cards** - Reduced padding and gaps
3. **Smaller text on small cards** - Font sizes scale down proportionally
4. **Generous spacing on large cards** - More breathing room
5. **Container-based detection** - Uses MUI breakpoints to detect size

### Fixed Heights (Special Type)

To prevent layout shift when special is absent:
- xs: 0.72rem
- sm: 0.9rem
- md: 1.08rem
- lg: 1.32rem

## Integration

### Basic Setup

```typescript
import { CollectionEntryOverlay } from '../components/molecules/Cards/CollectionEntryOverlay';

function CardDisplay({ card }) {
  const [overlayState, setOverlayState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    mode: 'collection' as 'collection' | 'wishlist'
  });

  return (
    <Box sx={{ position: 'relative', width: 300, height: 418 }}>
      <img src={card.imageUrl} alt={card.name} />

      <CollectionEntryOverlay
        visible={overlayState.visible}
        count={overlayState.count}
        isNegative={overlayState.isNegative}
        mode={overlayState.mode}
        variant="card"
        finish={overlayState.finish}
        special={overlayState.special}
      />
    </Box>
  );
}
```

### With Keyboard Shortcuts

```typescript
function CardWithKeyboard({ card }) {
  const [state, setState] = useState({ /* ... */ });

  useEffect(() => {
    const handleKeyPress = (e: KeyboardEvent) => {
      const num = parseInt(e.key);
      if (!isNaN(num) && num >= 1 && num <= 9) {
        setState(s => ({ ...s, visible: true, count: num }));
      } else if (e.key === 'Escape') {
        setState(s => ({ ...s, visible: false }));
      } else if (e.key === 'f') {
        // Toggle finish
        const finishes: CardFinish[] = ['non-foil', 'foil', 'etched'];
        setState(s => ({
          ...s,
          finish: finishes[(finishes.indexOf(s.finish) + 1) % 3]
        }));
      } else if (e.key === 's') {
        // Toggle special
        const specials: CardSpecial[] = ['none', 'signed', 'artist-proof', 'altered'];
        setState(s => ({
          ...s,
          special: specials[(specials.indexOf(s.special) + 1) % 4]
        }));
      } else if (e.key === 'm') {
        // Toggle mode
        setState(s => ({
          ...s,
          mode: s.mode === 'collection' ? 'wishlist' : 'collection'
        }));
      }
    };

    window.addEventListener('keydown', handleKeyPress);
    return () => window.removeEventListener('keydown', handleKeyPress);
  }, []);

  return (
    <Box sx={{ position: 'relative' }}>
      <CardImage card={card} />
      <CollectionEntryOverlay {...state} variant="card" />
    </Box>
  );
}
```

### Error Flash Example

```typescript
function CardWithValidation({ card }) {
  const [state, setState] = useState({ /* ... */ });

  const handleSubmit = async () => {
    try {
      await addToCollection(card.id, state.count, state.finish);
      setState(s => ({ ...s, visible: false }));
    } catch (error) {
      // Flash red on error
      setState(s => ({ ...s, flash: true, visible: true }));
      setTimeout(() => {
        setState(s => ({ ...s, flash: false }));
      }, 150);
    }
  };

  return (
    <Box sx={{ position: 'relative' }}>
      <CardImage card={card} />
      <CollectionEntryOverlay {...state} variant="card" />
    </Box>
  );
}
```

### Sealed Product Example

```typescript
function SealedProductCard({ product }) {
  const [state, setState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    mode: 'collection' as 'collection' | 'wishlist'
  });

  return (
    <Box sx={{ position: 'relative', width: 300, height: 300 }}>
      <img src={product.imageUrl} alt={product.name} />

      <CollectionEntryOverlay
        visible={state.visible}
        count={state.count}
        isNegative={state.isNegative}
        mode={state.mode}
        variant="sealed"
      />
    </Box>
  );
}
```

## Visual States

### Collection Mode
- Background: Black with transparency `rgba(0, 0, 0, 0.95)`
- Border: Blue `#1976d2`
- Label: "COLLECTION UPDATE"
- Count color: Blue (or red if negative)

### Wishlist Mode
- Background: Purple with transparency `rgba(45, 20, 45, 0.95)`
- Border: Pink `#e91e63`
- Label: "♡ WISHLIST ♡"
- Count color: Pink (or red if negative)

### Flash State
- Rapidly flashes red background `rgba(211, 47, 47, 0.95)`
- 150ms animation
- Used to indicate errors or invalid operations

## Layout Consistency

The special field always renders (with `visibility: hidden` when 'none') to prevent layout shifts when toggling special status.

```tsx
<Typography
  sx={{
    visibility: hasSpecial ? 'visible' : 'hidden',
    minHeight: { xs: '0.72rem', sm: '0.9rem', md: '1.08rem', lg: '1.32rem' }
  }}
>
  {specialDisplayName || '\u00A0'}
</Typography>
```

## Positioning

The overlay is positioned absolutely within its parent card container:
- Bottom: 0
- Height: 33% of card height
- Full width
- z-index: 1000

**Parent card must have `position: relative` for proper positioning.**

## Performance

- Uses `will-change: opacity` for smooth animations
- Backdrop blur effect with fallback
- Minimal re-renders with proper prop structure
- Box-sizing border-box for predictable layout

## Accessibility

- Semantic HTML structure
- Proper z-index layering (1000)
- Pointer events disabled when not visible
- Smooth transitions with `will-change` optimization
- High contrast text colors
- Readable font sizes at all breakpoints

## Migration from Old Variants

**BREAKING CHANGE:** Variants have changed from `'full'/'compact'` to `'card'/'sealed'`.

### Old Approach (WRONG)
```typescript
// This was wrong - hiding finish/special on small cards
<CollectionEntryOverlay
  variant={cardWidth < 300 ? 'compact' : 'full'}  // WRONG
  finish="foil"
  special="signed"
/>
```

### New Approach (CORRECT)
```typescript
// Always show finish/special for cards, scale down automatically
<CollectionEntryOverlay
  variant="card"  // Content-based, not size-based
  finish="foil"
  special="signed"
/>
// Component handles scaling automatically
```

### Migration Guide

1. **For MTG cards**: Change `variant="full"` or `variant="compact"` to `variant="card"`
2. **For sealed products**: Change to `variant="sealed"` and remove `finish`/`special` props
3. **Remove size-based logic**: Delete any code that switches variants based on card size
4. **Trust responsive scaling**: The component now handles all sizing automatically

### Before
```typescript
const variant = cardWidth < 300 ? 'compact' : 'full';
<CollectionEntryOverlay variant={variant} finish={finish} special={special} />
```

### After
```typescript
<CollectionEntryOverlay variant="card" finish={finish} special={special} />
```

See `MIGRATION.md` for detailed migration examples.
