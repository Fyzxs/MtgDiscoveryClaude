namespace Lib.Shared.DataModels.Entities.Itrs;

public interface IAllPartsItrEntity
{
    string Id { get; }
    string Component { get; }
    string Name { get; }
    string TypeLine { get; }
    string Uri { get; }
}
