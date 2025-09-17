using System.Net;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class FakeOperationResponse<T> : IOperationResponse<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => IsSuccess is false;
    public T ResponseData { get; init; } = default!;
    public OperationException OuterException { get; init; } = default!;
    public HttpStatusCode Status { get; init; } = HttpStatusCode.OK;
}
