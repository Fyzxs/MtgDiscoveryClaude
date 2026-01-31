using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

internal sealed class RenameCollectionArgEntity : IRenameCollectionArgEntity
{
    public string CollectionId { get; init; }
    public string Name { get; init; }
}
