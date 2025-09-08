# Lib.Cosmos CLAUDE.md

## Purpose
Azure Cosmos DB integration library providing complete MicroObjects-compliant data access patterns, connection management, and container operations.

## Narrative Summary
This library implements comprehensive Cosmos DB functionality following strict MicroObjects principles. It provides authentication adapters for multiple Azure authentication modes (SAS, Entra ID), container adapters for CRUD operations, and configuration management for Cosmos DB connections. The library abstracts all Cosmos SDK complexity behind domain-specific interfaces and value objects, ensuring type safety and consistent patterns across the platform.

## Key Files
### Container Operations
- `Apis/CosmosContainerAdapter.cs:16-50` - Main container adapter abstraction
- `Apis/Adapters/ICosmosContainerAdapter.cs` - Container operations interface
- `Apis/CosmosItem.cs` - Base item implementation for Cosmos documents

### Authentication and Connection
- `Adapters/CosmosGenesisClientAdapter.cs` - Primary Cosmos client adapter
- `Adapters/CosmosEntraAuthGenesisDevice.cs` - Entra ID authentication device
- `Adapters/CosmosSasAuthGenesisDevice.cs` - SAS token authentication device
- `Adapters/MonoStateCosmosClientAdapter.cs` - Singleton client adapter

### Configuration System
- `Apis/ConfigCosmosConfiguration.cs` - Root configuration provider
- `Apis/Configurations/CosmosAccountEndpoint.cs` - Account endpoint value object
- `Apis/Configurations/CosmosConnectionString.cs` - Connection string wrapper
- `Apis/Configurations/CosmosAuthMode.cs` - Authentication mode enumeration

### Container Management
- `Configurations/ICosmosContainerDefinition.cs` - Container definition interface
- `Configurations/CosmosContainerAutoscaleMax.cs` - Autoscale configuration
- `Configurations/CosmosContainerTimeToLive.cs` - TTL configuration

### Operation Patterns
- `Operators/CosmosContainerQueryOperator.cs` - Query operations
- `Operators/CosmosContainerUpsertOperator.cs` - Upsert operations
- `Operators/CosmosContainerDeleteOperator.cs` - Delete operations
- `Operators/CosmosContainerReadOperator.cs` - Read operations

## Architecture Patterns
### Container Adapter Pattern
- Abstract base class for container operations
- Composition of specialized operators
- Consistent interface across container types
- Dependency injection of operators

### Genesis Device Pattern
- Authentication abstraction for client creation
- Multiple authentication modes (SAS, Entra ID, Connection String)
- Consistent client initialization regardless of auth method

### Configuration Hierarchy
- Root: `ConfigCosmosConfiguration`
- Account level: Connection and authentication settings
- Container level: Container-specific configurations

## Container Operations
### CRUD Operations
- Query: SQL query execution with result streaming
- Read: Single item retrieval by ID and partition key
- Upsert: Insert or update item operations
- Delete: Item deletion by ID and partition key

### Query Patterns
- Synchronous queries with iterator pattern
- Asynchronous queries with async enumerable
- SQL query string construction and execution
- Result mapping to domain objects

## Authentication Integration
### Supported Authentication Modes
- **Connection String**: Direct connection string authentication
- **SAS Token**: Shared Access Signature token authentication
- **Entra ID**: Azure Active Directory authentication

### Genesis Device Implementation
- `IGenesisDevice` interface for client creation abstraction
- Mode-specific implementations for each authentication type
- Consistent `CreateCosmosClient` method across all devices

## Configuration Management
### Configuration Structure
- Uses `Lib.Universal.MonoStateConfig` as base
- Hierarchical configuration with colon separators
- Value objects for all configuration primitives
- Type-safe configuration access

### Configuration Keys
- Account endpoint configuration
- Authentication mode and credentials
- Connection mode settings (Gateway/Direct)
- Container definitions and settings

## Dependencies
- Infrastructure: `Lib.Universal` - Configuration and utilities
- External: `Microsoft.Azure.Cosmos`, `Azure.ResourceManager.CosmosDb`
- Logging: `Microsoft.Extensions.Logging.Abstractions`
- Serialization: `Newtonsoft.Json`

## Integration Points
### Consumes
- Universal: Configuration system and utilities
- Azure: Cosmos DB SDK and Resource Manager

### Provides
- Container adapter abstractions to data layers
- Authentication and connection management
- Configuration management for Cosmos operations
- CRUD operation patterns for document storage

## Key Patterns
- Container Adapter pattern for operation abstraction
- Genesis Device pattern for client initialization
- Operator pattern for specialized CRUD operations
- MonoState pattern for client singleton management
- Value Object pattern for configuration primitives
- Interface Segregation for focused operation contracts

## MicroObjects Implementation
- All primitives wrapped in value objects
- Interface for every abstraction
- Immutable configuration objects
- Constructor injection throughout
- No public statics except MonoState infrastructure
- Composition over inheritance for operators

## Container Definition Pattern
### Usage
Inherit from `CosmosContainerAdapter` and implement:
- Container name and partition key definition
- Item type mapping and serialization
- Query construction for specific item types

### Example Structure
```csharp
public class SpecificContainerAdapter : CosmosContainerAdapter
{
    // Container-specific operations
    // Item type mapping
    // Query implementations
}
```

## Related Documentation
- `../Lib.Universal/CLAUDE.md` - Base configuration and utilities
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Specific Cosmos implementations
- Architecture.md - Overall data layer architecture