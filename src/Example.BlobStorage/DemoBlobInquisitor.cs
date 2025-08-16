using Lib.BlobStorage.Apis.Operations;

namespace Example.BlobStorage;

public interface IDemoBlobInquisitor : IBlobInquisitor;

internal sealed class DemoBlobInquisitor : BlobInquisitor, IDemoBlobInquisitor
{
    public DemoBlobInquisitor(IDemoContainerAdapter containerAdapter)
        : base(containerAdapter)
    {
    }
}
