---
paths:
  - "csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/ErrorHandling/**"
---

# Error Handling Pattern

## Overview

Errors flow through two parallel paths:

1. **Application errors** -- captured as `IOperationResponse<T>` failures, mapped to `FailureResponseModel` union branch
2. **Infrastructure errors** -- caught by HotChocolate's `IErrorFilter`, enriched with metadata

Normal failures (validation, not found, business rule violations) use path 1. Unexpected exceptions use path 2.

## Error Flow

```
Entry Service returns IOperationResponse<T>
    ↓
OperationResponseToResponseModelMapper
    ├─ IsSuccess → SuccessDataResponseModel<T>
    └─ IsFailure → FailureResponseModel { StatusCode, Message }
    ↓
GraphQL returns ResponseModel (union: Success | Failure)
```

```
Unexpected exception thrown
    ↓
HotChocolate catches it
    ↓
HttpStatusCodeErrorFilter.OnError()
    ├─ Adds timestamp
    ├─ Auth errors → 401 status code
    └─ Dev mode → exception type, message, path, locations
    ↓
GraphQL returns error response with extensions
```

## HttpStatusCodeErrorFilter

**Location**: `ErrorHandling/HttpStatusCodeErrorFilter.cs`

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/ErrorHandling/HttpStatusCodeErrorFilter.cs`

**Key behaviors:**
- All errors get a UTC timestamp
- Auth errors (`AUTH_NOT_AUTHENTICATED`, `AUTH_NOT_AUTHORIZED`) receive 401 status
- Development mode includes exception details, path, and locations
- Production mode strips exception details for security

## Registration

In `Startup.ConfigureServices()`:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Startup.cs`

## ResponseModel Union Pattern

Every endpoint returns a union of success or failure:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL/Actions/Mappers/OperationResponseToResponseModelMapper.cs`

See: `layers/app/response-models.md` for full ResponseModel documentation.

## Key Rules

1. **Never throw from query/mutation methods** -- always return `ResponseModel` union
2. **FailureResponseModel carries HTTP status codes** -- embedded in `StatusDataModel`
3. **Exception details are environment-gated** -- dev mode includes details, prod strips them
4. **Introspection disabled in production** -- prevents schema discovery
5. **Auth errors get explicit 401** -- `HttpStatusCodeErrorFilter` maps auth error codes

## Reference Files

- **Error filter**: `ErrorHandling/HttpStatusCodeErrorFilter.cs`
- **Registration**: `Startup.cs` (`.AddErrorFilter<>()`, `.ModifyRequestOptions()`)
- **Response mapper**: `Actions/Mappers/OperationResponseToResponseModelMapper.cs`
- **ResponseModel types**: `Entities/Types/ResponseModels/`
