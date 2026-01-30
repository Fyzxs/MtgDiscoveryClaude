using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

public sealed class UpdateCollectionVisibilityArgEntity : IUpdateCollectionVisibilityArgEntity
{
    public string CollectionId { get; init; }
    public string Visibility { get; init; }
}
