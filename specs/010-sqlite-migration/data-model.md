# Data Model: SQLite Migration & Scryfall-Level Search

**Phase 1 Output** | Generated: 2026-01-26

## Overview

This document maps the SQLite schema (defined in the design doc) to the existing .NET entity layer, and defines the new entity types needed for SQLite adapter implementations.

## Entity Type Mapping

### Existing Entity Flow (Cosmos Path)

```
GraphQL Request → ArgEntity → ItrEntity → XfrEntity → [Cosmos Gopher/Inquisitor] → ExtEntity → OufEntity → OutEntity → GraphQL Response
```

### New Entity Flow (SQLite Path)

```
GraphQL Request → ArgEntity → ItrEntity → XfrEntity → [SQLite Query] → SqlEntity → OufEntity → OutEntity → GraphQL Response
```

The only difference is at the adapter layer: `ExtEntity` (Cosmos documents with `dynamic Data`) is replaced by `SqlEntity` (strongly-typed row mappings from SQLite). The `XfrEntity` input and `OufEntity`/`ItrEntity` output interfaces remain identical, ensuring the aggregator layer is unaware of the storage backend.

---

## SQLite Entity Definitions (SqlEntity)

These are the new entity types that map directly to SQLite table rows. They replace the `dynamic Data` pattern used by Cosmos `ExtEntity` classes.

### CardSqlEntity

**Source table**: `cards`
**Replaces**: `ScryfallCardItemExtEntity.Data` (dynamic), `ScryfallSetCardItemExtEntity.Data` (dynamic), `ScryfallCardByNameExtEntity.Data` (dynamic)

| Property | Type | SQLite Column | Notes |
|----------|------|---------------|-------|
| Id | string | `id` | PK, TEXT |
| OracleId | string | `oracle_id` | |
| ArenaId | int? | `arena_id` | |
| MtgoId | int? | `mtgo_id` | |
| MtgoFoilId | int? | `mtgo_foil_id` | |
| TcgplayerId | int? | `tcgplayer_id` | |
| TcgplayerEtchedId | int? | `tcgplayer_etched_id` | |
| CardmarketId | int? | `cardmarket_id` | |
| Name | string | `name` | NOT NULL |
| NameLower | string | `name_lower` | Computed at ingestion |
| FlavorName | string | `flavor_name` | |
| Lang | string | `lang` | |
| PrintedName | string | `printed_name` | |
| PrintedTypeLine | string | `printed_type_line` | |
| PrintedText | string | `printed_text` | |
| Layout | string | `layout` | |
| ManaCost | string | `mana_cost` | |
| Cmc | double? | `cmc` | REAL |
| TypeLine | string | `type_line` | |
| OracleText | string | `oracle_text` | |
| FlavorText | string | `flavor_text` | |
| Power | string | `power` | Handles '*', 'X' |
| PowerNumeric | double? | `power_numeric` | Parsed at ingestion |
| Toughness | string | `toughness` | |
| ToughnessNumeric | double? | `toughness_numeric` | |
| Loyalty | string | `loyalty` | |
| LoyaltyNumeric | double? | `loyalty_numeric` | |
| Defense | string | `defense` | |
| DefenseNumeric | double? | `defense_numeric` | |
| LifeModifier | string | `life_modifier` | |
| HandModifier | string | `hand_modifier` | |
| SetId | string | `set_id` | NOT NULL |
| SetCode | string | `set_code` | NOT NULL |
| SetName | string | `set_name` | |
| SetType | string | `set_type` | |
| SetGroupId | string | `set_group_id` | App-computed |
| CollectorNumber | string | `collector_number` | |
| Rarity | string | `rarity` | |
| RarityOrd | int? | `rarity_ord` | 0-5 ordinal |
| Artist | string | `artist` | Primary artist |
| IllustrationId | string | `illustration_id` | |
| CardBackId | string | `card_back_id` | |
| BorderColor | string | `border_color` | |
| Frame | string | `frame` | |
| Watermark | string | `watermark` | |
| SecurityStamp | string | `security_stamp` | |
| FullArt | bool | `full_art` | INTEGER 0/1 |
| Textless | bool | `textless` | |
| Digital | bool | `digital` | |
| Reserved | bool | `reserved` | |
| Foil | bool | `foil` | |
| NonFoil | bool | `non_foil` | |
| Reprint | bool | `reprint` | |
| Promo | bool | `promo` | |
| Oversized | bool | `oversized` | |
| Variation | bool | `variation` | |
| Booster | bool | `booster` | |
| StorySpotlight | bool | `story_spotlight` | |
| GameChanger | bool | `game_changer` | |
| ContentWarning | bool | `content_warning` | |
| ImageStatus | string | `image_status` | |
| HighResImage | bool | `high_res_image` | |
| EdhrecRank | int? | `edhrec_rank` | |
| PennyRank | int? | `penny_rank` | |
| ColorCount | int? | `color_count` | Computed |
| IdentityCount | int? | `identity_count` | Computed |
| IsSpell | bool | `is_spell` | Computed |
| IsPermanent | bool | `is_permanent` | Computed |
| IsVanilla | bool | `is_vanilla` | Computed |
| ImageUriSmall | string | `image_uri_small` | Denormalized |
| ImageUriNormal | string | `image_uri_normal` | Denormalized |
| ImageUriLarge | string | `image_uri_large` | Denormalized |
| ImageUriPng | string | `image_uri_png` | Denormalized |
| ImageUriArtCrop | string | `image_uri_art_crop` | Denormalized |
| ImageUriBorderCrop | string | `image_uri_border_crop` | Denormalized |
| PriceUsd | double? | `price_usd` | Denormalized |
| PriceUsdFoil | double? | `price_usd_foil` | Denormalized |
| PriceUsdEtched | double? | `price_usd_etched` | Denormalized |
| PriceEur | double? | `price_eur` | Denormalized |
| PriceEurFoil | double? | `price_eur_foil` | Denormalized |
| PriceTix | double? | `price_tix` | Denormalized |
| Uri | string | `uri` | Scryfall API URI |
| ScryfallUri | string | `scryfall_uri` | Scryfall web URI |
| RulingsUri | string | `rulings_uri` | |
| PrintsSearchUri | string | `prints_search_uri` | |
| SetUri | string | `set_uri` | |
| SetSearchUri | string | `set_search_uri` | |
| ScryfallSetUri | string | `scryfall_set_uri` | |
| ArtistIds | string | `artist_ids` | JSON array |
| MultiverseIds | string | `multiverse_ids` | JSON array |
| Colors | string | `colors` | JSON array |
| ColorIdentity | string | `color_identity` | JSON array |
| ColorIndicator | string | `color_indicator` | JSON array |
| Keywords | string | `keywords` | JSON array |
| ProducedMana | string | `produced_mana` | JSON array |
| Finishes | string | `finishes` | JSON array |
| PromoTypes | string | `promo_types` | JSON array |
| FrameEffects | string | `frame_effects` | JSON array |
| Games | string | `games` | JSON array |
| AttractionLights | string | `attraction_lights` | JSON array |
| Legalities | string | `legalities` | JSON object |
| PurchaseUris | string | `purchase_uris` | JSON object |
| RelatedUris | string | `related_uris` | JSON object |
| Preview | string | `preview` | JSON object |
| CardFaces | string | `card_faces` | JSON array |
| AllParts | string | `all_parts` | JSON array |

### SetSqlEntity

**Source table**: `sets`
**Replaces**: `ScryfallSetItemExtEntity.Data` (dynamic), `ScryfallSetCodeIndexExtEntity`

| Property | Type | SQLite Column | Notes |
|----------|------|---------------|-------|
| Id | string | `id` | PK, TEXT |
| Code | string | `code` | UNIQUE, NOT NULL |
| Name | string | `name` | NOT NULL |
| TcgplayerId | int? | `tcgplayer_id` | |
| SetType | string | `set_type` | |
| ReleasedAt | string | `released_at` | |
| BlockCode | string | `block_code` | |
| Block | string | `block` | |
| ParentSetCode | string | `parent_set_code` | |
| CardCount | int? | `card_count` | |
| PrintedSize | int? | `printed_size` | |
| Digital | bool | `digital` | INTEGER 0/1 |
| FoilOnly | bool | `foil_only` | |
| NonfoilOnly | bool | `nonfoil_only` | |
| Uri | string | `uri` | |
| ScryfallUri | string | `scryfall_uri` | |
| SearchUri | string | `search_uri` | |
| IconSvgUri | string | `icon_svg_uri` | |
| Groupings | string | `groupings` | JSON array |

### ArtistSqlEntity

**Source table**: `artists`
**Replaces**: `ScryfallArtistExtEntity` / `ArtistAggregateExtEntity`

| Property | Type | SQLite Column | Notes |
|----------|------|---------------|-------|
| Id | string | `id` | PK, TEXT |
| Name | string | `name` | NOT NULL |
| NameLower | string | `name_lower` | NOT NULL |
| CardCount | int? | `card_count` | Pre-computed |
| SetCount | int? | `set_count` | Pre-computed |

### RulingSqlEntity

**Source table**: `rulings`

| Property | Type | SQLite Column | Notes |
|----------|------|---------------|-------|
| Id | string | `id` | PK, TEXT |
| OracleId | string | `oracle_id` | NOT NULL |
| Source | string | `source` | |
| PublishedAt | string | `published_at` | |
| Comment | string | `comment` | |

### SealedProductSqlEntity

**Source table**: `sealed_products`
**Replaces**: `SealedProductExtEntity`

| Property | Type | SQLite Column | Notes |
|----------|------|---------------|-------|
| Id | string | `id` | PK, TEXT (uuid) |
| TcgplayerProductId | string | `tcgplayer_product_id` | |
| McmId | string | `mcm_id` | |
| CardtraderId | string | `cardtrader_id` | |
| Name | string | `name` | NOT NULL |
| Category | string | `category` | |
| Subtype | string | `subtype` | |
| CardCount | int? | `card_count` | |
| ReleaseDate | string | `release_date` | |
| SetId | string | `set_id` | |
| SetCode | string | `set_code` | |
| SetName | string | `set_name` | |
| ImageUrl | string | `image_url` | |
| PurchaseUrlTcgplayer | string | `purchase_url_tcgplayer` | |
| PurchaseUrlCardmarket | string | `purchase_url_cardmarket` | |
| PurchaseUrlCardKingdom | string | `purchase_url_card_kingdom` | |

---

## Configuration Entity

### StaticDataSourceConfig

**Source**: `appsettings.json` root key `"StaticDataSource"`
**Pattern**: Follows existing `ConfigCosmosClientAuthMode` pattern (string value -> behavior methods)

| Method | Returns | Behavior |
|--------|---------|----------|
| `IsSourceSqlite()` | bool | `true` when value is `"source_sqlite"` or `"source_both"` |
| `IsSourceCosmos()` | bool | `true` when value is `"source_cosmos"` or `"source_both"` |

**Valid values**: `"source_sqlite"`, `"source_cosmos"`, `"source_both"`

**Self-Governing Adapter Pattern**: Both adapters are always called by the aggregator. Each adapter checks its own config method (`IsSourceSqlite()` / `IsSourceCosmos()`) and either executes or returns a null-object response. The aggregator does not decide which adapter to call.

**Merge Semantics (`source_both`)**: When both adapters return data, SQLite results take precedence. Cosmos results are used only if SQLite returns an empty/null-object response. This ensures the adapter layer merges transparently — no layer above is aware of the dual-source configuration.

---

## Query Parser AST Entities

### Search Node Hierarchy (Lib.Search.QueryParser)

All nodes implement `ISearchNode` with `Accept(ISearchNodeVisitor<T>)`.

| Interface | Properties | Description |
|-----------|-----------|-------------|
| `ISearchNode` | - | Base interface for all AST nodes |
| `IAndNode` | `IReadOnlyList<ISearchNode> Children` | Logical AND |
| `IOrNode` | `IReadOnlyList<ISearchNode> Children` | Logical OR |
| `INotNode` | `ISearchNode Child` | Logical NOT |
| `IFieldComparisonNode` | `ISearchField Field`, `IComparisonOperator Operator`, `string Value` | e.g., `cmc>=3` |
| `ITextSearchNode` | `string SearchText` | Free-text FTS5 search |
| `IFieldTextNode` | `ISearchField Field`, `string SearchText` | Field-scoped FTS5, e.g., `o:"draw a card"` |

### Token Hierarchy (Lexer Output)

Each token implements `IToken` with `ITokenKind Kind`, `string Value`, `int Position`.

| Token Kind | Example Input | Description |
|------------|--------------|-------------|
| `FieldPrefix` | `name:` | Field name followed by colon |
| `ComparisonOperator` | `>=`, `<=`, `>`, `<`, `=` | Numeric comparison |
| `QuotedString` | `"draw a card"` | Double-quoted text |
| `BareWord` | `bolt` | Unquoted text |
| `BooleanOr` | `OR` | Explicit OR keyword |
| `BooleanAnd` | `AND` | Explicit AND keyword |
| `BooleanNot` | `NOT` | Explicit NOT keyword |
| `NegationPrefix` | `-` | Unary negation |
| `OpenParen` | `(` | Group open |
| `CloseParen` | `)` | Group close |
| `ExactMatch` | `!` | Exact name match prefix |

---

## Adapter Interface Mapping

### Cards (Lib.Adapter.Cards)

Existing interfaces remain unchanged. Both `.Cosmos` and `.Sqlite` implement the same interface.

| Interface Method | Cosmos Implementation | SQLite Implementation |
|-----------------|----------------------|-----------------------|
| `GetCardsByIdsAsync(ICardIdsXfrEntity)` | Gopher point reads → `ScryfallCardItemExtEntity` | `SELECT * FROM cards WHERE id IN (?)` → `CardSqlEntity` |
| `GetCardsBySetCodeAsync(ISetCodeXfrEntity)` | SetCodeIndex + Inquisitor → `ScryfallSetCardItemExtEntity` | `SELECT * FROM cards WHERE set_code = ?` → `CardSqlEntity` |
| `GetCardsByNameAsync(ICardNameXfrEntity)` | Inquisitor → `ScryfallCardByNameExtEntity` | `SELECT * FROM cards WHERE name_lower = ?` → `CardSqlEntity` |
| `SearchCardNamesAsync(ICardSearchTermXfrEntity)` | Trigram search → `CardNameTrigramExtEntity` | `SELECT * FROM card_names_fts WHERE MATCH ?` → names |

### Sets (Lib.Adapter.Sets)

| Interface Method | Cosmos Implementation | SQLite Implementation |
|-----------------|----------------------|-----------------------|
| `SetsByIdsAsync(ISetIdsXfrEntity)` | Gopher point reads → `ScryfallSetItemExtEntity` | `SELECT * FROM sets WHERE id IN (?)` → `SetSqlEntity` |
| `SetsByCodesAsync(ISetCodesXfrEntity)` | SetCodeIndex + Gopher → `ScryfallSetItemExtEntity` | `SELECT * FROM sets WHERE code IN (?)` → `SetSqlEntity` |
| `AllSetsAsync(IAllSetsXfrEntity)` | Inquisitor → `ScryfallSetItemExtEntity` | `SELECT * FROM sets` → `SetSqlEntity` |

### Artists (Lib.Adapter.Artists)

| Interface Method | Cosmos Implementation | SQLite Implementation |
|-----------------|----------------------|-----------------------|
| `SearchArtistsAsync(IArtistSearchTermXfrEntity)` | Trigram search → `ArtistNameTrigramDataExtEntity` | `SELECT * FROM artist_names_fts WHERE MATCH ?` → `ArtistSqlEntity` |
| `CardsByArtistIdAsync(IArtistIdXfrEntity)` | Inquisitor → `ScryfallArtistCardExtEntity` | `SELECT c.* FROM cards c JOIN card_artists ca ... WHERE ca.artist_id = ?` → `CardSqlEntity` |
| `CardsByArtistNameAsync(IArtistNameXfrEntity)` | Trigram + ArtistCards → `ScryfallArtistCardExtEntity` | FTS5 MATCH on artist_names_fts + JOIN → `CardSqlEntity` |

### Sealed Products (Lib.Adapter.SealedProducts)

| Interface Method | Cosmos Implementation | SQLite Implementation |
|-----------------|----------------------|-----------------------|
| `SealedProductsBySetCodeAsync(ISealedProductsBySetCodeXfrEntity)` | SetCodeIndex + Inquisitor → `SealedProductExtEntity` | `SELECT * FROM sealed_products WHERE set_code = ?` → `SealedProductSqlEntity` |

---

## Relationships

```
cards ──┤ 1:N ├── card_faces
cards ──┤ N:M ├── artists        (via card_artists)
cards ──┤ 1:N ├── card_colors
cards ──┤ 1:N ├── card_color_identity
cards ──┤ 1:N ├── card_keywords
cards ──┤ 1:N ├── card_produced_mana
cards ──┤ 1:N ├── card_finishes
cards ──┤ 1:N ├── card_promo_types
cards ──┤ 1:N ├── card_games
cards ──┤ 1:N ├── card_frame_effects
cards ──┤ 1:N ├── card_parts
cards ──┤ 1:N ├── card_multiverse_ids
cards ──┤ 1:N ├── card_images          (via image_types)
cards ──┤ 1:N ├── card_legalities
cards ──┤ 1:N ├── card_prices
cards ──┤ 1:N ├── card_uris
cards ──┤ N:1 ├── sets                 (via set_id)

card_faces ──┤ 1:N ├── card_face_colors
card_faces ──┤ 1:N ├── card_face_images

sets ──┤ N:M ├── artists              (via set_artists)
sets ──┤ 1:N ├── set_associations
sets ──┤ 1:N ├── sealed_products      (via set_code)

rulings ──┤ N:1 ├── cards              (via oracle_id, logical FK)
```

---

## Validation Rules

### Ingestion-Time Validation (Write Path)
- `cards.id` and `sets.id` are UUIDs from Scryfall (always present)
- `cards.name` and `sets.name` are NOT NULL (enforced by schema)
- `cards.set_code` and `cards.set_id` are NOT NULL (every card belongs to a set)
- `sets.code` is UNIQUE (enforced by schema)
- Boolean columns store 0/1 as INTEGER (SQLite has no native boolean)
- JSON columns validated during ingestion (well-formed JSON TEXT)
- Computed columns (`power_numeric`, `rarity_ord`, `color_count`, etc.) derived at ingestion time

### Runtime Validation (Read Path)
- XfrEntity input validated at adapter boundary (null checks per constitution)
- FTS5 MATCH expressions sanitized before execution (double-quote wrapping)
- Search query parser validates syntax and produces structured errors
- SQLite `query_only=ON` pragma prevents accidental writes
