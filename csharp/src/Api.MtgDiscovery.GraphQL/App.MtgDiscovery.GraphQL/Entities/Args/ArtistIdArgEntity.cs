using Lib.Shared.DataModels.Entities.Args.Artists;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class ArtistIdArgEntity : IArtistIdArgEntity
{
    public string ArtistId { get; set; }
    public string UserId { get; set; }
}
