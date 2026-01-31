# Theme Folder

## Purpose
MUI theme configuration: colors, typography, breakpoints, design tokens. Provides consistent visual language across the app. Dark theme optimized for MTG aesthetic with rarity-based colors.

## Organization

```
theme/
  ├─ index.ts              (Main theme export)
  └─ designTokens.ts       (Design system tokens and helpers)
```

## Theme Export (`index.ts`)

Main MUI theme configuration with dark mode optimized for Magic: The Gathering.

See: `theme/index.ts` for:
- Color palette (rarity colors, mana colors, status colors)
- Typography configuration
- Typography scale (xs-lg breakpoints)

## Design Tokens (`designTokens.ts`)

Responsive design system with helper functions.

See: `designTokens.ts` for:
- **Responsive Spacing**: `responsiveSpacing` object and `getResponsiveSpacing()` helper
- **Touch Targets**: `touchTargets` sizes (36px-64px)
- **Responsive Font Sizes**: Typography scale per breakpoint
- **Container Sizes**: Max-width breakpoints
- **Grid Configuration**: Responsive columns, gaps, padding
- **Card Dimensions**: Responsive card sizes (compact/normal/large)
- **Z-Index Scale**: Layering order for stacked elements
- **Animation Timing**: Durations and easing functions
- **Responsive Patterns**: hideOnMobile, showOnMobile, responsiveStack, responsiveAlign

## Using Theme in Components

**Option 1: useTheme Hook**
```typescript
const theme = useTheme()
<Box sx={{ color: theme.palette.primary.main }} />
```

**Option 2: Theme Function in sx Prop**
```typescript
<Box sx={(theme) => ({
  color: theme.palette.primary.main,
  padding: theme.spacing(2)
})} />
```

**Option 3: Design Tokens**
```typescript
import { getCardDimensions, getResponsiveSpacing } from '@/theme/designTokens'
<Box sx={getCardDimensions('normal')} />
```

## Theming Guidelines

- ✓ Always use theme values (never hardcode colors)
- ✓ Use responsive design tokens for spacing/sizing
- ✓ Follow touch target minimums (44px standard, 48px comfortable)
- ✓ Leverage z-index scale for layering
- ✓ Use animation durations/easings from tokens
- ✓ Ensure sufficient color contrast (AA or AAA)
- ✓ Use rarity colors consistently across app
- ✓ Test on mobile devices
- ✓ Keep theme DRY (reuse tokens, don't duplicate)

## Customizing Theme

To extend the theme:
1. Add color to theme palette in `index.ts`
2. Add token to `designTokens.ts`
3. Export helper function if applicable
4. Use in components

See: `theme/index.ts` and `theme/designTokens.ts` for complete configuration.
