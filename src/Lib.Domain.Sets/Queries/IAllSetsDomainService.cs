using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Marker interface for retrieving all sets.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IAllSetsDomainService
    : IOperationResponseService<IAllSetsItrEntity, ISetItemCollectionOufEntity>;
