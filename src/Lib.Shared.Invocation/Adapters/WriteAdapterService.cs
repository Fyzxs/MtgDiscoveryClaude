using System.Threading.Tasks;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.Invocation.Commands;

namespace Lib.Shared.Invocation.Adapters;

public abstract class WriteAdapterService<TRequestDomain, TCosmosItem> where TCosmosItem : CosmosItem, new()
{
    private readonly ICosmosScribe _scribe;
    private readonly IMapper<TRequestDomain, TCosmosItem> _mapper;
    private readonly ICommandValidatorAction<TRequestDomain> _validator;
    private readonly ICosmosRequestStatusFactory _factory;

    protected WriteAdapterService(ICosmosScribe scribe,
        IMapper<TRequestDomain, TCosmosItem> mapper,
        ICommandValidatorAction<TRequestDomain> validator,
        ICosmosRequestStatusFactory factory)
    {
        _scribe = scribe;
        _mapper = mapper;
        _validator = validator;
        _factory = factory;
    }

    public async Task<CommandOperationStatus> Write(TRequestDomain domain)
    {
        IValidatorActionResult<CommandOperationStatus> result = await _validator.Validate(domain).ConfigureAwait(false);
        if (result.IsNotValid()) return result.FailureStatus();

        TCosmosItem cosmosItem = new();
        await _mapper.Map(domain, cosmosItem).ConfigureAwait(false);
        OpResponse<TCosmosItem> cosmosResponse = await _scribe.UpsertAsync(cosmosItem).ConfigureAwait(false);
        if (cosmosResponse.IsNotSuccessful()) return _factory.CosmosFailure(cosmosResponse);

        return _factory.CosmosSuccess();
    }
}
