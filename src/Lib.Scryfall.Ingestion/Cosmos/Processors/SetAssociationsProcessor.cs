using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Mappers;
using Lib.Scryfall.Ingestion.Cosmos.Operators;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Cosmos.Processors;

internal sealed class SetAssociationsProcessor : ISetAssociationsProcessor
{
    private readonly IScryfallSetAssociationsScribe _scribe;
    private readonly IScryfallSetToAssociationMapper _mapper;
    private readonly ILogger _logger;

    public SetAssociationsProcessor(
        IScryfallSetAssociationsScribe scribe,
        IScryfallSetToAssociationMapper mapper,
        ILogger logger)
    {
        _scribe = scribe;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ProcessAsync(IScryfallSet set)
    {
        if (_mapper.HasParentSet(set))
        {
            Entities.ScryfallSetAssociation association = _mapper.Map(set);
            Lib.Cosmos.Apis.Operators.OpResponse<Entities.ScryfallSetAssociation> response = await _scribe.UpsertAsync(association).ConfigureAwait(false);

            if (response.IsSuccessful())
            {
#pragma warning disable CA1848 // Use LoggerMessage delegates
                _logger.LogInformation("Successfully stored association for set {Code} with parent {Parent}",
                    set.Code(), association.ParentSetCode);
#pragma warning restore CA1848
            }
            else
            {
#pragma warning disable CA1848 // Use LoggerMessage delegates
                _logger.LogError("Failed to store association for set {Code}. Status: {Status}",
                    set.Code(), response.StatusCode);
#pragma warning restore CA1848
            }
        }
        else
        {
#pragma warning disable CA1848 // Use LoggerMessage delegates
            _logger.LogDebug("Set {Code} has no parent set", set.Code());
#pragma warning restore CA1848
        }
    }
}
