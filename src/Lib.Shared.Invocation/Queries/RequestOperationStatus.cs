using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Queries;

public abstract class RequestOperationStatus<TResponse>
{
    protected RequestOperationStatus(RequestException ex)
    {
        IsSuccess = false;
        OuterException = ex;
    }

    protected RequestOperationStatus(TResponse response)
    {
        IsSuccess = true;
        ResponseValue = response;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => IsSuccess is false;
    public RequestException OuterException { get; }
    public TResponse ResponseValue { get; }
    public abstract HttpStatusCode StatusCode { get; }
}
