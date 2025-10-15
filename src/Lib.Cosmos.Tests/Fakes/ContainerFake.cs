using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Cosmos.Tests.Fakes;

public sealed class ItemResponseFake<T> : ItemResponse<T>
{
    public T ResourceResult { get; init; }
    public HttpStatusCode StatusCodeResult { get; init; }
    public double RequestChargeResult { get; init; }
    public CosmosDiagnostics DiagnosticsResult { get; init; }

    public override T Resource => ResourceResult;
    public override HttpStatusCode StatusCode => StatusCodeResult;
    public override double RequestCharge => RequestChargeResult;
    public override CosmosDiagnostics Diagnostics => DiagnosticsResult;
}

public sealed class FeedIteratorFake<T> : FeedIterator<T>
{
    public FeedResponse<T> ReadNextAsyncResponse { get; init; }

    private bool _hasReturned;

    public override Task<FeedResponse<T>> ReadNextAsync(CancellationToken cancellationToken = default)
    {
        _hasReturned = true;
        return Task.FromResult(ReadNextAsyncResponse);
    }

    public override bool HasMoreResults => _hasReturned is false;
}

public sealed class FeedResponseFake<T> : FeedResponse<T>
{
    private readonly Collection<T> _documents;
    public FeedResponseFake(Collection<T> documents) => _documents = documents;

    public double RequestChargeResult { get; init; }
    public CosmosDiagnostics DiagnosticsResult { get; init; }

    // ReSharper disable UnassignedGetOnlyAutoProperty
    public override string ContinuationToken { get; }
    public override int Count => _documents.Count;
    public override string IndexMetrics { get; }
    public override Headers Headers { get; }
    public override IEnumerable<T> Resource => _documents;
    public HttpStatusCode StatusCodeResult { get; init; }
    public override HttpStatusCode StatusCode => StatusCodeResult;
    public override double RequestCharge => RequestChargeResult;

    public override CosmosDiagnostics Diagnostics => DiagnosticsResult;

    public override IEnumerator<T> GetEnumerator() => _documents.GetEnumerator();
    // ReSharper restore UnassignedGetOnlyAutoProperty
}

public sealed class ContainerFake<TType> : Container
{
    public ItemResponse<TType> UpsertItemAsyncResponse { get; init; }
    public int UpsertItemAsyncInvokeCount { get; private set; }

    public ItemResponse<TType> DeleteItemAsyncResponse { get; init; }
    public int DeleteItemAsyncInvokeCount { get; private set; }

    public ItemResponse<TType> ReadItemAsyncResponse { get; init; }
    public int ReadItemAsyncInvokeCount { get; private set; }

    public FeedIterator<TType> GetItemQueryIteratorResponse { get; init; }
    public int GetItemQueryIteratorInvokeCount { get; private set; }

    public override Task<ItemResponse<T>> UpsertItemAsync<T>(T item, PartitionKey? partitionKey = null,
        ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpsertItemAsyncInvokeCount++;
        return Task.FromResult(UpsertItemAsyncResponse as ItemResponse<T>);
    }

    public override FeedIterator<T> GetItemQueryIterator<T>(QueryDefinition queryDefinition,
        string continuationToken = null,
        QueryRequestOptions requestOptions = null)
    {
        GetItemQueryIteratorInvokeCount++;
        return GetItemQueryIteratorResponse as FeedIterator<T>;
    }

    public override Task<ItemResponse<T>> DeleteItemAsync<T>(string id, PartitionKey partitionKey,
        ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DeleteItemAsyncInvokeCount++;
        return Task.FromResult(DeleteItemAsyncResponse as ItemResponse<T>);
    }

    public override Task<ItemResponse<T>> ReadItemAsync<T>(string id, PartitionKey partitionKey, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ReadItemAsyncInvokeCount++;
        return Task.FromResult(ReadItemAsyncResponse as ItemResponse<T>);
    }

    #region Not Yet Used
    public override Task<ContainerResponse> ReadContainerAsync(ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> ReadContainerStreamAsync(ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ContainerResponse> ReplaceContainerAsync(ContainerProperties containerProperties, ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> ReplaceContainerStreamAsync(ContainerProperties containerProperties, ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ContainerResponse> DeleteContainerAsync(ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> DeleteContainerStreamAsync(ContainerRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<int?> ReadThroughputAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public override Task<ThroughputResponse> ReadThroughputAsync(RequestOptions requestOptions, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public override Task<ThroughputResponse> ReplaceThroughputAsync(int throughput, RequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ThroughputResponse> ReplaceThroughputAsync(ThroughputProperties throughputProperties, RequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> CreateItemStreamAsync(Stream streamPayload, PartitionKey partitionKey, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ItemResponse<T>> CreateItemAsync<T>(T item, PartitionKey? partitionKey = null, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> ReadItemStreamAsync(string id, PartitionKey partitionKey, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> UpsertItemStreamAsync(Stream streamPayload, PartitionKey partitionKey, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();
    public override Task<ResponseMessage> ReplaceItemStreamAsync(Stream streamPayload, string id, PartitionKey partitionKey,
        ItemRequestOptions requestOptions = null, CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ItemResponse<T>> ReplaceItemAsync<T>(T item, string id, PartitionKey? partitionKey = null, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> ReadManyItemsStreamAsync(IReadOnlyList<(string id, PartitionKey partitionKey)> items, ReadManyRequestOptions readManyRequestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<FeedResponse<T>> ReadManyItemsAsync<T>(IReadOnlyList<(string id, PartitionKey partitionKey)> items, ReadManyRequestOptions readManyRequestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ItemResponse<T>> PatchItemAsync<T>(string id, PartitionKey partitionKey, IReadOnlyList<PatchOperation> patchOperations,
        PatchItemRequestOptions requestOptions = null, CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> PatchItemStreamAsync(string id, PartitionKey partitionKey, IReadOnlyList<PatchOperation> patchOperations,
        PatchItemRequestOptions requestOptions = null, CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override Task<ResponseMessage> DeleteItemStreamAsync(string id, PartitionKey partitionKey, ItemRequestOptions requestOptions = null,
        CancellationToken cancellationToken = new CancellationToken()) =>
        throw new NotImplementedException();

    public override FeedIterator GetItemQueryStreamIterator(QueryDefinition queryDefinition, string continuationToken = null,
        QueryRequestOptions requestOptions = null) =>
        throw new NotImplementedException();

    public override FeedIterator GetItemQueryStreamIterator(string queryText = null, string continuationToken = null,
        QueryRequestOptions requestOptions = null) =>
        throw new NotImplementedException();

    public override FeedIterator<T> GetItemQueryIterator<T>(string queryText = null, string continuationToken = null,
        QueryRequestOptions requestOptions = null) =>
        throw new NotImplementedException();

    public override FeedIterator GetItemQueryStreamIterator(FeedRange feedRange, QueryDefinition queryDefinition, string continuationToken,
        QueryRequestOptions requestOptions = null) =>
        throw new NotImplementedException();

    public override FeedIterator<T> GetItemQueryIterator<T>(FeedRange feedRange, QueryDefinition queryDefinition,
        string continuationToken = null, QueryRequestOptions requestOptions = null) =>
        throw new NotImplementedException();

    public override IOrderedQueryable<T> GetItemLinqQueryable<T>(bool allowSynchronousQueryExecution = false, string continuationToken = null,
        QueryRequestOptions requestOptions = null, CosmosLinqSerializerOptions linqSerializerOptions = null) =>
        throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder<T>(string processorName, ChangesHandler<T> onChangesDelegate) => throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedEstimatorBuilder(string processorName,
        ChangesEstimationHandler estimationDelegate, TimeSpan? estimationPeriod = null) =>
        throw new NotImplementedException();

    public override ChangeFeedEstimator GetChangeFeedEstimator(string processorName, Container leaseContainer) => throw new NotImplementedException();

    public override TransactionalBatch CreateTransactionalBatch(PartitionKey partitionKey) => throw new NotImplementedException();

    public override Task<IReadOnlyList<FeedRange>> GetFeedRangesAsync(CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public override FeedIterator GetChangeFeedStreamIterator(ChangeFeedStartFrom changeFeedStartFrom, ChangeFeedMode changeFeedMode,
        ChangeFeedRequestOptions changeFeedRequestOptions = null) =>
        throw new NotImplementedException();

    public override FeedIterator<T> GetChangeFeedIterator<T>(ChangeFeedStartFrom changeFeedStartFrom, ChangeFeedMode changeFeedMode,
        ChangeFeedRequestOptions changeFeedRequestOptions = null) =>
        throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder<T>(string processorName, ChangeFeedHandler<T> onChangesDelegate) => throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilderWithManualCheckpoint<T>(string processorName,
        ChangeFeedHandlerWithManualCheckpoint<T> onChangesDelegate) =>
        throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder(string processorName,
        ChangeFeedStreamHandler onChangesDelegate) =>
        throw new NotImplementedException();

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilderWithManualCheckpoint(string processorName,
        ChangeFeedStreamHandlerWithManualCheckpoint onChangesDelegate) =>
        throw new NotImplementedException();

    public override string Id { get; }
    public override Database Database { get; }
    public override Conflicts Conflicts { get; }
    public override Scripts Scripts { get; }
    #endregion
}
