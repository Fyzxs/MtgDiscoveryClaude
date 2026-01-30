using System.Threading.Tasks;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Apis;

internal interface ICollectionNameUniquenessChecker
{
    Task<bool> IsNameUniqueForOwnerAsync(string name, string ownerId, string excludeCollectionId = null);
}
