using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs;
using App.MtgDiscovery.GraphQL.Entities.Types;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;
#pragma warning disable CA1515

namespace App.MtgDiscovery.GraphQL.Queries;

public class CardQuery;

[ExtendObjectType(typeof(CardQuery))]
public class CardQueryMethods
{
    private readonly IScryfallCardMapper _scryfallCardMapper;
    private readonly IEntryService _entryService;

    public CardQueryMethods(ILogger logger) : this(new ScryfallCardMapper(), new EntryService(logger))
    {
    }

    private CardQueryMethods(IScryfallCardMapper scryfallCardMapper, IEntryService entryService)
    {
        _scryfallCardMapper = scryfallCardMapper;
        _entryService = entryService;
    }

    public string Test() => "Card query endpoint is working!";

    [GraphQLType(typeof(ResponseModelUnionType))]
    public async Task<ResponseModel> CardsById(CardIdsArgEntity ids)
    {
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<ScryfallCardOutEntity> results = [];

        foreach (ICardItemItrEntity cardItem in response.ResponseData.Data)
        {
            ScryfallCardOutEntity outEntity = await _scryfallCardMapper.Map(cardItem).ConfigureAwait(false);
            results.Add(outEntity);
        }

        return new SuccessDataResponseModel<List<ScryfallCardOutEntity>>() { Data = results };
    }
}
