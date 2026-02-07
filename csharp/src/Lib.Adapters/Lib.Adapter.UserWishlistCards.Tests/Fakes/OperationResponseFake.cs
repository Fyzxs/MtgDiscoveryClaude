using System.Net;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Tests.Fakes;

public sealed class OperationResponseFake<T> : IOperationResponse<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => IsSuccess is false;
    public T ResponseData { get; init; }
    public OperationException OuterException { get; init; }
    public HttpStatusCode Status { get; init; }
}
