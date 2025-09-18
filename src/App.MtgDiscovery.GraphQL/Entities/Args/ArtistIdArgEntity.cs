using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistIdArgEntity : IArtistIdArgEntity
{
    public string ArtistId { get; set; }
}
