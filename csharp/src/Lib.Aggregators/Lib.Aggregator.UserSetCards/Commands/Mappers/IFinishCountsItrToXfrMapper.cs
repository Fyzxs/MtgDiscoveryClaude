using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal interface IFinishCountsItrToXfrMapper : ICreateMapper<IFinishCountsItrEntity, IFinishCountsXfrEntity>
{
}
