using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.UserSetCards.Queries.Mappers;

internal interface IUserSetCardsGetXfrToExtMapper : ICreateMapper<IUserSetCardGetXfrEntity, ReadPointItem>;
