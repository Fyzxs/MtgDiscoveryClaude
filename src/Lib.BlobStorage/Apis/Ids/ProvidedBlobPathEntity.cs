namespace Lib.BlobStorage.Apis.Ids;

/// <summary>
/// Represents a provided blob path entity.
/// </summary>
public sealed class ProvidedBlobPathEntity : BlobPathEntity
{
    /// <summary>
    /// The origin of the blob path.
    /// </summary>
    private readonly string _origin;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidedBlobPathEntity"/> class.
    /// </summary>
    /// <param name="origin">The origin of the blob path.</param>
    public ProvidedBlobPathEntity(string origin) => _origin = origin;

    /// <summary>
    /// Converts the blob path entity to its system type representation.
    /// </summary>
    /// <returns>The origin of the blob path as a string.</returns>
    public override string AsSystemType() => _origin;
}
