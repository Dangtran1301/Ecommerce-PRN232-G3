namespace SharedKernel.Domain.Common.Results;

public class Error(string code, string message)
{
    public string Code { get; } = code;

    public string Message { get; } = message;

    public static readonly Error None = new("NONE", string.Empty);
}