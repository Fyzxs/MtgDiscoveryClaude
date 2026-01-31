# Set Collection Display Design Specification

## Overview
This document defines the UX design for displaying set collection progress and statistics. Unlike individual card collection displays, set displays focus on completion percentage, unique cards collected, and customizable group inclusion criteria.

## Design Goals
- **Clear Progress**: Prominently show completion percentage and progress
- **Flexible Criteria**: Allow users to define what "complete" means per set
- **Detailed Breakdown**: Provide rarity and group details on demand
- **Intuitive Defaults**: Smart defaults for different card group types
- **Visual Consistency**: Maintain design language with existing card displays

## Current Base Design
The existing set display provides an excellent foundation:
- **"259 of 302 set cards"** - Unique cards collected/total
- **"85%"** with progress bar - Completion percentage  
- **"388 cards collected"** - Total physical cards (including duplicates)

## Enhanced Design Specifications

### Default Display (Single Group)
When a set has only one card group, display remains clean:
```
259 of 302 set cards (⚪15 🔵12 🟡14 🟠0)
94% ████████████████████████████████░░
259 cards collected
```

### Default Display (Multiple Groups)
When multiple groups exist and more than one is included:
```
259 of 302 set cards [3/5 groups] (⚪15 🔵12 🟡14 🟠1)
85% ████████████████████████████░░░░  
388 cards collected
```

**Components**:
- **"259 of 302 set cards"** - Unique cards from included groups
- **"[3/5 groups]"** - Included groups / total groups (only shown if >1 group included)
- **"(⚪15 🔵12 🟡14 🟠1)"** - Rarity completion for included groups
- **"85%"** with progress bar - Completion percentage of included groups
- **"388 cards collected"** - Total physical cards from included groups

### Detailed Breakdown (Click to Expand)
```
📊 Collection Groups:
✅ Main Set: 234/250 (94%) [Toggle: Include/Exclude]
✅ Showcase: 15/20 (75%) [Toggle: Include/Exclude]  
✅ Borderless: 10/12 (83%) [Toggle: Include/Exclude]
❌ Serialized: 0/15 (0%) [Toggle: Include/Exclude]
❌ Promos: 5/5 (100%) [Toggle: Include/Exclude]

📊 Rarity Breakdown (Included Groups Only):
⚪ Common: 15/15 complete (47 total cards)
🔵 Uncommon: 12/15 (80%) (38 total cards)  
🟡 Rare: 14/15 (93%) (28 total cards)
🟠 Mythic: 1/8 (12%) (3 total cards)

📄 Regular: 234 cards | ✨ Foil: 89 cards | 🌟 Etched: 65 cards
```

## Card Group Selection System

### Auto-Include Logic

**Rule Priority (Applied in Order)**:

1. **Single Group Sets**: 
   - If only 1 group exists → automatically included (regardless of name)

2. **Multiple Group Sets - Auto-Include**:
   - Group names containing: "In Boosters", "Draft Cards", "Cards", "Main Set"
   - These represent core draftable/booster cards

3. **Multiple Group Sets - Auto-Exclude**:
   - All other groups including: "Serialized", "Promos", "Special Guests", "Showcase", "Borderless", etc.

### Group Selection Interface

**Toggle Controls**:
- Each group shows include/exclude toggle in detailed breakdown
- Real-time recalculation when toggling groups
- Settings persist per-set (not global preference)

**Visual Indicators**:
- ✅ = Included in completion calculation
- ❌ = Excluded from completion calculation
- Progress percentages and counts update immediately

## Rarity Icon System

### Rarity Types
- ⚪ **Common** - Most frequent cards
- 🔵 **Uncommon** - Less frequent cards
- 🟡 **Rare** - Rare cards
- 🟠 **Mythic** - Mythic rare cards

### Display Rules
- Show rarity completion counts in parentheses: `(⚪15 🔵12 🟡14 🟠1)`
- Only show rarities that exist in included groups
- Order consistently: Common → Uncommon → Rare → Mythic

## Finish Type Integration
Reuse finish indicators from card collection display:
- 📄 **Regular/Nonfoil** - Standard finish
- ✨ **Foil** - Foil finish cards  
- 🌟 **Etched** - Etched foil finish cards

## Data Structure Requirements

### Expected Backend Format
```json
{
  "setCode": "MOM",
  "collectionSummary": {
    "totalUniqueOwned": 259,
    "totalUniqueInSet": 302,
    "totalCardsOwned": 388,
    "completionPercentage": 85.7,
    "includedGroups": ["main-set", "showcase", "borderless"],
    "excludedGroups": ["serialized", "promos"]
  },
  "groups": [
    {
      "groupId": "main-set",
      "groupName": "Main Set",
      "uniqueOwned": 234,
      "uniqueTotal": 250,
      "totalCardsOwned": 234,
      "completionPercentage": 93.6,
      "isIncluded": true
    },
    {
      "groupId": "showcase",
      "groupName": "Showcase",
      "uniqueOwned": 15,
      "uniqueTotal": 20,
      "totalCardsOwned": 89,
      "completionPercentage": 75.0,
      "isIncluded": true
    }
  ],
  "rarityBreakdown": {
    "common": { "owned": 15, "total": 15 },
    "uncommon": { "owned": 12, "total": 15 },
    "rare": { "owned": 14, "total": 15 },
    "mythic": { "owned": 1, "total": 8 }
  },
  "finishBreakdown": {
    "regular": 234,
    "foil": 89,
    "etched": 65
  }
}
```

### Group Selection Persistence
```json
{
  "setCode": "MOM",
  "userGroupPreferences": {
    "main-set": { "included": true },
    "showcase": { "included": true },
    "borderless": { "included": true },
    "serialized": { "included": false },
    "promos": { "included": false }
  }
}
```

## Component Implementation Notes

### Component Name
`SetCollectionSummary` - Located at `components/molecules/Sets/SetCollectionSummary.tsx`

### Props Interface
```typescript
interface SetCollectionSummaryProps {
  setCode: string;
  collectionData: SetCollectionData;
  onGroupToggle?: (groupId: string, included: boolean) => void;
  size?: 'small' | 'medium' | 'large';
}
```

### State Management
- Group inclusion preferences stored per-set
- Real-time recalculation when toggling groups
- Optimistic UI updates with backend sync

### Interaction Behavior
- **Desktop**: Click to expand detailed breakdown
- **Mobile**: Click to expand, second click to modify groups
- **Accessibility**: Proper ARIA labels for progress bars and toggles

### Business Logic Rules

1. **Group Count Display**:
   - Hide `[x/y groups]` when only 1 group is included
   - Show when multiple groups are included

2. **Progress Calculation**:
   - Only include cards from groups marked as "included"
   - Recalculate all percentages when groups change

3. **Rarity Display**:
   - Only show rarities that exist in included groups
   - Update counts based on included group filters

4. **Default Group Behavior**:
   - Apply auto-include/exclude rules on first load
   - Respect user preferences on subsequent loads

## Future Enhancements

### Potential Features
- **Collection Goals**: Set target completion percentages
- **Missing Card Lists**: Show specific missing cards
- **Collection Value**: Display estimated collection value
- **Trade Lists**: Generate want/have lists based on collection
- **Bulk Actions**: Toggle multiple groups simultaneously
- **Collection History**: Track completion progress over time

### Technical Considerations
- **Performance**: Efficient recalculation with large sets
- **Caching**: Cache group preferences and calculations
- **Offline Support**: Work offline with cached data
- **Sync Conflicts**: Handle concurrent group preference changes

---
**Created**: 2025-01-15  
**Last Updated**: 2025-01-15