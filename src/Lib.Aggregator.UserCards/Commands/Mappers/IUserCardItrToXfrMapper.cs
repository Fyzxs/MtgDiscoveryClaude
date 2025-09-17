using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal interface IUserCardItrToXfrMapper : ICreateMapper<IUserCardItrEntity, IUserCardXfrEntity>;