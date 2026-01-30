using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Newtonsoft.Json;

namespace Lib.Shared.Invocation.Response.Models;

public abstract class ResponseModel
{
    [JsonProperty("status")]
    public virtual StatusDataModel Status { get; set; }

    [JsonProperty("meta")]
    public virtual MetaDataModel MetaData { get; set; } = new();
}

public sealed class FailureResponseModel : ResponseModel;

public sealed class SuccessResponseModel : ResponseModel;

public sealed class SuccessDataResponseModel<T> : ResponseModel
{
    [JsonProperty("data")]
    public T Data { get; set; }
}

public sealed class StatusDataModel
{
    [JsonProperty("code")]
    public HttpStatusCode StatusCode { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}

public sealed class MetaDataModel
{
    [JsonProperty("timestamp")]
    public string TimeStamp => DateTime.UtcNow.ToString("O");

    [JsonProperty("invocationId")]
    public string InvocationId { get; set; } = "not_provided";

    [JsonProperty("elapsedTime")]
    public string ElapsedTime { get; set; } = "not_provided";
}

public sealed class UnhandledExceptionInternalServerErrorResponseModel : ResponseModel
{
    private readonly IExecutionContext _exCtx;
    private readonly Exception _ex;

    public UnhandledExceptionInternalServerErrorResponseModel([NotNull] IExecutionContext exCtx, [NotNull] Exception ex)
    {
        _exCtx = exCtx;
        _ex = ex;
    }

    public override StatusDataModel Status => new() { Message = _ex.Message, StatusCode = HttpStatusCode.InternalServerError };
    public override MetaDataModel MetaData => new() { ElapsedTime = _exCtx.ElapsedTime().ToString(), InvocationId = _exCtx.InvocationId() };
}

public sealed class UnhandledExceptionBadRequestResponseModel : ResponseModel
{
    private readonly IExecutionContext _exCtx;
    private readonly Exception _ex;

    public UnhandledExceptionBadRequestResponseModel([NotNull] IExecutionContext exCtx, [NotNull] Exception ex)
    {
        _exCtx = exCtx;
        _ex = ex;
    }

    public override StatusDataModel Status => new() { Message = _ex.Message, StatusCode = HttpStatusCode.BadRequest };
    public override MetaDataModel MetaData => new() { ElapsedTime = _exCtx.ElapsedTime().ToString(), InvocationId = _exCtx.InvocationId() };
}
