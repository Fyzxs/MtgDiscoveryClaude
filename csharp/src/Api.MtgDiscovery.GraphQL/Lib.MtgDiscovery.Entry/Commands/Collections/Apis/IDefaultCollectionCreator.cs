using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Apis;

public interface IDefaultCollectionCreator
{
    Task CreateDefaultCollectionAsync(IUserIdArgEntity args, CancellationToken cancellationToken);
}
