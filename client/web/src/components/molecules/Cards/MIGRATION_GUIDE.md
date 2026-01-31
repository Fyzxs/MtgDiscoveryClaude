# Migration Guide: DirectDomOverlay → CollectionEntryOverlay

This guide walks through replacing the direct DOM manipulation overlay with the new React/Material-UI component.

## Overview

**Old System**: `directDomOverlay.ts` - Direct DOM manipulation, CSS-in-JS injection, manual lifecycle management
**New System**: `CollectionEntryOverlay.tsx` - React component, Material-UI sx props, declarative state management

## Benefits of Migration

### 1. **React Integration**
- Declarative state management
- Proper React lifecycle
- Component composition
- Better testability

### 2. **Material-UI Consistency**
- Uses MUI theme system
- Responsive breakpoints
- sx props for styling
- Theme-aware colors

### 3. **Performance**
- Virtual DOM diffing
- Efficient re-renders
- No direct DOM queries
- Better memory management

### 4. **Maintainability**
- TypeScript props interface
- Component reusability
- Clear separation of concerns
- No global state pollution

## Step-by-Step Migration

### Step 1: Import the Component

```typescript
// Remove old import
- import { domOverlay } from '../utils/directDomOverlay';

// Add new import
+ import { CollectionEntryOverlay } from '../components/molecules/Cards';
```

### Step 2: Add State Management

```typescript
// Add overlay state to your component
const [overlayState, setOverlayState] = useState({
  visible: false,
  count: 0,
  isNegative: false,
  finish: 'non-foil' as CardFinish,
  special: 'none' as CardSpecial,
  flash: false
});
```

### Step 3: Update Card Container

```typescript
// OLD: Card container with no overlay
<Box data-card-id={card.id}>
  <CardImage src={card.imageUrl} />
</Box>

// NEW: Card container with overlay component
<Box sx={{ position: 'relative' }} data-card-id={card.id}>
  <CardImage src={card.imageUrl} />
  <CollectionEntryOverlay
    visible={overlayState.visible}
    count={overlayState.count}
    isNegative={overlayState.isNegative}
    finish={overlayState.finish}
    special={overlayState.special}
    mode={entryMode}
    variant={cardWidth < 300 ? 'compact' : 'full'}
    flash={overlayState.flash}
  />
</Box>
```

### Step 4: Replace DOM Manipulation Calls

```typescript
// OLD: Direct DOM manipulation
domOverlay.show(cardId, entryState);
domOverlay.hide(cardId);
domOverlay.flash(cardId);

// NEW: React state updates
setOverlayState({
  visible: true,
  count: entryState.count,
  isNegative: entryState.isNegative,
  finish: entryState.finish,
  special: entryState.special
});

setOverlayState(s => ({ ...s, visible: false }));

setOverlayState(s => ({ ...s, flash: true }));
setTimeout(() => setOverlayState(s => ({ ...s, flash: false })), 150);
```

### Step 5: Update Keyboard Handlers

```typescript
// OLD: Called domOverlay methods
const handleKeyPress = (event: KeyboardEvent) => {
  const num = parseInt(event.key);
  if (num >= 1 && num <= 9) {
    const newState = { ...entryState, count: num.toString() };
    domOverlay.show(cardId, newState);
  }
};

// NEW: Update React state
const handleKeyPress = (event: KeyboardEvent) => {
  const num = parseInt(event.key);
  if (num >= 1 && num <= 9) {
    setOverlayState(s => ({
      ...s,
      visible: true,
      count: num,
      isNegative: false
    }));
  }
};
```

### Step 6: Remove Lifecycle Management

```typescript
// OLD: Manual lifecycle management
useEffect(() => {
  domOverlay.ensureOverlay(cardId);
  return () => domOverlay.cleanup(cardId);
}, [cardId]);

// NEW: No lifecycle management needed - React handles it!
// Just render the component
```

### Step 7: Update Mode Switching

```typescript
// OLD: Set data attribute on body
document.body.dataset.entryMode = mode;

// NEW: Pass mode as prop
<CollectionEntryOverlay
  mode={entryMode}
  // ... other props
/>
```

## Complete Before/After Example

### Before (Direct DOM)

```typescript
import { domOverlay } from '../utils/directDomOverlay';

const CardComponent: React.FC<CardProps> = ({ card }) => {
  const [entryState, setEntryState] = useState({
    count: '0',
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    isNegative: false
  });

  useEffect(() => {
    domOverlay.ensureOverlay(card.id);
    return () => domOverlay.cleanup(card.id);
  }, [card.id]);

  const handleKeyPress = (event: KeyboardEvent) => {
    const num = parseInt(event.key);
    if (num >= 1 && num <= 9) {
      const newState = { ...entryState, count: num.toString() };
      setEntryState(newState);
      domOverlay.show(card.id, newState);
    }
  };

  const handleHide = () => {
    domOverlay.hide(card.id);
  };

  return (
    <Box data-card-id={card.id}>
      <CardImage src={card.imageUrl} />
    </Box>
  );
};
```

### After (React Component)

```typescript
import { CollectionEntryOverlay } from '../components/molecules/Cards';

const CardComponent: React.FC<CardProps> = ({ card, entryMode, cardWidth }) => {
  const [entryState, setEntryState] = useState({
    count: '0',
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    isNegative: false
  });

  const [overlayState, setOverlayState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    flash: false
  });

  const handleKeyPress = (event: KeyboardEvent) => {
    const num = parseInt(event.key);
    if (num >= 1 && num <= 9) {
      const newState = { ...entryState, count: num.toString() };
      setEntryState(newState);
      setOverlayState({
        visible: true,
        count: num,
        isNegative: false,
        finish: entryState.finish,
        special: entryState.special,
        flash: false
      });
    }
  };

  const handleHide = () => {
    setOverlayState(s => ({ ...s, visible: false }));
  };

  return (
    <Box sx={{ position: 'relative' }} data-card-id={card.id}>
      <CardImage src={card.imageUrl} />
      <CollectionEntryOverlay
        visible={overlayState.visible}
        count={overlayState.count}
        isNegative={overlayState.isNegative}
        finish={overlayState.finish}
        special={overlayState.special}
        mode={entryMode}
        variant={cardWidth < 300 ? 'compact' : 'full'}
        flash={overlayState.flash}
      />
    </Box>
  );
};
```

## Cleanup Checklist

After migration, remove the old system:

- [ ] Remove `directDomOverlay.ts` file
- [ ] Remove all `domOverlay` imports
- [ ] Remove `data-entry-mode` attributes on body element
- [ ] Remove overlay lifecycle `useEffect` hooks
- [ ] Remove overlay cleanup calls
- [ ] Verify no direct DOM queries for overlays
- [ ] Test across all card sizes
- [ ] Test both collection and wishlist modes
- [ ] Test error flash states
- [ ] Test keyboard shortcuts

## Testing Migration

### Manual Test Cases

1. **Show Overlay**
   - Press 1-9 keys
   - Overlay appears with correct number
   - Correct mode (collection/wishlist)
   - Correct variant (full/compact)

2. **Hide Overlay**
   - Press Escape or submit
   - Overlay fades out smoothly

3. **Error Flash**
   - Invalid operation
   - Red flash animation plays
   - Returns to normal state

4. **Finish/Special**
   - Cycle through finishes (F key)
   - Cycle through specials (S key)
   - Display updates correctly
   - No layout shift when special changes

5. **Negative Numbers**
   - Press minus key
   - Number shows with minus sign
   - Red color applied

6. **Responsive**
   - Test on mobile (xs)
   - Test on tablet (sm)
   - Test on desktop (lg)
   - Verify text scales properly

7. **Card Sizes**
   - Small binder cards (200px) - compact variant
   - Medium cards (300px) - full variant
   - Large cards (400px+) - full variant
   - Verify 33% height maintained

8. **Mode Switching**
   - Switch to wishlist mode
   - Colors update (pink/purple)
   - Label updates
   - Switch back to collection
   - Colors revert

## Common Issues & Solutions

### Issue: Overlay not visible
**Solution**: Ensure parent has `position: relative`

### Issue: Overlay wrong size
**Solution**: Verify parent card container has explicit dimensions

### Issue: Text too small/large
**Solution**: Check variant prop - use `compact` for cards < 300px

### Issue: Flash animation not working
**Solution**: Ensure flash prop triggers, then resets after 150ms

### Issue: Layout shift when special changes
**Solution**: Component already handles this with fixed height - verify latest version

### Issue: Wrong colors in wishlist mode
**Solution**: Ensure `mode` prop is set correctly to 'wishlist'

## Performance Considerations

### React Rendering

The new component is optimized for performance:

1. **Minimal Re-renders**: Only re-renders when props change
2. **CSS Animations**: Uses GPU-accelerated CSS keyframes
3. **No Layout Thrashing**: Fixed heights prevent reflows
4. **Will-Change Hints**: Optimizes for animations

### Memory Usage

- Old system: Global singleton with Map of overlays
- New system: Component instances managed by React
- Result: Better memory management with React's garbage collection

## Rollback Plan

If issues arise during migration:

1. Keep `directDomOverlay.ts` temporarily
2. Migrate one component at a time
3. A/B test old vs new
4. Monitor for performance issues
5. Complete migration after validation

## Questions?

Review the following files for reference:
- `CollectionEntryOverlay.tsx` - Component implementation
- `CollectionEntryOverlay.README.md` - Full documentation
- `CollectionEntryOverlay.example.tsx` - Usage examples
- `directDomOverlay.ts` - Old implementation (for comparison)
