using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal sealed class UserCardEntryService : IUserCardEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardArgEntityValidator _userCardArgEntityValidator;
    private readonly IUserCardArgToItrMapper _userCardArgToItrMapper;
    private readonly ICollectionUserCardOufToOutMapper _userCardOufToOutMapper;

    public UserCardEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardArgEntityValidatorContainer(),
        new UserCardArgToItrMapper(),
        new CollectionUserCardOufToOutMapper())
    { }

    private UserCardEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardArgEntityValidator userCardArgEntityValidator,
        IUserCardArgToItrMapper userCardArgToItrMapper,
        ICollectionUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _userCardArgEntityValidator = userCardArgEntityValidator;
        _userCardArgToItrMapper = userCardArgToItrMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> Execute(IUserCardArgEntity cardArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> validatorResult = await _userCardArgEntityValidator.Validate(cardArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<UserCardOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardItrEntity itrEntity = await _userCardArgToItrMapper.Map(cardArgs).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserCardOufEntity>> opResponse = await _userCardsDomainService.UserCardAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<UserCardOutEntity>>(opResponse.OuterException);

        List<UserCardOutEntity> outEntities = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserCardOutEntity>>(outEntities);
    }
}
