using Lib.Universal.Primitives;

namespace Lib.BlobStorage.Apis.Operations.Responses;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BlobOpResponse<T> : ToSystemType<T>
{
    /// <summary>
    /// 
    /// </summary>
    public abstract T Value { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract bool HasValue();

    /// <summary>
    /// Checks if the operation was not successful based on the status code.
    /// </summary>
    /// <returns>True if the status code indicates failure; otherwise, false.</returns>
    public bool MissingValue() => HasValue() is false;

    /// <summary>
    /// Returns the value from the operation response.
    /// </summary>
    /// <returns>The operation response value.</returns>
    public override T AsSystemType() => Value;
}
