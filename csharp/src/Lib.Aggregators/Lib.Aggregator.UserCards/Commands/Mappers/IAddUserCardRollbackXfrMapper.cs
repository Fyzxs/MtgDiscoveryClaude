using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IAddUserCardRollbackXfrMapper : ICreateMapper<IAddUserCardXfrEntity, IAddUserCardXfrEntity>;
