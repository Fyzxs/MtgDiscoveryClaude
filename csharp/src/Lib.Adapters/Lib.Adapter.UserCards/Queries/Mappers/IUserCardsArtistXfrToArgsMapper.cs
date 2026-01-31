using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal interface IUserCardsArtistXfrToArgsMapper : ICreateMapper<IUserCardsArtistXfrEntity, UserCardItemsByArtistExtEntitys>;
