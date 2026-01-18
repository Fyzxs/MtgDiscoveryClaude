# Mobile Responsive Implementation Guide

## Overview

This guide provides a practical implementation roadmap for making the MTG Discovery website fully functional on mobile devices. It synthesizes the technical analysis with the specification framework and introduces a **view-only mode** for very small screens.

---

## Design Philosophy

### Mobile-First with Progressive Enhancement
- Design for smallest screens first, then enhance for larger
- Use MUI breakpoints consistently (`xs`, `sm`, `md`, `lg`, `xl`)
- Eliminate all Tailwind CSS usage in favor of MUI `sx` props

### View-Only Mode for Small Screens
Below a certain breakpoint (< 600px / `xs`), the site operates in **view-only mode**:
- All mutation operations (add to collection, remove, update counts) are **disabled**
- Users see a subtle indicator that editing requires a larger screen
- Simplifies touch interactions and reduces accidental edits
- Full functionality available on tablets and desktop

### Breakpoint Strategy
```typescript
const breakpoints = {
  xs: 0,      // 0-599px   - View-only mobile
  sm: 600,    // 600-899px - Full mobile (mutations enabled)
  md: 900,    // 900-1199px - Tablet
  lg: 1200,   // 1200-1535px - Desktop
  xl: 1536    // 1536px+   - Large desktop
};
```

---

## Phase 1: Foundation Infrastructure

### Task 1.1: Create View-Only Mode Context

**Purpose:** Provide app-wide awareness of whether mutations should be enabled

**File:** `client/src/contexts/ViewModeContext.tsx`

**Implementation:**
```typescript
interface ViewModeContextValue {
  isViewOnly: boolean;        // true when < sm breakpoint
  isMobile: boolean;          // true when < md breakpoint
  isTouch: boolean;           // true when device has touch
  breakpoint: 'xs' | 'sm' | 'md' | 'lg' | 'xl';
}
```

**Behavior:**
- Uses `useMediaQuery` to detect current breakpoint
- Uses `@media (hover: hover)` to detect touch vs pointer devices
- Provides `isViewOnly` flag for components to disable mutations
- Re-evaluates on window resize

**Acceptance Criteria:**
- [ ] Context created and exported
- [ ] Hook `useViewMode()` available
- [ ] Correctly identifies breakpoints on load and resize
- [ ] Touch detection works

---

### Task 1.2: Create Mobile Utility Hooks

**Purpose:** Centralize responsive behavior detection

**Files:**
- `client/src/hooks/useBreakpoint.ts` - Current breakpoint detection
- `client/src/hooks/useTouchDevice.ts` - Touch capability detection
- `client/src/hooks/useResponsiveValue.ts` - Get value based on breakpoint

**Implementation Pattern:**
```typescript
// useResponsiveValue example
function useResponsiveValue<T>(values: { xs?: T; sm?: T; md?: T; lg?: T; xl?: T }): T {
  const breakpoint = useBreakpoint();
  return values[breakpoint] ?? values.xs;
}
```

**Acceptance Criteria:**
- [ ] Hooks created and tested
- [ ] Work with SSR (handle window undefined)
- [ ] Efficient (debounced resize listener)

---

### Task 1.3: Create View-Only Indicator Component

**Purpose:** Inform users when they're in view-only mode

**File:** `client/src/components/atoms/shared/ViewOnlyBanner.tsx`

**Implementation:**
- Subtle, non-intrusive banner at bottom of screen
- Shows only on `xs` breakpoint
- Message: "Viewing mode - rotate device or use larger screen to edit collection"
- Dismissible for session

**Acceptance Criteria:**
- [ ] Banner visible only on xs breakpoint
- [ ] Can be dismissed
- [ ] Doesn't interfere with content
- [ ] Accessible (proper ARIA)

---

## Phase 2: Critical Layout Fixes

### Task 2.1: Implement Mobile Navigation Drawer

**File:** `client/src/components/organisms/Header.tsx`

**Current Issue:** Header navigation overflows on mobile, items cramped or cut off

**Implementation:**
1. Add hamburger menu button (visible on `xs` and `sm`)
2. Create MUI `Drawer` component for mobile navigation
3. Move all nav items into drawer on mobile
4. Keep minimal header: logo + hamburger + search icon

**Mobile Header Structure:**
```
[Hamburger] [Logo] [Search Icon] [User Avatar]
```

**Drawer Contents:**
- Search input (full width)
- All Sets link
- Card Search link
- User info / Login button
- View-only mode indicator (if applicable)

**Responsive Visibility:**
```typescript
// Desktop nav
sx={{ display: { xs: 'none', md: 'flex' } }}

// Mobile hamburger
sx={{ display: { xs: 'flex', md: 'none' } }}
```

**Acceptance Criteria:**
- [ ] Hamburger menu visible on screens < 900px
- [ ] Desktop navigation hidden on screens < 900px
- [ ] Drawer opens from right side
- [ ] All navigation options accessible in drawer
- [ ] Search input works in drawer
- [ ] Drawer closes on item click
- [ ] Drawer closes on outside tap/swipe
- [ ] Auth state displays correctly in drawer

---

### Task 2.2: Fix ResponsiveGrid Overflow

**File:** `client/src/components/molecules/layouts/ResponsiveGrid.tsx`

**Current Issue:** Grid items overflow when container < minItemWidth

**Fix:**
```typescript
// Before
gridTemplateColumns: `repeat(auto-fill, ${itemWidth})`

// After
gridTemplateColumns: `repeat(auto-fill, minmax(min(${itemWidth}, 100%), 1fr))`
```

**Acceptance Criteria:**
- [ ] Grid items never exceed container width
- [ ] Single column works when container is narrow
- [ ] Gaps maintained at all sizes
- [ ] Desktop layouts unchanged

---

### Task 2.3: Make MtgSetCard Responsive

**File:** `client/src/components/molecules/Sets/MtgSetCard.tsx`

**Current Issue:** Fixed 240px width causes overflow on narrow screens

**Fix:**
```typescript
// Before
height: '360px',
width: '240px'

// After
width: { xs: '100%', sm: '240px' },
maxWidth: '240px',
height: 'auto',
aspectRatio: '240 / 360'  // Maintain proportions
```

**Acceptance Criteria:**
- [ ] Cards scale to fit container on mobile
- [ ] Aspect ratio maintained
- [ ] No horizontal scroll on 320px viewport
- [ ] Desktop appearance preserved

---

## Phase 3: Touch Interaction & View-Only Integration

### Task 3.1: Create Mutation-Disabled Wrapper

**Purpose:** Wrap mutation components to respect view-only mode

**File:** `client/src/components/atoms/shared/MutationEnabled.tsx`

**Implementation:**
```typescript
interface MutationEnabledProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;  // What to show when disabled
  showTooltip?: boolean;       // Show "requires larger screen" tooltip
}

const MutationEnabled: React.FC<MutationEnabledProps> = ({
  children,
  fallback = null,
  showTooltip = true
}) => {
  const { isViewOnly } = useViewMode();

  if (isViewOnly) {
    return showTooltip ? (
      <Tooltip title="Rotate device or use larger screen to edit">
        <Box component="span" sx={{ opacity: 0.5, pointerEvents: 'none' }}>
          {fallback || children}
        </Box>
      </Tooltip>
    ) : fallback;
  }

  return <>{children}</>;
};
```

**Usage:**
```typescript
<MutationEnabled>
  <AddToCollectionButton card={card} />
</MutationEnabled>
```

**Acceptance Criteria:**
- [ ] Wrapper hides/disables mutations on xs breakpoint
- [ ] Tooltip explains why disabled
- [ ] Visual indication (opacity/disabled state)
- [ ] Children render normally on sm+ breakpoints

---

### Task 3.2: Update Collection Components for View-Only

**Files to Update:**
- `client/src/components/molecules/Cards/QuickEntryKeysFab.tsx`
- Any "Add to Collection" buttons
- Any card count increment/decrement controls

**Implementation:**
- Wrap mutation triggers with `MutationEnabled`
- Hide FABs entirely on xs breakpoint (less clutter)
- Show read-only collection counts on mobile

**QuickEntryKeysFab Changes:**
```typescript
// Hide entirely on xs
sx={{
  display: { xs: 'none', sm: 'block' },
  // ... existing styles
}}
```

**Acceptance Criteria:**
- [ ] QuickEntryKeysFab hidden on xs breakpoint
- [ ] Add/remove buttons disabled or hidden on xs
- [ ] Collection counts visible but not editable on xs
- [ ] Full functionality on sm+ breakpoints

---

### Task 3.3: Implement Touch Target Compliance

**Files to Update:**
- `client/src/components/molecules/Cards/CardImageDisplay.tsx` (flip button)
- All `IconButton` components
- Interactive card overlays

**Theme Touch Values:**
```typescript
// Already in theme - use these!
theme.mtg.spacing.touch.minTarget  // 44
theme.mtg.spacing.touch.comfortable  // 48
```

**Implementation Pattern:**
```typescript
<IconButton
  sx={{
    minWidth: { xs: 44, sm: 'auto' },
    minHeight: { xs: 44, sm: 'auto' },
    // Visual size can be smaller, touch target larger
    '& .MuiSvgIcon-root': {
      fontSize: { xs: 20, sm: 24 }
    }
  }}
>
```

**Acceptance Criteria:**
- [ ] All interactive elements have 44px minimum touch target on mobile
- [ ] Visual appearance appropriate (not oversized)
- [ ] Theme touch values used consistently

---

## Phase 4: Component Responsive Updates

### Task 4.1: Fix AuthButton Truncation

**File:** `client/src/components/auth/AuthButton.tsx`

**Current Issue:** Long usernames overflow

**Fix:**
```typescript
<Typography
  sx={{
    maxWidth: { xs: 100, sm: 150, md: 200 },
    overflow: 'hidden',
    textOverflow: 'ellipsis',
    whiteSpace: 'nowrap'
  }}
>
  {user.name}
</Typography>
```

**Mobile Behavior:**
- On xs: Show avatar only (username in dropdown)
- On sm+: Show truncated username + avatar

**Acceptance Criteria:**
- [ ] Username truncates with ellipsis
- [ ] No overflow from auth section
- [ ] Avatar-only display on xs (optional)

---

### Task 4.2: Make QuickEntryKeysFab Panel Responsive

**File:** `client/src/components/molecules/Cards/QuickEntryKeysFab.tsx`

**Current Issue:** 320px panel exceeds viewport on narrow screens

**Note:** Since FAB is hidden on xs (Task 3.2), this fix targets sm breakpoint

**Fix:**
```typescript
// Panel width
width: { xs: 'calc(100vw - 32px)', sm: 280, md: 320 },
maxWidth: 320,

// Or use bottom sheet pattern on sm
```

**Acceptance Criteria:**
- [ ] Panel doesn't overflow on sm breakpoint (600px)
- [ ] Content remains usable
- [ ] Desktop appearance unchanged

---

### Task 4.3: Improve FilterPanel Mobile Layout

**File:** `client/src/components/organisms/filters/FilterPanel.tsx`

**Current Issue:** Controls stack awkwardly with inconsistent widths

**Fix Options:**

**Option A: Vertical Stack on Mobile**
```typescript
<Grid container spacing={{ xs: 1, sm: 2 }}>
  <Grid item xs={12} sm={6} md={3}>
    {/* Each control full width on xs */}
  </Grid>
</Grid>
```

**Option B: Filter Button + Bottom Sheet (Enhanced)**
- On mobile, collapse to "Filters" button
- Tap opens bottom sheet with all filter options
- More native mobile feel

**Acceptance Criteria:**
- [ ] All filter controls full-width on xs
- [ ] Consistent spacing
- [ ] All filters functional on mobile
- [ ] Desktop layout unchanged

---

### Task 4.4: Add Touch Alternative for Hover-Only Card Info

**File:** `client/src/components/organisms/CardDisplayResponsive.tsx`

**Current Issue:** Oracle text, flavor text, P/T only visible on hover (inaccessible on touch)

**Existing Infrastructure:**
- Touch state tracking (`isTouched`)
- Long press hooks (disabled by default)
- `@media (hover: hover)` detection

**Implementation Options:**

**Option A: Tap to Toggle Details**
```typescript
// On touch devices, single tap toggles info panel
const [showDetails, setShowDetails] = useState(false);

// For touch devices
onClick={() => {
  if (!hasHover) {
    setShowDetails(!showDetails);
  }
}}
```

**Option B: Always Show Essential Info on Mobile**
- P/T and type line always visible on mobile layout
- Full oracle text on tap or in detail view

**Option C: Info Button (Mobile Only)**
- Small "i" button visible on touch devices
- Tap opens card details overlay

**Recommended: Option A + B**
- Essential info (P/T, type) always visible on mobile
- Tap card to see full oracle text

**Acceptance Criteria:**
- [ ] Card details accessible on touch devices
- [ ] Clear visual indication of how to access details
- [ ] Desktop hover behavior unchanged
- [ ] P/T visible on mobile without interaction

---

### Task 4.5: Fix SearchInput Responsive Width

**File:** `client/src/components/molecules/shared/SearchInput.tsx`

**Current Issue:** Fixed pixel widths don't adapt to mobile

**Note:** With mobile drawer (Task 2.1), search moves into drawer on mobile

**For Drawer Search:**
```typescript
// Full width in drawer
<SearchInput
  expandedWidth="100%"
  collapsedWidth="100%"
  alwaysExpanded={true}  // No collapse in drawer
/>
```

**For Other Contexts (pages, filters):**
```typescript
// Support percentage/responsive widths
expandedWidth={{ xs: '100%', sm: 200, md: 250 }}
```

**Acceptance Criteria:**
- [ ] Search input full-width in mobile drawer
- [ ] Search functional at all screen sizes
- [ ] No overflow in any context

---

## Phase 5: Polish & Optimization

### Task 5.1: FAB Positioning System

**Files:**
- `client/src/components/molecules/Cards/QuickEntryKeysFab.tsx`
- `client/src/components/molecules/shared/BackToTopFab.tsx`

**Issue:** Multiple FABs may overlap

**Implementation:**
```typescript
// Establish positioning hierarchy
const fabPositions = {
  primary: { bottom: 16, right: 16 },      // BackToTop
  secondary: { bottom: 16, right: 80 },    // QuickEntry
  tertiary: { bottom: 80, right: 16 },     // Future use
};
```

**On Mobile (sm):**
- Only show one FAB at a time, or use SpeedDial pattern
- BackToTop takes priority

**Acceptance Criteria:**
- [ ] FABs don't overlap
- [ ] Clear visual hierarchy
- [ ] Accessible at all screen sizes

---

### Task 5.2: Mobile Input Enhancements

**Files:** All input components

**Enhancements:**
```typescript
<TextField
  inputProps={{
    inputMode: 'search',        // Shows search keyboard
    enterKeyHint: 'search',     // Shows "Search" on enter key
  }}
/>

// For numeric inputs
inputProps={{
  inputMode: 'numeric',
  pattern: '[0-9]*'
}}
```

**Acceptance Criteria:**
- [ ] Search inputs show search keyboard
- [ ] Numeric inputs show number keyboard
- [ ] Enter key shows appropriate hint

---

### Task 5.3: Footer Readability

**File:** `client/src/components/organisms/Footer.tsx`

**Fix:**
```typescript
<Typography
  variant="body2"
  sx={{
    fontSize: { xs: '0.75rem', sm: '0.875rem' },
    lineHeight: { xs: 1.5, sm: 1.7 }
  }}
>
```

**Acceptance Criteria:**
- [ ] Footer text readable on mobile
- [ ] Appropriate font size for small screens

---

## Implementation Order (Recommended)

### Sprint 1: Core Infrastructure (Tasks 1.x, 2.x)
1. **Task 1.1** - ViewModeContext (enables view-only)
2. **Task 1.2** - Utility hooks
3. **Task 1.3** - ViewOnlyBanner
4. **Task 2.1** - Mobile Navigation Drawer (highest user impact)
5. **Task 2.2** - ResponsiveGrid fix (quick win)
6. **Task 2.3** - MtgSetCard responsive (quick win)

### Sprint 2: View-Only & Touch (Tasks 3.x)
7. **Task 3.1** - MutationEnabled wrapper
8. **Task 3.2** - Collection components view-only
9. **Task 3.3** - Touch target compliance

### Sprint 3: Component Updates (Tasks 4.x)
10. **Task 4.1** - AuthButton truncation
11. **Task 4.2** - QuickEntryKeysFab panel
12. **Task 4.3** - FilterPanel mobile
13. **Task 4.4** - Touch card info alternative
14. **Task 4.5** - SearchInput responsive

### Sprint 4: Polish (Tasks 5.x)
15. **Task 5.1** - FAB positioning
16. **Task 5.2** - Mobile input hints
17. **Task 5.3** - Footer readability

---

## Testing Requirements

### Device Testing Matrix
| Device | Width | Mode | Priority |
|--------|-------|------|----------|
| iPhone SE | 375px | View-only | High |
| iPhone 14 | 390px | View-only | High |
| iPhone 14 Max | 430px | View-only | Medium |
| Small Android | 360px | View-only | High |
| iPad Mini | 768px | Full | High |
| iPad | 820px | Full | Medium |
| iPad Pro | 1024px | Full | Medium |

### Test Scenarios

**View-Only Mode (xs < 600px):**
- [ ] Can browse all sets
- [ ] Can view card details
- [ ] Cannot add/remove from collection
- [ ] View-only banner displays
- [ ] Navigation drawer works
- [ ] No horizontal scrolling

**Mobile Full Mode (sm 600-899px):**
- [ ] All view features work
- [ ] Can add/remove from collection
- [ ] QuickEntryKeysFab visible and usable
- [ ] Touch targets adequate (44px+)

**Tablet/Desktop (md+):**
- [ ] All features work as before
- [ ] Hover interactions work
- [ ] No regression from changes

---

## Files Summary

### New Files to Create
```
client/src/contexts/ViewModeContext.tsx
client/src/hooks/useBreakpoint.ts
client/src/hooks/useTouchDevice.ts
client/src/hooks/useResponsiveValue.ts
client/src/components/atoms/shared/ViewOnlyBanner.tsx
client/src/components/atoms/shared/MutationEnabled.tsx
```

### Files to Modify
```
client/src/components/organisms/Header.tsx
client/src/components/molecules/layouts/ResponsiveGrid.tsx
client/src/components/molecules/Sets/MtgSetCard.tsx
client/src/components/molecules/Cards/QuickEntryKeysFab.tsx
client/src/components/molecules/Cards/CardImageDisplay.tsx
client/src/components/auth/AuthButton.tsx
client/src/components/organisms/filters/FilterPanel.tsx
client/src/components/organisms/CardDisplayResponsive.tsx
client/src/components/molecules/shared/SearchInput.tsx
client/src/components/molecules/shared/BackToTopFab.tsx
client/src/components/organisms/Footer.tsx
client/src/App.tsx (wrap with ViewModeProvider)
```

---

## Key Decisions Summary

| Decision | Choice | Rationale |
|----------|--------|-----------|
| View-only breakpoint | < 600px (xs) | Standard mobile portrait; editing on phones is error-prone |
| Navigation pattern | Drawer on mobile | Standard mobile UX, keeps header clean |
| Mutation handling | Disable + indicator | Simpler than complex touch gestures for editing |
| Touch card info | Tap to toggle + essential always visible | Best balance of accessibility and simplicity |
| FAB on mobile | Hide on xs, show on sm+ | Reduces clutter in view-only mode |
