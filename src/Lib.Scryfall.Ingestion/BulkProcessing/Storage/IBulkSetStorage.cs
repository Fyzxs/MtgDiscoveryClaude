using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal interface IBulkSetStorage
{
    void Add(IScryfallSet set);
    void AddCard(string setId, string cardId);
    IScryfallSet Get(string setId);
    IEnumerable<IScryfallSet> GetAll();
    bool Contains(string setId);
}