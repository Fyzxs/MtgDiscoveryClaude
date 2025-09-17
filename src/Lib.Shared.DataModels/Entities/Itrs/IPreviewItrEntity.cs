namespace Lib.Shared.DataModels.Entities;

public interface IPreviewItrEntity
{
    string Source { get; }
    string SourceUri { get; }
    string PreviewedAt { get; }
}
