# Mobile-First UI Redesign - Comprehensive Analysis & Implementation Plan

**Document Version:** 1.1
**Date:** January 5, 2026
**Status:** Ready for Review

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Current State Analysis](#current-state-analysis)
3. [Requirements Summary](#requirements-summary)
4. [Breakpoint Strategy](#breakpoint-strategy)
5. [Component Redesign Specifications](#component-redesign-specifications)
6. [Page Layout Redesign](#page-layout-redesign)
7. [Navigation Redesign](#navigation-redesign)
8. [Filter System Redesign](#filter-system-redesign)
9. [Image Loading Strategy](#image-loading-strategy)
10. [Infrastructure Requirements](#infrastructure-requirements)
11. [Dead Code Analysis](#dead-code-analysis)
12. [Implementation Phases](#implementation-phases)
13. [Risk Assessment](#risk-assessment)
14. [Testing Strategy](#testing-strategy)
15. [Appendix: File Inventory](#appendix-file-inventory)

---

## 1. Executive Summary

### Purpose
This document provides a comprehensive analysis and implementation plan for converting the MTG Discovery platform from a desktop-first to a mobile-first responsive design. The current implementation uses fixed dimensions that do not adapt to different screen sizes, resulting in poor mobile user experience.

### Scope
- **In Scope:** All card display components, set display components, page layouts, navigation, filter systems
- **Out of Scope:** Collection modification features (explicitly excluded from mobile), offline mode, backend changes

### Key Outcomes
- Cards viewable in grids of 3-9 items across all screen sizes
- User-controllable density (compact/normal/large cards)
- Mobile-optimized navigation and filter interactions
- Platform-appropriate UX patterns (iOS/Android conventions)
- Improved desktop experience alongside mobile support

---

## 2. Current State Analysis

### 2.1 Card Display Components

#### MtgCard Component
**File:** `src/components/organisms/Cards/MtgCard.tsx`
**Lines:** 126
**Status:** Actively used, no responsive behavior

**Current Implementation:**
```typescript
// Fixed aspect ratio, no responsive props
<Box sx={{
  width: '100%',
  aspectRatio: '745 / 1040',
  position: 'relative'
}}>
```

**Issues:**
- Width determined entirely by parent grid (fixed 280px)
- No breakpoint-specific behavior
- Always shows all elements (badges, zoom indicator, overlay)
- Touch interactions not optimized

#### CardOverlay Component
**File:** `src/components/molecules/Cards/CardOverlay.tsx`
**Lines:** 167
**Status:** Actively used, no responsive behavior

**Current Information Display:**
1. Release date (optional)
2. Collector info row (rarity badge, collector number, reserved list, collection summary)
3. Artist links
4. Card name (unless on card page)
5. Set link (unless hidden)
6. Price and external links row

**Issues:**
- All information always displayed
- No progressive disclosure
- Information density too high for small cards
- No tap-to-expand behavior

#### CollectionSummary Component
**File:** `src/components/molecules/Cards/CollectionSummary.tsx`
**Lines:** 361
**Status:** Actively used, **already has mobile behavior**

**Current Mobile Behavior (Positive Example):**
```typescript
const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

const handleClick = (event: React.MouseEvent<HTMLElement>) => {
  if (isMobile && !isHovered) {
    setIsHovered(true); // First click shows hover state
  } else {
    setAnchorEl(event.currentTarget); // Second click shows popover
  }
};
```

**Emoji Indicators:**
- 🔹 Nonfoil, ✨ Foil, 🌟 Etched
- 📜 Artist Proof, ✍️ Signed, 🎨 Altered
- ⭕ Empty collection

**Note:** This component demonstrates the tap-to-reveal pattern that should be applied elsewhere.

#### CardDetailsModal Component
**File:** `src/components/organisms/Cards/CardDetailsModal.tsx`
**Lines:** 618
**Status:** Actively used, **completely broken on mobile**

**Current Layout:**
```typescript
<ModalContainer
  width="90vw"
  maxWidth={1400}
  height="90vh"
>
  {/* Left side - 45% */}
  <Box sx={{ width: '45%', ... }}>
    <CardImageDisplay size="large" />
  </Box>

  {/* Right side - 55% */}
  <Box sx={{ flex: 1, overflow: 'auto' }}>
    {/* All card details */}
  </Box>
</ModalContainer>
```

**Issues:**
- Fixed 45%/55% split doesn't work on narrow screens
- No mobile-specific layout
- Navigation arrows small and hard to tap
- Content overflow handling problematic on mobile

#### CardImageDisplay Component
**File:** `src/components/organisms/Cards/CardImageDisplay.tsx`
**Status:** Actively used

**Supported Sizes:**
- `small`: 146x204px (~20KB)
- `normal`: 488x680px (~100KB)
- `large`: 672x936px (~200KB)

**Available but Unused:**
- `artCrop`: Variable, art only
- `borderCrop`: Variable, no black border

#### CardGrid Component
**File:** `src/components/organisms/Cards/CardGrid.tsx`
**Status:** Actively used, responsive issues

**Current Grid Implementation:**
```typescript
<ResponsiveGridAutoFit
  minItemWidth={minItemWidth} // Default: 280
  spacing={spacing}           // Default: 1.5
>
```

**Issue:** Fixed `minItemWidth=280` means:
- On 375px screen: 1 card (280px) + 95px wasted space
- No control over column count
- No user density preference

### 2.2 Set Display Components

#### MtgSetCard Component
**File:** `src/components/molecules/Sets/MtgSetCard.tsx`
**Lines:** 276
**Status:** Actively used, fixed dimensions

**Current Dimensions:**
```typescript
sx={{
  height: '360px',
  width: '240px',
  // Fixed dimensions regardless of screen size
}}
```

**Content Structure:**
1. Set title (SetTitle component)
2. Top badges (code, release date)
3. Set icon (large, centered)
4. Bottom badges (type, digital, foil-only)
5. Collection progress bar (if collector)
6. Card count display

**Issues:**
- Fixed 240x360px regardless of screen
- Too tall for mobile viewport
- All content always visible

### 2.3 Page Components

#### AllSetsPage
**File:** `src/components/pages/AllSetsPage.tsx`
**Lines:** 259

**Current Layout:**
- BrowseTemplate wrapper
- Header: "All Sets" title + collection stats
- Filters: FilterPanel (horizontal)
- Content: ResponsiveGridAutoFit with minItemWidth=240

**Issues:**
- Fixed grid sizing
- Filters always expanded
- No mobile-specific header

#### SetPage
**File:** `src/components/pages/SetPage.tsx`
**Lines:** 138

**Current Layout:**
- SetPageTemplate wrapper
- Header: SetPageHeader (set info)
- Filters: SetPageFilters (horizontal)
- Content: SetPageCardDisplay with CardGroups

**Issues:**
- Same grid issues as AllSetsPage
- Group headers not optimized for mobile
- Filter panel overwhelming on small screens

#### CardAllPrintingsPage
**File:** `src/components/pages/CardAllPrintingsPage.tsx`

**Current Layout:**
- PageContainer wrapper
- CardFilterPanel
- CardGrid

**Issues:** Same responsive grid issues

#### ArtistCardsPage
**File:** `src/components/pages/ArtistCardsPage.tsx`

**Current Layout:**
- BrowseTemplate
- ArtistPageHeader
- ArtistPageFilters
- ArtistPageCardDisplay with CardGrid

**Issues:** Same responsive grid issues

### 2.4 Navigation Components

#### Header Component
**File:** `src/components/organisms/shared/Header.tsx`
**Status:** No mobile adaptation

**Current Structure:**
```
| Logo | SetCodeSearch | AllSets | SearchDropdown | AuthButton |
```

**SearchDropdown Contents:**
- Cards search
- Artists search
- Convention Signing (conditional on hasCollector)

**Issues:**
- All items always visible
- No hamburger menu on mobile
- Search input takes space on mobile
- Convention signing needs to be hidden on mobile

### 2.5 Grid Components

#### ResponsiveGrid / ResponsiveGridAutoFit
**File:** `src/components/molecules/layouts/ResponsiveGrid.tsx`

**Implementation:**
```typescript
// ResponsiveGrid uses auto-fill (keeps empty tracks)
gridTemplateColumns: `repeat(auto-fill, minmax(${minItemWidth}px, 1fr))`

// ResponsiveGridAutoFit uses auto-fit (collapses empty tracks)
gridTemplateColumns: `repeat(auto-fit, minmax(${minItemWidth}px, 1fr))`
```

**Issue:** `minItemWidth` is fixed, not responsive to breakpoints

### 2.6 Theme & Infrastructure

#### Existing Responsive Infrastructure
**File:** `src/theme/index.ts`

**MTG-specific breakpoints:**
```typescript
mtg: {
  breakpoints: {
    mobile: '0px',
    tablet: '768px',
    desktop: '1024px',
    wide: '1440px'
  },
  dimensions: {
    cardWidth: {
      xs: '140px',
      sm: '180px',
      md: '200px',
      lg: '250px',
      xl: '280px'
    }
  },
  spacing: {
    touch: {
      minTarget: 44,    // iOS/Android minimum
      comfortable: 48,
      large: 56
    }
  }
}
```

**Note:** These values exist but are NOT being used by components.

#### useResponsiveBreakpoints Hook
**File:** `src/hooks/useResponsiveBreakpoints.ts`

**Returns:**
```typescript
{
  isMobile: boolean,
  isTablet: boolean,
  isDesktop: boolean,
  isWide: boolean,
  current: 'mobile' | 'tablet' | 'desktop' | 'wide',
  screenWidth: number
}
```

**Status:** Available but underutilized in card components

---

## 3. Requirements Summary

### 3.1 User Requirements (from conversation)

| Requirement | Detail |
|-------------|--------|
| Breakpoints | Industry standard MUI breakpoints |
| Priority Components | SetCard, CardDisplay (core), then pages |
| Grid Density | 3-9 cards visible; slider option on mobile |
| Card Info Required | Image, name, artist, set, price, collection status |
| Image Strategy | Consider art_crop, border_crop (top half) |
| Card Details Reference | Moxfield card details UX |
| Tap Interactions | Acceptable; already have tap-to-open |
| Filter Priority | Collection count filter most important |
| Offline Mode | **Not planned** - App requires network connection. No service workers, offline caching, or sync-when-reconnected functionality. Basic loading/error states for failed requests are still needed. |
| Convention Signing | Hide link on small screens |
| Collection Modification | Exclude from mobile (for now) |
| Desktop Changes | Expected - not just "no changes" |
| Platform Norms | iOS toggles, Android patterns, etc. |

### 3.2 Technical Requirements

| Requirement | Detail |
|-------------|--------|
| Framework | React 19, Material-UI v7 |
| Styling | MUI sx props only (Tailwind being phased out) |
| State Management | Apollo Client, React Context |
| Build Tool | Vite |
| Browser Support | Modern browsers, Safari iOS, Chrome Android |
| Accessibility | WCAG AA, touch targets 44px minimum |

---

## 4. Breakpoint Strategy

### 4.1 Standard MUI Breakpoints

| Breakpoint | Range | Device Target |
|------------|-------|---------------|
| xs | 0-599px | Mobile phones |
| sm | 600-899px | Tablet portrait, large phones |
| md | 900-1199px | Tablet landscape, small laptops |
| lg | 1200-1535px | Desktop |
| xl | 1536px+ | Large desktop, wide monitors |

### 4.2 Breakpoint Usage by Component

| Component | xs | sm | md | lg | xl |
|-----------|----|----|----|----|-----|
| MtgCard | 3 cols, compact | 4-5 cols | auto-fill | auto-fill | auto-fill |
| MtgSetCard | 2 cols, small | 3 cols | 4 cols | 5 cols | 6 cols |
| CardOverlay | Collapsed | Tap-expand | Full | Full | Full |
| CardDetailsModal | Full-screen sheet | 85vw modal | 90vw modal | 90vw modal | 90vw modal |
| Header | Hamburger | Hamburger | Full nav | Full nav | Full nav |
| FilterPanel | Drawer | Accordion | Inline | Inline | Inline |

### 4.3 Card Width Mapping

| User Preference | xs | sm | md | lg | xl |
|-----------------|----|----|----|----|-----|
| Compact | 100px | 120px | 140px | 160px | 180px |
| Normal | 110px | 150px | 180px | 220px | 250px |
| Large | 130px | 180px | 220px | 260px | 280px |

---

## 5. Component Redesign Specifications

### 5.1 MtgCard Component

#### 5.1.1 Proposed Interface
```typescript
interface MtgCardProps extends StyledComponentProps {
  card: Card;
  context?: CardContext;
  index: number;
  groupId: string;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;

  // NEW RESPONSIVE PROPS
  size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl';           // Override auto size
  displayMode?: 'full' | 'compact';                   // Info density
  overlayBehavior?: 'always' | 'hover' | 'tap';       // Overlay interaction
}
```

#### 5.1.2 Mobile Behavior (xs: 0-599px)

**Visual Layout (Collapsed - Default):**
```
+------------------+
|                  |
|   Card Image     |
|   (small)        |
|                  |
+------------------+
| 3  Card Name     |  <- Collection count + name, always visible
+------------------+
```

**Expanded Overlay (after tap):**
```
+------------------+
|                  |
|   Card Image     |
|   (small)        |
|                  |
+------------------+
| 3  Card Name     |
| Artist Name      |
| SET #123  $5.99  |
+------------------+
```

**Specifications:**
- **Collection Count + Card Name**: Always visible at bottom in format "[count] [name]" or "⭕ [name]"
- **Collection Count**: Raw number (e.g., "3") for owned cards, ⭕ (red circle) for 0 collected
- **Card Name**: Truncated with ellipsis if too long
- Image size: `small` (146x204px)
- Aspect ratio: Maintain 745:1040
- CardBadges: Show only primary badge or hide
- ZoomIndicator: Hidden (tap opens details)
- Touch target: Entire card (min 44px height)
- Tap behavior: First tap expands overlay (shows artist, set, price), second tap opens details
- Full collection details (foil/nonfoil breakdown, special versions) available in CardDetailsSheet only

**Information Hierarchy (Mobile):**
1. Collection count + Card name (ALWAYS visible - bottom overlay)
2. Artist (on expand)
3. Set code + Collector number + Price (on expand)
4. Full collection breakdown (modal only)

#### 5.1.3 Tablet Behavior (sm-md: 600-1199px)

**Specifications:**
- Image size: `normal` (488x680px)
- CardBadges: Show 2-3 most relevant
- ZoomIndicator: Show on hover/focus
- Overlay: Show on tap OR hover (if pointer: fine)
- Touch target: Full card

#### 5.1.4 Desktop Behavior (lg+: 1200px+)

**Specifications:**
- Image size: `normal` or `large`
- CardBadges: Show all relevant
- ZoomIndicator: Show on hover
- Overlay: Full display on hover
- Current behavior preserved with polish

#### 5.1.5 Implementation Approach

**New Hook: `useCardDisplaySettings`**
```typescript
function useCardDisplaySettings(
  explicitSize?: CardSize,
  explicitMode?: DisplayMode
): {
  size: CardSize;
  displayMode: DisplayMode;
  overlayBehavior: OverlayBehavior;
  imageScryfallSize: 'small' | 'normal' | 'large';
  showBadges: boolean;
  showZoomIndicator: boolean;
}
```

**Component Structure Change:**
```typescript
const MtgCardComponent: React.FC<MtgCardProps> = ({
  card,
  context = {},
  size: explicitSize,
  displayMode: explicitMode,
  ...props
}) => {
  const displaySettings = useCardDisplaySettings(explicitSize, explicitMode);
  const [overlayExpanded, setOverlayExpanded] = useState(false);
  const { isMobile } = useResponsiveBreakpoints();

  const handleCardClick = () => {
    if (displaySettings.overlayBehavior === 'tap' && !overlayExpanded) {
      setOverlayExpanded(true);
    } else {
      // Open details modal/sheet
      handleZoomClick();
    }
  };

  // Get total collection count (sum of all copies)
  const collectionCount = card.userCollection?.totalCount ?? 0;

  return (
    <MuiCard onClick={handleCardClick} ...>
      <CardImageDisplay
        size={displaySettings.imageScryfallSize}
        ...
      />

      {displaySettings.showBadges && (
        <CardBadges
          compact={displaySettings.displayMode === 'compact'}
          ...
        />
      )}

      {displaySettings.showZoomIndicator && (
        <ZoomIndicator />
      )}

      <CardOverlay
        variant={displaySettings.displayMode === 'compact' ? 'minimal' : 'full'}
        expanded={overlayExpanded}
        onExpandToggle={() => setOverlayExpanded(!overlayExpanded)}
        collectionCount={collectionCount}  // Pass count for mobile display
        ...
      />

      {/* CardDetailsModal or CardDetailsSheet based on breakpoint */}
      {isMobile ? (
        <CardDetailsSheet open={modalOpen} onClose={handleModalClose} card={card} />
      ) : (
        <CardDetailsModal open={modalOpen} onClose={handleModalClose} card={card} />
      )}
    </MuiCard>
  );
};
```

**Note on Mobile Collection Display:**
- Collection count + card name shown in bottom overlay (single visual zone)
- Format: "[count] [name]" (e.g., "3 Lightning Bolt") or "⭕ [name]" for unowned cards
- Count is raw number for owned cards, ⭕ (red circle) for 0 collected (stands out visually)
- No foil/nonfoil/etched breakdown on mobile (saves space)
- Full collection details available in CardDetailsSheet modal
- Consistent with desktop overlay pattern (all info at bottom)

### 5.2 CardOverlay Component

#### 5.2.1 Proposed Interface
```typescript
interface CardOverlayProps {
  card: Card;
  isSelected?: boolean;
  context?: CardContext;
  onCardClick?: (cardId?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  className?: string;

  // NEW RESPONSIVE PROPS
  variant?: 'full' | 'compact' | 'minimal';
  expanded?: boolean;
  onExpandToggle?: () => void;
  collectionCount?: number;  // Raw count for mobile display
}
```

#### 5.2.2 Variant Specifications

**Variant: `minimal` (Mobile collapsed)**
- Shows: Collection count + Card name in format "[count] [name]" or "⭕ [name]"
- Single row, 44px height (meets touch target minimum)
- Semi-transparent background
- Collection count: raw number for owned cards, ⭕ (red circle) for 0 collected (stands out)
- Card name truncated with ellipsis if needed

**Variant: `compact` (Mobile expanded, Tablet)**
- Shows: Collection count + Card name, Artist, Set code + Collector # + Price
- Stacked rows with tight spacing
- 60-80px height
- Gradient background

**Variant: `full` (Desktop)**
- Current behavior: All information displayed
- Release date, collector info (with full breakdown), artist, name, set, price, links
- Full gradient overlay
- Full CollectionSummary with emoji indicators (🔹✨🌟 etc.)

#### 5.2.3 Implementation Structure
```typescript
export const CardOverlay: React.FC<CardOverlayProps> = React.memo(({
  card,
  variant = 'full',
  expanded = false,
  onExpandToggle,
  collectionCount = 0,
  ...props
}) => {
  const theme = useTheme();

  if (variant === 'minimal') {
    return (
      <Box sx={{
        ...minimalOverlayStyles,
        minHeight: 44,  // Touch target minimum
        p: 1,
      }}>
        <Box sx={{
          display: 'flex',
          alignItems: 'center',
          gap: 1,
        }}>
          {/* Collection count + Card name - always visible */}
          {props.context?.hasCollector && (
            <Typography
              component="span"
              sx={{ fontWeight: 'bold', minWidth: 20 }}
            >
              {collectionCount > 0 ? collectionCount : '⭕'}
            </Typography>
          )}
          <Typography
            noWrap
            sx={{
              flex: 1,
              overflow: 'hidden',
              textOverflow: 'ellipsis',
            }}
          >
            {card.name}
          </Typography>
        </Box>

        <Collapse in={expanded}>
          {/* Expanded content - artist, set, price */}
          <ArtistLinks ... />
          <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
            <SetLink ... />
            <PriceDisplay price={card.prices?.usd} />
          </Box>
        </Collapse>
      </Box>
    );
  }

  if (variant === 'compact') {
    return (
      <Box sx={compactOverlayStyles}>
        {/* Same as minimal but always expanded */}
      </Box>
    );
  }

  // variant === 'full' - current implementation with full CollectionSummary
  return (
    <Box sx={fullOverlayStyles}>
      {/* Current overlay content with emoji indicators */}
    </Box>
  );
});
```

### 5.3 MtgSetCard Component

#### 5.3.1 Proposed Interface
```typescript
interface MtgSetCardProps {
  set: MtgSet;
  context?: SetContext;
  onSetClick?: (setCode?: string) => void;
  className?: string;

  // NEW RESPONSIVE PROPS
  variant?: 'card' | 'compact';
  size?: 'sm' | 'md' | 'lg';
}
```

#### 5.3.2 Size Specifications

| Size | Width | Height | Icon Size | Content |
|------|-------|--------|-----------|---------|
| sm | calc-based (2 cols) | 200px | 48x48 | Icon, name, progress |
| md | 180px | 280px | 64x64 | Icon, name, code, date, progress |
| lg | 240px | 360px | 80x80 | Full content (current) |

#### 5.3.3 Mobile Layout (variant: 'compact')
```
+------+---------------------------+--------+
| Icon | Set Name                  | 85/100 |
| 48px | CODE - Jan 2024           | [====] |
+------+---------------------------+--------+
```

**Specifications:**
- Horizontal layout, 80-100px height
- Touch target: Entire row (min 48px)
- Progress bar: Inline with count

#### 5.3.4 Implementation Approach
```typescript
export const MtgSetCard: React.FC<MtgSetCardProps> = ({
  set,
  variant = 'card',
  size = 'lg',
  ...props
}) => {
  const { isMobile } = useResponsiveBreakpoints();

  const effectiveVariant = variant || (isMobile ? 'compact' : 'card');
  const effectiveSize = size || (isMobile ? 'sm' : 'lg');

  if (effectiveVariant === 'compact') {
    return <MtgSetListItem set={set} {...props} />;
  }

  return (
    <Card sx={getCardSizeStyles(effectiveSize)}>
      {/* Current card content with responsive adjustments */}
    </Card>
  );
};
```

### 5.4 CardDetailsModal/Sheet Component

#### 5.4.1 Mobile: CardDetailsSheet (NEW)

**File to Create:** `src/components/organisms/Cards/CardDetailsSheet.tsx`

**Layout Specification:**
```
+----------------------------------------+
| [X] Card Name                    [<] [>]|  <- Sticky header (56px)
+----------------------------------------+
|                                        |
|           Card Image                   |
|           (tap to zoom)                |  <- 40% viewport max
|           [Flip]                       |
|                                        |
+----------------------------------------+
|  Type Line                    [Rarity] |
|  Mana Cost: {W}{U}                     |
|  Set: Set Name [icon]  #123            |
|  Price: $5.99         [🔹✨ 3]         |
+----------------------------------------+
| ▼ Oracle Text                          |  <- Accordion sections
|   Card text here...                    |
+----------------------------------------+
| ▶ Flavor Text                          |
+----------------------------------------+
| ▶ Legalities (6 legal)                 |
+----------------------------------------+
| ▶ External Links                       |
+----------------------------------------+
| ▶ Other Printings (12)                 |
+----------------------------------------+
```

**Gesture Support:**
- Swipe down: Dismiss sheet
- Swipe left/right on image: Navigate to prev/next card
- Pinch on image: Zoom
- Double-tap image: Toggle zoom
- Tap outside content: Dismiss

**Component Structure:**
```typescript
interface CardDetailsSheetProps {
  open: boolean;
  onClose: () => void;
  card: Card;
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
}

export const CardDetailsSheet: React.FC<CardDetailsSheetProps> = ({
  open,
  onClose,
  card,
  ...navProps
}) => {
  return (
    <SwipeableDrawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      onOpen={() => {}}
      sx={{
        '& .MuiDrawer-paper': {
          height: '100vh',
          borderTopLeftRadius: 16,
          borderTopRightRadius: 16,
        }
      }}
    >
      {/* Drag handle */}
      <Box sx={dragHandleStyles} />

      {/* Sticky header */}
      <Box sx={stickyHeaderStyles}>
        <IconButton onClick={onClose}><CloseIcon /></IconButton>
        <Typography noWrap>{card.name}</Typography>
        <Box>
          <IconButton disabled={!navProps.hasPrevious} onClick={navProps.onPrevious}>
            <NavigateBeforeIcon />
          </IconButton>
          <IconButton disabled={!navProps.hasNext} onClick={navProps.onNext}>
            <NavigateNextIcon />
          </IconButton>
        </Box>
      </Box>

      {/* Scrollable content */}
      <Box sx={{ overflow: 'auto', flex: 1 }}>
        {/* Image section */}
        <CardImageSection card={card} />

        {/* Quick info */}
        <QuickInfoSection card={card} />

        {/* Accordion sections */}
        <Accordion defaultExpanded>
          <AccordionSummary>Oracle Text</AccordionSummary>
          <AccordionDetails>{card.oracleText}</AccordionDetails>
        </Accordion>

        {/* ... more accordions */}
      </Box>
    </SwipeableDrawer>
  );
};
```

#### 5.4.2 Desktop: CardDetailsModal Updates

**Changes to existing `CardDetailsModal.tsx`:**
1. Add breakpoint detection
2. Conditionally render modal vs sheet
3. Improve spacing and typography

```typescript
export const CardDetailsModal: React.FC<CardDetailsModalProps> = (props) => {
  const { isMobile } = useResponsiveBreakpoints();

  if (isMobile) {
    return <CardDetailsSheet {...props} />;
  }

  // Existing modal implementation with improvements
  return (
    <ModalContainer
      width={{ xs: '100%', sm: '85vw', md: '90vw' }}
      maxWidth={1400}
      height={{ xs: '100%', sm: '85vh', md: '90vh' }}
      ...
    >
      {/* Existing content */}
    </ModalContainer>
  );
};
```

---

## 6. Page Layout Redesign

### 6.1 AllSetsPage

#### 6.1.1 Mobile Layout (xs: 0-599px)
```
+----------------------------------------+
| [=]        Logo        [🔍]            |  <- Mobile header
+----------------------------------------+
| [Filter (2)]    [Sort ▼]               |  <- Sticky filter bar
+----------------------------------------+
| Showing 145 of 892 sets                |
+----------------------------------------+
| +------+ +------+ |  <- 2 column grid
| | Set  | | Set  | |
| | Card | | Card | |
| +------+ +------+ |
| +------+ +------+ |
| | Set  | | Set  | |
| | Card | | Card | |
| +------+ +------+ |
+----------------------------------------+
```

**Specifications:**
- Header: MobileHeader (hamburger, logo, search icon)
- Filter bar: Sticky, shows active filter count
- Grid: 2 columns, compact set cards
- Set cards: ~160px wide, 200px tall

#### 6.1.2 Tablet Layout (sm-md: 600-1199px)
```
+----------------------------------------+
| [=] Logo    [Search...]    [Auth]      |
+----------------------------------------+
|            All Sets                    |
|      [Collection Stats Summary]        |
+----------------------------------------+
| [Search] [Types▼] [Status▼] [Sort▼]    |  <- Inline filters
+----------------------------------------+
| Showing 145 of 892 sets                |
+----------------------------------------+
| +----+ +----+ +----+ +----+           |  <- 3-4 column grid
| |Set | |Set | |Set | |Set |           |
| +----+ +----+ +----+ +----+           |
+----------------------------------------+
```

#### 6.1.3 Desktop Layout (lg+: 1200px+)
- Current layout with improvements
- 5-6 column grid
- Full-size set cards (240x360px)

#### 6.1.4 Implementation Changes

**File:** `src/components/pages/AllSetsPage.tsx`

```typescript
export const AllSetsPage: React.FC = () => {
  const { isMobile, isTablet } = useResponsiveBreakpoints();
  const { showFilterDrawer, toggleFilterDrawer } = useMobileLayout();

  // ... existing hooks ...

  return (
    <GraphQLQueryStateContainer ...>
      <BrowseTemplate
        maxWidth={false}
        stickyFilters={isMobile}
        filterMode={isMobile ? 'drawer' : 'inline'}
        mobileFilterTrigger={
          <MobileFilterButton
            count={activeFilterCount}
            onClick={toggleFilterDrawer}
          />
        }
        header={
          <Box sx={{ width: '100%' }}>
            <Heading
              variant={{ xs: 'h5', sm: 'h4', md: 'h3' }}
              sx={{ textAlign: 'center' }}
            >
              All Sets
            </Heading>
            {hasCollector && !isMobile && <CollectionStatsSummary sets={sets} />}
          </Box>
        }
        filters={
          isMobile ? null : (
            <FilterPanel config={...} layout="horizontal" />
          )
        }
        content={
          <ResponsiveGridAutoFit
            minItemWidth={{ xs: 160, sm: 180, md: 200, lg: 220, xl: 240 }}
            spacing={{ xs: 1, sm: 1.5 }}
          >
            {filteredSets.map((set) => (
              <MtgSetCard
                key={set.id}
                set={set}
                size={isMobile ? 'sm' : isTablet ? 'md' : 'lg'}
              />
            ))}
          </ResponsiveGridAutoFit>
        }
      />

      {/* Mobile filter drawer */}
      <FilterDrawer
        open={showFilterDrawer}
        onClose={toggleFilterDrawer}
      >
        <FilterPanel config={...} layout="vertical" />
      </FilterDrawer>
    </GraphQLQueryStateContainer>
  );
};
```

### 6.2 SetPage

#### 6.2.1 Mobile Layout
```
+----------------------------------------+
| [=]        Logo        [🔍]            |
+----------------------------------------+
| [Set Icon] Set Name                    |
| CODE - 280 cards - Jan 2024            |
+----------------------------------------+
| [🔍 Search]  [Filter (1)]  [Sort▼]     |  <- Sticky
+----------------------------------------+
| Showing 280 of 280 cards               |
+----------------------------------------+
| ═══ COMMANDER CARDS (24) ═══           |  <- Group header
+----------------------------------------+
| +---+ +---+ +---+                      |  <- 3 column grid
| |   | |   | |   |                      |
| +---+ +---+ +---+                      |
+----------------------------------------+
```

#### 6.2.2 Implementation Changes

**File:** `src/components/pages/SetPage.tsx`

Key changes:
- Responsive header sizing
- Sticky filter bar on mobile
- Simplified group headers
- 3+ column card grid on mobile

### 6.3 CardAllPrintingsPage

#### 6.3.1 Changes
- Responsive title: h4 on mobile, h2 on desktop
- Same grid and filter patterns as SetPage
- Card context: `isOnCardPage: true` to hide card names

### 6.4 ArtistCardsPage

#### 6.4.1 Changes
- Responsive artist header
- Alternate names: Collapsible on mobile
- Same grid and filter patterns

---

## 7. Navigation Redesign

### 7.1 Current Header Structure
**File:** `src/components/organisms/shared/Header.tsx`

```
| Logo | SetCodeSearch | AllSets | SearchDropdown | AuthButton |
```

### 7.2 Proposed Mobile Header

**New File:** `src/components/organisms/shared/MobileHeader.tsx`

```
| [=]               Logo               [🔍] |
   ↓                                      ↓
  Hamburger                            Search
  opens drawer                         overlay
```

**Specifications:**
- Height: 56px (standard mobile app bar)
- Fixed position with safe-area-inset-top padding
- Logo: Centered, smaller
- Touch targets: 48px minimum

### 7.3 Navigation Drawer

**New File:** `src/components/organisms/shared/NavigationDrawer.tsx`

**Contents:**
```
+-------------------------+
| [User Avatar]           |
| User Name               |
| user@email.com          |
+-------------------------+
| 📚 All Sets             |
| 🔍 Search Cards         |
| 🎨 Search Artists       |
|                         |
| [Convention Signing]    |  <- HIDDEN on mobile (xs)
|                         |
+-------------------------+
| ⚙️ Settings             |
| 🚪 Sign Out             |
+-------------------------+
```

**Convention Signing Visibility Logic:**
```typescript
const showConventionSigning = hasCollector && !isMobile;
// Only show on tablet (sm) and above
```

### 7.4 Responsive Header Component

**Modified File:** `src/components/organisms/shared/Header.tsx`

```typescript
export const Header: React.FC = () => {
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  if (isMobile) {
    return <MobileHeader />;
  }

  if (isTablet) {
    return <TabletHeader />;  // Condensed with hamburger
  }

  return <DesktopHeader />;  // Current full navigation
};
```

---

## 8. Filter System Redesign

### 8.1 Current FilterPanel
**File:** `src/components/organisms/filters/FilterPanel.tsx`

**Current Layout:** Horizontal Grid with all filters visible

### 8.2 Mobile Filter Strategy

#### 8.2.1 Sticky Filter Bar
```
+----------------------------------------+
| [🔍 Search cards...]  [Filter(2)] [▼]  |
+----------------------------------------+
```

- Search: Always visible (most common action)
- Filter button: Shows active filter count badge
- Sort: Dropdown or part of filter drawer

#### 8.2.2 Filter Drawer

**New File:** `src/components/molecules/shared/FilterDrawer.tsx`

```typescript
interface FilterDrawerProps {
  open: boolean;
  onClose: () => void;
  filterCount?: number;
  children: React.ReactNode;  // FilterPanel content
}

export const FilterDrawer: React.FC<FilterDrawerProps> = ({
  open,
  onClose,
  filterCount = 0,
  children
}) => {
  return (
    <SwipeableDrawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      onOpen={() => {}}
      sx={{
        '& .MuiDrawer-paper': {
          maxHeight: '70vh',
          borderTopLeftRadius: 16,
          borderTopRightRadius: 16,
        }
      }}
    >
      {/* Drag handle */}
      <Box sx={dragHandleStyles}>
        <Box sx={{ width: 32, height: 4, bgcolor: 'grey.400', borderRadius: 2 }} />
      </Box>

      {/* Header */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', p: 2 }}>
        <Typography variant="h6">
          Filters {filterCount > 0 && `(${filterCount})`}
        </Typography>
        <IconButton onClick={onClose}><CloseIcon /></IconButton>
      </Box>

      <Divider />

      {/* Filter content */}
      <Box sx={{ overflow: 'auto', flex: 1, p: 2 }}>
        {children}
      </Box>

      {/* Sticky footer */}
      <Box sx={{ p: 2, borderTop: 1, borderColor: 'divider' }}>
        <Button fullWidth variant="contained" onClick={onClose}>
          Apply Filters
        </Button>
      </Box>
    </SwipeableDrawer>
  );
};
```

#### 8.2.3 Mobile Filter Button

**New File:** `src/components/molecules/shared/MobileFilterButton.tsx`

```typescript
interface MobileFilterButtonProps {
  count: number;
  onClick: () => void;
}

export const MobileFilterButton: React.FC<MobileFilterButtonProps> = ({
  count,
  onClick
}) => {
  return (
    <Button
      variant="outlined"
      onClick={onClick}
      startIcon={<FilterListIcon />}
      endIcon={count > 0 ? <Badge badgeContent={count} color="primary" /> : null}
    >
      Filter
    </Button>
  );
};
```

### 8.3 Filter Priority (Mobile)

When space is limited, prioritize these filters:

1. **Search** - Always visible
2. **Collection Count** - Most important for collectors
3. **Rarity** - Common filter
4. **Sort** - Essential for browsing

Lower priority (in drawer):
- Artist
- Finish
- Signed
- Set (on artist page)
- Format (on card page)

---

## 9. Image Loading Strategy

### 9.1 Scryfall Image Sizes

| Field | Dimensions | File Size | Use Case |
|-------|------------|-----------|----------|
| small | 146x204px | ~20KB | Thumbnails, compact grids |
| normal | 488x680px | ~100KB | Standard display |
| large | 672x936px | ~200KB | Detail views, zoom |
| png | 745x1040px | ~300KB | Not recommended (too large) |
| artCrop | Variable | Variable | Art-only displays |
| borderCrop | Variable | Variable | Compact without frame |

### 9.2 Image Selection by Context

| Context | Compact Pref | Normal Pref | Large Pref |
|---------|--------------|-------------|------------|
| Grid xs | small | small | normal |
| Grid sm | small | normal | normal |
| Grid md | normal | normal | normal |
| Grid lg+ | normal | normal | large |
| Details Modal | large | large | large |
| Zoom View | large | large | large |

### 9.3 art_crop and border_crop Usage

**art_crop:**
- Optional "art gallery" view mode (future enhancement)
- Horizontal aspect ratio
- Shows only card art without frame
- Useful for browsing/discovery

**border_crop:**
- Consider for "top half" card display
- Slightly smaller than normal
- Card without black border

**Current Recommendation:** Focus on small/normal/large for initial implementation. Add art_crop/border_crop as optional view modes in future phase.

### 9.4 Implementation in CardImageDisplay

**File:** `src/components/organisms/Cards/CardImageDisplay.tsx`

```typescript
interface CardImageDisplayProps {
  card: Card;
  size?: 'small' | 'normal' | 'large' | 'artCrop' | 'borderCrop';
  // ... existing props
}

const getImageUrl = (card: Card, size: ImageSize): string => {
  const imageUris = card.imageUris || card.cardFaces?.[0]?.imageUris;
  if (!imageUris) return FALLBACK_IMAGE;

  switch (size) {
    case 'small': return imageUris.small || imageUris.normal;
    case 'normal': return imageUris.normal;
    case 'large': return imageUris.large || imageUris.normal;
    case 'artCrop': return imageUris.artCrop || imageUris.normal;
    case 'borderCrop': return imageUris.borderCrop || imageUris.normal;
    default: return imageUris.normal;
  }
};
```

### 9.5 Lazy Loading

Current implementation is good:
- IntersectionObserver with rootMargin
- `loading="lazy"` attribute
- Skeleton placeholder

**Enhancement:** Increase rootMargin on mobile for faster perceived loading:
```typescript
const rootMargin = isMobile ? '200px' : '100px';
```

---

## 10. Infrastructure Requirements

### 10.1 New Hooks

#### useCardSizePreference
```typescript
// File: src/hooks/useCardSizePreference.ts

type CardSizePref = 'compact' | 'normal' | 'large';

interface CardSizePreference {
  size: CardSizePref;
  setSize: (size: CardSizePref) => void;
  getMinItemWidth: (breakpoint: Breakpoint) => number;
  getImageSize: () => 'small' | 'normal' | 'large';
}

export function useCardSizePreference(): CardSizePreference {
  const [size, setSizeState] = useState<CardSizePref>(() => {
    const stored = localStorage.getItem('cardSizePreference');
    return (stored as CardSizePref) || 'normal';
  });

  const setSize = useCallback((newSize: CardSizePref) => {
    setSizeState(newSize);
    localStorage.setItem('cardSizePreference', newSize);
  }, []);

  const getMinItemWidth = useCallback((breakpoint: Breakpoint): number => {
    const widthMap: Record<CardSizePref, Record<Breakpoint, number>> = {
      compact: { xs: 100, sm: 120, md: 140, lg: 160, xl: 180 },
      normal: { xs: 110, sm: 150, md: 180, lg: 220, xl: 250 },
      large: { xs: 130, sm: 180, md: 220, lg: 260, xl: 280 },
    };
    return widthMap[size][breakpoint];
  }, [size]);

  const getImageSize = useCallback(() => {
    const imageMap: Record<CardSizePref, 'small' | 'normal' | 'large'> = {
      compact: 'small',
      normal: 'normal',
      large: 'normal',
    };
    return imageMap[size];
  }, [size]);

  return { size, setSize, getMinItemWidth, getImageSize };
}
```

#### useMobileLayout
```typescript
// File: src/hooks/useMobileLayout.ts

interface MobileLayoutState {
  isMobile: boolean;
  isTouch: boolean;
  showFilterDrawer: boolean;
  toggleFilterDrawer: () => void;
  safeAreaInsets: {
    top: number;
    bottom: number;
    left: number;
    right: number;
  };
}

export function useMobileLayout(): MobileLayoutState {
  const { isMobile } = useResponsiveBreakpoints();
  const isTouch = useMediaQuery('(pointer: coarse)');
  const [showFilterDrawer, setShowFilterDrawer] = useState(false);

  const toggleFilterDrawer = useCallback(() => {
    setShowFilterDrawer(prev => !prev);
  }, []);

  // Safe area insets are handled via CSS env() in practice
  const safeAreaInsets = {
    top: 0,
    bottom: 0,
    left: 0,
    right: 0,
  };

  return {
    isMobile,
    isTouch,
    showFilterDrawer,
    toggleFilterDrawer,
    safeAreaInsets,
  };
}
```

### 10.2 Theme Extensions

**File:** `src/theme/index.ts`

```typescript
// Add to mtg object
mtg: {
  // ... existing properties

  mobile: {
    headerHeight: 56,
    filterBarHeight: 48,
    bottomNavHeight: 56,
    drawerMaxWidth: '85vw',
    sheetMaxHeight: '70vh',
    sheetBorderRadius: 16,
  },

  mediaQueries: {
    touch: '@media (pointer: coarse)',
    hover: '@media (hover: hover)',
    prefersReducedMotion: '@media (prefers-reduced-motion: reduce)',
  },
}
```

### 10.3 CSS Custom Properties

**File:** `src/index.css` (add to existing)

```css
:root {
  /* Card sizing variables */
  --card-width-compact: 140px;
  --card-width-normal: 200px;
  --card-width-large: 280px;

  /* Safe area insets (iOS) */
  --safe-area-top: env(safe-area-inset-top, 0px);
  --safe-area-bottom: env(safe-area-inset-bottom, 0px);
  --safe-area-left: env(safe-area-inset-left, 0px);
  --safe-area-right: env(safe-area-inset-right, 0px);
}

/* Reduced motion support */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* iOS momentum scrolling */
.scroll-container {
  -webkit-overflow-scrolling: touch;
}
```

### 10.4 Responsive Grid Updates

**File:** `src/components/molecules/layouts/ResponsiveGrid.tsx`

```typescript
interface ResponsiveGridProps {
  minItemWidth?: number | Record<Breakpoint, number>;  // Now accepts responsive values
  spacing?: number | Record<Breakpoint, number>;
  children: React.ReactNode;
  // ... existing props
}

export const ResponsiveGridAutoFit: React.FC<ResponsiveGridProps> = ({
  minItemWidth = 250,
  spacing = 3,
  children,
  ...props
}) => {
  const theme = useTheme();

  // Calculate responsive minItemWidth
  const getMinWidth = (breakpoint: string): string => {
    if (typeof minItemWidth === 'number') {
      return `${minItemWidth}px`;
    }
    return `${minItemWidth[breakpoint as Breakpoint] || 250}px`;
  };

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: {
          xs: `repeat(auto-fit, minmax(${getMinWidth('xs')}, 1fr))`,
          sm: `repeat(auto-fit, minmax(${getMinWidth('sm')}, 1fr))`,
          md: `repeat(auto-fit, minmax(${getMinWidth('md')}, 1fr))`,
          lg: `repeat(auto-fit, minmax(${getMinWidth('lg')}, 1fr))`,
          xl: `repeat(auto-fit, minmax(${getMinWidth('xl')}, 1fr))`,
        },
        gap: spacing,
        justifyContent: 'center',
      }}
      {...props}
    >
      {children}
    </Box>
  );
};
```

---

## 11. Dead Code Analysis

### 11.1 CardDisplayResponsive

**File:** `src/components/organisms/Cards/CardDisplayResponsive.tsx`
**Lines:** ~413
**Status:** NOT IMPORTED ANYWHERE

**Analysis:**
- Has dual layout (horizontal mobile, vertical desktop)
- Uses gesture hooks correctly
- More feature-rich than current MtgCard
- Contains patterns that could inform the redesign

**Recommendation:** **DELETE** after extracting useful patterns into documentation.

**Useful Patterns to Document:**
```typescript
// Mobile horizontal layout pattern
<Box sx={{
  display: { xs: 'flex', sm: 'none' },
  flexDirection: 'row',
  alignItems: 'center',
  gap: 2,
}}>
  <Box sx={{ width: 80, aspectRatio: '745/1040' }}>
    <CardImage />
  </Box>
  <Box sx={{ flex: 1 }}>
    <Typography>{name}</Typography>
    <Typography>{artist}</Typography>
    <Typography>{price}</Typography>
  </Box>
</Box>
```

### 11.2 CardCompact

**File:** `src/components/organisms/Cards/CardCompact.tsx`
**Status:** May be used, needs verification

**Issues Found:**
- Contains debug `console.log` statements (lines 45-47)
- Should be removed regardless of usage

**Recommendation:**
1. Search for imports: `grep -r "CardCompact" src/`
2. If unused: **DELETE**
3. If used: Remove debug logging, consider merging patterns into MtgCard

### 11.3 Cleanup Checklist

| File | Action | Reason |
|------|--------|--------|
| CardDisplayResponsive.tsx | DELETE | Unused, patterns documented |
| CardCompact.tsx | VERIFY & CLEAN | Remove console.log, verify usage |
| useLongPress.ts | KEEP | Well-implemented, ready for use |
| useSwipeGesture.ts | KEEP | Well-implemented, ready for use |
| useHapticFeedback.ts | KEEP | Well-implemented, ready for use |

---

## 12. Implementation Phases

### Phase 1: Infrastructure Foundation
**Risk:** Low
**Dependencies:** None

**Deliverables:**
1. `src/hooks/useCardSizePreference.ts` - NEW
2. `src/hooks/useMobileLayout.ts` - NEW
3. `src/theme/index.ts` - Extended with mobile tokens
4. `src/index.css` - CSS custom properties added

**Acceptance Criteria:**
- [ ] Hooks return correct values at each breakpoint
- [ ] Card size preference persists in localStorage
- [ ] CSS variables accessible in components
- [ ] No breaking changes to existing functionality

### Phase 2: Responsive Grid System
**Risk:** Medium
**Dependencies:** Phase 1

**Deliverables:**
1. `src/components/molecules/layouts/ResponsiveGrid.tsx` - Updated to accept responsive minItemWidth
2. `src/components/organisms/Cards/CardGrid.tsx` - Uses responsive grid

**Acceptance Criteria:**
- [ ] Grid shows 3+ cards on mobile (375px)
- [ ] Grid adapts columns at each breakpoint
- [ ] No layout shift during resize
- [ ] Existing pages still function

### Phase 3: MtgCard Component
**Risk:** Medium
**Dependencies:** Phase 1, Phase 2

**Deliverables:**
1. `src/components/organisms/Cards/MtgCard.tsx` - Responsive props added
2. `src/hooks/useMtgCardStyles.ts` - Updated for responsive sizing
3. `src/hooks/useMtgCardInteractions.ts` - Tap-to-expand behavior

**Acceptance Criteria:**
- [ ] Cards render correctly at all sizes
- [ ] Tap-to-expand works on mobile
- [ ] Hover behavior preserved on desktop
- [ ] Image size appropriate for card size

### Phase 4: CardOverlay Component
**Risk:** Low-Medium
**Dependencies:** Phase 3

**Deliverables:**
1. `src/components/molecules/Cards/CardOverlay.tsx` - Variant support added

**Acceptance Criteria:**
- [ ] Minimal variant shows only price/collection
- [ ] Compact variant shows essential info
- [ ] Full variant unchanged (desktop)
- [ ] Expand/collapse animation smooth

### Phase 5: MtgSetCard Component
**Risk:** Medium
**Dependencies:** Phase 1, Phase 2

**Deliverables:**
1. `src/components/molecules/Sets/MtgSetCard.tsx` - Responsive sizing

**Acceptance Criteria:**
- [ ] Set cards render at 3 sizes (sm/md/lg)
- [ ] 2 columns on mobile, 3-4 on tablet, 5-6 on desktop
- [ ] Collection progress visible at all sizes
- [ ] Touch targets meet 44px minimum

### Phase 6: CardDetailsModal/Sheet
**Risk:** High
**Dependencies:** Phase 3

**Deliverables:**
1. `src/components/organisms/Cards/CardDetailsSheet.tsx` - NEW
2. `src/components/organisms/Cards/CardDetailsModal.tsx` - Updated to switch by breakpoint

**Acceptance Criteria:**
- [ ] Sheet opens full-screen on mobile
- [ ] Swipe down dismisses sheet
- [ ] Navigation between cards works
- [ ] All card info accessible in accordions
- [ ] Modal unchanged on desktop

### Phase 7: Mobile Filter Drawer
**Risk:** Medium
**Dependencies:** Phase 1

**Deliverables:**
1. `src/components/molecules/shared/FilterDrawer.tsx` - NEW
2. `src/components/molecules/shared/MobileFilterButton.tsx` - NEW
3. `src/components/organisms/filters/FilterPanel.tsx` - Updated for drawer mode

**Acceptance Criteria:**
- [ ] Filter button shows active count
- [ ] Drawer opens from bottom
- [ ] Swipe down closes drawer
- [ ] All filters accessible
- [ ] Apply button closes drawer

### Phase 8: Mobile Header & Navigation
**Risk:** Medium
**Dependencies:** Phase 1

**Deliverables:**
1. `src/components/organisms/shared/MobileHeader.tsx` - NEW
2. `src/components/organisms/shared/NavigationDrawer.tsx` - NEW
3. `src/components/organisms/shared/Header.tsx` - Updated to be responsive

**Acceptance Criteria:**
- [ ] Hamburger menu works on mobile
- [ ] Navigation drawer contains all links
- [ ] Convention Signing hidden on xs
- [ ] Search overlay works
- [ ] Desktop header unchanged

### Phase 9: Page Layout Updates
**Risk:** Medium
**Dependencies:** Phase 2, 5, 7, 8

**Deliverables:**
1. `src/components/pages/AllSetsPage.tsx` - Responsive layout
2. `src/components/pages/SetPage.tsx` - Responsive layout
3. `src/components/pages/CardAllPrintingsPage.tsx` - Responsive layout
4. `src/components/pages/ArtistCardsPage.tsx` - Responsive layout
5. `src/components/templates/pages/BrowseTemplate.tsx` - Responsive props

**Acceptance Criteria:**
- [ ] All pages render correctly on mobile
- [ ] Sticky filter bar on mobile
- [ ] Group headers simplified on mobile
- [ ] No horizontal scroll
- [ ] Touch navigation works

### Phase 10: Image Loading Strategy
**Risk:** Low
**Dependencies:** Phase 3

**Deliverables:**
1. `src/components/organisms/Cards/CardImageDisplay.tsx` - Size mapping updated

**Acceptance Criteria:**
- [ ] Correct image size loaded per card size
- [ ] Lazy loading works on mobile
- [ ] No unnecessary large images on mobile

### Phase 11: Touch & Platform Polish
**Risk:** Low
**Dependencies:** All previous phases

**Deliverables:**
1. CSS updates for reduced motion
2. iOS safe area handling
3. Touch target verification
4. Dead code removal

**Acceptance Criteria:**
- [ ] All touch targets >= 44px
- [ ] Safe areas respected on iOS
- [ ] Reduced motion respected
- [ ] Dead code removed
- [ ] No console warnings/errors

---

## 13. Risk Assessment

**Deployment Approach:** Direct changes - no feature flags, no A/B testing, no gradual rollout.

### High Risk Items

| Risk | Impact | Mitigation |
|------|--------|------------|
| CardDetailsModal rewrite | Core user flow affected | Thorough testing before merge |
| Grid system changes | All pages affected | Test all breakpoints thoroughly |
| Navigation changes | App-wide impact | Extensive testing across devices |

### Medium Risk Items

| Risk | Impact | Mitigation |
|------|--------|------------|
| MtgCard changes | Core display component | Test all card display scenarios |
| Filter drawer | User workflow change | Clear affordances, animation feedback |
| Set card sizing | AllSetsPage affected | Test at all breakpoints |

### Low Risk Items

| Risk | Impact | Mitigation |
|------|--------|------------|
| New hooks | Additive only | Unit tests |
| Theme extensions | Additive only | Backward compatible |
| CSS variables | Additive only | Fallback values |
| Image sizing | Performance improvement | Fallback to normal size |

---

## 14. Testing Strategy

### 14.1 Unit Tests

**New Hooks:**
- `useCardSizePreference`: Test preference persistence, width calculations
- `useMobileLayout`: Test breakpoint detection, drawer state

**Components:**
- CardOverlay: Test variant rendering, expand/collapse
- MtgCard: Test size prop, displayMode prop
- FilterDrawer: Test open/close, filter count

### 14.2 Integration Tests

**Page Tests:**
- AllSetsPage: Verify grid at breakpoints, filter drawer
- SetPage: Verify card grouping, filter bar
- CardDetailsModal: Verify modal vs sheet switching

### 14.3 Visual Regression Tests

**Breakpoint Screenshots:**
- 375px (iPhone SE)
- 414px (iPhone Plus)
- 768px (iPad portrait)
- 1024px (iPad landscape)
- 1440px (Desktop)

**Components to Capture:**
- MtgCard at each size
- MtgSetCard at each size
- CardOverlay variants
- CardDetailsSheet
- FilterDrawer

### 14.4 Device Testing Matrix

| Device | Browser | Priority |
|--------|---------|----------|
| iPhone SE | Safari | High |
| iPhone 14 | Safari | High |
| iPhone 14 Pro Max | Safari | Medium |
| iPad | Safari | High |
| Pixel 6 | Chrome | High |
| Samsung Galaxy | Chrome | Medium |
| Desktop | Chrome | High |
| Desktop | Firefox | Medium |
| Desktop | Safari | Medium |

### 14.5 Accessibility Testing

- Touch targets: Verify 44px minimum
- Focus management: Test drawer focus trap
- Screen reader: Test card announcements
- Reduced motion: Verify animation disabled

---

## 15. Appendix: File Inventory

### Files to Create

| File | Purpose | Phase |
|------|---------|-------|
| `src/hooks/useCardSizePreference.ts` | Card size preference management | 1 |
| `src/hooks/useMobileLayout.ts` | Mobile layout state management | 1 |
| `src/components/organisms/Cards/CardDetailsSheet.tsx` | Mobile card details | 6 |
| `src/components/molecules/shared/FilterDrawer.tsx` | Mobile filter drawer | 7 |
| `src/components/molecules/shared/MobileFilterButton.tsx` | Filter trigger button | 7 |
| `src/components/organisms/shared/MobileHeader.tsx` | Mobile app header | 8 |
| `src/components/organisms/shared/NavigationDrawer.tsx` | Mobile navigation | 8 |

### Files to Modify

| File | Changes | Phase |
|------|---------|-------|
| `src/theme/index.ts` | Add mobile tokens | 1 |
| `src/index.css` | Add CSS variables | 1 |
| `src/components/molecules/layouts/ResponsiveGrid.tsx` | Responsive minItemWidth | 2 |
| `src/components/organisms/Cards/CardGrid.tsx` | Use responsive grid | 2 |
| `src/components/organisms/Cards/MtgCard.tsx` | Add responsive props | 3 |
| `src/hooks/useMtgCardStyles.ts` | Responsive sizing | 3 |
| `src/hooks/useMtgCardInteractions.ts` | Tap behavior | 3 |
| `src/components/molecules/Cards/CardOverlay.tsx` | Add variants | 4 |
| `src/components/molecules/Sets/MtgSetCard.tsx` | Responsive sizing | 5 |
| `src/components/organisms/Cards/CardDetailsModal.tsx` | Switch by breakpoint | 6 |
| `src/components/organisms/filters/FilterPanel.tsx` | Drawer mode | 7 |
| `src/components/organisms/shared/Header.tsx` | Responsive switching | 8 |
| `src/components/pages/AllSetsPage.tsx` | Responsive layout | 9 |
| `src/components/pages/SetPage.tsx` | Responsive layout | 9 |
| `src/components/pages/CardAllPrintingsPage.tsx` | Responsive layout | 9 |
| `src/components/pages/ArtistCardsPage.tsx` | Responsive layout | 9 |
| `src/components/templates/pages/BrowseTemplate.tsx` | Responsive props | 9 |
| `src/components/organisms/Cards/CardImageDisplay.tsx` | Size mapping | 10 |

### Files to Delete

| File | Reason | Phase |
|------|--------|-------|
| `src/components/organisms/Cards/CardDisplayResponsive.tsx` | Unused dead code | 11 |
| `src/components/organisms/Cards/CardCompact.tsx` | Unused (verify first) | 11 |

---

## Document Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.1 | 2026-01-05 | Revised mobile card layout: collection count + name in bottom overlay (single visual zone); simplified collection display to raw count only with ⭕ for 0 collected; clarified offline mode decision (not planned - requires network); removed card name header at top |
| 1.0 | 2026-01-04 | Initial comprehensive analysis and plan |
