using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;

internal sealed class HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator : OperationResponseValidator<IUpdateCollectionVisibilityArgEntity, IUpdateCollectionVisibilityItrEntity>
{
    public HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUpdateCollectionVisibilityArgEntity>
    {
        public Task<bool> IsValid(IUpdateCollectionVisibilityArgEntity arg) => Task.FromResult(arg.CollectionId.IzNotNullOrWhiteSpace() && Guid.TryParse(arg.CollectionId, out _));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection ID must be a valid GUID";
    }
}
