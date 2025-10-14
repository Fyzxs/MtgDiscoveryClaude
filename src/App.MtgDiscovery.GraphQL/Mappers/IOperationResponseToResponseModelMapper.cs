using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IOperationResponseToResponseModelMapper<TData> : ICreateMapper<IOperationResponse<TData>, ResponseModel>
{
}
