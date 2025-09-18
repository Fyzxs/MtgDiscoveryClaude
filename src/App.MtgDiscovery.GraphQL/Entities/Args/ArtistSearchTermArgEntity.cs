using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class ArtistSearchTermArgEntity : IArtistSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
