namespace IbnelveApi.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public ApiResponse()
    {
    }

    public ApiResponse(T data, string message = "Operação realizada com sucesso")
    {
        Success = true;
        Message = message;
        Data = data;
    }

    public ApiResponse(string message, List<string>? errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors ?? new List<string>();
    }

    public static ApiResponse<T> SuccessResult(T data, string message = "Operação realizada com sucesso")
    {
        return new ApiResponse<T>(data, message);
    }

    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(message, errors);
    }

    public static ApiResponse<T> ErrorResult(string message, string error)
    {
        return new ApiResponse<T>(message, new List<string> { error });
    }
}

