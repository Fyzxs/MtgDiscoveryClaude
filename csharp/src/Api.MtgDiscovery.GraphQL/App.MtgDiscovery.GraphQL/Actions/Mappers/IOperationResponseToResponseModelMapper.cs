using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IOperationResponseToResponseModelMapper<TData> : ICreateMapper<IOperationResponse<TData>, ResponseModel>
{
}
