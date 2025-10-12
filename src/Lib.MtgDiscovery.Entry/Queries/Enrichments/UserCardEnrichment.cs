using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardEnrichment : IUserCardEnrichment
{
    private readonly IUserCardByIdsEnrichment _byIdsEnrichment;
    private readonly IUserCardBySetEnrichment _bySetEnrichment;
    private readonly IUserCardByArtistEnrichment _byArtistEnrichment;
    private readonly IUserCardByNameEnrichment _byNameEnrichment;

    public UserCardEnrichment(ILogger logger) : this(
        new UserCardByIdsEnrichment(logger),
        new UserCardBySetEnrichment(logger),
        new UserCardByArtistEnrichment(logger),
        new UserCardByNameEnrichment(logger))
    {
    }

    private UserCardEnrichment(
        IUserCardByIdsEnrichment byIdsEnrichment,
        IUserCardBySetEnrichment bySetEnrichment,
        IUserCardByArtistEnrichment byArtistEnrichment,
        IUserCardByNameEnrichment byNameEnrichment)
    {
        _byIdsEnrichment = byIdsEnrichment;
        _bySetEnrichment = bySetEnrichment;
        _byArtistEnrichment = byArtistEnrichment;
        _byNameEnrichment = byNameEnrichment;
    }

    public async Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args) =>
        await _byIdsEnrichment.Enrich(outEntities, args).ConfigureAwait(false);

    public async Task EnrichBySet(List<CardItemOutEntity> outEntities, IUserCardsSetItrEntity context) =>
        await _bySetEnrichment.Enrich(outEntities, context).ConfigureAwait(false);

    public async Task EnrichByArtist(List<CardItemOutEntity> outEntities, IUserCardsArtistItrEntity context) =>
        await _byArtistEnrichment.Enrich(outEntities, context).ConfigureAwait(false);

    public async Task EnrichByName(List<CardItemOutEntity> outEntities, IUserCardsNameItrEntity context) =>
        await _byNameEnrichment.Enrich(outEntities, context).ConfigureAwait(false);
}
