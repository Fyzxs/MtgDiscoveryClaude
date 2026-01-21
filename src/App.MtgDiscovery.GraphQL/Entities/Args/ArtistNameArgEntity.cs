using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistNameArgEntity : IArtistNameArgEntity
{
    public string ArtistName { get; set; }
}
