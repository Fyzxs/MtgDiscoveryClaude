namespace Lib.Shared.DataModels.Entities.Itrs.Cards;

public interface IPreviewItrEntity
{
    string Source { get; }
    string SourceUri { get; }
    string PreviewedAt { get; }
}
