using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserCardsEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args);
}
