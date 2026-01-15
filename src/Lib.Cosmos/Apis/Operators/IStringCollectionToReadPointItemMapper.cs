using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Cosmos.Apis.Operators;

public interface IStringCollectionToReadPointItemMapper : ICreateMapper<IEnumerable<string>, ICollection<ReadPointItem>>;
