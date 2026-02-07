using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserSealedProducts.Commands.Mappers;

internal interface IUserSealedProductXfrToDeletePointMapper
    : ICreateMapper<IUserSealedProductXfrEntity, DeletePointItem>;
