using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Category not found with ID: {id}");

        public static Error NameTaken(string name) =>
            Error.Conflict($"Category name already exists: {name}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid category data");
    }
}
