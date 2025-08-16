using Lib.BlobStorage.Apis.Operations;

namespace Example.BlobStorage;

public interface IDemoBlobScribe : IBlobWriteScribe { }

internal sealed class DemoBlobScribe : BlobWriteScribe, IDemoBlobScribe
{
    public DemoBlobScribe(IDemoContainerAdapter containerAdapter)
        : base(containerAdapter)
    {
    }
}
