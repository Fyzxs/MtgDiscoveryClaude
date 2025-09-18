using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistNameArgEntity : IArtistNameArgEntity
{
    public string ArtistName { get; set; }
}
