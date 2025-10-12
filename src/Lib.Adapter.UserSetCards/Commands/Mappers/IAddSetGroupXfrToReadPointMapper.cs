using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal interface IAddSetGroupXfrToReadPointMapper : ICreateMapper<IAddSetGroupToUserSetCardXfrEntity, ReadPointItem>;
