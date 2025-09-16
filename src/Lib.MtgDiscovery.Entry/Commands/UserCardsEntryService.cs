using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands;

internal sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IAddCardToCollectionArgEntityValidator _validator;
    private readonly IAddCardToCollectionArgsToItrMapper _mapper;

    public UserCardsEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddCardToCollectionArgsToItrMapper())
    { }

    private UserCardsEntryService(
        IUserCardsDomainService userCardsDomainService,
        IAddCardToCollectionArgEntityValidator validator,
        IAddCardToCollectionArgsToItrMapper mapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddCardToCollectionArgEntity args)
    {
        // Validate the card collection args
        IValidatorActionResult<IOperationResponse<IUserCardCollectionItrEntity>> result = await _validator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        // Map args to ITR entity - mapper will combine authUser.UserId with args
        IUserCardCollectionItrEntity mappedArgs = await _mapper.Map(authUser, args).ConfigureAwait(false);
        return await _userCardsDomainService.AddUserCardAsync(mappedArgs).ConfigureAwait(false);
    }
}
