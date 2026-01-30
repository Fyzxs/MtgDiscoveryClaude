# Lib.Scryfall.Ingestion CLAUDE.md

## Purpose
Handles ingestion of Magic: The Gathering card and set data from the Scryfall API into Azure Cosmos DB storage, supporting both standard API-based ingestion and bulk processing capabilities.

## Architecture Overview
The service implements a layered ingestion pipeline that fetches MTG data from Scryfall, transforms it into domain objects, processes aggregations, and persists to Cosmos DB. The architecture supports configurable processing modes and batch operations for efficient data management.

## Module Structure
- `Apis/` - Public service interfaces and implementations
  - `ScryfallIngestionService.cs` - Main entry point for ingestion operations
  - `Collections/` - Filtered set collection implementations
- `BulkProcessing/` - Bulk data processing infrastructure (planned)
  - `Models/` - Bulk processing data models
  - `Storage/` - Bulk data storage abstractions
  - `Fetchers/` - Bulk data retrieval components
  - `Loaders/` - Bulk data loading utilities
  - `Processors/` - Bulk processing orchestration
  - `Writers/` - Bulk data persistence
  - `Services/` - Orchestration services
- `Collections/` - Data collection abstractions
  - `HttpScryfallCollection.cs` - Base HTTP collection
  - `HttpScryfallCardCollection.cs` - Card-specific collection
  - `HttpScryfallSetCollection.cs` - Set-specific collection
- `Configurations/` - Service configuration objects
  - `ConfigScryfallIngestionConfiguration.cs:18-35` - Main configuration
  - Processing flags and batch settings
- `Processors/` - Data processing pipeline
  - `BatchSetProcessor.cs:59-92` - Batch processing orchestration
  - `SetProcessor.cs:36-47` - Individual set processing
  - `CardProcessor.cs` - Card data processing
  - Artist and trigram aggregation processors
- `Http/` - HTTP client and rate limiting
  - `ScryfallHttpClient.cs` - Scryfall API client
  - `RateLimitedHttpClient.cs` - Rate-limited HTTP operations
  - `ScryfallRateLimiter.cs` - Rate limiting implementation
- `Filters/` - Set filtering logic
  - `NonDigitalSetFilter.cs` - Excludes digital-only sets
  - `SpecificSetsFilter.cs` - Processes only specified sets
- `Mappers/` - Data transformation
  - Scryfall DTO to Cosmos model mapping
- `Models/` - Domain models for ingestion
- `Dtos/` - Data transfer objects for Scryfall API

## Key Components
### Main Service (Apis/ScryfallIngestionService.cs)
- `IngestAllSetsAsync:37-59` - Primary ingestion orchestration
- Collects all sets and delegates to batch processor
- Handles top-level logging and error management

### Batch Processing (Processors/BatchSetProcessor.cs)
- `ProcessSetsAsync:59-92` - Processes sets in configurable batches
- Supports set-only mode and full processing mode
- Handles artist aggregation and trigram updates
- Individual set error isolation

### HTTP Client Infrastructure (Http/)
- `ScryfallHttpClient.cs` - Scryfall API integration
- `RateLimitedHttpClient.cs` - Polly-based rate limiting
- `ScryfallRateLimiter.cs` - Token-based rate limiting

### Processing Pipeline
- Set processing: Items, associations, code indexing
- Card processing: Items, images, artist relationships
- Aggregation: Artist data and card name trigrams
- Persistence: Cosmos DB integration via adapters

## Dependencies
- External: Polly, Polly.RateLimiting, Newtonsoft.Json
- Internal: Lib.Adapter.Scryfall.Cosmos, Lib.BlobStorage, Lib.Cosmos
- Shared: Lib.Scryfall.Shared, Lib.Shared.Abstractions, Lib.Universal

## Configuration
- Main config: `IScryfallIngestionConfiguration`
- Processing modes: Set-only vs full processing
- Batch settings: Size, reverse order, specific sets
- Rate limiting: Built into HTTP client layer
- Environment: See Lib.Universal configuration patterns

## Bulk Processing Capabilities (Planned)
The BulkProcessing subfolder provides infrastructure for:
- **Models**: Bulk operation data structures
- **Storage**: Bulk data storage abstractions
- **Fetchers**: Bulk data retrieval from Scryfall
- **Loaders**: Bulk data loading and parsing
- **Processors**: Bulk processing orchestration
- **Writers**: Efficient bulk persistence
- **Services**: High-level bulk operation coordination

## Processing Modes
### Standard API Processing
- Fetches data via Scryfall REST API
- Processes sets individually with rate limiting
- Handles incremental updates
- Configured via `ProcessOnlySetItems` flag

### Bulk Processing (Planned)
- Downloads bulk data files from Scryfall
- Processes large datasets efficiently
- Minimizes API calls and rate limiting concerns
- Ideal for initial data loads or full refreshes

## Integration Points
### Consumes
- Scryfall API: Set and card data
- Scryfall CDN: Card images and set icons
- Configuration: Processing settings and connection strings

### Provides
- Cosmos DB: Persisted card and set data
- Blob Storage: Card images and set icons
- Aggregated data: Artist relationships, card name trigrams

## Key Patterns
- Rate-limited HTTP client with Polly policies
- Batch processing with error isolation
- Configurable processing modes
- MicroObjects pattern throughout (see CODING_CRITERIA.md)
- Artist aggregation with dirty tracking
- Pipeline-based data transformation

## Testing
- Test project: `Lib.Scryfall.Ingestion.Tests/`
- Example applications: `Example.Scryfall.CosmosIngestion/`
- Run tests: See CLAUDE.md build commands

## Related Documentation
- `CODING_CRITERIA.md` - MicroObjects patterns used throughout
- `src/Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Cosmos data models
- `src/Lib.BlobStorage/CLAUDE.md` - Image storage integration
- `src/Lib.Scryfall.Shared/` - Shared Scryfall abstractions