using System;
using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkArtistData
{
    public string ArtistId { get; }
    public HashSet<string> ArtistNames { get; }
    public HashSet<string> CardIds { get; }
    public HashSet<string> SetIds { get; }

    public BulkArtistData(string artistName)
    {
        ArtistId = Guid.NewGuid().ToString();
        ArtistNames = new HashSet<string> { artistName };
        CardIds = new HashSet<string>();
        SetIds = new HashSet<string>();
    }

    public void AddCard(string cardId, string setId)
    {
        CardIds.Add(cardId);
        SetIds.Add(setId);
    }

    public void AddArtistName(string artistName)
    {
        ArtistNames.Add(artistName);
    }
}