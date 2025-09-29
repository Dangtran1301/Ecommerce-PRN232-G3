using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace UserService.Application.Errors;

public static class UserErrors
{
    public static Error NotFound(Guid id) =>
        new(ErrorCodes.NotFound, $"User not found with ID: {id}");

    public static Error EmailTaken(string email) =>
        new(ErrorCodes.Conflict, $"Email already exists: {email}");

    public static Error InvalidData(string? message = null) =>
        new(ErrorCodes.BadRequest, message ?? "Invalid user data");
}