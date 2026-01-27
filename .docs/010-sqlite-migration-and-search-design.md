# SQLite Migration & Scryfall-Level Search

## Goal

Move all production reads for static (non-user) data from Cosmos DB to a single SQLite database. Cosmos containers are retained for ingestion writes and manual investigation. This achieves two things simultaneously:
1. **Cost reduction** - 14 static Cosmos containers no longer serve production reads, enabling minimal RU/s or serverless pricing
2. **Scryfall-level search** - FTS5 provides full-text search across all card fields at zero additional cost

## Scope

### Moves to SQLite (static, read-only at runtime, rebuilt during ingestion)

| Cosmos Container | SQLite Replacement | Current Read Operations |
|---|---|---|
| CardItems | `cards` table | Point read by ID (CardsById query) |
| SetCards | `cards` table (set_code column) | Query by set ID (CardsBySetCode query) |
| SetItems | `sets` table | Point read by ID, all sets (SetsById, AllSets queries) |
| CardsByName | `cards` table + FTS5 | Query by name GUID (CardsByName query) |
| CardNameTrigrams | FTS5 `card_names_fts` (trigram tokenizer) | Substring search (CardNameSearch query) |
| ArtistItems | `artists` table | Not queried at runtime currently |
| ArtistCards | `card_artists` junction table | Query by artist ID (CardsByArtist query) |
| ArtistNameTrigrams | FTS5 `artist_names_fts` (trigram tokenizer) | Substring search (ArtistSearch query) |
| SetCodeToIdAssociations | `sets` table (code column) | Code-to-ID lookup (used by CardsBySetCode, SetsByCode) |
| SetArtists, ArtistSets | `set_artists` junction (indexed both directions) | Relationship tracking |
| SetParentAssociations | `set_associations` table | Parent set relationships |
| RulingItems | `rulings` table | Not heavily queried at runtime |
| SealedProductItems | `sealed_products` table | SealedProductsBySetCode query |

### Stays in Cosmos DB (dynamic user data - read + write at runtime)

| Container | Reason |
|---|---|
| UserInfo | Written at user registration, read per-session |
| UserCards | Written when users add/remove cards |
| UserSetCards | Written when users track set completion |
| UserWishlistCards | Written when users manage wishlists |
| UserSealedProducts | Written when users track sealed products |

### Stays in Cosmos DB (static data - write during ingestion only, not queried by the app)

All 14 static containers continue to receive writes during Scryfall ingestion. This preserves the data in Cosmos DB for manual investigation via Azure Portal / Data Explorer. The containers are **not queried by the application** at runtime - all production reads come from SQLite.

Since these containers no longer serve production traffic, their provisioned throughput can be reduced to the minimum (400 RU/s or lower with serverless) to minimize cost while retaining investigatory access.

---

## SQLite Database Schema

### Entity Tables

```sql
-- Cards (replaces: CardItems, SetCards, CardsByName)
-- Contains EVERY field from the Scryfall card object.
-- Deterministic scalars are flattened columns. Non-deterministic arrays are JSON TEXT.
-- Denormalized columns duplicate data from normalized tables (junction/type-keyed)
-- so card display never requires joins. Normalized tables remain the source of
-- truth for search and filter queries.
CREATE TABLE cards (
    -- ── Identification ────────────────────────────────────────────────
    id TEXT PRIMARY KEY,
    oracle_id TEXT,
    arena_id INTEGER,
    mtgo_id INTEGER,
    mtgo_foil_id INTEGER,
    tcgplayer_id INTEGER,
    tcgplayer_etched_id INTEGER,
    cardmarket_id INTEGER,
    -- ── Name & text ───────────────────────────────────────────────────
    name TEXT NOT NULL,
    name_lower TEXT NOT NULL,          -- computed: for exact case-insensitive match
    flavor_name TEXT,                  -- alternate name for special printings
    lang TEXT,
    printed_name TEXT,                 -- originally printed name
    printed_type_line TEXT,            -- originally printed type line
    printed_text TEXT,                 -- originally printed text
    -- ── Card type & rules ─────────────────────────────────────────────
    layout TEXT,
    mana_cost TEXT,
    cmc REAL,
    type_line TEXT,
    oracle_text TEXT,
    flavor_text TEXT,
    -- ── Combat stats ──────────────────────────────────────────────────
    power TEXT,                         -- string, handles '*' and 'X'
    power_numeric REAL,                 -- computed: parsed numeric for comparisons
    toughness TEXT,
    toughness_numeric REAL,
    loyalty TEXT,
    loyalty_numeric REAL,
    defense TEXT,
    defense_numeric REAL,
    -- ── Vanguard stats ────────────────────────────────────────────────
    life_modifier TEXT,
    hand_modifier TEXT,
    -- ── Set & collector info ──────────────────────────────────────────
    set_id TEXT NOT NULL,
    set_code TEXT NOT NULL,
    set_name TEXT,
    set_type TEXT,
    set_group_id TEXT,                 -- app-computed: variant grouping (borderless, showcase, etc.)
    collector_number TEXT,
    rarity TEXT,
    rarity_ord INTEGER,                -- computed: 0=common, 1=uncommon, 2=rare, 3=mythic, 4=special, 5=bonus
    -- ── Art & visual ──────────────────────────────────────────────────
    artist TEXT,                        -- primary artist name
    illustration_id TEXT,
    card_back_id TEXT,
    border_color TEXT,
    frame TEXT,
    watermark TEXT,
    security_stamp TEXT,
    -- ── Boolean flags ─────────────────────────────────────────────────
    full_art INTEGER,
    textless INTEGER,
    digital INTEGER,
    reserved INTEGER,
    foil INTEGER,
    non_foil INTEGER,
    reprint INTEGER,
    promo INTEGER,
    oversized INTEGER,
    variation INTEGER,
    booster INTEGER,
    story_spotlight INTEGER,
    game_changer INTEGER,
    content_warning INTEGER,
    -- ── Image status ──────────────────────────────────────────────────
    image_status TEXT,
    high_res_image INTEGER,
    -- ── Rankings ───────────────────────────────────────────────────────
    edhrec_rank INTEGER,
    penny_rank INTEGER,
    -- ── Computed fields ───────────────────────────────────────────────
    color_count INTEGER,               -- computed: count from card_colors
    identity_count INTEGER,            -- computed: count from card_color_identity
    is_spell INTEGER,                  -- computed: not a land
    is_permanent INTEGER,              -- computed: creature/artifact/enchantment/planeswalker/battle/land
    is_vanilla INTEGER,                -- computed: creature with no oracle_text
    -- ── Flattened image URIs (from card_images + image_types) ─────────
    image_uri_small TEXT,
    image_uri_normal TEXT,
    image_uri_large TEXT,
    image_uri_png TEXT,
    image_uri_art_crop TEXT,
    image_uri_border_crop TEXT,
    -- ── Flattened prices (from card_prices) ───────────────────────────
    price_usd REAL,
    price_usd_foil REAL,
    price_usd_etched REAL,
    price_eur REAL,
    price_eur_foil REAL,
    price_tix REAL,
    -- ── Flattened URIs (from card_uris) ───────────────────────────────
    uri TEXT,                           -- Scryfall API URI
    scryfall_uri TEXT,                  -- Scryfall web page URI
    rulings_uri TEXT,
    prints_search_uri TEXT,
    set_uri TEXT,
    set_search_uri TEXT,
    scryfall_set_uri TEXT,
    -- ── Non-deterministic arrays (JSON TEXT) ──────────────────────────
    -- Duplicated from normalized tables for join-free card display.
    artist_ids TEXT,                    -- JSON array: '["id1","id2"]'
    multiverse_ids TEXT,               -- JSON array: '[12345, 67890]'
    colors TEXT,                        -- JSON array: '["W","U"]'
    color_identity TEXT,                -- JSON array: '["W","U","B"]'
    color_indicator TEXT,               -- JSON array: '["W","U"]' (multi-faced cards)
    keywords TEXT,                      -- JSON array: '["Flying","Haste"]'
    produced_mana TEXT,                 -- JSON array: '["W","U","B","R","G"]'
    finishes TEXT,                      -- JSON array: '["nonfoil","foil"]'
    promo_types TEXT,                   -- JSON array: '["boosterfun","prerelease"]'
    frame_effects TEXT,                 -- JSON array: '["legendary","extendedart"]'
    games TEXT,                         -- JSON array: '["paper","arena","mtgo"]'
    attraction_lights TEXT,             -- JSON array: '[1,3,6]' (Attraction cards)
    -- ── Non-deterministic objects (JSON TEXT) ─────────────────────────
    legalities TEXT,                    -- JSON object: '{"standard":"legal","modern":"banned",...}'
    purchase_uris TEXT,                -- JSON object: '{"tcgplayer":"...","cardmarket":"..."}'
    related_uris TEXT,                 -- JSON object: '{"gatherer":"...","edhrec":"..."}'
    preview TEXT,                       -- JSON object: '{"source":"...","source_uri":"...","previewed_at":"..."}'
    -- ── Non-deterministic nested arrays (JSON TEXT) ───────────────────
    card_faces TEXT,                    -- JSON array: full face data for DFC rendering
    all_parts TEXT                      -- JSON array: tokens, meld parts, combo pieces
);

-- Sets (replaces: SetItems, SetCodeToIdAssociations)
-- Contains EVERY field from the Scryfall set object.
-- Deterministic scalars are flattened columns. Non-deterministic arrays are JSON TEXT.
CREATE TABLE sets (
    -- ── Identification ────────────────────────────────────────────────
    id TEXT PRIMARY KEY,
    code TEXT NOT NULL UNIQUE,
    name TEXT NOT NULL,
    tcgplayer_id INTEGER,
    -- ── Type & release ────────────────────────────────────────────────
    set_type TEXT,
    released_at TEXT,
    block_code TEXT,
    block TEXT,
    parent_set_code TEXT,
    -- ── Counts ────────────────────────────────────────────────────────
    card_count INTEGER,
    printed_size INTEGER,              -- cards with unique collector numbers
    -- ── Boolean flags ─────────────────────────────────────────────────
    digital INTEGER,
    foil_only INTEGER,
    nonfoil_only INTEGER,
    -- ── URIs ──────────────────────────────────────────────────────────
    uri TEXT,                           -- Scryfall API URI
    scryfall_uri TEXT,                  -- Scryfall web page URI
    search_uri TEXT,
    icon_svg_uri TEXT,
    -- ── Non-deterministic arrays (JSON TEXT) ──────────────────────────
    groupings TEXT                      -- JSON array: set card groupings/variant categories
);

-- Artists (replaces: ArtistItems)
CREATE TABLE artists (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    name_lower TEXT NOT NULL,
    card_count INTEGER,                -- pre-computed during ingestion
    set_count INTEGER                  -- pre-computed during ingestion
);

-- Rulings (replaces: RulingItems)
CREATE TABLE rulings (
    id TEXT PRIMARY KEY,
    oracle_id TEXT NOT NULL,
    source TEXT,
    published_at TEXT,
    comment TEXT
);

-- Sealed Products (replaces: SealedProductItems)
-- Contains EVERY field from the sealed product source object (MtgJson).
-- All fields are deterministic scalars.
CREATE TABLE sealed_products (
    -- ── Identification ────────────────────────────────────────────────
    id TEXT PRIMARY KEY,               -- uuid from MtgJson
    tcgplayer_product_id TEXT,
    mcm_id TEXT,                       -- Cardmarket ID
    cardtrader_id TEXT,
    -- ── Product info ──────────────────────────────────────────────────
    name TEXT NOT NULL,
    category TEXT,                     -- booster_box, booster_pack, bundle, etc.
    subtype TEXT,
    card_count INTEGER,
    release_date TEXT,
    -- ── Set info ──────────────────────────────────────────────────────
    set_id TEXT,
    set_code TEXT,
    set_name TEXT,
    -- ── Image ─────────────────────────────────────────────────────────
    image_url TEXT,
    -- ── Purchase URIs ─────────────────────────────────────────────────
    purchase_url_tcgplayer TEXT,
    purchase_url_cardmarket TEXT,
    purchase_url_card_kingdom TEXT
);
```

### Card Faces Table

```sql
-- Card Faces (for double-faced, split, flip, etc. cards)
-- Image URIs for faces go in card_face_images table
CREATE TABLE card_faces (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    card_id TEXT NOT NULL REFERENCES cards(id),
    face_index INTEGER NOT NULL,       -- 0 = front, 1 = back
    name TEXT,
    mana_cost TEXT,
    cmc REAL,
    type_line TEXT,
    oracle_text TEXT,
    flavor_text TEXT,
    power TEXT,
    toughness TEXT,
    loyalty TEXT,
    defense TEXT,
    artist TEXT,
    artist_id TEXT,
    illustration_id TEXT,
    watermark TEXT,
    layout TEXT
);
```

### Junction / Crosswalk Tables

```sql
-- Card <-> Artist relationship (replaces: artist_ids JSON array + ArtistCards container)
CREATE TABLE card_artists (
    card_id TEXT NOT NULL REFERENCES cards(id),
    artist_id TEXT NOT NULL REFERENCES artists(id),
    PRIMARY KEY (card_id, artist_id)
);

-- Card colors (replaces: colors JSON array)
CREATE TABLE card_colors (
    card_id TEXT NOT NULL REFERENCES cards(id),
    color TEXT NOT NULL,               -- W, U, B, R, G
    PRIMARY KEY (card_id, color)
);

-- Card color identity (replaces: color_identity JSON array)
CREATE TABLE card_color_identity (
    card_id TEXT NOT NULL REFERENCES cards(id),
    color TEXT NOT NULL,               -- W, U, B, R, G
    PRIMARY KEY (card_id, color)
);

-- Card face colors (faces can have their own colors, e.g. transform cards)
CREATE TABLE card_face_colors (
    card_id TEXT NOT NULL REFERENCES cards(id),
    card_face_id INTEGER NOT NULL REFERENCES card_faces(id),
    color TEXT NOT NULL,               -- W, U, B, R, G
    PRIMARY KEY (card_face_id, color)
);

-- Card keywords (replaces: keywords JSON array)
CREATE TABLE card_keywords (
    card_id TEXT NOT NULL REFERENCES cards(id),
    keyword TEXT NOT NULL,             -- Flying, Haste, Trample, etc.
    PRIMARY KEY (card_id, keyword)
);

-- Card produced mana (replaces: produced_mana JSON array)
CREATE TABLE card_produced_mana (
    card_id TEXT NOT NULL REFERENCES cards(id),
    mana_type TEXT NOT NULL,           -- W, U, B, R, G, C, T
    PRIMARY KEY (card_id, mana_type)
);

-- Card finishes (replaces: finishes JSON array)
CREATE TABLE card_finishes (
    card_id TEXT NOT NULL REFERENCES cards(id),
    finish TEXT NOT NULL,              -- nonfoil, foil, etched, glossy
    PRIMARY KEY (card_id, finish)
);

-- Card promo types (replaces: promo_types JSON array)
CREATE TABLE card_promo_types (
    card_id TEXT NOT NULL REFERENCES cards(id),
    promo_type TEXT NOT NULL,          -- boosterpack, gateway, prerelease, etc.
    PRIMARY KEY (card_id, promo_type)
);

-- Card games (replaces: games JSON array)
CREATE TABLE card_games (
    card_id TEXT NOT NULL REFERENCES cards(id),
    game TEXT NOT NULL,                -- paper, arena, mtgo
    PRIMARY KEY (card_id, game)
);

-- Card frame effects (replaces: frame_effects JSON array)
CREATE TABLE card_frame_effects (
    card_id TEXT NOT NULL REFERENCES cards(id),
    effect TEXT NOT NULL,              -- legendary, miracle, nyxtouched, etc.
    PRIMARY KEY (card_id, effect)
);

-- Card related parts (replaces: all_parts JSON array)
CREATE TABLE card_parts (
    card_id TEXT NOT NULL REFERENCES cards(id),
    related_card_id TEXT NOT NULL,
    component TEXT NOT NULL,           -- token, meld_part, meld_result, combo_piece
    name TEXT NOT NULL,
    type_line TEXT,
    uri TEXT,
    PRIMARY KEY (card_id, related_card_id)
);

-- Card multiverse IDs
CREATE TABLE card_multiverse_ids (
    card_id TEXT NOT NULL REFERENCES cards(id),
    multiverse_id INTEGER NOT NULL,
    PRIMARY KEY (card_id, multiverse_id)
);
```

### Type-Keyed Tables (replaces flattened columns)

Uses an `image_type` table to define the types, referenced by card and face image tables.

```sql
-- Image type lookup (small, normal, large, png, art_crop, border_crop)
CREATE TABLE image_types (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL UNIQUE          -- small, normal, large, png, art_crop, border_crop
);

-- Card images (replaces: flattened image_uri_* columns)
CREATE TABLE card_images (
    card_id TEXT NOT NULL REFERENCES cards(id),
    image_type_id INTEGER NOT NULL REFERENCES image_types(id),
    uri TEXT NOT NULL,
    PRIMARY KEY (card_id, image_type_id)
);

-- Card face images (replaces: flattened image_uri_* columns on card_faces)
CREATE TABLE card_face_images (
    card_id TEXT NOT NULL REFERENCES cards(id),
    card_face_id INTEGER NOT NULL REFERENCES card_faces(id),
    image_type_id INTEGER NOT NULL REFERENCES image_types(id),
    uri TEXT NOT NULL,
    PRIMARY KEY (card_face_id, image_type_id)
);

-- Card legalities (replaces: flattened legal_* columns)
CREATE TABLE card_legalities (
    card_id TEXT NOT NULL REFERENCES cards(id),
    format TEXT NOT NULL,              -- standard, modern, legacy, vintage, commander, etc.
    status TEXT NOT NULL,              -- legal, not_legal, banned, restricted
    PRIMARY KEY (card_id, format)
);

-- Card prices (replaces: flattened price_* columns)
CREATE TABLE card_prices (
    card_id TEXT NOT NULL REFERENCES cards(id),
    currency TEXT NOT NULL,            -- usd, usd_foil, usd_etched, eur, eur_foil, tix
    amount REAL NOT NULL,
    PRIMARY KEY (card_id, currency)
);

-- Card URIs (replaces: flattened scryfall_uri, rulings_uri, purchase_uri_*, related_uri_*)
CREATE TABLE card_uris (
    card_id TEXT NOT NULL REFERENCES cards(id),
    category TEXT NOT NULL,            -- scryfall, rulings, purchase, related
    source TEXT NOT NULL,              -- self, tcgplayer, cardmarket, cardhoarder, gatherer, edhrec, etc.
    uri TEXT NOT NULL,
    PRIMARY KEY (card_id, category, source)
);
```

### Relationship Tables

```sql
-- Set Associations (replaces: SetParentAssociations)
CREATE TABLE set_associations (
    set_id TEXT NOT NULL,
    parent_set_id TEXT,
    relationship TEXT,
    PRIMARY KEY (set_id)
);

-- Set <-> Artist relationship (replaces: SetArtists + ArtistSets containers)
-- PK covers set_id lookups; idx_set_artists_artist covers artist_id lookups.
CREATE TABLE set_artists (
    set_id TEXT NOT NULL,
    artist_id TEXT NOT NULL,
    PRIMARY KEY (set_id, artist_id)
);
```

### Design Rationale: Normalized + Denormalized Display

Every multi-valued field and every fixed-key object has a proper normalized table. The `cards` table also carries denormalized display columns that duplicate data from those normalized tables, so card display never requires joins.

**Normalized tables** (junction, type-keyed) are the source of truth for:
1. **Search & filter queries** - `WHERE id IN (SELECT card_id FROM card_colors WHERE color = 'U')`
2. **Indexed lookups** on all junction/type-keyed tables
3. **Referential integrity** via foreign keys
4. **Type tables** (e.g., `image_types`) make the schema self-documenting
5. **Consistent pattern** - legalities, prices, URIs, and images all follow the same key-value table pattern
6. **Extensible** - adding a new format, currency, or image type is a row insert, not a schema migration

**Denormalized display columns** on the `cards` table provide:
1. **Join-free card display** - `SELECT * FROM cards WHERE set_code = 'mh2'` returns everything needed to render cards
2. **Sort by denormalized fields** - `ORDER BY price_usd DESC`, `ORDER BY artist ASC`
3. **No read-time penalty** - data is duplicated at ingestion time, not at query time
4. **JSON arrays for multi-valued fields** - `colors`, `keywords`, etc. as JSON TEXT columns for direct consumption by the API layer

The database is read-only at runtime and rebuilt during ingestion, so denormalization introduces no consistency risk. Both representations are written from the same source data in the same ingestion pass.

### FTS5 Virtual Tables (replaces trigram containers + enables advanced search)

Two tokenizer strategies serve different search needs:
- **`trigram`** — character-level substring matching. `MATCH 'bolt'` finds "Thunderbolt". Used for name search (replicates current Cosmos trigram behavior).
- **`porter unicode61`** — word-level matching with English stemming. `MATCH 'draw'` finds "drawing". Used for oracle text, type line, flavor text search.

```sql
-- Card name substring search (replaces CardNameTrigrams container)
-- Trigram tokenizer enables substring matching: 'bolt' matches 'Thunderbolt'
CREATE VIRTUAL TABLE card_names_fts USING fts5(
    name,
    content='cards',
    content_rowid='rowid',
    tokenize='trigram'
);

-- Artist name substring search (replaces ArtistNameTrigrams container)
CREATE VIRTUAL TABLE artist_names_fts USING fts5(
    name,
    content='artists',
    content_rowid='rowid',
    tokenize='trigram'
);

-- Card text search (oracle text, type line, flavor text)
-- Porter tokenizer for word-level matching with stemming
CREATE VIRTUAL TABLE cards_fts USING fts5(
    oracle_text,
    type_line,
    flavor_text,
    content='cards',
    content_rowid='rowid',
    tokenize='porter unicode61'
);
```

### Indexes

```sql
-- Card scalar lookups
CREATE INDEX idx_cards_oracle_id ON cards(oracle_id);
CREATE INDEX idx_cards_name_lower ON cards(name_lower);
CREATE INDEX idx_cards_set_code ON cards(set_code);
CREATE INDEX idx_cards_set_id ON cards(set_id);
CREATE INDEX idx_cards_set_group_id ON cards(set_group_id);
CREATE INDEX idx_cards_rarity ON cards(rarity);
CREATE INDEX idx_cards_rarity_ord ON cards(rarity_ord);
CREATE INDEX idx_cards_cmc ON cards(cmc);
CREATE INDEX idx_cards_layout ON cards(layout);
CREATE INDEX idx_cards_collector_number ON cards(set_code, collector_number);
CREATE INDEX idx_cards_released ON cards(released_at);
CREATE INDEX idx_cards_artist ON cards(artist);
CREATE INDEX idx_cards_price_usd ON cards(price_usd);

-- Card faces
CREATE INDEX idx_card_faces_card ON card_faces(card_id);

-- Junction tables (reverse lookups)
CREATE INDEX idx_card_artists_artist ON card_artists(artist_id);
CREATE INDEX idx_card_colors_color ON card_colors(color);
CREATE INDEX idx_card_color_identity_color ON card_color_identity(color);
CREATE INDEX idx_card_keywords_keyword ON card_keywords(keyword);
CREATE INDEX idx_card_produced_mana_type ON card_produced_mana(mana_type);
CREATE INDEX idx_card_finishes_finish ON card_finishes(finish);
CREATE INDEX idx_card_games_game ON card_games(game);
CREATE INDEX idx_card_frame_effects_effect ON card_frame_effects(effect);
CREATE INDEX idx_card_parts_related ON card_parts(related_card_id);

-- Type-keyed tables
CREATE INDEX idx_card_images_type ON card_images(image_type_id);
CREATE INDEX idx_card_face_colors_card ON card_face_colors(card_id);
CREATE INDEX idx_card_face_images_card ON card_face_images(card_id);
CREATE INDEX idx_card_face_images_face ON card_face_images(card_face_id);
CREATE INDEX idx_card_legalities_format ON card_legalities(format, status);
CREATE INDEX idx_card_prices_currency ON card_prices(currency, amount);
CREATE INDEX idx_card_uris_category ON card_uris(category, source);

-- Set lookups
CREATE INDEX idx_sets_code ON sets(code);
CREATE INDEX idx_sets_type ON sets(set_type);

-- Relationship tables (PK covers set_id direction; this index covers artist_id direction)
CREATE INDEX idx_set_artists_artist ON set_artists(artist_id);

-- Rulings
CREATE INDEX idx_rulings_oracle ON rulings(oracle_id);

-- Sealed products
CREATE INDEX idx_sealed_set_code ON sealed_products(set_code);
CREATE INDEX idx_sealed_set_id ON sealed_products(set_id);
```

---

## Existing Query Migration

Every current GraphQL query maps cleanly to SQL:

| GraphQL Query | Current Path | SQLite Query |
|---|---|---|
| `cardsById(ids)` | Gopher -> CardItems | `SELECT * FROM cards WHERE id IN (?)` |
| `cardsBySetCode(code)` | Gopher -> SetCodeIndex, Inquisitor -> SetCards | `SELECT * FROM cards WHERE set_code = ?` |
| `cardsByName(name)` | Inquisitor -> CardsByName | `SELECT * FROM cards WHERE name_lower = ?` |
| `cardNameSearch(term)` | Inquisitor -> CardNameTrigrams | `SELECT * FROM card_names_fts WHERE card_names_fts MATCH ?` |
| `setsById(ids)` | Gopher -> SetItems | `SELECT * FROM sets WHERE id IN (?)` |
| `setsByCode(codes)` | Gopher -> SetCodeIndex + SetItems | `SELECT * FROM sets WHERE code IN (?)` |
| `allSets` | Inquisitor -> SetItems | `SELECT * FROM sets` |
| `artistSearch(term)` | Inquisitor -> ArtistNameTrigrams | `SELECT * FROM artist_names_fts WHERE artist_names_fts MATCH ?` |
| `cardsByArtist(id)` | Inquisitor -> ArtistCards | `SELECT c.* FROM cards c JOIN card_artists ca ON c.id = ca.card_id WHERE ca.artist_id = ?` |
| `cardsByArtistName(name)` | Trigram search + ArtistCards | `SELECT c.* FROM cards c JOIN card_artists ca ON c.id = ca.card_id JOIN artists a ON ca.artist_id = a.id WHERE a.id IN (SELECT rowid FROM artist_names_fts WHERE artist_names_fts MATCH ?)` |
| `sealedProductsBySetCode(code)` | Inquisitor -> SealedProductItems | `SELECT * FROM sealed_products WHERE set_code = ?` |

Note: Several queries that currently require 2 Cosmos containers (code-to-ID lookup then data fetch) become single SQLite queries.

---

## Scryfall Search Syntax -> SQL Translation

### Text Fields (use FTS5 MATCH)

| Scryfall | SQL |
|---|---|
| `name:bolt` | `WHERE id IN (SELECT rowid FROM card_names_fts WHERE card_names_fts MATCH 'bolt')` (trigram — substring match, finds "Thunderbolt") |
| `o:"draw a card"` | `WHERE id IN (SELECT rowid FROM cards_fts WHERE oracle_text MATCH '"draw a card"')` (porter — word match) |
| `t:creature` | `WHERE id IN (SELECT rowid FROM cards_fts WHERE type_line MATCH 'creature')` (porter — word match) |
| `ft:dragon` | `WHERE id IN (SELECT rowid FROM cards_fts WHERE flavor_text MATCH 'dragon')` (porter — word match) |
| `a:"john avon"` | `WHERE id IN (SELECT ca.card_id FROM card_artists ca JOIN artists a ON ca.artist_id = a.id WHERE a.id IN (SELECT rowid FROM artist_names_fts WHERE artist_names_fts MATCH '"john avon"'))` (trigram — substring match) |
| `kw:flying` | `WHERE id IN (SELECT card_id FROM card_keywords WHERE keyword = 'flying')` |

### Numeric Fields (use SQL WHERE)

| Scryfall | SQL |
|---|---|
| `cmc>3` | `WHERE cmc > 3` |
| `cmc=0` | `WHERE cmc = 0` |
| `pow>=4` | `WHERE power_numeric >= 4` |
| `tou<=2` | `WHERE toughness_numeric <= 2` |
| `pow>tou` | `WHERE power_numeric > toughness_numeric` |
| `loy=3` | `WHERE loyalty_numeric = 3` |
| `usd>10` | `WHERE id IN (SELECT card_id FROM card_prices WHERE currency = 'usd' AND amount > 10)` |

### Color Fields (use junction tables)

| Scryfall | SQL |
|---|---|
| `c:U` (includes blue) | `WHERE id IN (SELECT card_id FROM card_colors WHERE color = 'U')` |
| `c:UR` (includes U and R) | `WHERE id IN (SELECT card_id FROM card_colors WHERE color = 'U') AND id IN (SELECT card_id FROM card_colors WHERE color = 'R')` |
| `c=UR` (exactly U and R) | `WHERE color_count = 2 AND id IN (SELECT card_id FROM card_colors WHERE color = 'U') AND id IN (SELECT card_id FROM card_colors WHERE color = 'R')` |
| `c>=UR` (superset of UR) | `WHERE id IN (SELECT card_id FROM card_colors WHERE color = 'U') AND id IN (SELECT card_id FROM card_colors WHERE color = 'R')` |
| `c<=UR` (subset of UR) | `WHERE color_count <= 2 AND id NOT IN (SELECT card_id FROM card_colors WHERE color NOT IN ('U','R'))` |
| `id:UBG` (identity includes) | Same pattern on `card_color_identity` table |
| `c:colorless` | `WHERE color_count = 0` |

### Set, Rarity, Format

| Scryfall | SQL |
|---|---|
| `s:mh2` | `WHERE set_code = 'mh2'` |
| `r:mythic` | `WHERE rarity = 'mythic'` |
| `r>=rare` | `WHERE rarity_ord >= 2` |
| `f:modern` | `WHERE id IN (SELECT card_id FROM card_legalities WHERE format = 'modern' AND status = 'legal')` |
| `banned:legacy` | `WHERE id IN (SELECT card_id FROM card_legalities WHERE format = 'legacy' AND status = 'banned')` |
| `restricted:vintage` | `WHERE id IN (SELECT card_id FROM card_legalities WHERE format = 'vintage' AND status = 'restricted')` |
| `st:expansion` | `WHERE set_type = 'expansion'` |

### Boolean Flags

| Scryfall | SQL |
|---|---|
| `is:foil` | `WHERE foil = 1` |
| `is:reprint` | `WHERE reprint = 1` |
| `is:reserved` | `WHERE reserved = 1` |
| `is:digital` | `WHERE digital = 1` |
| `is:fullart` | `WHERE full_art = 1` |
| `is:promo` | `WHERE promo = 1` |
| `is:transform` | `WHERE layout = 'transform'` |
| `is:mdfc` | `WHERE layout = 'modal_dfc'` |
| `is:split` | `WHERE layout = 'split'` |
| `is:dfc` | `WHERE layout IN ('transform','modal_dfc','reversible_card')` |
| `is:spell` | `WHERE is_spell = 1` |
| `is:permanent` | `WHERE is_permanent = 1` |
| `is:vanilla` | `WHERE is_vanilla = 1` |
| `is:fetchland` | Static lookup table mapping to specific card oracle text patterns |
| `not:reprint` | `WHERE reprint = 0` |
| `-is:digital` | `WHERE digital = 0` |

### Boolean Logic

| Scryfall | SQL |
|---|---|
| `c:U t:creature` (implicit AND) | `WHERE ... AND ...` |
| `t:goblin or t:elf` | `WHERE ... OR ...` |
| `-is:digital` (NOT) | `WHERE digital = 0` or `WHERE NOT (...)` |
| `(t:goblin or t:elf) c:R` | `WHERE (...OR...) AND ...` |

### Date Fields

| Scryfall | SQL |
|---|---|
| `year:2023` | `WHERE released_at >= '2023-01-01' AND released_at < '2024-01-01'` |
| `year>=2020` | `WHERE released_at >= '2020-01-01'` |
| `date:2023-06-15` | `WHERE released_at = '2023-06-15'` |

### Sorting

| Scryfall | SQL |
|---|---|
| `order:name` | `ORDER BY name_lower ASC` |
| `order:cmc` | `ORDER BY cmc ASC` |
| `order:price` | `ORDER BY price_usd DESC` |
| `order:rarity` | `ORDER BY rarity_ord ASC` |
| `order:released` | `ORDER BY released_at DESC` |
| `order:edhrec` | `ORDER BY edhrec_rank ASC` |
| `direction:desc` | Reverses the ORDER BY direction |

### Regex

SQLite supports `REGEXP` when a regex function is registered. `Microsoft.Data.Sqlite` allows registering custom functions:

| Scryfall | SQL |
|---|---|
| `o:/\{T\}:.*draw/` | `WHERE oracle_text REGEXP '\{T\}:.*draw'` (register REGEXP function at startup) |

### Exact Name Match

| Scryfall | SQL |
|---|---|
| `!Lightning Bolt` | `WHERE name_lower = 'lightning bolt'` |
| `!"Sift Through Sands"` | `WHERE name_lower = 'sift through sands'` |

---

## Architecture Integration

### Where SQLite Fits in the Layers

The adapter layer is the right seam. The layers above (Entry, Domain, Aggregator) don't know or care whether data comes from Cosmos or SQLite. Only the adapter implementations change.

```
App Layer:        Unchanged (GraphQL queries)
Entry Layer:      Unchanged (validation, mapping)
Domain Layer:     Unchanged (business rules)
Aggregator Layer: Unchanged (orchestration)
Adapter Layer:    NEW implementations backed by SQLite instead of Cosmos Gophers/Inquisitors
```

### Adapter Project Structure

Existing adapter projects are renamed to include `.Cosmos`. New `.Sqlite` projects are created alongside them. Both implement the same adapter interfaces.

**Renamed projects** (existing Cosmos adapters):
- `Lib.Adapter.Cards` → **`Lib.Adapter.Cards.Cosmos`**
- `Lib.Adapter.Sets` → **`Lib.Adapter.Sets.Cosmos`**
- `Lib.Adapter.Artists` → **`Lib.Adapter.Artists.Cosmos`**
- `Lib.Adapter.User` → **`Lib.Adapter.User.Cosmos`**
- `Lib.Adapter.UserCards` → **`Lib.Adapter.UserCards.Cosmos`**
- `Lib.Adapter.UserSetCards` → **`Lib.Adapter.UserSetCards.Cosmos`**

**New SQLite adapter projects** (static data only):
- **`Lib.Adapter.Cards.Sqlite`**
- **`Lib.Adapter.Sets.Sqlite`**
- **`Lib.Adapter.Artists.Sqlite`**
- **`Lib.Adapter.SealedProducts.Sqlite`**

**New infrastructure project**:
- **`Lib.Sqlite`** - Infrastructure project (like `Lib.Cosmos` but for SQLite)
  - Connection management, read-only connection pool
  - Base query execution patterns
  - REGEXP function registration

**New search library**:
- **`Lib.Search.QueryParser`** - Standalone library
  - Lexer (tokenizer)
  - Parser (AST builder)
  - SQL translator (AST → SQLite query)
  - No infrastructure dependencies — pure logic

Aggregators update their references from `Lib.Adapter.Cards` to `Lib.Adapter.Cards.Cosmos` and/or `Lib.Adapter.Cards.Sqlite`.

### Static Data Source Configuration

A root-level `appsettings.json` key controls which adapters execute:

```json
{
  "StaticDataSource": "source_both"
}
```

**Valid values**: `"source_sqlite"`, `"source_cosmos"`, `"source_both"`

Config objects expose two boolean methods that interpret the value:

| Value | `IsSourceSqlite()` | `IsSourceCosmos()` |
|---|---|---|
| `"source_sqlite"` | `true` | `false` |
| `"source_cosmos"` | `false` | `true` |
| `"source_both"` | `true` | `true` |

Each adapter checks the method relevant to its implementation:
- Cosmos adapters call `IsSourceCosmos()` — if `false`, return empty/null-object response
- SQLite adapters call `IsSourceSqlite()` — if `false`, return empty/null-object response

This is self-governing: each adapter owns the decision of whether it executes. No routing layer, no aggregator awareness, no composite adapter. The config objects follow the existing pattern (like `auth_mode`).

**Deployment configurations**:
- **Ingestion pipeline**: `"source_both"` — writes to Cosmos and SQLite during migration; `"source_sqlite"` once migration is complete
- **Live GraphQL API**: `"source_cosmos"` initially; `"source_sqlite"` once verified; never `"source_both"` for reads (only one source serves data)

### SQLite File Lifecycle

1. **Generation**: During Scryfall ingestion (`BulkIngestionService`), generate `mtg-data.db`
2. **Storage**: Upload to Azure Blob Storage (pennies/month) or include in container image
3. **Loading**: Container App downloads on startup (or baked into image during CI/CD)
4. **Runtime**: Opened read-only, shared across all GraphQL query threads
5. **Updates**: New ingestion -> new SQLite file -> container restart or hot-swap

### Connection Management

```
- Open mode: ReadOnly
- Journal mode: OFF (read-only, no WAL needed)
- Cache: Shared (single connection, multiple readers)
- Thread safety: SQLite supports concurrent reads natively
- Memory: ~10-50MB for SQLite engine + page cache (well within 1GB container)
```

### Estimated SQLite File Size

- ~300K card rows across cards table + junction/type-keyed tables
- Fully normalized + denormalized display columns, no raw JSON blob
- Estimated total: ~200-400MB
- Compressed for transfer: ~50-100MB gzipped

---

## What Cannot Be Replicated from Scryfall

| Feature | Reason | Workaround |
|---|---|---|
| Tagger tags (`art:`, `function:`) | Community-curated, not in API data | None - skip |
| Cube data (`cube:`) | External dataset | None - skip |
| `prints=`, `sets=` (printing counts) | Requires aggregation | Pre-compute during ingestion as columns |
| Language-specific search (`lang:`) | Data is English-only | None unless multi-language ingestion added |
| `is:fetchland`, `is:dual`, etc. | Scryfall-specific classifications | Static mapping table (oracle text patterns or card name lists) |

---

## Implementation Phases

### Phase 0: Adapter Rename & Configuration
- Rename existing adapter projects: `Lib.Adapter.Cards` → `Lib.Adapter.Cards.Cosmos`, etc. (folders + .csproj files)
- Update all aggregator references to point to renamed `.Cosmos` projects
- Add `"StaticDataSource": "source_cosmos"` to `appsettings.json`
- Create config class with `IsSourceSqlite()` / `IsSourceCosmos()` methods (follows `auth_mode` pattern)
- Add config check to each existing Cosmos adapter (calls `IsSourceCosmos()`, no-ops if `false`)
- Verify everything works unchanged with `"source_cosmos"` config
- **No behavior change** — this phase only renames and adds the config plumbing

### Phase 1: SQLite Infrastructure & Card Data Migration
- Create `Lib.Sqlite` project with connection management
- Create `Lib.Adapter.Cards.Sqlite` project with config check (`IsSourceSqlite()`)
- Add SQLite generation step to ingestion pipeline (cards table + all junction/type-keyed tables + indexes)
- Implement SQLite card query adapters: `CardsByIds`, `CardsBySetCode`, `CardsByName`
- Add aggregator references to `Lib.Adapter.Cards.Sqlite`
- Ingestion: set `"source_both"` — writes to Cosmos and SQLite
- Live API: set `"source_sqlite"` — reads from SQLite, Cosmos adapters no-op
- Verify existing GraphQL card queries return identical results

### Phase 2: Sets, Artists, Remaining Static Data
- Add sets, artists, rulings, sealed_products tables to SQLite generation
- Create `Lib.Adapter.Sets.Sqlite`, `Lib.Adapter.Artists.Sqlite`, `Lib.Adapter.SealedProducts.Sqlite`
- Implement SQLite adapters for sets, artists, sealed products
- Add aggregator references to new `.Sqlite` projects
- **All static data reads now served by SQLite**

### Phase 3: FTS5 Search (replaces trigrams)
- Add FTS5 virtual tables to SQLite generation
- Implement FTS5-backed card name search and artist name search adapters in respective `.Sqlite` projects
- **All static Cosmos containers now serve investigation only; reduce RU/s or switch to serverless**

### Phase 4: Scryfall Query Parser - Core
- Create `Lib.Search.QueryParser` project
- Build lexer for field prefixes, operators, quoted strings, negation
- Build parser for AST construction
- Build SQL translator
- Support: `name:`, `o:`, `t:`, `c:`, `id:`, `r:`, `s:`, `f:`, `pow:`, `tou:`, `cmc:`, `a:`, `kw:`
- Add new GraphQL query: `advancedCardSearch(query: String)`
- Wire through Entry -> Domain -> Aggregator -> Adapter layers

### Phase 5: Scryfall Query Parser - Advanced
- Add OR, parentheses, `is:`/`not:` boolean flags
- Add color expansion (guild names, shard names)
- Add mana cost matching (`m:`)
- Add sorting (`order:`, `direction:`)
- Add regex support (register REGEXP function)
- Add exact name match (`!`)
- Add date/year, price filters

### Phase 6: Frontend Search UI
- Advanced search bar with Scryfall-style syntax
- Syntax help/autocomplete
- Faceted filter sidebar (color, rarity, set, format)
- Result pagination
- Visual query builder alternative

---

## Verification Plan

### Phase 1-3 (Migration)
- Run existing GraphQL queries against SQLite-backed adapters
- Compare results to Cosmos-backed results for same inputs
- Verify all card, set, artist data is present and correct
- Performance comparison: SQLite query time vs Cosmos query time
- Run `dotnet test` to ensure all existing tests pass

### Phase 4-5 (Search)
- Unit tests for lexer (input string -> expected tokens)
- Unit tests for parser (tokens -> expected AST)
- Unit tests for translator (AST -> expected SQL)
- Integration tests: run Scryfall syntax queries and verify results match expectations
- Test boolean logic combinations, edge cases, malformed input handling

### Phase 6 (Frontend)
- Manual testing of search UI with various query patterns
- Verify faceted filters work correctly
- Test pagination with large result sets
- Mobile responsiveness of search UI

---

## Cost Impact

**Before**: ~14 static containers at 400 RU/s each + 5 user containers at 400 RU/s each = ~7,600 RU/s provisioned
**After**:
- **SQLite**: Included in Container App cost (no additional charge)
- **Static Cosmos containers**: Retained for investigation only. Can drop to minimum RU/s (400 per container or switch account to serverless mode for pay-per-request). No production read traffic.
- **User Cosmos containers**: Unchanged (5 containers serving runtime reads/writes)
- **Blob Storage** (optional, for SQLite file hosting): ~$0.01/month

The primary savings come from eliminating production read traffic against the static containers, enabling a switch to serverless Cosmos pricing where you pay only for the ingestion writes.

---

## Key Files to Modify

### Renamed Projects (folders + .csproj files)
- `src/Lib.Adapter.Cards/` → `src/Lib.Adapter.Cards.Cosmos/`
- `src/Lib.Adapter.Sets/` → `src/Lib.Adapter.Sets.Cosmos/`
- `src/Lib.Adapter.Artists/` → `src/Lib.Adapter.Artists.Cosmos/`
- `src/Lib.Adapter.User/` → `src/Lib.Adapter.User.Cosmos/`
- `src/Lib.Adapter.UserCards/` → `src/Lib.Adapter.UserCards.Cosmos/`
- `src/Lib.Adapter.UserSetCards/` → `src/Lib.Adapter.UserSetCards.Cosmos/`

### New Projects
- `src/Lib.Sqlite/` — SQLite infrastructure (connection, query execution, REGEXP registration)
- `src/Lib.Adapter.Cards.Sqlite/` — Card query/write adapters backed by SQLite
- `src/Lib.Adapter.Sets.Sqlite/` — Set query adapters backed by SQLite
- `src/Lib.Adapter.Artists.Sqlite/` — Artist query adapters backed by SQLite
- `src/Lib.Adapter.SealedProducts.Sqlite/` — Sealed product query adapters backed by SQLite
- `src/Lib.Search.QueryParser/` — Scryfall syntax parser (lexer, parser, SQL translator)

### Configuration
- `appsettings.json` — Add `"StaticDataSource"` root key (`"source_sqlite"`, `"source_cosmos"`, `"source_both"`)
- Config class with `IsSourceSqlite()` and `IsSourceCosmos()` methods (follows existing `auth_mode` pattern)

### Aggregator Reference Updates
- All aggregator projects update references from `Lib.Adapter.Cards` to `Lib.Adapter.Cards.Cosmos` (and add `Lib.Adapter.Cards.Sqlite` where applicable)
- Same for Sets, Artists, SealedProducts aggregators

### Ingestion Pipeline
- `src/Lib.Scryfall.Ingestion/BulkIngestion/BulkIngestionService.cs` — Add SQLite generation step

### GraphQL (new endpoint only)
- `src/App.MtgDiscovery.GraphQL/Queries/` — Add advanced search query method

### Unchanged
- All Entry, Domain layer code (interfaces preserved)
- All frontend code (until Phase 6)
- User-data Cosmos adapters (UserCards, UserSetCards) — renamed but not duplicated to SQLite
