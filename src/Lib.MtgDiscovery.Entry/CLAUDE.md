# Lib.MtgDiscovery.Entry CLAUDE.md

## Purpose
Entry service layer providing request validation, argument mapping, and service coordination for all domain operations.

## Narrative Summary
This library implements the entry point for all business operations in the MTG Discovery platform. It follows the MicroObjects pattern with strict validation, argument transformation, and service coordination. The entry layer handles JWT authentication arguments, validates all inputs, maps external arguments to internal entities, and orchestrates calls to the domain layer. Each operation follows the same pattern: validate → map → delegate → respond.

## Key Files
- `Apis/IEntryService.cs:3` - Main service interface combining all entry services
- `Apis/IUserEntryService.cs:7-10` - User-specific entry service interface
- `Queries/UserEntryService.cs` - User entry service implementation
- `Queries/Mappers/AuthUserArgsToItrMapper.cs` - JWT argument transformation
- `Queries/Validators/AuthUserArgEntityValidatorContainer.cs` - User argument validation
- `Apis/EntryService.cs` - Composite entry service implementation

## Module Structure
- `Apis/` - Public service interfaces
- `Queries/` - Service implementations organized by domain
- `Queries/Mappers/` - Argument transformation logic
- `Queries/Validators/` - Input validation containers and components
- `Entities/` - Internal ITR entity definitions

## Key Components
### User Registration Components
- `IUserEntryService:9` - `RegisterUserAsync` method signature
- `AuthUserArgsToItrMapper` - Transforms JWT claims to internal user entities
- `AuthUserArgEntityValidator` - Validates JWT claim extraction

### Validation Pattern
All entry services follow consistent validation:
1. Argument validation using validator containers
2. Argument mapping to ITR entities
3. Domain service delegation
4. Response handling with operation response pattern

### Mapping Pattern
Argument mappers transform external entities to internal ITR entities:
- Preserve all data integrity
- Apply domain-specific transformations
- Maintain type safety through MicroObjects approach

## Dependencies
- Domain Services: `Lib.Domain.User`, `Lib.Domain.Cards`
- Shared Abstractions: `Lib.Shared.DataModels`, `Lib.Shared.UserInfo`
- Response Patterns: `Lib.Shared.Invocation`

## Integration Points
### Consumes
- Domain Services: Business logic execution
- JWT Claims: User authentication context

### Provides
- `IEntryService` - Unified entry point for all operations
- Request validation and sanitization
- Argument mapping and transformation
- Operation response coordination

## Configuration
Entry services use dependency injection for:
- Domain service instances
- Logger instances for operation tracking
- Validator and mapper component resolution

## Key Patterns
- Validator container pattern for comprehensive input validation
- Mapper pattern for argument transformation
- Operation response pattern for consistent error handling
- Service composition pattern for unified entry interface
- MicroObjects wrapping for all primitive values

## Related Documentation
- `../Lib.Domain.User/CLAUDE.md` - User domain logic
- `../Lib.Shared.DataModels/CLAUDE.md` - Entity interfaces
- `../App.MtgDiscovery.GraphQL/CLAUDE.md` - API integration