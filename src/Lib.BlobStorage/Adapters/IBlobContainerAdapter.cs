using Lib.BlobStorage.Apis.Operations;

namespace Lib.BlobStorage.Adapters;

/// <summary>
/// 
/// </summary>
public interface IBlobContainerAdapter :
    IBlobContainerExistsOperator,
    IBlobContainerDeleteOperator,
    IBlobContainerWriteOperator,
    IBlobContainerListOperator;
