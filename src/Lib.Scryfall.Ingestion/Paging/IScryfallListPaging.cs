using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Dtos;

namespace Lib.Scryfall.Ingestion.Paging;
internal interface IScryfallListPaging<out T> where T : IScryfallDto
{
    IAsyncEnumerable<T> Items();
}
