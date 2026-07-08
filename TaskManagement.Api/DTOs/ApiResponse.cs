namespace TaskManagement.Api.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T> { Success = true, Data = data, Message = message, StatusCode = statusCode };
    }

    public static ApiResponse<T> Error(string message, int statusCode = 400)
    {
        return new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }

    public static ApiResponse Ok(string message = "Success", int statusCode = 200)
    {
        return new ApiResponse { Success = true, Message = message, StatusCode = statusCode };
    }

    public static ApiResponse Error(string message, int statusCode = 400)
    {
        return new ApiResponse { Success = false, Message = message, StatusCode = statusCode };
    }

    public static ApiResponse<T> Ok<T>(T data, string message = "Success", int statusCode = 200)
    {
        return ApiResponse<T>.Ok(data, message, statusCode);
    }
}
