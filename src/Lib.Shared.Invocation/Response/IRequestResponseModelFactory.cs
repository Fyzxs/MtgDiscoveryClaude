using Lib.Shared.Invocation.Queries;
using Lib.Shared.Invocation.Response.Models;

namespace Lib.Shared.Invocation.Response;

public interface IRequestResponseModelFactory<TResponse>
{
    ResponseModel Success<TData>(RequestOperationStatus<TResponse> requestOperationStatus, TData data);
    ResponseModel Failure(RequestOperationStatus<TResponse> requestOperationStatus);
}
