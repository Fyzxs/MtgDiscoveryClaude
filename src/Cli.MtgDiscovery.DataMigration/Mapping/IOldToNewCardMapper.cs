using System.Collections.Generic;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

internal interface IOldToNewCardMapper : ICreateMapper<(CollectorDataRecord sqlRecord, OldDiscoveryCardExtEntity oldCosmosCard, ICardItemItrEntity newSystemCard, string targetUserId), IEnumerable<IAddCardToCollectionArgsEntity>>
{
}
