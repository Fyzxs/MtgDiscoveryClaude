using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Operations;

public abstract class OperationStatus
{
    protected OperationStatus(OperationException ex)
    {
        IsSuccess = false;
        OuterException = ex;
    }

    protected OperationStatus()
    {
        IsSuccess = true;
    }

    public virtual bool IsSuccess { get; }
    public bool IsFailure => IsSuccess is false;
    public OperationException OuterException { get; }
    public virtual HttpStatusCode Status { get; protected set; }
}
