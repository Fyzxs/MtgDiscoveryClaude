using System.Net;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandOperationStatus
{
    public virtual bool IsSuccess { get; protected set; }
    public bool IsFailure => IsSuccess is false;
    public virtual HttpStatusCode Status { get; protected set; }
    public virtual string Message { get; protected set; }
}
