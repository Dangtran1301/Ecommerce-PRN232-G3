using SharedKernel.Domain.Common.Results;
using System.Text.Json.Serialization;

namespace SharedKernel.Application.Common;

public class ApiResponse
{
    [JsonPropertyOrder(1)]
    public bool Success { get; init; }

    [JsonPropertyOrder(2)]
    public Error? Error { get; init; }

    [JsonPropertyOrder(4)]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public static implicit operator ApiResponse(Result result)
        => new()
        {
            Success = result.IsSuccess,
            Error = result.IsSuccess ? null : result.Error
        };
}

public class ApiResponse<T> : ApiResponse
{
    [JsonPropertyOrder(3)]
    public T? Data { get; init; }

    public static implicit operator ApiResponse<T>(Result<T> result)
        => new()
        {
            Success = result.IsSuccess,
            Error = result.IsSuccess ? null : result.Error,
            Data = result.Value
        };

    public static implicit operator ApiResponse<T>(T value)
        => new()
        {
            Success = true,
            Data = value
        };
}