using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Dtos;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Interface for Scryfall list pagination.
/// </summary>
/// <typeparam name="T">The type of items to page through.</typeparam>
public interface IScryfallListPaging<out T> where T : IScryfallDto
{
    /// <summary>
    /// Gets the items from all pages.
    /// </summary>
    IAsyncEnumerable<T> Items();
}
