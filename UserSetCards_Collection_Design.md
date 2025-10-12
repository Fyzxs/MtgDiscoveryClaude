# UserSetCards Collection Design

## Document Structure

```typescript
interface UserSetCards {
  userId: string;       // User ID (partition key)
  setId: string;        // Set ID (e.g., "neo", "foundations")
  totalCards: number;   // Total count of all cards collected in set
  uniqueCards: number;  // Count of unique array entries across all groups/finishes
  collecting: {
    setGroupId: string;   // Group identifier (e.g., "main-set", "commander")
    collecting: boolean;  // Whether this group counts toward set completion
    count: number;        // Number of cards in this group (unique cards across all finishes)
  }[];
  groups: {
    key: string;          // setGroupId
    value: {
      nonFoil: { cards: string[] };  // Array of cardIds
      foil: { cards: string[] };     // Array of cardIds
      etched: { cards: string[] };   // Array of cardIds
    }
  }[]
}
```

## Example Document

```json
{
  "userId": "6cd19505-4db6-5a83-94b5-8d58f7f8aa38",
  "setId": "neo",
  "totalCards": 47,
  "uniqueCards": 23,
  "collecting": [
    {
      "setGroupId": "main-set",
      "collecting": true,
      "count": 15
    },
    {
      "setGroupId": "commander",
      "collecting": false,
      "count": 8
    }
  ],
  "groups": [
    {
      "key": "main-set",
      "value": {
        "nonFoil": { "cards": ["card1", "card2", "card3"] },
        "foil": { "cards": ["card1", "card4"] },
        "etched": { "cards": [] }
      }
    },
    {
      "key": "commander",
      "value": {
        "nonFoil": { "cards": ["card5"] },
        "foil": { "cards": ["card5", "card6"] },
        "etched": { "cards": ["card7"] }
      }
    }
  ]
}
```

## Update Logic

### Adding a Card

```typescript
// Example: User adds "card123" as foil to "main-set" group
function addCard(cardId: string, groupName: string, finish: string) {
  const finishArray = groups[groupName][finish];

  if (!finishArray.includes(cardId)) {
    finishArray.push(cardId);
    unique_cards++; // Increment because this cardId is new to this specific array
  }
  total_cards++; // Always increment (even duplicates count)
}
```

### Removing a Card

```typescript
// Example: User removes "card123" foil from "main-set" group
function removeCard(cardId: string, groupName: string, finish: string) {
  const finishArray = groups[groupName][finish];

  if (finishArray.includes(cardId)) {
    groups[groupName][finish] = finishArray.filter(id => id !== cardId);
    unique_cards--; // Decrement because cardId was removed from this array
  }
  total_cards--; // Always decrement
}
```

## Key Characteristics

### Counting Logic
- **totalCards**: Sum of ALL cards across ALL groups and finishes (duplicates count)
- **uniqueCards**: Count of distinct array entries (same card in different finish/group = multiple entries)
- **collecting.count**: Number of unique cards in this group (across all finishes)
- **collecting.collecting**: Whether this group is included in set completion calculations

### Examples
```json
{
  "totalCards": 5,
  "uniqueCards": 5,
  "collecting": [
    { "setGroupId": "main-set", "collecting": true, "count": 2 },
    { "setGroupId": "commander", "collecting": false, "count": 3 }
  ],
  "groups": [
    {
      "key": "main-set",
      "value": {
        "foil": { "cards": ["card1"] },      // +1 uniqueCards
        "nonFoil": { "cards": ["card1"] }    // +1 uniqueCards (same card, different finish)
      }
    },
    {
      "key": "commander",
      "value": {
        "foil": { "cards": ["card1"] },      // +1 uniqueCards (same card, different group)
        "nonFoil": { "cards": ["card2", "card3"] }  // +2 uniqueCards
      }
    }
  ]
}
```

### Cosmos DB Benefits
- **Efficient Partitioning**: `id=setId, partition=userId`
- **Fast Queries**: Single document per user/set combination
- **Pre-computed Metrics**: No need to count arrays for display
- **Single Document Updates**: Atomic operations for collection changes

### Performance Considerations
- **Maximum Document Size**: ~500 cards × 3 finishes × multiple groups = manageable within 2MB limit
- **Array Operations**: Simple push/filter operations for card management
- **Query Performance**: Partition key (userId) + id (setId) = point queries

## Integration Points

### Frontend Display
- **All Sets Page**: Use `total_cards` and `unique_cards` for progress display
- **Set Page**: Query user's collection status per card using group arrays
- **Collection UI**: Show progress by group and finish type

### Backend Operations
- **Card Addition**: Update appropriate group/finish array + increment counters
- **Card Removal**: Update appropriate group/finish array + decrement counters
- **Group Configuration**: Create/modify group structure as user changes preferences

### Data Consistency
- **Always Track Everything**: Even if user hasn't configured groups yet
- **Lazy Initialization**: Create document on first card addition to set
- **Atomic Updates**: Use Cosmos DB transactions for counter consistency

## UI Design Specification

### Default Set Display (All Sets Page)

```
----------------------------
-   EXISTING SET DISPLAY   -
-                          -
-                          -
-                          -
- {unique} of {set total} cards  -
- [{progress bar} X%     ] -
- {total cards} cards collected -
-                          -
----------------------------
```

### Expanded Set Display (When Collector Parameter Present)

When there's a collector parameter, the set card expands to show group-by-group breakdown:

```
-------------------------------------------------------
- [X] {group name} | {unique cards} : X% [non-foil emoji] -
-                  | {unique cards} : X% [foil emoji]     -
-                  | {unique cards} : X% [etched emoji]   -
- [X] {group name} | {unique cards} : X% [non-foil emoji] -
-                  | {unique cards} : X% [foil emoji]     -
- {repeats for each group}                               -
-------------------------------------------------------
```

**UI Elements:**
- **[X] Checkbox**: Toggles inclusion of group in overall set percentage calculation
- **{group name}**: Display name of the collection group (e.g., "Main Set", "Commander")
- **{unique cards}**: Count of unique cards collected in this group/finish combination
- **X%**: Percentage completion for this group/finish combination
- **[emoji]**: Finish type indicator (🔹 non-foil, ✨ foil, ⚡ etched)

### Tooltip Breakdown

Hovering over any `{unique cards} : X% [emoji]` line shows detailed breakdown:

```
--------------------------------
- x of Y [🔹] Non-foil cards    -
- x of Y [✨] Foil cards        -
- x of Y [⚡] Etched cards      -
--------------------------------
```

**Tooltip Details:**
- **x**: Number of cards collected in this finish
- **Y**: Total cards available in this finish for this group
- **[emoji]**: Finish type visual indicator

### Interaction Behavior

1. **Checkbox Toggle**: Updates which groups count toward overall set completion percentage
2. **Group Configuration**: Checking/unchecking groups modifies the user's collection tracking preferences
3. **Real-time Updates**: Progress bars and percentages update as user adds/removes cards from collection
4. **Responsive Design**: Layout adapts for mobile vs desktop display

### Data Binding

```typescript
interface SetDisplayData {
  setId: string;
  setName: string;
  setTotalCards: number;

  // Overall metrics (based on selected groups)
  overallUniqueCards: number;
  overallPercentage: number;
  overallTotalCards: number;

  // Per-group breakdown (from userSetCards.collecting)
  groups: {
    setGroupId: string;
    displayName: string;
    isCollecting: boolean;    // From collecting.collecting
    count: number;            // From collecting.count
    finishes: {
      finishType: 'nonFoil' | 'foil' | 'etched';
      collectedCards: number;
      totalCards: number;     // From set metadata (to be implemented)
      percentage: number;
      emoji: string;
    }[];
  }[];
}
```

## GraphQL API

### Query Structure

```graphql
query GetUserSetCards($setCardArgs: UserSetCardInput!) {
  userSetCards(setCardArgs: $setCardArgs) {
    __typename
    ... on UserSetCardSuccessResponse {
      data {
        userId
        setId
        totalCards
        uniqueCards
        collecting {
          setGroupId
          collecting
          count
        }
        groups {
          key
          value {
            nonFoil { cards }
            foil { cards }
            etched { cards }
          }
        }
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```

### Query Variables

```json
{
  "setCardArgs": {
    "userId": "6cd19505-4db6-5a83-94b5-8d58f7f8aa38",
    "setId": "neo"
  }
}
```

### Initial State Behavior

**When user has NO collection data for a set:**
- `totalCards`: 0
- `uniqueCards`: 0
- `collecting`: [] (empty array)
- `groups`: [] (empty array)

**Default group selection:**
- All groups default to `collecting: false`
- User must explicitly opt-in to tracking groups
- Empty `collecting` array indicates no groups are being tracked

### Set Completion Calculation

The completion percentage is calculated based on **only the groups where `collecting: true`**:

```typescript
// Example calculation
const selectedGroups = userSetCards.collecting.filter(g => g.collecting === true);
const totalCollectedInSelectedGroups = selectedGroups.reduce((sum, g) => sum + g.count, 0);
const totalAvailableInSelectedGroups = selectedGroups.reduce((sum, g) => {
  // Get from set metadata (to be implemented)
  return sum + setMetadata.groups[g.setGroupId].totalCards;
}, 0);
const completionPercentage = (totalCollectedInSelectedGroups / totalAvailableInSelectedGroups) * 100;
```

### Mutation: Add Set Group to Tracking

```graphql
mutation AddSetGroupNormal($input: AddSetGroupToUserSetCardInput!) {
  addSetGroupToUserSetCard(input: $input) {
    __typename
    ... on UserSetCardSuccessResponse {
      data {
        userId
        setId
        totalCards
        uniqueCards
        collecting {
          setGroupId
          collecting
          count
        }
      }
      status {
        message
        statusCode
      }
    }
    ... on FailureResponse {
      status {
        message
        statusCode
      }
    }
  }
}
```

**Mutation Variables:**

```json
{
  "input": {
    "setId": "neo",
    "setGroupId": "other",
    "collecting": true,
    "count": 399
  }
}
```

**Usage:**
- Adds or updates a set group in the user's `collecting` array
- `collecting: true` marks group for inclusion in set completion
- `collecting: false` keeps group tracked but excludes from completion calculations
- `count` represents total unique cards in this group (across all finishes)

## Implementation Plan

### Backend Card Processing Enhancement

**Add grouping information during card ingestion/processing:**

```typescript
interface Card {
  // existing fields...
  setGroupId?: string; // e.g., "main-set", "commander", "special-treatments"
}
```

**Grouping Detection Logic:**
- **Collector Number Patterns**: Main set vs supplemental products
- **Set Type Analysis**: Commander, special releases, etc.
- **Card Properties**: Promo types, frame effects, special treatments
- **Set-Specific Rules**: Per-set customization for unique structures

### Frontend Collection Updates

**Simplified collection logic when cards already have grouping:**

```typescript
function addCardToCollection(card: Card, finish: string, count: number) {
  const groupName = card.grouping || 'main-set'; // fallback to main-set

  // Update the appropriate group/finish array
  if (!userSetCards.groups[groupName][finish].includes(card.id)) {
    userSetCards.groups[groupName][finish].push(card.id);
    userSetCards.unique_cards++;
  }
  userSetCards.total_cards += count;
}

function removeCardFromCollection(card: Card, finish: string, count: number) {
  const groupName = card.grouping || 'main-set';

  // Remove from appropriate group/finish array
  const finishArray = userSetCards.groups[groupName][finish];
  if (finishArray.includes(card.id)) {
    userSetCards.groups[groupName][finish] = finishArray.filter(id => id !== card.id);
    userSetCards.unique_cards--;
  }
  userSetCards.total_cards -= count;
}
```

### Benefits of Backend Processing Approach

✅ **Frontend Simplicity**: No complex grouping logic needed in UI
✅ **Consistency**: Grouping determined once during ingestion
✅ **No Runtime Lookups**: Grouping comes with card data
✅ **Easy Updates**: `card.grouping` directly maps to collection structure
✅ **Scalable**: Can handle complex set structures without frontend changes

### Processing Pipeline Integration

1. **Scryfall Data Ingestion**: Analyze card properties and determine grouping
2. **Set Analysis**: Create group metadata (totals, available finishes)
3. **Card Document Update**: Add `grouping` field to card records
4. **Frontend Query**: Cards come pre-tagged with grouping information