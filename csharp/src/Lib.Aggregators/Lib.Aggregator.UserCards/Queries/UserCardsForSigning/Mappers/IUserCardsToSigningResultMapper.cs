using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Mappers;

/// <summary>
/// Mapper that transforms raw user card entities into grouped signing result entities.
/// Groups cards by set, then by artist within each set, calculating signing statistics.
/// Note: Card names, image URIs, set codes, set names, and artist names are populated
/// with empty strings and enriched at the Entry layer using card data.
/// </summary>
internal interface IUserCardsToSigningResultMapper
{
    /// <summary>
    /// Maps user card entities to a grouped signing result, organizing by set and artist
    /// with calculated statistics for unsigned copies and partially signed cards.
    /// </summary>
    /// <param name="userCards">The raw user card entities from storage</param>
    /// <param name="selectedArtistIds">The artist IDs selected for the convention</param>
    /// <returns>The grouped signing result with statistics (names/images to be enriched at Entry layer)</returns>
    Task<ISigningResultOufEntity> Map(IEnumerable<UserCardExtEntity> userCards, IEnumerable<string> selectedArtistIds);
}
