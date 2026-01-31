using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

internal sealed class UpdateCollectionVisibilityArgEntity : IUpdateCollectionVisibilityArgEntity
{
    public string CollectionId { get; init; }
    public string Visibility { get; init; }
}
