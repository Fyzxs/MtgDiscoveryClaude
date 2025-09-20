using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Validators;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal sealed class AddCardToCollectionEntryService : IAddCardToCollectionEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IAddCardToCollectionArgEntityValidator _addCardToCollectionArgEntityValidator;
    private readonly IAddUserCardArgToItrMapper _addUserCardArgToItrMapper;
    private readonly IUserCardOufToOutMapper _userCardOufToOutMapper;

    public AddCardToCollectionEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddUserCardArgToItrMapper(),
        new UserCardOufToOutMapper())
    { }

    private AddCardToCollectionEntryService(
        IUserCardsDomainService userCardsDomainService,
        IAddCardToCollectionArgEntityValidator addCardToCollectionArgEntityValidator,
        IAddUserCardArgToItrMapper addUserCardArgToItrMapper,
        IUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _addCardToCollectionArgEntityValidator = addCardToCollectionArgEntityValidator;
        _addUserCardArgToItrMapper = addUserCardArgToItrMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<UserCardOutEntity>> Execute(AddCardToCollectionArgsEntity input)
    {
        IValidatorActionResult<IOperationResponse<IUserCardOufEntity>> validatorResult = await _addCardToCollectionArgEntityValidator.Validate(input.AddUserCard).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<UserCardOutEntity>(validatorResult.FailureStatus().OuterException);

        IUserCardItrEntity itrEntity = await _addUserCardArgToItrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IUserCardOufEntity> opResponse = await _userCardsDomainService.AddUserCardAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<UserCardOutEntity>(opResponse.OuterException);

        UserCardOutEntity outEntity = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<UserCardOutEntity>(outEntity);
    }
}
