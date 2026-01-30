namespace Lib.Shared.DataModels.Entities.Args.Collections;

public interface ICreateCollectionArgEntity
{
    string Name { get; }
    string Type { get; }
    string Visibility { get; }
}
