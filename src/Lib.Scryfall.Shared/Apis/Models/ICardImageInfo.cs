using System.Collections.Generic;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Shared.Apis.Models;

/// <summary>
/// 
/// </summary>
public interface ICardImageInfo
{
    Url ImageUrl();
    string StoragePath();
    IDictionary<string, string> Metadata();
    string LogValue();
}
