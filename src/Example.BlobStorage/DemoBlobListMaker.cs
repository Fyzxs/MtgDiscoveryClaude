using Lib.BlobStorage.Apis.Operations;

namespace Example.BlobStorage;

public interface IDemoBlobListMaker : IBlobListMaker;

internal sealed class DemoBlobListMaker : BlobListMaker, IDemoBlobListMaker
{
    public DemoBlobListMaker(IDemoContainerAdapter containerAdapter)
        : base(containerAdapter)
    {
    }
}
