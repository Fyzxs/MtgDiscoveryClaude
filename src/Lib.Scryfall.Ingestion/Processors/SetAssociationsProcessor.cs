using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Processors;

internal sealed class SetAssociationsProcessor : ISetProcessor
{
    private readonly ICosmosScribe _scribe;
    private readonly IScryfallSetToAssociationMapper _mapper;
    private readonly ILogger _logger;

    public SetAssociationsProcessor(ILogger logger)
        : this(
            new ScryfallSetAssociationsScribe(logger),
            new ScryfallSetToAssociationMapper(),
            logger)
    {
    }

    private SetAssociationsProcessor(
        ICosmosScribe scribe,
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

        ScryfallSetParentAssociationItem parentAssociationItem = _mapper.Map(set);
        OpResponse<ScryfallSetParentAssociationItem> response = await _scribe.UpsertAsync(parentAssociationItem).ConfigureAwait(false);

        if (response.IsSuccessful())
        {
            _logger.LogAssociationStored(set.Code(), parentAssociationItem.ParentSetCode);
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
