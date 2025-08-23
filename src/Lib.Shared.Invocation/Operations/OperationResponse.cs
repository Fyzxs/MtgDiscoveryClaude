using System.Net;
using Lib.Shared.Invocation.Exceptions;
using Lib.Universal.Primitives;

namespace Lib.Shared.Invocation.Operations;

public abstract class OperationResponseMessage : ToSystemType<string>;

public sealed class SuccessOperationResponse<TResponseData> : OperationResponse<TResponseData>
{
    public SuccessOperationResponse(TResponseData responseData) : base(responseData)
    {
        IsSuccess = true;
    }
}
public sealed class FailureOperationResponse<TResponseData> : OperationResponse<TResponseData>
{
    public FailureOperationResponse(TResponseData responseData) : base(responseData)
    {
        IsSuccess = false;
    }
}

public interface IOperationResponse<out TResponseData>
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    TResponseData ResponseData { get; }
    OperationException OuterException { get; }
    HttpStatusCode Status { get; }
}

public abstract class OperationResponse<TResponseData> : IOperationResponse<TResponseData>
{
    protected OperationResponse(OperationException ex)
    {
        IsSuccess = false;
        OuterException = ex;
    }

    protected OperationResponse(TResponseData responseData)
    {
        ResponseData = responseData;
    }

    public bool IsSuccess { get; protected set; }
    public bool IsFailure => IsSuccess is false;
    public TResponseData ResponseData { get; protected set; }
    public OperationException OuterException { get; protected set; }
    public HttpStatusCode Status { get; protected set; }
}
