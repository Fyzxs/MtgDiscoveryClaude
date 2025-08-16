using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Cosmos.Entities;
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
        if (_mapper.HasNoParentSet(set))
        {
            _logger.LogSetHasNoParent(set.Code());
            return;
        }

        ScryfallSetAssociation association = _mapper.Map(set);
        OpResponse<ScryfallSetAssociation> response = await _scribe.UpsertAsync(association).ConfigureAwait(false);

        if (response.IsSuccessful())
        {
            _logger.LogAssociationStored(set.Code(), association.ParentSetCode);
            return;
        }

        _logger.LogAssociationStoreFailed(set.Code(), response.StatusCode);
    }
}

internal static partial class SetAssociationsProcessorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Successfully stored association for set {Code} with parent {Parent}")]
    public static partial void LogAssociationStored(this ILogger logger, string code, string parent);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to store association for set {Code}. Status: {Status}")]
    public static partial void LogAssociationStoreFailed(this ILogger logger, string code, HttpStatusCode status);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Set {Code} has no parent set")]
    public static partial void LogSetHasNoParent(this ILogger logger, string code);
}
