using System.Threading.Tasks;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class OperationResponseToResponseModelMapper<TData> : IOperationResponseToResponseModelMapper<TData>
{
    public Task<ResponseModel> Map(IOperationResponse<TData> source)
    {
        if (source.IsSuccess) return Task.FromResult((ResponseModel)new SuccessDataResponseModel<TData> { Data = source.ResponseData });

        return Task.FromResult((ResponseModel)new FailureResponseModel
        {
            Status = new StatusDataModel
            {
                Message = source.OuterException.StatusMessage,
                StatusCode = source.OuterException.StatusCode
            }
        });
    }
}
