using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;

internal sealed class HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator : OperationResponseValidator<IUpdateCollectionVisibilityArgEntity, IUpdateCollectionVisibilityItrEntity>
{
    private static readonly string[] s_validVisibilityValues = ["private", "public"];

    public HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUpdateCollectionVisibilityArgEntity>
    {
        public Task<bool> IsValid(IUpdateCollectionVisibilityArgEntity arg)
        {
            if (arg.Visibility.IzNullOrWhiteSpace())
            {
                return Task.FromResult(false);
            }

            string normalizedVisibility = arg.Visibility.ToLowerInvariant();
            return Task.FromResult(Array.Exists(s_validVisibilityValues, v => v == normalizedVisibility));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Visibility must be either 'private' or 'public'";
    }
}
