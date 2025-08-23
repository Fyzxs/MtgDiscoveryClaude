namespace Lib.Shared.DataModels.Operations;

public sealed class FailureOperationStatus : OperationStatus
{
    private readonly string _message;

    public FailureOperationStatus(string message)
    {
        _message = message;
    }

    public override bool IsSuccessful => false;
    public override string Message => _message;
}
