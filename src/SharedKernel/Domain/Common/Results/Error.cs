namespace SharedKernel.Domain.Common.Results;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public object? Details { get; }

    private Error(string code, string message, object? details = null)
    {
        Code = code;
        Message = message;
        Details = details;
    }

    // Factory methods
    public static Error Validation(string message, object? details = null)
        => new("ValidationError", message, details);

    public static Error NotFound(string message)
        => new("NotFound", message);

    public static Error Conflict(string message)
        => new("Conflict", message);

    public static Error Failure(string message, object? details = null)
        => new("Failure", message, details);

    public static readonly Error None = new("None", string.Empty);

    public override string ToString()
        => Details is null
            ? $"{Code}: {Message}"
            : $"{Code}: {Message} | Details: {System.Text.Json.JsonSerializer.Serialize(Details)}";
}