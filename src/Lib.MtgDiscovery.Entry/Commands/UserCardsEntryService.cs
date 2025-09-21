using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands;

internal sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IAddCardToCollectionEntryService _addCardToCollection;

    public UserCardsEntryService(ILogger logger) : this(
        new AddCardToCollectionEntryService(logger))
    { }

    private UserCardsEntryService(IAddCardToCollectionEntryService addCardToCollection)
    {
        _addCardToCollection = addCardToCollection;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args)
    {
        return await _addCardToCollection.Execute(new AddCardToCollectionArgsEntity { AuthUser = authUser, AddUserCard = args }).ConfigureAwait(false);
    }
}
