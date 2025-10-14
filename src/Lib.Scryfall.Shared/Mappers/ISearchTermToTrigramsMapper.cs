using Lib.Scryfall.Shared.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Scryfall.Shared.Mappers;

public interface ISearchTermToTrigramsMapper : ICreateMapper<string, ITrigramCollectionEntity>
{
}
