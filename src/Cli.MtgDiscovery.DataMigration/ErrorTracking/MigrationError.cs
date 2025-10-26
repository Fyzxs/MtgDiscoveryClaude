namespace Cli.MtgDiscovery.DataMigration.ErrorTracking;

internal sealed class MigrationError
{
    public required string OldCardId { get; init; }
    public required string ScryfallId { get; init; }
    public required string SetId { get; init; }
    public required string ErrorReason { get; init; }
}
