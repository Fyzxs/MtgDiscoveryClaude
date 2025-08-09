# Implementation Plan

## Task Overview
The Scryfall Ingestion implementation follows a bottom-up approach, starting with foundational configuration and domain models, then building the HTTP adapter with Polly-based rate limiting, followed by data persistence layers, and finally the orchestration and monitoring components. Each task is designed to be atomic and testable, following MicroObjects patterns.

## Steering Document Compliance
Tasks follow structure.md conventions:
- Library naming: Lib.Scryfall.Ingestion
- Test projects: Lib.Scryfall.Ingestion.Tests
- Example apps: Example.ScryfallIngestion
- All tasks use MicroObjects pattern with domain value objects
- Configuration follows Lib.Cosmos patterns
- Uses MonoStateHttpClient from Lib.Universal
- Implements Polly for rate limiting and retries
- Uses Semaphores for synchronization (no object locks)

## Atomic Task Requirements
**Each task must meet these criteria for optimal agent execution:**
- **File Scope**: Touches 1-3 related files maximum
- **Time Boxing**: Completable in 15-30 minutes
- **Single Purpose**: One testable outcome per task
- **Specific Files**: Must specify exact files to create/modify
- **Agent-Friendly**: Clear input/output with minimal context switching

## Task Format Guidelines
- Use checkbox format: `- [ ] Task number. Task description`
- **Specify files**: Always include exact file paths to create/modify
- **Include implementation details** as bullet points
- Reference requirements using: `_Requirements: X.Y, Z.A_`
- Reference existing code to leverage using: `_Leverage: path/to/file.ts, path/to/component.tsx_`
- Focus only on coding tasks (no deployment, user testing, etc.)
- **Avoid broad terms**: No "system", "integration", "complete" in task titles

## Tasks

### Project Setup

- [ ] 1. Create Lib.Scryfall.Ingestion project
  - File: src/Lib.Scryfall.Ingestion/Lib.Scryfall.Ingestion.csproj
  - Create minimal Class Library project file
  - Directory.Build.props handles framework and settings
  - Purpose: Establish main library project structure
  - _Leverage: src/Lib.Cosmos/Lib.Cosmos.csproj_
  - _Requirements: FR-001, TC-002_

- [ ] 2. Create Lib.Scryfall.Ingestion.Tests project
  - File: src/Lib.Scryfall.Ingestion.Tests/Lib.Scryfall.Ingestion.Tests.csproj
  - Create minimal test project file
  - Directory.Build.props auto-references main project and test packages
  - Purpose: Establish test project structure
  - _Leverage: src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj structure_
  - _Requirements: TC-002, TC-003_

- [ ] 3. Add NuGet packages and project references
  - File: src/Lib.Scryfall.Ingestion/Lib.Scryfall.Ingestion.csproj
  - Add PackageReference for Polly, Polly.RateLimiting, Newtonsoft.Json
  - Add ProjectReference to Lib.Universal, Lib.Cosmos, Lib.BlobStorage
  - Purpose: Configure required dependencies
  - _Requirements: TC-002_

### Configuration Layer

- [ ] 4. Create IScryfallConfiguration root interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Configurations/IScryfallConfiguration.cs
  - Define root configuration interface with sub-configs
  - Include ApiConfig(), RateLimitConfig(), CacheConfig(), RetryConfig() methods
  - Purpose: Define configuration contract
  - _Leverage: src/Lib.Cosmos/Apis/Configurations/ICosmosConfiguration.cs_
  - _Requirements: FR-001, TC-002, TC-005_

- [ ] 5. Create API configuration interfaces
  - File: src/Lib.Scryfall.Ingestion/Apis/Configurations/IScryfallApiConfig.cs
  - Define BaseUrl(), TimeoutSeconds() methods
  - Return wrapped domain types (not primitives)
  - Purpose: Define API endpoint configuration
  - _Requirements: FR-001_

- [ ] 6. Create rate limit configuration interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Configurations/IScryfallRateLimitConfig.cs
  - Define RequestsPerSecond(), BurstSize() methods
  - Include ThrottleWarningThreshold() for monitoring
  - Purpose: Configure Polly rate limiting
  - _Requirements: FR-001, US-001, US-008_

- [ ] 7. Create cache configuration interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Configurations/IScryfallCacheConfig.cs
  - Define CardTtlHours(), SetTtlHours(), MaxCacheSize() methods
  - All return domain value objects
  - Purpose: Configure MonoStateMemoryCache settings
  - _Requirements: FR-002, US-002_

- [ ] 8. Create retry configuration interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Configurations/IScryfallRetryConfig.cs
  - Define MaxRetries(), InitialDelayMs(), MaxDelayMs(), BackoffMultiplier()
  - Purpose: Configure Polly retry policies
  - _Requirements: US-008_

- [ ] 9. Implement ConfigScryfallConfiguration root
  - File: src/Lib.Scryfall.Ingestion/Configurations/ConfigScryfallConfiguration.cs
  - Implement IScryfallConfiguration using IConfig
  - Compose sub-configurations using lazy initialization
  - Purpose: Provide root configuration implementation
  - _Leverage: src/Lib.Cosmos/Configurations/ConfigCosmosConfiguration.cs_
  - _Requirements: FR-001, TC-002, TC-005_

- [ ] 10. Implement API configuration
  - File: src/Lib.Scryfall.Ingestion/Configurations/ConfigScryfallApiConfig.cs
  - Read from "ScryfallConfig:Api:BaseUrl" and "ScryfallConfig:Api:TimeoutSeconds"
  - Return domain value objects
  - Purpose: Read API settings from appsettings.json
  - _Leverage: src/Lib.Cosmos/Configurations/ConfigCosmosContainerDefinition.cs_
  - _Requirements: FR-001_

- [ ] 11. Implement rate limit configuration
  - File: src/Lib.Scryfall.Ingestion/Configurations/ConfigScryfallRateLimitConfig.cs
  - Read from "ScryfallConfig:RateLimit" section
  - Default to 10 requests per second
  - Purpose: Configure Polly rate limiter
  - _Requirements: FR-001, US-001_

- [ ] 12. Implement cache configuration
  - File: src/Lib.Scryfall.Ingestion/Configurations/ConfigScryfallCacheConfig.cs
  - Read from "ScryfallConfig:Cache" section
  - Default TTLs: cards 24 hours, sets 7 days
  - Purpose: Configure cache expiration
  - _Requirements: FR-002, US-002, TC-004_

- [ ] 13. Implement retry configuration
  - File: src/Lib.Scryfall.Ingestion/Configurations/ConfigScryfallRetryConfig.cs
  - Read from "ScryfallConfig:Retry" section
  - Default: 3 retries, 1s initial delay, 2x backoff
  - Purpose: Configure exponential backoff
  - _Requirements: US-008_

### Domain Value Objects

- [ ] 14. Create ScryfallCardId value object
  - File: src/Lib.Scryfall.Ingestion/Apis/Ids/ScryfallCardId.cs
  - Extend ToSystemType<Guid> with validation
  - Override ToString() for API formatting
  - Purpose: Type-safe card identifier
  - _Leverage: src/Lib.Universal/Apis/ToSystemType.cs_
  - _Requirements: TC-002, TC-003_

- [ ] 15. Create ScryfallSetCode value object
  - File: src/Lib.Scryfall.Ingestion/Apis/Ids/ScryfallSetCode.cs
  - Extend ToSystemType<string> with 3-5 char validation
  - Add ToLowerInvariant() for API consistency
  - Purpose: Type-safe set code
  - _Leverage: src/Lib.Universal/Apis/ToSystemType.cs_
  - _Requirements: TC-002, TC-003_

- [ ] 16. Create CollectorNumber value object
  - File: src/Lib.Scryfall.Ingestion/Apis/Ids/CollectorNumber.cs
  - Extend ToSystemType<string> (handles special chars like "123a")
  - Add parsing logic for numeric sorting
  - Purpose: Type-safe collector number
  - _Requirements: TC-002, TC-003_

- [ ] 17. Create ImageResolution enum wrapper
  - File: src/Lib.Scryfall.Ingestion/Apis/Ids/ImageResolution.cs
  - Define Small, Normal, Large, Png, ArtCrop, BorderCrop
  - Add ToUriSegment() method for blob paths
  - Purpose: Type-safe image resolution specifier
  - _Requirements: US-006, FR-005_

### Domain Models

- [ ] 18. Create IScryfallCard interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Models/IScryfallCard.cs
  - Define Id(), Name(), SetCode(), CollectorNumber() properties
  - Include ImageUris(), Prices(), Legalities() complex types
  - Purpose: Define card data contract
  - _Requirements: FR-003, US-005_

- [ ] 19. Create IScryfallSet interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Models/IScryfallSet.cs
  - Define Id(), Code(), Name(), ReleasedAt() properties
  - Include SetType(), CardCount() metadata
  - Purpose: Define set data contract
  - _Requirements: FR-003, US-005_

- [ ] 20. Create IScryfallBulkData interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Models/IScryfallBulkData.cs
  - Define Type(), DownloadUri(), UpdatedAt() properties
  - Include ContentType(), ContentEncoding() metadata
  - Purpose: Define bulk download contract
  - _Requirements: US-003, FR-001_

- [ ] 21. Create IScryfallList interface for paginated results
  - File: src/Lib.Scryfall.Ingestion/Apis/Models/IScryfallList.cs
  - Define Data<T>(), HasMore(), NextPage() properties
  - Support generic type parameter for data items
  - Purpose: Handle paginated API responses
  - _Requirements: FR-001_

- [ ] 22. Implement ScryfallCard model part 1 - basic properties
  - File: src/Lib.Scryfall.Ingestion/Models/ScryfallCard.cs
  - Implement IScryfallCard with basic properties
  - Use JsonProperty attributes for API deserialization
  - Purpose: Card data implementation (basic)
  - _Requirements: FR-003, US-005_

- [ ] 23. Implement ScryfallCard model part 2 - complex properties
  - File: src/Lib.Scryfall.Ingestion/Models/ScryfallCard.cs
  - Add ImageUris, Prices, Legalities nested objects
  - Handle null/optional fields properly
  - Purpose: Card data implementation (complex)
  - _Requirements: FR-003, US-005_

- [ ] 24. Implement ScryfallSet model
  - File: src/Lib.Scryfall.Ingestion/Models/ScryfallSet.cs
  - Implement IScryfallSet with all properties
  - Add JsonProperty attributes
  - Purpose: Set data implementation
  - _Requirements: FR-003, US-005_

- [ ] 25. Create SourceDataDocument for Cosmos
  - File: src/Lib.Scryfall.Ingestion/Models/SourceDataDocument.cs
  - Extend CosmosItem base class
  - Add DataType, SourceId, Content properties
  - Purpose: Cosmos storage wrapper
  - _Leverage: src/Lib.Cosmos/Apis/CosmosItem.cs_
  - _Requirements: US-005, FR-004_

### Polly Integration

- [ ] 26. Create IScryfallPolicyProvider interface
  - File: src/Lib.Scryfall.Ingestion/Apis/IScryfallPolicyProvider.cs
  - Define GetRateLimitPolicy(), GetRetryPolicy(), GetCombinedPolicy()
  - Return IAsyncPolicy<HttpResponseMessage>
  - Purpose: Define Polly policy contract
  - _Requirements: US-001, US-008_

- [ ] 27. Create Polly rate limit policy
  - File: src/Lib.Scryfall.Ingestion/Operations/ScryfallPolicyProvider.cs
  - Implement rate limiter using Polly.RateLimiting
  - Configure 10 requests per second with sliding window
  - Purpose: Enforce API rate limits
  - _Requirements: US-001, NFR-001_

- [ ] 28. Add Polly retry policy with exponential backoff
  - File: src/Lib.Scryfall.Ingestion/Operations/ScryfallPolicyProvider.cs
  - Add retry policy for transient failures
  - Handle 429, 503, timeout exceptions
  - Purpose: Implement resilient retries
  - _Requirements: US-008_

- [ ] 29. Combine Polly policies with monitoring
  - File: src/Lib.Scryfall.Ingestion/Operations/ScryfallPolicyProvider.cs
  - Combine rate limit and retry policies
  - Add telemetry callbacks for monitoring
  - Purpose: Create comprehensive resilience strategy
  - _Requirements: US-007, NFR-004_

- [ ] 30. Create ScryfallPolicyProvider unit tests
  - File: src/Lib.Scryfall.Ingestion.Tests/Operations/ScryfallPolicyProviderTests.cs
  - Test rate limiting enforcement
  - Test retry with backoff
  - Purpose: Verify policy behavior
  - Note: Create HttpMessageHandler fake for testing
  - _Requirements: TC-003, AT-002_

### HTTP Adapter

- [ ] 31. Create IScryfallHttpAdapter interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Adapters/IScryfallHttpAdapter.cs
  - Define GetAsync<T>(), PostAsync<T>() methods
  - Include endpoint and query parameter support
  - Purpose: Define HTTP adapter contract
  - _Requirements: FR-001, US-001_

- [ ] 32. Create ScryfallEndpoint value object
  - File: src/Lib.Scryfall.Ingestion/Models/ScryfallEndpoint.cs
  - Extend ToSystemType<string> for type safety
  - Add static factory methods for common endpoints
  - Purpose: Type-safe endpoint representation
  - _Leverage: src/Lib.Universal/Apis/ToSystemType.cs_
  - _Requirements: FR-001_

- [ ] 33. Create cache key generator
  - File: src/Lib.Scryfall.Ingestion/Operations/Caching/ScryfallCacheKeyGenerator.cs
  - Generate keys from endpoint + parameters
  - Handle query string normalization
  - Purpose: Consistent cache key generation
  - _Requirements: FR-002, US-002_

- [ ] 34. Implement MonoStateScryfallAdapter part 1 - basic structure
  - File: src/Lib.Scryfall.Ingestion/Adapters/MonoStateScryfallAdapter.cs
  - Create MonoState class with lazy initialization
  - Inject MonoStateHttpClient and configuration
  - Purpose: HTTP adapter implementation foundation
  - _Leverage: src/Lib.Universal/MonoStateHttpClient.cs_
  - _Requirements: FR-001, US-001_

- [ ] 35. Implement MonoStateScryfallAdapter part 2 - caching
  - File: src/Lib.Scryfall.Ingestion/Adapters/MonoStateScryfallAdapter.cs
  - Add MonoStateMemoryCache integration
  - Implement cache-aside pattern
  - Purpose: Add response caching
  - _Leverage: src/Lib.Universal/MonoStateMemoryCache.cs_
  - _Requirements: FR-002, US-002_

- [ ] 36. Implement MonoStateScryfallAdapter part 3 - Polly integration
  - File: src/Lib.Scryfall.Ingestion/Adapters/MonoStateScryfallAdapter.cs
  - Apply Polly policies to HTTP calls
  - Add telemetry for monitoring
  - Purpose: Apply resilience policies
  - _Requirements: US-001, US-008_

- [ ] 37. Create MonoStateScryfallAdapter unit tests
  - File: src/Lib.Scryfall.Ingestion.Tests/Adapters/MonoStateScryfallAdapterTests.cs
  - Test caching behavior
  - Test policy application
  - Purpose: Verify adapter functionality
  - _Leverage: src/TestConvenience.Core/Fakes/MemoryCacheFake.cs_
  - _Requirements: TC-003, AT-003_

### Scryfall Client

- [ ] 38. Create IScryfallClient interface
  - File: src/Lib.Scryfall.Ingestion/Apis/IScryfallClient.cs
  - Define GetCard(), GetSet(), SearchCards() methods
  - Include bulk data operations
  - Purpose: High-level client interface
  - _Requirements: FR-001, US-001_

- [ ] 39. Create IScryfallGopher for single items
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IScryfallGopher.cs
  - Define FetchCard(), FetchSet() methods
  - Return single items by ID
  - Purpose: Single item retrieval
  - _Requirements: FR-001, US-001_

- [ ] 40. Create IScryfallInquisitor for searches
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IScryfallInquisitor.cs
  - Define Search() with query support
  - Handle pagination automatically
  - Purpose: Search and list operations
  - _Requirements: FR-001_

- [ ] 41. Create IScryfallHarvester for bulk data
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IScryfallHarvester.cs
  - Define GetBulkDataInfo(), DownloadBulkData()
  - Support streaming for large files
  - Purpose: Bulk data operations
  - _Requirements: US-003, FR-001_

- [ ] 42. Implement ScryfallGopher
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallGopher.cs
  - Use IScryfallHttpAdapter for HTTP calls
  - Deserialize responses to domain models
  - Purpose: Implement single item fetching
  - _Requirements: FR-001, US-001_

- [ ] 43. Implement ScryfallInquisitor pagination logic
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallInquisitor.cs
  - Handle HasMore flag and NextPage URL
  - Yield results as IAsyncEnumerable
  - Purpose: Automatic pagination handling
  - _Requirements: FR-001_

- [ ] 44. Implement ScryfallHarvester streaming
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallHarvester.cs
  - Use HttpCompletionOption.ResponseHeadersRead
  - Stream JSON parsing for memory efficiency
  - Purpose: Handle large bulk files
  - _Requirements: US-003, FR-001_

- [ ] 45. Implement ScryfallClient orchestration
  - File: src/Lib.Scryfall.Ingestion/Apis/ScryfallClient.cs
  - Compose Gopher, Inquisitor, Harvester
  - Provide unified interface
  - Purpose: High-level client implementation
  - _Requirements: FR-001, US-001_

### CosmosDB Storage

- [ ] 46. Create SetItems container definition
  - File: src/Lib.Scryfall.Ingestion/Apis/Containers/SetItemsContainerDefinition.cs
  - Implement ICosmosContainerDefinition
  - Partition key: /partition, RU: 1000
  - Purpose: Define Sets container
  - _Leverage: src/Lib.Cosmos/Apis/ICosmosContainerDefinition.cs_
  - _Requirements: US-005, FR-004_

- [ ] 47. Create CardItems container definition
  - File: src/Lib.Scryfall.Ingestion/Apis/Containers/CardItemsContainerDefinition.cs
  - Implement ICosmosContainerDefinition
  - Partition key: /partition, RU: 4000
  - Purpose: Define Cards container
  - _Leverage: src/Lib.Cosmos/Apis/ICosmosContainerDefinition.cs_
  - _Requirements: US-005, FR-004_

- [ ] 48. Create IScryfallCosmosScribe interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IScryfallCosmosScribe.cs
  - Define UpsertCard(), UpsertSet(), BatchUpsert()
  - Include query methods for retrieval
  - Purpose: Cosmos persistence contract
  - _Requirements: US-005, FR-004_

- [ ] 49. Implement ScryfallCosmosScribe part 1 - setup
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallCosmosScribe.cs
  - Initialize with CosmosContainerAdapter
  - Configure Polly policies for Cosmos
  - Purpose: Cosmos scribe foundation
  - _Leverage: src/Lib.Cosmos/Apis/Operators/CosmosContainerAdapter.cs_
  - _Requirements: US-005, FR-004_

- [ ] 50. Implement ScryfallCosmosScribe part 2 - operations
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallCosmosScribe.cs
  - Implement upsert with conflict resolution
  - Add batch processing for bulk imports
  - Purpose: Cosmos CRUD operations
  - _Requirements: US-003, US-005_

- [ ] 51. Create ScryfallCosmosScribe unit tests
  - File: src/Lib.Scryfall.Ingestion.Tests/Apis/Operators/ScryfallCosmosScribeTests.cs
  - Test upsert logic
  - Test batch operations
  - Purpose: Verify persistence layer
  - _Leverage: src/Lib.Cosmos.Tests/Fakes/ContainerFake.cs pattern_
  - _Requirements: TC-003, AT-001_

### Blob Storage

- [ ] 52. Create CardImages blob container definition
  - File: src/Lib.Scryfall.Ingestion/Apis/Containers/CardImagesContainerDefinition.cs
  - Implement IBlobContainerDefinition
  - Configure for public read access
  - Purpose: Define image storage container
  - _Leverage: src/Lib.BlobStorage/Apis/IBlobContainerDefinition.cs_
  - _Requirements: US-006, FR-005_

- [ ] 53. Create IScryfallImageHandler interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IScryfallImageHandler.cs
  - Define DownloadAndStore(), GetImageUrl()
  - Support multiple resolutions
  - Purpose: Image handling contract
  - _Requirements: US-006, FR-005_

- [ ] 54. Create ImageUris model
  - File: src/Lib.Scryfall.Ingestion/Models/ImageUris.cs
  - Implement IImageUris with all resolution URLs
  - Add null handling for optional resolutions
  - Purpose: Image URL collection
  - _Requirements: US-006, FR-005_

- [ ] 55. Implement ScryfallImageHandler part 1 - download
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallImageHandler.cs
  - Download images using HttpClient
  - Apply retry policies for resilience
  - Purpose: Image download logic
  - _Requirements: US-006, FR-005_

- [ ] 56. Implement ScryfallImageHandler part 2 - storage
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/ScryfallImageHandler.cs
  - Store using BlobWriteScribe
  - Use path: {set}/{number}/{resolution}.jpg
  - Purpose: Image persistence
  - _Leverage: src/Lib.BlobStorage/Apis/Operators/BlobWriteScribe.cs_
  - _Requirements: US-006, FR-005_

- [ ] 57. Create ScryfallImageHandler unit tests
  - File: src/Lib.Scryfall.Ingestion.Tests/Apis/Operators/ScryfallImageHandlerTests.cs
  - Test download retry logic
  - Test path generation
  - Purpose: Verify image handling
  - _Leverage: src/Lib.BlobStorage.Tests/Fakes/BlobOpResponseFake.cs pattern_
  - _Requirements: TC-003, AT-006_

### Monitoring

- [ ] 58. Create IScryfallIngestionMonitor interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Monitoring/IScryfallIngestionMonitor.cs
  - Define RecordApiCall(), RecordCacheHit(), RecordError()
  - Include GetMetrics() for dashboard
  - Purpose: Monitoring contract
  - _Requirements: US-007, NFR-004_

- [ ] 59. Create IngestionMetrics model
  - File: src/Lib.Scryfall.Ingestion/Models/Monitoring/IngestionMetrics.cs
  - Track ApiCalls, CacheHits, Errors, ResponseTimes
  - Calculate rates and percentages
  - Purpose: Metrics data structure
  - _Requirements: US-007, NFR-004_

- [ ] 60. Implement ScryfallIngestionMonitor
  - File: src/Lib.Scryfall.Ingestion/Operations/Monitoring/ScryfallIngestionMonitor.cs
  - Use thread-safe counters
  - Calculate rolling windows
  - Purpose: Metrics collection
  - _Requirements: US-007, NFR-004_

### State Management

- [ ] 61. Create IIngestionStateRepository interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IIngestionStateRepository.cs
  - Define SaveProgress(), LoadProgress(), MarkComplete()
  - Support resumable operations
  - Purpose: State persistence contract
  - _Requirements: US-004, FR-006_

- [ ] 62. Implement IngestionStateRepository
  - File: src/Lib.Scryfall.Ingestion/Operations/IngestionStateRepository.cs
  - Store state in Cosmos metadata container
  - Track last sync timestamp
  - Purpose: Ingestion state tracking
  - _Leverage: src/Lib.Cosmos/Apis/Operators/CosmosContainerAdapter.cs_
  - _Requirements: US-004, FR-006_

### Orchestration

- [ ] 63. Create IIngestionOrchestrator interface
  - File: src/Lib.Scryfall.Ingestion/Apis/IIngestionOrchestrator.cs
  - Define RunFullIngestion(), RunIncremental()
  - Include progress callbacks
  - Purpose: Orchestration contract
  - _Requirements: US-004, FR-006_

- [ ] 64. Implement IngestionOrchestrator part 1 - setup
  - File: src/Lib.Scryfall.Ingestion/Operations/IngestionOrchestrator.cs
  - Compose client, scribe, monitor
  - Initialize with semaphore for concurrency
  - Purpose: Orchestrator foundation
  - _Requirements: US-004, FR-006_

- [ ] 65. Implement IngestionOrchestrator part 2 - execution
  - File: src/Lib.Scryfall.Ingestion/Operations/IngestionOrchestrator.cs
  - Implement bulk import logic
  - Add incremental update logic
  - Purpose: Ingestion workflows
  - _Requirements: US-003, US-004_

- [ ] 66. Create IngestionOrchestrator unit tests
  - File: src/Lib.Scryfall.Ingestion.Tests/Operations/IngestionOrchestratorTests.cs
  - Test full ingestion flow
  - Test incremental updates
  - Purpose: Verify orchestration
  - _Requirements: TC-003, AT-004_

- [ ] 67. Create IImageCleanupJanitor interface
  - File: src/Lib.Scryfall.Ingestion/Apis/Operators/IImageCleanupJanitor.cs
  - Define FindOrphans(), CleanupOldImages()
  - Include dry-run mode
  - Purpose: Image cleanup contract
  - _Requirements: TC-004_

- [ ] 68. Implement ImageCleanupJanitor
  - File: src/Lib.Scryfall.Ingestion/Operations/ImageCleanupJanitor.cs
  - Compare blob storage with Cosmos records
  - Delete orphaned images older than 7 days
  - Purpose: Storage maintenance
  - _Leverage: src/Lib.BlobStorage/Apis/Operators/BlobJanitor.cs_
  - _Requirements: TC-004_

### Example Application

- [ ] 69. Create Example.ScryfallIngestion project
  - File: src/Example.ScryfallIngestion/Example.ScryfallIngestion.csproj
  - Create minimal project file
  - Directory.Build.props auto-configures as Exe for Example.* projects
  - Add ProjectReference to Lib.Scryfall.Ingestion
  - Purpose: Example app project
  - _Leverage: src/Example.Core/Example.Core.csproj_
  - _Requirements: TC-003_

- [ ] 70. Create ScryfallIngestionApplication class
  - File: src/Example.ScryfallIngestion/ScryfallIngestionApplication.cs
  - Extend ExampleApplication base class
  - Override RunAsync() with demo logic
  - Purpose: Example implementation
  - _Leverage: src/Example.Core/ExampleApplication.cs_
  - _Requirements: TC-003_

- [ ] 71. Implement example ingestion scenarios
  - File: src/Example.ScryfallIngestion/ScryfallIngestionApplication.cs
  - Demo single card fetch
  - Demo bulk import
  - Purpose: Usage examples
  - _Requirements: TC-003_

- [ ] 72. Create appsettings.json configuration
  - File: src/Example.ScryfallIngestion/appsettings.json
  - Configure CerberusCosmosConfig section
  - Configure ScryfallConfig section
  - Purpose: Example configuration
  - _Leverage: src/Example.Core/appsettings.json_
  - _Requirements: TC-002, TC-005_

- [ ] 73. Create Program.cs entry point
  - File: src/Example.ScryfallIngestion/Program.cs
  - Bootstrap configuration
  - Run ScryfallIngestionApplication
  - Purpose: Application entry
  - _Leverage: src/Example.Core/Program.cs_
  - _Requirements: TC-003_

### Test Utilities (Optional)

- [ ] 78. Create TestConvenience.Scryfall project if needed
  - File: src/TestConvenience.Scryfall/TestConvenience.Scryfall.csproj
  - Only create if fakes need to be shared across multiple test projects
  - Move any reusable test fakes from Lib.Scryfall.Ingestion.Tests
  - Purpose: Share test utilities across projects
  - _Leverage: src/TestConvenience.Core/TestConvenience.Core.csproj_
  - _Requirements: TC-003_

### Integration Tests

- [ ] 74. Create integration test for card fetching
  - File: src/Lib.Scryfall.Ingestion.Tests/Integration/CardFetchTests.cs
  - Test single card retrieval
  - Verify caching behavior
  - Purpose: Validate card operations
  - _Requirements: AT-001, AT-003_

- [ ] 75. Create integration test for rate limiting
  - File: src/Lib.Scryfall.Ingestion.Tests/Integration/RateLimitTests.cs
  - Test 10 requests per second limit
  - Verify no 429 errors
  - Purpose: Validate rate limiting
  - _Requirements: AT-002_

- [ ] 76. Create integration test for bulk import
  - File: src/Lib.Scryfall.Ingestion.Tests/Integration/BulkImportTests.cs
  - Test partial file processing
  - Verify resume capability
  - Purpose: Validate bulk operations
  - _Requirements: AT-004, AT-005_

- [ ] 77. Create integration test for image handling
  - File: src/Lib.Scryfall.Ingestion.Tests/Integration/ImageTests.cs
  - Test download and storage
  - Verify path conventions
  - Purpose: Validate image pipeline
  - _Requirements: AT-006_

## Implementation Order

**Phase 1: Foundation (Tasks 1-13)**
- Project setup and configuration
- Configuration interfaces and implementations
- Complete configuration layer before moving forward

**Phase 2: Domain Layer (Tasks 14-25)**
- Value objects for type safety
- Domain models and interfaces
- Cosmos storage documents

**Phase 3: Infrastructure (Tasks 26-37)**
- Polly policies for resilience
- HTTP adapter with caching
- Complete infrastructure before client

**Phase 4: Business Logic (Tasks 38-57)**
- Scryfall client and operators
- CosmosDB persistence
- Blob storage for images

**Phase 5: Operations (Tasks 58-68)**
- Monitoring and metrics
- State management
- Orchestration logic
- Cleanup operations

**Phase 6: Validation (Tasks 69-77)**
- Example application
- Integration tests
- End-to-end validation

## Summary
- **Total Tasks**: 78
- **Estimated Duration**: Each task 15-30 minutes
- **Dependencies**: Tasks build on each other within phases
- **Testing**: Unit tests included with implementations
- **Note**: Test project references handled by Directory.Build.props; fakes should be in TestConvenience projects if shared