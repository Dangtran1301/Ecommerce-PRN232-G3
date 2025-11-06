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

    public static Error UsernameAlreadyExisted =>
        Error.Failure("User already existed");

    public static Error EmailAlreadyExisted =>
        Error.Failure("Email already in use");

    public static Error InvalidRefreshToken =>
        Error.Unauthorized("Invalid refresh token");

    public static Error InvalidResetToken =>
        Error.Unauthorized("Invalid reset token");

    public static Error RefreshTokenRevoked =>
        Error.Unauthorized("Refresh token has been revoked");

    public static Error RefreshTokenExpired =>
        Error.Unauthorized("Refresh token has expired");

    public static Error TokenAlreadyRevoked =>
        Error.Unauthorized("Token already revoked");

    public static Error BadRequest(string mess, object? detail = null) =>
        Error.Failure(mess, detail);

    public static Error InternalFailure(string message, object? details = null) =>
        Error.Internal(message, details);
}