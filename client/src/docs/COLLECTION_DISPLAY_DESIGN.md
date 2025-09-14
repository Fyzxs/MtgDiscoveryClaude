# Collection Display Design Specification

## Overview
This document defines the UX design for displaying user collection information on MTG cards. The design uses a progressive disclosure approach with three interaction states: Default → Hover → Click.

## Design Goals
- **Concise**: Show total count prominently, details on demand
- **Scannable**: Consistent icon ordering and visual hierarchy
- **Informative**: Full breakdown available without overwhelming the UI
- **Responsive**: Works on both desktop (hover) and mobile (click)

## Icon System

### Finish Types (Primary Grouping)
- 📄 **Nonfoil** - Regular, matte finish cards
- ✨ **Foil** - Foil finish cards  
- 🌟 **Etched** - Etched foil finish cards

### Special Types (Secondary Attributes)
- 🏆 **Slabbed** - Graded/certified cards
- 📜 **Artist Proof** - Artist proof cards
- ✍️ **Signed** - Artist signed cards
- 🎨 **Altered** - Altered art cards

### Display Order
**Always maintain this consistent ordering:**
1. **Finishes**: 📄 → ✨ → 🌟
2. **Specials**: 🏆 → 📜 → ✍️ → 🎨

## Interaction States

### Default State
Shows total count and present indicators only.

**Format**: `[total] finish_indicators | special_indicators`

**Rules**:
- Only show finish indicators if multiple finish types exist
- Separator `|` only appears when special types are present
- Hide indicators that aren't present in the collection

**Examples**:
```
[10]                           // Single finish, no specials
[47] 📄✨🌟                    // Multiple finishes, no specials  
[15] | 🏆✍️🎨                  // Single finish, has specials
[103] 📄✨🌟 | 🏆📜✍️🎨        // Multiple finishes and specials
```

### Hover State (Desktop)
Shows counts for all present indicators.

**Format**: `[total] finish_icon count finish_icon count | special_icon count special_icon count`

**Examples**:
```
[47] 📄32 ✨10 🌟5
[103] 📄18 ✨33 🌟52 | 🏆5 📜14 ✍️24 🎨4
```

### Click State (Detailed Breakdown)
Shows complete breakdown with sub-categories.

**Format**:
```
📄 Nonfoil: total (unmodified_count, special_icon count, special_icon count)
✨ Foil: total (unmodified_count, special_icon count, special_icon count)
🌟 Etched: total (unmodified_count, special_icon count, special_icon count)
────────────────────────────────────────────────
🏆 Slabbed
  - Grade: finish_icon special_icon count, finish_icon special_icon count
```

**Example**:
```
📄 Nonfoil: 18 (10, ✍️ 2, 📜 1, 🎨 5)
✨ Foil: 33 (11, ✍️ 21, 📜 11)  
🌟 Etched: 52 (1, ✍️ 1, 📜 2, 🎨 3)
────────────────────────────────────────────────
🏆 Slabbed
  - PSA 9: ✨✍️ 1
  - BGS 6: 📄 2
  - PSA 10: 🌟 1, 📄🎨 1
```

## Data Structure

### Expected Backend Format
```json
{
  "collected": [
    {
      "finish": "nonfoil",
      "special": "none", 
      "count": 10
    },
    {
      "finish": "nonfoil",
      "special": "none", 
      "count": 10
    },
    {
      "finish": "foil",
      "special": "signed",
      "count": 21,
      "grade": "PSA 9",
    }
  ]
}
```

### Display Logic Rules

1. **Finish Indicators**:
   - Show only if multiple finish types exist in collection
   - Always in order: 📄 → ✨ → 🌟

2. **Special Indicators**: 
   - Always show if any special types exist
   - Always in order: 🏆 → 📜 → ✍️ → 🎨
   - Separated from finish indicators with ` | `

3. **Counts in Hover**:
   - Show count immediately after each indicator
   - Space between icon and count: `📄18`

4. **Detailed Breakdown**:
   - Unmodified cards show count with no prefix
   - Special cards show as: `icon space count` (e.g., `✍️ 2`)
   - Slabbed cards show as: `finish_icon special_icon space count`

## Component Implementation Notes

### Component Name
`CollectionSummary` - Located at `components/molecules/Cards/CollectionSummary.tsx`

### Props Interface
```typescript
interface CollectionSummaryProps {
  collection: CollectionData;
  size?: 'small' | 'medium' | 'large';
}
```

### Responsive Behavior
- **Desktop**: Hover shows counts, click shows detailed popover
- **Mobile**: Click shows counts inline, second click shows detailed popover
- **Accessibility**: Proper ARIA labels and keyboard navigation

### Styling Notes
- Use Material-UI Popover for detailed breakdown
- Consistent spacing with existing card components
- Proper color contrast for accessibility
- Icons should scale with text size

## Future Considerations
- **Localization**: Icon meanings may need tooltips in other languages
- **Customization**: Users may want to hide certain special types
- **Sorting**: Collection view might want to sort by total count
- **Filtering**: Future filtering by finish/special types
- **Animation**: Subtle transitions between states for better UX

---
**Created**: 2025-01-15  
**Last Updated**: 2025-01-15