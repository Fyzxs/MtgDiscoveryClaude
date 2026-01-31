using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Entities.Args.Collections;

internal sealed class CreateCollectionArgEntity : ICreateCollectionArgEntity
{
    public string Name { get; init; }
    public string Type { get; init; }
    public string Visibility { get; init; }
}
