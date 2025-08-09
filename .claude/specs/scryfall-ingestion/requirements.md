# Requirements: Scryfall Ingestion

## Introduction

### Overview
The Scryfall Ingestion feature provides a robust system for consuming, caching, and storing Magic: The Gathering card data from the Scryfall API. This feature serves as the foundation for all card-related functionality in MtgDiscoveryVibe by maintaining an up-to-date local database of MTG cards, sets, and related metadata.

### Purpose
Enable MtgDiscoveryVibe to maintain a complete, up-to-date repository of MTG card data by integrating with Scryfall's comprehensive API, ensuring users have immediate access to card information without external dependencies.

### Value Proposition
- **Reliability**: Local data storage ensures availability even when external APIs are down
- **Performance**: Cached responses provide sub-second query times
- **Cost Efficiency**: Minimized API calls reduce potential rate limiting and API costs
- **Foundation**: Enables all downstream features (collection management, deck building, price tracking)

## Alignment with Product Vision

This feature directly supports the PRD's Phase 1 Foundation goals by:
- **Data Foundation**: Establishing the core card database required for all other features
- **API Integration**: Implementing the first external service integration with proper rate limiting
- **Caching Strategy**: Demonstrating the MonoStateMemoryCache pattern for performance optimization
- **MicroObjects Architecture**: Serving as the reference implementation for future API integrations

The Scryfall Ingestion aligns with the product vision of consolidating multiple platforms by providing the comprehensive card data layer that powers the "Collector View" MVP.

## User Stories

### US-001: API Data Consumption
**As a** system administrator  
**I want** the application to automatically ingest Scryfall data  
**So that** users always have access to the latest card information

**Acceptance Criteria:**
- WHEN the ingestion process runs, IF a request is made within 100ms of the previous request, THEN the system must wait until 100ms have elapsed
- WHEN fetching card data, IF the API returns a successful response, THEN the data must be parsed and stored in CosmosDB
- WHEN an API call fails, IF the failure is due to rate limiting (429 status), THEN the system must implement exponential backoff
- WHEN an API call fails, IF the failure is not recoverable after 3 retries, THEN the error must be logged and the process must continue with the next item

### US-002: Data Caching
**As a** developer  
**I want** API responses to be cached in memory  
**So that** we minimize redundant API calls and improve performance

**Acceptance Criteria:**
- WHEN a Scryfall API endpoint is called, IF the same endpoint was called within the cache TTL, THEN return the cached response
- WHEN cached data expires, IF a new request is made, THEN fetch fresh data from the API
- WHEN the cache reaches memory limits, IF new data needs to be cached, THEN evict least recently used entries
- WHEN caching responses, IF the response indicates an error, THEN do not cache the response

### US-003: Bulk Data Import
**As a** system administrator  
**I want** to import Scryfall bulk data files  
**So that** I can perform initial data population efficiently

**Acceptance Criteria:**
- WHEN initiating bulk import, IF a bulk data file is available from Scryfall, THEN download and process the file
- WHEN processing bulk data, IF the file contains valid JSON, THEN parse and store all cards in batches of 100
- WHEN importing bulk data, IF a card already exists in the database, THEN update it with the latest information
- WHEN bulk import completes, IF successful, THEN log the number of cards imported/updated
- WHEN processing bulk data, IF the file is corrupted or contains invalid JSON, THEN log the error and attempt to process valid portions
- WHEN bulk import fails, IF after 3 retries, THEN save progress state and allow resume from last successful batch

### US-004: Incremental Updates
**As a** system administrator  
**I want** the system to check for new and updated cards daily  
**So that** the database stays current without full re-imports

**Acceptance Criteria:**
- WHEN the daily update runs, IF new cards are found, THEN add them to the database
- WHEN checking for updates, IF a card has been modified, THEN update the existing record
- WHEN performing incremental updates, IF the process is interrupted, THEN it must be resumable from the last successful position
- WHEN incremental update completes, IF changes were made, THEN log the summary of additions and updates

### US-005: Card Data Storage
**As a** developer  
**I want** Scryfall data stored in a structured format in CosmosDB  
**So that** it can be efficiently queried by other system components

**Acceptance Criteria:**
- WHEN storing a card, IF all required fields are present, THEN create a document in the Cards container
- WHEN storing a set, IF all required fields are present, THEN create a document in the Sets container
- WHEN storing data, IF the data includes nested objects, THEN preserve the complete structure
- WHEN querying cards, IF searching by name or set, THEN results must return within 100ms for indexed queries

### US-006: Image Management
**As a** system administrator  
**I want** card images to be downloaded and stored locally  
**So that** users can view cards without external dependencies

**Acceptance Criteria:**
- WHEN a card is ingested, IF it has image URIs, THEN queue the images for download
- WHEN downloading images, IF multiple resolutions are available, THEN download normal and large versions
- WHEN storing images, IF the download succeeds, THEN save to Azure Blob Storage with appropriate naming
- WHEN an image is requested, IF it exists in blob storage, THEN serve it from there instead of Scryfall

### US-007: Monitoring Dashboard
**As a** system administrator  
**I want** to view ingestion metrics and status  
**So that** I can monitor system health and performance

**Acceptance Criteria:**
- WHEN accessing the monitoring dashboard, IF ingestion is running, THEN display current progress percentage and estimated completion time
- WHEN viewing metrics, IF data is available, THEN show cache hit ratio, API response times, and error rates for the last 24 hours
- WHEN an error threshold is exceeded, IF the error rate exceeds 5% in a 5-minute window, THEN trigger an alert notification
- WHEN viewing historical data, IF requested, THEN provide metrics for up to 30 days with hourly granularity

### US-008: Error Handling and Recovery
**As a** system administrator  
**I want** comprehensive error handling and logging  
**So that** I can monitor and troubleshoot the ingestion process

**Acceptance Criteria:**
- WHEN any error occurs, IF it's recoverable, THEN retry with exponential backoff (initial: 1s, max: 32s, multiplier: 2x)
- WHEN any error occurs, IF it's not recoverable, THEN log the error with timestamp, endpoint, request parameters, response code, and stack trace
- WHEN monitoring the system, IF ingestion metrics are requested, THEN provide success/failure counts, average response time, and cache hit ratios
- WHEN rate limits are approached, IF the threshold is 80% (8 requests in 1 second), THEN log a warning with current rate

## Functional Requirements

### FR-001: Scryfall API Client
- Must support all Scryfall REST API endpoints needed for card and set data
- Must implement rate limiting of maximum 10 requests per second (100ms between requests)
- Must handle pagination for endpoints returning multiple pages
- Must support both individual card fetching and bulk operations
- Must parse and validate JSON responses according to Scryfall schema

### FR-002: Caching System
- Must use MonoStateMemoryCache for in-memory caching
- Must support configurable TTL for different types of data (cards: 24 hours, sets: 7 days)
- Must implement cache key generation based on endpoint and parameters
- Must handle cache invalidation for updated data
- Must track cache hit/miss ratios for monitoring

### FR-003: Data Transformation
- Must map Scryfall JSON schema to internal domain models
- Must handle all card layouts (normal, split, flip, transform, modal DFCs, etc.)
- Must preserve all metadata including legalities, prices, and related cards
- Must normalize data for consistent storage format
- Must validate required fields before storage

### FR-004: Storage Operations
- Must store cards in CosmosDB Cards container
- Must store sets in CosmosDB Sets container  
- Must use appropriate partition keys for optimal query performance
- Must support batch operations for bulk inserts/updates
- Must implement upsert logic to handle updates efficiently

### FR-005: Image Processing
- Must download images asynchronously without blocking data ingestion
- Must retry failed image downloads with exponential backoff
- Must store images with consistent naming convention: {set_code}/{collector_number}_{version}.jpg
- Must handle missing images gracefully
- Must support multiple image faces for double-faced cards

### FR-006: Scheduling and Orchestration
- Must support manual triggering of ingestion processes
- Must support scheduled daily incremental updates
- Must track last successful sync timestamp
- Must provide ability to resume interrupted processes
- Must prevent concurrent execution of same ingestion type

## Non-Functional Requirements

### NFR-001: Performance
- API response caching must serve cached responses in under 10ms
- Database writes must complete within 500ms for single cards
- Bulk import must process at least 1000 cards per minute
- Memory usage for caching must not exceed 500MB

### NFR-002: Reliability
- System must handle Scryfall API downtime gracefully
- Failed ingestion attempts must not corrupt existing data
- System must recover from transient failures automatically
- Data consistency must be maintained during concurrent operations

### NFR-003: Scalability
- Design must support future addition of other data sources
- Caching layer must handle growth to 100,000+ cached items
- Storage schema must accommodate future Scryfall schema changes
- System must handle full MTG card database (30,000+ unique cards)

### NFR-004: Monitoring
- All API calls must be logged with timing information
- Error rates must be tracked and alertable
- Cache performance metrics must be available
- Ingestion progress must be observable in real-time

### NFR-005: Security
- API keys must be stored securely in configuration
- No sensitive data should be logged
- Network communications must use HTTPS
- Rate limiting must prevent API abuse

## Technical Constraints

### TC-001: External Dependencies
- Must respect Scryfall API rate limits (10 requests/second maximum)
- Must handle Scryfall API versioning and deprecations
- Must work within Azure CosmosDB RU limits
- Must work within Azure Blob Storage transaction limits

### TC-002: Technology Stack
- Must use C# .NET 9.0
- Must follow MicroObjects architectural pattern
- Must use existing MonoStateMemoryCache for caching
- Must use existing Lib.Cosmos for database operations
- Must use existing Lib.BlobStorage for image storage

### TC-003: Development Constraints
- Must include comprehensive unit tests with fakes (not mocks)
- Must follow existing coding patterns in CODING_CRITERIA.md
- Must provide Example app demonstrating functionality
- Must not use primitive types directly (wrap in domain objects)
- All Scryfall data types must be wrapped in domain-specific objects following ToSystemType pattern

### TC-004: Data Governance
- Cache entries must expire after configured TTL (cards: 24 hours, sets: 7 days)
- Ingestion logs must be retained for 30 days minimum
- Failed ingestion attempts must be logged with full context for 7 days
- Orphaned images in blob storage must be cleaned up weekly

### TC-005: Configuration Management
- All configuration values must be externally configurable via IConfig
- Rate limiting parameters must be adjustable without code changes
- Cache TTL values must be configurable per data type
- Retry policies must be configurable (count, delays, backoff strategy)

## Acceptance Tests

### AT-001: Basic Card Ingestion
**Given** the Scryfall API is available  
**When** I request to ingest a specific card by ID  
**Then** the card data should be stored in CosmosDB  
**And** the card should be retrievable by ID

### AT-002: Rate Limiting Compliance
**Given** the system is ingesting multiple cards  
**When** requests are made in rapid succession  
**Then** no more than 10 requests should be made per second  
**And** no 429 (rate limit) errors should occur

### AT-003: Cache Functionality
**Given** a card has been fetched from the API  
**When** the same card is requested within the cache TTL  
**Then** the cached version should be returned  
**And** no API call should be made

### AT-004: Bulk Import Success
**Given** a Scryfall bulk data file is available  
**When** bulk import is initiated  
**Then** all cards in the file should be imported  
**And** the count of imported cards should match the file

### AT-005: Error Recovery
**Given** the Scryfall API returns a transient error  
**When** the ingestion process encounters the error  
**Then** it should retry with exponential backoff  
**And** eventually succeed or log the failure after max retries

### AT-006: Image Download
**Given** a card with image URIs is ingested  
**When** image processing completes  
**Then** the images should be available in Blob Storage  
**And** be retrievable by the expected path

## Dependencies

### External Dependencies
- Scryfall API (https://api.scryfall.com)
- Azure CosmosDB instance
- Azure Blob Storage account

### Internal Dependencies  
- Lib.Universal (for MonoStateMemoryCache and base patterns)
- Lib.Cosmos (for CosmosDB operations)
- Lib.BlobStorage (for image storage)
- TestConvenience.Core (for testing utilities)

## Risks and Mitigations

### Risk: API Rate Limiting
**Impact**: High - Could block all data ingestion  
**Mitigation**: Implement strict rate limiting with configurable delays and exponential backoff

### Risk: API Schema Changes
**Impact**: Medium - Could break data parsing  
**Mitigation**: Implement versioned parsing logic and comprehensive error handling

### Risk: Large Data Volume
**Impact**: Medium - Could cause memory/storage issues  
**Mitigation**: Implement streaming for bulk data and efficient batch processing

### Risk: Network Failures
**Impact**: Low - Could interrupt ingestion  
**Mitigation**: Implement resume capability and comprehensive retry logic

## Open Questions
1. Should we implement a priority queue for different types of ingestion requests?
2. What should be the cache eviction strategy when memory limits are reached?
3. Should we support partial card updates or always replace the entire document?
4. How should we handle Scryfall API deprecation notices?

## Success Metrics
- 100% of Scryfall cards successfully ingested
- Zero rate limiting violations after initial implementation
- 80%+ cache hit rate for frequently accessed cards
- 99.9% success rate for incremental updates
- Sub-second response time for cached data retrieval