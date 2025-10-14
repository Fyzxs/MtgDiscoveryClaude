# Atoms Directory

This directory contains ALL atoms for the application:
1. **MUI Component Wrappers** - Minimal wrappers for Material-UI components (Box, Typography, Button, etc.)
2. **Domain-Specific Atoms** - Card atoms, Set atoms, shared atoms, layout atoms

## Architecture Principle

**NEVER import from `@mui/material` directly in your components.**

All MUI components must be imported from the `atoms` folder instead. This ensures:

1. **Consistent abstraction layer** - All external UI library usage goes through atoms
2. **Easy library migration** - If we ever switch UI libraries, changes are isolated to atoms
3. **Type safety** - Clean type names without "Mui" prefix clutter
4. **No raw HTML** - All UI uses Material-UI components, no raw `<div>`, `<span>`, etc.

## Usage

### ✅ Correct Usage

```typescript
// In ANY component (molecule, organism, template, page)
import { Box, Typography, Button } from '../atoms';
// or with appropriate path
import { Box, Typography, Button } from '@/components/atoms/mui';

function MyComponent() {
  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h1">Hello</Typography>
      <Button>Click me</Button>
    </Box>
  );
}
```

### ❌ Incorrect Usage

```typescript
// NEVER do this!
import { Box, Typography } from '@mui/material';

// NEVER use raw HTML
<div className="container">  // NO! Use <Box>
  <h1>Title</h1>            // NO! Use <Typography variant="h1">
  <span>Text</span>         // NO! Use <Typography variant="span">
</div>
```

## File Structure

The atoms directory is organized as follows:

```
atoms/
├── types.ts              // MUI type definitions
├── index.ts              // Barrel file - exports ALL atoms (MUI + domain)
├── README.md             // This file
│
├── Box.tsx               // MUI component wrappers
├── Typography.tsx
├── Button.tsx
├── ... (32 total MUI wrappers)
│
├── Cards/                // Card-specific atoms
│   ├── index.ts
│   ├── CardName.tsx
│   ├── RarityBadge.tsx
│   └── ...
│
├── Sets/                 // Set-specific atoms
│   ├── index.ts
│   ├── SetIcon.tsx
│   └── ...
│
├── shared/               // Shared atoms
│   ├── index.ts
│   ├── AppButton.tsx
│   ├── AppCard.tsx
│   └── ...
│
└── layouts/              // Layout atoms
    ├── index.ts
    └── ResponsiveGrid.tsx
```

### Example Wrapper (Box.tsx)

```typescript
import { Box as MuiBox } from '@mui/material';
import type { BoxProps } from './types';

const Box = (props: BoxProps) => {
  return <MuiBox {...props} />;
};

export default Box;
```

### Type Definitions (types.ts)

```typescript
import type { BoxProps as MuiBoxProps } from '@mui/material';

export type BoxProps = MuiBoxProps;
```

This pattern keeps types clean in application code - you use `BoxProps` not `MuiBoxProps`.

## Available Components

### Layout
- `Box` - Generic container (replaces `<div>`)
- `Container` - Responsive max-width container
- `Stack` - Flexbox layout helper
- `Paper` - Elevated surface
- `Divider` - Visual separator

### Typography
- `Typography` - All text elements (replaces `<h1>`, `<p>`, `<span>`, etc.)

### Buttons
- `Button` - Standard button
- `IconButton` - Icon-only button
- `Fab` - Floating action button

### Form Components
- `TextField` - Text input
- `Select` - Dropdown select
- `Checkbox` - Checkbox input
- `Switch` - Toggle switch
- `FormControl` - Form control wrapper
- `FormControlLabel` - Label for form inputs
- `InputLabel` - Input label
- `InputAdornment` - Input decorations (icons, text)
- `MenuItem` - Select menu item

### Feedback
- `Alert` - Alert messages
- `CircularProgress` - Loading spinner
- `LinearProgress` - Progress bar
- `Skeleton` - Loading placeholder

### Card Components
- `Card` - Card container
- `CardMedia` - Card image/media
- `CardContent` - Card content area
- `CardActionArea` - Clickable card area

### Other
- `Chip` - Chip/badge element
- `Link` - Hyperlink
- `Tooltip` - Tooltip overlay
- `Modal` - Modal dialog
- `Collapse` - Collapsible content
- `Icon` - Icon wrapper
- `Zoom` - Zoom transition

## Utilities & Types

Some MUI utilities and types are re-exported for convenience:

```typescript
import { useTheme, keyframes, type SxProps, type Theme } from '../atoms';
```

These don't need wrapping as they're not components.

## Migration Guide

### Step 1: Update Import Statement

```typescript
// BEFORE
import { Box, Typography, Button } from '@mui/material';

// AFTER
import { Box, Typography, Button } from '../atoms';
```

### Step 2: Replace Raw HTML with MUI Components

```typescript
// BEFORE
<div className="container">
  <h1>Title</h1>
  <p>Paragraph</p>
  <span>Text</span>
</div>

// AFTER
<Box className="container">
  <Typography variant="h1">Title</Typography>
  <Typography variant="body1">Paragraph</Typography>
  <Typography variant="span">Text</Typography>
</Box>
```

### Step 3: Verify Build

After changes, always verify:
```bash
npm run build
# or
npx tsc -b
```

## Why This Pattern?

### 1. Atomic Design Principle
Atoms are the smallest building blocks. By wrapping all MUI components in atoms, we ensure:
- **Atoms** = Single MUI components (Box, Button, Typography)
- **Molecules** = Combinations of atoms (ManaCost, CardLinks)
- **Organisms** = Complex molecules (CardGrid, Header)
- **Templates** = Page layouts
- **Pages** = Complete pages

### 2. Separation of Concerns
- **Atoms folder** = ONLY place where `@mui/material` is imported (in MUI wrapper files)
- **All other code** = Imports from `atoms`, never from `@mui/material`
- **Domain atoms** = Import MUI wrappers from `..` (parent atoms folder), not from `@mui/material`

### 3. Easy Refactoring
If we need to:
- Customize a component globally
- Switch UI libraries
- Add tracking/analytics
- Modify behavior

We only change the atom wrappers, not every component file.

### 4. Type Safety Without Clutter
Instead of:
```typescript
import type { BoxProps as MuiBoxProps } from '@mui/material';
const MyComponent: React.FC<MuiBoxProps> = (props) => ...
```

We write:
```typescript
import type { BoxProps } from '../atoms';
const MyComponent: React.FC<BoxProps> = (props) => ...
```

## Best Practices

1. **Never use raw HTML elements** - Always use MUI atoms
2. **Import from atoms/mui only** - Never from `@mui/material`
3. **Keep atoms simple** - No logic, just pure wrappers
4. **Use molecules for composition** - Combine atoms into meaningful components
5. **Consistent file structure** - Each atom has ComponentName.tsx

## Examples

### Example 1: Replacing div with Box

```typescript
// BEFORE
<div style={{ padding: '16px', backgroundColor: '#f5f5f5' }}>
  Content
</div>

// AFTER
<Box sx={{ p: 2, bgcolor: 'grey.100' }}>
  Content
</Box>
```

### Example 2: Replacing h1/p with Typography

```typescript
// BEFORE
<h1 className="title">My Title</h1>
<p className="description">My description</p>

// AFTER
<Typography variant="h1" className="title">My Title</Typography>
<Typography variant="body1" className="description">My description</Typography>
```

### Example 3: Building a Domain Atom

```typescript
// atoms/Cards/CardName.tsx
import { Typography } from '..';  // Import from parent atoms folder

interface CardNameProps {
  name: string;
  isLegendary?: boolean;
}

export const CardName = ({ name, isLegendary }: CardNameProps) => (
  <Typography
    variant="h5"
    sx={{
      fontWeight: isLegendary ? 700 : 500,
      color: isLegendary ? 'gold' : 'inherit'
    }}
  >
    {name}
  </Typography>
);
```

## Future Enhancements

As the application evolves, we may:
1. Add application-specific props to atoms (e.g., `loading` state)
2. Add global analytics tracking
3. Add accessibility helpers
4. Add performance optimizations
5. Create specialized variants (e.g., `CardBox`, `FilterBox`)

All enhancements stay in the atoms layer, keeping other code clean.

## Questions?

If you need a MUI component that's not wrapped yet:
1. Check if it exists in `types.ts`
2. Create the wrapper following the pattern above
3. Add it to `index.ts` exports
4. Update this README

---

**Remember:** Atoms are simple wrappers. Complex behavior goes in molecules, organisms, or custom hooks.
