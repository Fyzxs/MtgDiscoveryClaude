namespace Cli.MtgDiscovery.DataMigration.Migration;

internal sealed class MigrationResult
{
    public required int TotalRecords { get; init; }
    public required int SuccessfulMigrations { get; init; }
    public required int CardsNotFound { get; init; }
    public required int OtherErrors { get; init; }
}
