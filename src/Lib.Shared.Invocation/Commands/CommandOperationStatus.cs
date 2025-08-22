using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Commands;

public abstract class CommandOperationStatus : OperationStatus
{
    public virtual string Message { get; protected set; }
}
