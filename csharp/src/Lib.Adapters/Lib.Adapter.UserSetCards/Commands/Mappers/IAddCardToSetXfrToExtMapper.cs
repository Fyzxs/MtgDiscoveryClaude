using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal interface IAddCardToSetXfrToExtMapper : ICreateMapper<IAddCardToSetXfrEntity, ReadPointItem>;
