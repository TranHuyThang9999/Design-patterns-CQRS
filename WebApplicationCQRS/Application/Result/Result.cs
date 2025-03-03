using System.Net;
using System.Text.Json.Serialization;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain;

public class Result<T>
{
    public ResponseCode Code { get; }
    public string Message { get; }
    public HttpStatusCode StatusCode { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; }

    public Result(ResponseCode code, HttpStatusCode statusCode, string message, T? data = default)
    {
        Code = code;
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    public static Result<T> Success(T data, string message = "Success", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Result<T>(ResponseCode.Success, statusCode, message, data);
    }

    public static Result<T> Failure(ResponseCode code, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(code, statusCode, message);
    }
}