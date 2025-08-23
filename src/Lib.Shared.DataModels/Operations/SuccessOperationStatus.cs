namespace Lib.Shared.DataModels.Operations;

public sealed class SuccessOperationStatus<T> : OperationStatus
{
    private readonly T _value;
    private readonly string _message;

    public SuccessOperationStatus(T value) : this(value, "Operation completed successfully")
    {
    }

    public SuccessOperationStatus(T value, string message)
    {
        _value = value;
        _message = message;
    }

    public override bool IsSuccessful => true;
    public override string Message => _message;
    public T Value => _value;
}
