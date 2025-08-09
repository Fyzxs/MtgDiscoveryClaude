using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Ids;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;

namespace Lib.BlobStorage.Tests.Fakes;

public sealed class BlobContainerExistsOperatorFake : IBlobContainerExistsOperator
{
    public BlobOpResponse<bool> ExistsAsyncResult { get; init; }
    public int ExistsAsyncInvokeCount { get; private set; }

    public async Task<BlobOpResponse<bool>> ExistsAsync(BlobPathEntity blobItemPath)
    {
        ExistsAsyncInvokeCount++;
        await Task.CompletedTask.ConfigureAwait(false);
        return ExistsAsyncResult;
    }
}