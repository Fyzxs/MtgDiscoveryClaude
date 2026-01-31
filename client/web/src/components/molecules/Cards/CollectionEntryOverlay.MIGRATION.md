# CollectionEntryOverlay Migration Guide

This guide helps you migrate from the old `variant='full'/'compact'` approach to the new `variant='card'/'sealed'` approach.

## Breaking Changes Summary

### Variants Changed
- `variant='full'` → `variant='card'`
- `variant='compact'` → `variant='card'` (for MTG cards) or `variant='sealed'` (for sealed products)

### Prop Structure Changed
```typescript
// OLD Props Interface
interface CollectionEntryOverlayProps {
  visible: boolean;
  count: number;
  isNegative: boolean;
  finish: CardFinish;        // Always required
  special?: CardSpecial;
  mode: 'collection' | 'wishlist';
  variant: 'full' | 'compact';  // Size-based
  flash?: boolean;
}

// NEW Props Interface
interface CollectionEntryOverlayProps {
  visible: boolean;
  count: number;
  isNegative: boolean;
  mode: 'collection' | 'wishlist';
  variant: 'card' | 'sealed';    // Content-based
  flash?: boolean;

  // Card-specific props (required when variant='card')
  finish?: CardFinish;
  special?: CardSpecial;
}
```

## Why This Change?

### The Problem with Old Approach

The old approach was **fundamentally wrong** because it hid critical information based on card size:

```typescript
// WRONG - Users couldn't see finish/special on small cards
const variant = cardWidth < 300 ? 'compact' : 'full';
<CollectionEntryOverlay
  variant={variant}  // compact = hidden finish/special
  finish="foil"
  special="signed"
/>
```

This created UX problems:
1. Users couldn't tell if they were adding foil or non-foil cards in binder view
2. Special status was invisible on small cards
3. Size-based logic was fragile and inconsistent

### The New Solution

The new approach **never hides information**, only scales it:

```typescript
// CORRECT - Always shows finish/special, scales automatically
<CollectionEntryOverlay
  variant="card"  // Content-based, not size-based
  finish="foil"
  special="signed"
/>
```

Benefits:
1. All critical information always visible
2. Component handles responsive scaling automatically
3. Clearer separation between card and sealed product types
4. No manual size detection needed

## Migration Steps

### Step 1: Identify Your Use Cases

#### For MTG Cards
```typescript
// OLD
<CollectionEntryOverlay
  variant="full"  // or "compact"
  finish={finish}
  special={special}
  // ... other props
/>

// NEW
<CollectionEntryOverlay
  variant="card"
  finish={finish}
  special={special}
  // ... other props
/>
```

#### For Sealed Products
```typescript
// OLD
<CollectionEntryOverlay
  variant="compact"  // Maybe used for sealed?
  finish="non-foil"  // Dummy value
  // ... other props
/>

// NEW
<CollectionEntryOverlay
  variant="sealed"
  // No finish/special props
  // ... other props
/>
```

### Step 2: Remove Size-Based Logic

#### Before
```typescript
function CardDisplay({ card }) {
  const cardRef = useRef<HTMLDivElement>(null);
  const [cardWidth, setCardWidth] = useState(0);

  useEffect(() => {
    if (cardRef.current) {
      setCardWidth(cardRef.current.offsetWidth);
    }
  }, []);

  const variant = cardWidth < 300 ? 'compact' : 'full';

  return (
    <Box ref={cardRef} sx={{ position: 'relative' }}>
      <CardImage card={card} />
      <CollectionEntryOverlay
        variant={variant}
        finish={finish}
        special={special}
        {...otherProps}
      />
    </Box>
  );
}
```

#### After
```typescript
function CardDisplay({ card }) {
  // No size detection needed!
  return (
    <Box sx={{ position: 'relative' }}>
      <CardImage card={card} />
      <CollectionEntryOverlay
        variant="card"
        finish={finish}
        special={special}
        {...otherProps}
      />
    </Box>
  );
}
```

### Step 3: Update Type Checking

#### Before
```typescript
if (variant === 'full') {
  // Show finish/special
}
```

#### After
```typescript
if (variant === 'card') {
  // Card-specific logic
}
```

## Migration Examples

### Example 1: Simple Card Display

#### Before
```typescript
function SimpleCard({ card }) {
  return (
    <Box sx={{ position: 'relative' }}>
      <img src={card.imageUrl} alt={card.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={1}
        isNegative={false}
        finish="foil"
        special="none"
        mode="collection"
        variant="full"
      />
    </Box>
  );
}
```

#### After
```typescript
function SimpleCard({ card }) {
  return (
    <Box sx={{ position: 'relative' }}>
      <img src={card.imageUrl} alt={card.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={1}
        isNegative={false}
        mode="collection"
        variant="card"
        finish="foil"
        special="none"
      />
    </Box>
  );
}
```

**Changes:**
- `variant="full"` → `variant="card"`
- `mode` moved before `variant` for consistency
- That's it!

### Example 2: Binder Card with Size Detection

#### Before
```typescript
function BinderCard({ card, binderMode }) {
  const [size, setSize] = useState({ width: 0, height: 0 });
  const cardRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (cardRef.current) {
      const { offsetWidth, offsetHeight } = cardRef.current;
      setSize({ width: offsetWidth, height: offsetHeight });
    }
  }, [binderMode]);

  const variant = size.width < 300 ? 'compact' : 'full';

  return (
    <Box ref={cardRef} sx={{ position: 'relative' }}>
      <img src={card.imageUrl} alt={card.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={count}
        isNegative={false}
        finish={finish}
        special={special}
        mode="collection"
        variant={variant}
      />
    </Box>
  );
}
```

#### After
```typescript
function BinderCard({ card }) {
  // No size detection needed!
  return (
    <Box sx={{ position: 'relative' }}>
      <img src={card.imageUrl} alt={card.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={count}
        isNegative={false}
        mode="collection"
        variant="card"
        finish={finish}
        special={special}
      />
    </Box>
  );
}
```

**Changes:**
- Removed `size` state
- Removed `cardRef` ref
- Removed `useEffect` for size detection
- Changed `variant={variant}` to `variant="card"`
- Much simpler!

### Example 3: Conditional Rendering Based on Variant

#### Before
```typescript
function CardWithControls({ card }) {
  const variant = cardWidth < 300 ? 'compact' : 'full';

  return (
    <Box>
      <CollectionEntryOverlay
        variant={variant}
        finish={finish}
        special={special}
        {...props}
      />

      {variant === 'full' && (
        <FinishControls onChange={setFinish} />
      )}
      {variant === 'full' && (
        <SpecialControls onChange={setSpecial} />
      )}
    </Box>
  );
}
```

#### After
```typescript
function CardWithControls({ card }) {
  return (
    <Box>
      <CollectionEntryOverlay
        variant="card"
        finish={finish}
        special={special}
        {...props}
      />

      {/* Always show controls - overlay always shows the info */}
      <FinishControls onChange={setFinish} />
      <SpecialControls onChange={setSpecial} />
    </Box>
  );
}
```

**Changes:**
- No conditional rendering of controls
- Users can always see and change finish/special
- Controls match what overlay displays

### Example 4: Sealed Product

#### Before
```typescript
function SealedProduct({ product }) {
  return (
    <Box sx={{ position: 'relative' }}>
      <img src={product.imageUrl} alt={product.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={count}
        isNegative={false}
        finish="non-foil"  // Dummy value
        mode="collection"
        variant="compact"  // Used compact for simplicity
      />
    </Box>
  );
}
```

#### After
```typescript
function SealedProduct({ product }) {
  return (
    <Box sx={{ position: 'relative' }}>
      <img src={product.imageUrl} alt={product.name} />
      <CollectionEntryOverlay
        visible={overlayVisible}
        count={count}
        isNegative={false}
        mode="collection"
        variant="sealed"
        // No finish/special props!
      />
    </Box>
  );
}
```

**Changes:**
- `variant="compact"` → `variant="sealed"`
- Removed `finish` prop (not needed for sealed)
- Much clearer intent

### Example 5: Polymorphic Component

#### Before
```typescript
function UniversalOverlay({ item, itemType }) {
  const variant = itemType === 'card'
    ? (cardWidth < 300 ? 'compact' : 'full')
    : 'compact';

  return (
    <CollectionEntryOverlay
      variant={variant}
      finish={itemType === 'card' ? finish : 'non-foil'}
      special={itemType === 'card' ? special : undefined}
      {...props}
    />
  );
}
```

#### After
```typescript
function UniversalOverlay({ item, itemType }) {
  return (
    <CollectionEntryOverlay
      variant={itemType === 'card' ? 'card' : 'sealed'}
      finish={itemType === 'card' ? finish : undefined}
      special={itemType === 'card' ? special : undefined}
      {...props}
    />
  );
}
```

**Changes:**
- Variant based on content type, not size
- Clean conditional props
- No size detection

## Common Migration Patterns

### Pattern 1: Remove Width State

```typescript
// Before
const [width, setWidth] = useState(0);
const variant = width < 300 ? 'compact' : 'full';

// After
const variant = 'card'; // Just use 'card'
```

### Pattern 2: Remove Resize Observers

```typescript
// Before
useEffect(() => {
  const observer = new ResizeObserver(entries => {
    setWidth(entries[0].contentRect.width);
  });
  observer.observe(cardRef.current);
  return () => observer.disconnect();
}, []);

// After
// Delete this entire useEffect!
```

### Pattern 3: Simplify Conditional Logic

```typescript
// Before
{variant === 'full' && <FinishDisplay />}

// After
<FinishDisplay />  // Always show it
```

## Testing Your Migration

After migrating, verify:

1. **Small cards (150-250px)**: Finish and special are visible (tiny text)
2. **Medium cards (250-350px)**: Finish and special are readable
3. **Large cards (350px+)**: Finish and special are prominent
4. **Sealed products**: No finish/special displayed
5. **All breakpoints**: Overlay scales smoothly

## Troubleshooting

### Problem: TypeScript Error "finish is required"

```
Property 'finish' is missing in type
```

**Solution:** Add `finish` prop for card variant:
```typescript
<CollectionEntryOverlay
  variant="card"
  finish={finish}  // Add this
  special={special}
  {...otherProps}
/>
```

### Problem: Finish/Special Not Showing

**Solution:** Make sure you're using `variant="card"`, not `variant="sealed"`:
```typescript
// Wrong
<CollectionEntryOverlay variant="sealed" finish="foil" />

// Correct
<CollectionEntryOverlay variant="card" finish="foil" />
```

### Problem: Text Too Small on Tiny Cards

**Answer:** This is intentional! Users need to see the information. If it's too small to read comfortably, consider:
1. Using larger card sizes
2. Adding a tooltip/hover state
3. Accepting that tiny text is better than hidden text

### Problem: Layout Shift When Toggling Special

**Solution:** This should already be fixed in the new component. The special field always renders with `visibility: hidden` when 'none'.

## Rollback Plan

If you need to rollback:

1. Keep the old component as `CollectionEntryOverlay.old.tsx`
2. Import from the old file temporarily
3. File a bug report with specific issues
4. Migrate forward when issues are resolved

## Need Help?

Common issues:
- Variant still size-based? Search for `cardWidth`, `offsetWidth`, `ResizeObserver`
- Finish/special hidden? Check for conditional rendering based on `variant`
- TypeScript errors? Ensure `finish` is provided for `variant="card"`

## Summary Checklist

- [ ] Changed `variant="full"` to `variant="card"`
- [ ] Changed `variant="compact"` to `variant="card"` or `variant="sealed"`
- [ ] Removed all size detection logic (width state, refs, ResizeObserver)
- [ ] Removed conditional rendering based on variant
- [ ] Removed dummy `finish` props for sealed products
- [ ] Updated TypeScript types/interfaces
- [ ] Tested at multiple breakpoints (xs, sm, md, lg)
- [ ] Verified finish/special always visible for cards
- [ ] Verified sealed products show no finish/special
- [ ] Removed any variant-switching logic
