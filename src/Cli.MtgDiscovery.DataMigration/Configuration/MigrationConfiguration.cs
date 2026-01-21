namespace Cli.MtgDiscovery.DataMigration.Configuration;

internal sealed class MigrationConfiguration
{
    public required string SourceCollectorId { get; init; }
    public required string TargetUserId { get; init; }
    public required string ErrorOutputPath { get; init; }
    public required string SuccessOutputPath { get; init; }
}
