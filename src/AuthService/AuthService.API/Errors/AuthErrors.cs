using SharedKernel.Domain.Common.Results;

namespace AuthService.API.Errors;

public class AuthErrors
{
    public static Error InvalidCredentials(string? details = null) =>
        Error.Unauthorized(details ?? "Invalid username/email or password");

    public static Error UserNotFound =>
        Error.Unauthorized("User not found");

    public static Error NullUser =>
        Error.Unauthorized("User is null");

    public static Error AlreadyExisted =>
        Error.Failure("User or email already existed");

    public static Error InvalidRefreshToken =>
        Error.Validation("Invalid refresh token");

    public static Error RefreshTokenRevoked =>
        Error.Validation("Refresh token has been revoked");

    public static Error RefreshTokenExpired =>
        Error.Validation("Refresh token has expired");

    public static Error TokenAlreadyRevoked =>
        Error.Validation("Token already revoked");

    public static Error InternalFailure(string message, object? details = null) =>
        Error.Internal(message, details);
}