using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

public sealed class GrantCollectionAccessArgEntity : IGrantCollectionAccessArgEntity
{
    public string CollectionId { get; init; }
    public string TargetUserId { get; init; }
    public string Role { get; init; }
}
