# Set Grouping Feature - Design Document

## Overview

Group child sets under their parent sets (like Scryfall does at https://scryfall.com/sets), with a mobile-first approach. This saves screen space while maintaining discoverability.

## User Requirements

| Requirement | Decision |
|-------------|----------|
| Data source | Backend update to expose `parentSetCode` in GraphQL API |
| Expand/collapse UX | Tap on child count badge ("+5 sets") |
| Default view | Grouped on mobile, flat grid on desktop |
| Mobile parent sets | Icon + name + set code + collection % (no card count) |
| Mobile child sets | Icon + name only (indented) |

---

## Visual Design

### Mobile Grouped View (Default)

```
┌──────────────────────────────────────────┐
│  ┌────────────────┐  ┌────────────────┐  │
│  │     [icon]     │  │     [icon]     │  │
│  │    Set Name    │  │    Set Name    │  │
│  │  [DMU]   75% ✓ │  │  [2X2]   42% ✓ │  │ ← Code + collection on same line
│  │        +3 sets │  └────────────────┘  │ ← Child count badge
│  └────────────────┘                      │   (NO card count)
│     ┌─ [icon] Promo Set Name            │ ← Expanded children
│     ├─ [icon] Token Set Name            │   (indented, icon + name only)
│     └─ [icon] Art Series Name           │
│                                          │
│  ┌────────────────┐  ┌────────────────┐  │
│  │     [icon]     │  │     [icon]     │  │
│  │  Another Set   │  │  Another Set   │  │
│  │  [BRO]  100% ✓ │  │  [ONE]    0%   │  │
│  └────────────────┘  └────────────────┘  │
└──────────────────────────────────────────┘
```

**Parent Card Content (Mobile) - SetCardCompact:**
- Set icon (standard size)
- Set name
- Bottom row: set code badge + collection % (same line)
- Child count badge ("+N sets") if has children
- **NO card count** - saves vertical space

### Desktop Flat View (Default)

```
┌────────────────────────────────────────────────────────────────────┐
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐  │
│  │ [icon]  │  │ [icon]  │  │ [icon]  │  │ [icon]  │  │ [icon]  │  │
│  │ Set 1   │  │ Set 2   │  │ Set 3   │  │ Set 4   │  │ Set 5   │  │
│  │ Core    │  │ Promo   │  │ Tokens  │  │ Core    │  │ Promo   │  │
│  │ 250     │  │ 50      │  │ 20      │  │ 280     │  │ 45      │  │
│  └─────────┘  └─────────┘  └─────────┘  └─────────┘  └─────────┘  │
│                                                                    │
│  (All sets shown individually in grid, current behavior)           │
└────────────────────────────────────────────────────────────────────┘
```

---

## Component Architecture

### New Components

```
atoms/Sets/
  ├── ChildSetsBadge.tsx           # "+N sets" chip, tappable
  └── CollectionProgressCompact.tsx # Compact inline collection % for grouped view

molecules/Sets/
  ├── SetCardCompact.tsx      # Compact card: icon, name, [code] + % (for grouped view)
  ├── ChildSetRow.tsx         # Minimal row: icon + name only
  └── SetGroupCard.tsx        # Parent card + collapsible children

hooks/
  └── useSetGrouping.ts       # Grouping logic + expand state
```

### Component Separation (No Flags)

Instead of flag-laden components, use **purpose-built components at every level**.
The more flags an atomic control has, the less atomic it is.

```
Mobile Grouped View                    Desktop Flat View
───────────────────                    ─────────────────
SetCardCompact (NEW)                   MtgSetCard (existing, unchanged)
  ├─ SetIcon (shared atom)               ├─ SetIcon (shared atom)
  ├─ Typography (set name)               ├─ SetName (text)
  ├─ SetCodeBadge (shared atom)          ├─ CardCount
  └─ CollectionProgressCompact (NEW)     ├─ CollectionProgressFull (existing)
                                         └─ InfoBadges (type, digital, etc.)
```

**Atomic Control Philosophy:**
- Each atom has ONE purpose, no variant flags
- `CollectionProgressCompact` - small inline display for grouped view
- `CollectionProgressFull` - full display for desktop cards
- Shared atoms (SetIcon, SetCodeBadge) remain simple and reusable
- Page-level logic decides which molecule/organism to render

Benefits:
- True atomic components with single responsibility
- No conditional flag logic buried in components
- Easier to test and maintain
- Clear mental model for each component's purpose

**Note:** This feature establishes a pattern for ongoing UI refactoring. As more features are built, existing components will be split into screen-size-specific variants (e.g., `CardCompact`, `CardFull`) rather than adding flags to existing components. This creates a cleaner codebase with purpose-built UIs for each breakpoint.

### Component Details

#### ChildSetsBadge
```
┌─────────────┐
│  +3 sets    │  ← Collapsed state
└─────────────┘

┌─────────────┐
│  Hide 3     │  ← Expanded state
└─────────────┘

- Small chip (20px height)
- Primary color when collapsed
- Outlined when expanded
- 44px touch target (via padding)
```

#### ChildSetRow
```
     ┌──────────────────────────────────────┐
     │ [24px icon]  Promo Set Name          │
     └──────────────────────────────────────┘

- Indented (margin-left: 24px)
- 44px min height
- Icon + name only
- Tappable → navigates to set
```

#### SetGroupCard
```
┌─────────────────────────────────────┐
│  ┌─────────────────────────────┐    │
│  │     Standard MtgSetCard     │    │
│  │         (parent)            │    │
│  │                     +3 sets │←───┼── Badge overlay
│  └─────────────────────────────┘    │
│                                     │
│  ┌─ ChildSetRow (child 1) ─────┐    │  ← Collapse animation
│  ├─ ChildSetRow (child 2) ─────┤    │
│  └─ ChildSetRow (child 3) ─────┘    │
└─────────────────────────────────────┘
```

---

## Data Flow

### Current (Missing Link)
```
Scryfall API (parent_set_code)
    ↓
ScryfallSetItemExtEntity.Data (dynamic, has parent_set_code)
    ↓
SetItemExtToItrMapper ──────→ ISetItemItrEntity (❌ no parentSetCode)
    ↓
SetItemOutEntity (❌ no parentSetCode)
    ↓
GraphQL Set type (❌ no parentSetCode)
```

### Proposed (Complete Flow)
```
Scryfall API (parent_set_code)
    ↓
ScryfallSetItemExtEntity.Data (dynamic)
    ↓
SetItemExtToItrMapper ──────→ ISetItemItrEntity.ParentSetCode ✅
    ↓
SetItemOutEntity.ParentSetCode ✅
    ↓
GraphQL Set.parentSetCode ✅
    ↓
Frontend MtgSet.parentSetCode ✅
    ↓
useSetGrouping hook (groups by parentSetCode)
    ↓
SetGroupCard / ChildSetRow components
```

---

## Grouping Logic

### useSetGrouping Hook

```typescript
interface SetGroup {
  parent: MtgSet;      // The main set
  children: MtgSet[];  // Sets where parentSetCode === parent.code
}

// Classification:
// - Parent set: parentSetCode is null/undefined
// - Child set: parentSetCode matches a parent's code
// - Orphan: parentSetCode exists but parent not in current list (treat as parent)
```

### Example Data

| Set Code | Name | Parent Code | Classification |
|----------|------|-------------|----------------|
| `2x2` | Double Masters 2022 | `null` | Parent |
| `p2x2` | Double Masters 2022 Promos | `2x2` | Child of 2x2 |
| `t2x2` | Double Masters 2022 Tokens | `2x2` | Child of 2x2 |
| `dmu` | Dominaria United | `null` | Parent |
| `pdmu` | Dominaria United Promos | `dmu` | Child of dmu |

Result: 2 groups (2x2 with 2 children, dmu with 1 child)

---

## Responsive Behavior

| Breakpoint | Default View | Parent Card Content | Child Display |
|------------|--------------|---------------------|---------------|
| xs (<600px) | Grouped | icon, name, set code, collection % | ChildSetRow (icon+name) |
| sm (600-900px) | Grouped | icon, name, set code, collection % | ChildSetRow (icon+name) |
| md (900-1200px) | Grouped | icon, name, set code, collection % | ChildSetRow (icon+name) |
| lg (1200-1536px) | Flat | full info (name, card count, badges) | N/A |
| xl (>1536px) | Flat | full info (name, card count, badges) | N/A |

### SetCardCompact (New Component)
Purpose-built compact card for grouped mobile view:
- Set icon (standard size)
- Set name
- Bottom row: `[CODE]` badge + collection % (same line, space-between)
- No card count, no info badges
- Reuses existing SetIcon atom

**MtgSetCard remains unchanged** - used only in desktop flat view with full info.

---

## Interaction States

### Expand/Collapse Animation

```
Collapsed → Expanded:
1. User taps "+N sets" badge
2. Badge text changes to "Hide N"
3. Children slide down with Collapse animation (150ms)
4. Children fade in

Expanded → Collapsed:
1. User taps "Hide N" badge
2. Badge text changes to "+N sets"
3. Children slide up with Collapse animation (150ms)
```

### Navigation

- Tap parent card body → Navigate to parent set page
- Tap "+N sets" badge → Toggle expand/collapse (no navigation)
- Tap child row → Navigate to child set page

---

## Backend Changes Summary

### Files to Modify (C#)

| File | Change |
|------|--------|
| `ISetItemItrEntity.cs` | Add `string ParentSetCode { get; }` |
| `SetItemItrEntity.cs` | Add `public string ParentSetCode { get; init; }` |
| `SetItemExtToItrMapper.cs` | Extract `data.parent_set_code` |
| `SetItemOutEntity.cs` | Add `public string ParentSetCode { get; init; }` |
| `SetItemOufToOutMapper.cs` | Map `ParentSetCode` |
| `ScryfallSetOutEntityType.cs` | Expose `parentSetCode` in GraphQL |

---

## Frontend Changes Summary

### Files to Modify

| File | Change |
|------|--------|
| `sets.ts` (query) | Add `parentSetCode` to GET_ALL_SETS |
| `set.ts` (type) | Add `parentSetCode?: string` |
| `AllSetsPage.tsx` | Conditional grouped/flat rendering |

### New Files

| File | Purpose |
|------|---------|
| `useSetGrouping.ts` | Hook for grouping logic + state |
| `ChildSetsBadge.tsx` | "+N sets" badge atom |
| `CollectionProgressCompact.tsx` | Compact inline collection % atom |
| `SetCardCompact.tsx` | Compact card molecule (icon, name, code + %) |
| `ChildSetRow.tsx` | Minimal child set row molecule (icon + name) |
| `SetGroupCard.tsx` | Parent + children wrapper molecule with collapse |

### Unchanged Files

| File | Notes |
|------|-------|
| `MtgSetCard.tsx` | No changes - used only in desktop flat view |

---

## Edge Cases

| Case | Handling |
|------|----------|
| Set has no children | Show normally, no badge |
| Orphan child (parent not in list) | Treat as parent (standalone) |
| Filter hides parent but not children | Children become orphans |
| Search matches child only | Show child in results (future: show with parent context?) |
| All children filtered out | Parent shows without badge |

---

## Future Enhancements (Out of Scope)

1. **View toggle on desktop** - Let users enable grouped view on any screen
2. **Persist expansion state** - Remember which groups are expanded
3. **Search with parent context** - When child matches, show parent too
4. **Expand all / Collapse all** - Bulk controls
5. **Sort children** - By release date or card count

---

## Implementation Order

1. **Backend** (can be deployed independently)
   - Add `parentSetCode` through all C# layers
   - Deploy backend

2. **Frontend Data**
   - Update GraphQL query
   - Update TypeScript types
   - Run codegen

3. **Components**
   - Create `useSetGrouping` hook
   - Create `ChildSetsBadge`
   - Create `ChildSetRow`
   - Create `SetGroupCard`

4. **Integration**
   - Update `AllSetsPage` with conditional rendering

5. **Polish**
   - Test animations
   - Test touch targets
   - Test edge cases
