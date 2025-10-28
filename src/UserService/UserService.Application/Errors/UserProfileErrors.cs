using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace UserService.Application.Errors;

public static class UserProfileErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound($"User Profile not found with ID: {id}");

    public static Error EmailTaken(string email) =>
        Error.Conflict($"Email already exists: {email}");

    public static Error UserIdTaken(Guid id) =>
        Error.Conflict($"User ID already exist: {id}");

    public static Error InvalidData(string? message = null) =>
        Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid user data");
}