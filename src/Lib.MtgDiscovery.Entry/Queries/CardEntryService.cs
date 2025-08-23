using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardDataService _cardDataService;
    private readonly CardsExtEntityValidator _validator;
    private readonly CardsArgsToItrMapper _mapper;

    public CardEntryService(ICardDataService cardDataService) : this(cardDataService, new CardsExtEntityValidator(), new CardsArgsToItrMapper())
    {
    }

    private CardEntryService(ICardDataService cardDataService, CardsExtEntityValidator validator, CardsArgsToItrMapper mapper)
    {
        _cardDataService = cardDataService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<OperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        //if (!_validator.IsValid(args))
        //{
        //    return new FailureOperationResponse<ICardItemCollectionItrEntity>(new InvalidCardIdsOperationResponseMessage());
        //}

        ICardIdsItrEntity mappedArgs = await _mapper.Map(args).ConfigureAwait(false);
        return await _cardDataService.CardsByIdsAsync(mappedArgs).ConfigureAwait(false);
    }
}
