using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

internal sealed class RevokeCollectionAccessArgEntity : IRevokeCollectionAccessArgEntity
{
    public string CollectionId { get; init; }
    public string TargetUserId { get; init; }
}
