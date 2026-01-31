using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

internal sealed class DeleteCollectionArgEntity : IDeleteCollectionArgEntity
{
    public string CollectionId { get; init; }
}
