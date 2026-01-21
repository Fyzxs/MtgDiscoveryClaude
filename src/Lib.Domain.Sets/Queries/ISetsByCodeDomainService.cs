using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Marker interface for retrieving sets by code collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ISetsByCodeDomainService
    : IOperationResponseService<ISetCodesItrEntity, ISetItemCollectionOufEntity>;
