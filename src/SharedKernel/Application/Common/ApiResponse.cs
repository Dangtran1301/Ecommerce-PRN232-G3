using SharedKernel.Domain.Common.Results;

namespace SharedKernel.Application.Common;

public class ApiResponse
{
    public bool Success { get; }
    public Error? Error { get; }
    public DateTime Timestamp { get; }

    private ApiResponse(bool success, Error? error)
    {
        Success = success;
        Error = error;
        Timestamp = DateTime.UtcNow;
    }

    public static ApiResponse FromResult(Result result)
        => new(result.IsSuccess, result.Error);

    public static ApiResponse<T> FromResult<T>(Result<T> result)
        => ApiResponse<T>.FromResult(result);
}

public class ApiResponse<T>
{
    public bool Success { get; }
    public Error? Error { get; }
    public T? Data { get; }
    public DateTime Timestamp { get; }

    private ApiResponse(bool success, Error? error, T? data)
    {
        Success = success;
        Error = error;
        Data = data;
        Timestamp = DateTime.UtcNow;
    }

    public static ApiResponse<T> FromResult(Result<T> result)
        => new(result.IsSuccess, result.Error, result.Value);
}