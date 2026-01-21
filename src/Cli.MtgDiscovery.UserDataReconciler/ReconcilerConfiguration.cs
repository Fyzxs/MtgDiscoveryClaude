namespace Cli.MtgDiscovery.UserDataReconciler;

internal sealed class ReconcilerConfiguration
{
    public bool DryRun { get; init; } = true;
    public string SpecificUserId { get; init; } = string.Empty;
}
