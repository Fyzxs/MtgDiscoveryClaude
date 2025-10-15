using System.Net;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class OperationResponseFake<T> : IOperationResponse<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => IsSuccess is false;
    public T ResponseData { get; init; } = default!;
    public OperationException OuterException { get; init; } = default!;
    public HttpStatusCode Status { get; init; } = HttpStatusCode.OK;
}
