using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistSearchTermArgEntity : IArtistSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
