using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal interface ISetItemExtToItrMapper : ICreateMapper<ScryfallSetItemExtEntity, ISetItemItrEntity>;
