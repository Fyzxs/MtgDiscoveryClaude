---
task: m-implement-bulk-ingestion-set-groupings
branch: feature/bulk-ingestion-set-groupings
status: completed
created: 2025-09-02
modules: [Lib.Scryfall.Ingestion, Lib.Adapter.Scryfall.Cosmos]
---

# Implement Set Groupings for Bulk Ingestion

## Problem/Goal
The bulk ingestion process (BatchSetProcessor and SetProcessor) currently doesn't utilize the scryfall_set_groupings.json file, while the Cosmos ingestion flow already does through ScryfallSetToCosmosMapper. This creates inconsistency in how set data is enriched with grouping information across different ingestion pathways.

## Success Criteria
- [ ] BatchSetProcessor and SetProcessor use the MonoStateSetGroupingsLoader to enrich set data
- [ ] Set groupings are consistently applied across all ingestion pathways
- [ ] Existing Cosmos ingestion functionality remains unchanged
- [ ] Unit tests verify groupings are properly attached to set data during bulk processing
- [ ] The scryfall_set_groupings.json file is loaded once and cached appropriately

## Context Files
<!-- Added by context-gathering agent or manually -->
- src/Lib.Scryfall.Ingestion/Processors/BatchSetProcessor.cs
- src/Lib.Scryfall.Ingestion/Processors/SetProcessor.cs
- src/Lib.Scryfall.Ingestion/Processors/SetItemsOnlyProcessor.cs
- src/Lib.Scryfall.Ingestion/Services/MonoStateSetGroupingsLoader.cs
- src/Lib.Scryfall.Ingestion/Mappers/ScryfallSetToCosmosMapper.cs  # Reference implementation
- src/Example.Scryfall.CosmosIngestion/scryfall_set_groupings.json  # Source data file

## Context Manifest

### How Bulk Ingestion Currently Works: BatchSetProcessor and SetProcessor Flow

The bulk ingestion system processes Magic: The Gathering sets in batches through a hierarchical processor architecture. When a bulk ingestion operation begins, the entry point is the `BatchSetProcessor`, which orchestrates the entire process by dividing sets into configurable batches and coordinating multiple sub-processors.

The `BatchSetProcessor` receives a collection of `IScryfallSet` objects and processes them in batches based on the `SetBatchSize` configuration from `IScryfallProcessingConfig`. For each batch, it iterates through each set and delegates processing to either a `SetProcessor` (full processing mode) or `SetItemsOnlyProcessor` (set metadata only mode), determined by the `ProcessOnlySetItems` configuration flag. This architectural decision allows the system to support both lightweight set metadata ingestion and full set+card data processing depending on operational requirements.

The `SetProcessor` coordinates multiple specialized processors in a specific sequence. First, it processes set metadata through three parallel processors: `SetItemsProcessor` (which handles the core set data persistence), `SetAssociationsProcessor` (which manages parent-child set relationships), and `SetCodeIndexProcessor` (which maintains search indices). After set metadata processing completes, it processes all cards within the set through the `CardProcessor`, fetching them asynchronously via the `IScryfallSet.Cards()` method which makes API calls to Scryfall's search endpoint.

The critical architectural boundary for our implementation is the `SetItemsProcessor`, which is responsible for persisting set metadata to Cosmos DB. This processor uses the `ScryfallSetToCosmosMapper` to transform `IScryfallSet` objects into `ScryfallSetItem` entities before writing them to storage via the `ScryfallSetItemsScribe`. This is where set groupings are currently applied in the Cosmos ingestion flow but are missing in the bulk ingestion flow.

### How Set Groupings Currently Work: ScryfallSetToCosmosMapper Integration

Set groupings are currently implemented only in the Cosmos ingestion pathway through the `ScryfallSetToCosmosMapper`. When the mapper's `Map(IScryfallSet scryfallSet)` method is called, it extracts the set code using `scryfallSet.Code()` and queries the `MonoStateSetGroupingsLoader` to retrieve grouping data via `GetGroupingsForSet(setCode)`.

The mapper follows a conditional enrichment pattern: if grouping data exists for the set code, it converts the `IScryfallSet.Data()` dynamic object to a `JObject` using Newtonsoft.Json, then adds a "groupings" property containing the `SetGroupingData.Groupings` collection. This approach preserves the original Scryfall data structure while augmenting it with locally-defined card groupings that organize cards within sets by visual themes, rarity patterns, or collector categories.

The transformation is non-destructive - the original set data remains intact, and groupings are additive. If no groupings exist for a set code, the mapper returns the set data unmodified, ensuring backward compatibility with sets that don't have custom groupings defined.

### The MonoState Pattern and SetGroupingsLoader Architecture

The set groupings system implements the MonoState pattern through `MonoStateSetGroupingsLoader`, which provides a singleton-like interface while maintaining the flexibility of dependency injection. The MonoState wraps a static instance of `SetGroupingsLoader` that loads the `scryfall_set_groupings.json` file once during application startup and caches the data in memory.

The underlying `SetGroupingsLoader` constructor takes a file path parameter ("scryfall_set_groupings.json") and immediately loads all grouping data into a private `Dictionary<string, SetGroupingData>` field. The loading process reads the JSON file using `File.ReadAllText()` and deserializes it with `JsonConvert.DeserializeObject<Dictionary<string, SetGroupingData>>()`. Error handling is defensive - if the file doesn't exist, it returns an empty dictionary, allowing the system to operate gracefully without groupings.

The MonoState pattern ensures that the expensive file I/O and JSON deserialization only occurs once per application instance, regardless of how many `MonoStateSetGroupingsLoader` instances are created. This is critical for bulk processing scenarios where thousands of sets might be processed, as it prevents repeated file system access.

### Structure and Purpose of scryfall_set_groupings.json

The `scryfall_set_groupings.json` file is a dictionary mapping set codes to `SetGroupingData` objects. Each `SetGroupingData` contains a `SetCode` string and a `Groupings` array of `CardGrouping` objects. Each `CardGrouping` represents a thematic collection of cards within a set, such as "Alternate-Art Borderless Cards" or "Showcase Cards".

The `CardGrouping` structure includes an `Id` (unique identifier within the set), `DisplayName` (human-readable category name), `Order` (for UI sorting), `CardCount` (number of cards in the grouping), and `RawQuery` (Scryfall search syntax). The `ParsedFilters` property contains structured filter data with `CollectorNumberRange` for numeric ranges and `Properties` dictionary for card attributes like frame effects, border styles, or special treatments.

For example, the "tla" set defines groupings for borderless cards (excluding planeswalkers), showcase frame cards, neon ink treatments, and extended art variants. Each grouping captures both the search criteria (RawQuery: "set:tla border:borderless -type:planeswalker") and structured filters (Properties: {"border": "borderless", "type_line_excludes": "planeswalker"}) to support different query mechanisms.

### Where Set Data Should Be Enriched in Bulk Flow

The bulk ingestion flow currently bypasses set groupings because the `BatchSetProcessor` and `SetProcessor` delegate to specialized processors that don't use the groupings-aware `ScryfallSetToCosmosMapper`. The `SetItemsProcessor` creates its own instance of `ScryfallSetToCosmosMapper` in its constructor, which already includes groupings support, but this only affects the Cosmos storage pathway.

To achieve consistency across ingestion pathways, set groupings enrichment should occur at the earliest possible point in the data flow - ideally within the `IScryfallSet` transformation or mapping layer. However, since `IScryfallSet` represents the raw Scryfall API data contract, the most appropriate integration point is ensuring that all mappers and processors that transform set data consistently apply groupings enrichment.

The key insight is that the `SetItemsProcessor` already uses `ScryfallSetToCosmosMapper`, which includes groupings support. The disconnect occurs because bulk ingestion and Cosmos ingestion are separate code paths, but they converge at the same mapper. Therefore, the bulk ingestion flow already has access to set groupings - the issue is ensuring that all pathways through the system utilize the same enrichment logic.

### For New Feature Implementation: Ensuring Consistency Across Pathways

Since the `SetItemsProcessor` already uses the `ScryfallSetToCosmosMapper` which includes groupings support through the `MonoStateSetGroupingsLoader`, the bulk ingestion flow should already be applying set groupings to set data before persisting to Cosmos DB. The task description suggests there's an inconsistency, but examining the code shows that both the bulk and Cosmos ingestion pathways converge on the same mapper.

To verify and ensure consistent behavior, we need to create comprehensive unit tests that confirm groupings are properly attached during bulk processing. The testing approach should create scenarios with sets that have groupings defined in the JSON file and verify that the resulting `ScryfallSetItem` entities contain the enriched data with the "groupings" property.

If there is indeed a gap in the bulk flow, it would likely be in processors or pathways that don't use the `ScryfallSetToCosmosMapper`, or in test scenarios where the `scryfall_set_groupings.json` file isn't available. The solution would involve ensuring that all set transformation logic consistently uses the `MonoStateSetGroupingsLoader` to enrich set data with groupings where appropriate.

The MonoState pattern ensures efficient resource usage - the JSON file is loaded once and cached throughout the application lifecycle, making it safe to use across all processors without performance concerns. The key architectural requirement is maintaining the separation of concerns: groupings are additive metadata that enhance set data without changing core ingestion logic.

### Technical Reference Details

#### Component Interfaces & Signatures

```csharp
// Core interfaces for set processing
interface ISetProcessor
{
    Task ProcessAsync(IScryfallSet set);
}

interface IBatchSetProcessor
{
    Task ProcessSetsAsync(IEnumerable<IScryfallSet> sets);
}

// Set data transformation
interface IScryfallSetToCosmosMapper
{
    ScryfallSetItem Map(IScryfallSet scryfallSet);
}

// Groupings loading
interface ISetGroupingsLoader
{
    SetGroupingData GetGroupingsForSet(string setCode);
    bool HasGroupingsForSet(string setCode);
}
```

#### Data Structures

```csharp
// Set grouping data model
internal sealed class SetGroupingData
{
    public string SetCode { get; set; }
    public List<CardGrouping> Groupings { get; set; }
}

internal sealed class CardGrouping
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
    public int CardCount { get; set; }
    public string RawQuery { get; set; }
    public GroupingFilters ParsedFilters { get; set; }
}
```

#### Configuration Requirements

- `ProcessOnlySetItems`: Boolean flag determining whether to process only set metadata or include cards
- `SetBatchSize`: Integer controlling batch size for bulk processing
- File path: "scryfall_set_groupings.json" (hardcoded in MonoStateSetGroupingsLoader)

#### File Locations

- Core processors: `/src/Lib.Scryfall.Ingestion/Processors/`
- Mappers with groupings support: `/src/Lib.Scryfall.Ingestion/Mappers/ScryfallSetToCosmosMapper.cs`
- MonoState loader: `/src/Lib.Scryfall.Ingestion/Services/MonoStateSetGroupingsLoader.cs`
- Groupings data: `/src/Example.Scryfall.CosmosIngestion/scryfall_set_groupings.json`
- Test fakes: `/src/Lib.Scryfall.Ingestion.Tests/Fakes/`

## User Notes
- The scryfall_set_groupings.json file already exists and is working with Cosmos ingestion
- The implementation in ScryfallSetToCosmosMapper can serve as a reference
- Ensure the MonoState pattern is properly utilized for efficient resource usage

## Work Log
<!-- Updated as work progresses -->
- [2025-09-02] Task created, ready for context gathering
- [2025-09-02] Task completed - Modified ScryfallSetDtoFactory to inject groupings data using MonoStateSetGroupingsLoader