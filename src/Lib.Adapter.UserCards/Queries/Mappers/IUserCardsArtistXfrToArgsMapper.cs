using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal interface IUserCardsArtistXfrToArgsMapper : ICreateMapper<IUserCardsArtistXfrEntity, UserCardItemsByArtistExtEntitys>;
