using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserCardsQueryEntryService : IUserCardsQueryEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsSetArgEntityValidator _setValidator;
    private readonly IUserCardsSetArgToItrMapper _setMapper;
    private readonly IUserCardArgEntityValidator _cardValidator;
    private readonly IUserCardArgToItrMapper _cardMapper;
    private readonly IUserCardsByIdsArgEntityValidator _idsValidator;
    private readonly IUserCardsByIdsArgToItrMapper _idsMapper;

    public UserCardsQueryEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsSetArgEntityValidatorContainer(),
        new UserCardsSetArgToItrMapper(),
        new UserCardArgEntityValidatorContainer(),
        new UserCardArgToItrMapper(),
        new UserCardsByIdsArgEntityValidatorContainer(),
        new UserCardsByIdsArgToItrMapper())
    { }

    private UserCardsQueryEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsSetArgEntityValidator setValidator,
        IUserCardsSetArgToItrMapper setMapper,
        IUserCardArgEntityValidator cardValidator,
        IUserCardArgToItrMapper cardMapper,
        IUserCardsByIdsArgEntityValidator idsValidator,
        IUserCardsByIdsArgToItrMapper idsMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _setValidator = setValidator;
        _setMapper = setMapper;
        _cardValidator = cardValidator;
        _cardMapper = cardMapper;
        _idsValidator = idsValidator;
        _idsMapper = idsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardArgEntity cardArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> result = await _cardValidator.Validate(cardArgs).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IUserCardItrEntity oufEntity = await _cardMapper.Map(cardArgs).ConfigureAwait(false);
        return await _userCardsDomainService.UserCardAsync(oufEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> result = await _setValidator.Validate(bySetArgs).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IUserCardsSetItrEntity itrEntity = await _setMapper.Map(bySetArgs).ConfigureAwait(false);
        return await _userCardsDomainService.UserCardsBySetAsync(itrEntity).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> result = await _idsValidator.Validate(cardsArgs).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IUserCardsByIdsItrEntity itrEntity = await _idsMapper.Map(cardsArgs).ConfigureAwait(false);
        return await _userCardsDomainService.UserCardsByIdsAsync(itrEntity).ConfigureAwait(false);
    }
}
