using System.Net;

namespace Lib.Shared.Invocation.Commands;

internal sealed class ValidatorFailedCommandOperationStatus : CommandOperationStatus
{
    public ValidatorFailedCommandOperationStatus(string message) => Message = message;
    public override bool IsSuccess => false;
    public override HttpStatusCode Status => HttpStatusCode.BadRequest;
}

internal sealed class FilterFailedCommandOperationStatus : CommandOperationStatus
{
    public FilterFailedCommandOperationStatus(string message, HttpStatusCode status)
    {
        Status = status;
        Message = message;
    }

    public override bool IsSuccess => false;
}
