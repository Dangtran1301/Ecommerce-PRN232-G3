using SharedKernel.Domain.Common.Results;
using System.Text.Json.Serialization;

namespace SharedKernel.Application.Common;

public class ApiResponse(bool success, Error? error)
{
    [JsonPropertyOrder(1)]
    public bool Success { get; } = success;

    [JsonPropertyOrder(2)]
    public Error? Error { get; } = error;

    [JsonPropertyOrder(4)]
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public static implicit operator ApiResponse(Result result)
        => new(result.IsSuccess, result.Error);

    public static ApiResponse<T> FromResult<T>(Result<T> result)
        => result;
}

public class ApiResponse<T>(bool success, Error? error, T? data) : ApiResponse(success, error)
{
    [JsonPropertyOrder(3)]
    public T? Data { get; } = data;

    public static implicit operator ApiResponse<T>(Result<T> result)
        => new(result.IsSuccess, result.Error, result.Value);

    public static implicit operator ApiResponse<T>(T value)
        => new(true, null, value);
}