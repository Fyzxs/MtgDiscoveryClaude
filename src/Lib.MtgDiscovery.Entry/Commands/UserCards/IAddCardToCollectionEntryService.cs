using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal interface IAddCardToCollectionEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IAuthUserArgEntity authUser, IAddUserCardArgEntity args);
}
