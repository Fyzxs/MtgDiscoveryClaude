using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSetCards;

internal sealed class UserSetCardsQueryEntryService : IUserSetCardsQueryEntryService
{
    private readonly IUserSetCardEntryService _userSetCardEntryService;

    public UserSetCardsQueryEntryService(ILogger logger) : this(
        new UserSetCardEntryService(logger))
    { }

    private UserSetCardsQueryEntryService(IUserSetCardEntryService userSetCardEntryService) =>
        _userSetCardEntryService = userSetCardEntryService;

    public async Task<IOperationResponse<UserSetCardOutEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs) =>
        await _userSetCardEntryService.Execute(userSetCardArgs).ConfigureAwait(false);
}
