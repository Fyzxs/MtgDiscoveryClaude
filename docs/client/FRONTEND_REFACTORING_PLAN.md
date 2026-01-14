# Frontend Architecture Refactoring Implementation Plan

**Based on:** FRONTEND_ARCHITECTURE_REVIEW.md
**Scope:** Component consolidation and atomic design corrections
**Created:** 2026-01-14

---

## Executive Summary

This plan addresses 4 major refactoring initiatives:
1. **Badge System Consolidation** - 11 badges → 5 unified components
2. **Duplicate Component Resolution** - FilterDrawer, SearchInput, LoadingContainer
3. **Atomic Design Corrections** - Move 5 misplaced atoms to molecules
4. **Barrel Export Updates** - Ensure correct layer boundaries

**Estimated effort:** 6-8 hours total
**Risk level:** Low-Medium (mostly file moves and import updates)

---

## Key Architectural Principle

**MUI controls should ONLY be imported in atoms.**

This ensures:
- Single place to apply consistent styling
- Centralized ARIA/accessibility modifications
- Consistent behavior across the application

**Implication:** All new badge/ui molecules must import from `../../atoms`, not from `@mui/material` directly.

---

## Phase 1: Atomic Design Corrections (Priority: Critical)

Move 5 components from atoms to molecules - they violate atomic design by composing multiple atoms with logic.

### 1.1 Create molecules/feedback/ Directory

**New files to create:**
- `src/components/molecules/feedback/index.ts`

### 1.2 Move ErrorAlert + LoadingContainer

These are always imported together (3 files use both).

| Component | From | To |
|-----------|------|-----|
| ErrorAlert.tsx | atoms/shared/ | molecules/feedback/ |
| LoadingContainer.tsx | atoms/shared/ | molecules/feedback/ |

**Files requiring import updates:**
- `src/components/organisms/Cards/RelatedCardsDisplay.tsx`
- `src/components/organisms/Cards/AllPrintingsDisplay.tsx`
- `src/components/molecules/Cards/RulingsDisplay.tsx`

**Barrel exports to update:**
- Remove from: `atoms/shared/index.ts`
- Add to: `molecules/feedback/index.ts`

### 1.3 Move DarkBadge

| Component | From | To |
|-----------|------|-----|
| DarkBadge.tsx | atoms/shared/ | molecules/ui/ |

**Files requiring import updates:**
- `src/components/atoms/shared/PriceDisplay.tsx`
- `src/components/atoms/Cards/CardName.tsx`
- `src/components/atoms/Cards/ArtistLink.tsx`

**Barrel exports to update:**
- Remove from: `atoms/shared/index.ts`
- Add to: `molecules/ui/index.ts`

### 1.4 Move BadgePill

| Component | From | To |
|-----------|------|-----|
| BadgePill.tsx | atoms/Cards/ | molecules/Cards/ |

**Files requiring import updates:**
- `src/components/molecules/Cards/RarityCollectorBadge.tsx` (becomes same-dir import)

**Barrel exports to update:**
- Remove from: `atoms/Cards/index.ts`
- Add to: `molecules/Cards/index.ts`

### 1.5 Move CollectionToast

| Component | From | To |
|-----------|------|-----|
| CollectionToast.tsx | atoms/Cards/ | molecules/feedback/ |

**Files requiring import updates:**
- `src/components/organisms/shared/NotificationToastStack.tsx` (component + ToastMessage type)

**Barrel exports to update:**
- Remove from: `atoms/Cards/index.ts`
- Add to: `molecules/feedback/index.ts` (include ToastMessage type export)

---

## Phase 2: FilterDrawer Deduplication (Priority: Critical)

Two implementations exist with overlapping functionality.

### Analysis

| File | Lines | Base Component | Key Features |
|------|-------|----------------|--------------|
| molecules/shared/FilterDrawer.tsx | 177 | SwipeableDrawer | Generic children, exports FilterDrawerTrigger |
| organisms/filters/FilterDrawer.tsx | 172 | Drawer | Uses FilterPanelConfig, Clear All button |

### Resolution

**Keep:** `organisms/filters/FilterDrawer.tsx`
**Reason:** Composes FilterPanel (organism), correctly placed in organisms layer

**Action:** Rename `molecules/shared/FilterDrawer.tsx` → `GenericDrawer.tsx`

**Files currently using molecules version:**
- Check all 7 files importing FilterDrawer and update to use organisms version or GenericDrawer

**Usages to update:**
- `src/components/pages/SetPage.tsx`
- `src/components/pages/AllSetsPage.tsx`
- `src/components/pages/WishlistPage.tsx`
- `src/components/pages/CardAllPrintingsPage.tsx`
- `src/components/pages/ArtistCardsPage.tsx`
- `src/components/templates/SetPageTemplate.tsx`
- `src/hooks/useMobileLayout.ts`

---

## Phase 3: SearchInput Consolidation (Priority: High)

Two search input components with overlapping features.

### Analysis

| File | Lines | Key Features |
|------|-------|--------------|
| SearchInput.tsx | 111 | Expandable, form/submit, simple state |
| DebouncedSearchInput.tsx | 207 | Debounce, skeleton loading, Tab nav, complex refs |

### Resolution

**Create:** Unified `SearchInput.tsx` with configurable behavior

**Proposed Props Interface:**
```typescript
interface UnifiedSearchInputProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;

  // Debounce behavior
  debounceMs?: number;        // If provided, enables debouncing

  // Expandable behavior
  expandable?: boolean;
  expandedWidth?: number;
  collapsedWidth?: number;

  // States
  loading?: boolean;          // Show skeleton
  disabled?: boolean;

  // Callbacks
  onSubmit?: () => void;      // Form submit
  onEnter?: (value: string) => void;  // Enter key

  // Styling
  size?: 'small' | 'medium';
  fullWidth?: boolean;
  minWidth?: number;
  sx?: SxProps<Theme>;
}
```

**Migration:**
1. Create new unified component
2. Update usages of both components
3. Remove old separate components

---

## Phase 4: Badge System Consolidation (Priority: High)

Consolidate 11 badge components into 5 unified components.

### Current Badge Inventory (11 components)

| Component | Location | Base | Usages |
|-----------|----------|------|--------|
| LastDeltaBadge (Cards) | atoms/Cards/ | Chip | 2 |
| LastDeltaBadge (Sealed) | atoms/Sealed/ | Chip | 1 |
| BadgePill | atoms/Cards/ → molecules/Cards/ | Box | 1 |
| DarkBadge | atoms/shared/ → molecules/ui/ | Box | 3 |
| ItemDateBadge | atoms/shared/ | Typography | 2 |
| SetDateBadge | atoms/shared/ | Chip | 3 |
| SetCodeBadge | atoms/Sets/ | Chip | 3 |
| RarityBadge | atoms/Cards/ | Chip | 4 |
| DigitalBadge | atoms/Sets/ | Chip | 2 |
| FoilOnlyBadge | atoms/Sets/ | Chip | 2 |
| SetTypeBadge | atoms/Sets/ | Chip | 1 |

### Target Structure (5 components)

**Create:** `src/components/molecules/shared/badges/`

```
badges/
├── index.ts
├── AppBadge.tsx       // Base badge with variants
├── DeltaBadge.tsx     // Merges both LastDeltaBadge implementations
├── DateBadge.tsx      // Merges ItemDateBadge + SetDateBadge
├── StatusBadge.tsx    // Consolidates DigitalBadge, FoilOnlyBadge, SetTypeBadge
└── BadgePill.tsx      // Already moved in Phase 1
```

### AppBadge Variant System

```typescript
type BadgeVariant =
  | 'delta'    // +/- numbers with color
  | 'date'     // Formatted dates
  | 'code'     // Monospace set codes (SetCodeBadge)
  | 'rarity'   // Color-coded rarity (RarityBadge)
  | 'info'     // Info color (DigitalBadge)
  | 'foil'     // Golden foil style
  | 'type'     // Dynamic set type colors
  | 'dark'     // DarkBadge style
  | 'default';
```

### Migration Order

1. **DeltaBadge** - Merge Cards + Sealed LastDeltaBadge
2. **DateBadge** - Merge ItemDateBadge + SetDateBadge with variant prop
3. **StatusBadge** - Consolidate DigitalBadge, FoilOnlyBadge, SetTypeBadge
4. **AppBadge** - Create base with code + rarity variants
5. Update 18 files with import changes

---

## Phase 5: Text Wrapper Audit (Priority: Low)

### Architectural Principle

**MUI controls should ONLY be imported in atoms.** This provides:
- Single place to apply consistent styling
- Centralized ARIA/accessibility modifications
- Consistent behavior across the application

### Current Status: KEEP THESE COMPONENTS

| Component | Status | Reason |
|-----------|--------|--------|
| molecules/text/Heading.tsx | ✅ Keep | Wraps atoms/Typography, correct pattern |
| molecules/text/BodyText.tsx | ✅ Keep | Wraps atoms/Typography, correct pattern |

These are **not violations** - they correctly import Typography from atoms layer.

### Optional Enhancement

Consider moving to `atoms/text/` since they're thin wrappers that enforce variant defaults:
- Heading → could be `atoms/text/Heading.tsx` (enforces h1 variant)
- BodyText → could be `atoms/text/BodyText.tsx` (enforces body1 variant)

**Decision:** Keep as-is in molecules - they serve as semantic wrappers that pages can use without knowing MUI details.

---

## Phase 6: Loading Component Consolidation (Priority: Medium)

### Analysis

| Component | Location | Features |
|-----------|----------|----------|
| LoadingContainer | atoms/shared/ → molecules/feedback/ | size, message, centerVertically |
| LoadingIndicator | molecules/feedback/ | size, message, centered, withContainer |

### Resolution

After moving LoadingContainer to molecules/feedback (Phase 1):

1. **Evaluate overlap** - Both have nearly identical functionality
2. **Option A:** Keep LoadingIndicator as primary, deprecate LoadingContainer
3. **Option B:** Merge into single component with consistent API

**Recommendation:** Option A - Update LoadingContainer usages to LoadingIndicator

---

## Execution Checklist

### Phase 1: Atomic Corrections
- [ ] Create `molecules/feedback/` directory
- [ ] Create `molecules/feedback/index.ts`
- [ ] Move ErrorAlert.tsx to molecules/feedback/
- [ ] Move LoadingContainer.tsx to molecules/feedback/
- [ ] Update 3 files importing ErrorAlert/LoadingContainer
- [ ] Move DarkBadge.tsx to molecules/ui/
- [ ] Update 3 files importing DarkBadge
- [ ] Move BadgePill.tsx to molecules/Cards/
- [ ] Update RarityCollectorBadge import
- [ ] Move CollectionToast.tsx to molecules/feedback/
- [ ] Update NotificationToastStack import
- [ ] Update all barrel exports
- [ ] Run `npm run build` to verify

### Phase 2: FilterDrawer
- [ ] Analyze all 7 FilterDrawer usages
- [ ] Rename molecules/shared/FilterDrawer.tsx to GenericDrawer.tsx
- [ ] Update imports to use organisms or GenericDrawer
- [ ] Run `npm run build` to verify

### Phase 3: SearchInput
- [ ] Create unified SearchInput component
- [ ] Test with both debounced and expandable modes
- [ ] Update all SearchInput usages
- [ ] Update all DebouncedSearchInput usages
- [ ] Remove old component files
- [ ] Run `npm run build` to verify

### Phase 4: Badge System
- [ ] Create `molecules/shared/badges/` directory
- [ ] Create DeltaBadge (merge 2 LastDeltaBadge)
- [ ] Create DateBadge (merge 2 date badges)
- [ ] Create StatusBadge (merge 3 status badges)
- [ ] Create AppBadge with remaining variants
- [ ] Update all 18 files with badge imports
- [ ] Remove old badge files
- [ ] Run `npm run build` to verify

### Phase 5: Text Wrapper Audit
- [ ] Verify Heading.tsx imports from atoms (not MUI directly) ✅
- [ ] Verify BodyText.tsx imports from atoms (not MUI directly) ✅
- [ ] No changes needed - these follow correct pattern

### Phase 6: Loading Consolidation
- [ ] Evaluate LoadingContainer vs LoadingIndicator overlap
- [ ] Deprecate/merge as decided
- [ ] Update any remaining usages
- [ ] Run `npm run build` to verify

---

## Verification

After each phase:
1. `npm run build` - Verify compilation
2. `npm run lint` - Check for issues
3. `npm run dev` - Manual testing of affected pages
4. Grep for old import paths to catch missed updates

### Key Pages to Test
- AllSetsPage (badges, heading)
- SetPage (FilterDrawer, badges)
- CardSearchPage (search input, heading)
- WishlistPage (collection toast, filters)

---

## Risk Mitigation

1. **Commit after each phase** - Easy rollback if issues
2. **Keep old files temporarily** - Can restore if needed
3. **Test builds frequently** - Catch import errors early
4. **Verify MUI imports** - Ensure all molecules import from atoms, not MUI directly

---

## Future Work (Out of Scope)

Components identified for future evaluation:
- `PriceDisplay` - Uses DarkBadge, has logic (likely molecule)
- `CardName` - Uses hooks + DarkBadge (likely molecule)
- `ArtistLink` - Uses hooks + DarkBadge (likely molecule)
- `GridContainer` atom - May be redundant with ResponsiveGrid molecule

---

## Related Documents

- [FRONTEND_ARCHITECTURE_REVIEW.md](./FRONTEND_ARCHITECTURE_REVIEW.md) - Original analysis findings
- [CLAUDE.md](./CLAUDE.md) - Frontend architecture guidelines

---

## REFACTORING REVIEW FEEDBACK

**Reviewed by:** Architecture Review Agent, Frontend Developer Agent, Refactoring Specialist Agent
**Date:** 2026-01-14

### Critical Issues - Must Fix Before Execution

#### Issue 1: DarkBadge Move Creates Layer Violation

**Problem:** Phase 1.3 moves DarkBadge from `atoms/shared/` to `molecules/ui/`, but three **atoms** import DarkBadge:
- `atoms/shared/PriceDisplay.tsx`
- `atoms/Cards/CardName.tsx`
- `atoms/Cards/ArtistLink.tsx`

This creates atoms importing from molecules - an architectural violation.

**Resolution Options:**
1. Move PriceDisplay, CardName, ArtistLink to molecules BEFORE Phase 1.3
2. Keep DarkBadge in atoms and rename to `GlassBadge` (recommended)
3. Create `atoms/badges/` folder for DarkBadge

**Recommendation:** Option 2. DarkBadge is a styled primitive, not a composed molecule.

#### Issue 2: molecules/shared/FilterDrawer.tsx is Dead Code

**Finding:** All 6 current FilterDrawer imports use `organisms/filters/FilterDrawer`. The molecules version and `FilterDrawerTrigger` have zero usages.

**Resolution:** Delete the file instead of renaming to `GenericDrawer`.

**Updated Phase 2:**
```markdown
### Phase 2: FilterDrawer Cleanup
- [ ] Verify molecules/shared/FilterDrawer.tsx has zero imports
- [ ] Delete molecules/shared/FilterDrawer.tsx
- [ ] Remove from molecules/shared/index.ts barrel export
- [ ] Run `npm run build` to verify
```

#### Issue 3: Missing Type Export Handling

**Problem:** CollectionToast exports `ToastMessage` type. Plan doesn't address type-only imports.

**Add to Phase 1.5:**
```typescript
// molecules/feedback/index.ts
export { CollectionToast, type ToastMessage } from './CollectionToast';
```

#### Issue 4: Sealed LastDeltaBadge MUI Import Violation

**Must fix before Phase 4 badge consolidation:**
```typescript
// WRONG (current)
import { Chip } from '@mui/material';

// CORRECT (fix to)
import Chip from '../Chip';
```

### Phase Order Corrections

**Recommended Execution Order:**

1. **Phase 0 (NEW)**: Pre-migration fixes
   - Fix Sealed LastDeltaBadge MUI import
   - Delete dead code (molecules/shared/FilterDrawer.tsx)
   - Audit all atoms for MUI import violations

2. **Phase 1a**: Move ErrorAlert, LoadingContainer to molecules/feedback (safe)

3. **Phase 1b**: Move CollectionToast to molecules/feedback (safe)

4. **Phase 1c (CONDITIONAL)**: If keeping DarkBadge move:
   - Move PriceDisplay, CardName, ArtistLink to molecules first
   - Then move DarkBadge to molecules/ui/

   **OR (Recommended):** Keep DarkBadge in atoms, rename to GlassBadge

5. **Phase 1d**: Move BadgePill to molecules/Cards/ (safe)

6. **Phase 2**: SearchInput consolidation (sub-phases recommended)

7. **Phase 3**: Loading component consolidation

8. **Phase 4**: Badge system consolidation (sub-phases recommended)

### Badge Architecture Revision

**Problem:** Proposed variant-based AppBadge is a "god component" anti-pattern.

**Revised Architecture:**

**Create BaseBadge atom first:**
```typescript
// atoms/shared/BaseBadge.tsx
import Chip from '../Chip';

interface BaseBadgeProps {
  children: React.ReactNode;
  variant?: 'filled' | 'outlined' | 'glass';
  size?: 'small' | 'medium' | 'large';
  'aria-label'?: string;
  sx?: SxProps<Theme>;
}

export const BaseBadge: React.FC<BaseBadgeProps> = ({ ... }) => (
  <Chip sx={{ ...baseBadgeStyles, ...sx }} {...props} />
);
```

**Then create specialized molecules:**
```typescript
// molecules/badges/DeltaBadge.tsx
import { BaseBadge } from '../../atoms/shared/BaseBadge';

export const DeltaBadge: React.FC<DeltaBadgeProps> = ({ value, ...props }) => (
  <BaseBadge
    aria-label={`Change: ${value > 0 ? '+' : ''}${value}`}
    sx={{ color: value > 0 ? 'success.main' : 'error.main' }}
    {...props}
  >
    {value > 0 ? `+${value}` : value}
  </BaseBadge>
);
```

### SearchInput Consolidation Refinement

**Problem:** Mixing controlled/uncontrolled patterns with debouncing is confusing.

**Recommended Props Interface:**
```typescript
interface SearchInputProps {
  value: string;
  onChange: (value: string) => void;

  // Debounce config (separate from display value)
  debounce?: {
    delay: number;
    onCommit: (value: string) => void;
  };

  // Expandable config
  expandable?: boolean | {
    collapsed: number;
    expanded: number;
  };

  loading?: boolean;
  disabled?: boolean;
}
```

**Consider `useDeferredValue` (React 18)** instead of manual debounce.

### Missing Verification Steps

**Add to each phase:**
```bash
# TypeScript type checking
npx tsc --noEmit

# Circular dependency detection
npx madge --circular src/components/

# Verify old imports removed (example for Phase 1.2)
grep -r "from.*atoms/shared.*ErrorAlert" src/ && echo "ERROR: Old imports found!"
```

### Rollback Strategy

**Add explicit rollback per phase:**
```bash
# Phase 1 rollback
git checkout HEAD~1 -- src/components/atoms/shared/
git checkout HEAD~1 -- src/components/molecules/feedback/

# Use feature branch
git checkout -b refactor/component-consolidation
# Execute phases, PR for review before merge
```

### Phase 4 Should Be Split

18 files is too many for single-phase migration. Split into:
- **Phase 4a**: Create new badge components (no removals)
- **Phase 4b**: Migrate DeltaBadge usages (2 files)
- **Phase 4c**: Migrate DateBadge usages (5 files)
- **Phase 4d**: Migrate StatusBadge usages (5 files)
- **Phase 4e**: Migrate remaining badges (6 files)
- **Phase 4f**: Remove old badge files

### Accessibility Requirements

**All consolidated badges MUST include:**
```typescript
interface BadgeAccessibilityProps {
  'aria-label'?: string;      // Required for screen readers
  role?: string;              // 'img' for status indicators
  tabIndex?: number;          // For keyboard navigation
  onKeyDown?: (e: React.KeyboardEvent) => void;  // Enter/Space handling
}
```

**Example accessible badge:**
```typescript
<BaseBadge
  aria-label={`Rarity: ${rarity}`}
  role="img"
  tabIndex={clickable ? 0 : undefined}
  onKeyDown={clickable ? handleKeyDown : undefined}
>
  {symbol}
</BaseBadge>
```

### Testing Strategy

**Before refactoring (Phase 0):**
1. Create snapshot tests for components being moved
2. Document current import counts per component
3. Create verification scripts

**Example snapshot test:**
```typescript
describe('ErrorAlert snapshots', () => {
  it('matches snapshot - standard', () => {
    const { container } = render(<ErrorAlert message="Test" />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('matches snapshot - centered', () => {
    const { container } = render(<ErrorAlert message="Test" centered />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
```

**After each phase:**
- Run snapshot tests
- If snapshot differs → investigate before proceeding

### Updated Risk Assessment

| Change | Original Risk | Updated Risk | Mitigation |
|--------|---------------|--------------|------------|
| ErrorAlert move | Low | Low | Snapshot tests |
| DarkBadge move | Low | **HIGH** | Keep in atoms OR move dependents first |
| FilterDrawer rename | Low | **NONE** | Delete dead code instead |
| SearchInput merge | Medium | **HIGH** | Split into sub-phases, add feature flag |
| Badge consolidation | Medium | Medium | Split into sub-phases, create BaseBadge first |

### Summary of Required Changes

**Before execution:**
- [ ] Fix Sealed LastDeltaBadge MUI import violation
- [ ] Verify and delete molecules/shared/FilterDrawer.tsx
- [ ] Decide on DarkBadge: keep in atoms OR move dependents first
- [ ] Add Phase 0 for pre-migration fixes
- [ ] Split Phase 4 into sub-phases
- [ ] Add snapshot tests for key components
- [ ] Add TypeScript/circular dependency checks to verification
- [ ] Create explicit rollback commands per phase
