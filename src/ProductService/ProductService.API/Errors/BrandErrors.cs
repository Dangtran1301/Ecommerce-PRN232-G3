using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Errors
{
    public static class BrandErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Brand not found with ID: {id}");

        public static Error NameTaken(string name) =>
            Error.Conflict($"Brand name already exists: {name}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid brand data");
    }
}
