using Lib.Shared.DataModels.Entities.Args.Artists;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class ArtistSearchTermArgEntity : IArtistSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
