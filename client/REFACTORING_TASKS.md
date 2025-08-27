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

### Task 11: Create CardGrid and SetGrid Specialized Grids
**Files to modify:**
- Create: `src/components/atoms/layouts/CardGrid.tsx`
- Create: `src/components/atoms/layouts/SetGrid.tsx`
- Update: Relevant pages

**Steps:**
1. Create CardGrid extending ResponsiveGrid
2. Create SetGrid extending ResponsiveGrid
3. Replace card grids across pages
4. Replace set grids across pages

**Testing focus:** Specialized grid behaviors

---

## Phase 5: Additional UI Components (Lower Priority)

### Task 12: Create BackToTopFab Molecule
**Files to modify:**
- Create: `src/components/molecules/shared/BackToTopFab.tsx`
- Update: `src/pages/SetPage.tsx`

**Steps:**
1. Extract BackToTopFab from SetPage
2. Make it reusable
3. Test scroll behavior
4. Add to other long pages if needed

**Testing focus:** Scroll threshold, smooth scrolling

### Task 13: Removed
---

## Phase 6: Type Definitions & Interfaces

### Task 14: Create Shared Type Definitions
**Files to modify:**
- Create/Update: `src/types/filters.ts`
- Create/Update: `src/types/components.ts`
- Update: All components using these types

**Steps:**
1. Extract common filter interfaces
2. Extract common component prop interfaces
3. Update components to use shared types
4. Ensure TypeScript compilation passes

**Testing focus:** Type safety, no runtime errors

---

## Phase 7: Performance Optimizations

### Task 15: Add React.memo to Filter Components
**Files to modify:**
- All newly created filter components
- Existing filter-related components

**Steps:**
1. Identify components that re-render unnecessarily
2. Add React.memo with proper comparison functions
3. Test performance with React DevTools Profiler
4. Verify no functionality breaks

**Testing focus:** Re-render count, performance metrics

---

### Task 16: Optimize Card Selection Logic
**Files to modify:**
- `src/components/organisms/MtgCard.tsx`
- Related card components

**Steps:**
1. Review current DOM-based selection logic
2. Optimize if possible while maintaining performance
3. Test selection behavior

**Testing focus:** Selection performance, keyboard navigation

---

## Phase 8: Utilities & Helpers

### Task 17: Create Date Formatting Utilities
**Files to modify:**
- Create: `src/utils/dateFormatters.ts`
- Update: `src/components/molecules/Cards/CardOverlay.tsx`
- Update: Any other components formatting dates

**Steps:**
1. Extract date formatting from CardOverlay (lines 53-57)
2. Create reusable date utilities
3. Replace inline date formatting
4. Test date displays

**Testing focus:** Date format consistency

---

### Task 18: Create Navigation Hook
**Files to modify:**
- Create: `src/hooks/useNavigation.ts`
- Update: Components with navigation handlers

**Steps:**
1. Extract common navigation patterns
2. Create useNavigation hook
3. Update components to use the hook
4. Test navigation functionality

**Testing focus:** Navigation to sets, cards, artists

---

## Phase 9: Style Refactoring

### Task 19: Extract Common Styles
**Files to modify:**
- Create: `src/styles/filterStyles.ts`
- Create: `src/styles/cardStyles.ts`
- Update: Components with inline styles

**Steps:**
1. Identify repeated style patterns
2. Create style utilities or constants
3. Replace inline styles
4. Verify visual consistency

**Testing focus:** Visual regression testing

---

### Task 20: Create Theme Extensions
**Files to modify:**
- Update: `src/main.tsx` (theme configuration)
- Create: `src/theme/index.ts`
- Update: Components using custom styles

**Steps:**
1. Extend MUI theme with custom tokens
2. Create consistent spacing, colors, etc.
3. Update components to use theme
4. Test theming consistency

**Testing focus:** Theme application, dark mode if applicable

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