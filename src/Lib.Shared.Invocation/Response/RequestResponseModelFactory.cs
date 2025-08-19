using System.Diagnostics.CodeAnalysis;
using Lib.Shared.Invocation.Queries;
using Lib.Shared.Invocation.Response.Models;

namespace Lib.Shared.Invocation.Response;

public abstract class RequestResponseModelFactory<TResponse> : IRequestResponseModelFactory<TResponse>
{
    private readonly IExecutionContext _exCtx;

    protected RequestResponseModelFactory(IExecutionContext exCtx) => _exCtx = exCtx;

    public ResponseModel Success<TData>([NotNull] RequestOperationStatus<TResponse> requestOperationStatus, TData data)
    {
        return new SuccessDataResponseModel<TData>
        {
            Data = data,
            Status = new StatusDataModel
            {
                StatusCode = requestOperationStatus.StatusCode,
                Message = requestOperationStatus.StatusCode.ToString()
            },
            MetaData = MetaData()
        };
    }

    public ResponseModel Failure([NotNull] RequestOperationStatus<TResponse> requestOperationStatus)
    {
        return new FailureResponseModel
        {
            Status = new StatusDataModel
            {
                StatusCode = requestOperationStatus.StatusCode,
                Message = requestOperationStatus.OuterException.Message
            },
            MetaData = MetaData()
        };
    }

    private MetaDataModel MetaData() => new() { ElapsedTime = _exCtx.ElapsedTime().ToString(), InvocationId = _exCtx.InvocationId() };
}
