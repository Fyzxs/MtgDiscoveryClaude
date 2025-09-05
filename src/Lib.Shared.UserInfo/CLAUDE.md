# Lib.Shared.UserInfo CLAUDE.md

## Purpose
User-specific value objects implementing MicroObjects pattern for user identity and information management.

## Narrative Summary
This library provides strongly-typed value objects for user-related information in the MTG Discovery platform. It implements the MicroObjects philosophy by wrapping all user-related primitive values in dedicated classes with proper equality semantics. These value objects ensure type safety, prevent primitive obsession, and provide consistent user identity handling across all layers of the application.

## Key Files
- `Values/UserId.cs:5-12` - Unique user identifier value object
- `Values/UserSourceId.cs` - External identity provider ID wrapper
- `Values/UserNickname.cs` - User display name wrapper

## Value Objects
### UserId
- **Purpose**: Unique identifier for users in the system
- **Base Class**: `StringEqualityToSystemType<UserId>`
- **Implementation**: Wraps string GUID value with proper equality
- **Usage**: Primary key for user operations and storage

### UserSourceId
- **Purpose**: External identity provider identifier (e.g., Auth0 subject)
- **Implementation**: Wraps external system user identifiers
- **Usage**: Links internal users to external authentication systems

### UserNickname
- **Purpose**: User display name from authentication provider
- **Implementation**: Wraps display name string with validation
- **Usage**: User-facing display purposes and profile information

## Architecture Patterns
### MicroObjects Implementation
- All primitives wrapped in dedicated classes
- No primitive values exposed in public APIs
- Proper equality semantics for value comparison
- Immutable value objects with readonly fields

### Type Safety
- Prevents accidental mixing of different string types
- Compile-time type checking for user-related operations
- Explicit conversion methods when system types needed

## Dependencies
- Base Types: `Lib.Universal.Primitives` - Base equality types
- No external dependencies - pure value objects

## Usage Patterns
### Value Object Creation
```csharp
// From constructor
UserId userId = new UserId(guidString);

// System type conversion
string systemValue = userId.AsSystemType();
```

### Equality Operations
- Value objects implement proper equality semantics
- Can be used as dictionary keys and in collections
- Hash code generation for performance optimization

## Integration Points
### Consumes
- Universal Primitives: Base equality type implementations

### Provides
- User value objects to all layers requiring user identification
- Type-safe user information handling
- Consistent user identity representation

## Key Patterns
- Value Object pattern for user information wrapping
- MicroObjects pattern for primitive elimination
- Equality pattern for value comparison
- Immutable object pattern for data integrity
- Factory pattern through constructors

## Type Hierarchy
- `UserId` : `StringEqualityToSystemType<UserId>`
- `UserSourceId` : Similar pattern for external IDs
- `UserNickname` : String-based value object for display names

## Related Documentation
- `../Lib.Universal/CLAUDE.md` - Base primitive types
- `../Lib.Shared.DataModels/CLAUDE.md` - Entity interfaces using these values
- `../App.MtgDiscovery.GraphQL/CLAUDE.md` - Value object usage in authentication