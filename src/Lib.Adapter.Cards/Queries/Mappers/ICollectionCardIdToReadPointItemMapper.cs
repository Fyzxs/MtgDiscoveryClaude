using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Adapter.Cards.Queries.Mappers;

internal interface ICollectionCardIdToReadPointItemMapper : ICreateMapper<IEnumerable<string>, ICollection<ReadPointItem>>;
