using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSetCards;

internal sealed class UserSetCardsQueryEntryService : IUserSetCardsQueryEntryService
{
    private readonly IUserSetCardEntryService _userSetCardEntryService;
    private readonly IAllUserSetCardsEntryService _allUserSetCardsEntryService;

    public UserSetCardsQueryEntryService(ILogger logger) : this(
        new UserSetCardEntryService(logger),
        new AllUserSetCardsEntryService(logger))
    {
    }

    private UserSetCardsQueryEntryService(
        IUserSetCardEntryService userSetCardEntryService,
        IAllUserSetCardsEntryService allUserSetCardsEntryService)
    {
        _userSetCardEntryService = userSetCardEntryService;
        _allUserSetCardsEntryService = allUserSetCardsEntryService;
    }

    public async Task<IOperationResponse<UserSetCardOutEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs) =>
        await _userSetCardEntryService.Execute(userSetCardArgs).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserSetCardOutEntity>>> GetAllUserSetCardsAsync(IAllUserSetCardsArgEntity userSetCardsArgs) =>
        await _allUserSetCardsEntryService.Execute(userSetCardsArgs).ConfigureAwait(false);
}
