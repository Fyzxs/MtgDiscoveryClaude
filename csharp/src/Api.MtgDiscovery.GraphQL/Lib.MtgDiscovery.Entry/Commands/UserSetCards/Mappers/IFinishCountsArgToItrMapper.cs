using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal interface IFinishCountsArgToItrMapper : ICreateMapper<IFinishCountsArgEntity, IFinishCountsItrEntity>
{
}
