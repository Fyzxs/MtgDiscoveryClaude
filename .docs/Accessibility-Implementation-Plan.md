# Accessibility Implementation Plan (WCAG 2.2)

## Executive Summary

This document provides a comprehensive accessibility audit and implementation plan for the MtgDiscovery frontend application. The analysis is based on WCAG 2.2 standards at AA conformance level.

**Current State**: The application has a solid foundation with many accessibility features already implemented, including skip navigation, ARIA attributes, keyboard navigation, and focus management. However, several gaps exist that need to be addressed.

**Overall Assessment**: Good baseline with targeted improvements needed.

---

## Current Accessibility State

### Existing Implementations (Positive Findings)

| Feature | Location | Status |
|---------|----------|--------|
| Skip Navigation | `/client/src/components/atoms/shared/SkipNavigation.tsx` | Implemented with three skip links |
| ARIA Labels | Header, FilterPanel, MtgCard, MultiSelectDropdown | Comprehensive coverage |
| Keyboard Navigation | Header, Card links, Search inputs | Basic implementation present |
| Focus Indicators | Multiple components using `&:focus-visible` | Consistent pattern |
| Semantic HTML | Layout uses `role="main"`, `role="banner"`, `role="navigation"` | Proper landmarks |
| Screen Reader Hidden Elements | MtgCard has `aria-hidden` for decorative content | Implemented |
| Alt Text | CardImageDisplay, SetIconDisplay | Present but could be enhanced |
| Live Regions | MultiSelectDropdown uses `aria-live="polite"` | Limited usage |

### Key Files with Accessibility Features

- `/client/src/components/atoms/shared/SkipNavigation.tsx` - Skip navigation component
- `/client/src/components/organisms/shared/Header.tsx` - Main navigation with ARIA
- `/client/src/components/templates/Layout.tsx` - Page structure with landmarks
- `/client/src/components/organisms/Cards/MtgCard.tsx` - Card component with aria-describedby
- `/client/src/components/molecules/shared/MultiSelectDropdown.tsx` - Accessible dropdown

---

## WCAG 2.2 Audit by Principle

### 1. PERCEIVABLE

#### 1.1 Text Alternatives (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 1.1.1 | `CardImageDisplay.tsx:168` | `alt={card.name}` | Alt text is card name only | Enhance with card type and rarity | Medium |
| 1.1.1 | `SetIconDisplay.tsx:37` | `alt={setName} icon` | Good | None | N/A |
| 1.1.1 | `ExternalLinkIcon.tsx:79` | `alt={type} logo` | Good | None | N/A |
| 1.1.1 | `CardImage.tsx:116` | `alt={cardName || 'Magic card'}` | Generic fallback | Provide more descriptive fallback | Low |

**Recommended Fix for CardImageDisplay:**
```typescript
// Current
alt={card.name}

// Enhanced
alt={`${card.name}${card.typeLine ? ` - ${card.typeLine}` : ''}${card.rarity ? `, ${card.rarity}` : ''}`}
```

#### 1.3 Adaptable (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 1.3.1 | `Layout.tsx` | Uses semantic landmarks | Good | None | N/A |
| 1.3.1 | `SetPage.tsx` | No heading hierarchy | Missing `<h1>` for page title | Add proper heading structure | High |
| 1.3.2 | Multiple | Reading order matches visual | Good | None | N/A |
| 1.3.5 | `SearchInput.tsx` | Uses form element | Missing `autocomplete` attribute | Add `autocomplete="off"` or appropriate value | Medium |

**Recommended Fix for Heading Hierarchy:**
```typescript
// Each page should have exactly one <h1>
// SetPageHeader should use <h1> for set name
// Card names in grids should use <h2> or <h3>
```

#### 1.4 Distinguishable (Level AA)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 1.4.1 | `theme/index.ts` | Rarity colors used | Color alone conveys meaning for rarity | Add text/pattern indicators | High |
| 1.4.3 | `theme/index.ts` | Dark theme | Some color combinations need verification | Verify contrast ratios | High |
| 1.4.4 | Theme | Font sizes use rem | Good | None | N/A |
| 1.4.10 | Multiple | Responsive design | Good | None | N/A |
| 1.4.11 | Focus styles | Uses outline | Verify 3:1 contrast ratio | Test and adjust | Medium |

**Color Contrast Analysis:**

The theme defines these key color combinations that need verification:

| Background | Foreground | Use Case | Estimated Ratio | Status |
|------------|------------|----------|-----------------|--------|
| `#0a0e1a` | `#ffffff` | Default text | ~18:1 | Pass |
| `#0a0e1a` | `rgba(255,255,255,0.7)` | Secondary text | ~11:1 | Pass |
| `#0a0e1a` | `#424242` (common rarity) | Rarity badge | ~2.2:1 | **FAIL** |
| `#0a0e1a` | `#9e9e9e` (uncommon) | Rarity badge | ~6.5:1 | Pass |
| `#1a1f2e` | `rgba(255,255,255,0.7)` | Paper secondary | ~9:1 | Pass |

**Recommended Fix for Rarity Colors:**
```typescript
// theme/index.ts
rarity: {
  common: '#757575',      // Increased from #424242 for better contrast
  uncommon: '#9e9e9e',
  rare: '#ffc107',
  mythic: '#ff6f00',
  // ... others
},
```

---

### 2. OPERABLE

#### 2.1 Keyboard Accessible (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 2.1.1 | `MtgCard.tsx:54` | `tabIndex={-1}` | Cards not in tab order individually | Add card grid keyboard navigation | High |
| 2.1.1 | `CardGrid.tsx:140-141` | Has `onKeyDown` handler | Grid-level keyboard nav exists | Enhance with arrow key support | Medium |
| 2.1.1 | `CardCompact.tsx:142` | `tabIndex: onClick ? 0 : undefined` | Good pattern | None | N/A |
| 2.1.2 | `CardImageDisplay.tsx:110` | Animation timeout 300ms | No way to skip animation | Add `prefers-reduced-motion` support | Medium |

**Recommended Fix for Keyboard Navigation:**
```typescript
// CardGrid.tsx - Enhance keyboard navigation
const handleKeyDown = (event: React.KeyboardEvent) => {
  switch (event.key) {
    case 'ArrowRight':
      // Move to next card
      break;
    case 'ArrowLeft':
      // Move to previous card
      break;
    case 'ArrowDown':
      // Move to next row
      break;
    case 'ArrowUp':
      // Move to previous row
      break;
    case 'Home':
      // Move to first card
      break;
    case 'End':
      // Move to last card
      break;
  }
};
```

#### 2.4 Navigable (Level A/AA)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 2.4.1 | `SkipNavigation.tsx` | Three skip links implemented | Good | None | N/A |
| 2.4.2 | `index.html` | Needs verification | Likely missing `<title>` per page | Add dynamic page titles | High |
| 2.4.3 | `Header.tsx` | Logo is first in tab order | Good | None | N/A |
| 2.4.4 | `Header.tsx:119-188` | Links have aria-label | Good | None | N/A |
| 2.4.6 | Multiple pages | No headings visible | Add visible headings to pages | High |
| 2.4.7 | Multiple | Focus visible styles exist | Good | None | N/A |

**Recommended Fix for Page Titles:**
```typescript
// Use react-helmet-async or similar
import { Helmet } from 'react-helmet-async';

// In SetPage.tsx
<Helmet>
  <title>{setName} - MtgDiscovery</title>
</Helmet>
```

#### 2.5 Input Modalities (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 2.5.1 | Touch handlers | Long press implemented | May need alternative | Provide context menu alternative | Low |
| 2.5.2 | `CardCompact.tsx` | Gesture handlers | Gestures are enhancements only | Good | N/A |
| 2.5.4 | Global shortcuts | No conflicts | Good | None | N/A |

---

### 3. UNDERSTANDABLE

#### 3.1 Readable (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 3.1.1 | `index.html` | Needs `lang="en"` | Verify `lang` attribute | Add if missing | Critical |
| 3.1.2 | Card text | English content | N/A for now | Future i18n consideration | Low |

#### 3.2 Predictable (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 3.2.1 | `SearchInput.tsx` | Focus changes state | Form only submits on Enter | Good | N/A |
| 3.2.2 | Filters | Changes apply automatically | Add explicit "Apply" option or announce changes | Medium |
| 3.2.3 | `Header.tsx` | Navigation consistent | Good | None | N/A |

**Recommended Fix for Filter Changes:**
```typescript
// Add announcement for filter changes
const [announcement, setAnnouncement] = useState('');

useEffect(() => {
  if (filteredCards.length !== previousCount) {
    setAnnouncement(`Showing ${filteredCards.length} cards`);
  }
}, [filteredCards.length]);

<div role="status" aria-live="polite" className="sr-only">
  {announcement}
</div>
```

#### 3.3 Input Assistance (Level A/AA)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 3.3.1 | `ErrorAlert.tsx` | Generic error display | Missing specific field identification | Enhance error messages | Medium |
| 3.3.2 | `SearchInput.tsx:107` | Label conditionally shown | Always associate label with input | Use `InputLabel` component properly | Medium |
| 3.3.3 | Form inputs | No error suggestions | Add suggestions for common errors | Low |

**Recommended Fix for Form Labels:**
```typescript
// SearchInput.tsx - Always have associated label
<InputLabel htmlFor={inputId} shrink={showLabel}>
  {label}
</InputLabel>
<TextField
  id={inputId}
  aria-labelledby={`${inputId}-label`}
  // ...
/>
```

---

### 4. ROBUST

#### 4.1 Compatible (Level A)

| Criterion | File | Current State | Issue | Fix Required | Priority |
|-----------|------|---------------|-------|--------------|----------|
| 4.1.2 | `MtgCard.tsx` | Has role="button" | Good | None | N/A |
| 4.1.2 | `Header.tsx:98` | Uses role="menubar" | Good | None | N/A |
| 4.1.2 | `MultiSelectDropdown.tsx` | Comprehensive ARIA | Good | None | N/A |
| 4.1.3 | Status messages | Limited aria-live usage | Add more live regions | High |

**Recommended Fix for Status Messages:**
```typescript
// Create a reusable announcement component
interface AnnouncementProviderProps {
  children: React.ReactNode;
}

export const AnnouncementProvider: React.FC<AnnouncementProviderProps> = ({ children }) => {
  const [message, setMessage] = useState('');

  return (
    <AnnouncementContext.Provider value={{ announce: setMessage }}>
      {children}
      <div
        role="status"
        aria-live="polite"
        aria-atomic="true"
        style={srOnly}
      >
        {message}
      </div>
    </AnnouncementContext.Provider>
  );
};
```

---

## Component-Level Audit

### Header/Navigation (`Header.tsx`)

**Status**: Good with minor improvements needed

| Issue | Line | Fix Required | Priority |
|-------|------|--------------|----------|
| Menu items missing aria-current | 118-191 | Add aria-current="page" for active route | Medium |
| Search menu role="menu" | 154 | Correct - no changes needed | N/A |

### Forms and Inputs (`SearchInput.tsx`, `MultiSelectDropdown.tsx`)

**Status**: Good foundation

| Issue | File | Fix Required | Priority |
|-------|------|--------------|----------|
| No autocomplete attribute | SearchInput.tsx | Add autocomplete="off" | Medium |
| Clear button missing visible text | SearchInput.tsx:85-92 | Add visually hidden text | Low |

### Modals/Dialogs (`CardDetailsModal.tsx`)

**Status**: Needs improvements

| Issue | Line | Fix Required | Priority |
|-------|------|--------------|----------|
| Missing aria-labelledby | 124-131 | Add modal title association | High |
| Navigation buttons missing labels | 145-158 | Add descriptive aria-labels | High |
| Focus trap verification needed | N/A | Verify MUI Dialog handles focus | Medium |

**Recommended Fix:**
```typescript
// CardDetailsModal.tsx
<ModalContainer
  open={open}
  onClose={onClose}
  aria-labelledby="card-details-title"
  aria-describedby="card-details-description"
>
  <Typography id="card-details-title" variant="h2" component="h2">
    {card.name}
  </Typography>

  <IconButton
    onClick={onPrevious}
    disabled={!hasPrevious}
    aria-label="Go to previous card"
  >
    <NavigateBeforeIcon />
  </IconButton>
```

### Card Grid (`CardGrid.tsx`, `MtgCard.tsx`)

**Status**: Needs keyboard navigation enhancement

| Issue | File | Fix Required | Priority |
|-------|------|--------------|----------|
| Individual cards not focusable | MtgCard.tsx:54 | Consider roving tabindex pattern | High |
| No announcement of grid position | CardGrid.tsx | Add aria-posinset, aria-setsize | Medium |
| Selected state not announced | MtgCard.tsx | Add aria-pressed or aria-selected | Medium |

**Recommended Fix:**
```typescript
// Implement roving tabindex for card grid
const CardGrid = ({ cards }) => {
  const [focusedIndex, setFocusedIndex] = useState(0);

  return (
    <div role="grid" aria-label="Card grid">
      {cards.map((card, index) => (
        <MtgCard
          key={card.id}
          card={card}
          tabIndex={index === focusedIndex ? 0 : -1}
          aria-posinset={index + 1}
          aria-setsize={cards.length}
          onFocus={() => setFocusedIndex(index)}
        />
      ))}
    </div>
  );
};
```

### Error Boundaries (`ErrorBoundaries.tsx`, `ErrorAlert.tsx`)

**Status**: Good baseline

| Issue | File | Fix Required | Priority |
|-------|------|--------------|----------|
| Error not announced | ErrorBoundaries.tsx | Add role="alert" to error fallback | High |
| No recovery instructions | ErrorAlert.tsx | Add actionable recovery steps | Medium |

### Loading States

**Status**: Needs aria-live regions

| Issue | File | Fix Required | Priority |
|-------|------|--------------|----------|
| Loading not announced | Multiple | Add aria-busy and aria-live | Medium |
| Skeleton lacks context | Skeleton usage | Add aria-label to skeleton containers | Low |

---

## Implementation Plan

### Phase 1: Critical (Week 1-2)

**Effort: ~16-24 hours**

1. **Add `lang` attribute verification** (1 hour)
   - File: `/client/index.html`
   - Verify `<html lang="en">` exists

2. **Fix color contrast for common rarity** (2 hours)
   - File: `/client/src/theme/index.ts`
   - Change `common: '#424242'` to `common: '#757575'`

3. **Add dynamic page titles** (4 hours)
   - Install `react-helmet-async`
   - Add `<Helmet>` to each page component:
     - `SetPage.tsx`
     - `AllSetsPage.tsx`
     - `CardSearchPage.tsx`
     - `ArtistSearchPage.tsx`
     - `ArtistCardsPage.tsx`
     - `CardAllPrintingsPage.tsx`

4. **Fix modal accessibility** (4 hours)
   - File: `/client/src/components/organisms/Cards/CardDetailsModal.tsx`
   - Add `aria-labelledby`, `aria-describedby`
   - Add descriptive labels to navigation buttons

5. **Add error announcements** (4 hours)
   - File: `/client/src/components/utils/ErrorBoundaries.tsx`
   - Add `role="alert"` to error fallback components

6. **Create status announcement system** (6 hours)
   - Create `/client/src/contexts/AnnouncementContext.tsx`
   - Integrate with filter changes, loading states, and results count

### Phase 2: High Priority (Week 3-4)

**Effort: ~20-28 hours**

1. **Enhance card keyboard navigation** (8 hours)
   - File: `/client/src/components/organisms/Cards/CardGrid.tsx`
   - Implement roving tabindex pattern
   - Add arrow key navigation between cards
   - Add Home/End key support

2. **Add prefers-reduced-motion support** (4 hours)
   - Files: Multiple components with transitions
   - Add CSS media query checks
   - Respect user's motion preferences

3. **Fix heading hierarchy** (4 hours)
   - Audit all pages for heading structure
   - Ensure single `<h1>` per page
   - Fix heading level progression

4. **Enhance alt text** (4 hours)
   - File: `/client/src/components/organisms/Cards/CardImageDisplay.tsx`
   - Include card type and rarity in alt text

5. **Add form autocomplete attributes** (2 hours)
   - File: `/client/src/components/molecules/shared/SearchInput.tsx`
   - Add appropriate autocomplete values

6. **Fix filter change announcements** (4 hours)
   - Add announcements when filters change
   - Announce results count updates

### Phase 3: Medium Priority (Week 5-6)

**Effort: ~16-20 hours**

1. **Add aria-current for navigation** (2 hours)
   - File: `/client/src/components/organisms/shared/Header.tsx`
   - Track active route and apply aria-current

2. **Enhance loading state accessibility** (4 hours)
   - Add `aria-busy` to loading containers
   - Announce loading start and completion

3. **Improve form label associations** (4 hours)
   - Ensure all form inputs have proper label associations
   - Add visible labels where missing

4. **Add grid position information** (4 hours)
   - Add `aria-posinset` and `aria-setsize` to card items
   - Announce position in grid

5. **Verify focus trap in modals** (4 hours)
   - Test modal focus management
   - Ensure focus returns to trigger on close

### Phase 4: Low Priority (Week 7-8)

**Effort: ~8-12 hours**

1. **Enhance error messages** (4 hours)
   - Add recovery suggestions
   - Improve error specificity

2. **Add skeleton accessibility** (2 hours)
   - Add aria-labels to skeleton containers
   - Improve loading perception

3. **Long press alternatives** (4 hours)
   - Add context menu or alternative interaction
   - Document keyboard alternatives

---

## Testing Approach

### Automated Testing

1. **axe-core Integration**
   ```bash
   npm install --save-dev @axe-core/react
   ```

   ```typescript
   // In development mode
   import React from 'react';
   import ReactDOM from 'react-dom';

   if (process.env.NODE_ENV !== 'production') {
     import('@axe-core/react').then(axe => {
       axe.default(React, ReactDOM, 1000);
     });
   }
   ```

2. **ESLint Plugin**
   ```bash
   npm install --save-dev eslint-plugin-jsx-a11y
   ```

   Add to `.eslintrc`:
   ```json
   {
     "extends": ["plugin:jsx-a11y/recommended"]
   }
   ```

3. **Jest Tests with jest-axe**
   ```typescript
   import { axe, toHaveNoViolations } from 'jest-axe';

   expect.extend(toHaveNoViolations);

   test('Header is accessible', async () => {
     const { container } = render(<Header />);
     const results = await axe(container);
     expect(results).toHaveNoViolations();
   });
   ```

### Manual Testing Checklist

- [ ] Keyboard-only navigation through all pages
- [ ] Screen reader testing (NVDA, VoiceOver)
- [ ] Color contrast verification (WebAIM Contrast Checker)
- [ ] Browser zoom to 200% functionality
- [ ] High contrast mode support
- [ ] Reduced motion preference respect
- [ ] Focus indicator visibility
- [ ] Form error handling
- [ ] Modal focus management

### Browser/AT Testing Matrix

| Screen Reader | Browser | Platform |
|---------------|---------|----------|
| NVDA | Chrome, Firefox | Windows |
| VoiceOver | Safari | macOS |
| VoiceOver | Safari | iOS |
| TalkBack | Chrome | Android |

---

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| axe-core violations | 0 critical, 0 serious | Automated testing |
| Lighthouse Accessibility Score | >= 90 | Chrome DevTools |
| Keyboard navigation | 100% of features accessible | Manual testing |
| Color contrast | All text AA compliant | Contrast checker |
| Screen reader compatibility | All content accessible | Manual testing |

---

## Resources

- [WCAG 2.2 Quick Reference](https://www.w3.org/WAI/WCAG22/quickref/)
- [Material-UI Accessibility Guide](https://mui.com/material-ui/guides/accessibility/)
- [WAI-ARIA Authoring Practices](https://www.w3.org/WAI/ARIA/apg/)
- [axe-core Documentation](https://github.com/dequelabs/axe-core)
- [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/)

---

## Appendix A: File Index

Files requiring modifications:

| File | Phase | Changes |
|------|-------|---------|
| `/client/index.html` | 1 | Verify lang attribute |
| `/client/src/theme/index.ts` | 1 | Adjust rarity.common color |
| `/client/src/components/organisms/Cards/CardDetailsModal.tsx` | 1 | Add ARIA attributes |
| `/client/src/components/utils/ErrorBoundaries.tsx` | 1 | Add role="alert" |
| `/client/src/components/organisms/Cards/CardGrid.tsx` | 2 | Keyboard navigation |
| `/client/src/components/organisms/Cards/CardImageDisplay.tsx` | 2 | Enhanced alt text |
| `/client/src/components/molecules/shared/SearchInput.tsx` | 2, 3 | autocomplete, labels |
| `/client/src/components/organisms/shared/Header.tsx` | 3 | aria-current |
| `/client/src/components/pages/SetPage.tsx` | 1 | Page title |
| `/client/src/components/pages/AllSetsPage.tsx` | 1 | Page title |
| `/client/src/components/pages/CardSearchPage.tsx` | 1 | Page title |
| `/client/src/components/pages/ArtistSearchPage.tsx` | 1 | Page title |
| `/client/src/components/pages/ArtistCardsPage.tsx` | 1 | Page title |

New files to create:

| File | Phase | Purpose |
|------|-------|---------|
| `/client/src/contexts/AnnouncementContext.tsx` | 1 | Screen reader announcements |
| `/client/src/hooks/useAnnounce.ts` | 1 | Hook for announcements |
| `/client/src/hooks/useReducedMotion.ts` | 2 | Motion preference detection |
