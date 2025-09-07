using System.Collections.Generic;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Models;

internal sealed class ScryfallRuling : IScryfallRuling
{
    public string OracleId { get; init; }
    public ICollection<dynamic> Rulings { get; init; }
}
