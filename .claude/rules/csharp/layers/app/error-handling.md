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

```csharp
internal sealed class HttpStatusCodeErrorFilter : IErrorFilter
{
    private readonly IHostEnvironment _environment;

    public HttpStatusCodeErrorFilter(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public IError OnError([NotNull] IError error)
    {
        Dictionary<string, object> extensions = new()
        {
            ["timestamp"] = DateTime.UtcNow.ToString("O")
        };

        if (error.Code == "AUTH_NOT_AUTHENTICATED" || error.Code == "AUTH_NOT_AUTHORIZED")
        {
            extensions["statusCode"] = 401;
        }

        if (_environment.IsDevelopment())
        {
            extensions["path"] = error.Path?.ToList();
            extensions["locations"] = error.Locations?.ToList();
            if (error.Exception != null)
            {
                extensions["exceptionType"] = error.Exception.GetType().Name;
                extensions["exceptionMessage"] = error.Exception.Message;
                if (error.Exception.InnerException != null)
                {
                    extensions["innerException"] = error.Exception.InnerException.Message;
                    extensions["innerExceptionType"] = error.Exception.InnerException.GetType().Name;
                }
            }
        }

        return error.WithExtensions(extensions);
    }
}
```

**Key behaviors:**
- All errors get a UTC timestamp
- Auth errors (`AUTH_NOT_AUTHENTICATED`, `AUTH_NOT_AUTHORIZED`) receive 401 status
- Development mode includes exception details, path, and locations
- Production mode strips exception details for security

## Registration

In `Startup.ConfigureServices()`:

```csharp
_ = services
    .AddGraphQLServer()
    .AddErrorFilter<HttpStatusCodeErrorFilter>()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = _environment.IsDevelopment();
    })
    .DisableIntrospection(_environment.IsDevelopment() is false);
```

## ResponseModel Union Pattern

Every endpoint returns a union of success or failure:

```csharp
// Success path
new SuccessDataResponseModel<TData> { Data = response.ResponseData }

// Failure path
new FailureResponseModel
{
    Status = new StatusDataModel
    {
        Message = response.OuterException.StatusMessage,
        StatusCode = response.OuterException.StatusCode
    }
}
```

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
