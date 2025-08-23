namespace Lib.Shared.Invocation.Commands;

public sealed class ProvidedCommandOperationStatusMessage : CommandOperationStatusMessage
{
    private readonly string _origin;

    public ProvidedCommandOperationStatusMessage(string origin) => _origin = origin;

    public override string AsSystemType() => _origin;
}