using Lib.Shared.DataModels.Entities.Args.Artists;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class ArtistNameArgEntity : IArtistNameArgEntity
{
    public string ArtistName { get; set; }
    public string UserId { get; set; }
}
