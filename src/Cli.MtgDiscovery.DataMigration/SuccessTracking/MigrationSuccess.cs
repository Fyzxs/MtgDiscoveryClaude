namespace Cli.MtgDiscovery.DataMigration.SuccessTracking;

public sealed class MigrationSuccess
{
    public required string OldCardId { get; init; }
    public required string ScryfallId { get; init; }
    public required string SetId { get; init; }
    public required string Finish { get; init; }
    public required string Special { get; init; }
    public required string SetGroupId { get; init; }
    public required int Count { get; init; }
}
