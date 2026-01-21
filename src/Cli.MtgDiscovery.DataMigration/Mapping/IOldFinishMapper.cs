using Lib.Shared.Abstractions.Mappers;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

internal interface IOldFinishMapper : ICreateMapper<(bool foil, bool nonfoil, bool etched), string>
{
}
