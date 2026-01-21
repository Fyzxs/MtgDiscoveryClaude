using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands;

internal sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IAddCardToCollectionArgEntityValidator _validator;
    private readonly IAddUserCardArgsToItrMapper _mapper;

    public UserCardsEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddUserCardArgsToItrMapper())
    { }

    private UserCardsEntryService(
        IUserCardsDomainService userCardsDomainService,
        IAddCardToCollectionArgEntityValidator validator,
        IAddUserCardArgsToItrMapper mapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<IUserCardOufEntity>> result = await _validator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IUserCardItrEntity mappedArgs = await _mapper.Map(authUser, args).ConfigureAwait(false);
        return await _userCardsDomainService.AddUserCardAsync(mappedArgs).ConfigureAwait(false);
    }
}
