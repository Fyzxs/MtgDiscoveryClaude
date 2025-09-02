using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistIdArgEntity : IArtistIdArgEntity
{
    public string ArtistId { get; set; }
}