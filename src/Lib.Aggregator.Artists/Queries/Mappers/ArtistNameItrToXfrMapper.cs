using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistNameItrToXfrMapper : IArtistNameItrToXfrMapper
{
    public Task<IArtistNameXfrEntity> Map(IArtistNameItrEntity source)
    {
        string normalized = new([.. source.ArtistName.ToLowerInvariant().Where(char.IsLetter)]);

        List<string> trigrams = [];
        for (int i = 0; i <= normalized.Length - 3; i++)
        {
            trigrams.Add(normalized.Substring(i, 3));
        }

        return Task.FromResult<IArtistNameXfrEntity>(new ArtistNameXfrEntity
        {
            ArtistName = source.ArtistName,
            Normalized = normalized,
            Trigrams = trigrams
        });
    }
}
