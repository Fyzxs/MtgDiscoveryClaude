namespace TestConvenience.Core.Fakes;

public sealed class MemoryCacheFake : IMemoryCache
{
    private readonly IMemoryCache _delegate = new MemoryCache(new MemoryCacheOptions());

    public int TryGetValueInvokeCount { get; set; }

    public void Dispose() => _delegate.Dispose();

    public bool TryGetValue(object key, out object value)
    {
        TryGetValueInvokeCount++;
        return _delegate.TryGetValue(key, out value);
    }

    public ICacheEntry CreateEntry(object key) => _delegate.CreateEntry(key);

    public void Remove(object key) => _delegate.Remove(key);
}
