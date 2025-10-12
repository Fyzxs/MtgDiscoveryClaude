using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Artists;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal sealed class CardsByArtistNameEntryService : ICardsByArtistNameEntryService
{
    private readonly IArtistDomainService _artistDomainService;
    private readonly IArtistNameArgEntityValidator _artistNameArgEntityValidator;
    private readonly IArtistNameArgToItrMapper _artistNameArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardsByArtistNameEntryService(ILogger logger) : this(
        new ArtistDomainService(logger),
        new ArtistNameArgEntityValidatorContainer(),
        new ArtistNameArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardsByArtistNameEntryService(
        IArtistDomainService artistDomainService,
        IArtistNameArgEntityValidator artistNameArgEntityValidator,
        IArtistNameArgToItrMapper artistNameArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _artistDomainService = artistDomainService;
        _artistNameArgEntityValidator = artistNameArgEntityValidator;
        _artistNameArgToItrMapper = artistNameArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IArtistNameArgEntity artistName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _artistNameArgEntityValidator.Validate(artistName).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistNameItrEntity itrEntity = await _artistNameArgToItrMapper.Map(artistName).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _artistDomainService.CardsByArtistNameAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        // Use efficient query by artist ID if userId is present
        if (string.IsNullOrEmpty(artistName.UserId) is false)
        {
            // Extract all unique artist IDs from the returned cards
            IEnumerable<string> artistIds = outEntities
                .Where(card => card.ArtistIds is not null)
                .SelectMany(card => card.ArtistIds)
                .Distinct();

            // Enrich by each artist ID to collect all user cards for these artists
            foreach (string artistId in artistIds)
            {
                IUserCardsArtistItrEntity artistContext = new UserCardsArtistItrEntity
                {
                    UserId = artistName.UserId,
                    ArtistId = artistId
                };
                await _userCardEnrichment.EnrichByArtist(outEntities, artistContext).ConfigureAwait(false);
            }
        }

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    private sealed class UserCardsArtistItrEntity : IUserCardsArtistItrEntity
    {
        public string UserId { get; init; }
        public string ArtistId { get; init; }
    }
}
