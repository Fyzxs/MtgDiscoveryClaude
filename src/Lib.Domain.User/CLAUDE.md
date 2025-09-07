# Lib.Domain.User CLAUDE.md

## Purpose
User domain logic layer implementing business rules and operations for user management and registration.

## Narrative Summary
This library contains the core business logic for user operations in the MTG Discovery platform. It implements user registration workflows, business rule validation, and coordinates user data operations. The domain layer processes user registration requests from the entry layer and orchestrates data persistence through the aggregator layer, ensuring all business rules and constraints are properly enforced.

## Key Files
- `Apis/IUserDomainService.cs` - Domain service interface for user operations
- `Apis/UserDomainService.cs` - Domain service implementation

## Domain Operations
### User Registration
- Processes user registration requests with JWT-derived user information
- Enforces business rules for user data integrity
- Coordinates with data layer for user persistence
- Handles registration response generation

## Key Components
### Domain Service
- `IUserDomainService` - Interface defining user domain operations
- `UserDomainService` - Implementation of user business logic
- Registration workflow orchestration
- Business rule enforcement

## Dependencies
- Data Layer: `Lib.MtgDiscovery.Data` - Data coordination
- Shared Models: `Lib.Shared.DataModels` - Entity interfaces
- User Info: `Lib.Shared.UserInfo` - User value objects
- Response Patterns: `Lib.Shared.Invocation` - Operation responses

## Business Logic
### User Registration Process
1. Receives user information from entry layer
2. Validates business rules for user data
3. Coordinates user data creation through data layer
4. Generates registration response with user ID
5. Handles registration success and failure scenarios

### Business Rules
- User ID uniqueness enforcement
- Display name validation
- Source ID verification from JWT claims

## Integration Points
### Consumes
- Entry Layer: `Lib.MtgDiscovery.Entry` - Request coordination
- Data Layer: User data persistence operations

### Provides
- User domain operations to entry layer
- Business rule enforcement
- User workflow orchestration

## Configuration
Domain services configured through dependency injection:
- Data service instances for persistence operations
- Logger instances for domain operation tracking

## Key Patterns
- Domain service pattern for business logic encapsulation
- Operation response pattern for consistent error handling
- MicroObjects pattern for value object wrapping
- Dependency injection for service composition

## Related Documentation
- `../Lib.MtgDiscovery.Entry/CLAUDE.md` - Entry layer coordination
- `../Lib.MtgDiscovery.Data/CLAUDE.md` - Data layer integration
- `../Lib.Aggregator.User/CLAUDE.md` - User data aggregation