namespace Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;

public sealed class CollectorDataRecord
{
    public required string CollectorId { get; init; }
    public required string SetId { get; init; }
    public required string CardId { get; init; }
    public required int Unmodified { get; init; }
    public required int Signed { get; init; }
    public required int Proof { get; init; }
    public required int Altered { get; init; }
}
