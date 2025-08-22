using System.Net;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Queries;

public abstract class RequestOperationStatus<TResponse> : OperationStatus
{
    protected RequestOperationStatus(OperationException ex) : base(ex)
    { }

    protected RequestOperationStatus(TResponse response)
    {
        ResponseValue = response;
    }
    public TResponse ResponseValue { get; }
    public abstract HttpStatusCode StatusCode { get; }
}
