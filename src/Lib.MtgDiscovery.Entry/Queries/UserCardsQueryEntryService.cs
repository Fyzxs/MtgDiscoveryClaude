using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserCardsQueryEntryService : IUserCardsQueryEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsSetArgEntityValidator _userCardsSetArgEntityValidator;
    private readonly IUserCardsSetArgToItrMapper _userCardsSetArgToItrMapper;
    private readonly IUserCardArgEntityValidator _userCardArgEntityValidator;
    private readonly IUserCardArgToItrMapper _userCardArgToItrMapper;
    private readonly IUserCardsByIdsArgEntityValidator _userCardsByIdsArgEntityValidator;
    private readonly IUserCardsByIdsArgToItrMapper _userCardsByIdsArgToItrMapper;
    private readonly ICollectionUserCardOufToOutMapper _userCardOufToOutMapper;

    public UserCardsQueryEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsSetArgEntityValidatorContainer(),
        new UserCardsSetArgToItrMapper(),
        new UserCardArgEntityValidatorContainer(),
        new UserCardArgToItrMapper(),
        new UserCardsByIdsArgEntityValidatorContainer(),
        new UserCardsByIdsArgToItrMapper(),
        new CollectionUserCardOufToOutMapper())
    { }

    private UserCardsQueryEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsSetArgEntityValidator userCardsSetArgEntityValidator,
        IUserCardsSetArgToItrMapper userCardsSetArgToItrMapper,
        IUserCardArgEntityValidator userCardArgEntityValidator,
        IUserCardArgToItrMapper userCardArgToItrMapper,
        IUserCardsByIdsArgEntityValidator userCardsByIdsArgEntityValidator,
        IUserCardsByIdsArgToItrMapper userCardsByIdsArgToItrMapper,
        ICollectionUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _userCardsSetArgEntityValidator = userCardsSetArgEntityValidator;
        _userCardsSetArgToItrMapper = userCardsSetArgToItrMapper;
        _userCardArgEntityValidator = userCardArgEntityValidator;
        _userCardArgToItrMapper = userCardArgToItrMapper;
        _userCardsByIdsArgEntityValidator = userCardsByIdsArgEntityValidator;
        _userCardsByIdsArgToItrMapper = userCardsByIdsArgToItrMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> validatorResult = await _userCardArgEntityValidator.Validate(cardArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<UserCardOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardItrEntity itrEntity = await _userCardArgToItrMapper.Map(cardArgs).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserCardOufEntity>> opResponse = await _userCardsDomainService.UserCardAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<UserCardOutEntity>>(opResponse.OuterException);

        List<UserCardOutEntity> outEntities = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserCardOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> validatorResult = await _userCardsSetArgEntityValidator.Validate(bySetArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<UserCardOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardsSetItrEntity itrEntity = await _userCardsSetArgToItrMapper.Map(bySetArgs).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserCardOufEntity>> opResponse = await _userCardsDomainService.UserCardsBySetAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<UserCardOutEntity>>(opResponse.OuterException);

        List<UserCardOutEntity> outEntities = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserCardOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> validatorResult = await _userCardsByIdsArgEntityValidator.Validate(cardsArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<UserCardOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardsByIdsItrEntity itrEntity = await _userCardsByIdsArgToItrMapper.Map(cardsArgs).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserCardOufEntity>> opResponse = await _userCardsDomainService.UserCardsByIdsAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<UserCardOutEntity>>(opResponse.OuterException);

        List<UserCardOutEntity> outEntities = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserCardOutEntity>>(outEntities);
    }
}
