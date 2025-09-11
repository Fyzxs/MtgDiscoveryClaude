# Lib.Shared.Invocation CLAUDE.md

## Purpose
Shared operation response patterns, request/command validation, and execution context infrastructure for consistent service communication across all layers.

## Narrative Summary
This library implements the foundational patterns for operation responses, request validation, and execution context management throughout the MTG Discovery platform. It provides consistent success/failure response patterns, request and command validation frameworks, and authentication context abstractions. The library ensures all service operations return standardized response objects with proper error handling, validation, and execution context tracking.

## Key Files
### Operation Response System
- `Operations/OperationResponse.cs:24-40` - Core operation response interfaces and implementations
- `Operations/OperationResponseValidator.cs` - Response validation logic

### Command Framework
- `Commands/CommandOperationResponse.cs` - Command-specific response patterns
- `Commands/ICommandValidatorAction.cs` - Command validation interface
- `Commands/DefaultTrueCommandValidatorAction.cs` - Default validation implementation
- `Commands/CommandOperationStatusMessage.cs` - Command status messaging

### Request Framework
- `Requests/IRequestValidatorAction.cs` - Request validation interface
- `Requests/OperationStatusResponseValidator.cs` - Request response validation
- `Requests/ValidatorFailedOperationResponse.cs` - Validation failure responses

### Execution Context
- `IExecutionContext.cs` - Basic execution context interface
- `IAuthNExecutionContext.cs` - Authentication execution context
- `ICaller.cs` - Caller identification interface

### Response Models
- `Response/Models/ResponseModel.cs` - Base response model implementation
- `Response/IRequestResponseModelFactory.cs` - Request response factory interface
- `Response/ICommandResponseModelFactory.cs` - Command response factory interface

### Exception Handling
- `Exceptions/OperationException.cs` - Base operation exception
- `Requests/ValidatorOperationException.cs` - Validation-specific exceptions

## Operation Response Pattern
### Core Response Types
- `IOperationResponse<T>:24-31` - Main operation response interface
- `SuccessOperationResponse<T>:9-15` - Success response implementation
- `FailureOperationResponse<T>:16-22` - Failure response implementation

### Response Properties
- `IsSuccess:26` - Boolean success indicator
- `IsFailure:27` - Boolean failure indicator  
- `ResponseData:28` - Generic response payload
- `OuterException:29` - Exception information for failures
- `Status:30` - HTTP status code for API responses

## Validation Framework
### Command Validation
- Command argument validation through `ICommandValidatorAction`
- Status response filtering and validation
- Default validation implementations for common scenarios
- Command-specific exception handling and response generation

### Request Validation
- Request parameter validation through `IRequestValidatorAction`
- Response validation and filtering
- Validation failure response generation
- Request-specific exception handling

## Execution Context System
### Context Types
- **Basic Context**: `IExecutionContext` - Basic operation tracking
- **Authentication Context**: `IAuthNExecutionContext` - User authentication tracking
- **Caller Context**: `ICaller` - Service caller identification

### Context Usage
- Operation tracing and logging
- User authentication and authorization
- Service-to-service communication tracking
- Performance monitoring and metrics

## Response Model Factory Pattern
### Factory Interfaces
- `IRequestResponseModelFactory` - Creates response models for requests
- `ICommandResponseModelFactory` - Creates response models for commands
- Consistent response model creation across operations
- Standardized error response generation

### Model Creation
- Success response model generation
- Failure response model generation
- Status code mapping and HTTP response integration
- Serialization-friendly response structures

## Validation Pipeline
### Validation Flow
1. Input validation through validator actions
2. Business rule validation in domain layers
3. Response validation and filtering
4. Error response generation for failures
5. Success response creation for valid operations

### Validation Components
- **Validator Actions**: Interface-based validation logic
- **Status Filters**: Response filtering and processing
- **Status Validators**: Response validation and verification
- **Exception Wrappers**: Consistent exception handling

## Dependencies
- Universal: `Lib.Universal` - Primitive wrappers and base types
- External: System.Net (HttpStatusCode), standard .NET libraries
- No domain-specific dependencies (shared foundation library)

## Integration Points
### Provides
- Operation response patterns to all service layers
- Validation framework for request/command processing
- Execution context abstractions for operation tracking
- Exception handling patterns for consistent error management
- Response model factories for API layer integration

### Usage Across Layers
- **Entry Layer**: Request validation and response generation
- **Domain Layer**: Business rule validation and operation responses
- **Data Layer**: Data operation responses and error handling
- **API Layer**: HTTP response model generation and status mapping

## Key Patterns
- Operation Response pattern for consistent service communication
- Validator Action pattern for pluggable validation logic
- Factory pattern for response model creation
- Context pattern for execution tracking
- Exception Wrapper pattern for error handling

## MicroObjects Implementation
- All response types implement proper interfaces
- Value objects for primitive response data
- Immutable response objects with readonly properties
- Constructor injection for response dependencies
- Interface segregation for specialized response types

## HTTP Status Integration
### Status Code Mapping
- Success operations return appropriate 2xx codes
- Validation failures return 4xx codes
- System errors return 5xx codes
- Custom status codes for domain-specific scenarios

## Related Documentation
- All service layer CLAUDE.md files reference these patterns
- `../Lib.Universal/CLAUDE.md` - Base primitive and utility types
- Architecture.md - Overall operation response architecture