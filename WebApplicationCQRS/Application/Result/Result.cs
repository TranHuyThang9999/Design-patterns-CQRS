using System.Text.Json.Serialization;
using WebApplicationCQRS.Domain;

public class Result<T>
{
    public ResponseCode Code { get; }
    public string Message { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; }

    private Result(ResponseCode code, string message, T? data = default)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public static Result<T> Success(T data, string message = "Success")
    {
        return new Result<T>(ResponseCode.Success, message, data);
    }

    public static Result<T> Failure(ResponseCode code, string message)
    {
        return new Result<T>(code, message);
    }
}