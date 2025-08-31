using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal interface IBulkArtistStorage
{
    void AddArtistToCard(string artist, string cardId, string setId);
    IEnumerable<BulkArtistData> GetAll();
}