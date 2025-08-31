using System.Collections.Generic;
using System.Linq;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkArtistStorage : IBulkArtistStorage
{
    private readonly Dictionary<string, BulkArtistData> _artistsByNormalizedName = new();

    public void AddArtistToCard(string artist, string cardId, string setId)
    {
        if (string.IsNullOrWhiteSpace(artist))
        {
            return;
        }

        string normalizedName = NormalizeArtistName(artist);

        if (_artistsByNormalizedName.TryGetValue(normalizedName, out BulkArtistData existingArtist) is false)
        {
            existingArtist = new BulkArtistData(artist);
            _artistsByNormalizedName[normalizedName] = existingArtist;
        }
        else
        {
            existingArtist.AddArtistName(artist);
        }

        existingArtist.AddCard(cardId, setId);
    }

    public IEnumerable<BulkArtistData> GetAll()
    {
        return _artistsByNormalizedName.Values;
    }

    private static string NormalizeArtistName(string artistName)
    {
        return artistName.ToLowerInvariant()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("'", "")
            .Replace(".", "")
            .Replace(",", "");
    }
}