using System.Collections.Generic;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by set code.
/// </summary>
internal interface ICardsBySetCodeAdapter
    : IOperationResponseService<ISetCodeXfrEntity, IEnumerable<ScryfallSetCardItemExtEntity>>;
