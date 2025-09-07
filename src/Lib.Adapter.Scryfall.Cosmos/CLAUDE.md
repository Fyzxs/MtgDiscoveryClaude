# Lib.Adapter.Scryfall.Cosmos CLAUDE.md

## Purpose
Azure Cosmos DB integration adapter providing MTG data and user information persistence with MicroObjects patterns.

## Narrative Summary
This library serves as the primary adapter for Azure Cosmos DB operations, handling both MTG card data and user information storage. It implements the adapter pattern with Cosmos-specific entities, containers, and operators. The library includes comprehensive support for card, set, artist, and user data operations, following strict MicroObjects patterns for all data models. Recent additions include user information storage capabilities with dedicated containers and operators.

## Key Files
### User Storage Components
- `Apis/Entities/UserInfoItem.cs:6-19` - User information Cosmos document model
- `Apis/Operators/UserInfoScribe.cs:7-13` - User data persistence operations
- `Cosmos/Containers/UserInfoCosmosContainer.cs` - User data container definition
- `Cosmos/Values/UserInfoCosmosContainerName.cs` - Container name value object

### Core Infrastructure
- `Apis/Entities/IScryfallPayload.cs` - Base payload interface
- `Cosmos/Values/MtgDiscoveryCosmosDatabaseName.cs` - Database name definition
- `Cosmos/Values/MtgDiscoveryCosmosAccountName.cs` - Account name definition

### MTG Data Components
- `Apis/Entities/ScryfallCardItem.cs` - Card document model
- `Apis/Entities/ScryfallSetItem.cs` - Set document model
- `Apis/Entities/ScryfallArtistItem.cs` - Artist document model
- `Apis/Operators/` - Persistence operators (Scribe, Gopher, Inquisitor)

## Container Architecture
### User Information Container
- **Container**: `UserInfoCosmosContainer`
- **Entity**: `UserInfoItem` with user ID as partition key
- **Properties**: `user_id`, `display_name`, `source_id`
- **Operator**: `UserInfoScribe` for persistence operations

### MTG Data Containers
- Card Items, Set Items, Artist Items
- Trigram containers for search functionality
- Association containers for relationships
- Index containers for optimized queries

## Key Components
### User Data Storage
- `UserInfoItem:8-18` - Cosmos document with user ID partitioning
- `UserInfoScribe:7-13` - Extends `CosmosScribe` for user operations
- `UserInfoCosmosContainer` - Container configuration for user data

### Operator Patterns
- **Scribe**: Write operations (Create, Update, Upsert)
- **Gopher**: Read operations (Get, Query)
- **Inquisitor**: Query operations (Search, Filter)

### Entity Patterns
All entities extend `CosmosItem` with:
- `Id` property for document identification
- `Partition` property for partitioning strategy
- JSON property mapping for serialization

## Dependencies
- Infrastructure: `Lib.Cosmos` - Core Cosmos DB operations
- Configuration: Cosmos connection and container definitions
- Serialization: Newtonsoft.Json for document serialization

## Storage Strategy
### User Information
- Partition by user ID for optimal query performance
- Single document per user with complete user profile
- JSON property mapping for API compatibility

### MTG Data
- Optimized partitioning strategies per entity type
- Trigram indexing for search operations
- Association tables for relationship mapping

## Integration Points
### Consumes
- Aggregator Layer: Data coordination requests
- Configuration: Connection strings and container settings

### Provides
- User data persistence to `Lib.Aggregator.User`
- MTG data storage to card/set/artist aggregators
- Search and query operations through operators

## Configuration
- Container definitions with throughput and indexing policies
- Database and account name value objects
- Connection string management through configuration

## Key Patterns
- Adapter pattern for external system integration
- Container pattern for Cosmos DB resource management
- Operator pattern for specialized data operations (Scribe/Gopher/Inquisitor)
- Value object pattern for configuration and naming
- MicroObjects pattern for all domain value wrapping

## Related Documentation
- `../Lib.Cosmos/CLAUDE.md` - Core Cosmos DB infrastructure
- `../Lib.Aggregator.User/CLAUDE.md` - User data aggregation
- `../Lib.Shared.UserInfo/CLAUDE.md` - User value objects