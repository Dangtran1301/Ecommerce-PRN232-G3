using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Errors
{
    public static class ProductAttributeErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Product attribute not found with ID: {id}");

        public static Error DuplicateAttribute(string attributeName) =>
            Error.Conflict($"Duplicate attribute name: {attributeName}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid product attribute data");

        public static Error NameTaken(string attributeName) =>
            Error.Conflict($"Attribute name '{attributeName}' is already taken");
    }
}