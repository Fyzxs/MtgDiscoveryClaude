using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Processors;

/// <summary>
/// Processes a Scryfall set.
/// </summary>
public interface ISetProcessor
{
    /// <summary>
    /// Processes the specified set.
    /// </summary>
    /// <param name="set">The set to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessAsync(IScryfallSet set);
}