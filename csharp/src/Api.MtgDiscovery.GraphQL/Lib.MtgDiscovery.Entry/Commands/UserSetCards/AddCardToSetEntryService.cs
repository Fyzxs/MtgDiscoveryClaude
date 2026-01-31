using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

/// <summary>
/// Entry service for adding a card to a user's set collection.
/// Used by migration tools for direct UserSetCards updates.
/// </summary>
internal sealed class AddCardToSetEntryService : IAddCardToSetEntryService
{
    private readonly IUserSetCardsDomainService _userSetCardsDomainService;
    private readonly IAddCardToSetArgToItrMapper _argToItrMapper;
    private readonly IUserSetCardOufToOutMapper _oufToOutMapper;

    public AddCardToSetEntryService(ILogger logger) : this(
        new UserSetCardsDomainService(logger),
        new AddCardToSetArgToItrMapper(),
        new UserSetCardOufToOutMapper())
    { }

    private AddCardToSetEntryService(
        IUserSetCardsDomainService userSetCardsDomainService,
        IAddCardToSetArgToItrMapper argToItrMapper,
        IUserSetCardOufToOutMapper oufToOutMapper)
    {
        _userSetCardsDomainService = userSetCardsDomainService;
        _argToItrMapper = argToItrMapper;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<UserSetCardOutEntity>> Execute(IAddCardToSetArgsEntity argsEntity)
    {
        IAddCardToSetItrEntity itrEntity = await _argToItrMapper.Map(argsEntity).ConfigureAwait(false);

        IOperationResponse<IUserSetCardOufEntity> domainResponse = await _userSetCardsDomainService.AddCardToSetAsync(itrEntity).ConfigureAwait(false);
        if (domainResponse.IsFailure)
            return new FailureOperationResponse<UserSetCardOutEntity>(domainResponse.OuterException);

        UserSetCardOutEntity outEntity = await _oufToOutMapper.Map(domainResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<UserSetCardOutEntity>(outEntity);
    }
}
