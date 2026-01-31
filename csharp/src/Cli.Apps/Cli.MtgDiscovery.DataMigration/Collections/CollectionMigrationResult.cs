namespace Cli.MtgDiscovery.DataMigration.Collections;

internal sealed class CollectionMigrationResult
{
    public required int TotalUsers { get; init; }
    public required int CollectionsCreated { get; init; }
    public required int CollectionsSkipped { get; init; }
    public required int UserCardsUpdated { get; init; }
    public required int UserWishlistCardsUpdated { get; init; }
    public required int UserSetCardsUpdated { get; init; }
    public required int Errors { get; init; }
}
