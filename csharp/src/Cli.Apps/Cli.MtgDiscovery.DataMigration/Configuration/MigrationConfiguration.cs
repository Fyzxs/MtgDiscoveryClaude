namespace Cli.MtgDiscovery.DataMigration.Configuration;

internal sealed class MigrationConfiguration
{
    public required string SourceCollectorId { get; init; }
    public required string TargetUserId { get; init; }
    public required string ErrorOutputPath { get; init; }
    public required string SuccessOutputPath { get; init; }

    /// <summary>
    /// When true, replaces existing card counts instead of adding to them.
    /// When false (default), adds to existing counts.
    /// </summary>
    public bool ReplaceExistingCounts { get; init; }

    /// <summary>
    /// Optional: When set, only migrates cards from this specific set code (e.g., "lea", "2ed").
    /// Used for testing migration behavior on a smaller dataset.
    /// </summary>
    public string TestSetCode { get; init; }
}
