using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities;

public interface ISetCodesItrEntity
{
    IReadOnlyCollection<string> SetCodes { get; }
}