# Deck Check Feature -- Design Document

Replaces the full deck builder with a focused, ephemeral tool: "Which cards in this deck list do I already own, and in which printings?"

---

## Concept

User pastes a deck list (or loads from CSV) -> system matches cards against their collection -> shows owned (with printings) vs missing -> user selects preferred printings -> exports a printing-aware deck list for re-import into their deck provider.

**No persistence. No new Cosmos containers. No deck editor. No external API calls.** The deck lives in Moxfield/Archidekt. MtgDiscovery overlays collection knowledge.

**Value proposition:** Your deck builder knows *what* you play. MtgDiscovery knows *what you own and in which printing*. Bridge the two.

---

## User Flow

1. Navigate to `/deck-check`
2. Paste text (Arena/MTGO/generic format) into the text area -- or click "Paste from Clipboard" / "Load from CSV" to populate the text area
3. Click "Check Deck"
4. See results: owned cards (with printings), missing cards, couldn't-match lines
5. Click any card to select it into the left detail panel -- change printing for export from there. Standard card actions (wishlist, collection) available via card controls in the stacks.
6. Click "Export" -> export modal shows deck list preview -> "Copy for Moxfield" or "Copy Plain Text" -> re-import into deck provider

---

## Backend Architecture

### New Projects (2)

| Project | Layer | Purpose |
|---------|-------|---------|
| `Lib.Aggregator.DeckCheck` | Aggregator | Orchestrates: parse -> card match -> collection cross-reference -> printing selection |
| `Lib.Domain.DeckCheck` | Domain | Thin pass-through to Aggregator (follows existing pattern) |

No external API adapter needed -- all input comes from user-provided text. No outbound HTTP calls.

### Additions to Existing Projects

| Project | Additions |
|---------|-----------|
| `Lib.MtgDiscovery.Entry` | `CheckDeckFromTextEntryService` + validators + mappers |
| `Lib.Shared.DataModels` | Arg, Itr, Xfr, Ouf entity interfaces for deck check flow |
| `App.MtgDiscovery.GraphQL` | `DeckCheckQueryMethods`, input/output types, union responses |

### Existing Projects Reused (no modifications)

- `Lib.Adapter.Cards` -- `CardsByNameAdapter` (exact match)
- `Lib.Adapter.UserCards` -- `UserCardsByIdsAdapter` (batch collection lookup)
- `Lib.Adapter.UserWishlistCards` -- existing wishlist write path (standard card actions)
- All corresponding Domain/Aggregator projects for cards, user cards, wishlist

---

## Data Flow

### Text Input Flow

```
GraphQL: checkDeckFromText(textContent)
  -> Entry: validate (size limits, non-empty) -> parse text into card lines -> map to Itr
    -> Domain: pass-through
      -> Aggregator:
        1. Card resolution: use most specific data available per card:
           a. Set code + collector number -> exact card lookup (point read)
           b. Set code + name -> card from that set
           c. Name only -> CardsByNameAdapter (exact match, point reads)
           Batched 20 at a time.
        2. Collection check: UserCardsByIdsAdapter (batch) -> fallback by name
        3. Printing selection: apply priority rules
        4. Format legality: intersect all matched cards' format legalities (from Scryfall data)
        5. Return: ownedCards + missingCards + unmatchedLines + legalFormats
```

All input arrives as text. CSV files are parsed client-side into text format before submission.

### Export

**Client-side.** The check result already contains card names, set codes, collector numbers, quantities, and zones. Frontend formats directly -- no server round-trip.

### Wishlist / Collection Actions

Standard card display interactions. Each card in the stacks uses the same card display components as elsewhere on the site, which include collection modification via their existing controls (kebab menu, etc.). No deck-check-specific mutations needed. The left detail panel only provides "Change Printing" for export -- it does not duplicate collection actions.

---

## Printing Substitution Priority

For each owned card, select the printing to use in exports:

1. **Imported set code matches owned printing** -> use it
2. **User owns a different printing** -> use most recently released owned printing
3. **User doesn't own the card** -> use most recent Scryfall printing (for full-list export)

User can override via the left detail panel (click any card to select it, then change printing from the detail panel actions).

---

## Collection Matching Strategy

Card resolution uses the most specific data available from the parsed line, falling back to less specific lookups:

| Step | When | What | How | Cost |
|------|------|------|-----|------|
| 1. Set+Number match | Set code AND collector number present | Exact card lookup | Point read by set code + collector number | ~3 RU/card |
| 2. Set+Name match | Set code present, no collector number | Card from specific set | Look up by name within the given set | ~3 RU/card |
| 3. Name-only match | No set code in parsed line | Card by name | `CardsByNameAdapter` exact match via CardNameGuid (point reads) | ~3 RU/card |
| 4. Collection check | Card resolved | User owns this card? | `UserCardsByIdsAdapter` batch (20 at a time) | ~1 RU/card |
| 5. Fallback (by name) | ID misses in collection | Check other printings | `UserCardsByNameAdapter` for cards not found by ID | ~3 RU/card |

Steps 1-3 are mutually exclusive (only one fires per card, based on what the parser extracted). Steps 4-5 always run.

No fuzzy matching. Input is expected from deck builder exports with exact card names. Unmatched names go to the "Couldn't Match" list with no suggestions.

**100-card deck estimate:** ~400 RU, ~300-400ms. Well within limits.

---

## GraphQL Schema

```graphql
# === INPUT TYPES ===

input CheckDeckFromTextInput {
  textContent: String!   # Max 10,000 chars, max 500 lines
}

# === RESULT TYPES ===

type DeckCheckResult {
  ownedCards: [DeckCheckOwnedCard!]!
  missingCards: [DeckCheckMissingCard!]!
  unmatchedLines: [DeckCheckUnmatchedLine!]!
  legalFormats: [String!]!    # Formats where ALL matched cards are legal (e.g., ["commander", "vintage", "legacy"])
  totalCards: Int!
  ownedCount: Int!
  missingCount: Int!
  unmatchedCount: Int!
  deckName: String            # From first comment line if present
}

type DeckCheckOwnedCard {
  requestedName: String!
  requestedQuantity: Int!
  zone: String              # Commander, Sideboard, Maindeck, etc.
  card: Card!               # Existing Card type from Scryfall data
  ownedPrintings: [DeckCheckOwnedPrinting!]!
  selectedPrinting: DeckCheckSelectedPrinting!
}

type DeckCheckOwnedPrinting {
  setCode: String!
  setName: String!
  collectorNumber: String!
  finish: String!           # nonfoil, foil, etched
  count: Int!
}

type DeckCheckSelectedPrinting {
  cardId: ID!
  setCode: String!
  collectorNumber: String!
  name: String!
  finish: String!             # nonfoil, foil, etched
}

type DeckCheckMissingCard {
  requestedName: String!
  requestedQuantity: Int!
  zone: String
  card: Card!
  defaultPrintingSetId: ID!
}

type DeckCheckUnmatchedLine {
  originalText: String!     # The line that couldn't be matched to any card
}

# === SUCCESS/FAILURE TYPES ===

type CheckDeckFromTextSuccess {
  result: DeckCheckResult!
}

type CheckDeckFromTextFailure {
  message: String!
  code: String!
}

# === UNION RESPONSES (follows HotChocolate success/failure pattern) ===

union CheckDeckFromTextResponse = CheckDeckFromTextSuccess | CheckDeckFromTextFailure

# === QUERIES ===
# Check operation is a query -- it reads and computes, no side effects.

extend type Query {
  checkDeckFromText(input: CheckDeckFromTextInput!): CheckDeckFromTextResponse! @authorize
}

# No deck-check-specific mutations. Wishlist and collection actions use
# existing standard mutations available on card display components.
```

---

## Entry Layer Validators

### Text flow (`CheckDeckFromTextArgEntityValidatorContainer`)

- `HasValidUserIdCheckDeckArgValidator` -- userId is non-empty
- `AuthUserMatchesUserIdCheckDeckValidator` -- JWT user matches request userId
- `HasValidTextContentCheckDeckArgValidator` -- non-null, max 10K chars, max 500 lines

No additional validators needed. Wishlist and collection actions use their existing validation paths.

---

## Frontend

### Page: `DeckCheckPage.tsx` at `/deck-check`

#### Input View

```
+--------------------------------------------------------------+
| Deck Check                                                    |
+--------------------------------------------------------------+
|                                                                |
| +----------------------------------------------------------+ |
| |                                                            | |
| |  (text area -- paste deck list here)                       | |
| |                                                            | |
| +----------------------------------------------------------+ |
|                                                                |
| [Paste from Clipboard]  [Load from CSV]        [Check Deck]  |
+--------------------------------------------------------------+
```

#### Results View (Visual Stacks, Grouped by Type)

Uses the existing "Visual Stacks (Split)" card display, grouped by card type. Same layout used elsewhere on the site for collection browsing.

```
+--------------------------------------------------------------+
| Deck Check Results    Owned: 72/100  Missing: 25  Unmatched: 3|
| Legal in: Commander, Vintage, Legacy                          |
| [Export]                                                      |
+--------------------------------------------------------------+
|              |                                                 |
| +----------+| ⭐ Commander (1)                                    |
| | [card   ]|| [card]                                           |
| | [image  ]||                                                  |
| | [       ]|| 🐾 Creatures (14)  ⚡ Instants (16)  🏔 Lands (24) |
| | [       ]|| [stack]            [stack]           [stack]     |
| |          || [stack]            [stack]           [stack]     |
| | [Change ]||                                                  |
| | [Qty: 4 ]|| 📜 Sorceries (9)   🔮 Artifacts (3)  Sideboard (1)|
| |          || [stack]            [stack]           [stack]     |
| | [Print. ]||                                                  |
| +----------+| Couldn't Match (3):                              |
|              | "Tefari"                                         |
|              | "Bollt Lightning"                                |
+--------------------------------------------------------------+
```

**Layout:**
- **Left panel (detail panel):** Full-size card image with a "Change Printing" button beneath it. When the card has quantity > 1, a "Change Qty" control appears next to "Change Printing" to specify how many copies switch to the new printing (default: all). No buy/sell buttons, no wishlist button -- those actions are handled through the standard card display interactions on the cards in the stacks (kebab menu, etc.).
- **Right area (card stacks):** Cards grouped by type in vertical overlapping stacks. Each type group has an icon + header with count (e.g., "🐾 Creatures (14)"). Same visual stacking as the existing collection "Visual Stacks (Split)" view.
- **Commander group:** If a Commander is identified (explicit zone header or inferred from 100-card deck), shown as its own group at the top.
- **Sideboard group:** Shown as its own column alongside the type groups.
- **Couldn't Match section:** Shown below the card stacks as a plain text list.

**Card stacking behavior:**
- Cards of the same name stack vertically with overlap, showing each copy's name bar.
- Multiple copies fan out slightly in the stack.
- Each card in the stack shows the image of its assigned printing. If 4x Lightning Bolt is split across 2 printings (2x M11, 2x M10), the stack shows 2 cards with M11 art and 2 with M10 art.
- One card per stack can be expanded to show the full card image inline.
- Changing a printing updates the card images in the stack immediately.

**Interaction:**
- **Click any card in a stack** -> it appears in the left detail panel with full card image and a "Change Printing" button. All other interactions (wishlist, collection) use the standard card display controls on the cards in the stacks.
- **Selected card** is highlighted in the stack (visual indicator -- border glow or outline).
- **Non-selected cards** in stacks show the card with no interactive elements -- just the card display.
- **Owned cards** show with standard appearance (or green ownership indicator).
- **Missing cards** show with a visual indicator (red border, dimmed, or badge) to distinguish from owned.

**Printing changes:** The "Change Printing" button in the left detail panel opens a printing selector. When quantity > 1, the "Change Qty" control (default: all copies) lets the user specify how many copies switch to the new printing. For example, 4x Lightning Bolt could become 2x M11 + 2x M10. The stack images and export output update immediately.

### Input Area

Single text area with helper buttons:

- **Paste from Clipboard:** Reads clipboard text (`navigator.clipboard.readText()`) and populates the text area
- **Load from CSV:** Opens a file picker (hidden `<input type="file" accept=".csv">`), reads the file client-side via `FileReader`, parses CSV rows into text format (e.g., Arena-style `1 CardName (SET) 123`), and populates the text area. User can review/edit the converted text before submitting.
- **Check Deck:** Sends text area content to `checkDeckFromText` query

No tabs. No file upload control. CSV is a convenience loader that converts to text.

### Components (Atomic Design)

**Page:**
- `DeckCheckPage` -- local state only, no Context needed (ephemeral tool)

**Organisms:**
- `DeckCheckInputPanel` -- text area + action buttons (paste, load CSV, check)
- `DeckCheckResultsPanel` -- wrapper for detail panel + type-grouped card stacks
- `DeckCheckCardDetailPanel` -- left panel showing selected card's full image + "Change Printing" button + "Change Qty" control (shown when quantity > 1)
- `DeckCheckTypeGroupArea` -- right area containing all type group columns + sideboard + unmatched
- `DeckCheckUnmatchedSection` -- plain text list of lines that couldn't be matched
- `DeckCheckActionsBar` -- summary stats + "Export" button (opens export modal)
- `DeckCheckExportModal` -- export dialog with deck list preview + "Copy for Moxfield" / "Copy Plain Text" buttons

**Molecules:**
- `DeckCheckTypeGroup` -- single type column (e.g., "Creatures (14)") with icon, header, count, and stacked cards. Reuses existing visual stack grouping component.
- `DeckCheckCardStack` -- overlapping vertical stack of cards of the same name. Click any card to select it into the detail panel.
- `DeckCheckSummaryStats` -- owned/missing/couldn't match counts
- `DeckCheckUnmatchedRow` -- single unmatched line displayed as-is

**Atoms:**
- `OwnershipBadge` -- visual indicator on each card: owned vs missing (green/red border, badge, or opacity)

Card display uses the same card components and visual stacking used elsewhere on the site. Standard card display interactions (wishlist, collection modification) are available on the cards in the stacks via their existing controls. The left detail panel is deck-check-specific: large card image + "Change Printing" button + "Change Qty" control (when quantity > 1).

### State Management

No React Context needed. `DeckCheckPage` manages all state locally since this is a single-page ephemeral tool:

```typescript
interface PrintingAssignment {
  printing: SelectedPrinting;
  quantity: number;
}

interface DeckCheckPageState {
  textContent: string;
  isLoading: boolean;
  error: string | null;
  result: DeckCheckResult | null;
  selectedCardKey: string | null;              // which card is shown in the left detail panel
  printingOverrides: Map<string, PrintingAssignment[]>; // per card name, supports split across printings
}
```

Example: 4x Lightning Bolt initially assigned to M11. User changes 2 to M10:
```typescript
printingOverrides.get("Lightning Bolt") = [
  { printing: { setCode: "M11", ... }, quantity: 2 },
  { printing: { setCode: "M10", ... }, quantity: 2 },
]
```

### Export Modal

Opened by clicking the "Export" button in the results header. Shows a formatted deck list preview with per-format copy buttons.

```
+----------------------------------------------+
| Export Options                            [X] |
+----------------------------------------------+
|                                                |
| Deck List                                      |
| +--------------------------------------------+|
| | 1 Saskia the Unyielding (PZ2) 61          ||
| | 1 Arcane Signet (ZNC) 106                  ||
| | 1 Ascend from Avernus (CLB) 5              ||
| | 1 Assault Strobe (SOM) 82                  ||
| | 1 Austere Command (CMR) 12                 ||
| | 1 Badlands (3ED) 282                       ||
| | ...                                         ||
| +--------------------------------------------+|
|                                                |
|               [Copy for Moxfield]              |
|               [Copy Plain Text]                |
|                                                |
+----------------------------------------------+
```

**Behavior:**
- **Deck list preview:** Read-only text area showing the full deck list with set codes and collector numbers from the user's selected printings. Scrollable for large decks.
- **Copy for Moxfield:** Copies Moxfield-compatible format to clipboard (`1 Card Name (SET) 123`). Grouped by zone.
- **Copy Plain Text:** Copies simple `1x Card Name` format to clipboard.
- **After copy:** Button text changes to "Copied!" briefly to confirm.

**Component:** `DeckCheckExportModal` (Organism)
- **Props:** cards (with printing assignments)
- **Internal state:** which button was last clicked (for "Copied!" feedback)

**Formatting functions (client-side):**

```typescript
function formatMoxfield(cards: DeckCheckCard[]): string {
  // Group by zone, format each line as: "1 Sol Ring (C21) 263"
}

function formatPlainText(cards: DeckCheckCard[]): string {
  // Format each line as: "1x Sol Ring"
}
```

### Client-Side CSV Parsing

```typescript
function parseCsvToText(csvContent: string): string {
  // Parse CSV headers (case-insensitive)
  // Extract Name, Quantity, Set Code, Zone columns
  // Convert each row to Arena format: "1 CardName (SET) 123"
  // Group by Zone with zone headers
  // Return formatted text for the text area
}
```

Expected CSV columns: `Name` (required), `Quantity` (default 1), `Set Code` (optional), `Zone` (optional). Column headers are case-insensitive. Unknown columns are ignored. First row must be headers.

### Navigation

Add a "Deck Check" link nested under the "Browse" menu in the header. Only visible when a collection is in the URL (collection context required -- deck check needs a collection to match against).

---

## Security

| Concern | Mitigation |
|---------|------------|
| **Input size** | Text: 10K chars, 500 lines. Parsed cards: max 500 unique. Quantities: 1-99 per line. |
| **Authentication** | All queries require `@authorize` (Auth0 JWT). userId from JWT validated against request userId. |

No SSRF concerns -- there are no outbound HTTP calls. All input is user-provided text content. CSV is parsed client-side before submission.

---

## Text Parsing Formats

### Arena Format
```
Commander
1 Atraxa, Praetors' Voice (C16) 28

Deck
4 Lightning Bolt (M11) 146
4 Counterspell (MH2) 267

Sideboard
2 Negate (M20) 69
```
Regex: `^(\d+)\s+(.+?)\s+\(([A-Z0-9]+)\)\s+(\d+)$`

### MTGO Format
```
4 Lightning Bolt
4 Counterspell

Sideboard
2 Negate
```
Regex: `^(\d+)\s+(.+)$`

### Generic Text
```
4x Lightning Bolt
4x Counterspell
```
Regex: `^(\d+)x?\s+(.+)$`

### Zone Detection
- Lines matching `Commander`, `Deck`, `Sideboard`, `Companion`, `Maybeboard` (case-insensitive) set the current zone
- Blank lines between sections can also indicate zone transitions (Arena convention)
- Split card names: `Fire // Ice` -- try full name first, then first face
- **Commander inference:** If the deck has exactly 100 cards and no explicit `Commander` zone header, treat the first card in the list as the Commander

---

## CSV Format (Client-Side Parsing)

The CSV is parsed entirely client-side via `FileReader` and converted to text format before submission to the backend. Expected CSV structure:

```csv
Name,Quantity,Set Code,Zone
Lightning Bolt,4,M11,Deck
Sol Ring,1,C21,Deck
Counterspell,4,,Sideboard
Atraxa Praetors' Voice,1,C16,Commander
```

- `Name` column is required (case-insensitive header matching)
- `Quantity` defaults to 1 if omitted or empty
- `Set Code` is optional -- included in text conversion when present
- `Zone` is optional -- defaults to Maindeck
- Unknown columns are ignored
- First row must be headers

Converted to Arena-style text before populating the text area, so the user can review and edit before submitting.

---

## Reusable from Existing Planning

From `06-import-export-flows.md` -- directly reusable design patterns:

| Component | Reusable? | Notes |
|-----------|-----------|-------|
| Text format detection (Arena/MTGO/Generic) | Yes | Regex patterns for format auto-detection |
| Arena format parser | Yes | Regex with set code and collector number |
| MTGO format parser | Yes | Simple quantity + name regex |
| Generic text parser | Yes | `Nx name` pattern |
| Zone header detection | Yes | Commander/Sideboard/Companion parsing |
| Split card handling | Yes | `Fire // Ice` name variants |
| Moxfield export format | Adapted | Arena-style format (`1 Name (SET) 123`) for Moxfield import |
| Printing selection priority | Yes | Same 3-tier priority logic |
| Card name normalization | Yes | Lowercase, remove special characters |

**Not needed:** URL import, external API calls, SSRF prevention, rate limiting, Moxfield/Archidekt API models, JSON/CSV export, import confirmation flow, deck persistence, folder/version handling, format validation, companion handling, deck CRUD, drag-and-drop, opening hand simulator, collaborative editing, deck folders.

---

## Implementation Phases

### Phase 1: Backend

1. Create shared entity interfaces (Arg, Itr, Xfr, Ouf) in `Lib.Shared.DataModels`
2. Create `Lib.Aggregator.DeckCheck` -- card matching, collection cross-reference, printing selection
3. Create `Lib.Domain.DeckCheck` -- thin pass-through
4. Add `CheckDeckFromTextEntryService` + validators to `Lib.MtgDiscovery.Entry`
5. Add `DeckCheckQueryMethods` + types to `App.MtgDiscovery.GraphQL`

### Phase 2: Frontend

1. Create `DeckCheckPage` + route + navigation link
2. Build input panel (text area + paste/CSV/check buttons)
3. Build results panel (owned/missing/unmatched) using standard card display components
4. Wire up left detail panel (card selection + "Change Printing" button)
5. Build export modal (deck list preview, Copy for Moxfield, Copy Plain Text)
6. Client-side CSV parsing

### Phase 3: Polish

1. Error handling, loading states, empty states
2. Mobile responsive layout
3. Unit tests for parsers, matching, printing selection, export formatting

---

## Critical Files to Reference

| File | Why |
|------|-----|
| `Lib.MtgDiscovery.Entry/Commands/UserWishlistCards/AddCardToWishlistEntryService.cs` | Entry service pattern to follow (validators, mappers, domain call) |
| `Lib.Adapter.Cards/Queries/CardsByNameAdapter.cs` | Card name resolution via CardNameGuid (exact match) |
| `Lib.Adapter.UserCards/Queries/UserCardsByIdsAdapter.cs` | Batch collection lookup pattern |
| `client/src/contexts/WishlistContext.tsx` | Existing wishlist mutation + optimistic updates |
| Standard card display component | Reuse for deck check results -- provides collection modification actions |

---

## Verification

1. **Text-paste flow:** Paste a Commander deck list (100 cards) -> verify owned/missing split matches manual count -> Copy for Moxfield -> verify set codes use owned printings
2. **CSV load flow:** Load a CSV file -> verify it populates the text area in correct format -> verify same results as equivalent manual text paste
3. **Wishlist:** Use standard card action to add missing card to wishlist -> verify card appears in existing Wishlist page
4. **Export round-trip:** Copy for Moxfield -> paste into Moxfield -> verify printings match user's collection
5. **Input validation:** Oversized text, empty input -> verify rejection at Entry layer
6. **Performance:** 100-card deck check completes in <1s

---

**Last Updated:** 2026-01-26
**Status:** Design Complete, Ready for Implementation
