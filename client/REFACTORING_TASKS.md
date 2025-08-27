# Frontend Refactoring Task List

## Overview
This document contains a detailed task list for refactoring the React frontend. Each task should be completed and tested individually before moving to the next one.

## Testing Checklist (Run After Each Task)
- [ ] `npm run build` - Ensure no TypeScript errors
- [ ] `npm run dev` - Start dev server and verify functionality
- [ ] Test affected pages manually
- [ ] Verify no visual regressions
- [ ] Check browser console for errors

---

## Phase 1: Critical Component Extraction (High Priority)

### Task 1: Create DebouncedSearchInput Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/shared/DebouncedSearchInput.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create the new atom component with debounce logic
2. Replace search input in AllSetsPage (lines ~191-210)
3. Test AllSetsPage search functionality
4. Replace search input in SetPage (lines ~468-490)
5. Test SetPage search functionality
6. Remove duplicate debounce logic from both pages

**Testing focus:** Search typing, debounce delay, clear button

---

### Task 2: Create ResultsSummary Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/shared/ResultsSummary.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create ResultsSummary component
2. Replace "Showing X of Y" text in AllSetsPage (lines 309-313)
3. Test AllSetsPage results display
4. Replace similar text in SetPage (lines 680-688)
5. Test SetPage results display

**Testing focus:** Correct count display, pluralization

---

### Task 3: Create EmptyState Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/shared/EmptyState.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`
- Updated: `src/pages/CardDemoPage.tsx`
- Updated: `src/pages/SetDemoPage.tsx`

**Steps:**
1. Create EmptyState component for "No results found" scenarios
2. Replace empty state in AllSetsPage
3. Test with filters that yield no results
4. Replace in SetPage
5. Test SetPage empty states
6. Update demo pages if they have empty states

**Testing focus:** Empty state displays correctly when no data

---

### Task 4: Create SortDropdown Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/shared/SortDropdown.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create generic SortDropdown component
2. Replace sort dropdown in AllSetsPage
3. Test all sort options in AllSetsPage
4. Replace sort dropdown in SetPage
5. Test all sort options in SetPage

**Testing focus:** Sort functionality, selected state display

---

### Task 5: Create MultiSelectDropdown Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/shared/MultiSelectDropdown.tsx`
- Updated: `src/pages/AllSetsPage.tsx` (set types dropdown)
- Updated: `src/pages/SetPage.tsx` (rarity dropdown)

**Steps:**
1. Create MultiSelectDropdown with chip display and "Clear All"
2. Replace set types dropdown in AllSetsPage (lines 247-284)
3. Test set type filtering
4. Replace rarity dropdown in SetPage (lines 521-552)
5. Test rarity filtering
6. Replace artist dropdown if similar pattern exists

**Testing focus:** Multi-selection, chip removal, clear all functionality

---

## Phase 2: State Management & Hooks (Medium Priority)

### Task 6: Create useUrlState Hook [COMPLETED]
**Files to modify:**
- Created: `src/hooks/useUrlState.ts`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create useUrlState hook for URL parameter syncing
2. Replace URL state logic in AllSetsPage (lines 70-88)
3. Test URL updates when filters change
4. Replace URL state logic in SetPage (lines 217-237)
5. Test SetPage URL synchronization
6. Verify browser back/forward works correctly

**Testing focus:** URL updates, browser navigation, initial state from URL

---

### Task 7: Create useFilterState Hook [COMPLETED]
**Files to modify:**
- Created: `src/hooks/useFilterState.ts`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create useFilterState hook to manage filter state
2. Refactor AllSetsPage to use the hook
3. Test all filters work correctly
4. Refactor SetPage to use the hook
5. Test SetPage filters

**Testing focus:** Filter state updates, reset functionality

---

### Task 8: Create QueryStateContainer Molecule [COMPLETED]
**Files to modify:**
- Create: `src/components/molecules/shared/QueryStateContainer.tsx`
- Update: All page components with loading/error states

**Steps:**
1. Create QueryStateContainer for loading/error/data states
2. Replace pattern in AllSetsPage (lines 154-183)
3. Test loading and error states
4. Replace in SetPage (lines 394-441)
5. Test SetPage states
6. Replace in CardDemoPage (lines 121-134)
7. Replace in SetDemoPage (lines 81-94)

**Testing focus:** Loading spinner, error display, data rendering

---

## Phase 3: Filter Panel Extraction (Complex)

### Task 9: Create FilterPanel Molecule [COMPLETED]
**Files to modify:**
- Created: `src/components/molecules/shared/FilterPanel.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/pages/SetPage.tsx`

**Steps:**
1. Create FilterPanel using previously created atoms
2. Extract filter UI from AllSetsPage (lines 191-306)
3. Test all AllSetsPage filters work
4. Extract filter UI from SetPage (lines 468-678)
5. Test all SetPage filters work
6. Remove duplicate filter code

**Testing focus:** All filter combinations, responsive layout

---

## Phase 4: Layout Components (Medium Priority)

### Task 10: Create ResponsiveGrid Atom [COMPLETED]
**Files to modify:**
- Created: `src/components/atoms/layouts/ResponsiveGrid.tsx`
- Updated: `src/pages/AllSetsPage.tsx`
- Updated: `src/components/organisms/CardGroup.tsx`

**Steps:**
1. Create ResponsiveGrid component
2. Replace grid styles in SetPage (lines 52-58)
3. Test responsive behavior
4. Replace in CardDemoPage (lines 207-214)
5. Replace in AllSetsPage (lines 316-321)

**Testing focus:** Responsive breakpoints, grid spacing

---

### Task 11: Create CardGrid and SetGrid Specialized Grids [SKIPPED]
**Reason for skipping:**
- ResponsiveGrid and ResponsiveGridAutoFit already handle both use cases effectively
- Sets use `ResponsiveGrid` with appropriate sizing
- Cards use `ResponsiveGridAutoFit` with fixed widths
- Additional wrapper components would add unnecessary abstraction without benefit

---

## Phase 5: Additional UI Components (Lower Priority)

### Task 12: Create BackToTopFab Molecule [COMPLETED]
**Files to modify:**
- Created: `src/components/molecules/shared/BackToTopFab.tsx`
- Updated: `src/pages/SetPage.tsx`
- Updated: `src/pages/AllSetsPage.tsx`

**Steps:**
1. Extract BackToTopFab from SetPage
2. Make it reusable
3. Test scroll behavior
4. Add to other long pages if needed

**Testing focus:** Scroll threshold, smooth scrolling

### Task 13: [REMOVED]
---

## Phase 6: High Priority Improvements

### Task 14: Create Shared Type Definitions [COMPLETED]
**Priority: HIGH**
**Files to modify:**
- Created: `src/types/filters.ts`
- Created: `src/types/components.ts`  
- Created: `src/types/ui.ts`
- Updated: FilterPanel, SortDropdown, MultiSelectDropdown, DebouncedSearchInput
- Updated: useFilterState hook

**Steps:**
1. ~~Extract common filter interfaces~~ [COMPLETED]
2. ~~Extract common component prop interfaces~~ [COMPLETED]
3. Extend existing type files rather than replace
4. Update components to use shared types
5. Ensure TypeScript compilation passes

**Testing focus:** Type safety, IntelliSense improvements, no runtime errors

---

### Task 15: Add React.memo to Filter Components [COMPLETED]
**Priority: HIGH**
**Files modified:**
- `src/components/atoms/shared/MultiSelectDropdown.tsx` ✓
- `src/components/atoms/shared/SortDropdown.tsx` ✓
- `src/components/atoms/shared/DebouncedSearchInput.tsx` ✓
- `src/components/atoms/shared/EmptyState.tsx` ✓
- `src/components/atoms/shared/ResultsSummary.tsx` ✓
- `src/components/molecules/shared/FilterPanel.tsx` ✓

**Steps:**
1. Add React.memo to components listed above
2. Create proper comparison functions where needed
3. Test with React DevTools Profiler
4. Verify no functionality breaks

**Testing focus:** Re-render count reduction, filter interaction performance

---

### Task 16: Create Date Formatting Utilities [COMPLETED]
**Priority: HIGH**
**Files modified:**
- Created: `src/utils/dateFormatters.ts`
- Updated: `src/components/molecules/Cards/CardOverlay.tsx`
- Updated: `src/components/molecules/Cards/RulingsDisplay.tsx`
- Updated: `src/components/molecules/Cards/CardMetadata.tsx`
- Updated: `src/components/atoms/shared/ReleaseDateBadge.tsx`

**Steps completed:**
1. ✓ Created formatReleaseDate and formatRulingDate functions
2. ✓ Replaced duplicate date formatting in CardOverlay
3. ✓ Replaced in RulingsDisplay
4. ✓ Replaced in CardMetadata  
5. ✓ Updated ReleaseDateBadge to use utility

**Testing focus:** Consistent date formatting across all components

---

### Task 17: Create Theme Extensions [COMPLETED]
**Priority: HIGH**
**Files modified:**
- Created: `src/theme/index.ts`
- Updated: `src/main.tsx`
- Updated: `src/components/organisms/CardDetailsModal.tsx`
- Updated: `src/components/organisms/Header.tsx`
- Updated: `src/components/organisms/Footer.tsx`
- Updated: `src/components/molecules/Cards/CardOverlay.tsx`
- Updated: `src/components/organisms/MtgCard.tsx`
- Updated: `src/components/organisms/MtgSetCard.tsx`

**Steps completed:**
1. ✓ Created comprehensive theme with MTG-specific colors (rarity, legality)
2. ✓ Added custom spacing, dimensions, transitions, gradients, and shadows
3. ✓ Defined tokens for card dimensions, gradients, and effects
4. ✓ Updated components to use theme tokens
5. ✓ Removed hardcoded colors and replaced with theme references

**Testing focus:** Design consistency, theme application

---

## Phase 7: Medium Priority Enhancements

### Task 18: Enhance Card Selection Accessibility [COMPLETED]
**Priority: MEDIUM**
**Files modified:**
- `src/components/organisms/MtgCard.tsx`

**Steps completed:**
1. ✓ Added keyboard navigation support:
   - Enter/Space: Select card
   - Arrow keys: Navigate between cards in grid
   - Escape: Deselect card
2. ✓ Added comprehensive ARIA attributes:
   - role="button" for card interaction
   - aria-label with card details
   - aria-selected for selection state
   - aria-describedby for additional details
3. ✓ Used useCallback for all event handlers
4. ✓ Added focus styles for keyboard navigation visibility
5. ✓ Added hidden screen reader description element

**Testing focus:** Keyboard navigation, accessibility compliance

---

### Task 19: Extract Common Styles (Minimal) [COMPLETED]
**Priority: MEDIUM**
**Files created:**
- Created: `src/styles/cardStyles.ts` - Common card patterns and utilities
- Created: `src/styles/layoutStyles.ts` - Flexbox utilities and layout patterns

**Files updated:**
- Updated: `src/components/organisms/MtgCard.tsx` - Uses srOnly utility
- Updated: `src/components/organisms/CardDetailsModal.tsx` - Uses flexbox utilities  
- Updated: `src/components/templates/Layout.tsx` - Uses pageContainer and mainContent

**What was accomplished:**
1. ✓ Created reusable card style patterns (overlay positions, containers, srOnly)
2. ✓ Created flexbox utilities (flexRow, flexCol, flexCenter, flexBetween, etc.)
3. ✓ Created gap utilities and responsive helpers
4. ✓ Updated key components to demonstrate usage
5. ✓ Fixed TypeScript issues with combineStyles utility

**Note:** Most gradient and color patterns were already extracted in Task 17 (theme), so this task focused on positional and layout utilities only.

**Testing focus:** Visual consistency, reduced style duplication

---

### Task 20: Add Loading States to Filter Components [COMPLETED]
**Priority: MEDIUM**
**Files modified:**
- Updated: `src/components/atoms/shared/MultiSelectDropdown.tsx`
- Updated: `src/components/atoms/shared/SortDropdown.tsx`
- Updated: `src/components/atoms/shared/DebouncedSearchInput.tsx`
- Updated: `src/components/molecules/shared/FilterPanel.tsx`
- Updated: `src/types/filters.ts`

**Steps completed:**
1. ✓ Added loading and disabled props to all filter components
2. ✓ Created skeleton states using MUI Skeleton component
3. ✓ Updated FilterPanel to pass through loading states
4. ✓ Updated type definitions to include loading/disabled properties
5. ✓ Tested build and fixed compilation issues

**What was added:**
- Skeleton loaders appear when loading=true
- Components are disabled when disabled=true
- Search input shows CircularProgress when disabled
- All loading states maintain proper dimensions to prevent layout shift

**Testing focus:** Smooth loading transitions, no layout shift

---

## Phase 8: Lower Priority Additions

### Task 21: Implement Error Boundaries
**Priority: LOW**
**Files to modify:**
- Update: `src/components/ErrorBoundary.tsx`
- Add error boundaries to key component trees

**Steps:**
1. Enhance existing ErrorBoundary component
2. Wrap major sections (filters, card grids, etc.)
3. Add error recovery mechanisms
4. Test error handling scenarios

**Testing focus:** Graceful error handling, error recovery

---

## Tasks to Skip

### Task 18: Create Navigation Hook [SKIPPED]
**Reason for skipping:**
- App uses simple navigation patterns (direct URL changes)
- No React Router or complex routing state
- Would add unnecessary abstraction
- Current approach is intentionally simple

---

## Completion Criteria

### After All Tasks:
- [ ] Run full build: `npm run build`
- [ ] Run linter: `npm run lint`
- [ ] Test all pages thoroughly
- [ ] Check for TypeScript errors
- [ ] Verify no functionality regression
- [ ] Update REFACTORING_LEARNINGS.md with any new insights
- [ ] Consider creating component documentation

## Notes

- Each task should be completed in a separate commit
- Run tests after each task
- If a task breaks functionality, revert and investigate
- Document any issues in REFACTORING_LEARNINGS.md
- Consider creating Storybook stories for new atoms/molecules

## Estimated Timeline

- Phase 1: 2-3 days
- Phase 2: 1-2 days
- Phase 3: 1-2 days
- Phase 4: 1 day
- Phase 5: 1 day
- Phase 6-9: 2-3 days

Total: ~2 weeks for complete refactoring

## Risk Mitigation

- Create feature branch for refactoring
- Test each change individually
- Keep changes small and focused
- Maintain backwards compatibility
- Document breaking changes if any