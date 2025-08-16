using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Internal.Dtos;

namespace Lib.Scryfall.Ingestion.Internal.Paging;
internal interface IScryfallListPaging<out T> where T : IScryfallDto
{
    IAsyncEnumerable<T> Items();
}
