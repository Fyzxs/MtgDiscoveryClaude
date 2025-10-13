# Atomic Architecture Audit - MTG Discovery Frontend

**Date:** 2025-10-13
**Total Component Files:** 109
**Total Lines of Code:** ~15,000
**Codebase Status:** Mature, in active development

---

## Executive Summary

The MTG Discovery frontend follows atomic design principles with a generally well-organized component hierarchy. However, there are several opportunities to improve alignment with atomic design patterns, reduce complexity, and enhance composability.

### Overall Health: 7.5/10

**Strengths:**
- Clear directory structure matching atomic design layers
- Consistent Material-UI integration with custom theme
- Good separation between domain-specific components (Cards, Sets)
- Strong accessibility patterns throughout

**Key Areas for Improvement:**
1. **Tailwind CSS Leakage** - 20+ components still use Tailwind classes despite MUI-first strategy
2. **Complex Atoms** - Several atoms exceed atomic purity (e.g., DebouncedSearchInput, MultiSelectDropdown)
3. **Missing Molecule Abstractions** - Repeated patterns that should be extracted
4. **Organism Bloat** - Some organisms could be split into smaller, more focused components
5. **Template Underutilization** - Template layer is sparse, page components contain too much layout logic

---

## Priority Findings

### P0 - Critical Issues (Address Immediately)

#### 1. Tailwind CSS Contamination
**Issue:** Despite project mandate to use MUI-only, 20+ components still use Tailwind utility classes.

**Files Affected:**
- `/components/molecules/Cards/ManaCost.tsx:26` - Uses `flex items-center gap-1`
- `/components/atoms/Cards/ArtistLink.tsx` - Tailwind classes present
- `/components/atoms/Cards/CardName.tsx` - Tailwind classes present
- `/components/atoms/Cards/CollectorNumber.tsx` - Tailwind classes present
- `/components/molecules/Cards/CardMetadata.tsx` - Tailwind classes present
- Multiple others found via grep

**Impact:** Creates inconsistency, larger bundle size, and violates architectural decisions.

**Solution:**
```typescript
// BEFORE (ManaCost.tsx:26)
<div className={`flex items-center gap-1 ${className}`}>
  {symbols.map((symbol, index) => (
    <ManaSymbol key={index} symbol={symbol} size={size} />
  ))}
</div>

// AFTER
<Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }} className={className}>
  {symbols.map((symbol, index) => (
    <ManaSymbol key={index} symbol={symbol} size={size} />
  ))}
</Box>
```

**Action Items:**
1. Create utility script to find all Tailwind usage: `grep -r "className.*\(flex\|grid\|gap\|items\|justify\)" src/components`
2. Systematically replace with MUI Box + sx props
3. Remove Tailwind from project dependencies
4. Add ESLint rule to prevent future Tailwind usage

---

#### 2. Complex Atoms Violating Atomic Purity

**Issue:** Several "atoms" are actually molecules in disguise - they contain complex business logic, multiple state hooks, and extensive composition.

##### 2a. DebouncedSearchInput (380 lines!)
**File:** `/components/atoms/shared/DebouncedSearchInput.tsx`

**Why It's Not an Atom:**
- Contains 4 useState hooks
- Multiple useEffect hooks
- Complex debounce logic
- DOM manipulation (`querySelector`, focus management)
- Event handling with refs
- Global event listeners

**Proper Classification:** Molecule or specialized input organism

**Refactoring Strategy:**
```typescript
// atoms/shared/AppSearchInput.tsx (pure atom)
export const AppSearchInput: React.FC<SearchInputProps> = ({
  value,
  onChange,
  placeholder,
  disabled,
  loading,
  hasText,
  onClear
}) => (
  <TextField
    value={value}
    onChange={onChange}
    placeholder={placeholder}
    disabled={disabled}
    InputProps={{
      startAdornment: (
        <InputAdornment position="start">
          {disabled ? <CircularProgress size={20} /> : <SearchIcon />}
        </InputAdornment>
      ),
      endAdornment: hasText ? (
        <InputAdornment position="end">
          <IconButton size="small" onClick={onClear}>
            <ClearIcon fontSize="small" />
          </IconButton>
        </InputAdornment>
      ) : null
    }}
  />
);

// molecules/shared/DebouncedSearchInput.tsx (handles logic)
export const DebouncedSearchInput: React.FC = () => {
  const [localValue, setLocalValue] = useState('');
  const [hasText, setHasText] = useState(false);
  // ... debounce logic, keyboard navigation logic

  return (
    <AppSearchInput
      value={localValue}
      onChange={handleChange}
      hasText={hasText}
      onClear={handleClear}
      // ... other props
    />
  );
};
```

##### 2b. MultiSelectDropdown (186 lines)
**File:** `/components/atoms/shared/MultiSelectDropdown.tsx`

**Why It's Not an Atom:**
- Complex option normalization logic
- Multiple sub-components (FormControl, Select, MenuItem, Chip)
- Custom rendering logic
- Event handling with state transformations

**Should Be:** Molecule that composes simpler atoms

##### 2c. CardBadges (342 lines!)
**File:** `/components/atoms/Cards/CardBadges.tsx`

**Why It's Not an Atom:**
- Massive component with extensive conditional logic
- Multiple formatting functions (100+ lines)
- Complex badge filtering rules
- Composition of multiple Chip components
- Business logic for foil types, serialization, etc.

**Refactoring Strategy:**
```typescript
// atoms/Cards/BadgeChip.tsx (pure atom)
export const BadgeChip: React.FC<BadgeChipProps> = ({
  label,
  type, // 'foil' | 'promo' | 'frame' | etc.
  inline
}) => {
  const backgroundColor = getBadgeColor(type);
  return (
    <Chip
      label={label}
      size="small"
      sx={{
        height: 20,
        fontSize: '0.625rem',
        backgroundColor,
        // ... styling
      }}
    />
  );
};

// molecules/Cards/CardBadgeGroup.tsx (handles logic)
export const CardBadgeGroup: React.FC<CardBadgeGroupProps> = ({
  card,
  inline
}) => {
  const badges = useCardBadges(card); // Custom hook with all logic

  return (
    <Box sx={{ display: 'flex', gap: 0.5 }}>
      {badges.map(badge => (
        <BadgeChip key={badge.id} {...badge} />
      ))}
    </Box>
  );
};
```

**Impact:** High - These "atoms" are the foundation of the design system but don't follow atomic principles.

---

### P1 - Important Issues (Address Soon)

#### 3. Missing Atom Wrappers for Common MUI Components

**Issue:** Many components use MUI components directly instead of using or creating App* wrappers. This creates inconsistency and makes it harder to enforce design system standards.

**Existing App* Wrappers:**
- `AppButton` ✓
- `AppCard` ✓
- Missing wrappers for:
  - `TextField` (should have `AppTextField`)
  - `Select` (should have `AppSelect`)
  - `Chip` (should have `AppChip` with rarity/type variants)
  - `Box` with common patterns (should have layout atoms like `FlexBox`, `GridBox`)

**Example - Missing AppChip:**
```typescript
// atoms/shared/AppChip.tsx
export type ChipVariant = 'default' | 'rarity' | 'promo' | 'finish';

interface AppChipProps extends ChipProps {
  variant?: ChipVariant;
  rarity?: string;
}

export const AppChip: React.FC<AppChipProps> = ({
  variant = 'default',
  rarity,
  sx,
  ...props
}) => {
  const variantStyles = useChipVariantStyles(variant, rarity);

  return (
    <Chip
      sx={{
        ...variantStyles,
        ...sx
      }}
      {...props}
    />
  );
};
```

**Benefits:**
- Centralized styling
- Type-safe variant system
- Easier to refactor MUI upgrades
- Better design system documentation

---

#### 4. Organism Complexity - Page Logic Bleeding

**Issue:** Some organisms contain page-level logic that should be in pages or templates.

##### 4a. CardGrid (158 lines)
**File:** `/components/organisms/CardGrid.tsx`

**Current Responsibilities:**
- Progressive rendering logic (useState, useEffect)
- Deferred value handling
- Skeleton loading states
- Grid navigation
- Card rendering

**Should Be Split:**
```typescript
// organisms/CardGrid.tsx (simplified)
// Just handles grid layout and navigation
export const CardGrid: React.FC<CardGridProps> = ({
  cards,
  groupId,
  context,
  // ...
}) => {
  const { handleKeyDown } = useGridNavigation({ groupId });

  return (
    <Box data-card-group={groupId}>
      <ResponsiveGridAutoFit {...gridProps}>
        {cards.map((card, index) => (
          <MtgCard key={card.id} card={card} index={index} />
        ))}
      </ResponsiveGridAutoFit>
    </Box>
  );
};

// templates/ProgressiveCardGrid.tsx (handles optimization)
export const ProgressiveCardGrid: React.FC = ({ cards, ...props }) => {
  const deferredCards = useDeferredValue(cards);
  const visibleCards = useProgressiveRendering(deferredCards);

  return <CardGrid cards={visibleCards} {...props} />;
};
```

##### 4b. MtgCard - Hook Overload
**File:** `/components/organisms/MtgCard.tsx`

**Current Pattern:**
```typescript
const MtgCardComponent: React.FC<MtgCardProps> = ({ card, ... }) => {
  const { ariaLabel } = useMtgCardMemo({ card });
  const { cardStyles } = useMtgCardStyles({ card });
  const { modalOpen, isSelected, ... } = useMtgCardInteractions({ cardRef });
  useMtgCardCollectionActions({ card, isSelected, cardRef });
  // ...
};
```

**Issue:** Using custom hooks to split "molecule" logic defeats the purpose. These should be actual molecule components.

**Better Pattern:**
```typescript
// molecules/Cards/MtgCardBase.tsx
export const MtgCardBase: React.FC = ({ card, children }) => {
  const { cardStyles } = useMtgCardStyles({ card });
  return (
    <MuiCard sx={cardStyles} {...accessibilityProps}>
      {children}
    </MuiCard>
  );
};

// molecules/Cards/MtgCardInteractions.tsx
export const MtgCardInteractions: React.FC = ({ card, children }) => {
  // All interaction logic here
  return (
    <Box onClick={handleClick} onKeyDown={handleKeyDown}>
      {children}
    </Box>
  );
};

// organisms/MtgCard.tsx (pure composition)
export const MtgCard: React.FC<MtgCardProps> = ({ card }) => (
  <MtgCardBase card={card}>
    <MtgCardInteractions card={card}>
      <CardImageDisplay card={card} />
      <CardBadgeGroup card={card} />
      <CardOverlay card={card} />
    </MtgCardInteractions>
  </MtgCardBase>
);
```

---

#### 5. Duplicate Pattern - Query State Handling

**Issue:** Multiple molecules/organisms handle GraphQL loading/error states inconsistently.

**Found In:**
- `QueryStateContainer` molecule (good abstraction)
- `LoadingContainer` atom (too simple to be useful)
- Inline loading logic in pages (`SetPage`, `CardSearchPage`, etc.)

**Current Inconsistency:**
```typescript
// SetPage.tsx - Inline approach
if (loading) return <CircularProgress />;
if (error) return <Alert severity="error">{error.message}</Alert>;

// Other pages use QueryStateContainer
<QueryStateContainer loading={loading} error={error}>
  {/* content */}
</QueryStateContainer>
```

**Solution:** Standardize on ONE pattern for all pages:

```typescript
// templates/QueryTemplate.tsx
export const QueryTemplate: React.FC<QueryTemplateProps> = ({
  queries,
  header,
  filters,
  content,
  footer
}) => {
  const { isLoading, hasError, firstError } = useQueryStates(queries);

  return (
    <PageLayout>
      {header}
      {isLoading && <LoadingState />}
      {hasError && <ErrorState error={firstError} />}
      {!isLoading && !hasError && (
        <>
          {filters}
          {content}
          {footer}
        </>
      )}
    </PageLayout>
  );
};
```

---

### P2 - Nice-to-Have Improvements

#### 6. Template Layer Underutilization

**Current State:**
- Only 2 templates: `Layout.tsx` and `SetPageTemplate.tsx`
- Most layout logic exists in pages themselves
- Pages are 400-670 lines (too large)

**Missing Templates:**
1. **CardGridPageTemplate** - Standard layout for any page showing card grids
2. **SearchPageTemplate** - Standard layout for search pages
3. **DetailsPageTemplate** - Standard layout for detail views
4. **FilterableContentTemplate** - Standard layout for filterable content

**Example:**
```typescript
// templates/FilterableContentTemplate.tsx
interface FilterableContentTemplateProps {
  header: ReactNode;
  filters: ReactNode;
  content: ReactNode;
  footer?: ReactNode;
  summary: {
    current: number;
    total: number;
  };
}

export const FilterableContentTemplate: React.FC<FilterableContentTemplateProps> = ({
  header,
  filters,
  content,
  footer,
  summary
}) => (
  <PageLayout>
    <Container maxWidth="xl">
      {header}

      <Box sx={{ display: 'flex', gap: 3, my: 3 }}>
        <Box sx={{ flexShrink: 0, width: 280 }}>
          {filters}
        </Box>

        <Box sx={{ flex: 1, minWidth: 0 }}>
          <ResultsSummary current={summary.current} total={summary.total} />
          {content}
        </Box>
      </Box>

      {footer}
    </Container>
  </PageLayout>
);

// pages/SetPage.tsx (simplified from 670 lines to ~200)
export const SetPage: React.FC = () => {
  const { setCode } = useParams();
  // ... data fetching
  // ... filter logic

  return (
    <FilterableContentTemplate
      header={<SetPageHeader set={setInfo} />}
      filters={<SetPageFilters {...filterProps} />}
      content={<SetPageCardDisplay {...cardProps} />}
      summary={{ current: filteredCount, total: totalCount }}
    />
  );
};
```

---

#### 7. Composition vs Configuration - Inconsistent Patterns

**Issue:** Some components use composition (children), others use configuration (props), without clear reasoning.

**Examples of Inconsistency:**

```typescript
// EmptyState - Configuration pattern
<EmptyState
  message="No results"
  icon={<SearchOffIcon />}
  action={{ label: 'Clear', onClick: handleClear }}
/>

// ExpandableSection - Composition pattern
<ExpandableSection title="Filters" expanded={expanded}>
  <FilterContent />
</ExpandableSection>

// QueryStateContainer - Composition pattern
<QueryStateContainer loading={loading}>
  <Content />
</QueryStateContainer>
```

**Recommendation:** Establish clear guidelines:
- **Use Configuration** when: Component has few, predefined slots
- **Use Composition** when: Content varies significantly or is complex
- **Use Hybrid** when: Need both flexibility and consistency

**Example - EmptyState should accept children for flexibility:**
```typescript
// More flexible
<EmptyState icon={<SearchOffIcon />}>
  <EmptyStateMessage>No results found</EmptyStateMessage>
  <EmptyStateDescription>
    Try adjusting your filters or search terms
  </EmptyStateDescription>
  <EmptyStateAction onClick={handleClear}>
    Clear filters
  </EmptyStateAction>
</EmptyState>
```

---

#### 8. Missing Layout Atoms

**Issue:** Repeated layout patterns that should be atoms.

**Common Patterns Found:**
```typescript
// Pattern 1: Flex between (found 30+ times)
<Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>

// Pattern 2: Centered column (found 20+ times)
<Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>

// Pattern 3: Grid with gap (found 15+ times)
<Box sx={{ display: 'grid', gap: 2, gridTemplateColumns: '...' }}>
```

**Solution - Create Layout Atoms:**
```typescript
// atoms/layouts/FlexBetween.tsx
export const FlexBetween: React.FC<BoxProps> = ({ children, sx, ...props }) => (
  <Box
    sx={{
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      ...sx
    }}
    {...props}
  >
    {children}
  </Box>
);

// atoms/layouts/CenteredColumn.tsx
export const CenteredColumn: React.FC<BoxProps> = ({ children, sx, ...props }) => (
  <Box
    sx={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      ...sx
    }}
    {...props}
  >
    {children}
  </Box>
);

// Usage becomes cleaner
<FlexBetween>
  <CardCount />
  <PriceDisplay />
</FlexBetween>
```

---

#### 9. Prop Drilling in Card Display Components

**Issue:** Card context and event handlers are passed through multiple layers.

**Current Flow:**
```
SetPage (defines handlers)
  ↓ passes onSetClick, onArtistClick, context
SetPageCardDisplay
  ↓ passes same props
CardGroup
  ↓ passes same props
CardGrid
  ↓ passes same props
MtgCard
  ↓ passes same props
CardOverlay
  ↓ finally uses handlers
```

**Solution - Context API:**
```typescript
// contexts/CardActionContext.tsx
interface CardActionContextValue {
  onCardClick?: (cardId: string) => void;
  onSetClick?: (setCode: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
}

export const CardActionContext = createContext<CardActionContextValue>({});

export const CardActionProvider: React.FC = ({ children, ...handlers }) => (
  <CardActionContext.Provider value={handlers}>
    {children}
  </CardActionContext.Provider>
);

export const useCardActions = () => useContext(CardActionContext);

// SetPage.tsx
<CardActionProvider
  onSetClick={handleSetClick}
  onArtistClick={handleArtistClick}
>
  <SetPageCardDisplay />
</CardActionProvider>

// CardOverlay.tsx (no more prop drilling)
const { onSetClick, onArtistClick } = useCardActions();
```

---

## Component Inventory Issues by Layer

### Atoms (43 files)

#### Well-Designed Atoms ✓
- `AppButton` - Simple MUI wrapper with loading state
- `AppCard` - Simple MUI wrapper with consistent styling
- `RarityBadge` - Single responsibility, pure presentation
- `SetIcon` - Simple image display
- `LoadingContainer` - Simple loading spinner wrapper

#### Atoms That Should Be Molecules ⚠
- `DebouncedSearchInput` (380 lines) - Contains complex state and logic
- `MultiSelectDropdown` (186 lines) - Complex selection logic
- `CardBadges` (342 lines) - Massive with extensive business logic
- `LanguageSwitcher` - Contains i18n logic and state
- `SortDropdown` - Contains dropdown logic and option filtering

#### Atoms Using Tailwind (Should Use MUI) ⚠
- `ArtistLink`
- `CardName`
- `CollectorNumber`
- `ManaSymbol`
- `RarityBadge`
- `SetLink`
- `ZoomIndicator`
- `DarkBadge`
- `ExternalLinkIcon`
- `PriceDisplay`

### Molecules (33 files)

#### Well-Designed Molecules ✓
- `CardLinks` - Composes ExternalLinkIcon atoms
- `ManaCost` - Composes ManaSymbol atoms (needs Tailwind removal)
- `QueryStateContainer` - Good abstraction for loading/error states
- `ExpandableSection` - Good composition pattern

#### Molecules That Should Be Organisms ⚠
- `CardCompact` (271 lines) - Contains too much logic, multiple hooks
- `CardImageDisplay` - Complex flip/zoom logic could be organism
- `MtgCardStyles` (120 lines) - Not really a "component", should be a hook or utility

#### Molecules That Should Be Atoms ⚠
- `BackToTopFab` - Simple button with scroll behavior (could be atom)
- `ModalContainer` - Just a wrapper, should be AppModal atom

#### Molecules Using Hooks (Consider Extracting) ⚠
- `MtgCardCollectionActions` - Just a hook exported as module
- `MtgCardInteractions` - Just a hook exported as module
- `MtgCardMemo` - Just utilities exported as module
- These violate the molecule concept - they're not components

### Organisms (23 files)

#### Well-Structured Organisms ✓
- `CardGrid` - Handles grid layout and navigation
- `Footer` - Application footer
- `Header` - Application header
- `SetPageHeader` - Focused on header concerns

#### Organisms with Too Much Logic ⚠
- `MtgCard` (129 lines) - Uses 4 custom hooks, could be simplified with composition
- `CardFilterPanel` - Contains filter logic that could be in page
- `SetPageFilters` - Contains filter state management

#### Organisms That Could Be Split ⚠
- `CardDetailsModal` - Could extract modal logic from card display
- `AllPrintingsDisplay` - Could be split into display + logic

### Templates (2 files)

#### Current Templates
- `Layout.tsx` - Basic app layout (good)
- `SetPageTemplate.tsx` - Set page structure (good)

#### Missing Templates ⚠
- **CardGridPageTemplate** - For any card grid page
- **SearchPageTemplate** - For search pages
- **FilterableContentTemplate** - For pages with filters
- **DetailsPageTemplate** - For detail pages

### Pages (7 files)

#### Pages Doing Too Much ⚠
- `SetPage.tsx` (670 lines!) - Too much logic, needs template
- `CardSearchPage.tsx` (likely large) - Needs template
- `AllSetsPage.tsx` - Could use template

#### Pages That Are Right-Sized ✓
- `SignInRedirectPage.tsx` - Simple redirect logic

---

## Naming Consistency Issues

### Inconsistent Naming Patterns

1. **Card Components:**
   - Some use `Card` prefix: `CardBadges`, `CardLinks`, `CardName`
   - Some use `Mtg` prefix: `MtgCard`, `MtgSetCard`
   - **Recommendation:** Use `Card` for atoms/molecules, `MtgCard` for complete card organisms

2. **Set Components:**
   - Inconsistent: `MtgSetCard` vs `SetIcon` vs `SetPageHeader`
   - **Recommendation:** `Set` prefix for atoms, `MtgSet` prefix for complete set organisms

3. **Display Components:**
   - `CardDisplay` vs `CardImageDisplay` vs `CardCompactResponsive`
   - **Recommendation:** Add `Display` suffix only for complete rendering components

---

## Material-UI Integration Issues

### Inconsistent sx Prop Usage

**Issue:** Mix of inline styles, sx props, and styled components.

**Examples:**
```typescript
// Good - sx prop with theme reference
<Box sx={{ bgcolor: 'grey.900', borderRadius: 2 }} />

// Bad - Inline style object
<Box style={{ backgroundColor: '#1a1a1a' }} />

// Inconsistent - Some use theme directly, others use palette
sx={{ color: theme.palette.primary.main }}
sx={{ color: 'primary.main' }}
```

**Recommendation:** Always use sx prop with string-based theme references.

### Missing Theme Extensions

**Current Theme Extensions:**
```typescript
theme.palette.rarity.*
theme.palette.legality.*
theme.mtg.spacing.*
theme.mtg.dimensions.*
theme.mtg.shadows.*
theme.mtg.gradients.*
```

**Missing Extensions:**
- `theme.mtg.layouts.*` - Common layout patterns
- `theme.mtg.animations.*` - Card animations
- `theme.mtg.breakpoints.*` - MTG-specific breakpoints

---

## Performance Considerations

### Components That Should Use React.memo

**Currently Not Memoized:**
- Most atoms (should be memoized for grid performance)
- Several molecules used in grids
- Template components

**Recommendation:**
```typescript
// All atoms should be memoized by default
export const RarityBadge = React.memo(RarityBadgeComponent);

// Molecules in grids should be memoized
export const CardCompact = React.memo(CardCompactComponent);

// Use custom comparison for complex props
export const MtgCard = React.memo(MtgCardComponent, (prev, next) => {
  return prev.card.id === next.card.id &&
         prev.index === next.index;
});
```

### Components with Performance Issues

1. **CardBadges** - Recalculates badge logic on every render
   - **Solution:** Extract logic to `useCardBadges` hook with memoization

2. **CardGrid** - Progressive rendering is good, but could be optimized
   - **Solution:** Virtual scrolling for very large grids (1000+ cards)

3. **SetPage** - 670 lines of logic runs on every render
   - **Solution:** Extract to custom hooks and memoize results

---

## Accessibility Improvements

### Good Accessibility Patterns ✓
- Consistent aria-label usage
- Keyboard navigation in grids
- Focus management
- Screen reader announcements

### Areas for Improvement ⚠

1. **Skip Links** - Only in shared atoms, not in all pages
2. **Landmark Regions** - Inconsistent use of semantic HTML
3. **Focus Trapping** - Modals need better focus management
4. **Announcement Regions** - Missing live regions for dynamic content

---

## Refactoring Recommendations

### Phase 1: Foundation (1-2 weeks)
1. **Remove Tailwind CSS** - Convert all Tailwind to MUI
2. **Split Complex Atoms** - Extract DebouncedSearchInput, CardBadges, MultiSelectDropdown
3. **Create Missing App* Wrappers** - AppTextField, AppSelect, AppChip
4. **Create Layout Atoms** - FlexBetween, CenteredColumn, GridContainer

### Phase 2: Molecules (2-3 weeks)
5. **Fix Hook-Based Molecules** - Convert to actual components
6. **Extract Repeated Patterns** - Card action handlers, query states
7. **Standardize Composition** - Establish composition vs configuration guidelines
8. **Create Missing Molecules** - Badge groups, filter groups

### Phase 3: Organisms (2 weeks)
9. **Simplify MtgCard** - Use composition instead of multiple hooks
10. **Split Large Organisms** - CardGrid, CardFilterPanel
11. **Standardize Data/Presentation** - Separate logic from display

### Phase 4: Templates (1-2 weeks)
12. **Create Missing Templates** - FilterableContentTemplate, CardGridPageTemplate
13. **Extract Page Logic** - Move layout to templates
14. **Standardize Page Structure** - All pages use templates

### Phase 5: Polish (1 week)
15. **Naming Consistency** - Rename components following guidelines
16. **Performance Optimization** - Add React.memo strategically
17. **Documentation** - Document atomic patterns and guidelines
18. **Testing** - Add component tests for atoms and molecules

---

## Code Examples - Before/After

### Example 1: Simplifying MtgCard

**Before (129 lines, 4 hooks):**
```typescript
const MtgCardComponent: React.FC<MtgCardProps> = ({ card, context }) => {
  const cardRef = useRef<HTMLDivElement>(null);
  const { ariaLabel } = useMtgCardMemo({ card });
  const { cardStyles } = useMtgCardStyles({ card });
  const { modalOpen, isSelected, handleCardClick } = useMtgCardInteractions({ cardRef });
  useMtgCardCollectionActions({ card, isSelected, cardRef });

  return (
    <MuiCard ref={cardRef} onClick={handleCardClick} sx={cardStyles}>
      <CardImageDisplay card={card} />
      <CardBadges {...card} />
      <ZoomIndicator onZoomClick={handleZoomClick} />
      <CardOverlay card={card} context={context} />
      <CardDetailsModal open={modalOpen} card={card} />
    </MuiCard>
  );
};
```

**After (50 lines, pure composition):**
```typescript
// organisms/MtgCard.tsx
export const MtgCard: React.FC<MtgCardProps> = ({ card, context }) => {
  const [modalOpen, setModalOpen] = useState(false);

  return (
    <InteractiveCard
      card={card}
      onCardClick={() => handleCardNavigation(card)}
      onZoomClick={() => setModalOpen(true)}
    >
      <CardImageWithOverlay card={card}>
        <CardBadgeGroup card={card} />
        <ZoomIndicator />
      </CardImageWithOverlay>

      <CardOverlay card={card} context={context} />

      <CardDetailsModal
        open={modalOpen}
        card={card}
        onClose={() => setModalOpen(false)}
      />
    </InteractiveCard>
  );
};

// molecules/Cards/InteractiveCard.tsx
export const InteractiveCard: React.FC<InteractiveCardProps> = ({
  card,
  onCardClick,
  onZoomClick,
  children
}) => {
  const cardStyles = useCardStyles(card.rarity);
  const { handleClick, handleKeyDown } = useCardInteractions({
    onCardClick,
    onZoomClick
  });

  return (
    <MuiCard
      onClick={handleClick}
      onKeyDown={handleKeyDown}
      sx={cardStyles}
      tabIndex={0}
      role="button"
      aria-label={`${card.name} card`}
    >
      {children}
    </MuiCard>
  );
};
```

### Example 2: Extracting CardBadges Logic

**Before (342 lines in one file):**
```typescript
export const CardBadges: React.FC<CardBadgesProps> = ({
  foil, nonfoil, etched, promoTypes, frameEffects, ...
}) => {
  // 100+ lines of badge calculation logic
  const hasSpecialFoilPromo = promoTypes.some(promo => ...);
  const isSerialized = promoTypes.some(promo => ...);
  const displayProperties: string[] = [];

  // 200+ lines of formatting functions
  const formatFinishText = (finish: string) => { ... };
  const formatPromoText = (promoType: string) => { ... };
  const formatFrameEffectText = (effect: string) => { ... };

  return (
    <Box>
      {/* 40+ lines of badge rendering */}
    </Box>
  );
};
```

**After (split into multiple files):**
```typescript
// atoms/Cards/BadgeChip.tsx (20 lines)
export const BadgeChip: React.FC<BadgeChipProps> = ({ label, type }) => (
  <Chip
    label={label}
    size="small"
    sx={{ ...getBadgeStyles(type) }}
  />
);

// hooks/useCardBadges.ts (100 lines)
export const useCardBadges = (card: Card) => {
  return useMemo(() => {
    const badges: Badge[] = [];

    // All badge calculation logic here
    if (shouldShowFoilBadge(card)) {
      badges.push({ type: 'foil', label: 'Foil' });
    }
    // ...

    return badges;
  }, [card]);
};

// utils/badgeFormatters.ts (100 lines)
export const formatFinishText = (finish: string): string => { ... };
export const formatPromoText = (promoType: string): string => { ... };
export const formatFrameEffectText = (effect: string): string => { ... };

// molecules/Cards/CardBadgeGroup.tsx (20 lines)
export const CardBadgeGroup: React.FC<CardBadgeGroupProps> = ({ card, inline }) => {
  const badges = useCardBadges(card);

  return (
    <Box sx={{ display: 'flex', flexDirection: inline ? 'row' : 'column', gap: 0.5 }}>
      {badges.map(badge => (
        <BadgeChip key={badge.id} type={badge.type} label={badge.label} />
      ))}
    </Box>
  );
};
```

### Example 3: Creating FilterableContentTemplate

**Before (SetPage.tsx - 670 lines):**
```typescript
export const SetPage: React.FC = () => {
  // 100+ lines of state management
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('collector-asc');
  // ... many more states

  // 200+ lines of data fetching
  const { data: cardsData } = useQuery(...);
  const { data: setData } = useQuery(...);
  // ... more queries

  // 150+ lines of filter logic
  const filteredCards = useMemo(() => { ... }, [cards, filters]);
  const sortedCards = useMemo(() => { ... }, [filteredCards, sortBy]);

  // 220+ lines of JSX
  return (
    <Container>
      <SetPageHeader ... />
      <Box sx={{ display: 'flex', gap: 3 }}>
        <Box sx={{ width: 280 }}>
          <DebouncedSearchInput ... />
          <SortDropdown ... />
          <MultiSelectDropdown ... />
          {/* More filters */}
        </Box>
        <Box sx={{ flex: 1 }}>
          <ResultsSummary ... />
          {showGroups ? (
            <CardGroups ... />
          ) : (
            <CardGrid ... />
          )}
        </Box>
      </Box>
    </Container>
  );
};
```

**After (SetPage.tsx - ~150 lines):**
```typescript
export const SetPage: React.FC = () => {
  const { setCode } = useParams();
  const setData = useSetData(setCode);
  const filterState = useSetPageFilters(setData.cards);

  return (
    <FilterableContentTemplate
      queries={[setData.query, cardsData.query]}
      header={<SetPageHeader set={setData.set} />}
      filters={<SetPageFilters {...filterState} />}
      content={
        <SetPageCardDisplay
          cards={filterState.filteredCards}
          showGroups={filterState.showGroups}
        />
      }
      summary={{
        current: filterState.currentCount,
        total: setData.totalCount
      }}
    />
  );
};

// templates/FilterableContentTemplate.tsx (~80 lines)
export const FilterableContentTemplate: React.FC<FilterableContentTemplateProps> = ({
  queries,
  header,
  filters,
  content,
  summary
}) => {
  const { isLoading, hasError, firstError } = useQueryStates(queries);

  return (
    <PageLayout>
      <QueryStateContainer loading={isLoading} error={firstError}>
        <Container maxWidth="xl">
          {header}

          <Box sx={{ display: 'flex', gap: 3, my: 3 }}>
            <FilterSidebar>{filters}</FilterSidebar>

            <ContentArea>
              <ResultsSummary {...summary} />
              {content}
            </ContentArea>
          </Box>
        </Container>
      </QueryStateContainer>
    </PageLayout>
  );
};
```

---

## Testing Recommendations

### Components That Need Tests

**Priority 1 (Atoms):**
- `DebouncedSearchInput` - Complex logic needs thorough testing
- `MultiSelectDropdown` - Option normalization needs tests
- `CardBadges` - Badge logic has many edge cases

**Priority 2 (Molecules):**
- `CardCompact` - Interaction patterns need testing
- `ManaCost` - Symbol parsing needs tests
- `QueryStateContainer` - State transitions need tests

**Priority 3 (Organisms):**
- `CardGrid` - Navigation logic needs tests
- `MtgCard` - Interaction and state tests

### Testing Patterns

```typescript
// atoms/shared/__tests__/AppButton.test.tsx
describe('AppButton', () => {
  it('shows loading spinner when loading prop is true', () => {
    render(<AppButton loading>Click me</AppButton>);
    expect(screen.getByRole('progressbar')).toBeInTheDocument();
  });

  it('disables button when loading', () => {
    render(<AppButton loading>Click me</AppButton>);
    expect(screen.getByRole('button')).toBeDisabled();
  });

  it('calls onClick when clicked', () => {
    const handleClick = jest.fn();
    render(<AppButton onClick={handleClick}>Click me</AppButton>);
    fireEvent.click(screen.getByRole('button'));
    expect(handleClick).toHaveBeenCalledTimes(1);
  });
});
```

---

## Conclusion

The MTG Discovery frontend has a solid foundation with atomic design principles, but there are clear opportunities for improvement:

### Immediate Actions (P0):
1. Remove all Tailwind CSS usage
2. Split complex atoms into proper molecules
3. Create missing App* wrapper atoms

### Short-Term Goals (P1):
4. Standardize query state handling
5. Reduce organism complexity
6. Create missing templates

### Long-Term Goals (P2):
7. Establish composition guidelines
8. Create layout atom library
9. Implement context for prop drilling
10. Add comprehensive component tests

### Success Metrics:
- **Atomic Purity:** All atoms are truly atomic (< 100 lines, single responsibility)
- **Template Usage:** All pages use templates (< 200 lines per page)
- **Consistency:** 100% MUI usage, 0% Tailwind
- **Testability:** 80%+ component test coverage
- **Performance:** All list components use React.memo appropriately

By following these recommendations, the codebase will be more maintainable, consistent, and aligned with atomic design principles.
