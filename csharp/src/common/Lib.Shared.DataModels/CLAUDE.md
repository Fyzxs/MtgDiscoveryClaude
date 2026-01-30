# Lib.Shared.DataModels CLAUDE.md

## Purpose
Shared entity interfaces and data transfer objects providing contracts for data flow across all application layers.

## Narrative Summary
This library defines the core entity interfaces used throughout the MTG Discovery platform, establishing contracts for data transfer between layers. It includes interfaces for MTG data entities (cards, sets, artists) and user entities, following the ITR (Interface-To-Repository) pattern. Recent additions include comprehensive user entity interfaces supporting JWT-based authentication and user registration workflows.

## Key Files
### User Entity Interfaces
- `Entities/IAuthUserArgEntity.cs:3-8` - JWT authentication argument interface
- `Entities/User/IUserRegistrationItrEntity.cs:5-7` - User registration response interface
- `Entities/User/IUserInfoItrEntity.cs` - Complete user information interface
- `Entities/User/UserInfoItrEntity.cs` - User information implementation
- `Entities/User/UserRegistrationItrEntity.cs` - Registration response implementation

### MTG Data Interfaces
- `Entities/ICardItemItrEntity.cs` - Card entity interface
- `Entities/ISetItemItrEntity.cs` - Set entity interface
- `Entities/IArtistItemItrEntity.cs` - Artist entity interface
- Collection interfaces for bulk operations

## Entity Categories
### Authentication Entities
- `IAuthUserArgEntity` - JWT claims extraction interface
  - Properties: `UserId`, `SourceId`, `DisplayName`
  - Used by GraphQL authentication layer

### User Registration Entities
- `IUserRegistrationItrEntity:7` - Registration response contract
  - Property: `UserId` of type `Lib.Shared.UserInfo.Values.UserId`
  - Returned from registration operations

### User Information Entities
- `IUserInfoItrEntity` - Complete user profile interface
- `UserInfoItrEntity` - Implementation with user details
- Used for user data persistence and retrieval

## Interface Patterns
### ITR Pattern
- Interfaces define contracts between layers
- Repository-style access patterns
- Clear separation of concerns between layers

### Value Object Integration
- Uses `Lib.Shared.UserInfo` value objects for type safety
- Prevents primitive obsession in entity definitions
- Maintains MicroObjects patterns throughout

### Collection Interfaces
- Collection entities for bulk operations
- Search result entities for query operations
- Argument entities for request parameters

## Dependencies
- Value Objects: `Lib.Shared.UserInfo` - User-specific value types
- Base Types: Standard .NET interfaces and collections
- No implementation dependencies - interface-only library

## Data Flow Contracts
### User Registration Flow
1. `IAuthUserArgEntity` - Incoming JWT authentication data
2. Internal processing through service layers
3. `IUserRegistrationItrEntity` - Outgoing registration confirmation

### Entity Transformation
- Interfaces support transformation between layers
- GraphQL entities implement these interfaces
- Storage entities map to these contracts

## Integration Points
### Consumes
- User Info: Value objects for type-safe properties

### Provides
- Entity contracts to all application layers
- Data transfer object interfaces
- Authentication and user management contracts

## Key Patterns
- Interface Segregation Principle for focused contracts
- ITR pattern for repository-style data access
- Value Object pattern integration for type safety
- DTO pattern for data transfer between layers
- Contract pattern for layer communication

## Entity Responsibilities
- Define data structure contracts
- Establish property requirements
- Enable layer-to-layer communication
- Support serialization and transformation
- Maintain type safety through value objects

## Related Documentation
- `../Lib.Shared.UserInfo/CLAUDE.md` - User value objects
- `../App.MtgDiscovery.GraphQL/CLAUDE.md` - GraphQL entity implementations
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Storage entity mappings