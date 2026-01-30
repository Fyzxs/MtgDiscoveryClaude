using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CollectionIdArgEntity : ICollectionIdArgEntity
{
    public string CollectionId { get; set; }
    public string UserId { get; set; }
}
