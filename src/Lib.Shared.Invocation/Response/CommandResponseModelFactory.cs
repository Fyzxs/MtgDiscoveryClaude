//using System.Diagnostics.CodeAnalysis;
//using Lib.Shared.Invocation.Commands;
//using Lib.Shared.Invocation.Response.Models;

//namespace Lib.Shared.Invocation.Response;

//public sealed class CommandResponseModelFactory : ICommandResponseModelFactory
//{
//    private readonly IExecutionContext _exCtx;

//    public CommandResponseModelFactory(IExecutionContext exCtx) => _exCtx = exCtx;

//    public ResponseModel Success([NotNull] CommandOperationStatus commandOperationStatus)
//    {
//        return new SuccessResponseModel
//        {
//            Status = new StatusDataModel
//            {
//                StatusCode = commandOperationStatus.Status,
//                Message = commandOperationStatus.Message
//            },
//            MetaData = MetaData()
//        };
//    }

//    public ResponseModel Failure([NotNull] CommandOperationStatus commandOperationStatus)
//    {
//        return new FailureResponseModel
//        {
//            Status = new StatusDataModel
//            {
//                StatusCode = commandOperationStatus.Status,
//                Message = commandOperationStatus.Message
//            },
//            MetaData = MetaData()
//        };
//    }

//    private MetaDataModel MetaData() => new() { ElapsedTime = _exCtx.ElapsedTime().ToString(), InvocationId = _exCtx.InvocationId() };
//}
