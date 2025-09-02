using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface IArtistsPipelineService
{
    void TrackArtist(IScryfallCard card);
    Task WriteArtistsAsync();
}