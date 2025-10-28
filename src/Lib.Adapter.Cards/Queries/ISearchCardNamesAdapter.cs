using System.Collections.Generic;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Single-method adapter for searching card names using trigram matching.
/// </summary>
internal interface ISearchCardNamesAdapter
    : IOperationResponseService<ICardSearchTermXfrEntity, IEnumerable<string>>;
