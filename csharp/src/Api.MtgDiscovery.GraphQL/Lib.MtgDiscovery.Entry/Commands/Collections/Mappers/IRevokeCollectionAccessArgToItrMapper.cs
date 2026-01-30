using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal interface IRevokeCollectionAccessArgToItrMapper : ICreateMapper<IRevokeCollectionAccessArgEntity, string, IRevokeCollectionAccessItrEntity>
{
}
