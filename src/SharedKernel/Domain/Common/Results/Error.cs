namespace SharedKernel.Domain.Common.Results;

public class Error
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Details { get; init; }

    public static readonly Error None = new() { Code = "None", Message = "" };

    public static Error Validation(string message, string? details = null)
        => new() { Code = "ValidationError", Message = message, Details = details };

    public static Error NotFound(string message)
        => new() { Code = "NotFound", Message = message };

    public static Error Conflict(string message)
        => new() { Code = "Conflict", Message = message };

    public static Error Failure(string message, string? details = null)
        => new() { Code = "Failure", Message = message, Details = details };

    public override string ToString()
        => Details is null
            ? $"{Code}: {Message}"
            : $"{Code}: {Message} | Details: {Details}";
}