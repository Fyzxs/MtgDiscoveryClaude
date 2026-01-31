# Styles Folder

## Purpose
Reusable style definitions: animations, component-specific sx props, layout patterns, and touch targets. Styling uses MUI `sx` prop (not CSS files).

## Organization

```
styles/
  ├─ animations.ts      (Reusable animation definitions)
  ├─ cardStyles.ts      (Card component sx props)
  ├─ layoutStyles.ts    (Layout sx props)
  └─ touchTargets.ts    (Touch target size definitions)
```

## Styling Approach

**No CSS files.** All styling via MUI `sx` prop or styled components.

See: `styles/animations.ts` — Reusable keyframe animations (fadeIn, slideUp, pulse, etc.)

See: `styles/cardStyles.ts` — Card-specific sx props (cardContainerSx, cardImageSx, cardBadgeSx)

See: `styles/layoutStyles.ts` — Layout patterns (responsiveGridSx, sidebarLayoutSx, centerContentSx)

See: `styles/touchTargets.ts` — Touch target size definitions (36px-64px ranges)

## When to Create Style Definition

1. **Reused Across Components**: Pattern used in 2+ places
2. **Complex Styling**: Animation, gradient, multi-state styling
3. **Responsive Pattern**: Different layouts per breakpoint
4. **Accessibility**: Touch targets, focus states, contrast
5. **Theme-Dependent**: Colors, spacing, shadows from theme

**Don't extract to styles/:**
- Component-specific styles (keep in component)
- One-off styling (use sx prop directly)
- Simple properties (single color, single size)

## Responsive Design with Theme

Use MUI theme breakpoints. See: `styles/layoutStyles.ts` for responsive pattern examples.

## Accessibility

Always consider accessibility with touch targets >= 44px. See: `styles/touchTargets.ts` for guidelines.

## Guidelines

- ✓ Use `sx` prop, not CSS files
- ✓ Access theme via `(theme) => ({ ... })`
- ✓ Extract reused patterns to styles/ folder
- ✓ Use theme breakpoints for responsive design
- ✓ Follow MUI shadow levels
- ✓ Use theme color palette (not hardcoded colors)
- ✓ Define touch targets >= 44px
- ✓ Use animations from animations.ts
- ✓ Comment complex responsive logic
- ✓ Test on mobile devices

See: `styles/` directory for real style file examples.
