using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserWishlistCards.Commands.Mappers;

internal interface IRemoveUserWishlistCardXfrToReadPointMapper
    : ICreateMapper<IRemoveUserWishlistCardXfrEntity, ReadPointItem>;
