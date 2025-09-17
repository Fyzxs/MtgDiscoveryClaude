using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserCardsQueryEntryService : IUserCardsQueryEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsSetArgEntityValidator _validator;
    private readonly IUserCardsSetArgToItrMapper _mapper;

    public UserCardsQueryEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsSetArgEntityValidatorContainer(),
        new UserCardsSetArgToItrMapper())
    { }

    private UserCardsQueryEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsSetArgEntityValidator validator,
        IUserCardsSetArgToItrMapper mapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetArgEntity setArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> result = await _validator.Validate(setArgs).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IUserCardsSetItrEntity itrEntity = await _mapper.Map(setArgs).ConfigureAwait(false);

        return await _userCardsDomainService.UserCardsBySetAsync(itrEntity).ConfigureAwait(false);
    }
}
