using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistSearchTermItrToXfrMapper : IArtistSearchTermItrToXfrMapper
{
    public Task<IArtistSearchTermXfrEntity> Map(IArtistSearchTermItrEntity source)
    {
        string normalized = source.SearchTerm;

        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        ArtistSearchTermXfrEntity mapped = new()
        {
            Normalized = normalized,
            SearchTerms = trigrams
        };

        return Task.FromResult<IArtistSearchTermXfrEntity>(mapped);
    }
}
