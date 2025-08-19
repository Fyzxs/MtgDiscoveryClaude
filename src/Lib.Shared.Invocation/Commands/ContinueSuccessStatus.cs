using System.Net;

namespace Lib.Shared.Invocation.Commands;

public sealed class ContinueSuccessStatus : CommandOperationStatus
{
    public override bool IsSuccess => true;
    public override HttpStatusCode Status => HttpStatusCode.Continue;
    public override string Message => "Nothing happened, continue on your way.";
}