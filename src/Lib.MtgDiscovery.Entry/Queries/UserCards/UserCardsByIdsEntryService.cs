using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal sealed class UserCardsByIdsEntryService : IUserCardsByIdsEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsByIdsArgEntityValidator _userCardsByIdsArgEntityValidator;
    private readonly IUserCardsByIdsArgToItrMapper _userCardsByIdsArgToItrMapper;
    private readonly ICollectionUserCardOufToOutMapper _userCardOufToOutMapper;

    public UserCardsByIdsEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsByIdsArgEntityValidatorContainer(),
        new UserCardsByIdsArgToItrMapper(),
        new CollectionUserCardOufToOutMapper())
    { }

    private UserCardsByIdsEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsByIdsArgEntityValidator userCardsByIdsArgEntityValidator,
        IUserCardsByIdsArgToItrMapper userCardsByIdsArgToItrMapper,
        ICollectionUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _userCardsByIdsArgEntityValidator = userCardsByIdsArgEntityValidator;
        _userCardsByIdsArgToItrMapper = userCardsByIdsArgToItrMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> Execute(IUserCardsByIdsArgEntity cardsArgs)
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
