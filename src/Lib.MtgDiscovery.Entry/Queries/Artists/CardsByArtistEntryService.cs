using System.Collections.Generic;
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

internal sealed class CardsByArtistEntryService : ICardsByArtistEntryService
{
    private readonly IArtistDomainService _artistDomainService;
    private readonly IArtistIdArgEntityValidator _artistIdArgEntityValidator;
    private readonly IArtistIdArgToItrMapper _artistIdArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardsByArtistEntryService(ILogger logger) : this(
        new ArtistDomainService(logger),
        new ArtistIdArgEntityValidatorContainer(),
        new ArtistIdArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardsByArtistEntryService(
        IArtistDomainService artistDomainService,
        IArtistIdArgEntityValidator artistIdArgEntityValidator,
        IArtistIdArgToItrMapper artistIdArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _artistDomainService = artistDomainService;
        _artistIdArgEntityValidator = artistIdArgEntityValidator;
        _artistIdArgToItrMapper = artistIdArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IArtistIdArgEntity artistId)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _artistIdArgEntityValidator.Validate(artistId).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistIdItrEntity itrEntity = await _artistIdArgToItrMapper.Map(artistId).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _artistDomainService.CardsByArtistAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        await _userCardEnrichment.Enrich(outEntities, artistId).ConfigureAwait(false);

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}