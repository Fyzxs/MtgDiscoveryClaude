# Layout Atoms

This directory contains layout atoms - basic layout primitives that eliminate repeated layout patterns across the codebase.

## Philosophy

Layout atoms are foundational components that encapsulate common CSS layout patterns. They provide:
- **Single source of truth** for layout patterns
- **Reduced code duplication** (~30+ instances eliminated)
- **Consistent spacing and alignment** across the application
- **Easy customization** through sx prop

## Available Layout Atoms

### FlexBetween

A flex container with space-between justification and centered alignment.

**Common use case**: Headers, toolbars, navigation bars where you want items at opposite ends.

**Usage**:
```tsx
import { FlexBetween } from '../atoms';

<FlexBetween>
  <Logo />
  <NavigationMenu />
</FlexBetween>

// With custom styling
<FlexBetween sx={{ p: 2, bgcolor: 'background.paper' }}>
  <CardCount count={100} />
  <PriceDisplay price="$50.00" />
</FlexBetween>
```

**Replaces**:
```tsx
// Before
<Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
  {children}
</Box>

// After
<FlexBetween>
  {children}
</FlexBetween>
```

---

### CenteredColumn

A flex column with centered alignment.

**Common use case**: Vertically stacked content that needs to be centered (loading states, empty states, card displays).

**Usage**:
```tsx
import { CenteredColumn } from '../atoms';

<CenteredColumn>
  <CircularProgress />
  <Typography>Loading...</Typography>
</CenteredColumn>

// With spacing
<CenteredColumn sx={{ gap: 2, py: 4 }}>
  <EmptyStateIcon />
  <Typography variant="h6">No results found</Typography>
  <Button>Clear Filters</Button>
</CenteredColumn>
```

**Replaces**:
```tsx
// Before
<Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
  {children}
</Box>

// After
<CenteredColumn>
  {children}
</CenteredColumn>
```

---

### GridContainer

A CSS grid container with configurable columns and gap.

**Common use case**: Card grids, image galleries, dashboard layouts.

**Usage**:
```tsx
import { GridContainer } from '../atoms';

// Auto columns with 2 spacing gap
<GridContainer gap={2}>
  {cards.map(card => <CardDisplay key={card.id} card={card} />)}
</GridContainer>

// Fixed 3-column grid
<GridContainer columns={3} gap={3}>
  <StatCard label="Total" value={100} />
  <StatCard label="Collected" value={75} />
  <StatCard label="Remaining" value={25} />
</GridContainer>

// Responsive columns
<GridContainer
  columns="repeat(auto-fit, minmax(250px, 1fr))"
  gap={2}
>
  {items.map(item => <Item key={item.id} {...item} />)}
</GridContainer>
```

**Props**:
- `gap?: number | string` - Grid gap (default: 2)
- `columns?: string | number` - Grid template columns (default: 'auto')
  - Number: Creates N equal columns
  - String: Custom grid-template-columns value
- All BoxProps (sx, className, etc.)

**Replaces**:
```tsx
// Before
<Box sx={{ display: 'grid', gap: 2, gridTemplateColumns: 'repeat(3, 1fr)' }}>
  {children}
</Box>

// After
<GridContainer columns={3} gap={2}>
  {children}
</GridContainer>
```

---

## Guidelines

### When to Use

Use layout atoms when:
- ✅ You need a common layout pattern (flex-between, centered column, grid)
- ✅ The pattern is repeated across multiple components
- ✅ You want consistent spacing and alignment

### When NOT to Use

Don't use layout atoms when:
- ❌ You need highly custom, one-off layouts
- ❌ The layout has complex conditional logic
- ❌ You're building a molecule or organism with specific layout requirements

### Composition

Layout atoms compose beautifully:

```tsx
<FlexBetween sx={{ p: 2 }}>
  <CenteredColumn gap={1}>
    <Avatar />
    <Typography>Username</Typography>
  </CenteredColumn>

  <GridContainer columns={3} gap={1}>
    <IconButton><FavoriteIcon /></IconButton>
    <IconButton><ShareIcon /></IconButton>
    <IconButton><MoreIcon /></IconButton>
  </GridContainer>
</FlexBetween>
```

### Extending with sx

All layout atoms accept the `sx` prop for customization:

```tsx
<FlexBetween
  sx={{
    p: 2,
    borderBottom: 1,
    borderColor: 'divider',
    bgcolor: 'background.paper'
  }}
>
  {children}
</FlexBetween>
```

---

## Benefits

### Before Layout Atoms

**Repeated pattern found 30+ times:**
```tsx
<Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
  <Typography>Title</Typography>
  <Button>Action</Button>
</Box>
```

**Problems:**
- Code duplication
- Inconsistent property order
- Easy to miss alignment properties
- Verbose

### After Layout Atoms

**Clean, semantic pattern:**
```tsx
<FlexBetween>
  <Typography>Title</Typography>
  <Button>Action</Button>
</FlexBetween>
```

**Benefits:**
- ✅ Less code
- ✅ More readable
- ✅ Consistent behavior
- ✅ Single source of truth
- ✅ Easy to refactor globally

---

## Examples from Codebase

### Set Page Header

**Before:**
```tsx
<Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
  <SetTitle name={set.name} />
  <SetStats cardCount={set.cardCount} />
</Box>
```

**After:**
```tsx
<FlexBetween sx={{ mb: 2 }}>
  <SetTitle name={set.name} />
  <SetStats cardCount={set.cardCount} />
</FlexBetween>
```

### Card Grid

**Before:**
```tsx
<Box sx={{ display: 'grid', gap: 2, gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))' }}>
  {cards.map(card => <MtgCard key={card.id} card={card} />)}
</Box>
```

**After:**
```tsx
<GridContainer columns="repeat(auto-fit, minmax(250px, 1fr))" gap={2}>
  {cards.map(card => <MtgCard key={card.id} card={card} />)}
</GridContainer>
```

### Loading State

**Before:**
```tsx
<Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', py: 8 }}>
  <CircularProgress />
  <Typography sx={{ mt: 2 }}>Loading cards...</Typography>
</Box>
```

**After:**
```tsx
<CenteredColumn sx={{ py: 8, gap: 2 }}>
  <CircularProgress />
  <Typography>Loading cards...</Typography>
</CenteredColumn>
```

---

## TypeScript Support

All layout atoms are fully typed with BoxProps:

```tsx
import type { BoxProps } from '../atoms';

// FlexBetween accepts all Box props
const MyComponent: React.FC = () => (
  <FlexBetween
    component="header"
    role="banner"
    sx={{ /* ... */ }}
  >
    {/* ... */}
  </FlexBetween>
);

// GridContainer has additional props
interface GridContainerProps extends BoxProps {
  gap?: number | string;
  columns?: string | number;
}
```

---

## Future Enhancements

Potential additions to the layout atoms library:
- `FlexStart` - Flex with items at start
- `FlexEnd` - Flex with items at end
- `FlexCenter` - Flex with centered items
- `GridTwoColumn` - Common 2-column grid
- `GridThreeColumn` - Common 3-column grid
- `Stack` - Vertical or horizontal stack with spacing (may already exist in MUI)

---

## Related Documentation

- [Atomic Design Principles](../../docs/ATOMIC_DESIGN_ANALYSIS.md)
- [Component Guidelines](../../docs/COMPONENT_GUIDELINES.md)
- [Material-UI Theme](../../../theme/index.ts)
