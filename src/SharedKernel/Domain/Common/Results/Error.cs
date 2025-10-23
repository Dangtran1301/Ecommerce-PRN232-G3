using SharedKernel.Application.Common.Enums;

namespace SharedKernel.Domain.Common.Results;

public class Error
{
    public string Code { get; init; } = string.Empty;
    public string? Message { get; init; } = string.Empty;
    public object? Details { get; init; }

    public static readonly Error None = new() { Code = "None", Message = "" };

    public static Error Validation(string message, object? details = null)
        => new() { Code = ErrorCodes.Validation, Message = message, Details = details };

    public static Error NotFound(string message)
        => new() { Code = ErrorCodes.NotFound, Message = message };

    public static Error Conflict(string message)
        => new() { Code = ErrorCodes.Conflict, Message = message };

    public static Error Failure(string message, object? details = null)
        => new() { Code = ErrorCodes.BadRequest, Message = message, Details = details };

    public static Error Internal(string message, object? details = null)
        => new() { Code = ErrorCodes.InternalServerError, Message = message, Details = details };

    public static Error Unauthorized(string message, object? detail = null)
        => new() { Code = ErrorCodes.Unauthorized, Message = message, Details = detail};

    public static Error Forbidden(string message)
        => new() { Code = ErrorCodes.Forbidden, Message = message };

    public static Error TooManyRequests(string message)
        => new() { Code = ErrorCodes.TooManyRequests, Message = message };

    public override string ToString()
        => Details is null
            ? $"{Code}: {Message}"
            : $"{Code}: {Message} | Details: {Details}";
}