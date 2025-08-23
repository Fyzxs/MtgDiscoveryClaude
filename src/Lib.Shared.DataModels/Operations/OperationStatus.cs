namespace Lib.Shared.DataModels.Operations;

public abstract class OperationStatus
{
    public abstract bool IsSuccessful { get; }
    public abstract string Message { get; }
}
