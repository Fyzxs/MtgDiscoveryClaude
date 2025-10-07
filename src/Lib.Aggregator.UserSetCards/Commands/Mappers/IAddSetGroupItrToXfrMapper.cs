using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal interface IAddSetGroupItrToXfrMapper : ICreateMapper<IAddSetGroupToUserSetCardItrEntity, IAddSetGroupToUserSetCardXfrEntity>;
