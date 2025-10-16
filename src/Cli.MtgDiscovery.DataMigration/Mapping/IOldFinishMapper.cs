using Lib.Shared.Abstractions.Mappers;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

public interface IOldFinishMapper : ICreateMapper<(bool foil, bool nonfoil, bool etched), string>
{
}
