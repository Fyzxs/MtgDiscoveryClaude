using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

namespace App.MtgDiscovery.GraphQL.Entities.Types.Signing;

public sealed class SigningResultOutEntityType : ObjectType<SigningResultOutEntity>
{
    protected override void Configure([NotNull] IObjectTypeDescriptor<SigningResultOutEntity> descriptor)
    {
        descriptor.Name("SigningResult")
            .Description("Result of querying user cards for convention signing");

        descriptor.Field(f => f.Sets)
            .Name("sets")
            .Type<NonNullType<ListType<NonNullType<SigningSetGroupOutEntityType>>>>()
            .Description("Cards grouped by set");
    }
}
