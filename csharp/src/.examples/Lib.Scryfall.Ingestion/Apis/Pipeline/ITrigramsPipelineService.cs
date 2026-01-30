using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface ITrigramsPipelineService
{
    void TrackCard(IScryfallCard card);
    Task WriteTrigramsAsync();
}
