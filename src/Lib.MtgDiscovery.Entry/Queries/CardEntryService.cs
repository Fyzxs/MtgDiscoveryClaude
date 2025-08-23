using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardDataService _cardDataService;
    private readonly ICardIdsArgEntityValidator _validator;
    private readonly ICardsArgsToItrMapper _mapper;

    public CardEntryService(ILogger logger) : this(new DataService(logger), new CardIdsArgEntityValidatorContainer(), new CardsArgsToItrMapper())
    {
    }

    private CardEntryService(ICardDataService cardDataService, ICardIdsArgEntityValidator validator, ICardsArgsToItrMapper mapper)
    {
        _cardDataService = cardDataService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _validator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ICardIdsItrEntity mappedArgs = await _mapper.Map(args).ConfigureAwait(false);
        return await _cardDataService.CardsByIdsAsync(mappedArgs).ConfigureAwait(false);
    }
}
